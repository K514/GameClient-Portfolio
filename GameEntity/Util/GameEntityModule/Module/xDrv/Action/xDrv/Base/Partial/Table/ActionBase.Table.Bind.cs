using System;
using System.Collections.Generic;
using UnityEngine;

namespace k514.Mono.Common
{
    public partial class ActionBase
    {
        /// <summary>
        /// 생성 레코드를 기준으로 액션을 추가하는 메서드
        /// </summary>
        public void BindAction()
        {
            BindAction(_ActionModuleRecord);
            BindQuickCommand();
        }

        private void BindAction(IActionModuleDataTableRecordBridge p_Record)
        {
            ClearAction();
            
            if (!ReferenceEquals(null, p_Record))
            {
                var actionClusterPresetList = p_Record.ActionBindPresetList;
                if (!ReferenceEquals(null, actionClusterPresetList))
                {
                    foreach (var actionClusterPreset in actionClusterPresetList)
                    {
                        BindAction(actionClusterPreset);
                    }
                }
            }
        }

        public bool BindAction(ActionTool.ActionBindPreset p_Preset)
        {
            if (p_Preset.ValidFlag)
            {
                var actionId = p_Preset.Index;
                if (ActionDataTableQuery.GetInstanceUnsafe.TryGetLabel(actionId, out var o_ActionLabel))
                {
                    var actionEventHandler = ActionEventManager.GetInstanceUnsafe.GetHandler(actionId, new ActionEventHandlerActivateParams(this, p_Preset.StartLevel));
                    switch (o_ActionLabel)
                    {
                        case ActionDataTableQuery.TableLabel.Move:
                        {
                            _ActionEventHandlerListTableByLabel[o_ActionLabel].Add(actionEventHandler);
                            _QuickCommandHandlerTable[ActionTool.__MOVE_TRIGGER] = actionEventHandler;
                            actionEventHandler.SetTriggerKey(ActionTool.__MOVE_TRIGGER);

                            OnActionAdded(actionEventHandler);
                            return true;
                        }
                        case ActionDataTableQuery.TableLabel.Jump:
                        {
                            _ActionEventHandlerListTableByLabel[o_ActionLabel].Add(actionEventHandler);
                            _QuickCommandHandlerTable[ActionTool.__JUMP_TRIGGER] = actionEventHandler;
                            actionEventHandler.SetTriggerKey(ActionTool.__JUMP_TRIGGER);

                            OnActionAdded(actionEventHandler);
                            return true;
                        }
                        case ActionDataTableQuery.TableLabel.Dash:
                        {
                            _ActionEventHandlerListTableByLabel[o_ActionLabel].Add(actionEventHandler);
                            _QuickCommandHandlerTable[ActionTool.__DASH_TRIGGER] = actionEventHandler;
                            actionEventHandler.SetTriggerKey(ActionTool.__DASH_TRIGGER);

                            OnActionAdded(actionEventHandler);
                            return true;
                        }
                        case ActionDataTableQuery.TableLabel.Guard:
                        {
                            _ActionEventHandlerListTableByLabel[o_ActionLabel].Add(actionEventHandler);
                            _QuickCommandHandlerTable[ActionTool.__GUARD_TRIGGER] = actionEventHandler;
                            actionEventHandler.SetTriggerKey(ActionTool.__GUARD_TRIGGER);

                            OnActionAdded(actionEventHandler);
                            return true;
                        }
                        case ActionDataTableQuery.TableLabel.Interact:
                        {
                            _ActionEventHandlerListTableByLabel[o_ActionLabel].Add(actionEventHandler);
                            _QuickCommandHandlerTable[ActionTool.__INTERACT_TRIGGER] = actionEventHandler;
                            actionEventHandler.SetTriggerKey(ActionTool.__INTERACT_TRIGGER);

                            OnActionAdded(actionEventHandler);
                            return true;
                        }
                        case ActionDataTableQuery.TableLabel.Skill:
                        {
                            if (SkillDataTableQuery.GetInstanceUnsafe.TryGetLabel(actionId, out var o_SkillLabel))
                            {
                                switch (o_SkillLabel)
                                {
                                    case SkillDataTableQuery.TableLabel.Active:
                                    {
                                        _ActionEventHandlerListTableByLabel[o_ActionLabel].Add(actionEventHandler);

                                        var activeSkillEventHandler = actionEventHandler as IActiveSkillEventHandler;
                                        _ActiveSkillHandlerList.Add(activeSkillEventHandler);

                                        var seqCommand = activeSkillEventHandler.Record.SequenceCommand;
                                        if (seqCommand.ValidFlag)
                                        {
                                            if (!_SequenceCommandHandlerListTable.TryGetValue(seqCommand, out var o_List))
                                            {
                                                o_List = new List<IActiveSkillEventHandler>();
                                                _SequenceCommandHandlerListTable[seqCommand] = o_List;
                                            }
                                            o_List.Add(activeSkillEventHandler);

                                            if (seqCommand.ArrowSequence == 0)
                                            {
                                                var trigger = seqCommand.TriggerKey;
                                                switch (trigger)
                                                {
                                                    case ActionTool.__SEQ_COMMAND_TRIGGER:
                                                    case ActionTool.__SEQ_COMMAND_TRIGGER2:
                                                        _QuickCommandHandlerTable[trigger] = actionEventHandler;
                                                        actionEventHandler.SetTriggerKey(trigger);
                                                        break;
                                                }
                                            }
                                        }

                                        OnActionAdded(activeSkillEventHandler);
                                        return true;
                                    }
                                    case SkillDataTableQuery.TableLabel.Passive:
                                    {
                                        _ActionEventHandlerListTableByLabel[o_ActionLabel].Add(actionEventHandler);

                                        var passiveSkillEventHandler = actionEventHandler as IPassiveSkillEventHandler;
                                        _PassiveSkillHandlerList.Add(passiveSkillEventHandler);

                                        OnActionAdded(passiveSkillEventHandler);
                                        return true;
                                    }
                                }
                            }
                            break;
                        }
                    }
                }
            }
            
            return false;
        }
        
