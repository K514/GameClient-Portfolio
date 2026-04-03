using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace k514.Mono.Common
{
    /// <summary>
    /// SceneEnvironment를 제어하는 매니저
    /// </summary>
    public class SceneEnvironmentManager : SceneChangeEventReceiveAsyncSingleton<SceneEnvironmentManager>
    {
        #region <Consts>

        private const string SceneEnvironmentObjectName = "SceneObj";

        #endregion

        #region <Fields>

        /// <summary>
        /// 현재 씬의 Environment 객체
        /// </summary>
        public SceneEnvironment CurrentSceneEnvironmentObject { get; private set; }
     
        /// <summary>
        /// 현재 씬 상수 레코드
        /// </summary>
        public SceneConstantDataTable.TableRecord CurrentSceneConstantDataRecord;

        /// <summary>
        /// 현재 씬 변수 레코드
        /// </summary>
        public SceneVariableDataTable.TableRecord CurrentSceneVariableDataRecord;
        
        #endregion
        
        #region <Callbacks>

        protected override void TryInitializeDependency()
        {
            base.TryInitializeDependency();
            
            Priority = 50;
            _Dependencies.Add(typeof(SceneConstantDataTable));
            _Dependencies.Add(typeof(SceneVariableDataTable));
#if APPLY_PPS
            _Dependencies.Add(typeof(PpsObjectPoolManager));
#endif
        }

        protected override async UniTask OnInitiate(CancellationToken p_CancellationToken)
        {
            await UniTask.CompletedTask;
        }

        public void OnUpdate(float p_DeltaTime)
        {
            if (!ReferenceEquals(null, CurrentSceneEnvironmentObject))
            {
                CurrentSceneEnvironmentObject.OnUpdate(p_DeltaTime);
            }
        }
  
        /// <summary>
        /// 씬 로딩 성공 시, 수행할 작업을 수행한다.
        /// </summary>
        public override async UniTask OnScenePreload(CancellationToken p_CancellationToken)
        {
            CurrentSceneConstantDataRecord = SceneChangeManager.GetInstanceUnsafe.CurrentSceneControlPreset.SceneConstantDataRecord;
            CurrentSceneVariableDataRecord = SceneChangeManager.GetInstanceUnsafe.CurrentSceneControlPreset.SceneVariableDataRecord;

            var trySceneEnvironmentType = CurrentSceneVariableDataRecord.SceneEnvironmentType;
            var nativeSceneEnv = Object.FindFirstObjectByType<SceneEnvironment>();
            
            if (nativeSceneEnv == null)
            {
                CurrentSceneEnvironmentObject = new GameObject(SceneEnvironmentObjectName).AddComponent(trySceneEnvironmentType) as SceneEnvironment;
            }
            else
            {
                if (ReferenceEquals(trySceneEnvironmentType, nativeSceneEnv.GetType()))
                {
                    CurrentSceneEnvironmentObject = nativeSceneEnv;
                }
                else
                {
                    Object.DestroyImmediate(nativeSceneEnv.gameObject);
                    CurrentSceneEnvironmentObject = new GameObject(SceneEnvironmentObjectName).AddComponent(trySceneEnvironmentType) as SceneEnvironment;
                }
            }
            await CurrentSceneEnvironmentObject.OnScenePreload(p_CancellationToken); 
        }

        /// <summary>
        /// 씬 실행 시, 수행할 작업을 수행한다.
        /// </summary>
        public override async UniTask OnSceneStart(CancellationToken p_CancellationToken)
        {
            if (!ReferenceEquals(null, CurrentSceneEnvironmentObject))
            {
                await CurrentSceneEnvironmentObject.OnSceneStart(p_CancellationToken);
            }
        }

        /// <summary>
        /// 씬 전이 시, 수행할 작업을 수행한다.
        /// </summary>
        public override async UniTask OnSceneTerminate(CancellationToken p_CancellationToken)
        {
            if (!ReferenceEquals(null, CurrentSceneEnvironmentObject))
            {
                await CurrentSceneEnvironmentObject.OnSceneTerminate(p_CancellationToken);
            }
        }

        /// <summary>
        /// 로딩 씬으로 전이 시, 수행할 작업을 기술한다.
        /// </summary>
        public override async UniTask OnSceneTransition(CancellationToken p_CancellationToken)
        {
            if (!ReferenceEquals(null, CurrentSceneEnvironmentObject))
            {
                await CurrentSceneEnvironmentObject.OnSceneTransition(p_CancellationToken);
                CurrentSceneEnvironmentObject = null;
            }
        }

        #endregion

        #region <Methods>
        
        public bool TryGetSceneEnvironment(out SceneEnvironment o_SceneEnv)
        {
            if (ReferenceEquals(null, CurrentSceneEnvironmentObject))
            {
                o_SceneEnv = null;
                return false;
            }
            else
            {
                o_SceneEnv = CurrentSceneEnvironmentObject;
                return true;
            }
        }
        
        public bool TryGetGamePlaySceneEnvironment(out GameSceneEnvironmentBase o_GameSceneEnv)
        {
            if (TryGetSceneEnvironment(out var o_SceneEnv))
            {
                if (o_SceneEnv is GameSceneEnvironmentBase c_SceneEnv)
                {
                    o_GameSceneEnv = c_SceneEnv;
                    return true;
                }
                else
                {
                    o_GameSceneEnv = null;
                    return false;
                }
            }
            else
            {
                o_GameSceneEnv = null;
                return false;
            }
        }

        #endregion
    }
}