#if !SERVER_DRIVE

using UnityEngine;

namespace k514.Mono.Common
{
    public partial class UIxObjectRoot
    {
        #region <Callbacks>

        public void OnRootReleased(UIxTool.UIxRootPreset p_RootPreset)
        {
            if (_RootPresetGroup != null)
            {
                var tryRenderMode = p_RootPreset.RenderMode;
                _RootPresetGroup.Remove(tryRenderMode);
            }
        }

        #endregion
        
        #region <Methods>

        public UIxTool.UIxRootPreset AddRoot(RenderMode p_RenderMode)
        {
            if (_RootPresetGroup.TryGetValue(p_RenderMode, out var o_RootPreset))
            {
            }
            else
            {
                var rootRect = UIxTool.SpawnRectTransform(p_RenderMode.ToString(), _Rect, UIxTool.XAnchorType.Stretch, UIxTool.YAnchorType.Stretch, Vector2.zero, Vector2.zero);
                _RootPresetGroup.Add(p_RenderMode, o_RootPreset = new UIxTool.UIxRootPreset(p_RenderMode, rootRect));
            }

            return o_RootPreset;
        }
        
        public UIxTool.UIxRootPreset GetRootPreset(RenderMode p_RenderMode)
        {
            if (_RootPresetGroup.TryGetValue(p_RenderMode, out var o_RootPreset))
            {
                return o_RootPreset;
            }
   
            return null;
        }
        
        #endregion
    }
}

#endif