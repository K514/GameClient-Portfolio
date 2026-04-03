#if !SERVER_DRIVE

using UnityEngine;

namespace k514.Mono.Common
{
    public partial class UIxElementBase
    {
        #region <Fields>

        /// <summary>
        /// 가장 최근에 발생한 터치 hold 이벤트 프리셋
        /// </summary>
        protected TouchHoldEventPreset _LatestTouchHoldEventPreset;
        
        #endregion

        #region <Callbacks>

        private void OnUpdateInputGesture()
        {
            var gestureType = InputEventTool.InputGestureType.None;

#if !UNITY_ANDROID || UNITY_EDITOR
            // PC버전 및 에디터 모드에서는 드래그 여부와 상관 없이 마우스 휠을 기준으로 드래그 이벤트를 발생시킨다.
            var mouseWheelDelta = Input.mouseScrollDelta.y;
            if (mouseWheelDelta.IsReachedZero())
            {
                gestureType = InputEventTool.InputGestureType.Stable;
            }
            else if (mouseWheelDelta > 0f)
            {
                gestureType = InputEventTool.InputGestureType.Gather;
            }
            else
            {
                gestureType = InputEventTool.InputGestureType.Scatter;
            }
            
            _CurrentDragInputSet.Clear();
            _LatestTouchHoldEventPreset = new TouchHoldEventPreset(_LatestPointer, new InputGesture(gestureType, _CurrentArrowType, _LatestWorldPortUV), IsDragUpdated);
            IsDragUpdated = false;
#else
            // 안드로이드에서는 현재 해당 UI를 터치하고 있는 포인터 이벤트 정보를 리스트에 저장하여 그 갯수에 따라
            // 드래그 이벤트를 방생시킨다.
            var dragCount = _CurrentDragInputSet.Count;
            switch (dragCount)
            {
                // 터치한 채로 가만히 있으면 0으로 취급하는 듯 하다..
                case 0:
                // 터치가 하나인 경우
                case 1:
                {
                    _CurrentDragInputSet.Clear();
                    _LatestTouchHoldEventPreset = new TouchHoldEventPreset(_LatestPointer, new InputGesture(gestureType, _LatestArrowType, _LatestWorldPortUV), IsDragUpdated);
                    IsDragUpdated = false;
                    break;
                }
                // 터치가 둘 이상인 경우
                default:
                case 2:
                {
                    var touchEin = _CurrentDragInputSet[0];
                    var touchZwei = _CurrentDragInputSet[1];
                    var touchEinDelta = touchEin.delta;
                    var touchZweiDelta = touchZwei.delta;
                    var touchVectorEinToZwei = touchZwei.position - touchEin.position;
                    var einSimilarity = Vector3.Dot(touchEinDelta, touchVectorEinToZwei);
                    var zweiSimilarity = Vector3.Dot(touchZweiDelta, touchVectorEinToZwei);

                    if (einSimilarity > 0f && zweiSimilarity < 0f)
                    {
                        gestureType = InputEventTool.InputGestureType.Scatter;
                    }
                    else if (einSimilarity < 0f && zweiSimilarity > 0f)
                    {
                        gestureType = InputEventTool.InputGestureType.Gather;
                    }
                    else
                    {
                        gestureType = InputEventTool.InputGestureType.Stable;
                    }
                                
                    _CurrentDragInputSet.Clear();
                    _LatestTouchHoldEventPreset = new TouchHoldEventPreset(_LatestPointer, new InputGesture(gestureType, _LatestArrowType, _LatestWorldPortUV), IsDragUpdated);
                    IsDragUpdated = false;
                    break;
                }
            }
#endif
        }

        #endregion

        #region <Methods>

        private void CastPointeHoldingEvent()
        {
            var enumerator = UIxTool._UIEventTypeEnumerator;
            foreach (var inputEvent in enumerator)
            {
                if (_UIEventFlagMask.HasAnyFlagExceptNone(inputEvent))
                {
                    switch (inputEvent)
                    {
                        case UIxTool.UIEventType.KeyCodeEvent:
                            CastInputPointerHoldingEvent();
                            break;
                        case UIxTool.UIEventType.TouchEvent:
                            CastTouchPointerHoldingEvent();
                            break;
                    }
                }
            }

            if (_UIStaticStateFlagMask.HasAnyFlagExceptNone(UIxTool.UIxStaticStateType.ResetInputDragEveryFrame))
            {
                ResetInputDragEventPreset();
            }
        }

        protected virtual void CastInputPointerHoldingEvent()
        {
            InputLayerEventSenderManager.GetInstanceUnsafe
                .SendEvent
                (
                    _InputEventParams.InputLayerMask, 
                    new InputLayerEventParams
                    (
                        _InputEventParams.KeyCode, 
                        InputEventTool.InputDeviceType.UI, 
                        InputEventTool.InputStateType.Holding, 
                        _LatestTouchPressEventPreset.PointerEventData.TimeStamp,
                        _LatestTouchPressEventPreset.InputStack,
                        _LatestTouchHoldEventPreset.InputGesture
                    )
                );
        }

        protected virtual void CastTouchPointerHoldingEvent()
        {
            TouchEventManager.GetInstanceUnsafe.OnTouchDragging(_TouchEventParams, new TouchInputEventPreset(this, _LatestTouchPressEventPreset, _LatestTouchHoldEventPreset));
        }

        #endregion
    }
}
#endif