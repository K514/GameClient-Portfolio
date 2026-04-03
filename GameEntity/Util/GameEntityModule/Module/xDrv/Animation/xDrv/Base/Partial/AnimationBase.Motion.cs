namespace k514.Mono.Common
{
    public abstract partial class AnimationBase
    {
        #region <Methods>

        public abstract bool IsReservedMotion(AnimationTool.MotionType p_Type);
        public abstract bool IsCurrentMotion(AnimationTool.MotionType p_Type);
        public abstract bool IsCurrentMotionProgressing();
        public abstract bool SwitchMotion(AnimationTool.MotionType p_Type, AnimationTool.MotionTransitionType p_TransitionFlag);
        public abstract bool SwitchMotion(AnimationTool.MotionType p_Type, int p_Index, AnimationTool.MotionTransitionType p_TransitionFlag);

        #endregion
    }
}