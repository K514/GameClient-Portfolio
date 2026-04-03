using xk514;

namespace k514.Mono.Common
{
    public partial class ActionBase
    {
        public void OnAnimationMotionStartCue()
        {
            _MainActionEventHandler?.HandleClipCue(AnimationTool.ClipEventType.StartCue);
            AnimationModule.OnAnimationStart();
        }
        
        public void OnAnimationMotionEventCue()
        {
            _MainActionEventHandler?.HandleClipCue(AnimationTool.ClipEventType.EventCue);
        }
        
        public void OnAnimationMotionCancelCue()
        {
            _MainActionEventHandler?.HandleClipCue(AnimationTool.ClipEventType.CancelCue);
        }

        public void OnAnimationMotionStopCue()
        {
            _MainActionEventHandler?.HandleClipCue(AnimationTool.ClipEventType.StopCue);
            AnimationModule.OnAnimationStop();
        }
        
        public void OnAnimationMotionSoundCue()
        {
            _MainActionEventHandler?.HandleClipCue(AnimationTool.ClipEventType.SoundCue);
        }
        
        public void OnAnimationMotionEndCue()
        {
            _MainActionEventHandler?.HandleClipCue(AnimationTool.ClipEventType.EndCue);
            AnimationModule.OnAnimationEnd();
        }
    }
}