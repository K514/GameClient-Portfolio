namespace k514.Mono.Common
{
    public partial class GameEntityDeployStorage
    {
        private void OnCreateHero()
        {
            OnCreateAssassin();
            OnCreateKnight();
            OnCreateMage();
            OnCreateArcher();
        }
    }
}