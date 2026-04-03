namespace k514.Mono.Common
{
    public abstract partial class AnimationBase
    {
        #region <Fields>

        protected AnimationTool.IdleMotionType _IdleMode;

        #endregion
        
        #region <Methods>

        public bool IsIdleState(AnimationTool.IdleMotionType p_Type)
        {
            return _IdleMode == p_Type;
        }

        public void SetIdleState(AnimationTool.IdleMotionType p_Type)
        {
            _IdleMode = p_Type;
        }

        public AnimationTool.IdleMotionType GetIdleState()
        {
            return _IdleMode;
        }
        
        public abstract void SwitchToIdleMotion(AnimationTool.MotionTransitionType p_MotionTransitionType);
      
        public void SwitchToIdleMotion(AnimationTool.IdleMotionType p_Type, AnimationTool.MotionTransitionType p_MotionTransitionType)
        {
            SetIdleState(p_Type);
            SwitchToIdleMotion(p_MotionTransitionType);
        }
        
        #endregion
    }
}