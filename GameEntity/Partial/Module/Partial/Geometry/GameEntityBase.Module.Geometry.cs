using k514.Mono.Feature;

namespace k514.Mono.Common
{
    public partial class GameEntityBase<Content, CreateParams, ActivateParams>
    {
        #region <Fields>

        /// <summary>
        /// 기하 모듈을 제어하는 모듈 클러스터
        /// </summary>
        protected GeometryModuleCluster _GeometryModuleCluster;

        /// <summary>
        /// 현재 선택된 기하 모듈
        /// </summary>
        public IGeometryModule GeometryModule => _GeometryModuleCluster.GetCurrentModule();

        /// <summary>
        /// 현재 선택된 기하 모듈이 Null 타입인지 검증하는 프로퍼티
        /// </summary>
        public bool IsGeometryModuleNull => _GeometryModuleCluster.IsNullModule();

        #endregion
        
        #region <Callbacks>

        private void OnCreateGeometryModule()
        {
            _GeometryModuleCluster.InitModules(ComponentDataRecord.GeometryModuleList, GetFallbackGeometryModuleType());
        }
        
        #endregion
        
        #region <Methods>

        protected abstract GeometryModuleDataTableQuery.TableLabel GetFallbackGeometryModuleType();

        public void SwitchDefaultGeometryModule()
        {
            _GeometryModuleCluster.SwitchDefaultModule();
        }
        
        public void SwitchNullGeometryModule()
        {
            _GeometryModuleCluster.SwitchNullModule();
        }

        public void SwitchGeometryModule(int p_Index)
        {
            _GeometryModuleCluster.SwitchModule(p_Index);
        }

        public void SwitchGeometryModule(GeometryModuleDataTableQuery.TableLabel p_ModuleType)
        {
            _GeometryModuleCluster.SwitchModule(p_ModuleType);
        }

        #endregion
    }
}