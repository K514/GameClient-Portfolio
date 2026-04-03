using System;
using k514.Mono.Common;
using k514.Mono.Feature;
using UnityEngine;

namespace k514
{
    public partial class SystemLoop
    {
        #region <Fields>
        
        /// <summary>
        /// 프레임 이벤트 반복자
        /// </summary>
        private SystemOnceFrameEventType[] _Enumerator;
        
        /// <summary>
        /// 프레임 마지막에 처리할 시스템 이벤트 플래그 마스크
        /// </summary>
        private SystemOnceFrameEventType _LateEventFlagMask;

        /// <summary>
        /// 현재 SystemLoop 상태를 표시하는 페이즈 상수
        /// </summary>
        private bool _LoopValidFlag;

        /// <summary>
        /// 업데이트 시간변량
        /// </summary>
        public float LatestUpdateDeltaTime { get; private set; }
        
        /// <summary>
        /// 레이트 업데이트 시간변량
        /// </summary>
        public float LatestLateUpdateDeltaTime { get; private set; }
        
        /// <summary>
        /// 고정 업데이트 시간변량
        /// </summary>
        public float LatestFixedUpdateDeltaTime { get; private set; }

        #endregion
        
        #region <Enums>

        /// <summary>
        /// 시스템 업데이트 함수 이후에 처리할 이벤트 타입
        /// </summary>
        [Flags]
        public enum SystemOnceFrameEventType
        {
            None = 0,
             
            /// <summary>
            /// 화면 플래시
            /// </summary>
            FlashScreen = 1 << 0,
             
            /// <summary>
            /// 시스템이 특정 사운드를 출력해야하는 경우
            /// </summary>
            Beep = 1 << 1,
        }

        #endregion

        #region <Callbacks>

        private void OnCreateSystemLoopCallback()
        {
            _Enumerator = EnumFlag.GetEnumEnumerator<SystemOnceFrameEventType>(EnumFlag.GetEnumeratorType.ExceptNone);
        }
        
        private void Update()
        {
            if (_LoopValidFlag)
            {
                LatestUpdateDeltaTime = Time.deltaTime;

                KeyboardManager.GetInstanceUnsafe?.OnCheckKeyboardInput(LatestUpdateDeltaTime);
                InputLayerManager.GetInstanceUnsafe?.OnUpdate(LatestUpdateDeltaTime);
            }
        }

        private void LateUpdate()
        {
            if (_LoopValidFlag)
            {
                LatestLateUpdateDeltaTime = Time.deltaTime;
                    
                SceneEnvironmentManager.GetInstanceUnsafe?.OnUpdate(LatestLateUpdateDeltaTime);
                GameManager.GetInstanceUnsafe?.OnUpdate(LatestLateUpdateDeltaTime);
                InteractManager.GetInstanceUnsafe?.OnUpdate(LatestLateUpdateDeltaTime);
                CameraManager.GetInstanceUnsafe?.OnLateUpdate(LatestLateUpdateDeltaTime);
                InteractManager.GetInstanceUnsafe?.OnLateUpdate(LatestLateUpdateDeltaTime);
                DirectionalLightController.GetInstanceUnsafe?.OnUpdate(LatestLateUpdateDeltaTime);
                
                UIxObjectRoot.GetInstanceUnsafe?.OnLateUpdate(SystemBoot.UI_DeltaTime);
                SceneFaderManager.GetInstanceUnsafe?.OnLateUpdate(SystemBoot.UI_DeltaTime);
                
                OnHandleOnceFrameEvent();
            }
        }

        /// <summary>
        /// 프레임당 한번만 처리해야하는 이벤트
        /// </summary>
        private void OnHandleOnceFrameEvent()
        {
            foreach (var eventType in _Enumerator)
            {
                if (_LateEventFlagMask.HasAnyFlagExceptNone(eventType))
                {
                    switch (eventType)
                    {
                        case SystemOnceFrameEventType.FlashScreen:
                            break;
                        case SystemOnceFrameEventType.Beep:
                            break;
                    }
                }
            }

            _LateEventFlagMask = SystemOnceFrameEventType.None;
        }

        private void FixedUpdate()
        {
            LatestFixedUpdateDeltaTime = Time.fixedDeltaTime;
            
            if (_LoopValidFlag)
            {
#if ADD_FIXED_UPDATE_GAME_ENTITY
                InteractManager.GetInstanceUnsafe?.OnFixedUpdate(LatestFixedUpdateDeltaTime);
#endif
            }
        }
        
        #endregion

        #region <Methods>

        public void SetGameLoopStart()
        {
            _LoopValidFlag = true;
        }

        public void SetGameLoopPause()
        {
            _LoopValidFlag = false;
        }

        #endregion
    }
}