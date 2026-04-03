namespace k514.Mono.Common
{
    public partial class GameEntityBase<Content, CreateParams, ActivateParams>
    {
        #region <Fields>

        /// <summary>
        /// 물리 모듈을 제어하는 모듈 클러스터
        /// </summary>
        protected PhysicsModuleCluster _PhysicsModuleCluster;

        /// <summary>
        /// 현재 선택된 물리 모듈
        /// </summary>
        public IPhysicsModule PhysicsModule => _PhysicsModuleCluster.GetCurrentModule();

        /// <summary>
        /// 현재 선택된 물리 모듈이 Null 타입인지 검증하는 프로퍼티
        /// </summary>
        public bool IsPhysicsModuleNull => _PhysicsModuleCluster.IsNullModule();

        #endregion

        #region <Callbacks>

        private void OnCreatePhysicsModule()
        {
            _PhysicsModuleCluster.InitModules(ComponentDataRecord.PhysicsModuleList, GetFallbackPhysicsModuleType());
        }

#if ADD_FIXED_UPDATE_GAME_ENTITY
        /// <summary>
        /// 프레임이 아닌 고정된 시간 주기로 호출되는 콜백
        /// 부하가 크기 때문에 물리모듈에만 적용한다.
        /// </summary>
        public void OnFixedUpdate(float p_DeltaTime)
        {
            PhysicsModule.OnFixedUpdate(p_DeltaTime);
        }
#endif
        
        #endregion
    
        #region <Methods>

        protected abstract PhysicsModuleDataTableQuery.TableLabel GetFallbackPhysicsModuleType();

        public void SwitchDefaultPhysicsModule()
        {
            _PhysicsModuleCluster.SwitchDefaultModule();
        }
        
        public void SwitchNullPhysicsModule()
        {
            _PhysicsModuleCluster.SwitchNullModule();
        }

        public void SwitchPhysicsModule(int p_Index)
        {
            _PhysicsModuleCluster.SwitchModule(p_Index);
        }

        public void SwitchPhysicsModule(PhysicsModuleDataTableQuery.TableLabel p_ModuleType)
        {
            _PhysicsModuleCluster.SwitchModule(p_ModuleType);
        }

        #endregion
    }
}