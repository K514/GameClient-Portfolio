#if !SERVER_DRIVE

using UnityEngine;
using UnityEngine.UI;

namespace k514.Mono.Common
{
    public class UIxTraceGauge : UIxGauge
    {
        #region <Consts>

        private const float _TraceBarAnimationDuration = 0.5f;

        #endregion
        
        #region <Fields>

        private Image _TraceBar;
        private float _TraceBarTargetRate;
        private bool _TraceBarAnimationFlag;
        private ProgressTimer _TraceBarMoveAnimationTimer;

        #endregion
        
        #region <Callbacks>

        protected override void OnCreate(UIPoolManager.CreateParams p_CreateParams)
        {
            base.OnCreate(p_CreateParams);

            var (valid, bar) = RectTransform.FindRecursive("TraceBar");
            if (valid)
            {
                _TraceBar = bar.GetComponent<Image>();
                _TraceBar.type = Image.Type.Filled;
                _TraceBar.fillMethod = Image.FillMethod.Horizontal;
                _TraceBar.fillOrigin = 0;
            }
            
            _TraceBarMoveAnimationTimer = _TraceBarAnimationDuration;
            _TraceBarTargetRate = 1f;
            ResetTraceBarAnimation();
        }
        
        protected override void OnRetrieve(UIPoolManager.CreateParams p_CreateParams, bool p_IsPooled, bool p_IsDisposed)
        {
            base.OnRetrieve(p_CreateParams, p_IsPooled, p_IsDisposed);
            
            ResetTraceBarAnimation();
        }

        protected override void OnUpdateUIDeferredEvent(float p_DeltaTime)
        {
            base.OnUpdateUIDeferredEvent(p_DeltaTime);

            if (_TraceBarAnimationFlag)
            {
                if (_TraceBarMoveAnimationTimer.IsOver())
                {
                    ResetTraceBarAnimation();
                }
                else
                {
                    _TraceBarMoveAnimationTimer.Progress(p_DeltaTime);
                    _TraceBar.fillAmount = Mathf.Lerp(_TraceBar.fillAmount, _TraceBarTargetRate, _TraceBarMoveAnimationTimer.ProgressRate);
                }
            }
        }
        
        #endregion
        
        #region <Methods>
        
        public override void SetValue(float p_Rate)
        {
            base.SetValue(p_Rate);
            
            if (_TraceBarTargetRate < _TraceBar.fillAmount)
            {
                _TraceBarTargetRate = p_Rate;
                StartTraceBarAnimation();
            }
        }
        
        private void StartTraceBarAnimation()
        {
            _TraceBarAnimationFlag = true;
            _TraceBarMoveAnimationTimer.Reset();
        }
        
        private void ResetTraceBarAnimation()
        {
            _TraceBarAnimationFlag = false;
            _TraceBar.fillAmount = _TraceBarTargetRate;
        }

        #endregion
    }
}

#endif