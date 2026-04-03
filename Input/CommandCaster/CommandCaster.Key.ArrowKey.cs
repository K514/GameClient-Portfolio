using System.Collections.Generic;
using UnityEngine;

namespace k514.Mono.Common
{
    public partial class CommandCaster
    {
        #region <Consts>

        private const int _LatestHoldingUVUpdateUpperBound = 3;

        #endregion
        
        #region <Fields>

        private InputGesture _CurrentArrowGesture;
        private float _CurrentArrowInputStartTimeStamp;
        private float _CurrentArrowInputElapsed;
        private int _CurrentArrowSequenceCode;
        private int _LatestArrowCount;
        private int _LatestHoldingUVUpdateCount;
        private Vector3 _LatestHoldingUV;
        
        #endregion

        #region <Callbacks>

        private void OnArrowKeyEventInputted(ArrowType p_ArrowKeyType, Vector3 p_UV, int p_ArrowCount, float p_DeltaTime)
        {
            if (_CurrentArrowGesture.ArrowType == ArrowType.None)
            {
                _CurrentArrowInputElapsed = 0f;
                _LatestArrowCount = p_ArrowCount;
                _LatestHoldingUVUpdateCount = 0;
                _LatestHoldingUV = p_UV;
                _CurrentArrowGesture = new InputGesture(InputEventTool.InputGestureType.None, p_ArrowKeyType, _LatestHoldingUV);
                
                InputEventSenderManager.GetInstanceUnsafe
                    .SendEvent
                    (
                        _LayerType, InputEventTool.InputKeyType.ArrowKey, 
                        new CommandEventParams
                        (
                            new CommandKeyParams(_LayerType, InputEventTool.InputKeyType.ArrowKey, KeyCode.None, InputEventTool.TriggerKeyType.Move),
                            new CommandStateParams(InputEventTool.InputStateType.Press, _CurrentArrowSequenceCode, _CurrentInputFlagMask, _CurrentArrowGesture, _CurrentArrowInputStartTimeStamp, p_DeltaTime, _CurrentArrowInputElapsed),
                            InputEventTool.__INPUT_COMMAND_PRIORITY_DEFAULT
                        )
                    );
            }
            else
            {
                /* SE Cond */
                // 1. 입력 방향 갱신 카운트가 미달되었고,
                // 2. 현재 입력된 방향키의 숫자보다 마지막에 갱신된 입력 방향의 방향키 숫자가 많은 경우(즉, 2방향 입력하다가 1방향이 되었다면 갱신하지 않음.)
                // -> 입력 방향을 갱신하지 않는다.
                if (_LatestHoldingUVUpdateCount < _LatestHoldingUVUpdateUpperBound && _LatestArrowCount > p_ArrowCount)
                {
                    _CurrentArrowInputElapsed += p_DeltaTime;
                    _LatestHoldingUVUpdateCount++;
                    _CurrentArrowGesture = new InputGesture(InputEventTool.InputGestureType.Stable, p_ArrowKeyType, _LatestHoldingUV);
                }
                else
                {
                    _CurrentArrowInputElapsed += p_DeltaTime;
                    _LatestArrowCount = p_ArrowCount;
                    _LatestHoldingUVUpdateCount = 0;
                    _LatestHoldingUV = p_UV;
                    _CurrentArrowGesture = new InputGesture(InputEventTool.InputGestureType.Stable, p_ArrowKeyType, _LatestHoldingUV);
                }
   
                InputEventSenderManager.GetInstanceUnsafe
                    .SendEvent
                    (
                        _LayerType, InputEventTool.InputKeyType.ArrowKey, 
                        new CommandEventParams
                        (
                            new CommandKeyParams(_LayerType, InputEventTool.InputKeyType.ArrowKey, KeyCode.None, InputEventTool.TriggerKeyType.Move),
                            new CommandStateParams(InputEventTool.InputStateType.Holding, _CurrentArrowSequenceCode, _CurrentInputFlagMask, _CurrentArrowGesture, _CurrentArrowInputStartTimeStamp, p_DeltaTime, _CurrentArrowInputElapsed),
                            InputEventTool.__INPUT_COMMAND_PRIORITY_DEFAULT
                        )
                    );
            }
        }
        
