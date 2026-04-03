using System.Threading;
using Cysharp.Threading.Tasks;
using xk514;

namespace k514.Mono.Common
{
    public partial class TitleScene
    {
        #region <Callbacks>

        protected override void _OnEntryPhaseLoop()
        {
        }
        
        protected override async UniTask _OnTerminatePhaseLoop(CancellationToken p_CancellationToken)
        {
            await UniTask.CompletedTask;
            
            switch (_CurrentPhase)
            {
                case TitleScenePhase.StartButton:
                    SystemBoot.OnGameStart();
                    break;
                case TitleScenePhase.OptionButton:
                    break;
                case TitleScenePhase.ReplayButton:
                    break;
                default:
                case TitleScenePhase.None:
                case TitleScenePhase.TitleOpen:
                case TitleScenePhase.ExitButton:
                    break;
            }
        }

        #endregion

        #region <Method/Phase/BootStart>

        private async UniTask Test(IAsyncTaskHandler p_AsyncTaskRequest, CancellationToken p_CancellationToken)
        {
#if APPLY_PRINT_LOG
            CustomDebug.LogWarning("타이틀 씬 테스트를 시작합니다.");
#endif

#if APPLY_PRINT_LOG
            CustomDebug.LogWarning("타이틀 씬 테스트에 성공했습니다..");
#endif
        }
        
        private async UniTask StartButtonProgress(IAsyncTaskHandler p_AsyncTaskRequest, CancellationToken p_CancellationToken)
        {
#if APPLY_PRINT_LOG
            CustomDebug.LogWarning("스타트 버튼 이벤트를 시작합니다.");
#endif
            BridgeTerminatePhaseLoop();
#if APPLY_PRINT_LOG
            CustomDebug.LogWarning("스타트 버튼 이벤트가 끝났습니다.");
#endif
        }

        #endregion
    }
}