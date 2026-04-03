#if !SERVER_DRIVE

namespace k514.Mono.Common
{
    public partial class SceneController<Me, Phase, Sequence, Handler, Result> : ISceneFaderContainer
    {
        #region <Consts>

        private const string _FaderPanelName = "Fader";

        #endregion

        #region <Fields>

        public UIxFadePanel Fader;
        private bool _IsFaderValid; 
        
        #endregion

        #region <Callbacks>

        private void OnCreateFader()
        {
            _IsFaderValid = !ReferenceEquals(null, Fader);
            if (_IsFaderValid)
            {
                Fader.CheckAwake();
                Fader.SetFadeDuration(GameConst.DefaultSceneTransitionDelay, 0f, GameConst.DefaultSceneTransitionDelay);
                Fader.SetInstantFadeOut();
                
                SceneFaderManager.GetInstanceUnsafe.SetSceneFaderContainer(this, true);
            }
        }

        #endregion

        #region <Methods>

        public UIxFadePanel GetSceneFader()
        {
            return Fader;
        }

        #endregion
    }
}
#endif
