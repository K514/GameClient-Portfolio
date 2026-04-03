using TMPro;
using UnityEngine;
using UnityEngine.UI;

#if !SERVER_DRIVE
namespace k514.Mono.Common
{
    public partial class UIxTool
    {
        #region <Methods>

        public static void SetTextAlpha(this Text p_TargetText, float p_TargetAlpha)
        {
            p_TargetText.color = p_TargetText.color.SetAlpha(p_TargetAlpha);
        }
        
        public static void SetTextAlpha(this TextMeshProUGUI p_TargetText, float p_TargetAlpha)
        {
            p_TargetText.color = p_TargetText.color.SetAlpha(p_TargetAlpha);
        }
        
        #endregion 
        
        #region <Structs>

        public struct TextPreset
        {
            #region <Fields>

            public Color DefaultColor;
            public string DefaultText;
            public int DefaultFontSize;
            public Vector2 DefaultRectSize;

            #endregion

            #region <Constructor>

            public TextPreset(Text p_Text)
            {
                DefaultColor = p_Text.color;
                DefaultText = p_Text.text;
                DefaultFontSize = p_Text.fontSize;
                DefaultRectSize = p_Text.rectTransform.sizeDelta;
            }

            #endregion
        
            #region <Methods>

            public void ApplyPreset(Text p_TargetText)
            {
                p_TargetText.color = DefaultColor;
                p_TargetText.text = DefaultText;
                p_TargetText.fontSize = DefaultFontSize;
                p_TargetText.rectTransform.sizeDelta = DefaultRectSize;
            }

            #endregion
        }

        public struct TextMeshProPreset
        {
            #region <Fields>

            public Color DefaultColor;
            public string DefaultText;
            public float DefaultFontSize;
            public Vector2 DefaultRectSize;

            #endregion

            #region <Constructor>

            public TextMeshProPreset(TextMeshProUGUI p_Text)
            {
                DefaultColor = p_Text.color;
                DefaultText = p_Text.text;
                DefaultFontSize = p_Text.fontSize;
                DefaultRectSize = p_Text.rectTransform.sizeDelta;
            }

            #endregion
        
            #region <Methods>

            public void ApplyPreset(TextMeshProUGUI p_TargetText)
            {
                p_TargetText.color = DefaultColor;
                p_TargetText.text = DefaultText;
                p_TargetText.fontSize = DefaultFontSize;
                p_TargetText.rectTransform.sizeDelta = DefaultRectSize;
            }

            #endregion
        }
        
        #endregion
    }
}
#endif