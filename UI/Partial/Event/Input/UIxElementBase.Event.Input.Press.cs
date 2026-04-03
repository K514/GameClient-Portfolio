#if !SERVER_DRIVE
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace k514.Mono.Common
{
    public partial class UIxElementBase : IPointerDownHandler
    {
        #region <Fields>
        
        /// <summary>
        /// 최근에 송신한 포인터 이벤트 파라미터
        /// </summary>
        protected PointerEventDataPreset _LatestPointer;
        
        /// <summary>
        /// 마지막으로 입력이 발생한 타임스탬프
        /// </summary>
        private float _LatestInputTimeStamp;
        
        /// <summary>
        /// 현재 입력된 횟수
        /// </summary>
        private int _CurrentInputStack;

        /// <summary>
        /// 가장 최근에 발생한 터치 press 이벤트 프리셋
        /// </summary>
        protected TouchPressEventPreset _LatestTouchPressEventPreset;

        #endregion

        #region <Callbacks>

        /// <summary>
        /// 터치가 감지된 경우의 유니티 엔진 입력 콜백
        /// </summary>
        public void OnPointerDown(PointerEventData p_EventData)
        {
            if (!_CurInputFlag)
            {
                _CurInputFlag = true;
                    
                if (_UIStaticStateFlagMask.HasAnyFlagExceptNone(UIxTool.UIxStaticStateType.FloatPivotWhenPress))
                {
                    _PointerDownPosition = p_EventData.position;
                }

                if (_UIStaticStateFlagMask.HasAnyFlagExceptNone(UIxTool.UIxStaticStateType.FloatRect))
                {
                    var direction = p_EventData.position - _PointerDownPosition;
                    var uv = direction.normalized;
                    _CurrentDistance = direction.magnitude;
                    RectTransform.position = Mathf.Min(_CurrentDistance, _DragMaxDistance) * uv + _PointerDownPosition;
                }

                CastPointerDownEvent(p_EventData);
            }
        }

        #endregion

        #region <Methods>

        protected virtual void CastPointerDownEvent(PointerEventData p_EventData)
        {
            if (_UIDynamicStateFlagMask.HasAnyFlagExceptNone(UIxTool.UIxDynamicStateType.HasEvent))
            {
                UpdateInputStack();

                _LatestPointer = new PointerEventDataPreset(p_EventData);
                _LatestTouchPressEventPreset = new TouchPressEventPreset(_LatestPointer, _CurrentInputStack);
                
                if (_UIDynamicStateFlagMask.HasAnyFlagExceptNone(UIxTool.UIxDynamicStateType.DispatchInputEvent))
                {
                    var enumerator = UIxTool._UIEventTypeEnumerator;
                    foreach (var inputEvent in enumerator)
                    {
                        if (_UIEventFlagMask.HasAnyFlagExceptNone(inputEvent))
                        {
                            switch (inputEvent)
                            {
                                case UIxTool.UIEventType.KeyCodeEvent:
                                    CastInputPointerDownEvent();
                                    break;
                                case UIxTool.UIEventType.TouchEvent:
                                    CastTouchPointerDownEvent();
                                    break;
                            }
                        }
                    }
                }
            }
        }

        protected virtual void CastInputPointerDownEvent()
        {
            InputLayerEventSenderManager.GetInstanceUnsafe
                .SendEvent
                (
                    _InputEventParams.InputLayerMask, 
                    new InputLayerEventParams
                    (
                        _InputEventParams.KeyCode, 
                        InputEventTool.InputDeviceType.UI, 
                        InputEventTool.InputStateType.Press, 
                        _LatestTouchPressEventPreset.PointerEventData.TimeStamp,
                        _LatestTouchPressEventPreset.InputStack,
                        default
                    )
                );
        }

        protected virtual void CastTouchPointerDownEvent()
        {
            TouchEventManager.GetInstanceUnsafe.OnTouchStart(_TouchEventParams, new TouchInputEventPreset(this, _LatestTouchPressEventPreset));
        }
        
        private void UpdateInputStack()
        {
            var currentTimeStamp = Time.time;
            if (currentTimeStamp - _LatestInputTimeStamp < InputEventTool.UIInputStackUpdateIntervalUpperBound)
            {
                _CurrentInputStack++;
            }
            else
            {
                _CurrentInputStack = 1;
            }
            
            _LatestInputTimeStamp = currentTimeStamp;
        }

        private void ResetInputPressState()
        {
            _LatestPointer = default;
            _LatestTouchPressEventPreset = default;
            _LatestInputTimeStamp = 0f;
            _CurrentInputStack = 0;
        }

        #endregion
    }
}
#endif