namespace k514.Mono.Common
{
    public partial class ActionBase
    {
        private void BindQuickCommand()
        {
            var skillHandlerList = _ActiveSkillHandlerList;
            var skillKeyCodeList = ActionTool.__DEFAULT_SKILL_TRIGGER_KEYSET;
            var count = skillKeyCodeList.Length;
            var index = -1;
            
            foreach (var handler in skillHandlerList)
            {
                if (handler.TriggerKey == InputEventTool.TriggerKeyType.None)
                {
                    var tryCommandType = InputEventTool.TriggerKeyType.None;
                    while (++index < count)
                    {
                        tryCommandType = skillKeyCodeList[index];
                        if (!_QuickCommandHandlerTable.ContainsKey(tryCommandType))
                        {
                            break;
                        }
                    }
                    
                    if (tryCommandType == InputEventTool.TriggerKeyType.None)
                    {
                        break;
                    }
                    else
                    {
                        handler.SetTriggerKey(tryCommandType);
                        _QuickCommandHandlerTable.Add(tryCommandType, handler);
                    }
                }
            }
            
            OnActionChanged();
        }
        
        public void ChangeQuickCommand(InputEventTool.TriggerKeyType p_TargetCommand, IActiveSkillEventHandler p_Handler)
        {
            if (!ReferenceEquals(null, p_Handler) && ReferenceEquals(this, p_Handler.ActionModule))
            {
                if (p_Handler.TriggerKey == p_TargetCommand)
                {
                }
                else
                {
                    var prevQuickCommand = p_Handler.TriggerKey;
                    p_Handler.SetTriggerKey(p_TargetCommand);
                  
                    if (_QuickCommandHandlerTable.TryGetValue(p_TargetCommand, out var o_PrevAction))
                    {
                        o_PrevAction.SetTriggerKey(prevQuickCommand);
                        _QuickCommandHandlerTable[p_TargetCommand] = p_Handler;
                    }
                    else
                    {
                        if (p_TargetCommand != InputEventTool.TriggerKeyType.None)
                        {
                            _QuickCommandHandlerTable.Add(p_TargetCommand, p_Handler);
                        }
                    }
               
                    OnActionChanged();
                }
            }
        }
    }
}