using UnityEngine;

#if !SERVER_DRIVE

namespace k514.Mono.Common
{
    public partial class UIxObjectRoot
    {
        #region <Callbacks>

        public void OnLateUpdate(float p_DeltaTime)
        {
            if (gameObject.activeSelf)
            {
                foreach (var renderMode in _RenderMode_Enumerator)
                {
                    if (_RootPresetGroup.TryGetValue(renderMode, out var o_RootPreset))
                    {
                        var uiCanvasCollection = o_RootPreset.UIxCanvasCollection;
                        foreach (var uiCanvasPresetKV in uiCanvasCollection)
                        {
                            var uiCanvasPreset = uiCanvasPresetKV.Value;
                            var uiElementCollection = uiCanvasPreset.UIxElementCollection;
                            foreach (var uiElementKV in uiElementCollection)
                            {
                                var uiElement = uiElementKV.Value;
                                if (uiElement.IsVisible())
                                {
                                   uiElement.OnLateUpdate(p_DeltaTime);
                                }
                            }
                        }
                    }
                }
            }
        }

        #endregion
        
        #region <Methods>

        public void Broadcast_HideUI(bool p_HideFlag)
        {
            foreach (var renderMode in _RenderMode_Enumerator)
            {
                if (_RootPresetGroup.TryGetValue(renderMode, out var o_RootPreset))
                {
                    var uiCanvasCollection = o_RootPreset.UIxCanvasCollection;
                    foreach (var uiCanvasPresetKV in uiCanvasCollection)
                    {
                        var uiCanvasPreset = uiCanvasPresetKV.Value;
                        var uiElementCollection = uiCanvasPreset.UIxElementCollection;
                        foreach (var uiElementKV in uiElementCollection)
                        {
                            var uiElement = uiElementKV.Value;
                            uiElement.SetHide(p_HideFlag);
                        }
                    }
                }
            }
        }
        
        #endregion
    }
}

#endif