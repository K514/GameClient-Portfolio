using k514.Mono.Common;
using UnityEngine;

namespace k514.Mono.Feature
{
    public abstract partial class UnitBase : GameEntityBase<UnitBase, UnitPoolManager.CreateParams, UnitPoolManager.ActivateParams>, IUnitBridge
    {
        #region <Callbacks>

        protected override bool OnActivate(UnitPoolManager.CreateParams p_CreateParams, UnitPoolManager.ActivateParams p_ActivateParams, bool p_IsPooled)
        {
            if (base.OnActivate(p_CreateParams, p_ActivateParams, p_IsPooled))
            {
                TurnLayerTo(GameConst.GameLayerType.UnitA);
                SetLifeSpan(0f, 1f);
                
                return true;
            }
            else
            {
                return false;
            }
        }

        protected override void OnDeadSpanStarted()
        {
            base.OnDeadSpanStarted();

            TurnLayerTo(GameConst.GameLayerType.UnitC);
        }
        
        #endregion
    }
}