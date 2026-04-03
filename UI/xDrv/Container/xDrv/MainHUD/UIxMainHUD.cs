#if !SERVER_DRIVE

using k514.Mono.Common;
using UnityEngine;

namespace k514.Mono.Feature
{
    public class UIxMainHUD : UIxContainerBase
    {
        #region <Callbacks>
        
        protected override void OnVisible()
        {
            base.OnVisible();
            
            UIxControlRoot.GetInstanceUnsafe?.MainHUDPauseButton.SetHide(false);
            UIxControlRoot.GetInstanceUnsafe?.PlayerState.SetHide(false);
            UIxControlRoot.GetInstanceUnsafe?.Controller.SetHide(false);
        }

        protected override void OnHide()
        {
            base.OnHide();
            
            UIxControlRoot.GetInstanceUnsafe?.MainHUDPauseButton.SetHide(true);
            UIxControlRoot.GetInstanceUnsafe?.PlayerState.SetHide(true);
            UIxControlRoot.GetInstanceUnsafe?.Controller.SetHide(true);
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
                        UIxControlRoot.GetInstanceUnsafe.SystemMenu.PushToControlStack();
                    }
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
            }
        }
        
        #endregion
    }
}
#endif