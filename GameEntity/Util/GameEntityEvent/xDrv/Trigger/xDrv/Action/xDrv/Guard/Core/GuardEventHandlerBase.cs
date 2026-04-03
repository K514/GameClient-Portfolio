namespace k514.Mono.Common
{
    public interface IGuardEventHandler : IActionEventHandler
    {
    }
    
    public abstract class GuardEventHandlerBase<This> : ActionEventHandlerBase<This>, IGuardEventHandler
        where This : GuardEventHandlerBase<This>, new()
    {
        #region <Fields>

        protected new GuardActionDataTable.TableRecord Record { get; private set; }

        #endregion
        
        #region <Callbacks>

        public override void OnCreate(IObjectContent<ActionEventHandlerCreateParams> p_Wrapper, ActionEventHandlerCreateParams p_CreateParams)
        {
            base.OnCreate(p_Wrapper, p_CreateParams);

            Record = base.Record as GuardActionDataTable.TableRecord;
            ActionEventType = ActionEventTool.ActionEventType.Guard;
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

            Entity.AddState(GameEntityTool.EntityStateType.DRIVE_GUARD);
        }

        public override void OnInterrupted()
        {
            base.OnInterrupted();
            
            Entity.RemoveState(GameEntityTool.EntityStateType.DRIVE_GUARD);
        }
        
        #endregion

        #region <Methods>
        
        public override bool IsEnterableExceptCooldown()
        {
            return base.IsEnterableExceptCooldown() && IsAvailableGuard();
        }

        private bool IsAvailableGuard()
        {
            return !Entity.AnimationModule.IsCurrentMotion(AnimationTool.MotionType.Hit)
                   && Entity.HasState_Only(GameEntityTool.EntityStateType.GuardPassMask)
                   && !Entity.PhysicsModule.HasJumpForceBeforeFloat();
        }

        #endregion
    }
}