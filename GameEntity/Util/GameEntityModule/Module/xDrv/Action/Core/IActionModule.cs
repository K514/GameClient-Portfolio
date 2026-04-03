using System.Collections.Generic;

namespace k514.Mono.Common
{
    /// <summary>
    /// 유닛의 액션을 기술하는 인터페이스
    /// </summary>
    public interface IActionModule : IGameEntityModule, IMotionEventReceiver
    {
        /* Default */
        ActionModuleDataTableQuery.TableLabel GetActionModuleType();
        bool IsAvailableManualJump();
        bool IsManualJumped();

        /* Command */
        bool InputCommand(CommandEventParams p_Params);
        bool HasQuickCommand(InputEventTool.TriggerKeyType p_CommandType);
        List<InputEventTool.TriggerKeyType> GetValidQuickCommandList();
        bool TryGetRandomValidQuickCommand(out InputEventTool.TriggerKeyType o_Command);

        /* EventHandler */
        void TryInterruptMainHandler(IActionEventHandler p_Handler);
        void TryReleaseMainHandler(IActionEventHandler p_Handler);
        void ReleaseAllInput();
        void RunCooldown();
        void ProgressCooldown(float p_DeltaTime);
        void ResetCooldown();
        
        /* Table */
        bool BindAction(ActionTool.ActionBindPreset p_Preset);
        bool ReleaseAction(IActionEventHandler p_Handler);
        void ChangeQuickCommand(InputEventTool.TriggerKeyType p_TargetCommand, IActiveSkillEventHandler p_Handler);
#if APPLY_PRINT_LOG
        void PrintActionTable();
#endif
        
        /* Table Query */
        List<IActionEventHandler> GetActionHandlerList(ActionDataTableQuery.TableLabel p_Type);
        Dictionary<InputEventTool.TriggerKeyType, IActionEventHandler> GetQuickCommandHandlerTable();
        bool TryGetQuickCommandHandler(InputEventTool.TriggerKeyType p_Type, out IActionEventHandler o_Handler);
        IActionEventHandler GetQuickCommandHandlerUnSafe(InputEventTool.TriggerKeyType p_Type);
        List<IActiveSkillEventHandler> GetActiveSkillHandlerList();
        List<IPassiveSkillEventHandler> GetPassiveSkillHandlerList();
    }
}