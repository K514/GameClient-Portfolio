using UnityEngine;

namespace k514.Mono.Common
{
    /// <summary>
    /// 추가 입력 스킬
    ///
    /// 입력시 모션을 재생하고, 모션 종료시 핸들러를 반환된다.
    /// 추가 입력을 통해 추가 기능을 호출한다.
    /// </summary>
    public abstract class ExtraTriggerSkillEventHandlerBase<This> : ActiveSkillEventHandlerBase<This>
        where This : ExtraTriggerSkillEventHandlerBase<This>, new()
    {
        #region <Consts>

        protected static float _ExtraMainTriggerTimeStampValidDuration;
        protected static float _ExtraJumpTriggerTimeStampValidDuration;

        #endregion
        
        #region <Fields>

        private float _ExtraMainTriggerTimeStamp;
        private float _ExtraJumpTriggerTimeStamp;
        private int _CurrentSequenceIndex;

        #endregion
        
        #region <Callbacks>
    
        public override void OnInterruptSuccess()
        {
            base.OnInterruptSuccess();

            OnProgressSequence(_CurrentSequenceIndex = 0);
        }
        
        public override void OnInterrupted()
        {
            base.OnInterrupted();

            _ExtraMainTriggerTimeStamp = 0f;
            _ExtraJumpTriggerTimeStamp = 0f;
        }
        
        protected override void OnPress(CommandEventParams p_CommandPreset)
        {
            if (IsSelected())
            {
                var trigger = p_CommandPreset.GetTriggerKey();
                switch (trigger)
                {
                    default:
                        break;
                    case var _ when trigger == TriggerKey:
                    case var _ when trigger == Record.SequenceCommand.TriggerKey:
                    {
                        UpdateExtraMainTrigger(p_CommandPreset.EventTimeStamp);
                        break;
                    }
                    case var _ when trigger == ActionTool.__JUMP_TRIGGER:
                    {
                        UpdateExtraJumpTrigger(p_CommandPreset.EventTimeStamp);
                        break;
                    }
                }
            }
            else
            {
                ActionModule.TryInterruptMainHandler(this);
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
                    if (IsExtraMainTriggerValid())
                    {
                        OnProgressSequence(++_CurrentSequenceIndex);
                    }
                    else
                    {
                        ActionModule.TryReleaseMainHandler(this);
                    }
                    break;
                }
            }
        }

        protected abstract void OnProgressSequence(int p_SequenceIndex);
        protected abstract void OnPressExtraMainTrigger();
        protected abstract void OnPressExtraJumpTrigger();
            
        #endregion

        #region <Methods>
        
        protected bool IsExtraMainTriggerValid()
        {
            return Time.time - _ExtraMainTriggerTimeStamp < _ExtraMainTriggerTimeStampValidDuration;
        }
        
        protected bool IsExtraJumpTriggerValid()
        {
            return Time.time - _ExtraJumpTriggerTimeStamp < _ExtraJumpTriggerTimeStampValidDuration;
        }
        
        private void UpdateExtraMainTrigger(float p_TimeStamp)
        {
            _ExtraMainTriggerTimeStamp = p_TimeStamp;

            OnPressExtraMainTrigger();
        }
        
        private void UpdateExtraJumpTrigger(float p_TimeStamp)
        {
            _ExtraJumpTriggerTimeStamp = p_TimeStamp;

            OnPressExtraJumpTrigger();
        }
        
        #endregion
    }
}