using System.Threading;
using Cysharp.Threading.Tasks;
using k514.Mono.Feature;

namespace k514.Mono.Common
{
    public abstract class RoleBase : GameEntityModuleBase, IRoleModule
    {
        #region <Consts>

        protected static (bool, RoleModuleDataTableQuery.TableLabel, Module) CreateModule<Module>(Module p_Module)
            where Module : RoleBase
        {
            if (ReferenceEquals(null, p_Module))
            {
                return (false, RoleModuleDataTableQuery.TableLabel.None, default);
            }
            else
            {
                return (true, p_Module._RoleModuleType, p_Module);
            }
        }
        
        protected static async UniTask<(bool, RoleModuleDataTableQuery.TableLabel, Module)> CreateModule<Module>(Module p_Module, CancellationToken p_CancellationToken)
            where Module : RoleBase
        {
            if (ReferenceEquals(null, p_Module))
            {
                return (false, RoleModuleDataTableQuery.TableLabel.None, default);
            }
            else
            {
                return (true, p_Module._RoleModuleType, p_Module);
            }
        }

        #endregion
        
        #region <Fields>

        private RoleModuleDataTableQuery.TableLabel _RoleModuleType { get; set; }
        private IRoleModuleDataTableRecordBridge _RoleModuleRecord { get; set; }
        
        #endregion

        #region <Constructor>

        protected RoleBase(RoleModuleDataTableQuery.TableLabel p_ModuleType, IRoleModuleDataTableRecordBridge p_ModuleRecord, IGameEntityBridge p_Entity) : base(GameEntityModuleTool.ModuleType.Role, p_ModuleRecord, p_Entity)
        {
            _RoleModuleType = p_ModuleType;
            _RoleModuleRecord = p_ModuleRecord;
        }

        #endregion

        #region <Callbacks>

        protected override void _OnAwakeModule()
        {
            // _Entity.SetPreFix(GetPrefix());
        }

        #endregion
        
        #region <Methods>

        public RoleModuleDataTableQuery.TableLabel GetRoleModuleType()
        {
            return _RoleModuleType;
        }
        
#if !SERVER_DRIVE
        protected abstract string GetPrefix();
#endif

        #endregion
    }
}