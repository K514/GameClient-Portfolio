using System.Threading;
using Cysharp.Threading.Tasks;

namespace k514.Mono.Common
{
    public class DummyBoundedMind : BoundedMind
    {
        #region <Consts>

        public static (bool, MindModuleDataTableQuery.TableLabel, DummyBoundedMind) CreateModule(IMindModuleDataTableRecordBridge p_ModuleRecord, IGameEntityBridge p_Entity)
        {
            return BoundedMind.CreateModule(new DummyBoundedMind(p_ModuleRecord, p_Entity));
        }
        
        public static async UniTask<(bool, MindModuleDataTableQuery.TableLabel, DummyBoundedMind)> CreateModule(IMindModuleDataTableRecordBridge p_ModuleRecord, IGameEntityBridge p_Entity, CancellationToken p_CancellationToken)
        {
            return await BoundedMind.CreateModule(new DummyBoundedMind(p_ModuleRecord, p_Entity), p_CancellationToken);
        }

        #endregion

        #region <Constructor>

        private DummyBoundedMind(IMindModuleDataTableRecordBridge p_ModuleRecord, IGameEntityBridge p_Entity) : base(BoundedModuleDataTableQuery.TableLabel.Dummy, p_ModuleRecord, p_Entity)
        {
        }
        
        #endregion
    }
}