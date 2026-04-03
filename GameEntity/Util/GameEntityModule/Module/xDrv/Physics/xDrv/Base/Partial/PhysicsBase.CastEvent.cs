using UnityEngine;

namespace k514.Mono.Common
{
    public partial class PhysicsBase
    {
        #region <Callbacks>

        public void OnBeginFloat(PhysicsTool.ForceType p_Mask, Vector3 p_CurrentForce)
        {
            Entity.OnModule_BeginFloat(p_Mask, p_CurrentForce);
        }

        public void OnVelocity_Y_Changed(PhysicsTool.ForceType p_Mask, CustomMath.Significant p_PrevY, CustomMath.Significant p_CurY)
        {
            if (
                p_Mask.HasAnyFlagExceptNone(PhysicsTool.ForceType.Jump) 
                && p_CurY == CustomMath.Significant.Plus
                && !Entity.IsDrivingAction
               )
            {
                Entity.OnModule_ManualJump();
            }
        }

        private void OnReachedGround()
        {
            OnStampingObjects();
            Entity.OnModule_ReachedGround(_LatestStampPreset);
        }
        
        #endregion
    }
}