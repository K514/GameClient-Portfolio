using System.Threading;
using Cysharp.Threading.Tasks;

namespace k514.Mono.Common
{
    public abstract partial class GameEntityModuleClusterBase<This, ModuleLabelType, TableBridge, RecordBridge, ModuleBase>
    {
        protected abstract (bool, ModuleLabelType, ModuleBase) SpawnModule(int p_Index);
        protected abstract UniTask<(bool, ModuleLabelType, ModuleBase)> SpawnModule(int p_Index, CancellationToken p_CancellationToken);
    }
}