namespace k514.Mono.Common
{
    /// <summary>
    /// 단순 발동 스킬
    ///
    /// 입력시 모션을 재생하고, 모션 종료시 핸들러를 반환된다.
    /// </summary>
    public abstract class DefaultSkillEventHandlerBase<This> : ActiveSkillEventHandlerBase<This>
        where This : DefaultSkillEventHandlerBase<This>, new()
    {
        #region <Callbacks>

        protected override void OnPress(CommandEventParams p_CommandPreset)
        {
            ActionModule.TryInterruptMainHandler(this);
        }

        protected override void OnHolding(CommandEventParams p_CommandPreset)
        {
        }

        protected override void OnRelease(CommandEventParams p_CommandPreset)
        {
        }
        
        protected override void OnClipCue(AnimationTool.ClipEventType p_Type)
        {
            base.OnClipCue(p_Type);
            
            switch (p_Type)
            {
                case AnimationTool.ClipEventType.EndCue:
                {
                    ActionModule.TryReleaseMainHandler(this);
                    break;
                }
            }
        }
        
        #endregion
    }
}