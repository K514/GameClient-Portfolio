using k514.Mono.Feature;
using UnityEngine;

namespace k514.Mono.Common
{
    public class NullAnimation : GameEntityModuleBase, IAnimationModule
    {
        public NullAnimation() : base(GameEntityModuleTool.ModuleType.None, default, default)
        {
        }

        protected override void _OnAwakeModule()
        {
        }

        protected override void _OnSleepModule()
        {
        }

        protected override void _OnResetModule()
        {
        }

        public void OnAnimatorMove()
        {
        }

        public AnimationModuleDataTableQuery.TableLabel GetAnimationModuleType()
        {
            return default;
        }

        public bool IsReservedMotion(AnimationTool.MotionType p_Type)
        {
            return false;
        }

        public bool IsCurrentMotion(AnimationTool.MotionType p_Type)
        {
            return false;
        }

        public bool IsCurrentMotionProgressing()
        {
            return false;
        }

        public bool SwitchMotion(AnimationTool.MotionType p_Type, AnimationTool.MotionTransitionType p_TransitionFlag)
        {
            return false;
        }

        public bool SwitchMotion(AnimationTool.MotionType p_Type, int p_Index, AnimationTool.MotionTransitionType p_TransitionFlag)
        {
            return false;
        }

        public bool IsIdleState(AnimationTool.IdleMotionType p_Type)
        {
            return false;
        }

        public void SetIdleState(AnimationTool.IdleMotionType p_Type)
        {
        }

        public AnimationTool.IdleMotionType GetIdleState()
        {
            return default;
        }

        public void SwitchToIdleMotion(AnimationTool.MotionTransitionType p_MotionTransitionType)
        {
        }

        public void SwitchToIdleMotion(AnimationTool.IdleMotionType p_Type, AnimationTool.MotionTransitionType p_MotionTransitionType)
        {
        }

        public bool IsMoveState(AnimationTool.MoveMotionType p_Type)
        {
            return false;
        }

        public void SetMoveState(AnimationTool.MoveMotionType p_Type)
        {
        }

        public void SetMoveRunOrWalk(bool p_Flag)
        {
        }

        public AnimationTool.MoveMotionType GetMoveState()
        {
            return default;
        }

        public void SwitchToMoveMotion(AnimationTool.MotionTransitionType p_MotionTransitionType)
        {
        }

        public void SwitchToMoveMotion(AnimationTool.MoveMotionType p_Type, AnimationTool.MotionTransitionType p_MotionTransitionType)
        {
        }

        public void SwitchHitMotion()
        {
        }

        public void TryHitMotionResume()
        {
        }

        public void SaveMotionAffine()
        {
        }

        public void LoadMotionAffine()
        {
        }

        public void SetAnimationPause()
        {
        }

        public void SetAnimationResume()
        {
        }

        public void SetAnimationSpeed(float p_Factor)
        {
        }

        public void ResetAnimationSpeed()
        {
        }

        public void OnAnimationStart()
        {
        }

        public void OnAnimationStop()
        {
        }

        public void OnAnimationEnd()
        {
        }
    }
}