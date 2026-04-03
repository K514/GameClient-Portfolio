namespace k514.Mono.Common
{
    public abstract partial class GameSceneEnvironmentBase : SceneEnvironment
    {
        #region <Callbacks>

        protected virtual void OnCreateGameSceneEnvironment()
        {
            OnCreateTerrain();
            OnCreateSceneLocation();
            OnCreateSceneDependency();
            OnCreateSceneUI();
        }
        
        #endregion
    }
}