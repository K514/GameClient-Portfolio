using System.Threading;
using Cysharp.Threading.Tasks;
using xk514;

namespace k514.Mono.Common
{
    public partial class SceneEnvironment
    {
        public virtual async UniTask OnScenePreload(CancellationToken p_CancellationToken)
        {
            SceneEnvironmentState = SceneTool.SceneChangeEventType.ScenePreload;
#if APPLY_PRINT_LOG
            if (CustomDebug.CustomDebugLogFlag.PrintSceneControlLog.HasOpen())
            {
                CustomDebug.Log((this, $"[{name}] Scene Environment Loading !"));
            }
#endif
            SystemBoot.GetSystemLinkedCancellationTokenSource(ref _CancellationTokenSource);
            
            OnCreateAttribute();
#if APPLY_PPS
            await OnCreatePPS(p_CancellationToken);
#endif
            
            SceneEventSenderManager.GetInstanceUnsafe
                .SendEvent
                (
                    SceneTool.SceneEventType.OnSceneEnvironmentPreload, 
                    new SceneEventParams
                    (
                        SceneEnvironmentManager.GetInstanceUnsafe.CurrentSceneConstantDataRecord,
                        SceneEnvironmentManager.GetInstanceUnsafe.CurrentSceneVariableDataRecord,
                        this
                    )
                );

            await UniTask.CompletedTask;
        }

        public virtual async UniTask OnSceneStart(CancellationToken p_CancellationToken)
        {
            SceneEnvironmentState = SceneTool.SceneChangeEventType.SceneStart;
#if APPLY_PRINT_LOG
            if (CustomDebug.CustomDebugLogFlag.PrintSceneControlLog.HasOpen())
            {
                CustomDebug.Log((this, $"[{name}] Scene Environment Rizap !"));
            }
#endif
            await UniTask.CompletedTask;
        }

        public virtual async UniTask OnSceneTerminate(CancellationToken p_CancellationToken)
        {
            SceneEnvironmentState = SceneTool.SceneChangeEventType.SceneTerminate;
#if APPLY_PRINT_LOG
            if (CustomDebug.CustomDebugLogFlag.PrintSceneControlLog.HasOpen())
            {
                CustomDebug.Log((this, $"[{name}] Scene Environment Disposed"));
            }
#endif
#if APPLY_PPS
            OnDisposePPS();
#endif
            AsyncTaskTool.Dispose(ref _CancellationTokenSource);

            await UniTask.CompletedTask;
        }

        public virtual async UniTask OnSceneTransition(CancellationToken p_CancellationToken)
        {
            SceneEnvironmentState = SceneTool.SceneChangeEventType.SceneTransition;
#if APPLY_PRINT_LOG
            if (CustomDebug.CustomDebugLogFlag.PrintSceneControlLog.HasOpen())
            {
                CustomDebug.Log((this, $"[{name}] Scene Environment Transition to LoadingScene"));
            }
#endif
            await UniTask.CompletedTask;
        }
    }
}