namespace k514.Mono.Common
{
    public class MashingShoulderBangHandler : MashingSkillEventHandlerBase<MashingShoulderBangHandler>
    {
        #region <Consts>

        static MashingShoulderBangHandler()
        {
            _MaxMashingCount = 5;
        }

        #endregion
        
        #region <Callbacks>

        public override void OnCreate(IObjectContent<ActionEventHandlerCreateParams> p_Wrapper, ActionEventHandlerCreateParams p_CreateParams)
        {
            base.OnCreate(p_Wrapper, p_CreateParams);

            ActionEventType = ActionEventTool.ActionEventType.SkillGroup0;
            InterruptableMask = ActionEventTool.ActionEventType.None;
        }
        
        public override void OnInterruptSuccess()
        {
            base.OnInterruptSuccess();

            Entity.AnimationModule.SwitchMotion(AnimationTool.MotionType.Punch, 0, AnimationTool.MotionTransitionType.Bypass_StateMachine);
        }
        
        #endregion
        
        #region <Methods>

        public override void PreloadEvent()
        {
        }

        #endregion
    }
}