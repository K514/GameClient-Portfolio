namespace k514.Mono.Common
{
    public interface IDashEventHandler : IActionEventHandler
    {
    }
    
    public abstract class DashEventHandlerBase<This> : ActionEventHandlerBase<This>, IDashEventHandler
        where This : DashEventHandlerBase<This>, new()
    {
        #region <Fields>

        protected new DashActionDataTable.TableRecord Record { get; private set; }

        #endregion
        
        #region <Callbacks>

        public override void OnCreate(IObjectContent<ActionEventHandlerCreateParams> p_Wrapper, ActionEventHandlerCreateParams p_CreateParams)
        {
            base.OnCreate(p_Wrapper, p_CreateParams);

            Record = base.Record as DashActionDataTable.TableRecord;
            ActionEventType = ActionEventTool.ActionEventType.Dash;
            InterruptableMask = ActionEventTool.ActionEventType.Jump 
                                | ActionEventTool.ActionEventType.Dash 
                                | ActionEventTool.ActionEventType.Guard;
        }

        protected override void OnActionLevelChanged(int p_Prev, int p_Cur)
        {
        }
        
        public override void OnInterruptSuccess()
        {
            base.OnInterruptSuccess();

            Entity.AddState(GameEntityTool.EntityStateType.DRIVE_DASH);
        }
        
        public override void OnInterrupted()
        {
            base.OnInterrupted();
            
            Entity.RemoveState(GameEntityTool.EntityStateType.DRIVE_DASH);
        }
        
        protected override void OnPress(CommandEventParams p_CommandPreset)
        {
            ActionModule.TryInterruptMainHandler(this);
        }

        protected override void OnHolding(CommandEventParams p_CommandPreset)
        {
        }

        protected override void OnRelease(CommandEventParams p_CommandPreset)
        {
        }
        
        #endregion
        
        #region <Methods>

        public override bool IsEnterableExceptCooldown()
        {
            return base.IsEnterableExceptCooldown() && IsAvailableDash();
        }

        private bool IsAvailableDash()
        {
            return !Entity.AnimationModule.IsCurrentMotion(AnimationTool.MotionType.Hit)
                   && Entity.HasState_Only(GameEntityTool.EntityStateType.DashPassMask);
        }
        
        #endregion
    }
}