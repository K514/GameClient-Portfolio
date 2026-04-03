using System.Threading;
using UnityEngine;

namespace k514.Mono.Common
{
    /// <summary>
    /// 각 씬 별로 존재하는 초기화나 전용 씬 기믹, 사용할 리소스 등을 기술하는 공통 클래스
    /// </summary>
    public abstract partial class SceneEnvironment : MonoBehaviour, ISceneChangeEvent
    {
        #region <Fields>
        
        /// <summary>
        /// 현재 씬 환경 상태
        /// </summary>
        public SceneTool.SceneChangeEventType SceneEnvironmentState { get; private set; }

        /// <summary>
        /// 비동기 테스크 취소 토큰
        /// </summary>
        private CancellationTokenSource _CancellationTokenSource;

        #endregion
        
        #region <Callbacks>

        public void OnUpdate(float p_DeltaTime)
        {
            switch (SceneEnvironmentState)
            {
                default:
                case SceneTool.SceneChangeEventType.None:
                case SceneTool.SceneChangeEventType.SceneTransition:
                    break;
                case SceneTool.SceneChangeEventType.ScenePreload:
                    OnUpdateScenePreload(p_DeltaTime);
                    break;
                case SceneTool.SceneChangeEventType.SceneStart:
                    OnUpdateSceneStart(p_DeltaTime);
                    break;
                case SceneTool.SceneChangeEventType.SceneTerminate:
                    OnUpdateSceneTerminate(p_DeltaTime);
                    break;
            }
        }

        protected virtual void OnUpdateScenePreload(float p_DeltaTime)
        {
        }
        
        protected virtual void OnUpdateSceneStart(float p_DeltaTime)
        {
            OnUpdatePPS(p_DeltaTime);
        }
        
        protected virtual void OnUpdateSceneTerminate(float p_DeltaTime)
        {
        }
        
        #endregion
        
        #region <Methods>

        /// <summary>
        /// 해당 씬의 취소 토큰을 리턴하는 메서드
        /// </summary>
        protected CancellationToken GetSceneCancellationToken()
        {
            return _CancellationTokenSource.Token;
        }

        /// <summary>
        /// 씬 시작 프리셋을 가져온다.
        ///
        /// 씬 전이시 지정해준 프리셋이 있다면 해당 값을 우선적으로 가져온다.
        /// </summary>
        public SceneTool.SceneStartPreset GetSceneStartPreset()
        {
            if (SceneChangeManager.GetInstanceUnsafe.CurrentSceneControlPreset.SceneControlPreset.TryGetSceneStartPreset(out var o_Preset))
            {
                return o_Preset;
            }
            else
            {
                return SceneEnvironmentManager.GetInstanceUnsafe.CurrentSceneVariableDataRecord.SceneStartPreset;
            }
        }

        public Vector3 GetStartPosition()
        {
            return GetSceneStartPreset().StartPosition;
        }

        #endregion
    }
}