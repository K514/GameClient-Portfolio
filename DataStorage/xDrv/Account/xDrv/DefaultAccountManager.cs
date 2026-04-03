using System.Threading;
using Cysharp.Threading.Tasks;

namespace k514.Mono.Feature
{
    public partial class DefaultAccountManager : AccountDataStorageBase<DefaultAccountManager, DefaultAccountDataTable, DefaultAccountDataTable.TableRecord>
    {
        protected override void TryInitializeDependency()
        {
            base.TryInitializeDependency();

            _Dependencies.Add(typeof(DefaultAccountDataTable));
        }

        protected override async UniTask OnCreated(CancellationToken p_CancellationToken)
        {
            _DataTable = DefaultAccountDataTable.GetInstanceUnsafe;

            await base.OnCreated(p_CancellationToken);
        }
    }
}