        private void OnArrowKeyEventReleased(float p_DeltaTime)
        {
            if (_CurrentArrowGesture.ArrowType != ArrowType.None)
            {
                _CurrentArrowInputElapsed += p_DeltaTime;

                InputEventSenderManager.GetInstanceUnsafe
                    .SendEvent
                    (
                        _LayerType, InputEventTool.InputKeyType.ArrowKey, 
                        new CommandEventParams
                        (
                            new CommandKeyParams(_LayerType, InputEventTool.InputKeyType.ArrowKey, KeyCode.None, InputEventTool.TriggerKeyType.Move),
                            new CommandStateParams(InputEventTool.InputStateType.Release, _CurrentArrowSequenceCode, _CurrentInputFlagMask, _CurrentArrowGesture, _CurrentArrowInputStartTimeStamp, p_DeltaTime, _CurrentArrowInputElapsed),
                            InputEventTool.__INPUT_COMMAND_PRIORITY_DEFAULT
                        )
                    );

                ClearCommandGesture();
                _CurrentArrowInputStartTimeStamp = 0f;
                _CurrentArrowInputElapsed = 0f;
            }
        }

        #endregion
        
        #region <Methods>

        public void CastArrowKeyCommandEvent(List<KeyInputState> p_KeyCodeStateSet, float p_DeltaTime)
        {
            var uv = Vector3.zero;
            var isReleased = _CurrentArrowGesture.ArrowType == ArrowType.None;
            var latestArrowGesture = default(InputGesture);

            foreach (var keyCodeState in p_KeyCodeStateSet)
            {
                var inputCommand = keyCodeState.TriggerKeyType;
                var inputEventParams = keyCodeState.InputEventParams;
                var inputState = inputEventParams.InputStateType;

                switch (inputState)
                {
                    case InputEventTool.InputStateType.Release:
                    {
                        _CurrentInputFlagMask.RemoveFlag(inputCommand);
                        break;
                    }
                    case InputEventTool.InputStateType.Press:
                    {
                        AddArrowCommandQueue(keyCodeState.MIKeyCode).Forget();
                        
                        _CurrentInputFlagMask.AddFlag(inputCommand);
                        uv += inputEventParams.InputGesture.UV;
                        latestArrowGesture = inputEventParams.InputGesture;
                        
                        if (isReleased)
                        {
                            _CurrentArrowInputStartTimeStamp = keyCodeState.StartTimeStamp;
                        }
                        break;
                    }
                    case InputEventTool.InputStateType.Holding:
                    {
                        _CurrentInputFlagMask.AddFlag(inputCommand);
                        keyCodeState.UpdateElapsed(p_DeltaTime);
                        uv += inputEventParams.InputGesture.UV;
                        break;
                    }
                }
            }
            _CurrentArrowSequenceCode = GetArrowCommandCode();
            
            // 입력 비트마스크에서 방향키 부분만 가져온다.
            var currentArrowState = (ArrowType) _CurrentInputFlagMask.FilterFlag(InputEventTool.ArrowKeyCodeFilterMask);
            switch (currentArrowState)
            {
                case ArrowType.None:
                {
                    OnArrowKeyEventReleased(p_DeltaTime);
                    break;
                }
                case ArrowType.Up:
                case ArrowType.Left:
                case ArrowType.Down:
                case ArrowType.Right:
                {
                    OnArrowKeyEventInputted(currentArrowState, uv.normalized, 1, p_DeltaTime);
                    break;
                }
                case ArrowType.UpLeft:
                case ArrowType.LeftDown:
                case ArrowType.DownRight:
                case ArrowType.RightUp:
                {
                    OnArrowKeyEventInputted(currentArrowState, uv.normalized, 2, p_DeltaTime);
                    break;
                }
                case ArrowType.LeftRight:
                case ArrowType.UpDown:
                {
                    if (latestArrowGesture.ValidFlag)
                    {
                        _LatestHoldingUV = latestArrowGesture.UV;
                    }
                    OnArrowKeyEventInputted(currentArrowState, _LatestHoldingUV, 2, p_DeltaTime);
                    break;
                }
                case ArrowType.UpLeftDown:
                case ArrowType.LeftDownRight:
                case ArrowType.DownRightUp:
                case ArrowType.RightUpLeft:
                {
                    if (latestArrowGesture.ValidFlag)
                    {
                        _LatestHoldingUV = latestArrowGesture.UV;
                    }
                    OnArrowKeyEventInputted(currentArrowState, _LatestHoldingUV, 3, p_DeltaTime);
                    break;
                }
                case ArrowType.UpLeftDownRight:
                {
                    if (latestArrowGesture.ValidFlag)
                    {
                        _LatestHoldingUV = latestArrowGesture.UV;
                    }
                    OnArrowKeyEventInputted(currentArrowState, _LatestHoldingUV, 4, p_DeltaTime);
                    break;
                }
            }
        }

        private void ClearCommandGesture()
        {
            _CurrentArrowGesture = default;
        }

        #endregion
    }
}