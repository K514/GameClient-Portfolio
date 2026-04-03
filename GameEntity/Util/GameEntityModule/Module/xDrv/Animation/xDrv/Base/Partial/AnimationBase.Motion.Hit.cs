using UnityEngine;

namespace k514.Mono.Common
{
    public abstract partial class AnimationBase
    {
        #region <Methods>
        
        public void SwitchHitMotion()
        {
            if (Entity.IsAvailablePlayHitMotion())
            {
                SetAnimationResume();
                SwitchMotion(AnimationTool.MotionType.Hit, AnimationTool.MotionTransitionType.Bypass_StateMachine);
                Entity.OnModule_HitMotion_Start();
            }
        }

        public void TryHitMotionResume()
        {
            if (Entity.IsAvailableResumeHitMotion())
            {
                SetAnimationResume();
            }
        }

        #endregion
    }
}