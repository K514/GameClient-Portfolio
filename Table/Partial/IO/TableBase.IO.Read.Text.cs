using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace k514
{
    public partial class TableBase<Table, Meta, Key, Record>
    {
        /// <summary>
        /// 해당 테이블 싱글톤이 지정하고 있는 경로로부터 텍스트 테이블을 읽고 텍스트 에셋으로 리턴하는 메서드
        /// </summary>
        public async UniTask<(bool, TextAsset)> ReadTextTableToAsset(CancellationToken p_CancellationToken)
        {
            switch (TableType)
            {
#if UNITY_EDITOR
                case TableTool.TableType.EditorOnlyTable:
                {
                    var targetFileName = GetTableFileFullPath(ResourceLoadType.FromUnityResource, AssetPathType.RelativePathWithRoot, TableTool.TableNameQueryType.WithBranchHeaderMainTableExt);
                    var textAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(targetFileName);
            
                    return (!ReferenceEquals(null, textAsset), textAsset);
                }
#endif
                case TableTool.TableType.SystemTable:
                {
                    var targetFileName = GetTableFileFullPath(ResourceLoadType.FromUnityResource, AssetPathType.RelativePath, TableTool.TableNameQueryType.WithBranchHeaderMain);
                    var textAsset = await SystemTool.LoadAsync<TextAsset>(targetFileName, p_CancellationToken);
                    return (!ReferenceEquals(null, textAsset), textAsset);
                }
                case TableTool.TableType.OptionalTable:
                {
                    return default;
                }
                default:
                case TableTool.TableType.GameTable:
                {
                    var targetFileName = GetTableFileName(TableTool.TableNameQueryType.WithMainTableExt);
                    var assetManager = AssetLoaderManager.GetInstanceUnsafe;
                    if (ReferenceEquals(null, assetManager))
                    {
                        return default;
                    }
                    else
                    {
                        var assetLoadResult = await assetManager.LoadAssetAsync<TextAsset>((ResourceLifeCycleType.SceneUnload, targetFileName), p_CancellationToken);
                        return (assetLoadResult.ValidFlag, assetLoadResult.Asset);
                    }
                }
            }
        }
                
        /// <summary>
        /// 입력받은 RawText를 테이블 이미지로 리턴하는 메서드
        /// </summary>
        public async UniTask<(bool, TableTool.TableDataImage<Meta, Key, Record>)> ReadRawTextToTableImage(string p_RawText, CancellationToken p_CancellationToken)
        {
            if (string.IsNullOrEmpty(p_RawText))
            {
                return default;
            }
            else
            {
                var lexData = await TableLoader.GetInstanceUnsafe.LexicalizeText<Key>(GetTableFileType(), p_RawText, p_CancellationToken);
                var tableImage = await TableLoader.GetInstanceUnsafe.ParseTableLexicalData<Meta, Key, Record>(lexData, p_CancellationToken);
                
                return (tableImage.ValidFlag, tableImage);
            }
        }
        
        /// <summary>
        /// 해당 테이블 싱글톤이 지정하고 있는 경로로부터 텍스트 테이블을 읽고 테이블 이미지로 리턴하는 메서드
        /// </summary>
        public async UniTask<(bool, TableTool.TableDataImage<Meta, Key, Record>)> ReadTextTableToTableImage(CancellationToken p_CancellationToken)
        {
            var (valid, textAsset) = await ReadTextTableToAsset(p_CancellationToken);
            if (valid)
            {
                return await ReadRawTextToTableImage(textAsset.text, p_CancellationToken);
            }
            else
            {
                return default;
            }
        }

        /// <summary>
        /// 임의의 경로로부터 해당 싱글톤에 대응하는 테이블 파일 이름을 가지는 텍스트 에셋을 읽고 테이블 이미지로 리턴하는 메서드
        /// </summary>
        public async UniTask<(bool, TableTool.TableDataImage<Meta, Key, Record>)> ReadPathTableToTableImage(string p_Path, CancellationToken p_CancellationToken)
        {
            var targetFileName = $"{p_Path}{GetTableFileName(TableTool.TableNameQueryType.WithMainByteExt)}";
            if (File.Exists(targetFileName))
            {
                var rawText = await File.ReadAllTextAsync(targetFileName, p_CancellationToken);
                return await ReadRawTextToTableImage(rawText, p_CancellationToken);
            }
            else
            {
                return default;
            }
        }
        
        /// <summary>
        /// 임의의 경로로부터 해당 싱글톤에 대응하는 테이블 파일 이름을 가지는 텍스트 에셋을 읽고 테이블 이미지의 메타데이터로 리턴하는 메서드
        /// </summary>
        public async UniTask<(bool, Meta)> ReadPathToMetaData(string p_Path, CancellationToken p_CancellationToken)
        {
            var (valid, tableImage) = await ReadPathTableToTableImage(p_Path, p_CancellationToken);
            if (valid)
            {
                return (true, tableImage.TableMetaData);
            }
            else
            {
                return default;
            }
        }

        /// <summary>
        /// 임의의 경로로부터 해당 싱글톤에 대응하는 테이블 파일 이름을 가지는 텍스트 에셋을 읽고 테이블 이미지의 레코드데이터로 리턴하는 메서드
        /// </summary>
        public async UniTask<(bool, Dictionary<Key, Record>)> ReadPathToRecordData(string p_Path, CancellationToken p_CancellationToken)
        {
            var (valid, tableImage) = await ReadPathTableToTableImage(p_Path, p_CancellationToken);
            if (valid)
            {
                return (true, tableImage.TableRecordData);
            }
            else
            {
                return default;
            }
        }

        /// <summary>
        /// 해당 테이블 싱글톤이 지정하고 있는 경로로부터 텍스트 에셋을 읽고 초기화 하는 메서드
        /// </summary>
        private async UniTask<bool> LoadTextTable(CancellationToken p_CancellationToken)
        {
            var (valid, textAsset) = await ReadTextTableToAsset(p_CancellationToken);
            if (valid)
            {
                var text = textAsset.text;
                return await LoadRawText(text, p_CancellationToken);
            }
            else
            {
                return false;
            }
        }
        
        /// <summary>
        /// 지정한 텍스트로부터 테이블을 디코딩하는 메서드
        /// </summary>
        public async UniTask<bool> LoadRawText(string p_RawText, CancellationToken p_CancellationToken)
        {
            var (valid, tableImage) = await ReadRawTextToTableImage(p_RawText, p_CancellationToken);
            if (valid)
            {
                return await ReplaceTable(tableImage, p_CancellationToken);
            }
            else
            {
                return default;
            }
        }
    }
}