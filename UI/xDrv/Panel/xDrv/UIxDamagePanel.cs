using DamageNumbersPro;
using k514.Mono.Common;
using TMPro;
using UnityEngine;

namespace k514.Mono.Feature
{
    public class UIxDamagePanel : UIxPanelBase
    {
        #region <Fields>
        
        protected DamageNumber _DamageNumber;
        private UIxTool.TextMeshProPreset _DefaultMainTextPreset;
        
        #endregion

        #region <Callbacks>
        
        protected override void OnCreate(UIPoolManager.CreateParams p_CreateParams)
        {
            base.OnCreate(p_CreateParams);
            var mainDamageText = Affine.GetComponent<DamageNumber>();
            if (!ReferenceEquals(null, mainDamageText))
            {
                SetStateFlag(UIxTool.UIxStaticStateType.HasMainText);
                _DamageNumber = mainDamageText;

               // _DefaultMainTextPreset = new UIxTool.TextMeshProPreset(mainDamageText.GetTextMeshs()[0] as TextMeshProUGUI);
            }
        }

        protected override bool OnActivate(UIPoolManager.CreateParams p_CreateParams, UIPoolManager.ActivateParams p_ActivateParams,bool p_IsPooled)
        {
            if (base.OnActivate(p_CreateParams, p_ActivateParams, p_IsPooled))
            {
                SetMainTextVisible(true);
                
                return true;
            }
            else
            {
                return false;
            }
        }
        
        protected override void OnRetrieve(UIPoolManager.CreateParams p_CreateParams, bool p_IsPooled, bool p_IsDisposed)
        {
            base.OnRetrieve(p_CreateParams, p_IsPooled, p_IsDisposed);

           // ResetMainImage();
            ResetMainText();
        }
        
         #endregion

        #region <Method/MainText>

        public override void SetMainTextVisible(bool p_Flag)
        {
            if (_UIStaticStateFlagMask.HasAnyFlagExceptNone(UIxTool.UIxStaticStateType.HasMainText))
            {
                _DamageNumber.enabled = p_Flag;
                if (p_Flag)
                {
                    _DamageNumber.Restart();
                }

            }
        }

        public override void SetMainText(string p_Contents)
        {
            if (_UIStaticStateFlagMask.HasAnyFlagExceptNone(UIxTool.UIxStaticStateType.HasMainText))
            {
                _DamageNumber.number = float.Parse(p_Contents);
            }
        }
        public void SetMainText(string p_Contents, string p_HeadSrting)
        {
            if (_UIStaticStateFlagMask.HasAnyFlagExceptNone(UIxTool.UIxStaticStateType.HasMainText))
            {
                _DamageNumber.leftText = p_HeadSrting;
                _DamageNumber.number = string.IsNullOrEmpty(p_Contents) ? 0 : float.Parse(p_Contents);
            }
        }

        public override void SetMainTextColor(Color p_Color)
        {
            if (_UIStaticStateFlagMask.HasAnyFlagExceptNone(UIxTool.UIxStaticStateType.HasMainText))
            {
                _DamageNumber.SetColor(p_Color);
            }
        }

        public override void SetMainTextSize(int p_Size)
        {
            if (_UIStaticStateFlagMask.HasAnyFlagExceptNone(UIxTool.UIxStaticStateType.HasMainText))
            {
                var fonts = _DamageNumber.GetTextMeshs();
                for (int i = 0; i < fonts.Length; i++)
                {
                    fonts[i].fontSize = p_Size;
                }
            }
        }

        public override void ResetMainText()
        {
            if (_UIStaticStateFlagMask.HasAnyFlagExceptNone(UIxTool.UIxStaticStateType.HasMainText))
            {
                foreach(var text in _DamageNumber.GetTextMeshs())
                {
                 //   _DefaultMainTextPreset.ApplyPreset(text as TextMeshProUGUI);
                }
                
            }
        }

        public void UpdateText()
        {
            _DamageNumber.UpdateText();
        }
        
        protected override void UpdateLateText()
        {
            SetMainText(_LateUpdateText);
        }

        protected override void UpdateLateColor()
        {
           // SetMainTextColor(_LateUpdateColor);
        }
        

        public override void ResetMainImage() { }


        #endregion
    }
}