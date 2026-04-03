#if !SERVER_DRIVE

namespace k514.Mono.Common
{
    public class UIxFloattingJoystick : UIxJoystickBase
    {
        #region <Callbacks>

        protected override void OnCreate(UIPoolManager.CreateParams p_CreateParams)
        {
            base.OnCreate(p_CreateParams);
            
            SetStateFlag(UIxTool.UIxStaticStateType.FloatPivotWhenPress);
        }

        protected override bool OnActivate(UIPoolManager.CreateParams p_CreateParams, UIPoolManager.ActivateParams p_ActivateParams, bool p_IsPooled)
        {
            if (base.OnActivate(p_CreateParams, p_ActivateParams, p_IsPooled))
            {
                SetFadeDuration(0.5f, 0f, 0.1f);

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