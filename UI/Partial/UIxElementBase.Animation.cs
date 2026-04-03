#if !SERVER_DRIVE
using System;
using UnityEngine;

namespace k514.Mono.Common
{
    public abstract partial class UIxElementBase
    {
        #region <Fields>

        /// <summary>
        /// 페이드 연출 프리셋
        /// </summary>
        private SwitchIterator _FadeEventTimer;

        /// <summary>
        /// 페이드 인 종료 이벤트 핸들러
        /// </summary>
        private Action FadeInOver;

        /// <summary>
        /// 페이드 아웃 종료 이벤트 핸들러
        /// </summary>
        private Action FadeOutOver;

        /// <summary>
        /// 애니메이션 컴포넌트
        /// </summary>
        private Animation _Animation;

        /// <summary>
        /// 기본 애니메이션 클립
        /// </summary>
        private AnimationClip _DefaultAnimationClip;

        #endregion

        #region <Callbacks>

        private void OnCreateAnimation()
        {
            _Animation = GetComponent<Animation>();
            
            if (null != _Animation)
            {
                _DefaultAnimationClip = _Animation.clip;
                _Animation.playAutomatically = false;
                _Animation.animatePhysics = false;
                _Animation.cullingType = AnimationCullingType.AlwaysAnimate;
                
                SetStateFlag(UIxTool.UIxStaticStateType.HasAnimation);
            }
        }

        private void OnRetrieveAnimation()
        {
            _FadeEventTimer = default;
            FadeInOver = default;
            FadeOutOver = default;
            
            ResetAnimationClip();
        }
        
        private void OnUpdateAnimation(float p_DeltaTime)
        {
            var fadeEvent = _FadeEventTimer.OnUpdatePhase(p_DeltaTime);
            switch (fadeEvent)
            {
                case IteratorTool.LevelPhaseEvent.DynamicPositive:
                    SetOpaque(_FadeEventTimer.GetDynamicPositiveProgress());
                    break;
                case IteratorTool.LevelPhaseEvent.DynamicPositiveOver:
                    OnFadeInOver(!_FadeEventTimer.HasAutoDynamicNegativeFlag());
                    break;
                case IteratorTool.LevelPhaseEvent.DynamicNegative:
                    SetTransparent(_FadeEventTimer.GetDynamicNegativeProgress());
                    break;
                case IteratorTool.LevelPhaseEvent.DynamicNegativeOver:
                    OnFadeOutOver();
                    break;
            }
        }

        private void OnFadeInOver(bool p_ApplyAfterFadeFlag)
        {
            SetOpaque(1f);
            
            FadeInOver?.Invoke();
            FadeInOver = null;

            if (p_ApplyAfterFadeFlag)
            {
                if (_UIDynamicStateFlagMask.HasAnyFlagExceptNone(UIxTool.UIxDynamicStateType.DeferredRetrieve))
                {
                    _UIDynamicStateFlagMask.RemoveFlag(UIxTool.UIxDynamicStateType.DeferredRetrieve);
                    Pooling();
                }
                else if(_UIDynamicStateFlagMask.HasAnyFlagExceptNone(UIxTool.UIxDynamicStateType.DeferredHide))
                {
                    _UIDynamicStateFlagMask.RemoveFlag(UIxTool.UIxDynamicStateType.DeferredHide);
                    SetHide(true);
                }
            }
        }
        
        private void OnFadeOutOver()
        {
            SetTransparent(1f);
                        
            FadeOutOver?.Invoke();
            FadeOutOver = null;
                        
            if (_UIDynamicStateFlagMask.HasAnyFlagExceptNone(UIxTool.UIxDynamicStateType.DeferredRetrieve))
            {
                _UIDynamicStateFlagMask.RemoveFlag(UIxTool.UIxDynamicStateType.DeferredRetrieve);
                Pooling();
            }
            else if(_UIDynamicStateFlagMask.HasAnyFlagExceptNone(UIxTool.UIxDynamicStateType.DeferredHide))
            {
                _UIDynamicStateFlagMask.RemoveFlag(UIxTool.UIxDynamicStateType.DeferredHide);
                SetHide(true);
            }
        }
        
