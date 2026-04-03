using System.Threading;
using Cysharp.Threading.Tasks;
#if !SERVER_DRIVE

using UnityEngine;

namespace k514.Mono.Common
{
    public partial class UIxObjectRoot
    {
        #region <Methods>

        public async UniTask<UIxElementBase> AddElement(RenderMode p_RenderMode, int p_SortingLayer, UIxTool.UIxElementType p_ElementType, CancellationToken p_CancellationToken)
        {
            return await AddRoot(p_RenderMode).AddCanvas(p_SortingLayer).AddElement(p_ElementType, p_CancellationToken);
        }

        public bool TryGetElement(RenderMode p_RenderMode, int p_SortingLayer, UIxTool.UIxElementType p_ElementType, out UIxElementBase o_Element)
        {
            if (_RootPresetGroup.TryGetValue(p_RenderMode, out var o_RootPreset))
            {
                var uiCanvasCollection = o_RootPreset.UIxCanvasCollection;
                if (uiCanvasCollection.TryGetValue(p_SortingLayer, out var o_CanvasPreset))
                {
                    var uiElementCollection = o_CanvasPreset.UIxElementCollection;
                    if (uiElementCollection.TryGetValue(p_ElementType, out o_Element))
                    {
                        return true;
                    }
                }
            }

            o_Element = default;
            return false;
        }

        public UIxElementBase GetElement(RenderMode p_RenderMode, int p_SortingLayer, UIxTool.UIxElementType p_ElementType)
        {
            if (_RootPresetGroup.TryGetValue(p_RenderMode, out var o_RootPreset))
            {
                var uiCanvasCollection = o_RootPreset.UIxCanvasCollection;
                if (uiCanvasCollection.TryGetValue(p_SortingLayer, out var o_CanvasPreset))
                {
                    var uiElementCollection = o_CanvasPreset.UIxElementCollection;
                    if (uiElementCollection.TryGetValue(p_ElementType, out var o_Element))
                    {
                        return o_Element;
                    }
                }
            }

            return default;
        }

        public UIxElementBase GetElement(UIxTool.UIxElementType p_ElementType, int p_SortingLayer)
        {
            foreach (var renderMode in _RenderMode_Enumerator)
            {
                if (TryGetElement(renderMode, p_SortingLayer, p_ElementType, out var o_Element))
                {
                    return o_Element;
                }
            }

            return default;
        }

        public bool TryGetElement(RenderMode p_RenderMode, UIxTool.UIxElementType p_ElementType, out UIxElementBase o_Element)
        {
            if (_RootPresetGroup.TryGetValue(p_RenderMode, out var o_RootPreset))
            {
                var uiCanvasCollection = o_RootPreset.UIxCanvasCollection;
                foreach (var canvasPresetKV in uiCanvasCollection)
                {
                    var uiCanvasPreset = canvasPresetKV.Value;
                    var uiElementCollection = uiCanvasPreset.UIxElementCollection;
                    if (uiElementCollection.TryGetValue(p_ElementType, out o_Element))
                    {
                        return true;
                    }
                }
            }

            o_Element = default;
            return false;
        }

        public UIxElementBase GetElement(RenderMode p_RenderMode, UIxTool.UIxElementType p_ElementType)
        {
            if (TryGetElement(p_RenderMode, p_ElementType, out var o_Element))
            {
                return o_Element;
            }
            else
            {
                return default;
            }
        }

        public UIxElementBase GetElement(UIxTool.UIxElementType p_ElementType)
        {
            foreach (var renderMode in _RenderMode_Enumerator)
            {
                if(TryGetElement(renderMode, p_ElementType, out var o_Element))
                {
                    return o_Element;
                }
            }

            return default;
        }
        
        #endregion
    }
}

#endif