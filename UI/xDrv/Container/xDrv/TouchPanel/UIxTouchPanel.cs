using k514.Mono.Common;

#if !SERVER_DRIVE
namespace k514.Mono.Feature
{
    public class UIxTouchPanel : UIxContainerBase
    {
        #region <Fields>

        #endregion

        #region <Callbacks>

        protected override bool OnActivate(UIPoolManager.CreateParams p_CreateParams, UIPoolManager.ActivateParams p_ActivateParams, bool p_IsPooled)
        {
            if (base.OnActivate(p_CreateParams, p_ActivateParams, p_IsPooled))
            {
                var touchPanel = AddElement<UIxButton>("TouchPanel", new UIxTool.UITouchEventParams(TouchEventTool.TouchEventType.ControlView | TouchEventTool.TouchEventType.TouchWorldObject));
                touchPanel.SetStateFlag(UIxTool.UIxStaticStateType.UpdateDragDirectionUsingPointerData | UIxTool.UIxStaticStateType.ResetInputDragEveryFrame);
                
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