        public bool ReleaseAction(IActionEventHandler p_Handler)
        {
            if (!ReferenceEquals(null, p_Handler) && ReferenceEquals(this, p_Handler.ActionModule))
            {
                var actionId = p_Handler.EventId;
                if (ActionDataTableQuery.GetInstanceUnsafe.TryGetLabel(actionId, out var o_ActionLabel))
                {
                    switch (o_ActionLabel)
                    {
                        case ActionDataTableQuery.TableLabel.Move:
                        {
                            var actionHandlerList = _ActionEventHandlerListTableByLabel[o_ActionLabel];
                            if (actionHandlerList.Remove(p_Handler))
                            {
                                var actionHandlerCount = actionHandlerList.Count;
                                if (actionHandlerCount > 0)
                                {
                                    _QuickCommandHandlerTable[ActionTool.__MOVE_TRIGGER] = actionHandlerList[actionHandlerCount - 1];
                                }
                                else
                                {
                                    _QuickCommandHandlerTable[ActionTool.__MOVE_TRIGGER] = null;
                                }
                                
                                p_Handler.Pooling();
                                OnActionChanged();
                                
                                return true;
                            }
                            else
                            {
                                p_Handler.Pooling();
                                return false;
                            }
                        }
                        case ActionDataTableQuery.TableLabel.Jump:
                        {
                            var actionHandlerList = _ActionEventHandlerListTableByLabel[o_ActionLabel];
                            if (actionHandlerList.Remove(p_Handler))
                            {
                                var actionHandlerCount = actionHandlerList.Count;
                                if (actionHandlerCount > 0)
                                {
                                    _QuickCommandHandlerTable[ActionTool.__JUMP_TRIGGER] = actionHandlerList[actionHandlerCount - 1];
                                }
                                else
                                {
                                    _QuickCommandHandlerTable[ActionTool.__JUMP_TRIGGER] = null;
                                }
                                
                                p_Handler.Pooling();
                                OnActionChanged();
                                
                                return true;
                            }
                            else
                            {
                                p_Handler.Pooling();
                                return false;
                            }
                        }
                        case ActionDataTableQuery.TableLabel.Dash:
                        {
                            var actionHandlerList = _ActionEventHandlerListTableByLabel[o_ActionLabel];
                            if (actionHandlerList.Remove(p_Handler))
                            {
                                var actionHandlerCount = actionHandlerList.Count;
                                if (actionHandlerCount > 0)
                                {
                                    _QuickCommandHandlerTable[ActionTool.__DASH_TRIGGER] = actionHandlerList[actionHandlerCount - 1];
                                }
                                else
                                {
                                    _QuickCommandHandlerTable[ActionTool.__DASH_TRIGGER] = null;
                                }
                                
                                p_Handler.Pooling();
                                OnActionChanged();
                                
                                return true;
                            }
                            else
                            {
                                p_Handler.Pooling();
                                return false;
                            }
                        }
                        case ActionDataTableQuery.TableLabel.Guard:
                        {
                            var actionHandlerList = _ActionEventHandlerListTableByLabel[o_ActionLabel];
                            if (actionHandlerList.Remove(p_Handler))
                            {
                                var actionHandlerCount = actionHandlerList.Count;
                                if (actionHandlerCount > 0)
                                {
                                    _QuickCommandHandlerTable[ActionTool.__GUARD_TRIGGER] = actionHandlerList[actionHandlerCount - 1];
                                }
                                else
                                {
                                    _QuickCommandHandlerTable[ActionTool.__GUARD_TRIGGER] = null;
                                }
                                
                                p_Handler.Pooling();
                                OnActionChanged();
                                
                                return true;
                            }
                            else
                            {
                                p_Handler.Pooling();
                                return false;
                            }
                        }
                        case ActionDataTableQuery.TableLabel.Interact:
                        {
                            var actionHandlerList = _ActionEventHandlerListTableByLabel[o_ActionLabel];
                            if (actionHandlerList.Remove(p_Handler))
                            {
                                var actionHandlerCount = actionHandlerList.Count;
                                if (actionHandlerCount > 0)
                                {
                                    _QuickCommandHandlerTable[ActionTool.__INTERACT_TRIGGER] = actionHandlerList[actionHandlerCount - 1];
                                }
                                else
                                {
                                    _QuickCommandHandlerTable[ActionTool.__INTERACT_TRIGGER] = null;
                                }
                                
                                p_Handler.Pooling();
                                OnActionChanged();
                                
                                return true;
                            }
                            else
                            {
                                p_Handler.Pooling();
                                return false;
                            }
                        }
                        case ActionDataTableQuery.TableLabel.Skill:
                        {
                            var actionHandlerList = _ActionEventHandlerListTableByLabel[o_ActionLabel];
                            if (actionHandlerList.Remove(p_Handler))
                            {
                                if (SkillDataTableQuery.GetInstanceUnsafe.TryGetLabel(actionId, out var o_SkillLabel))
                                {
                                    switch (o_SkillLabel)
                                    {
                                        case SkillDataTableQuery.TableLabel.Active:
                                        {
                                            var activeSkillEventHandler = actionHandlerList as IActiveSkillEventHandler;
                                            _ActiveSkillHandlerList.Remove(activeSkillEventHandler);

                                            var quickCommand = activeSkillEventHandler.TriggerKey;
                                            if (quickCommand != InputEventTool.TriggerKeyType.None)
                                            {
                                                _QuickCommandHandlerTable.Remove(quickCommand);
                                            }
                                            var seqCommand = activeSkillEventHandler.Record.SequenceCommand;
                                            if (seqCommand.ValidFlag)
                                            {
                                                if (_SequenceCommandHandlerListTable.TryGetValue(seqCommand, out var o_List))
                                                {
                                                    if (o_List.Remove(activeSkillEventHandler))
                                                    {
                                                        if (seqCommand.ArrowSequence == 0)
                                                        {
                                                            var trigger = seqCommand.TriggerKey;
                                                            switch (trigger)
                                                            {
                                                                case ActionTool.__SEQ_COMMAND_TRIGGER:
                                                                case ActionTool.__SEQ_COMMAND_TRIGGER2:
                                                                    var actionHandlerCount = o_List.Count;
                                                                    if (actionHandlerCount > 0)
                                                                    {
                                                                        _QuickCommandHandlerTable[trigger] = o_List[actionHandlerCount - 1];
                                                                    }
                                                                    else
                                                                    {
                                                                        _QuickCommandHandlerTable[trigger] = null;
                                                                    }
                                                                    break;
                                                            }
                                                        }
                                                    }
                                                }
                                                
                              
                                            }
                                            break;
                                        }
                                        case SkillDataTableQuery.TableLabel.Passive:
                                        {
                                            var passiveSkillEventHandler = actionHandlerList as IPassiveSkillEventHandler;
                                            _PassiveSkillHandlerList.Remove(passiveSkillEventHandler);
                                            break;
                                        }
                                    }
                                }
                                
                                OnActionChanged();
                                p_Handler.Pooling();
                                return true;
                            }
                            else
                            {
                                p_Handler.Pooling();
                                return false;
                            }
                        }
                    }
                }
            }

            return false;
        }
        
        private void ClearAction()
        {
            foreach (var handlerListKV in _ActionEventHandlerListTableByLabel)
            {
                var handlerList = handlerListKV.Value;
                foreach (var handler in handlerList)
                {
                    handler.Pooling();
                }
                handlerList.Clear();
            }

            _ActiveSkillHandlerList.Clear();
            _QuickCommandHandlerTable.Clear();
            _SequenceCommandHandlerListTable.Clear();
            _PassiveSkillHandlerList.Clear();
            
            OnActionChanged();
        }
    }
}