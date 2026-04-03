using System;
using System.Collections.Generic;
using UnityEngine;

namespace k514.Mono.Common
{
    public partial class ActionBase
    {
        #region <Fields>

        private List<InputEventTool.TriggerKeyType> _ValidInputCommandList;

        #endregion
        
        #region <Callbacks>

        private void OnCreateCommand()
        {
            _ValidInputCommandList = new List<InputEventTool.TriggerKeyType>();
        }

        private void OnAwakeCommand()
        {
            CurrentJumpCount = 0;
        }
        
        #endregion
        
        #region <Methods>

        public bool InputCommand(CommandEventParams p_Params)
        {
            var triggerKey = p_Params.GetTriggerKey();
            var targetEventHandler = default(IActionEventHandler);
            switch (triggerKey)
            {
                case var _ when triggerKey == ActionTool.__MOVE_TRIGGER:
                case var _ when triggerKey == ActionTool.__JUMP_TRIGGER:
                case var _ when triggerKey == ActionTool.__DASH_TRIGGER:
                case var _ when triggerKey == ActionTool.__GUARD_TRIGGER:
                case var _ when triggerKey == ActionTool.__INTERACT_TRIGGER:
                {
                    targetEventHandler = _QuickCommandHandlerTable[triggerKey];
                    break;
                }
                case var _ when triggerKey == ActionTool.__SEQ_COMMAND_TRIGGER:
                case var _ when triggerKey == ActionTool.__SEQ_COMMAND_TRIGGER2:
                {
                    var handlerList = default(List<IActiveSkillEventHandler>);
                    var seqCommand = p_Params.GetSequenceCommand();
                    while (seqCommand.ArrowSequence > 0)
                    {
                        if (_SequenceCommandHandlerListTable.TryGetValue(seqCommand, out handlerList))
                        {
                            if (handlerList.CheckCollectionSafe())
                            {
                                break;
                            }
                        }
                        
                        var invSeqCommand = seqCommand.GetInverseSequence();
                        if (_SequenceCommandHandlerListTable.TryGetValue(invSeqCommand, out handlerList))
                        {
                            if (handlerList.CheckCollectionSafe())
                            {
                                break;
                            }
                        }
                        
                        seqCommand = seqCommand.GetLowerSequence();
                    }

                    if (seqCommand.ArrowSequence == 0)
                    {
                        _SequenceCommandHandlerListTable.TryGetValue(seqCommand, out handlerList);
                    }

                    if (handlerList.CheckCollectionSafe())
                    {
                        targetEventHandler = handlerList[0];
                    }
                    break;
                }
                default:
                {
                    if (_QuickCommandHandlerTable.TryGetValue(triggerKey, out var o_Handler))
                    {
                        targetEventHandler = o_Handler;
                    }
                    break;
                }
            }

            if (!ReferenceEquals(null, targetEventHandler))
            {
                var inputState = p_Params.InputState;
                switch (inputState)
                {
                    case InputEventTool.InputStateType.Press:
                        return targetEventHandler.InputPress(p_Params);
                    case InputEventTool.InputStateType.Holding:
                        return targetEventHandler.InputHolding(p_Params);
                    case InputEventTool.InputStateType.Release:
                        return targetEventHandler.InputRelease(p_Params);
                }
            }
        
            return false;
        }

        public bool HasQuickCommand(InputEventTool.TriggerKeyType p_CommandType)
        {
            return _QuickCommandHandlerTable.ContainsKey(p_CommandType);
        }

        public List<InputEventTool.TriggerKeyType> GetValidQuickCommandList()
        {
            _ValidInputCommandList.Clear();

            foreach (var handlerKV in _QuickCommandHandlerTable)
            {
                var handler = handlerKV.Value;
                if (handler.IsEnterable())
                {
                    _ValidInputCommandList.Add(handler.TriggerKey);
                }
            }

            return _ValidInputCommandList;
        }
        
        public bool TryGetRandomValidQuickCommand(out InputEventTool.TriggerKeyType o_Command)
        {
            var validQuickCommandList = GetValidQuickCommandList();
            return validQuickCommandList.TryGetRandomElement(out o_Command);
        }
        
        #endregion
    }
}