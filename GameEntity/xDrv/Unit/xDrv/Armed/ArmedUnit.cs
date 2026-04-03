using k514.Mono.Common;

namespace k514.Mono.Feature
{
    public partial class ArmedUnit : UnitBase
    {
        #region <Callbacks>
        
        protected override void OnCreate(UnitPoolManager.CreateParams p_CreateParams)
        {
            base.OnCreate(p_CreateParams);

            OnCreateWeapon();
        }

        protected override void OnDispose()
        {
            OnDisposeWeapon();
            
            base.OnDispose();
        }

        #endregion
    }
}