using UnityEngine;

namespace k514.Mono.Common
{
    public partial class KinematicPhysics
    {
        #region <Methods>

        protected override void ApplyVelocity(float p_DeltaTime)
        {
            var deltaVel = p_DeltaTime * _CurrentVelocity;
            
            Affine.position += deltaVel;
            Entity.ReserveUpdatePosition();
        }

        #endregion
    }
}