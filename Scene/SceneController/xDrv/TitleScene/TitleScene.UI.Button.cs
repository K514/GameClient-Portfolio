using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace k514.Mono.Common
{
    public partial class TitleScene
    {
        #region <Fields>

        public Button StartButton;
        public Button OptionButton;
        public Button ExitButton;

        #endregion

        #region <Callbacks>

        private void OnCreateButton()
        {
            StartButton.SetButtonHandler(OnClickStartButton);
            OptionButton.SetButtonHandler(OnClickOptionButton);
            ExitButton.SetButtonHandler(OnClickExitButton);
        }

        private void OnClickStartButton()
        {
            if (_CurrentPhase != TitleScenePhase.StartButton)
            {
                SwitchPhase(TitleScenePhase.StartButton);
            }
        }

        private void OnClickOptionButton()
        {
        }

        private void OnClickExitButton()
        {
            SystemBoot.QuitSystem();
        }

        #endregion
    }
}