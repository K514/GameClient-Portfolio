using System.Collections.Generic;
using xk514;

namespace k514.Mono.Common
{
    /// <summary>
    /// 게임 개체에 포함된 모듈을 제어하는 부분클래스
    /// </summary>
    public partial class GameEntityBase<Content, CreateParams, ActivateParams>
    {
        #region <Fields>

        /// <summary>
        /// 해당 개체에 포함된 모듈 리스트
        /// </summary>
        private Dictionary<GameEntityModuleTool.ModuleType, IGameEntityModuleCluster> _moduleClusterTable;
        
        #endregion
        
        #region <Callbacks>

        private void OnCreateModule()
        {
            _moduleClusterTable = new Dictionary<GameEntityModuleTool.ModuleType, IGameEntityModuleCluster>();
            _moduleClusterTable.Add(GameEntityModuleTool.ModuleType.Animation, _AnimationModuleCluster = new AnimationModuleCluster(this));
            _moduleClusterTable.Add(GameEntityModuleTool.ModuleType.Action, _ActionModuleCluster = new ActionModuleCluster(this));
            _moduleClusterTable.Add(GameEntityModuleTool.ModuleType.Physics, _PhysicsModuleCluster = new PhysicsModuleCluster(this));
            _moduleClusterTable.Add(GameEntityModuleTool.ModuleType.Geometry, _GeometryModuleCluster = new GeometryModuleCluster(this));
            _moduleClusterTable.Add(GameEntityModuleTool.ModuleType.Mind, _MindModuleCluster = new MindModuleCluster(this));
#if !SERVER_DRIVE
            _moduleClusterTable.Add(GameEntityModuleTool.ModuleType.Render, _RenderModuleCluster = new RenderModuleCluster(this));
#endif
            _moduleClusterTable.Add(GameEntityModuleTool.ModuleType.Role, _RoleModuleCluster = new RoleModuleCluster(this));
            
            OnCreateAnimationModule();
            OnCreateActionModule();
            OnCreatePhysicsModule();
            OnCreateGeometryModule();
            OnCreateMindModule();
#if !SERVER_DRIVE
            OnCreateRenderModule();
#endif
            OnCreateRoleModule();
        }

        private void OnActivateModule()
        {
#if !SERVER_DRIVE
            OnActivateRenderModule();
#endif
            foreach (var moduleClusterKV in _moduleClusterTable)
            {
                var moduleCluster = moduleClusterKV.Value;
                moduleCluster.OnActivateCluster();
            }
        }

        private void OnRetrieveModule(bool p_IsPooled, bool p_IsDisposed)
        {
            if (!p_IsDisposed && p_IsPooled)
            {
                foreach (var moduleClusterKV in _moduleClusterTable)
                {
                    var moduleCluster = moduleClusterKV.Value;
                    moduleCluster.OnRetrieveCluster();
                }
            }
        }

        private void OnDisposeModule()
        {       
            if (!ReferenceEquals(null, _moduleClusterTable))
            {
                foreach (var moduleClusterKV in _moduleClusterTable)
                {
                    var moduleCluster = moduleClusterKV.Value;
                    moduleCluster.Dispose();
                }
                _moduleClusterTable.Clear();
                _moduleClusterTable = null;
            }
        }

        #endregion

        #region <Methods>

        /// <summary>
        /// 모든 모듈을 활성화시키는 메서드
        /// </summary>
        private void AwakeAllModule()
        {
            if (IsAlive)
            {
                var enumerator = GameEntityModuleTool._ModuleTypeEnumerator;
                foreach (var moduleType in enumerator)
                {
                    _moduleClusterTable[moduleType].CurrentModule?.AwakeModule();
                }
            }
        }

        /// <summary>
        /// 모든 모듈을 비활성화시키는 메서드
        /// </summary>
        private void SleepAllModule()
        {
            if (IsAlive)
            {
                var enumerator = GameEntityModuleTool._ModuleTypeEnumerator;
                foreach (var moduleType in enumerator)
                {
                    _moduleClusterTable[moduleType].CurrentModule?.SleepModule();
                }
            }
        }

        /// <summary>
        /// 특정 타입의 모듈 클러스터에 등록된 모듈 개수를 리턴하는 메서드
        /// </summary>
        public int GetModuleCount(GameEntityModuleTool.ModuleType p_ModuleType)
        {
            return _moduleClusterTable[p_ModuleType].GetModuleCount();
        }
        
        /// <summary>
        /// 모든 타입의 모듈 클러스터에 등록된 모듈 개수를 리턴하는 메서드
        /// </summary>
        public int GetModuleCount()
        {
            var result = 0;
            var enumerator = GameEntityModuleTool._ModuleTypeEnumerator;
            foreach (var type in enumerator)
            {
                result += GetModuleCount(type);
            }

            return result;
        }

#if APPLY_PRINT_LOG
        public void PrintModuleInfo()
        {
            var enumerator = GameEntityModuleTool._ModuleTypeEnumerator;
            
            CustomDebug.LogError($"** 이름 : {name}");
            CustomDebug.LogError($"** 보유한 모듈 갯수 : {GetModuleCount()}");

            foreach (var moduleType in enumerator)
            {
                _moduleClusterTable[moduleType].PrintModuleClusterInfo();
            }
        }
#endif

        #endregion
    }
}