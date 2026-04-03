using k514.Mono.Common;

namespace k514.Mono.Feature
{
    public partial class UnitBase
    {
        protected override void OnCreateAffine()
        {
            base.OnCreateAffine();
            
            OnCreateAttachPoint();
        }

        protected override void OnActivateAffine(UnitPoolManager.ActivateParams p_ActivateParams)
        {
            base.OnActivateAffine(p_ActivateParams);

            OnActivateAttachPoint();
        }

        protected override void OnRetrieveAffine()
        {
            OnRetrieveAttachPoint();
            
            base.OnCreateAffine();
        }
    }
}