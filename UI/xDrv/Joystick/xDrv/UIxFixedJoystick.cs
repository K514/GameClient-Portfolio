#if !SERVER_DRIVE

namespace k514.Mono.Common
{
    public class UIxFixedJoystick : UIxJoystickBase
    {
        #region <Callbacks>
        
        protected override void OnCreate(UIPoolManager.CreateParams p_CreateParams)
        {
            base.OnCreate(p_CreateParams);
            
            SetStateFlag(UIxTool.UIxStaticStateType.ResetBaseWhenRelease);
        }

        #endregion
    }
}

#endif