using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using xk514;

namespace k514.Mono.Common
{
    public partial class SceneController<Me, Phase, Sequence, Handler, Result>
    {
        #region <Callbacks>

        protected abstract void OnCreatePhase(CancellationToken p_CancellationToken);
        
        protected abstract void _OnEntryPhaseLoop();

        private async void OnTerminatePhaseLoop()
        {
            TurnOffLocalAudioListener();
            Dispose();
            await _OnTerminatePhaseLoop(GetCancellationToken());
        }

        protected abstract UniTask _OnTerminatePhaseLoop(CancellationToken p_CancellationToken);

        #endregion
        
        #region <Methods>

        protected void BridgeEntryPhaseLoop()
        {
#if SERVER_DRIVE
            OnEntryPhaseLoop();
#else
            if (_IsFaderValid)
            {
                SceneFaderManager.GetInstanceUnsafe.SetFadeIn(_OnEntryPhaseLoop);
            }
            else
            {
                _OnEntryPhaseLoop();
            }
#endif
        }
        
        protected void BridgeTerminatePhaseLoop()
        {
#if SERVER_DRIVE
            OnTerminatePhaseLoop();
#else
            if (_IsFaderValid)
            {
                SceneFaderManager.GetInstanceUnsafe.SetFadeOut(OnTerminatePhaseLoop);
            }
            else
            {
                OnTerminatePhaseLoop();
            }
#endif
        }

#if UNITY_EDITOR
        protected async UniTask VoidTest(IAsyncTaskHandler p_AsyncTaskRequest)
        {
#if APPLY_PRINT_LOG
            CustomDebug.LogWarning("[Editor Only] 대기");
            await UniTask.Delay(1000);
            CustomDebug.LogWarning("[Editor Only] 대기 종료");
#else
            await UniTask.CompletedTask;
#endif
        }
#endif
        
        #endregion
    }
}