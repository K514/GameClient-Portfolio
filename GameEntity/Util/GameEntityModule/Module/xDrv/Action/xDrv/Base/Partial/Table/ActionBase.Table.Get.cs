using System.Collections.Generic;

namespace k514.Mono.Common
{
    public partial class ActionBase
    {
        public List<IActionEventHandler> GetActionHandlerList(ActionDataTableQuery.TableLabel p_Type)
        {
            return _ActionEventHandlerListTableByLabel[p_Type];
        }
        
        public Dictionary<InputEventTool.TriggerKeyType, IActionEventHandler> GetQuickCommandHandlerTable()
        {
            return _QuickCommandHandlerTable;
        }

        public bool TryGetQuickCommandHandler(InputEventTool.TriggerKeyType p_Type, out IActionEventHandler o_Handler)
        {
            return _QuickCommandHandlerTable.TryGetValue(p_Type, out o_Handler);
        }
        
        public IActionEventHandler GetQuickCommandHandlerUnSafe(InputEventTool.TriggerKeyType p_Type)
        {
            return _QuickCommandHandlerTable[p_Type];
        }
        
        public List<IActiveSkillEventHandler> GetActiveSkillHandlerList()
        {
            return _ActiveSkillHandlerList;
        }

        public List<IPassiveSkillEventHandler> GetPassiveSkillHandlerList()
        {
            return _PassiveSkillHandlerList;
        }
    }
}