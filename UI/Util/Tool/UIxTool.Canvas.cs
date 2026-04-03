#if !SERVER_DRIVE
using UnityEngine;
using UnityEngine.UI;

namespace k514.Mono.Common
{
    public partial class UIxTool
    {
        public static Canvas CreateDefaultCanvas(this RectTransform p_Wrapper)
        {
            var canvasRect = SpawnRectTransform("Canvas", p_Wrapper, XAnchorType.Stretch, YAnchorType.Stretch, Vector2.zero, Vector2.zero);
            var canvasGameObject = canvasRect.gameObject;
            var canvas = canvasGameObject.AddComponent<Canvas>();
            canvasGameObject.layer = (int)GameConst.GameLayerType.UI;
            canvasGameObject.AddComponent<CanvasScaler>().InitCanvasScale();
            canvasGameObject.AddComponent<GraphicRaycaster>();
            
            return canvas;
        }

        public static void InitCanvasScale(this CanvasScaler p_Target)
        {
            p_Target.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            p_Target.referenceResolution = SystemMaintenance.ScreenScaleVector2;
            p_Target.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            p_Target.matchWidthOrHeight = 1f;
            p_Target.referencePixelsPerUnit = 100;
        }
    }
}
#endif