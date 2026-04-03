using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using xk514;

namespace k514
{
    public partial class TableBase<Table, Meta, Key, Record>
    {
        #region <Methods>

        /// <summary>
        /// 테이블 타입에 따라 테이블 파일을 로드하고, 컬렉션에 초기화 시키는 메서드
        /// </summary>
        public async UniTask<bool> LoadTable(CancellationToken p_CancellationToken)
        {
            ClearTable(true);
            ClearMetaData(true);

            var (canceled, result) = default((bool, bool));
            switch (TableType)
            {
                case TableTool.TableType.GameTable:
                case TableTool.TableType.SystemTable:
                {
#if UNITY_EDITOR
                    if (SystemFlagTable.IsUsingSerializedTable())
                    {
                        switch (TableSerializeType)
                        {
                            case TableTool.TableSerializeType.NoneSerialize:
                                (canceled, result) = await LoadTextTable(p_CancellationToken).SuppressCancellationThrow();
                                break;
                            case TableTool.TableSerializeType.SerializeBinaryTableImage:
                                (canceled, result) = await LoadBinaryTableImage(p_CancellationToken).SuppressCancellationThrow();
                                break;
                        }
                    }
                    else
                    {
                        (canceled, result) = await LoadTextTable(p_CancellationToken).SuppressCancellationThrow();
                    }
#else
                    switch (TableSerializeType)
                    {
                        case TableTool.TableSerializeType.NoneSerialize:
                            (canceled, result) = await LoadTextTable(p_CancellationToken).SuppressCancellationThrow();
                            break;
                        case TableTool.TableSerializeType.SerializeBinaryTableImage:
                            (canceled, result) = await LoadBinaryTableImage(p_CancellationToken).SuppressCancellationThrow();
                            break;
                    }
#endif
                    break;
                }
#if APPLY_PRINT_LOG
                case TableTool.TableType.OptionalTable:
                {
                    CustomDebug.LogError("OptionalTable은 기본 디렉터리로부터 테이블을 읽을 수 없습니다.");
                    break;
                }
#endif                    
                case TableTool.TableType.EditorOnlyTable:
                {
                    // 에디터 전용 테이블은 바이트 테이블을 생성하지 않으므로
                    (canceled, result) = await LoadTextTable(p_CancellationToken).SuppressCancellationThrow();
                    break;
                }
            }

            if (canceled)
            {
                OnTableLoadCanceled();
            }
            else
            {
                if (!result)
                {
                    await OnTableLoadFailed(p_CancellationToken);
                }
            }

            return result;
        }
 
        /// <summary>
        /// 테이블을 비우고 다른 이름의 테이블을 로드하는 메서드
        ///
        /// 로드에 실패하면, 원본 테이블을 리로드한다.
        /// </summary>
        public async UniTask<bool> LoadTable(string p_TableName, CancellationToken p_CancellationToken)
        {
            SetTableName(p_TableName);
            
            var result = await LoadTable(p_CancellationToken);
            if (result)
            {
                return true;
            }
            else
            {
                SetTableName(null);
                await LoadTable(p_CancellationToken);

                return false;
            }
        }

        public async UniTask<bool> ReplaceTable(TableTool.TableDataImage<Meta, Key, Record> p_TableImage, CancellationToken p_CancellationToken)
        {
            ReplaceMetaData(p_TableImage.TableMetaData);
            return await ReplaceRecords(p_TableImage.TableRecordData, p_CancellationToken);
        }
       
        public void ReplaceMetaData(Meta p_MetaData)
        {
            _MetaData = p_MetaData;
            
            AddDefaultMetaData();
        }

        public async UniTask<bool> ReplaceRecords(Dictionary<Key, Record> p_TargetTable, CancellationToken p_CancellationToken)
        {
            ClearTable(true);
            
            if (p_TargetTable.CheckCollectionSafe())
            {
                foreach (var recordKV in p_TargetTable)
                {
                    await AddRecord(recordKV.Value, true, p_CancellationToken);
                }
            }

            return !(await OnTableLoadComplete(p_CancellationToken).SuppressCancellationThrow());
        }
        
        #endregion
    }
}