        #endregion

        #region <Method/Animation>

        public void SetAnimationClip(int p_ClipIndex)
        {
            if (_UIStaticStateFlagMask.HasAnyFlagExceptNone(UIxTool.UIxStaticStateType.HasAnimation))
            {
                _Animation.clip = AnimationClipManager.GetInstanceUnsafe?[p_ClipIndex];
            }
        }
        
        public void ResetAnimationClip()
        {
            if (_UIStaticStateFlagMask.HasAnyFlagExceptNone(UIxTool.UIxStaticStateType.HasAnimation))
            {
                _Animation.clip = _DefaultAnimationClip;
            }
        }

        private void SetPlayAnimation()
        {
            if (_UIStaticStateFlagMask.HasAnyFlagExceptNone(UIxTool.UIxStaticStateType.HasAnimation))
            {
                _Animation.Rewind();
                _Animation.Play();
            }
        }
        
        #endregion

        #region <Method/Fade>

        /// <summary>
        /// 페이드 인/아웃 연출의 지속시간을 설정하는 메서드
        /// </summary>
        public void SetFadeDuration(float p_FadeIn, float p_Display, float p_FadeOut)
        {
            _FadeEventTimer.SetPhaseDuration(p_FadeIn, p_FadeOut);
            _FadeEventTimer.SetStaticPositiveAutoReleaseDuration(p_Display);
        }

        /// <summary>
        /// 페이드 연출 진행도를 초기화시키는 메서드
        /// </summary>
        public void SetFadeRewind()
        {
            _FadeEventTimer.ResetPhase();
        }

        /// <summary>
        /// 즉시 화면을 가리는 메서드
        /// </summary>
        public void SetInstantFadeIn()
        {
            SetHide(false);
            _FadeEventTimer.SetPhase(IteratorTool.LevelPhaseType.StaticPositive);
            SetOpaque(1f);
        }
        
        /// <summary>
        /// 즉시 화면을 보이는 메서드
        /// </summary>
        public void SetInstantFadeOut()
        {
            SetHide(false);
            _FadeEventTimer.SetPhase(IteratorTool.LevelPhaseType.StaticNegative);
            SetTransparent(1f);
        }
        
        /// <summary>
        /// 해당 UI컴포넌트가 보유한 이미지의 투명도를 서서히 불투명하게 하는 메서드(N = 0 → P = 1)
        /// </summary>
        public void SetFadeIn(UIxTool.UIAfterFadeType p_Type, bool p_RewindFlag, Action p_FadeInOver = null)
        {
            switch (p_Type)
            {
                case UIxTool.UIAfterFadeType.None:
                    _UIDynamicStateFlagMask.RemoveFlag(UIxTool.UIxDynamicStateType.DeferredHide | UIxTool.UIxDynamicStateType.DeferredRetrieve);
                    break;
                case UIxTool.UIAfterFadeType.Hide:
                    _UIDynamicStateFlagMask.AddFlag(UIxTool.UIxDynamicStateType.DeferredHide);
                    break;
                case UIxTool.UIAfterFadeType.Retrieve:
                    _UIDynamicStateFlagMask.AddFlag(UIxTool.UIxDynamicStateType.DeferredRetrieve);
                    break;
            }
            
            SetHide(false);
            if (p_RewindFlag)
            {
                SetFadeRewind();
            }
            _FadeEventTimer.SetStaticNegativeAutoReleaseFlag(false);
            FadeInOver = p_FadeInOver;

            var fadeEvent = _FadeEventTimer.UpdatePositivePhase();
            switch (fadeEvent)
            {
                case IteratorTool.LevelPhaseEvent.StaticPositive:
                    OnFadeInOver(true);
                    break;
                case IteratorTool.LevelPhaseEvent.DynamicPositiveStart:
                    SetOpaque(0f);
                    break;
                case IteratorTool.LevelPhaseEvent.DynamicPositive:
                    SetOpaque(_FadeEventTimer.GetDynamicPositiveProgress());
                    break;
            }
        }

