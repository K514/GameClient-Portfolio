using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace k514
{
    /// <summary>
    /// 실제 런타임에는 포함되지 않는 테이블
    /// </summary>
    public abstract class SystemTable<Table, Meta, Key, Record> : TableBase<Table, Meta, Key, Record> 
        where Table : SystemTable<Table, Meta, Key, Record>, new()
        where Meta : TableMetaData, new()
        where Record : SystemTable<Table, Meta, Key, Record>.SystemTableRecordBase, new() 
    {
        [Serializable]
        public abstract class SystemTableRecordBase : TableRecordBase
        {
        }
        
        protected override async UniTask OnCreated(CancellationToken p_CancellationToken)
        {
            await base.OnCreated(p_CancellationToken);

            TableType = TableTool.TableType.SystemTable;
            TableSerializeType = TableTool.TableSerializeType.SerializeBinaryTableImage;
        }

        protected override async UniTask OnInitiate(CancellationToken p_CancellationToken)
        {
            await _OnInitiate(p_CancellationToken);
            
            await base.OnInitiate(p_CancellationToken);
        }

        protected virtual async UniTask _OnInitiate(CancellationToken p_CancellationToken)
        {
#if UNITY_EDITOR
            // 테이블이 없다면 기본값으로 생성한다.
            await WriteDefaultTable(false, p_CancellationToken);
#endif
        }
    }
}