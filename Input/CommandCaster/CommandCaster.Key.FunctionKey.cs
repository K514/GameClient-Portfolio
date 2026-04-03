using System.Collections.Generic;
using UnityEngine;

namespace k514.Mono.Common
{
    public partial class CommandCaster
    {
        #region <Methods>
        
        public void CastFunctionKeyCommandEvent(List<KeyInputState> p_KeyInputState, float p_DeltaTime)
        {
            foreach (var keyCodeState in p_KeyInputState)
            {
                var inputParams = keyCodeState.InputEventParams;
                var inputState = keyCodeState.InputEventParams.InputStateType;
                
                switch (inputState)
                {
                    case InputEventTool.InputStateType.Release:
                    {
                        InputEventSenderManager.GetInstanceUnsafe
                            .SendEvent
                            (
                                _LayerType, InputEventTool.InputKeyType.FunctionKey,
                                new CommandEventParams
                                (
                                    new CommandKeyParams(_LayerType, InputEventTool.InputKeyType.FunctionKey, keyCodeState.MKeyCode, keyCodeState.TriggerKeyType),
                                    new CommandStateParams(InputEventTool.InputStateType.Release,_CurrentArrowSequenceCode, _CurrentInputFlagMask, _CurrentArrowGesture, inputParams.InputGesture, keyCodeState.StartTimeStamp, p_DeltaTime, keyCodeState.ElapsedTime),
                                    InputEventTool.__INPUT_COMMAND_PRIORITY_DEFAULT
                                )
                            );
                        break;
                    }
                    case InputEventTool.InputStateType.Press:
                    {
                        InputEventSenderManager.GetInstanceUnsafe
                            .SendEvent
                            (
                                _LayerType, InputEventTool.InputKeyType.FunctionKey,
                                new CommandEventParams
                                (
                                    new CommandKeyParams(_LayerType, InputEventTool.InputKeyType.FunctionKey, keyCodeState.MKeyCode, keyCodeState.TriggerKeyType),
                                    new CommandStateParams(InputEventTool.InputStateType.Press,_CurrentArrowSequenceCode, _CurrentInputFlagMask, _CurrentArrowGesture, inputParams.InputGesture, keyCodeState.StartTimeStamp, p_DeltaTime, keyCodeState.ElapsedTime),
                                    InputEventTool.__INPUT_COMMAND_PRIORITY_DEFAULT
                                )
                            );
                        break;
                    }
                    case InputEventTool.InputStateType.Holding:
                    {
                        keyCodeState.UpdateElapsed(p_DeltaTime);
                        
                        InputEventSenderManager.GetInstanceUnsafe
                            .SendEvent
                            (
                                _LayerType, InputEventTool.InputKeyType.FunctionKey,
                                new CommandEventParams
                                (
                                    new CommandKeyParams(_LayerType, InputEventTool.InputKeyType.FunctionKey, keyCodeState.MKeyCode, keyCodeState.TriggerKeyType),
                                    new CommandStateParams(InputEventTool.InputStateType.Holding,_CurrentArrowSequenceCode, _CurrentInputFlagMask, _CurrentArrowGesture, inputParams.InputGesture, keyCodeState.StartTimeStamp, p_DeltaTime, keyCodeState.ElapsedTime),
                                    InputEventTool.__INPUT_COMMAND_PRIORITY_DEFAULT
                                )
                            );
                        break;
                    }
                }
            }
        }

        #endregion
    }
}