        /// <summary>
        /// 해당 UI컴포넌트가 보유한 이미지의 투명도를 서서히 투명하게 하는 메서드(P = 1 → N = 0)
        /// </summary>
        public void SetFadeOut(UIxTool.UIAfterFadeType p_Type, bool p_RewindFlag, Action p_FadeOutOver = null)
        {
            switch (p_Type)
            {
                case UIxTool.UIAfterFadeType.None:
                    _UIDynamicStateFlagMask.RemoveFlag(UIxTool.UIxDynamicStateType.DeferredHide | UIxTool.UIxDynamicStateType.DeferredRetrieve);
                    break;
                case UIxTool.UIAfterFadeType.Hide:
                    _UIDynamicStateFlagMask.AddFlag(UIxTool.UIxDynamicStateType.DeferredHide);
                    break;
                case UIxTool.UIAfterFadeType.Retrieve:
                    _UIDynamicStateFlagMask.AddFlag(UIxTool.UIxDynamicStateType.DeferredRetrieve);
                    break;
            }
                  
            SetHide(false);
            if (p_RewindFlag)
            {
                SetFadeRewind();
            }
            _FadeEventTimer.SetStaticPositiveAutoReleaseFlag(false);
            FadeOutOver = p_FadeOutOver;

            var fadeEvent = _FadeEventTimer.UpdateNegativePhase();
            switch (fadeEvent)
            {
                case IteratorTool.LevelPhaseEvent.StaticNegative:
                    OnFadeOutOver();
                    break;
                case IteratorTool.LevelPhaseEvent.DynamicNegativeStart:
                    SetTransparent(0f);
                    break;
                case IteratorTool.LevelPhaseEvent.DynamicNegative:
                    SetTransparent(_FadeEventTimer.GetDynamicNegativeProgress());
                    break;
            }
        }

        /// <summary>
        /// 해당 UI컴포넌트가 보유한 이미지의 알파도를 서서히 불투명하게 드러냈다 일정시간 대기후 투명하게 하는 메서드(N = 0 → P = 1 → N = 0)
        /// </summary>
        public void SetFadeInOut(UIxTool.UIAfterFadeType p_Type, bool p_RewindFlag, Action p_FadeInOver = null, Action p_FadeOutOver = null)
        {
            switch (p_Type)
            {
                case UIxTool.UIAfterFadeType.None:
                    _UIDynamicStateFlagMask.RemoveFlag(UIxTool.UIxDynamicStateType.DeferredHide | UIxTool.UIxDynamicStateType.DeferredRetrieve);
                    break;
                case UIxTool.UIAfterFadeType.Hide:
                    _UIDynamicStateFlagMask.AddFlag(UIxTool.UIxDynamicStateType.DeferredHide);
                    break;
                case UIxTool.UIAfterFadeType.Retrieve:
                    _UIDynamicStateFlagMask.AddFlag(UIxTool.UIxDynamicStateType.DeferredRetrieve);
                    break;
            }
            
            SetHide(false);
            if (p_RewindFlag)
            {
                SetFadeRewind();
            }
            _FadeEventTimer.SetStaticPositiveAutoReleaseFlag(true);
            FadeInOver = p_FadeInOver;
            FadeOutOver = p_FadeOutOver;

            var fadeEvent = _FadeEventTimer.UpdatePositivePhase();
            switch (fadeEvent)
            {
                case IteratorTool.LevelPhaseEvent.StaticPositive:
                    OnFadeInOver(false);
                    break;
                case IteratorTool.LevelPhaseEvent.DynamicPositiveStart:
                    SetOpaque(0f);
                    break;
                case IteratorTool.LevelPhaseEvent.DynamicPositive:
                    SetOpaque(_FadeEventTimer.GetDynamicPositiveProgress());
                    break;
            }
        }

        #endregion
    }
}
#endif