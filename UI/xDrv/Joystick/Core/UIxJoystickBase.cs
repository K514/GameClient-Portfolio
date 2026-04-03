using System;
using UnityEngine;

#if !SERVER_DRIVE

namespace k514.Mono.Common
{
    public abstract class UIxJoystickBase : UIxElementBase
    {
        #region <Consts>

        private const float _JoystickStateChangeBoundRate = 0.9f;

        #endregion
        
        #region <Fields>

        private UIJoystickHoldingState _CurrentJoystickHoldingState;
        protected ArrowType _PrevArrowType;
        
        #endregion
        
        #region <Enums>

        private enum UIJoystickHoldingState
        {
            /// <summary>
            /// 홀딩 상태가 아닌 경우
            /// </summary>
            None,
            
            /// <summary>
            /// 홀딩 중 스틱 이동거리가 일정 비율 이하인 경우
            /// </summary>
            HoldingAlpha,
            
            /// <summary>
            /// 홀딩 중 스틱 이동거리가 일정 비율 이상이 되어 입력을 한번 취소한 경우
            /// </summary>
            HoldingBeta,
                        
            /// <summary>
            /// 홀딩 중 스틱 이동거리가 일정 비율 이상이 되어 입력을 한번 취소 후 재입력된 경우
            /// </summary>
            HoldingGamma,
        }

        #endregion
        
        #region <Callbacks>
        
        protected override void OnCreate(UIPoolManager.CreateParams p_CreateParams)
        {
            base.OnCreate(p_CreateParams);
            
            SetStateFlag(UIxTool.UIxStaticStateType.FloatRect | UIxTool.UIxStaticStateType.ReleaseInputEventIndependentHover);
        }
        
        protected override void OnCreateInputEvent()
        {
            base.OnCreateInputEvent();
            
            _InputEventParams = new UIxTool.UIInputEventParams(_InputEventParams.InputLayerMask);
        }

        #endregion

        #region <Methods>
        
        protected override void CastInputPointerDownEvent()
        {
        }
        
        protected override void CastInputPointerHoldingEvent()
        {
            var addedArrowType = _CurrentArrowType;
            var keepArrowType = _CurrentArrowType;
            var removedArrowType = _PrevArrowType;
            
            _CurrentJoystickHoldingState = 
                _CurrentJoystickHoldingState switch
                {
                    UIJoystickHoldingState.None or UIJoystickHoldingState.HoldingAlpha 
                    => _CurrentDistance > _JoystickStateChangeBoundRate * _DragMaxDistance
                        ? UIJoystickHoldingState.HoldingBeta
                        : UIJoystickHoldingState.HoldingAlpha,
                    UIJoystickHoldingState.HoldingBeta => UIJoystickHoldingState.HoldingGamma,
                    UIJoystickHoldingState.HoldingGamma => UIJoystickHoldingState.HoldingGamma,
                };

            switch (_CurrentJoystickHoldingState)
            {
                case UIJoystickHoldingState.HoldingAlpha:
                {
                    addedArrowType.RemoveFlag(_PrevArrowType);
                    keepArrowType.RemoveFlag(addedArrowType);
                    removedArrowType.RemoveFlag(_CurrentArrowType);
                    _PrevArrowType = _CurrentArrowType;   
                    break;
                }
                case UIJoystickHoldingState.HoldingBeta:
                {
                    addedArrowType = ArrowType.None;
                    keepArrowType = ArrowType.None;
                    _PrevArrowType = ArrowType.None; 
                    break;
                }
                case UIJoystickHoldingState.HoldingGamma:
                {
                    addedArrowType.RemoveFlag(_PrevArrowType);
                    keepArrowType.RemoveFlag(addedArrowType);
                    removedArrowType.RemoveFlag(_CurrentArrowType);
                    _PrevArrowType = _CurrentArrowType;   
                    break;
                }
            }
            
            var arrowTypeEnumerator = EnumFlag.GetEnumEnumerator<ArrowType>(EnumFlag.GetEnumeratorType.ExceptMaskNone);
            foreach (var arrowType in arrowTypeEnumerator)
            {
                var arrowKeyCode = arrowType.TurnToKeyCode();
                switch (arrowType)
                {
                    case var _ when addedArrowType.HasAnyFlagExceptNone(arrowType):
                    {
                        InputLayerEventSenderManager.GetInstanceUnsafe
                            .SendEvent
                            (
                                _InputEventParams.InputLayerMask, 
                                new InputLayerEventParams
                                (
                                    arrowKeyCode, 
                                    InputEventTool.InputDeviceType.UI, 
                                    InputEventTool.InputStateType.Press, 
                                    _LatestTouchPressEventPreset.PointerEventData.TimeStamp,
                                    _LatestTouchPressEventPreset.InputStack,
                                    default
                                )
                            );
                        break;
                    }
                    case var _ when keepArrowType.HasAnyFlagExceptNone(arrowType):
                    {
                        InputLayerEventSenderManager.GetInstanceUnsafe
                            .SendEvent
                            (
                                _InputEventParams.InputLayerMask, 
                                new InputLayerEventParams
                                (
                                    arrowKeyCode, 
                                    InputEventTool.InputDeviceType.UI, 
                                    InputEventTool.InputStateType.Holding, 
                                    _LatestTouchPressEventPreset.PointerEventData.TimeStamp,
                                    _LatestTouchPressEventPreset.InputStack,
                                    _LatestTouchHoldEventPreset.InputGesture
                                )
                            );
                        break;
                    }
                    case var _ when removedArrowType.HasAnyFlagExceptNone(arrowType):
                    {
                        InputLayerEventSenderManager.GetInstanceUnsafe
                            .SendEvent
                            (
                                _InputEventParams.InputLayerMask, 
                                new InputLayerEventParams
                                (
                                    arrowKeyCode, 
                                    InputEventTool.InputDeviceType.UI, 
                                    InputEventTool.InputStateType.Release, 
                                    _LatestTouchPressEventPreset.PointerEventData.TimeStamp,
                                    _LatestTouchPressEventPreset.InputStack,
                                    _LatestTouchHoldEventPreset.InputGesture
                                )
                            );
                        break;
                    }
                }
            }
        }

        protected override void CastInputPointerUpEvent()
        {
            var arrowTypeEnumerator = EnumFlag.GetEnumEnumerator<ArrowType>(EnumFlag.GetEnumeratorType.ExceptMaskNone);
            foreach (var arrowType in arrowTypeEnumerator)
            {
                if (_PrevArrowType.HasAnyFlagExceptNone(arrowType))
                {
                    InputLayerEventSenderManager.GetInstanceUnsafe
                        .SendEvent
                        (
                            _InputEventParams.InputLayerMask, 
                            new InputLayerEventParams
                            (
                                arrowType.TurnToKeyCode(), 
                                InputEventTool.InputDeviceType.UI, 
                                InputEventTool.InputStateType.Holding, 
                                _LatestTouchPressEventPreset.PointerEventData.TimeStamp,
                                _LatestTouchPressEventPreset.InputStack,
                                _LatestTouchHoldEventPreset.InputGesture
                            )
                        );
                }
            }

            ResetInputDragEventPreset();
            _CurrentJoystickHoldingState = UIJoystickHoldingState.None;
            _PrevArrowType = ArrowType.None;
        }

        #endregion
    }
}
#endif