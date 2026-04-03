namespace k514.Mono.Common
{
    public abstract partial class ActionEventHandlerBase<This>
    {
        #region <Fields>

        protected int _CurrentCueIndex;

        #endregion
        
        #region <Callbacks>

        protected virtual void OnClipCue(AnimationTool.ClipEventType p_Type)
        {
            switch (p_Type)
            {
                case AnimationTool.ClipEventType.EventCue:
                    OnProgressEventCue(_CurrentCueIndex++);
                    break;
            }
        }
        
        protected virtual void OnProgressEventCue(int p_EventCueIndex)
        {
        }
        
        #endregion

        #region <Methods>

        public void HandleClipCue(AnimationTool.ClipEventType p_Type)
        {
            if (IsSelected())
            {
                OnClipCue(p_Type);
            }
        }

        #endregion
    }
}