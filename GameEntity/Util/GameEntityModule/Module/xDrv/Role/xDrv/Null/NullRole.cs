using k514.Mono.Feature;

namespace k514.Mono.Common
{
    public class NullRole : GameEntityModuleBase, IRoleModule
    {
        #region <Constructor>

        public NullRole() : base(GameEntityModuleTool.ModuleType.None, default, default)
        {
        }

        #endregion

        #region <Callbacks>
        
        protected override void _OnAwakeModule()
        {
        }

        protected override void _OnSleepModule()
        {
        }

        protected override void _OnResetModule()
        {
        }

        #endregion

        #region <Methods>

        public RoleModuleDataTableQuery.TableLabel GetRoleModuleType()
        {
            return RoleModuleDataTableQuery.TableLabel.None;
        }
        
        public string GetPrefix()
        {
            return default;
        }

        #endregion
    }
}