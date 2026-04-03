namespace k514.Mono.Common
{
    public partial class GameEntityBase<Content, CreateParams, ActivateParams>
    {
        private void OnCreateVolume()
        {
            OnCreateVolumePhysics();
            OnCreateVolumeCollider();
        }

        private void OnActivateVolume()
        {
            OnActivateVolumePhysics();
            OnActivateVolumeCollider();
        }

        private void OnRetrieveVolume()
        {
            OnRetrieveVolumeCollider();
            OnRetrieveVolumePhysics();
        }

        private void OnUpdateVolumeScale(float p_DeltaRatio)
        {
            OnUpdateVolumePhysics();
            OnUpdateVolumeCollider();
        }
    }
}