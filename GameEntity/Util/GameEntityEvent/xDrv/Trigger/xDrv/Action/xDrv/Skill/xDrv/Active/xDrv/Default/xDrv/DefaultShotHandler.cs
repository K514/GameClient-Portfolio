using UnityEngine;

namespace k514.Mono.Common
{
    public class DefaultShotHandler : DefaultSkillEventHandlerBase<DefaultShotHandler>
    {
        #region <Fields>

        private Vector3 _InputUV;

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

            Entity.AnimationModule.SwitchMotion(AnimationTool.MotionType.Kick, 1, AnimationTool.MotionTransitionType.Bypass_StateMachine);
        }

        protected override void OnProgressEventCue(int p_EventCueIndex)
        {
            switch (p_EventCueIndex)
            {
                case 0:
                    break;
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