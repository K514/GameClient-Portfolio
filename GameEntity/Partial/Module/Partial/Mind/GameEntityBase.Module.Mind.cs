using k514.Mono.Feature;

namespace k514.Mono.Common
{
    public partial class GameEntityBase<Content, CreateParams, ActivateParams>
    {
        #region <Fields>

        /// <summary>
        /// 사고 모듈을 제어하는 모듈 클러스터
        /// </summary>
        public MindModuleCluster _MindModuleCluster;
        
        /// <summary>
        /// 현재 선택된 사고 모듈
        /// </summary>
        public IMindModule MindModule => _MindModuleCluster.GetCurrentModule();

        /// <summary>
        /// 현재 선택된 사고 모듈이 Null 타입인지 검증하는 프로퍼티
        /// </summary>
        public bool IsMindModuleNull => _MindModuleCluster.IsNullModule();

        #endregion

        #region <Callbacks>

        private void OnCreateMindModule()
        {
            _MindModuleCluster.InitModules(ComponentDataRecord.MindModuleList, GetFallbackMindModuleType());
        }

        #endregion

        #region <Methods>

        protected abstract MindModuleDataTableQuery.TableLabel GetFallbackMindModuleType();

        public void SwitchDefaultPersona()
        {
            _MindModuleCluster.SwitchDefaultModule();
        }
        
        public void SwitchNullPersona()
        {
            _MindModuleCluster.SwitchNullModule();
        }

        public void SwitchPersona(int p_Index)
        {
            _MindModuleCluster.SwitchModule(p_Index);
        }

        public void SwitchPersona(AutonomyModuleDataTableQuery.TableLabel p_ModuleType)
        {
            _MindModuleCluster.SwitchModule(MindModuleDataTableQuery.TableLabel.Autonomy, p_ModuleType);
        }
        
        public void SwitchPersona(BoundedModuleDataTableQuery.TableLabel p_ModuleType)
        {
            _MindModuleCluster.SwitchModule(MindModuleDataTableQuery.TableLabel.Bounded, p_ModuleType);
        }

        #endregion
    }
}