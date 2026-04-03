using UnityEngine;
using xk514;

namespace k514.Mono.Common
{
    public partial class TitleScene
    {
        #region <Callbacks>

        public override void OnSequenceBegin(TitleSceneSequence p_AsyncTaskSequence)
        {
            switch (_CurrentPhase)
            {
                case TitleScenePhase.TitleOpen: 
                    BridgeEntryPhaseLoop();
                    break;
            }
        }

        public override void OnSequenceTerminate(TitleSceneSequence p_AsyncTaskSequence)
        {            
        }
        
        public override void OnTaskBegin(TitleSceneSequence p_AsyncTaskSequence, IAsyncTaskHandler p_Handler)
        {
        }

        public override void OnTaskProgress(IAsyncTaskHandler p_Handler, float p_ProgressRate)
        {
        }

        public override void OnTaskSuccess(TitleSceneSequence p_AsyncTaskSequence, IAsyncTaskHandler p_Handler)
        {
        }
        
        public override void OnTaskFail(TitleSceneSequence p_AsyncTaskSequence, IAsyncTaskHandler p_Handler)
        {
#if APPLY_PRINT_LOG
            if (CustomDebug.CustomDebugLogFlag.PrintSceneControlLog.HasOpen())
            {
                CustomDebug.LogWarning((this, "[Boot Error] : 타이틀 씬 로딩에 실패했습니다."));
            }
#endif

            SystemBoot.QuitSystem();
        }
        
        public override void OnTaskCancel(TitleSceneSequence p_AsyncTaskSequence, IAsyncTaskHandler p_Handler)
        {
        }

        #endregion
    }
    
    /// <summary>
    /// 다수의 비동기 작업의 순서를 관리하는 구현체
    /// </summary>
    public class TitleSceneSequence : AsyncTaskSequenceBase<TitleSceneSequence, TitleScene.TitleScenePhase, int, TitleSceneTaskHandler, DefaultAsyncTaskParams, DefaultAsyncTaskResult>
    {
    }
    
    /// <summary>
    /// 비동기 작업 내용을 기술하는 구현체
    /// </summary>
    public class TitleSceneTaskHandler : AsyncTaskHandlerBase<TitleSceneSequence, int, TitleSceneTaskHandler, DefaultAsyncTaskParams, DefaultAsyncTaskResult>
    {
    }
}