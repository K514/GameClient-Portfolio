namespace k514.Mono.Common
{
    public class DefaultGuardEventHandler : GuardEventHandlerBase<DefaultGuardEventHandler>
    {
        #region <Callbacks>

        public override void OnInterruptSuccess()
        {
            base.OnInterruptSuccess();

            Entity.AnimationModule.SwitchMotion(AnimationTool.MotionType.Punch, 0, AnimationTool.MotionTransitionType.Bypass_StateMachine);
        }

        public override void OnInterrupted()
        {
            base.OnInterrupted();
            
            Entity.AnimationModule.SetAnimationResume();
        }

        protected override void OnPress(CommandEventParams p_CommandPreset)
        {
            ActionModule.TryInterruptMainHandler(this);
        }

        protected override void OnHolding(CommandEventParams p_CommandPreset)
        {
            if (IsSelected())
            {
                Entity.PhysicsModule.ClearVelocityExceptGravity();
            }
            else
            {
                ManualInputRelease();
            }
        }

        protected override void OnRelease(CommandEventParams p_CommandPreset)
        {
            Entity.AnimationModule.SetAnimationResume();
        }
        
        protected override void OnClipCue(AnimationTool.ClipEventType p_Type)
        {
            base.OnClipCue(p_Type);
            
            switch (p_Type)
            {
                case AnimationTool.ClipEventType.StopCue:
                {
                    if (IsHolding())
                    {
                        Entity.AnimationModule.SetAnimationPause();
                    }
                    break;
                }
                case AnimationTool.ClipEventType.EndCue:
                {
                    ActionModule.TryReleaseMainHandler(this);
                    break;
                }
            }
        }
        
        #endregion

        #region <Methods>

        public override void PreloadEvent()
        {
        }
        
        #endregion
    }
}