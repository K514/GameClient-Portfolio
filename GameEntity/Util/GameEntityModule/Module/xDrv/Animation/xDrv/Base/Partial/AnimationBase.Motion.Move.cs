using UnityEngine;

namespace k514.Mono.Common
{
    public abstract partial class AnimationBase
    {
        #region <Fields>

        protected AnimationTool.MoveMotionType _MoveMode;

        #endregion
        
        #region <Methods>
        
        public bool IsMoveState(AnimationTool.MoveMotionType p_Type)
        {
            return _MoveMode == p_Type;
        }

        public void SetMoveState(AnimationTool.MoveMotionType p_Type)
        {
            _MoveMode = p_Type;
        }
        
        public void SetMoveRunOrWalk(bool p_Flag)
        {
            _MoveMode = p_Flag ? AnimationTool.MoveMotionType.Run : AnimationTool.MoveMotionType.Walk;
        }

        public AnimationTool.MoveMotionType GetMoveState()
        {
            return _MoveMode;
        }
        
        public abstract void SwitchToMoveMotion(AnimationTool.MotionTransitionType p_MotionTransitionType);

        public void SwitchToMoveMotion(AnimationTool.MoveMotionType p_Type, AnimationTool.MotionTransitionType p_MotionTransitionType)
        {
            SetMoveState(p_Type);
            SwitchToMoveMotion(p_MotionTransitionType);
        }
        
        #endregion
    }
}