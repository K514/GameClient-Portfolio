namespace k514.Mono.Common
{
    public partial class GameEntityBase<Content, CreateParams, ActivateParams>
    {
        #region <Fields>

        /// <summary>
        /// 애니메이션 모듈을 제어하는 모듈 클러스터
        /// </summary>
        protected AnimationModuleCluster _AnimationModuleCluster;

        /// <summary>
        /// 현재 선택된 애니메이션 모듈
        /// </summary>
        public IAnimationModule AnimationModule => _AnimationModuleCluster.GetCurrentModule();

        /// <summary>
        /// 현재 선택된 애니메이션 모듈이 Null 타입인지 검증하는 프로퍼티
        /// </summary>
        public bool IsAnimationModuleNull => _AnimationModuleCluster.IsNullModule();

        #endregion
        
        #region <Callbacks>

        private void OnCreateAnimationModule()
        {
            _AnimationModuleCluster.InitModules(ComponentDataRecord.AnimationModuleList, GetFallbackAnimationModuleType());
        }

        #endregion

        #region <Methods>

        protected abstract AnimationModuleDataTableQuery.TableLabel GetFallbackAnimationModuleType();
        
        public void SwitchDefaultAnimationModule()
        {
            _AnimationModuleCluster.SwitchDefaultModule();
        }

        public void SwitchNullAnimationModule()
        {
            _AnimationModuleCluster.SwitchNullModule();
        }

        public void SwitchAnimationModule(int p_Index)
        {
            _AnimationModuleCluster.SwitchModule(p_Index);
        }

        public void SwitchAnimationModule(AnimationModuleDataTableQuery.TableLabel p_ModuleType)
        {
            _AnimationModuleCluster.SwitchModule(p_ModuleType);
        }

        public int GetAnimatorIndex()
        {
            return ModelDataRecord.AnimatorIndex;
        }
        
        #endregion
    }
}