namespace k514.Mono.Common
{
    public partial class GameEntityDeployStorage 
    {
        #region <Callbacks>

        private void OnCreateBoss()
        {
            OnCreateDesertStorm();
            OnCreateDarkWing();
            OnCreateGlacierAnger();
            OnCreateDarkInvader();
        }

        #endregion
    }
}