using UnityEngine;

namespace k514.Mono.Common
{
    public partial class GameEntityBase<Content, CreateParams, ActivateParams>
    {
        #region <Fields>

        /// <summary>
        /// 액션 모듈을 제어하는 모듈 클러스터
        /// </summary>
        protected ActionModuleCluster _ActionModuleCluster;

        /// <summary>
        /// 현재 선택된 액션 모듈
        /// </summary>
        public IActionModule ActionModule => _ActionModuleCluster.GetCurrentModule();

        /// <summary>
        /// 현재 선택된 액션 모듈이 Null 타입인지 검증하는 프로퍼티
        /// </summary>
        public bool IsActionModuleNull => _ActionModuleCluster.IsNullModule();
        
        #endregion
        
        #region <Callbacks>

        private void OnCreateActionModule()
        {
            _ActionModuleCluster.InitModules(ComponentDataRecord.ActionModuleList, GetFallbackActionModuleType());
        }

        #endregion
        
        #region <Methods>
        
        protected abstract ActionModuleDataTableQuery.TableLabel GetFallbackActionModuleType();

        public void SwitchDefaultActionModule()
        {
            _ActionModuleCluster.SwitchDefaultModule();
        }
        
        public void SwitchNullActionModule()
        {
            _ActionModuleCluster.SwitchNullModule();
        }

        public void SwitchActionModule(int p_Index)
        {
            _ActionModuleCluster.SwitchModule(p_Index);
        }

        public void SwitchActionModule(ActionModuleDataTableQuery.TableLabel p_ModuleType)
        {
            _ActionModuleCluster.SwitchModule(p_ModuleType);
        }

#if APPLY_PRINT_LOG
        public void PrintActionInfo()
        {
            ActionModule.PrintActionTable();
        }
#endif
        
        #endregion
    }
}