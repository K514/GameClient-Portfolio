using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace k514.Mono.Common
{
    public abstract class GameValueTableBase<Table, Key, Record> : GameTable<Table, TableMetaData, Key, Record>
        where Table : GameValueTableBase<Table, Key, Record>, new()
        where Record : GameValueTableBase<Table, Key, Record>.GameValueTableRecordBase, new()
    {
        #region <Callbacks>

        protected override async UniTask OnCreated(CancellationToken p_CancellationToken)
        {
            await base.OnCreated(p_CancellationToken);
         
            SetBranchHeader("GameValue/");
        }
        
        protected override async UniTask OnInitiate(CancellationToken p_CancellationToken)
        {
#if UNITY_EDITOR
            // 테이블이 없다면 기본값으로 생성한다.
            await WriteDefaultTable(false, p_CancellationToken);
#endif
            await base.OnInitiate(p_CancellationToken);
        }

        #endregion
        
        #region <Record>
        
        [Serializable]
        public abstract class GameValueTableRecordBase : GameTableRecord
        {
        }
        
        #endregion
    }
}