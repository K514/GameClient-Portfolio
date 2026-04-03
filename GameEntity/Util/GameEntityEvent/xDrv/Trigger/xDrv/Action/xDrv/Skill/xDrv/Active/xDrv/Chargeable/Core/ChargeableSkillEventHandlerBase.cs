using UnityEngine;

namespace k514.Mono.Common
{
    /// <summary>
    /// 차징 스킬
    ///
    /// 입력시 모션을 재생하고, 정지한다. 입력 해제 시 혹은 최대 시간 충전 시, 다시 재생한다.
    /// 입력 해제 시 핸들러를 반환된다.
    /// </summary>
    public abstract class ChargeableSkillEventHandlerBase<This> : ActiveSkillEventHandlerBase<This>
        where This : ChargeableSkillEventHandlerBase<This>, new()
    {
        #region <Consts>

        protected static float _MaxChargeDuration;
        protected static float _MaxChargeDurationInv;

        protected static void SetMaxChargeDuration(float p_Duration)
        {
            _MaxChargeDuration = p_Duration;
            _MaxChargeDurationInv = 1f / _MaxChargeDuration;
        }
        
        #endregion
        
        #region <Fields>

        protected float _CurChargeDuration;

        #endregion
        
        #region <Callbacks>

        public override void OnInterrupted()
        {
            base.OnInterrupted();
            
            _CurChargeDuration = 0f;
            Entity.AnimationModule.SetAnimationResume();
        }

        protected override void OnPress(CommandEventParams p_CommandPreset)
        {
            ActionModule.TryInterruptMainHandler(this);
        }

        protected override void OnHolding(CommandEventParams p_CommandPreset)
        {
            if (IsSelected())
            {
                if (_MaxChargeDuration > _CurChargeDuration)
                {
                    _CurChargeDuration += p_CommandPreset.DeltaTime;
                }
                else
                {
                    ManualInputRelease();
                }
            }
            else
            {
                ManualInputRelease();
            }
        }

        protected override void OnRelease(CommandEventParams p_CommandPreset)
        {
            Debug.LogError($"charge rate {GetChargeRate()}");
            Entity.AnimationModule.SetAnimationResume();
        }
        
        protected override void OnClipCue(AnimationTool.ClipEventType p_Type)
        {
            base.OnClipCue(p_Type);
            
            switch (p_Type)
            {
                case AnimationTool.ClipEventType.StopCue:
                {
                    if (IsHolding())
                    {
                        Entity.AnimationModule.SetAnimationPause();
                    }
                    break;
                }
                case AnimationTool.ClipEventType.EndCue:
                {
                    ActionModule.TryReleaseMainHandler(this);
                    break;
                }
            }
        }
        
        #endregion

        #region <Methods>

        public float GetChargeRate() => Mathf.Min(1f, _CurChargeDuration * _MaxChargeDurationInv);

        #endregion
    }
}