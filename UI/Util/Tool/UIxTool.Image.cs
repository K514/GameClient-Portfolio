#if !SERVER_DRIVE

using UnityEngine;
using UnityEngine.UI;

namespace k514.Mono.Common
{
    public partial class UIxTool
    {
        #region <Methods>
        
        public static void SetImageAlpha(this Image p_TargetImage, float p_TargetAlpha)
        {
            p_TargetImage.color = p_TargetImage.color.SetAlpha(p_TargetAlpha);
        }

        #endregion
        
        #region <Structs>

        public struct ImagePreset
        {
            #region <Fields>

            public Color DefaultColor;
            public Sprite DefaultSprite;
            public Vector2 DefaultRectSize;
        
            #endregion

            #region <Constructor>

            public ImagePreset(Image p_Image)
            {
                DefaultColor = p_Image.color;
                DefaultSprite = p_Image.sprite;
                DefaultRectSize = p_Image.rectTransform.sizeDelta;
            }

            #endregion

            #region <Methods>

            public void ApplyPreset(Image p_TargetImage)
            {
                p_TargetImage.color = DefaultColor;
                p_TargetImage.sprite = DefaultSprite;
                p_TargetImage.rectTransform.sizeDelta = DefaultRectSize;
            }

            #endregion
        }

        #endregion 
    }
}
#endif