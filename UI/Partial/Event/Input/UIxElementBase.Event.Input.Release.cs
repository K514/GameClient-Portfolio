#if !SERVER_DRIVE

using UnityEngine.EventSystems;

namespace k514.Mono.Common
{
    public partial class UIxElementBase : IPointerUpHandler
    {
        #region <Callbacks>

        /// <summary>
        /// 터치가 해제된 경우의 유니티 엔진 입력 콜백
        /// </summary>
        public void OnPointerUp(PointerEventData p_EventData)
        {
            if (_CurInputFlag)
            {        
                _CurInputFlag = false;
           
                if (_UIStaticStateFlagMask.HasAnyFlagExceptNone(UIxTool.UIxStaticStateType.ResetBaseWhenRelease))
                {
                    RectTransform.position = _PointerDownPosition.XYZVector3();
                    _CurrentDistance = 0f;
                }

                CastPointerUpEvent(p_EventData);
            }
        }
        
        #endregion

        #region <Methods>
        
        protected virtual void CastPointerUpEvent(PointerEventData p_EventData)
        {
            if (_UIDynamicStateFlagMask.HasAnyFlagExceptNone(UIxTool.UIxDynamicStateType.HasEvent))
            {
                _LatestPointer = new PointerEventDataPreset(p_EventData);
                if (_UIStaticStateFlagMask.HasAnyFlagExceptNone(UIxTool.UIxStaticStateType.ReleaseInputEventIndependentHover) || IsHovering())
                {
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
                                        CastInputPointerUpEvent();
                                        break;
                                    case UIxTool.UIEventType.TouchEvent:
                                        CastTouchPointerUpEvent();
                                        break;
                                }
                            }
                        }
                    }
                }
                _LatestTouchPressEventPreset = default;
            }
        }

        protected virtual void CastInputPointerUpEvent()
        {
            InputLayerEventSenderManager.GetInstanceUnsafe
                .SendEvent
                (
                    _InputEventParams.InputLayerMask, 
                    new InputLayerEventParams
                    (
                        _InputEventParams.KeyCode, 
                        InputEventTool.InputDeviceType.UI, 
                        InputEventTool.InputStateType.Release, 
                        _LatestTouchPressEventPreset.PointerEventData.TimeStamp,
                        _LatestTouchPressEventPreset.InputStack,
                        _LatestTouchHoldEventPreset.InputGesture
                    )
                );
        }

        protected virtual void CastTouchPointerUpEvent()
        {
            TouchEventManager.GetInstanceUnsafe.OnTouchOver(_TouchEventParams, new TouchInputEventPreset(this, _LatestTouchPressEventPreset, _LatestTouchHoldEventPreset, new TouchReleaseEventPreset(_LatestPointer)));
        }
        
        private void ResetInputReleaseState()
        {
        }

        #endregion
    }
}
#endif