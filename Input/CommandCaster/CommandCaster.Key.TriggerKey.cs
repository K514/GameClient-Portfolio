using System.Collections.Generic;

namespace k514.Mono.Common
{
    public partial class CommandCaster
    {
        #region <Methods>
                
        public void CastTriggerKeyCommandEvent(List<KeyInputState> p_KeyInputState, float p_DeltaTime)
        {
            foreach (var keyCodeState in p_KeyInputState)
            {
                var inputCommand = keyCodeState.TriggerKeyType;
                var inputParams = keyCodeState.InputEventParams;
                var inputState = inputParams.InputStateType;
                
                switch (inputState)
                {
                    case InputEventTool.InputStateType.Release:
                    {
                        _CurrentInputFlagMask.RemoveFlag(inputCommand);
                        
                        InputEventSenderManager.GetInstanceUnsafe
                            .SendEvent
                            (
                                _LayerType, InputEventTool.InputKeyType.TriggerKey,
                                new CommandEventParams
                                (
                                    new CommandKeyParams(_LayerType, InputEventTool.InputKeyType.TriggerKey, keyCodeState.MKeyCode, keyCodeState.TriggerKeyType),
                                    new CommandStateParams(InputEventTool.InputStateType.Release, _CurrentArrowSequenceCode, _CurrentInputFlagMask, _CurrentArrowGesture, inputParams.InputGesture, keyCodeState.StartTimeStamp, p_DeltaTime, keyCodeState.ElapsedTime),
                                    InputEventTool.__INPUT_COMMAND_PRIORITY_DEFAULT
                                )
                            );
                        break;
                    }
                    case InputEventTool.InputStateType.Press:
                    {
                        _CurrentInputFlagMask.AddFlag(inputCommand);

                        InputEventSenderManager.GetInstanceUnsafe
                            .SendEvent
                            (
                                _LayerType, InputEventTool.InputKeyType.TriggerKey,
                                new CommandEventParams
                                (
                                    new CommandKeyParams(_LayerType, InputEventTool.InputKeyType.TriggerKey, keyCodeState.MKeyCode, keyCodeState.TriggerKeyType),
                                    new CommandStateParams(InputEventTool.InputStateType.Press, _CurrentArrowSequenceCode, _CurrentInputFlagMask, _CurrentArrowGesture, inputParams.InputGesture, keyCodeState.StartTimeStamp, p_DeltaTime, keyCodeState.ElapsedTime),
                                    InputEventTool.__INPUT_COMMAND_PRIORITY_DEFAULT
                                )
                            );
                        break;
                    }
                    case InputEventTool.InputStateType.Holding:
                    {
                        _CurrentInputFlagMask.AddFlag(inputCommand);
                        keyCodeState.UpdateElapsed(p_DeltaTime);

                        InputEventSenderManager.GetInstanceUnsafe
                            .SendEvent
                            (
                                _LayerType, InputEventTool.InputKeyType.TriggerKey,
                                new CommandEventParams
                                (
                                    new CommandKeyParams(_LayerType, InputEventTool.InputKeyType.TriggerKey, keyCodeState.MKeyCode, keyCodeState.TriggerKeyType),
                                    new CommandStateParams(InputEventTool.InputStateType.Holding, _CurrentArrowSequenceCode, _CurrentInputFlagMask, _CurrentArrowGesture, inputParams.InputGesture, keyCodeState.StartTimeStamp, p_DeltaTime, keyCodeState.ElapsedTime),
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