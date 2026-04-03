using System.Threading;
using Cysharp.Threading.Tasks;

namespace k514.Mono.Feature
{
    public partial class DefaultGameConfigureManager : GameConfigureDataStorageBase<DefaultGameConfigureManager, DefaultGameConfigureDataTable, DefaultGameConfigureDataTable.TableRecord>
    {
        protected override void TryInitializeDependency()
        {
            base.TryInitializeDependency();

            _Dependencies.Add(typeof(DefaultGameConfigureDataTable));
        }

        protected override async UniTask OnCreated(CancellationToken p_CancellationToken)
        {
            _DataTable = DefaultGameConfigureDataTable.GetInstanceUnsafe;
        
            await base.OnCreated(p_CancellationToken);
        }
    }
}