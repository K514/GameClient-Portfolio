using UnityEngine;
using xk514;

namespace k514.Mono.Common
{
    public partial class InitScene
    {
        #region <Callbacks>

        public override void OnSequenceBegin(InitSceneSequence p_AsyncTaskSequence)
        {
            switch (_CurrentPhase)
            {
                case InitScenePhase.InitSceneStart: 
                    BridgeEntryPhaseLoop();
                    break;
            }
        }

        public override void OnSequenceTerminate(InitSceneSequence p_AsyncTaskSequence)
        {            
            switch (_CurrentPhase)
            {
                case InitScenePhase.InitSceneStart:
                    if (SystemBoot.IsDebugMode())
                    {
                        goto case InitScenePhase.InitSceneTerminate;
                    }
                    else
                    {
                        SwitchPhase(InitScenePhase.InitSceneProcess);
                    }
                    break;
                case InitScenePhase.InitSceneProcess:
                    SwitchPhase(InitScenePhase.InitSceneProcess2);
                    break;
                case InitScenePhase.InitSceneProcess2:
                    SwitchPhase(InitScenePhase.InitSceneTerminate);
                    break;
                case InitScenePhase.InitSceneTerminate:
                    BridgeTerminatePhaseLoop();
                    break;
            }
        }
        
        public override void OnTaskBegin(InitSceneSequence p_AsyncTaskSequence, IAsyncTaskHandler p_Handler)
        {
        }

        public override void OnTaskProgress(IAsyncTaskHandler p_Handler, float p_ProgressRate)
        {
        }

        public override void OnTaskSuccess(InitSceneSequence p_AsyncTaskSequence, IAsyncTaskHandler p_Handler)
        {
        }
        
        public override void OnTaskFail(InitSceneSequence p_AsyncTaskSequence, IAsyncTaskHandler p_Handler)
        {
#if APPLY_PRINT_LOG
            if (CustomDebug.CustomDebugLogFlag.PrintSceneControlLog.HasOpen())
            {
                CustomDebug.LogWarning((this, "[Boot Error] : 시스템 초기화에 실패했습니다."));
            }
#endif
            
            SystemBoot.QuitSystem();
        }
        
        public override void OnTaskCancel(InitSceneSequence p_AsyncTaskSequence, IAsyncTaskHandler p_Handler)
        {
        }

        #endregion
    }
    
    /// <summary>
    /// 다수의 비동기 작업의 순서를 관리하는 구현체
    /// </summary>
    public class InitSceneSequence : AsyncTaskSequenceBase<InitSceneSequence, InitScene.InitScenePhase, int, InitSceneTaskHandler, DefaultAsyncTaskParams, DefaultAsyncTaskResult>
    {
    }
    
    /// <summary>
    /// 비동기 작업 내용을 기술하는 구현체
    /// </summary>
    public class InitSceneTaskHandler : AsyncTaskHandlerBase<InitSceneSequence, int, InitSceneTaskHandler, DefaultAsyncTaskParams, DefaultAsyncTaskResult>
    {
    }
}