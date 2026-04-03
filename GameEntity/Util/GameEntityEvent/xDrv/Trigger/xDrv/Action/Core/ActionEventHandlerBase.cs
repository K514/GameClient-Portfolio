using System;

namespace k514.Mono.Common
{
    public interface IActionEventHandler : ITriggerEventHandler<int, ActionEventHandlerCreateParams, ActionEventHandlerActivateParams, IActionDataTableRecordBridge>
    {
        /// <summary>
        /// 액션 모듈
        /// </summary>
        IActionModule ActionModule { get; }
        
        /// <summary>
        /// 해당 액션의 이벤트 타입
        /// </summary>
        ActionEventTool.ActionEventType ActionEventType { get; }
        
        /// <summary>
        /// 해당 액션을 취소하고 발동할 수 있는 다른 액션의 타입 마스크
        ///
        /// 다른 액션의 타입이 None타입인 경우에는 무조건 취소되고 다른 액션이 발동된다.
        /// </summary>
        ActionEventTool.ActionEventType InterruptableMask { get; }
        
        void OnInterruptSuccess();
        void OnInterruptFail();
        void OnInterrupted();
        void OnMindControl();
        void OnReachedGround();
        bool IsEnterableExceptCooldown();
        bool IsInterruptable(IActionEventHandler p_Interrupter);
        bool IsHolding();
        bool IsSelected();
        bool InputPress(CommandEventParams p_CommandPreset);
        bool InputHolding(CommandEventParams p_CommandPreset);
        bool InputRelease(CommandEventParams p_CommandPreset);
        bool ManualInputRelease();
        void HandleClipCue(AnimationTool.ClipEventType p_Type);
        InputEventTool.TriggerKeyType TriggerKey { get; }
        void SetTriggerKey(InputEventTool.TriggerKeyType p_Type);
    }
    
    public readonly struct ActionEventHandlerCreateParams : ITriggerEventHandlerCreateParams<int, IActionDataTableRecordBridge>
    {
        public int EventId { get; }
        public IActionDataTableRecordBridge Record { get; }
        public Type HandlerType => Record.EventHandlerType;

        public ActionEventHandlerCreateParams(int p_EventId)
        {
            EventId = p_EventId;
            Record = ActionDataTableQuery.GetInstanceUnsafe[EventId];
        }
    }

    public readonly struct ActionEventHandlerActivateParams : ITriggerEventHandlerActivateParams
    {
        public IGameEntityBridge Entity { get; }
        public int StartLevel { get; }
        public readonly IActionModule ActionModule;
        
        public ActionEventHandlerActivateParams(IActionModule p_ActionModule, int p_Level)
        {
            ActionModule = p_ActionModule;
            Entity = ActionModule.Entity;
            StartLevel = p_Level;
        }
    }
    
    public abstract partial class ActionEventHandlerBase<This> : TriggerEventHandlerBase<This, int, ActionEventHandlerCreateParams, ActionEventHandlerActivateParams, IActionDataTableRecordBridge>, IActionEventHandler
        where This : ActionEventHandlerBase<This>, new()
    {
        #region <Fields>

        public IActionModule ActionModule { get; private set; }
        public ActionEventTool.ActionEventType ActionEventType { get; protected set; }
        public ActionEventTool.ActionEventType InterruptableMask { get; protected set;  }

        #endregion

        #region <Callbacks>

        public override bool OnActivate(ActionEventHandlerCreateParams p_CreateParams, ActionEventHandlerActivateParams p_ActivateParams, bool p_IsPooled)
        {
            if (base.OnActivate(p_CreateParams, p_ActivateParams, p_IsPooled))
            {
                ActionModule = p_ActivateParams.ActionModule;
                
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void OnRetrieve(ActionEventHandlerCreateParams p_CreateParams)
        {
            InputRelease(default);
            SetTriggerKey(InputEventTool.TriggerKeyType.None);

            base.OnRetrieve(p_CreateParams);
        }
        
        #endregion

        #region <Methods>

        public sealed override bool IsEnterable()
        {
            return !IsCooldown() && IsEnterableExceptCooldown();
        }

        public virtual bool IsEnterableExceptCooldown()
        {
            return HasEnoughCost();
        }

        public bool IsInterruptable(IActionEventHandler p_Interrupter)
        {
            var interrupterActionType = p_Interrupter.ActionEventType;
            return InterruptableMask.HasAnyFlagExceptNone(interrupterActionType) 
                   || interrupterActionType == ActionEventTool.ActionEventType.None;
        }

        #endregion
    }
}