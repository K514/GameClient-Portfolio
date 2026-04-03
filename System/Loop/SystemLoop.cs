using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace k514
{
    /// <summary>
    /// 시스템 루프
    /// </summary>
    public partial class SystemLoop : SceneChangeEventReceiveUnityAsyncSingleton<SystemLoop>
    {
        #region <Callbacks>

        protected override async void Awake()
        {
            if (await SystemBoot.StartSystem())
            {
                base.Awake();
            }
        }

        protected override async UniTask OnCreated(CancellationToken p_CancellationToken)
        {
            await base.OnCreated(p_CancellationToken);
            
            // 부분클래스 초기화
            OnCreateSystemLoopCallback();
#if SERVER_DRIVE
            OnCreated_ServerDrive();
#endif

            await UniTask.CompletedTask;
        }

        protected override async UniTask OnInitiate(CancellationToken p_CancellationToken)
        {
            await UniTask.CompletedTask;
        }

        public override async UniTask OnScenePreload(CancellationToken p_CancellationToken)
        {
            SetGameLoopStart();
            
            await UniTask.CompletedTask;
        }

        public override async UniTask OnSceneStart(CancellationToken p_CancellationToken)
        {
            await UniTask.CompletedTask;
        }

        public override async UniTask OnSceneTerminate(CancellationToken p_CancellationToken)
        {
            
            await UniTask.CompletedTask;
        }

        public override async UniTask OnSceneTransition(CancellationToken p_CancellationToken)
        {
            SetGameLoopPause();
            
            await UniTask.CompletedTask;
        }
        
        /// <summary>
        /// 게임모드 혹은 어플리케이션 종료시, 이후 에디터 모드 등을 원할하게 하기 위해서
        /// 시스템에서 사용했던 테이블이나 에셋번들 등을 릴리스 해준다.
        /// </summary>
        protected override void OnDisposeSingleton()
        {
            SetGameLoopPause();

            base.OnDisposeSingleton();
        }

        #endregion

        #region <Methods>

        public void SetSystemLateFlag(SystemOnceFrameEventType p_EventType)
        {
            _LateEventFlagMask.AddFlag(p_EventType);
        }
        
        #endregion
    }
}