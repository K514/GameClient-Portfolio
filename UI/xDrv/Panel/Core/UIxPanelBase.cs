using UnityEngine;
using UnityEngine.UI;

#if !SERVER_DRIVE

namespace k514.Mono.Common
{
    /// <summary>
    /// 하나의 메인 이미지와 메인 텍스트 컴포넌트를 다루는 기본 판넬 컴포넌트 클래스
    /// </summary>
    public abstract class UIxPanelBase : UIxElementBase
    {
        #region <Fields>

        private AssetLoadResult<Sprite> _SpritePreset;
        protected Image _MainImage;
        private UIxTool.ImagePreset _DefaultMainImagePreset;
        protected Text _MainText;
        private UIxTool.TextPreset _DefaultMainTextPreset;

        #endregion
        
        #region <Callbacks>

        protected override void OnCreate(UIPoolManager.CreateParams p_CreateParams)
        {
            base.OnCreate(p_CreateParams);

            var (mainImageValid, mainImage) = Affine.FindRecursive<Image>("MainImage");
            if (mainImageValid)
            {
                SetStateFlag(UIxTool.UIxStaticStateType.HasMainImage);
                _MainImage = mainImage;
                _DefaultMainImagePreset = new UIxTool.ImagePreset(_MainImage);
            }

            var (mainTextValid, mainText) = Affine.FindRecursive<Text>("MainText");
            if (mainTextValid)
            {
                SetStateFlag(UIxTool.UIxStaticStateType.HasMainText);
                _MainText = mainText;
                _DefaultMainTextPreset = new UIxTool.TextPreset(_MainText);
            }
        }

        protected override bool OnActivate(UIPoolManager.CreateParams p_CreateParams, UIPoolManager.ActivateParams p_ActivateParams, bool p_IsPooled)
        {
            if (base.OnActivate(p_CreateParams, p_ActivateParams, p_IsPooled))
            {
                SetMainImageVisible(true);
                SetMainTextVisible(true);
                SetTransparent(1f);
                SetHide(true);
                
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
            
            ResetMainImage();
            ResetMainText();
        }
        
        #endregion

        #region <Method/MainImage>

        public void SetMainImageVisible(bool p_Flag)
        {
            if (_UIStaticStateFlagMask.HasAnyFlagExceptNone(UIxTool.UIxStaticStateType.HasMainImage))
            {
                _MainImage.enabled = p_Flag;
            }
        }

        public void SetMainImageSprite(int p_Index)
        {
            if (_UIStaticStateFlagMask.HasAnyFlagExceptNone(UIxTool.UIxStaticStateType.HasMainImage))
            {
                ResetMainImageSprite();

                // TODO<514> : 외부에서 필요에 의해 이미지를 로드하는게 아니라. 오브젝트 풀처럼 처음부터 용도에 따라 이미지단위로 오브젝트를 구분하고 가져다 써야한다.
/*                var assetLoadResult = ImageNameTable.GetInstanceUnsafe.GetResource(p_Index, ResourceLifeCycleType.Free_Condition);
                if (assetLoadResult)
                {
                    _SpritePreset = assetLoadResult;
                    _MainImage.sprite = assetLoadResult.Asset;
                    SetMainImageVisible(true);
                }*/
            }
        }
        
        private void ResetMainImageSprite()
        {
            if (_UIStaticStateFlagMask.HasAnyFlagExceptNone(UIxTool.UIxStaticStateType.HasMainImage))
            {
                // TODO<514> : 외부에서 필요에 의해 이미지를 로드하는게 아니라. 오브젝트 풀처럼 처음부터 용도에 따라 이미지단위로 오브젝트를 구분하고 가져다 써야한다.
                /*if (_SpritePreset)
                         {
                             AssetLoaderManager.GetInstanceUnsafe?.UnloadAsset(ref _SpritePreset);
                         }

                         _SpritePreset = default;
                         _MainImage.sprite = null;
                         SetMainImageVisible(false);*/
            }
        }
        
        public void SetMainImageSize(int p_Size)
        {
            if (_UIStaticStateFlagMask.HasAnyFlagExceptNone(UIxTool.UIxStaticStateType.HasMainImage))
            {
               _MainImage.rectTransform.sizeDelta = new Vector2(p_Size, p_Size);
            }
        }

        public virtual void ResetMainImage()
        {
            if (_UIStaticStateFlagMask.HasAnyFlagExceptNone(UIxTool.UIxStaticStateType.HasMainImage))
            {
                _DefaultMainImagePreset.ApplyPreset(_MainImage);
                ResetMainImageSprite();
            }
        }

        protected override void UpdateLateImage()
        {
            SetMainImageSprite(_LateUpdateImageIndex);
        }
        
        #endregion
        
        #region <Method/MainText>

        public virtual void SetMainTextVisible(bool p_Flag)
        {
            if (_UIStaticStateFlagMask.HasAnyFlagExceptNone(UIxTool.UIxStaticStateType.HasMainText))
            {
                _MainText.enabled = p_Flag;
            }
        }

        public virtual void SetMainText(string p_Contents)
        {
            if (_UIStaticStateFlagMask.HasAnyFlagExceptNone(UIxTool.UIxStaticStateType.HasMainText))
            {
                _MainText.text = p_Contents;
            }
        }

        public virtual void SetMainTextColor(Color p_Color)
        {
            if (_UIStaticStateFlagMask.HasAnyFlagExceptNone(UIxTool.UIxStaticStateType.HasMainText))
            {
                _MainText.color = p_Color;
            }
        }

        public virtual void SetMainTextSize(int p_Size)
        {
            if (_UIStaticStateFlagMask.HasAnyFlagExceptNone(UIxTool.UIxStaticStateType.HasMainText))
            {
                _MainText.fontSize = p_Size;
            }
        }

        public virtual void ResetMainText()
        {
            if (_UIStaticStateFlagMask.HasAnyFlagExceptNone(UIxTool.UIxStaticStateType.HasMainText))
            {
                _DefaultMainTextPreset.ApplyPreset(_MainText);
            }
        }
        
        protected override void UpdateLateText()
        {
            SetMainText(_LateUpdateText);
        }

        protected override void UpdateLateColor()
        {
            SetMainTextColor(_LateUpdateColor);
        }

        #endregion
    }
}
#endif