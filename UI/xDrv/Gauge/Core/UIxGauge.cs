#if !SERVER_DRIVE

using UnityEngine;
using UnityEngine.UI;

namespace k514.Mono.Common
{
    public class UIxGauge : UIxContainerBase
    {
        #region <Consts>

        private const float _MainBarAnimationDuration = 0.3f;

        #endregion
        
        #region <Fields>

        protected Image _MainBar;
        protected float _MainBarTargetRate;
        private bool _MainBarAnimationFlag;
        private ProgressTimer _MainBarMoveAnimationTimer;

        #endregion
        
        #region <Callbacks>

        protected override void OnCreate(UIPoolManager.CreateParams p_CreateParams)
        {
            base.OnCreate(p_CreateParams);

            var (valid, bar) = RectTransform.FindRecursive("MainBar");
            if (valid)
            {
                _MainBar = bar.GetComponent<Image>();
                _MainBar.type = Image.Type.Filled;
                _MainBar.fillMethod = Image.FillMethod.Horizontal;
                _MainBar.fillOrigin = 0;
            }

            _MainBarMoveAnimationTimer = _MainBarAnimationDuration;
            _MainBarTargetRate = 1f;
            ResetMainBarAnimation();
        }
        
        protected override void OnRetrieve(UIPoolManager.CreateParams p_CreateParams, bool p_IsPooled, bool p_IsDisposed)
        {
            base.OnRetrieve(p_CreateParams, p_IsPooled, p_IsDisposed);
            
            ResetMainBarAnimation();
        }
        
        protected override void OnUpdateUIDeferredEvent(float p_DeltaTime)
        {
            base.OnUpdateUIDeferredEvent(p_DeltaTime);

            if (_MainBarAnimationFlag)
            {
                if (_MainBarMoveAnimationTimer.IsOver())
                {
                    ResetMainBarAnimation();
                }
                else
                {
                    _MainBarMoveAnimationTimer.Progress(p_DeltaTime);
                    _MainBar.fillAmount = Mathf.Lerp(_MainBar.fillAmount, _MainBarTargetRate, _MainBarMoveAnimationTimer.ProgressRate);
                }
            }
        }
        
        #endregion

        #region <Methods>
        
        public virtual void SetValue(float p_Rate)
        {
            _MainBarTargetRate = Mathf.Clamp01(p_Rate);
            StartMainBarAnimation();
        }
        
        private void StartMainBarAnimation()
        {
            _MainBarAnimationFlag = true;
            _MainBarMoveAnimationTimer.Reset();
        }
        
        private void ResetMainBarAnimation()
        {
            _MainBarAnimationFlag = false;
            _MainBar.fillAmount = _MainBarTargetRate;
        }

        #endregion
    }
}

#endif