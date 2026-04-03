using System.Collections.Generic;

namespace k514.Mono.Common
{
    public class NullAction : GameEntityModuleBase, IActionModule
    {
        public NullAction() : base(GameEntityModuleTool.ModuleType.None, default, default)
        {
        }

        protected override void _OnAwakeModule()
        {
        }

        protected override void _OnSleepModule()
        {
        }

        protected override void _OnResetModule()
        {
        }

        public void OnAnimationMotionStartCue()
        {
        }

        public void OnAnimationMotionEventCue()
        {
        }

        public void OnAnimationMotionCancelCue()
        {
        }

        public void OnAnimationMotionStopCue()
        {
        }

        public void OnAnimationMotionSoundCue()
        {
        }

        public void OnAnimationMotionEndCue()
        {
        }

        public ActionModuleDataTableQuery.TableLabel GetActionModuleType()
        {
            return default;
        }

        public bool InputCommand(CommandEventParams p_Params)
        {
            return false;
        }

        public bool HasQuickCommand(InputEventTool.TriggerKeyType p_CommandType)
        {
            return false;
        }

        public List<InputEventTool.TriggerKeyType> GetValidQuickCommandList()
        {
            return null;
        }
        
        public bool TryGetRandomValidQuickCommand(out InputEventTool.TriggerKeyType o_Command)
        {
            o_Command = default;
            return false;
        }
        
        public bool IsAvailableManualJump()
        {
            return false;
        }
        
        public bool IsManualJumped()
        {
            return false;
        }

        public void TryInterruptMainHandler(IActionEventHandler p_Handler)
        {
        }

        public void TryReleaseMainHandler(IActionEventHandler p_Handler)
        {
        }

        public void ReleaseAllInput()
        {
        }

        public void RunCooldown()
        {
        }

        public void ProgressCooldown(float p_DeltaTime)
        {
        }

        public void ResetCooldown()
        {
        }

        public Dictionary<InputEventTool.TriggerKeyType, IActionEventHandler> GetQuickCommandHandlerTable()
        {
            return null;
        }

        public bool TryGetQuickCommandHandler(InputEventTool.TriggerKeyType p_Type, out IActionEventHandler o_Handler)
        {
            o_Handler = null;
            return false;
        }

        public IActionEventHandler GetQuickCommandHandlerUnSafe(InputEventTool.TriggerKeyType p_Type)
        {
            return null;
        }

        public List<IActionEventHandler> GetActionHandlerList(ActionDataTableQuery.TableLabel p_Type)
        {
            return null;
        }

        public List<IActiveSkillEventHandler> GetActiveSkillHandlerList()
        {
            return null;
        }

        public List<IPassiveSkillEventHandler> GetPassiveSkillHandlerList()
        {
            return null;
        }

        public bool BindAction(ActionTool.ActionBindPreset p_Preset)
        {
            return false;
        }

        public bool ReleaseAction(IActionEventHandler p_Handler)
        {
            return false;
        }

        public void ChangeQuickCommand(InputEventTool.TriggerKeyType p_TargetCommand, IActiveSkillEventHandler p_Handler)
        {
        }

        public void PrintActionTable()
        {
        }
    }
}