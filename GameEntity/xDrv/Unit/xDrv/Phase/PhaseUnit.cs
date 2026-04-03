using k514.Mono.Common;
using UnityEngine;

namespace k514.Mono.Feature
{
    public partial class PhaseUnit : UnitBase
    {
        #region <Callbacks>

        protected override void OnCreate(UnitPoolManager.CreateParams p_CreateParams)
        {
            base.OnCreate(p_CreateParams);
            
            OnCreatePhase();
        }

        protected override bool OnActivate(UnitPoolManager.CreateParams p_CreateParams, UnitPoolManager.ActivateParams p_ActivateParams, bool p_IsPooled)
        {
            if (base.OnActivate(p_CreateParams, p_ActivateParams, p_IsPooled))
            {
                OnActivatePhase();
                
                return true;
            }
            else
            {
                return false;
            }
        }

        protected override void OnUpdateEntity(float p_DeltaTime)
        {
            base.OnUpdateEntity(p_DeltaTime);
                
            OnUpdatePhase(p_DeltaTime);
        }

        #endregion
    }
}