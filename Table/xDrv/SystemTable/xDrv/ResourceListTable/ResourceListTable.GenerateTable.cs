#if UNITY_EDITOR

using System.Linq;
using UnityEditor;
using System.IO;
using System.Threading;
using Cysharp.Threading.Tasks;
using xk514;

namespace k514
{
    public partial class ResourceListTable
    {
        #region <Consts>

        private static readonly string __MiscSymbol = ResourceType.Misc.ToString();
        private static readonly string __TableSymbol = ResourceType.Table.ToString();

        #endregion

        #region <Methods>

        protected override async UniTask InitTableWriteData(CancellationToken p_CancellationToken)
        {
            await base.InitTableWriteData(p_CancellationToken);

            // 유니티 리소스 폴더로부터 에셋을 읽고, xml 파일로 만든다.
            var directoryInfo = new DirectoryInfo(SystemMaintenance.AssetAbsolutePath);
            var result = await ListingAsset(directoryInfo, true, p_CancellationToken);

            if (!result)
            {
                ClearTable(true);
                ClearMetaData(true);
            }
        }
     
        /// <summary>
        /// 특정 경로에 존재하는 파일을 전부 검증하여 리소스 리스트 레코드로 저장하는 메서드
        /// </summary>
        private async UniTask<bool> ListingAsset(DirectoryInfo p_DirectoryInfo, bool p_IsRoot, CancellationToken p_CancellationToken)
        {
            if (p_IsRoot)
            {
                // 현재 메타데이터를 초기화시킨다.
                AssetDatabase.Refresh();
            }
            
            var directoryEnumerator = p_DirectoryInfo.GetDirectories().ToList();
            
            // 최초 디렉터리로부터 너비 우선 탐색을 진행하며, 각 에셋의 정보를 테이블 레코드 인스턴스로 만들어준다.
            var fileInfoSet = p_DirectoryInfo.GetFiles();
            foreach (var fileInfo in fileInfoSet)
            {
                var fileName = fileInfo.Name;
                var fileFullName = fileInfo.FullName.TurnToSlash();
                if (fileFullName.TryGetAssetDatabasePath(out var o_AssetDatabasePath))
                {
                    var assetBundleInfo = AssetImporter.GetAtPath(o_AssetDatabasePath);
                    if (SystemMaintenance.HasResourceListBlockedExtension(fileName) || ReferenceEquals(null, assetBundleInfo))
                    {
                    }
                    else
                    {
                        // 파일 명으로부터 리소스 타입을 계산한다.
                        var resourceType = fileFullName.GetUnityResourceType();
                        
                        // 해당 파일의 리소스 타입이 항상 유니티 리소스 타입인 경우
                        if (resourceType.IsUnityLoadOnlyResourceType())
                        {
                            // 번들로 사용될 일이 없으므로, 번들 정보를 초기화 시킨다.
                            assetBundleInfo.SetAssetBundleNameAndVariant(string.Empty, null);
                        }
                        else
                        {
                            switch (resourceType)
                            {
                                case ResourceType.None:
                                {
                                    // 번들로 사용될 일이 없으므로, 번들 정보를 초기화 시킨다.
                                    assetBundleInfo.SetAssetBundleNameAndVariant(string.Empty, null);
                                    goto SEG_LOOP_END;
                                }
                                // Misc 리소스 타입은 내부에 있는 모든 에셋을 하나의 번들로 묶는다.
                                case ResourceType.Misc:
                                {
                                    assetBundleInfo.SetAssetBundleNameAndVariant(__MiscSymbol, null);
                                    break;
                                }
                                // Table 리소스 타입은 내부에 있는 모든 에셋을 하나의 번들로 묶는다.
                                case ResourceType.Table:
                                {
                                    assetBundleInfo.SetAssetBundleNameAndVariant(__TableSymbol, null);
                                    break;
                                }
                                // Terminal 심볼이 경로에 포함된 경우 해당 심볼이 붙은 곳을 번들 이름으로 한다.
                                case var _ when o_AssetDatabasePath.IsTerminalAssetName():
                                {
                                    // ResourceTool.TerminalSymbol 부분 제거
                                    var tryBundleName = o_AssetDatabasePath.CutString(ResourceTool.TerminalSymbol, true, true) 
                                                        + o_AssetDatabasePath.CutString(ResourceTool.TerminalSymbol, false, true).CutString("/", true, true);
                                    assetBundleInfo.SetAssetBundleNameAndVariant($"{tryBundleName}", null);   
                                    break;
                                }
                                // Block 심볼이 경로에 포함된 경우 해당 심볼이 붙은 곳을 번들 이름으로 한다.
                                case var _ when o_AssetDatabasePath.IsBlockedAssetName():
                                {
                                    // ResourceTool.BlockSymbol 부분 제거
                                    var tryBundleName = o_AssetDatabasePath.CutString(ResourceTool.BlockSymbol, true, true) 
                                                        + o_AssetDatabasePath.CutString(ResourceTool.BlockSymbol, false, true).CutString("/", true, true);
                                    assetBundleInfo.SetAssetBundleNameAndVariant($"{tryBundleName}", null);   
                                    break;
                                }
                                // 나머지 리소스 타입은 해당 리소스의 상위 경로를 이름으로 하는 번들로 묶는다.
                                default:
                                {
                                    var tryBundleName = o_AssetDatabasePath.CutString("/", true, false);
                                    if (directoryEnumerator.Any())
                                    {
                                        // 만약 해당 리소스의 디렉터리에 하위 디렉터리가 포함된 경우, 디렉터리 이름 앞에 _를 붙여 번들 이름 중복을 막는다.
                                        assetBundleInfo.SetAssetBundleNameAndVariant($"{tryBundleName.CutString("/", true, false)}/_{tryBundleName.CutString("/", false, false)}", null);
                                    }
                                    else
                                    {
                                        assetBundleInfo.SetAssetBundleNameAndVariant($"{tryBundleName}", null);
                                    }
                                    break;
                                }
                            }
                        }

                        // 해당 파일 경로가 블록되었는지 검증하는 프로퍼티
                        var isBlockedFileName = fileFullName.IsBlockedAssetName();
                        
                        // 리소스 리스트에서 제외되는 이름의 에셋이더라도 에셋번들 목록에는 추가해준다.
                        if (isBlockedFileName)
                        {
                        }
                        else
                        {
                            // 이미 해당 파일 이름을 가지고 있는 경우
                            if (HasKey(fileName))
                            {
    #if APPLY_PRINT_LOG
                                CustomDebug.LogError($"파일명 [{fileName}]이 중복되었습니다. 파일명이 중복되지 않도록 재지정해주세요.\n * 현재 테이블에 존재하는 파일 패스\n[{GetRecord(fileName).ResourceFullPath}]\n * 중복된 파일 패스\n[{fileFullName}]\n\n");
    #endif
                                // return false;
                            }
                            else
                            {
                                await AddRecord(fileName, true, p_CancellationToken, assetBundleInfo.assetBundleName, fileFullName);
                            }
                        }
                        
                        SEG_LOOP_END: ;
                    }
                }
            }

            // 현재 디렉터리의 하위 디렉터리에 대해서도 재귀호출해준다.
            foreach (var subDirectory in directoryEnumerator)
            {
                var result = await ListingAsset(subDirectory, false, p_CancellationToken);
                if (!result)
                {
                    return false;
                }
            }

            return true;
        }
        
        #endregion
    }
}

#endif
