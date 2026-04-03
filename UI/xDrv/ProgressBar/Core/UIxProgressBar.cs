using UnityEngine;
using UnityEngine.UI;

namespace k514.Mono.Common
{
    public class UIxProgressBar : UIxPanelBase
    {
        #region <Fields>

        private Image _FillAmountImage;
        private Text _CurrentValueText;
        private Text _TotalValueText;

        #endregion
        
        #region <Callbacks>

        protected override void OnCreate(UIPoolManager.CreateParams p_CreateParams)
        {
            base.OnCreate(p_CreateParams);
            
            {
                var (valid, fillAmountImage) = RectTransform.FindRecursive<Image>("FillAmountImage");
                if (valid)
                {
                    _FillAmountImage = fillAmountImage;
                }
            }
            {
                var (valid, currentValueText) = RectTransform.FindRecursive<Text>("CurrentValueText");
                if (valid)
                {
                    _CurrentValueText = currentValueText;
                }
            }
            {
                var (valid, totalValueText) = RectTransform.FindRecursive<Text>("TotalValueText");
                if (valid)
                {
                    _TotalValueText = totalValueText;
                }
            }
        }

        protected override bool OnActivate(UIPoolManager.CreateParams p_CreateParams, UIPoolManager.ActivateParams p_ActivateParams, bool p_IsPooled)
        {
            if (base.OnActivate(p_CreateParams, p_ActivateParams, p_IsPooled))
            {
                ResetBarInfo();
                
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region <Methods>

        public void ResetBarInfo()
        {
            _FillAmountImage.rectTransform.localScale = Vector3.one;
            _CurrentValueText.text = "0";
            _TotalValueText.text = "0";
        }

        public void UpdateBarInfo(float p_Rate, string p_CurrentValueText, string p_TotalValueText)
        {
            _FillAmountImage.rectTransform.localScale = p_Rate * Vector3.right + _FillAmountImage.rectTransform.localScale.YZVector();
            _CurrentValueText.text = p_CurrentValueText;
            _TotalValueText.text = p_TotalValueText;
        }

        #endregion
    }
}