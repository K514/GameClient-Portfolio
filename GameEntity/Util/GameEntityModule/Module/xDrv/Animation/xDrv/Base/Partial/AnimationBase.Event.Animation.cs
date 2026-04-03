namespace k514.Mono.Common
{
    public abstract partial class AnimationBase
    {
        #region <Callbacks>
        
        public abstract void OnAnimationStart();
        public abstract void OnAnimationStop();
        public abstract void OnAnimationEnd();
        public abstract void OnAnimatorMove();

        #endregion
    }
}