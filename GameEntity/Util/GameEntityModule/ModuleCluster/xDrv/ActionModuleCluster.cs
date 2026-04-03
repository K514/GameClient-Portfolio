using System.Threading;
using Cysharp.Threading.Tasks;

namespace k514.Mono.Common
{
    public class ActionModuleCluster : GameEntityModuleClusterBase<ActionModuleCluster, ActionModuleDataTableQuery.TableLabel, IActionModuleDataTableBridge, IActionModuleDataTableRecordBridge, IActionModule>
    {
        #region <Consts>

        static ActionModuleCluster()
        {
            _NullModule = new NullAction();
        }
        
        #endregion
        
        #region <Constructor>

        public ActionModuleCluster(IGameEntityBridge p_Entity) : base(p_Entity, GameEntityModuleTool.ModuleType.Action, ActionModuleDataTableQuery.GetInstanceUnsafe)
        {
        }

        #endregion

        #region <Methods>

        protected override (bool, ActionModuleDataTableQuery.TableLabel, IActionModule) SpawnModule(int p_Index)
        {
            if (ActionModuleDataTableQuery.GetInstanceUnsafe.TryGetLabelContext(p_Index, out var o_Label, out var o_Table, out var o_Record))
            {
                switch (o_Label)
                {
                    case ActionModuleDataTableQuery.TableLabel.Default:
                        return DefaultAction.CreateModule(o_Record, _GameEntity);
                }
            }

            return default;
        }

        protected override async UniTask<(bool, ActionModuleDataTableQuery.TableLabel, IActionModule)> SpawnModule(int p_Index, CancellationToken p_CancellationToken)
        {
            if (ActionModuleDataTableQuery.GetInstanceUnsafe.TryGetLabelContext(p_Index, out var o_Label, out var o_Table, out var o_Record))
            {
                switch (o_Label)
                {
                    case ActionModuleDataTableQuery.TableLabel.Default:
                        return await DefaultAction.CreateModule(o_Record, _GameEntity, p_CancellationToken);
                }
            }

            return default;
        }
        
        #endregion
    }
}