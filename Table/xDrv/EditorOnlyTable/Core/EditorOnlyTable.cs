#if UNITY_EDITOR

using System.Threading;
using Cysharp.Threading.Tasks;

namespace k514
{
    /// <summary>
    /// 실제 런타임에는 포함되지 않는 테이블
    /// </summary>
    public abstract class EditorOnlyTable<Table, Meta, Key, Record> : TableBase<Table, Meta, Key, Record> 
        where Table : EditorOnlyTable<Table, Meta, Key, Record>, new()
        where Meta : TableMetaData, new()
        where Record : EditorOnlyTable<Table, Meta, Key, Record>.EditorOnlyTableRecord, new() 
    {
        public abstract class EditorOnlyTableRecord : TableRecordBase
        {
        }

        protected override async UniTask OnCreated(CancellationToken p_CancellationToken)
        {
            await base.OnCreated(p_CancellationToken);
            
            TableType = TableTool.TableType.EditorOnlyTable;
            TableSerializeType = TableTool.TableSerializeType.NoneSerialize;
        }

        protected override async UniTask OnInitiate(CancellationToken p_CancellationToken)
        {
            // 테이블이 없다면 기본값으로 생성한다.
            await WriteDefaultTable(false, p_CancellationToken);

            await base.OnInitiate(p_CancellationToken);
        }
    }
}

#endif