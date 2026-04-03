using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace k514
{
    /// <summary>
    /// 테이블 인스턴스 초기화 시, 테이블 파일을 읽지 않고 이후 임의의 시간에 수동으로 테이블을 읽는 테이블
    /// </summary>
    public abstract class OptionalTable<Table, Meta, Key, Record> : TableBase<Table, Meta, Key, Record> 
        where Table : OptionalTable<Table, Meta, Key, Record>, new()
        where Meta : TableMetaData, new()
        where Record : OptionalTable<Table, Meta, Key, Record>.OptionalTableRecord, new() 
    {
        [Serializable]
        public abstract class OptionalTableRecord : TableRecordBase
        {
        }
        
        protected override async UniTask OnCreated(CancellationToken p_CancellationToken)
        {
            await base.OnCreated(p_CancellationToken);
        
            TableType = TableTool.TableType.OptionalTable;
            TableSerializeType = TableTool.TableSerializeType.NoneSerialize;
        }

        protected override async UniTask OnInitiate(CancellationToken p_CancellationToken)
        {
            CheckTable();
            await InitTableDefaultData(p_CancellationToken);
        }
    }
}