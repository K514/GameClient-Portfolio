using System.Threading;
using Cysharp.Threading.Tasks;
using xk514;

namespace k514.Mono.Common
{
    public partial class InitScene
    {
        #region <Callbacks>

        protected override void _OnEntryPhaseLoop()
        {
        }
        
        protected override async UniTask _OnTerminatePhaseLoop(CancellationToken p_CancellationToken)
        {
            await UniTask.CompletedTask;
            
            SystemBoot.OnGameInitSuccess();
        }

        #endregion

        #region <Method/Phase/BootStart>

        private async UniTask BootingStart(IAsyncTaskHandler p_AsyncTaskRequest, CancellationToken p_CancellationToken)
        {
#if APPLY_PRINT_LOG
            CustomDebug.LogWarning("부팅 연출 및 시스템 테이블 로딩을 시작합니다.");
#endif
#if !SERVER_DRIVE
            // InitializedUI();
            // SetPlayVideo();
#endif
#if APPLY_PRINT_LOG
            CustomDebug.LogWarning("시스템 테이블 로딩에 성공했습니다.");
#endif
        }

        #endregion

        #region <Method/Phase/SOP>

        private async UniTask LoadGameManager(IAsyncTaskHandler p_AsyncTaskRequest, CancellationToken p_CancellationToken)
        {
#if APPLY_PRINT_LOG
            CustomDebug.LogWarning("게임 매니저 로딩을 시작합니다.");
#endif
            await SystemBoot.LoadGameManager(p_CancellationToken);
#if APPLY_PRINT_LOG
            CustomDebug.LogWarning("게임 매니저 로딩에 성공했습니다.");
#endif
        }

        private async UniTask LoadSceneManager(IAsyncTaskHandler p_AsyncTaskRequest, CancellationToken p_CancellationToken)
        {
#if APPLY_PRINT_LOG
            CustomDebug.LogWarning("씬 매니저 로딩을 시작합니다.");
#endif
            await SystemBoot.LoadSceneManager(p_CancellationToken);
#if APPLY_PRINT_LOG
            CustomDebug.LogWarning("씬 매니저 로딩에 성공했습니다.");
#endif
        }

#if !SERVER_DRIVE
        private async UniTask LoadExtraManager(IAsyncTaskHandler p_AsyncTaskRequest, CancellationToken p_CancellationToken)
        {
#if APPLY_PRINT_LOG
            CustomDebug.LogWarning("엑스트라 매니저 로딩을 시작합니다.");
#endif
            await SystemBoot.LoadCameraManager(p_CancellationToken);
            await SystemBoot.LoadAudioManager(p_CancellationToken);
            await SystemBoot.LoadUI(p_CancellationToken);
            
#if APPLY_PRINT_LOG
            CustomDebug.LogWarning("엑스트라 매니저 로딩에 성공했습니다.");
#endif
        }
#endif

#if UNITY_EDITOR
        private async UniTask BootEditorOnly(IAsyncTaskHandler p_AsyncTaskRequest, CancellationToken p_CancellationToken)
        {
    #if APPLY_PRINT_LOG
            if (SystemFlagTable.IsSystemReleaseMode())
            {
                CustomDebug.LogWarning("Notice : 배포 모드로 실행됬습니다.");
            }
            else
            {
                CustomDebug.LogWarning("Notice : 개발 모드로 실행됬습니다.");
            }
    #endif
        }
#endif
        
        #endregion
    }
}