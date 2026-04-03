#if !SERVER_DRIVE

namespace k514.Mono.Common
{
    public abstract class UIxControllerBase<Joystick> : UIxContainerBase where Joystick : UIxJoystickBase
    {
        #region <Callbacks>

        protected override bool OnActivate(UIPoolManager.CreateParams p_CreateParams, UIPoolManager.ActivateParams p_ActivateParams, bool p_IsPooled)
        {
            if (base.OnActivate(p_CreateParams, p_ActivateParams, p_IsPooled))
            {
                SetFadeDuration(0.5f, 0f, 0.1f);
            
                var joyStickWrapper = AddElement<UIxFadeJoystickContainer>("JoyRange", new UIxTool.UITouchEventParams(TouchEventTool.TouchEventType.None));
                joyStickWrapper.AddElement<Joystick>("Joy", new UIxTool.UIInputEventParams(InputEventTool.InputLayerType.ControlUnit));

                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion
    }
}
#endif