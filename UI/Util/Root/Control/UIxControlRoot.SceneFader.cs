#if !SERVER_DRIVE

namespace k514.Mono.Common
{
    public partial class UIxControlRoot
    {
        #region <Fields>
        
        private bool _IsSceneFaderValid;

        #endregion

        #region <Callbacks>

        private void OnCreateSceneFader()
        {
            _IsSceneFaderValid = !ReferenceEquals(null, SceneFader);
            
            RegisterSceneFader();
        }
        
        #endregion
        
        #region <Methods>

        private void RegisterSceneFader()
        {
            if (_IsSceneFaderValid)
            {
                SceneFader.SetFadeDuration(GameConst.DefaultSceneTransitionDelay, 0f, GameConst.DefaultSceneTransitionDelay);
                SceneFader.SetInstantFadeOut();
                
                SceneFaderManager.GetInstanceUnsafe.SetSceneFaderContainer(this, false);
            }
        }
        
        public UIxFadePanel GetSceneFader()
        {
            return SceneFader;
        }

        #endregion
    }
}

#endif