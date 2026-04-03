#if !SERVER_DRIVE

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace k514.Mono.Common
{
    public partial class UIxElementBase
    {
        #region <Fields>

        /// <summary>
        /// 해당 컴포넌트가 보유한 이미지 리스트
        /// </summary>
        private List<Image> _ImageGroup;

        /// <summary>
        /// 해당 컴포넌트가 보유한 이미지 원본 데이터 리스트
        /// </summary>
        private List<UIxTool.ImagePreset> _DefaultmagePreset;

        /// <summary>
        /// 해당 컴포넌트가 보유한 이미지 숫자
        /// </summary>
        private int _ImageCount;

        /// <summary>
        /// 해당 컴포넌트가 보유한 텍스트 리스트
        /// </summary>
        private List<Text> _LegacyTextGroup;

        /// <summary>
        /// 해당 컴포넌트가 보유한 텍스트 원본 데이터 리스트
        /// </summary>
        private List<UIxTool.TextPreset> _DefaultTextPreset;

        /// <summary>
        /// 해당 컴포넌트가 보유한 텍스트 숫자
        /// </summary>
        private int _TextCount;
        
        /// <summary>
        /// 해당 컴포넌트가 보유한 텍스트 리스트
        /// </summary>
        private List<TextMeshProUGUI> _TextGroup;

        /// <summary>
        /// 해당 컴포넌트가 보유한 텍스트 원본 데이터 리스트
        /// </summary>
        private List<UIxTool.TextMeshProPreset> _DefaultTextMeshProPreset;

        /// <summary>
        /// 해당 컴포넌트가 보유한 텍스트 숫자
        /// </summary>
        private int _TextMeshProCount;
        
        #endregion

        #region <Callbacks>

        private void OnCreateGraphics()
        {
            _ImageGroup = new List<Image>();
            Affine.GetComponentsInChildren(_ImageGroup);
            _ImageCount = _ImageGroup.Count;
            _DefaultmagePreset = new List<UIxTool.ImagePreset>();
            foreach (var image in _ImageGroup)
            {
                _DefaultmagePreset.Add(new UIxTool.ImagePreset(image));
            }
            
            _LegacyTextGroup = new List<Text>();
            Affine.GetComponentsInChildren(_LegacyTextGroup);
            _TextCount = _LegacyTextGroup.Count;
            _DefaultTextPreset = new List<UIxTool.TextPreset>();
            foreach (var text in _LegacyTextGroup)
            {
                _DefaultTextPreset.Add(new UIxTool.TextPreset(text));
            }
                        
            _TextGroup = new List<TextMeshProUGUI>();
            Affine.GetComponentsInChildren(_TextGroup);
            _TextMeshProCount = _TextGroup.Count;
            _DefaultTextMeshProPreset = new List<UIxTool.TextMeshProPreset>();
            foreach (var tmp in _TextGroup)
            {
                _DefaultTextMeshProPreset.Add(new UIxTool.TextMeshProPreset(tmp));
            }
        }

        private void OnRetrieveGraphics()
        {
            ResetGraphics();
        }
        
        protected void OnDisposeGraphics()
        {
            _ImageGroup?.Clear();
            _ImageGroup = null;
            _DefaultmagePreset?.Clear();
            _DefaultmagePreset = null;
            
            _LegacyTextGroup?.Clear();
            _LegacyTextGroup = null;
            _DefaultTextPreset?.Clear();
            _DefaultTextPreset = null;
            
            _TextGroup?.Clear();
            _TextGroup = null;
            _DefaultTextMeshProPreset?.Clear();
            _DefaultTextMeshProPreset = null;
        }
        
        #endregion

        #region <Methods>

        public List<Image> GetImageGroup() => _ImageGroup;
        public List<Text> GetLegacyTextGroup() => _LegacyTextGroup;
        public List<TextMeshProUGUI> GetTextGroup() => _TextGroup;
        
        protected virtual void SetOpaque(float p_ProgressRate01)
        {
            SetOpaqueImage(p_ProgressRate01);
            SetOpaqueText(p_ProgressRate01);
        }
        
        protected virtual void SetTransparent(float p_ProgressRate01)
        {
            SetTransparentImage(p_ProgressRate01);
            SetTransparentText(p_ProgressRate01);
        }
        
        protected void SetColor(Color p_Color)
        {
            SetImageColor(p_Color);
            SetTextColor(p_Color);
        }
        
        protected void ResetGraphics()
        {
            ResetImage();
            ResetText();
        }
        
        public void SetOpaqueImage(float p_ProgressRate01)
        {
            for (int i = 0; i < _ImageCount; i++)
            {
                var tryImage = _ImageGroup[i];
                var defaultPreset = _DefaultmagePreset[i];
                tryImage.SetImageAlpha(defaultPreset.DefaultColor.a * p_ProgressRate01);
            }
        }
        
        public void SetTransparentImage(float p_ProgressRate01)
        {
            SetOpaqueImage(1f - p_ProgressRate01);
        }

        public void SetImageColor(Color p_Color)
        {
            foreach (var image in _ImageGroup)
            {
                image.color = p_Color;
            }
        }
        
        public void ResetImage()
        {
            for (int i = 0; i < _ImageCount; i++)
            {
                var tryImage = _ImageGroup[i];
                var defaultPreset = _DefaultmagePreset[i];
                defaultPreset.ApplyPreset(tryImage);
            }
        }
        
        protected void SetOpaqueText(float p_ProgressRate01)
        {
            for (int i = 0; i < _TextCount; i++)
            {
                var textImage = _LegacyTextGroup[i];
                var defaultPreset = _DefaultTextPreset[i];
                textImage.SetTextAlpha(defaultPreset.DefaultColor.a * p_ProgressRate01);
            }
            
            for (int i = 0; i < _TextMeshProCount; i++)
            {
                var textImage = _TextGroup[i];
                var defaultPreset = _DefaultTextMeshProPreset[i];
                textImage.SetTextAlpha(defaultPreset.DefaultColor.a * p_ProgressRate01);
            }
        }
        
        protected void SetTransparentText(float p_ProgressRate01)
        {
            SetOpaqueText(1f - p_ProgressRate01);
        }
              
        public void SetTextColor(Color p_Color)
        {
            foreach (var text in _LegacyTextGroup)
            {
                text.color = p_Color;
            }
            
            foreach (var text in _TextGroup)
            {
                text.color = p_Color;
            }
        }
        
        protected void ResetText()
        {
            for (int i = 0; i < _TextCount; i++)
            {
                var tryText = _LegacyTextGroup[i];
                var defaultPreset = _DefaultTextPreset[i];
                defaultPreset.ApplyPreset(tryText);
            }
            
            for (int i = 0; i < _TextMeshProCount; i++)
            {
                var tryText = _TextGroup[i];
                var defaultPreset = _DefaultTextMeshProPreset[i];
                defaultPreset.ApplyPreset(tryText);
            }
        }

        #endregion
    }
}
#endif