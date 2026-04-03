using Cysharp.Threading.Tasks;
using UnityEngine;
using xk514;
#if !UNITY_EDITOR
    using UnityEngine;
#endif

namespace k514.Mono.Common
{
    public partial class PatchScene
    {
        #region <Callbacks>

        public override void OnSequenceBegin(PatchSceneSequence p_AsyncTaskSequence)
        {
            switch (_CurrentPhase)
            {
                case PatchScenePhase.GetPatchList:
                    BridgeEntryPhaseLoop();
                    break;
            }
        }

        public override void OnSequenceTerminate(PatchSceneSequence p_AsyncTaskSequence)
        {
            switch (_CurrentPhase)
            {
                case PatchScenePhase.GetPatchList:
                    SwitchPhase(PatchScenePhase.PatchFile);
                    break;
                case PatchScenePhase.PatchFile:
                    SwitchPhase(PatchScenePhase.PatchTerminate);
                    break;
                case PatchScenePhase.PatchTerminate:
                    BridgeTerminatePhaseLoop();
                    break;
            }
        }
        
        public override void OnTaskBegin(PatchSceneSequence p_AsyncTaskSequence, IAsyncTaskHandler p_Handler)
        {
            var (taskPhase, taskSeqIndex, _) = p_AsyncTaskSequence.GetSequenceKey();
            switch (taskPhase)
            {
                case PatchScenePhase.GetPatchList:
                    switch (taskSeqIndex)
                    {
                        default:
#if !SERVER_DRIVE
                            SetLabelText(SystemMessageTable.KeyType.Patch_DownloadFile);
#endif
                            break;
                    }
                    break;
                case PatchScenePhase.PatchFile:
                    switch (taskSeqIndex)
                    {
                        default:
#if !SERVER_DRIVE
                            SetLabelText(SystemMessageTable.KeyType.Patch_CompareVersion);
#endif
                            break;
                    }
                    break;
                case PatchScenePhase.PatchTerminate:
                    switch (taskSeqIndex)
                    {
                        default:
#if !SERVER_DRIVE
                            SetLabelText(SystemMessageTable.KeyType.Patch_Terminate);
#endif
                            break;
                    }
                    break;
            }
        }
        
        public override void OnTaskProgress(IAsyncTaskHandler p_Handler, float p_ProgressRate)
        {
        }
        
        public override void OnTaskSuccess(PatchSceneSequence p_AsyncTaskSequence, IAsyncTaskHandler p_Handler)
        {
        }
        
        public override void OnTaskFail(PatchSceneSequence p_AsyncTaskSequence, IAsyncTaskHandler p_Handler)
        {
#if APPLY_PRINT_LOG
            if (CustomDebug.CustomDebugLogFlag.PrintSceneControlLog.HasOpen())
            {
                CustomDebug.LogWarning((this, "[Patch Error] : 패치에 실패했습니다."));
            }
#endif

            SystemBoot.QuitSystem();
        }

        public override void OnTaskCancel(PatchSceneSequence p_AsyncTaskSequence, IAsyncTaskHandler p_Handler)
        {
        }

        #endregion
    }
    
    /// <summary>
    /// 다수의 비동기 작업의 순서를 관리하는 구현체
    /// </summary>
    public class PatchSceneSequence : AsyncTaskSequenceBase<PatchSceneSequence, PatchScene.PatchScenePhase, int, PatchSceneTaskHandler, DefaultAsyncTaskParams, DefaultAsyncTaskResult>
    {
    }
    
    /// <summary>
    /// 비동기 작업 내용을 기술하는 구현체
    /// </summary>
    public class PatchSceneTaskHandler : AsyncTaskHandlerBase<PatchSceneSequence, int, PatchSceneTaskHandler, DefaultAsyncTaskParams, DefaultAsyncTaskResult>
    {
    }
}