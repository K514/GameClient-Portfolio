using System.Threading;
using Cysharp.Threading.Tasks;

namespace k514.Mono.Common
{
    public class DefaultAction : ActionBase
    {
        #region <Consts>

        public static (bool, ActionModuleDataTableQuery.TableLabel, DefaultAction) CreateModule(IActionModuleDataTableRecordBridge p_ModuleRecord, IGameEntityBridge p_Entity)
        {
            return ActionBase.CreateModule(new DefaultAction(p_ModuleRecord, p_Entity));
        }
        
        public static async UniTask<(bool, ActionModuleDataTableQuery.TableLabel, DefaultAction)> CreateModule(IActionModuleDataTableRecordBridge p_ModuleRecord, IGameEntityBridge p_Entity, CancellationToken p_CancellationToken)
        {
            return await ActionBase.CreateModule(new DefaultAction(p_ModuleRecord, p_Entity), p_CancellationToken);
        }

        #endregion

        #region <Callbacks>

        private DefaultAction(IActionModuleDataTableRecordBridge p_ModuleRecord, IGameEntityBridge p_Entity) : base(ActionModuleDataTableQuery.TableLabel.Default, p_ModuleRecord, p_Entity)
        {
        }

        #endregion
    }
}