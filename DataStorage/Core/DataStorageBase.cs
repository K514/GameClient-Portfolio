using System.Threading;
using Cysharp.Threading.Tasks;

namespace k514
{
    public interface IDataStorage<Record>
        where Record : class, IDataStorageTableRecord<int, Record>, new()
    {
        UniTask<bool> LoadData(CancellationToken p_Token);
    }
    
    public abstract partial class DataStorageBase<This, Table, Record> : AsyncSingleton<This>, IDataStorage<Record>
        where This : DataStorageBase<This, Table, Record>, new()
        where Table : class, IDataStorageTable<Record>
        where Record : class, IDataStorageTableRecord<int, Record>, new()
    {
        #region <Fields>

        protected string _DataKey;
        protected Table _DataTable;

        #endregion

        #region <Properties>

        public abstract Record DataRecord { get; }

        #endregion
        
        #region <Callbacks>

        protected override async UniTask OnCreated(CancellationToken p_CancellationToken)
        {
            await LoadData(p_CancellationToken);
        }

        protected override async UniTask OnInitiate(CancellationToken p_CancellationToken)
        {
            await UniTask.CompletedTask;
        }

        protected override void OnDisposeSingleton()
        {
            _DataTable?.Dispose();
            _DataTable = null;
            
            base.OnDisposeSingleton();
        }

        #endregion
    }
}