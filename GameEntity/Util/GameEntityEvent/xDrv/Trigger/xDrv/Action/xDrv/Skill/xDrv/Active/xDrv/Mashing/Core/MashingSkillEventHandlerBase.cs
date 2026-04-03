using UnityEngine;

namespace k514.Mono.Common
{
    /// <summary>
    /// 매싱 발동 스킬
    ///
    /// 입력시 모션을 재생하고, 모션 종료시 핸들러를 반환된다.
    /// 트리거 추가 입력 시 해당 횟수가 반영된다.
    /// </summary>
    public abstract class MashingSkillEventHandlerBase<This> : ActiveSkillEventHandlerBase<This>
        where This : MashingSkillEventHandlerBase<This>, new()
    {
        #region <Consts>

        protected static float _MaxMashingCount;

        #endregion
        
        #region <Fields>

        protected float _CurMashingCount;

        #endregion
        
        #region <Callbacks>

        public override void OnInterrupted()
        {
            base.OnInterrupted();
            
            _CurMashingCount = 0f;
        }
        
        protected override void OnPress(CommandEventParams p_CommandPreset)
        {
            ActionModule.TryInterruptMainHandler(this);

            if (IsSelected())
            {
                if (_MaxMashingCount > _CurMashingCount)
                {
                    _CurMashingCount++;
                }
            }
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
                    Debug.LogError($"mashing count {_CurMashingCount}");
                    ActionModule.TryReleaseMainHandler(this);
                    break;
                }
            }
        }
        
        #endregion
    }
}