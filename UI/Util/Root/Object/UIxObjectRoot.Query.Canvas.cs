#if !SERVER_DRIVE

using UnityEngine;

namespace k514.Mono.Common
{
    public partial class UIxObjectRoot
    {
        #region <Methods>

        public UIxTool.UIxCanvasPreset AddCanvas(RenderMode p_RenderMode, int p_SortingLayer)
        {
            return AddRoot(p_RenderMode).AddCanvas(p_SortingLayer);
        }
        
        public UIxTool.UIxCanvasPreset GetCanvasPreset(RenderMode p_RenderMode, int p_SortingLayer)
        {
            if (_RootPresetGroup.TryGetValue(p_RenderMode, out var o_RootPreset))
            {
                var uiCanvasCollection = o_RootPreset.UIxCanvasCollection;
                if (uiCanvasCollection.TryGetValue(p_SortingLayer, out var o_CanvasPreset))
                {
                    return o_CanvasPreset;
                }
            }
            
            return null;
        }

        #endregion
    }
}

#endif