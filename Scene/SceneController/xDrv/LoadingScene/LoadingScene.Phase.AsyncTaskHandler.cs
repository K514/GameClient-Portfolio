using Cysharp.Threading.Tasks;
using xk514;
#if !UNITY_EDITOR
using UnityEngine;
#endif

namespace k514.Mono.Common
{
    public partial class LoadingScene
    {
        #region <Callbacks>

        public override void OnSequenceBegin(LoadingSceneSequence p_AsyncTaskSequence)
        {
            switch (_CurrentPhase)
            {
                case LoadingScenePhase.UnloadingResource:
                    BridgeEntryPhaseLoop();
                    break;
            }
        }

        public override void OnSequenceTerminate(LoadingSceneSequence p_AsyncTaskSequence)
        {
            switch (_CurrentPhase)
            {
                case LoadingScenePhase.UnloadingResource:
                    SwitchPhase(LoadingScenePhase.LoadingResource);
                    break;
                case LoadingScenePhase.LoadingResource:
                    SwitchPhase(LoadingScenePhase.LoadingScene);
                    break;
                case LoadingScenePhase.LoadingScene:
                    SwitchPhase(LoadingScenePhase.LoadingSceneStageOn);
                    break;
                case LoadingScenePhase.LoadingSceneStageOn:
                    SwitchPhase(LoadingScenePhase.MergeScene);
                    break;
                case LoadingScenePhase.MergeScene:
                    SwitchPhase(LoadingScenePhase.AsyncLoadTerminate);
                    break;
                case LoadingScenePhase.AsyncLoadTerminate:
                    BridgeTerminatePhaseLoop();
                    break;
            }
        }
        
        public override void OnTaskBegin(LoadingSceneSequence p_AsyncTaskSequence, IAsyncTaskHandler p_Handler)
        {
        }
        
        public override void OnTaskProgress(IAsyncTaskHandler p_Handler, float p_ProgressRate)
        {
        }
        
        public override void OnTaskSuccess(LoadingSceneSequence p_AsyncTaskSequence, IAsyncTaskHandler p_Handler)
        {
        }
        
        public override void OnTaskFail(LoadingSceneSequence p_AsyncTaskSequence, IAsyncTaskHandler p_Handler)
        {
#if APPLY_PRINT_LOG
            if (CustomDebug.CustomDebugLogFlag.PrintSceneControlLog.HasOpen())
            {
                CustomDebug.LogError((this, "[SceneLoad Error] : 씬 로딩에 실패했습니다."));
            }
#endif
            
            SystemBoot.QuitSystem();
        }
        
        public override void OnTaskCancel(LoadingSceneSequence p_AsyncTaskSequence, IAsyncTaskHandler p_Handler)
        {
        }

        #endregion
    }
    
    /// <summary>
    /// 다수의 비동기 작업의 순서를 관리하는 구현체
    /// </summary>
    public class LoadingSceneSequence : AsyncTaskSequenceBase<LoadingSceneSequence, LoadingScene.LoadingScenePhase, int, LoadingSceneTaskHandler, DefaultAsyncTaskParams, DefaultAsyncTaskResult>
    {
    }
    
    /// <summary>
    /// 비동기 작업 내용을 기술하는 구현체
    /// </summary>
    public class LoadingSceneTaskHandler : AsyncTaskHandlerBase<LoadingSceneSequence, int, LoadingSceneTaskHandler, DefaultAsyncTaskParams, DefaultAsyncTaskResult>
    {
    }
}