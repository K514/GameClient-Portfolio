#if !SERVER_DRIVE

using k514.Mono.Feature;

namespace k514.Mono.Common
{
    public partial class UIxControlRoot
    {
        #region <Fields>
        
        public UIxFadePanel MainFader;
        public UIxFadePanel SceneFader;
        public UIxMainHUD MainHUD;
        public UIxMainHUDPauseButton MainHUDPauseButton;
        public UIxTouchPanel TouchPanel;
        public UIxPlayerState PlayerState;
        public UIxController Controller;
        public UIxSystemMenu SystemMenu;
        public UIxGameConfig GameConfig;
        public UIxHpBarTheater HpBarTheater;
        public UIxNameTheater NameTheater;
        public UIxNumberTheater NumberTheater;
        public UIxGaugeTheater GaugeTheater;
        
        #endregion

        #region <Callbacks>

        private void OnCreateControl()
        {
            MainFader = UIxObjectRoot.GetInstanceUnsafe.GetElement(UIxTool.UIxElementType.Fader, 98) as UIxFadePanel;
            SceneFader = UIxObjectRoot.GetInstanceUnsafe.GetElement(UIxTool.UIxElementType.Fader, 99) as UIxFadePanel;
            MainHUD = UIxObjectRoot.GetInstanceUnsafe.GetElement(UIxTool.UIxElementType.MainHUD) as UIxMainHUD;
            MainHUDPauseButton = UIxObjectRoot.GetInstanceUnsafe.GetElement(UIxTool.UIxElementType.MainHUDPauseButton) as UIxMainHUDPauseButton;
            TouchPanel = UIxObjectRoot.GetInstanceUnsafe.GetElement(UIxTool.UIxElementType.TouchPanel) as UIxTouchPanel;
            PlayerState = UIxObjectRoot.GetInstanceUnsafe.GetElement(UIxTool.UIxElementType.PlayerState) as UIxPlayerState;
            Controller = UIxObjectRoot.GetInstanceUnsafe.GetElement(UIxTool.UIxElementType.Controller) as UIxController;
            SystemMenu = UIxObjectRoot.GetInstanceUnsafe.GetElement(UIxTool.UIxElementType.SystemMenu) as UIxSystemMenu;
            GameConfig = UIxObjectRoot.GetInstanceUnsafe.GetElement(UIxTool.UIxElementType.GameConfig) as UIxGameConfig;
            HpBarTheater = UIxObjectRoot.GetInstanceUnsafe.GetElement(UIxTool.UIxElementType.HpBarTheater) as UIxHpBarTheater;
            NameTheater = UIxObjectRoot.GetInstanceUnsafe.GetElement(UIxTool.UIxElementType.NameTheater) as UIxNameTheater;
            NumberTheater = UIxObjectRoot.GetInstanceUnsafe.GetElement(UIxTool.UIxElementType.NumberTheater) as UIxNumberTheater;
            GaugeTheater = UIxObjectRoot.GetInstanceUnsafe.GetElement(UIxTool.UIxElementType.GaugeTheater) as UIxGaugeTheater;

            OnCreateControlUI();
        }

        #endregion
    }
}

#endif