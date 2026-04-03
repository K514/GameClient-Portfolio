using k514.Mono.Common;
using UnityEngine;

namespace k514.Mono.Feature
{
    public class UIxSystemMenu : UIxContainerBase
    {
        #region <Callbacks>

        protected override void OnCreate(UIPoolManager.CreateParams p_CreateParams)
        {
            base.OnCreate(p_CreateParams);

            AddElement<UIxButton>("ResumeBtn", new UIxTool.UIInputEventParams(InputEventTool.InputLayerType.ControlUI, KeyCode.Escape));
            AddElement<UIxButton>("StatusBtn", new UIxTool.UIInputEventParams(InputEventTool.InputLayerType.ControlUI, KeyCode.M));
            AddElement<UIxButton>("LogBtn", new UIxTool.UIInputEventParams(InputEventTool.InputLayerType.ControlUI, KeyCode.L));
            AddElement<UIxButton>("ConfigBtn", new UIxTool.UIInputEventParams(InputEventTool.InputLayerType.ControlUI, KeyCode.G));
            AddElement<UIxButton>("ExitBtn", new UIxTool.UIInputEventParams(InputEventTool.InputLayerType.ControlUI, KeyCode.F10));
        }

        public override void OnControlInput(InputLayerEventParams p_Params)
        {
            var keyCode = p_Params.KeyCode;
            switch (keyCode)
            {
                case KeyCode.Escape:
                {
                    if (p_Params.IsTouched)
                    {
                        UIxControlRoot.GetInstanceUnsafe.PopFromControlStack(this);
                    }
                    break;
                }
                case KeyCode.M:
                {
                    break;
                }
                case KeyCode.L:
                {
                    break;
                }
                case KeyCode.G:
                {
                    if (p_Params.IsTouched)
                    {
                        UIxControlRoot.GetInstanceUnsafe.GameConfig.PushToControlStack();
                    }
                    break;
                }
                case KeyCode.F10:
                {
                    if (p_Params.IsTouched)
                    {
                        UIxControlRoot.GetInstanceUnsafe.GoToLobby();
                    }
                    break;
                }
            }
        }

        #endregion
    }
}