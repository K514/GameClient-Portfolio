using k514.Mono.Feature;

#if !SERVER_DRIVE

namespace k514.Mono.Common
{
    public partial class GameEntityBase<Content, CreateParams, ActivateParams>
    {
        #region <Fields>

        /// <summary>
        /// 랜더 모듈을 제어하는 모듈 클러스터
        /// </summary>
        protected RenderModuleCluster _RenderModuleCluster;

        /// <summary>
        /// 현재 선택된 랜더 모듈
        /// </summary>
        public IRenderModule RenderModule => _RenderModuleCluster.GetCurrentModule();

        /// <summary>
        /// 현재 선택된 랜더 모듈이 Null 타입인지 검증하는 프로퍼티
        /// </summary>
        public bool IsRenderModuleNull => _RenderModuleCluster.IsNullModule();

        #endregion
        
        #region <Callbacks>

        private void OnCreateRenderModule()
        {
            _RenderModuleCluster.InitModules(ComponentDataRecord.RenderModuleList, GetFallbackRenderModuleType());
            OnCreateObserve();
        }
        
        private void OnActivateRenderModule()
        {
            OnActivateObserve();
        }
        
        #endregion
        
        #region <Methods>

        protected abstract RenderModuleDataTableQuery.TableLabel GetFallbackRenderModuleType();

        public void SwitchDefaultRenderModule()
        {
            _RenderModuleCluster.SwitchDefaultModule();
        }
        
        public void SwitchNullRenderModule()
        {
            _RenderModuleCluster.SwitchNullModule();
        }

        public void SwitchRenderModule(int p_Index)
        {
            _RenderModuleCluster.SwitchModule(p_Index);
        }

        public void SwitchRenderModule(RenderModuleDataTableQuery.TableLabel p_ModuleType)
        {
            _RenderModuleCluster.SwitchModule(p_ModuleType);
        }

        #endregion
    }
}
#endif