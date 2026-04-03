namespace k514.Mono.Common
{
    public interface IInteractEventHandler : IActionEventHandler
    {
    }
    
    public abstract class InteractEventHandlerBase<This> : ActionEventHandlerBase<This>, IInteractEventHandler
        where This : InteractEventHandlerBase<This>, new()
    {
        #region <Fields>

        protected new InteractActionDataTable.TableRecord Record { get; private set; }

        #endregion
        
        #region <Callbacks>

        public override void OnCreate(IObjectContent<ActionEventHandlerCreateParams> p_Wrapper, ActionEventHandlerCreateParams p_CreateParams)
        {
            base.OnCreate(p_Wrapper, p_CreateParams);

            Record = base.Record as InteractActionDataTable.TableRecord;
        }

        protected override void OnActionLevelChanged(int p_Prev, int p_Cur)
        {
        }
        
        #endregion
        
        #region <Methods>
        
        public override bool IsEnterableExceptCooldown()
        {
            return base.IsEnterableExceptCooldown() && IsAvailableInteraction();
        }

        private bool IsAvailableInteraction()
        {
            return !Entity.AnimationModule.IsCurrentMotion(AnimationTool.MotionType.Hit)
                   && Entity.HasState_Only(GameEntityTool.EntityStateType.SkillPassMask)
                   && !Entity.PhysicsModule.HasJumpForceBeforeFloat();
        }

        #endregion
    }
}