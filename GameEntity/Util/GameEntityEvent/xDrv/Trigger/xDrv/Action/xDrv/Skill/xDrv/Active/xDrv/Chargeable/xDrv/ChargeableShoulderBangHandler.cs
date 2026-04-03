namespace k514.Mono.Common
{
    public class ChargeableShoulderBangHandler : ChargeableSkillEventHandlerBase<ChargeableShoulderBangHandler>
    {
        #region <Consts>

        static ChargeableShoulderBangHandler()
        {
            SetMaxChargeDuration(1f);
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