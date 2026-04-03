using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace k514
{
    [Serializable]
    public class ResourceListTableMetaData : TableMetaData
    {
    }
    
    /// <summary>
    /// 모든 리소스의 이름과 그 경로 및 에셋번들 정보를 가지는 테이블
    /// </summary>
    public partial class ResourceListTable : SystemTable<ResourceListTable, ResourceListTableMetaData, string, ResourceListTable.TableRecord>
    {
        #region <Record>

        [Serializable]
        public class TableRecord : SystemTableRecordBase
        {
            #region <Fields>

            /// <summary>
            /// 에셋 번들 이름
            /// </summary>
            public string AssetBundleName { get; private set; }

            /// <summary>
            /// 리소스 풀 패스
            /// </summary>
            public string ResourceFullPath { get; private set; }

            /// <summary>
            /// 경로로부터 결정된 리소스 타입
            /// </summary>
            [TableTool.TableRecordAttribute(TableTool.TableRecordAttributeType.Runtime)]
            public ResourceType ResourceType { get; private set; }

            /// <summary>
            /// 유니티 리소스 로드 함수용 호출 형식에 맞는 리소스 패스
            /// 
            /// 프로젝트 Assets/Resources 폴더를 기준으로 하는 Assets/Resources를 제외한 상대경로에서 확장자를 제외한 형식을 하고 있다.
            /// </summary>
            [TableTool.TableRecordAttribute(TableTool.TableRecordAttributeType.Runtime)]
            public string UnityResourceLoadPath { get; private set; }
            
            #endregion

            #region <Callbacks>

            public override async UniTask OnRecordAdded(ResourceListTable p_Table, CancellationToken p_CancellationToken)
            {
                if (!string.IsNullOrEmpty(ResourceFullPath))
                {
                    ResourceType = ResourceFullPath.GetUnityResourceType();
                    if (ResourceType != ResourceType.None)
                    {
                        var resourceLoadType = ResourceType.GetResourceLoadType();
                        switch (resourceLoadType)
                        {
                            case ResourceLoadType.FromAssetBundle:
                            {
                                break;
                            }
                            case ResourceLoadType.FromUnityResource:
                            {
                                UnityResourceLoadPath = ResourceFullPath.CutString(SystemMaintenance.AssetResourcePathHeader, false, true).CutString(".", true, false);
                                break;
                            }
                        }
                    }
                }

                await UniTask.CompletedTask;
            }

            #endregion
            
            #region <Methods>

            public override async UniTask SetRecord(string p_Key, object[] p_RecordField, CancellationToken p_CancellationToken)
            {
                await base.SetRecord(p_Key, p_RecordField, p_CancellationToken);
                
                AssetBundleName = (string) p_RecordField.GetElementSafe(0);
                ResourceFullPath = (string) p_RecordField.GetElementSafe(1);
            }

            /// <summary>
            /// Asset/Resources/ 헤더 포함, 확장자 포함 경로
            /// </summary>
            public string GetAssetResourcePathFormat()
            {
                return ResourceFullPath.CutStringWithPivot(SystemMaintenance.AssetResourcePathHeader, false, true);
            }
            
#if UNITY_EDITOR
            /// <summary>
            /// Asset/Resources/ 헤더 포함, 확장자 포함 경로
            /// </summary>
            public string GetAssetDataBaseLoadPathFormat()
            {
                return ResourceFullPath.CutStringWithPivot(SystemMaintenance.AssetResourcePathHeader, false, true);
            }
#endif

            #endregion
        }

        #endregion

        #region <Callbacks>

        protected override async UniTask _OnInitiate(CancellationToken p_CancellationToken)
        {
            // 시스템 배포 모드가 되었다는 것은 더 이상 리소스 추가가 없다는 것을 의미하므로, 리소스 리스트를 업데이트 할 필요가 없다.
            if (SystemMaintenance.IsPlayOnTargetPlatform())
            {
            }
#if UNITY_EDITOR
            // 배포 모드가 아닌 경우, 리소스 리스트를 생성 혹은 업데이트한다.
            else
            {
                await WriteDefaultTable(SystemFlagTable.GetAutoUpdateResourceListFlag(), p_CancellationToken);
            }
#endif
        }

        #endregion

        #region <Methods>
        
        protected override async UniTask AddDefaultRecords(CancellationToken p_CancellationToken)
        {
            await base.AddDefaultRecords(p_CancellationToken);

            await AddRecord(ResourceTool.FallbackResourceName, false, p_CancellationToken, string.Empty, string.Empty);
        }

        #endregion
    }
}