#if !SERVER_DRIVE

using UnityEngine;

namespace k514.Mono.Common
{
    public partial class CameraManager
    {
        #region <Fields>

        /// <summary>
        /// 터치 입력 이벤트를 수신받는 오브젝트
        /// </summary>
        private TouchEventReceiver _TouchEventReceiver;

        #endregion

        #region <Callbakcs>

        private void OnCreateTouchEvent()
        {
            _TouchEventReceiver =
                new TouchEventReceiver
                (
                    TouchEventTool.TouchEventType.ControlView, 
                    OnHandleEvent
                );
        }

        private void OnDisposeTouchEvent()
        {
            _TouchEventReceiver?.Dispose();
            _TouchEventReceiver = null;
        }
        
        private void OnHandleEvent(TouchEventTool.TouchEventType p_Type, TouchEventParams p_Preset)
        {
            // 1. 뷰컨트롤이 블록된 경우
            // 2. 뷰컨트롤 회전 이벤트가 진행중이었던 경우
            if (_CameraState.HasAnyFlagExceptNone(CameraTool.CameraStateFlag.BlockManualControl)
                || _RotationLerpIterator.ValidFlag)
            {
                return;
            }

            var touchInputEventPreset = p_Preset.InputGesture;
            var inputState = touchInputEventPreset.InputStateType;
    
            switch (inputState)
            {
                case InputEventTool.InputStateType.Press:
                {
                    break;
                }
                case InputEventTool.InputStateType.Holding:
                {
                    // 커서가 변경되었을 때만 뷰 컨트롤이 동작함
                    var dragGesture = touchInputEventPreset.TouchHoldEventPreset.InputGesture;
                    if (dragGesture.IsDragHandled)
                    {
                        var arrowType = dragGesture.ArrowType;
                        
                        if (arrowType == ArrowType.None)
                        {
                            _CurrentRotationSpeedRate = _CurrentCameraConstantDataRecord.RotationSpeedMinRate;
                        }
                        else
                        {
                            _RotationLerpIterator.Terminate();
                            _RotationDirectionType = arrowType;
                        }
                    }
                    
                    var gestureType = dragGesture.GestureType;
                    switch (gestureType)
                    {
                        case InputEventTool.InputGestureType.None:
                        case InputEventTool.InputGestureType.Stable:
                            break;
                        case InputEventTool.InputGestureType.Gather:
                            AddViewControlZoom(_CurrentCameraConstantDataRecord.ZoomSpeed, Time.deltaTime, false);
                            break;
                        case InputEventTool.InputGestureType.Scatter:
                            AddViewControlZoom(-_CurrentCameraConstantDataRecord.ZoomSpeed, Time.deltaTime, false);
                            break;
                    }

                    break;
                }
                case InputEventTool.InputStateType.Release:
                {
                    _CurrentRotationSpeedRate = _CurrentCameraConstantDataRecord.RotationSpeedMinRate;
                    _RotationDirectionType = ArrowType.None;
                    
                    if (touchInputEventPreset.TouchPressEventPreset.InputStack % 2 == 0)
                    {
                        ResetViewControl();
                    }
                    
                    break;
                }
            }
        }

        #endregion
    }
}

#endif