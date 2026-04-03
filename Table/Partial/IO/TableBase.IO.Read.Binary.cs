using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using xk514;

namespace k514
{
    public partial class TableBase<Table, Meta, Key, Record>
    {
        #region <Methods>

        /// <summary>
        /// 바이트 에셋을 읽고 리턴하는 메서드
        /// </summary>
        private async UniTask<(bool, TextAsset)> ReadBinaryTableImage(CancellationToken p_CancellationToken)
        {
            var tableFileType = GetTableFileType();
            switch (tableFileType)
            {
                case TableTool.TableFileType.Xml:
                {
                    var result = TableType == TableTool.TableType.SystemTable ?
                        await SystemTool.LoadAsync<TextAsset>(GetByteTableRelativePath(), p_CancellationToken)
                        : (await AssetLoaderManager.GetInstanceUnsafe.LoadAssetAsync<TextAsset>((ResourceLifeCycleType.SceneUnload, GetTableFileName(TableTool.TableNameQueryType.WithMainByteExt)), p_CancellationToken)).Asset;
                    
                    return (!ReferenceEquals(null, result), result);
                }
                default:
                {
                    return default;
                }
            }
        }
        
        /// <summary>
        /// 바이트 에셋으로부터 테이블 바이너리 이미지를 읽어 해당 싱글톤에 초기화시키는 메서드
        /// </summary>
        private async UniTask<bool> LoadBinaryTableImage(CancellationToken p_CancellationToken)
        {
            var (valid, byteAsset) = await ReadBinaryTableImage(p_CancellationToken);
            if (valid)
            {
                // 바이트 에셋이 유효한 경우
                var binaryImage = byteAsset.bytes.DeserializeObject<TableTool.TableDataImage<Meta, Key, Record>>();
                await ReplaceTable(binaryImage, p_CancellationToken);
                return true;
            }
            else
            {
#if APPLY_PRINT_LOG
                CustomDebug.LogError(($"{GetTableFileName(TableTool.TableNameQueryType.WithMainByteExt)}({TableType}) 바이트 코드 로드에 실패해서 xml파일을 읽습니다.", Color.red));
#endif
                // 바이트 에셋이 유효하지 않은 경우, 정규 방식으로 테이블을 읽는다.
                return await LoadTextTable(p_CancellationToken);
            }
        }

        /// <summary>
        /// 바이트 에셋을 읽고 리턴하는 메서드
        /// </summary>
        private async UniTask<(bool, TextAsset)> ReadLexicalBinaryTable(CancellationToken p_CancellationToken)
        {
            var tableFileType = GetTableFileType();
            switch (tableFileType)
            {
                case TableTool.TableFileType.Xml:
                {
                    var result = TableType == TableTool.TableType.SystemTable ?
                        await SystemTool.LoadAsync<TextAsset>(GetByteTableRelativePath(), p_CancellationToken)
                        : (await AssetLoaderManager.GetInstanceUnsafe.LoadAssetAsync<TextAsset>((ResourceLifeCycleType.SceneUnload, GetTableFileName(TableTool.TableNameQueryType.WithMainByteExt)), p_CancellationToken)).Asset;
                    
                    return (!ReferenceEquals(null, result), result);
                }
                default:
                {
                    return default;
                }
            }
        }
        
        /// <summary>
        /// 바이트 에셋으로부터 Lexical 데이터를 읽어 해당 싱글톤에 초기화시키는 메서드
        /// </summary>
        private async UniTask<bool> LoadLexicalBinaryTable(CancellationToken p_CancellationToken)
        {
            var (valid, byteAsset) = await ReadLexicalBinaryTable(p_CancellationToken);
            if (valid)
            {
                // 바이트 에셋이 유효한 경우
                var lexicalData = byteAsset.bytes.DeserializeObject<TableTool.TableLexicalData<Key>>();
                var tableData = await TableLoader.GetInstanceUnsafe.ParseTableLexicalData<Meta, Key, Record>(lexicalData, p_CancellationToken);
                await ReplaceTable(tableData, p_CancellationToken);

                return true;
            }
            else
            {
                // 바이트 에셋이 유효하지 않은 경우, 정규 방식으로 테이블을 읽는다.
                return await LoadTextTable(p_CancellationToken);
            }
        }

        #endregion
    }
}
