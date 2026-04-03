#if !SERVER_DRIVE
using System;
using UnityEngine;

namespace k514.Mono.Common
{
    public partial class UIxElementBase
    {
        #region <Fields>

        /// <summary>
        /// 지연처리되어 적용될 위치
        /// </summary>
        protected Vector3 _LateUpdatePosition;
        
        /// <summary>
        /// 지연처리되어 적용될 텍스트
        /// </summary>
        protected string _LateUpdateText;

        /// <summary>
        /// 지연처리되어 적용될 텍스트 색상
        /// </summary>
        protected Color _LateUpdateColor;

        /// <summary>
        /// 지연처리되어 적용될 심볼 이미지 인덱스
        /// </summary>
        protected int _LateUpdateImageIndex;

        /// <summary>
        /// 발생한 유닛 이벤트 중에 지연처리해야할 이벤트 플래그마스크
        /// </summary>
        [NonSerialized] public UIxTool.UIxLateEventType LateEventMask;

        #endregion

        #region <Callbacks>
        
        protected override void OnCreateUpdate()
        {
        }

        protected override void OnActivateUpdate()
        {
        }
        
        protected override void OnRetrieveUpdate()
        {
            ResetLateUpdatePosition();
            ResetLateUpdateText();
            ResetLateUpdateColor();
            ResetLateUpdateImage();
            
            LateEventMask = UIxTool.UIxLateEventType.None;
            _FadeEventTimer.ResetPhase();
        }
        
        public sealed override void OnLateUpdate(float p_DeltaTime)
        {
            if (OnUpdateUILateEvent(p_DeltaTime))
            {
                OnUpdateUIDeferredEvent(p_DeltaTime);
            }
        }

        protected virtual void OnUpdateUIDeferredEvent(float p_DeltaTime)
        {
            OnUpdatePivotPosition();
            OnUpdateAnimation(p_DeltaTime);
            OnUpdateInputEvent(p_DeltaTime);
        }
        
        private bool OnUpdateUILateEvent(float p_DeltaTime)
        {
            var enumerator = UIxTool._UILateEventTypeEnumerator;
            var nextLateEventMask = UIxTool.UIxLateEventType.None;
            
            foreach (var lateEventType in enumerator)
            {
                if (LateEventMask.HasAnyFlagExceptNone(lateEventType))
                {
                    switch (lateEventType)
                    {
                        case UIxTool.UIxLateEventType.Retrieve:
                            if (LateEventMask.HasAnyFlagExceptNone(UIxTool.UIxLateEventType.TurnFadeOut))
                            {
                                _UIDynamicStateFlagMask.AddFlag(UIxTool.UIxDynamicStateType.DeferredRetrieve);
                            }
                            else
                            {
                                if (!_UIDynamicStateFlagMask.HasAnyFlagExceptNone(UIxTool.UIxDynamicStateType.DeferredRetrieve))
                                {
                                    Pooling();
                                    return false;
                                }
                            }
                            break;
                        case UIxTool.UIxLateEventType.TurnVisible:
                            if (!LateEventMask.HasAnyFlagExceptNone(UIxTool.UIxLateEventType.TurnHide))
                            {
                                SetHide(false);
                            }
                            break;
                        case UIxTool.UIxLateEventType.TurnHide:
                            SetHide(true);
                            break;
                        case UIxTool.UIxLateEventType.TurnFadeIn:
                            SetFadeIn(UIxTool.UIAfterFadeType.None, true);
                            break;
                        case UIxTool.UIxLateEventType.TurnFadeOut:
                            SetFadeOut(UIxTool.UIAfterFadeType.Hide, false);
                            break;
                        case UIxTool.UIxLateEventType.LateUpdatePosition:
                            UpdateLatePosition();
                            break;
                        case UIxTool.UIxLateEventType.LateUpdateText:
                            UpdateLateText();
                            break;
                        case UIxTool.UIxLateEventType.LateUpdateColor:
                            UpdateLateColor();
                            break;
                        case UIxTool.UIxLateEventType.LateUpdateImage:
                            UpdateLateImage();
                            break;
                        case UIxTool.UIxLateEventType.LateUpdateRate:
                            if (!UpdateRate(p_DeltaTime))
                            {
                                nextLateEventMask.AddFlag(UIxTool.UIxLateEventType.LateUpdateRate);
                            }
                            break;
                    }
                }
            }

            LateEventMask = nextLateEventMask;
            return true;
        }

        #endregion

        #region <Methods>

        public bool HasAnyLateEvent()
        {
            return LateEventMask != UIxTool.UIxLateEventType.None;
        }

        public void SetLateUpdateRate()
        {
            LateEventMask.AddFlag(UIxTool.UIxLateEventType.LateUpdateRate);
        }
        
        public void SetLateUpdatePosition(Vector3 p_Position)
        {
            LateEventMask.AddFlag(UIxTool.UIxLateEventType.LateUpdatePosition);
            _LateUpdatePosition = p_Position;
        }
        
        protected void ResetLateUpdatePosition()
        {
            LateEventMask.RemoveFlag(UIxTool.UIxLateEventType.LateUpdatePosition);
            _LateUpdatePosition = default;
        }
        
        public void SetLateUpdateText(string p_Text)
        {
            LateEventMask.AddFlag(UIxTool.UIxLateEventType.LateUpdateText);
            _LateUpdateText = p_Text;
        }
        
        protected void ResetLateUpdateText()
        {
            LateEventMask.RemoveFlag(UIxTool.UIxLateEventType.LateUpdateText);
            _LateUpdateText = default;
        }
        
        public void SetLateUpdateColor(Color p_Color)
        {
            LateEventMask.AddFlag(UIxTool.UIxLateEventType.LateUpdateColor);
            _LateUpdateColor = p_Color;
        }
        
        protected void ResetLateUpdateColor()
        {
            LateEventMask.RemoveFlag(UIxTool.UIxLateEventType.LateUpdateColor);
            _LateUpdateColor = default;
        }
        
        public void SetLateUpdateImage(int p_Index)
        {
            LateEventMask.AddFlag(UIxTool.UIxLateEventType.LateUpdateImage);
            _LateUpdateImageIndex = p_Index;
        }
        
        protected void ResetLateUpdateImage()
        {
            LateEventMask.RemoveFlag(UIxTool.UIxLateEventType.LateUpdateImage);
            _LateUpdateImageIndex = default;
        }

        protected virtual void UpdateLatePosition()
        {
            SetPosition(_LateUpdatePosition);
        }

        protected virtual void UpdateLateText()
        {
        }

        protected virtual void UpdateLateColor()
        {
        }

        protected virtual void UpdateLateImage()
        {
        }

        protected virtual bool UpdateRate(float p_DeltaTime)
        {
            return true;
        }

        #endregion
    }
}
#endif