using UnityEngine;
using xk514;

namespace k514.Mono.Common
{
    public partial class AnimatorAnimation
    {
        #region <Callbacks>

        public override void OnModule_BeginFloat(PhysicsTool.ForceType p_Mask, Vector3 p_CurrentForce)
        {
            if (!p_Mask.HasAnyFlagExceptNone(PhysicsTool.ForceType.Jump))
            {
                SwitchToIdleMotion(AnimationTool.MotionTransitionType.Bypass_StateMachine);
            }
        }
        
        public override void OnModule_ManualJump()
        {
            SwitchMotion(AnimationTool.MotionType.JumpUp, AnimationTool.MotionTransitionType.Bypass_StateMachine);
        }
        
        public override void OnModule_ReachedGround(PhysicsTool.StampPreset p_UnitStampPreset)
        {
            // 스킬 시전 중에 점프 모션이 진행되면 안되므로 제한을 건다.
            if (Entity.IsFreeAction)
            {
                SwitchMotion(AnimationTool.MotionType.JumpDown, AnimationTool.MotionTransitionType.Bypass_StateMachine);
            }
        }
 
        #endregion
    }
}