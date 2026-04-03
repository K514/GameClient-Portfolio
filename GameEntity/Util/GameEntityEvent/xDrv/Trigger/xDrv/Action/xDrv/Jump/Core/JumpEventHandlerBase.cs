namespace k514.Mono.Common
{
    public interface IJumpEventHandler : IActionEventHandler
    {
    }
    
    public abstract class JumpEventHandlerBase<This> : ActionEventHandlerBase<This>, IJumpEventHandler
        where This : JumpEventHandlerBase<This>, new()
    {
        #region <Fields>

        protected new JumpActionDataTable.TableRecord Record { get; private set; }

        #endregion
        
        #region <Callbacks>

        public override void OnCreate(IObjectContent<ActionEventHandlerCreateParams> p_Wrapper, ActionEventHandlerCreateParams p_CreateParams)
        {
            base.OnCreate(p_Wrapper, p_CreateParams);

            Record = base.Record as JumpActionDataTable.TableRecord;
            ActionEventType = ActionEventTool.ActionEventType.Jump;
            InterruptableMask = ActionEventTool.ActionEventType.Move 
                                | ActionEventTool.ActionEventType.Jump 
                                | ActionEventTool.ActionEventType.Dash 
                                | ActionEventTool.ActionEventType.SkillGroup0;
        }

        protected override void OnActionLevelChanged(int p_Prev, int p_Cur)
        {
        }
        
        public override void OnInterruptSuccess()
        {
            base.OnInterruptSuccess();

            OnJump();
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

        protected abstract void OnJump();

        public override void OnReachedGround()
        {
            ActionModule.TryReleaseMainHandler(this);
        }
        
        #endregion

        #region <Methods>

        public override bool IsEnterableExceptCooldown()
        {
            return base.IsEnterableExceptCooldown() && IsAvailableJump();
        }

        private bool IsAvailableJump()
        {
            return !Entity.AnimationModule.IsCurrentMotion(AnimationTool.MotionType.Hit)
                   && !Entity.GeometryModule.IsOnNavigate()
                   && Entity.HasState_Only(GameEntityTool.EntityStateType.JumpPassMask)
                   && ActionModule.IsAvailableManualJump();
        }

        #endregion
    }
}