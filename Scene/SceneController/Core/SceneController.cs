using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using xk514;

namespace k514.Mono.Common
{
    public abstract partial class SceneController<Me, Phase, Sequence, Handler, Result> : AsyncTaskUnitySingleton<Me, Phase, Sequence, int, Handler, DefaultAsyncTaskParams, Result>
        where Me : SceneController<Me, Phase, Sequence, Handler, Result>, new()
        where Phase : struct, Enum
        where Sequence : ObjectPoolContent<Sequence, IAsyncTaskSingleton<Sequence>, AsyncTaskSequenceInitiateParams<Phase>>, IAsyncTaskSequence<Handler>, new()
        where Handler : AsyncTaskHandlerBase<Sequence, int, Handler, DefaultAsyncTaskParams, Result>, new()
        where Result : IAsyncTaskResult
    {
        #region <Fields>

        [SerializeField] protected GameObject _LocalSceneCameraGameObject;
        [SerializeField] protected AudioListener _LocalSceneCameraAudioListener;
        
        #endregion
        
        #region <Callbacks>

        protected override async UniTask OnCreated(CancellationToken p_CancellationToken)
        {
            await base.OnCreated(p_CancellationToken);
            
            OnCreatePhase(p_CancellationToken);
#if !SERVER_DRIVE
            OnCreateUI();
#endif
            
            SystemLoop.GetInstanceUnsafe.SetGameLoopStart();
        }

        /// <summary>
        /// 해당 오브젝트를 제거한다.
        /// 오브젝트 제거는 매 로딩 종료시에 일어나므로, 해당 파기메서드의 호출은 반드시 게임 시스템의 종료를 의미하는 것은 아니다.
        /// </summary>
        protected override void OnDisposeSingleton()
        {
#if !SERVER_DRIVE
            if (_ProgressBar.ValidFlag)
            {
                _ProgressBar.Dispose();
            }
            _ProgressBar = default;
            
            if (_IsFaderValid)
            {
                _IsFaderValid = false;
                Fader.Dispose();
                Fader = null;
            }
#endif
            if (this != null)
            {
                Destroy(gameObject);
            }
            
#if APPLY_PRINT_LOG
            if (CustomDebug.CustomDebugLogFlag.PrintSceneControlLog.HasOpen())
            {
                CustomDebug.LogWarning((this, "Notice : 패치 씬이 파기되었습니다."));
            }
#endif
            
            base.OnDisposeSingleton();
        }

        #endregion
        
        #region <Methods>

        protected void TurnOffLocalCamera()
        {
            try
            {
                _LocalSceneCameraGameObject.SetActive(false);
            }
            catch
            {
                //
            }
        }

        protected void TurnOffLocalAudioListener()
        {
            try
            {
                _LocalSceneCameraAudioListener.enabled = false;
            }
            catch
            {
                //
            }
        }
        
        #endregion
    }
}