using System.Threading;
using Cysharp.Threading.Tasks;

namespace k514
{
    public abstract class AccountDataStorageBase<This, Table, Record> : DataStorageBase<This, Table, Record>
        where This : AccountDataStorageBase<This, Table, Record>, new()
        where Table : class, IAccountDataTable<Record>
        where Record : class, IAccountDataTableRecord<Record>, new()
    {
        #region <Properties>

        public override Record DataRecord => _DataTable.GetRecord(0);

        #endregion
        
        #region <Callbacks>

        protected override async UniTask OnCreated(CancellationToken p_CancellationToken)
        {
            _DataKey = "ACCC";

            await base.OnCreated(p_CancellationToken);
        }

        #endregion
    }
}