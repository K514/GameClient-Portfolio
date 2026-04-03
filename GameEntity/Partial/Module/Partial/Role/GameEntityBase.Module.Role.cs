using k514.Mono.Feature;

namespace k514.Mono.Common
{
    public partial class GameEntityBase<Content, CreateParams, ActivateParams>
    {
        #region <Fields>

        /// <summary>
        /// 역할 모듈을 제어하는 모듈 클러스터
        /// </summary>
        protected RoleModuleCluster _RoleModuleCluster;

        /// <summary>
        /// 현재 선택된 역할 모듈
        /// </summary>
        public IRoleModule RoleModule => _RoleModuleCluster.GetCurrentModule();

        /// <summary>
        /// 현재 선택된 역할 모듈이 Null 타입인지 검증하는 프로퍼티
        /// </summary>
        public bool IsRoleModuleNull => _RoleModuleCluster.IsNullModule();

        #endregion
         
        #region <Callbacks>
 
        private void OnCreateRoleModule()
        {
            _RoleModuleCluster.InitModules(ComponentDataRecord.RoleModuleList, GetFallbackRoleModuleType());
        }

        #endregion
         
        #region <Methods>
 
        protected abstract RoleModuleDataTableQuery.TableLabel GetFallbackRoleModuleType();

        public void SwitchDefaultRole()
        {
            _RoleModuleCluster.SwitchDefaultModule();
        }
        
        public void SwitchNullRole()
        {
            _RoleModuleCluster.SwitchNullModule();
        }

        public void SwitchRole(int p_Index)
        {
            _RoleModuleCluster.SwitchModule(p_Index);
        }

        public void SwitchRole(RoleModuleDataTableQuery.TableLabel p_ModuleType)
        {
            _RoleModuleCluster.SwitchModule(p_ModuleType);
        }

        #endregion
    }
}