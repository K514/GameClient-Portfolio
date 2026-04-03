using k514.Mono.Common;
using UnityEngine;

namespace k514.Mono.Feature
{
    public class DefenseNexus : GearEntityBase
    {
        #region <Callbacks>

        protected override bool OnActivate(GearPoolManager.CreateParams p_CreateParams, GearPoolManager.ActivateParams p_ActivateParams, bool p_IsPooled)
        {
            if (base.OnActivate(p_CreateParams, p_ActivateParams, p_IsPooled))
            {
                RemoveState(GameEntityTool.EntityStateType.STABLE);
                SetLifeSpan(0f, 1f);
                TurnLayerTo(GameConst.GameLayerType.UnitB);
                SetGroupMask(2);
                
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
            
            // GameManager.GetInstanceUnsafe.DefenceModeGameOver();
        }
        
        #endregion
    }
}