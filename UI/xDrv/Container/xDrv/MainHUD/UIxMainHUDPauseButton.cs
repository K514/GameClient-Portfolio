using k514.Mono.Common;
using UnityEngine;

namespace k514.Mono.Feature
{
    public class UIxMainHUDPauseButton : UIxContainerBase
    {
        protected override void OnCreate(UIPoolManager.CreateParams p_CreateParams)
        {
            base.OnCreate(p_CreateParams);

            AddElement<UIxButton>("PauseBtn", new UIxTool.UIInputEventParams(InputEventTool.InputLayerType.ControlUI, KeyCode.Escape));
        }
    }
}