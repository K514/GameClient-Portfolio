using UnityEngine;

namespace k514.Mono.Common
{
    public partial class RigidbodyPhysics
    {
        #region <Methods>

        protected override void ApplyVelocity(float p_DeltaTime)
        {
            var deltaVel = p_DeltaTime * _CurrentVelocity;
#if UNITY_6000_0_OR_NEWER
            _Rigidbody.linearVelocity = 30f * deltaVel;
#else
            _Rigidbody.velocity = 30f * deltaVel;
#endif
            _Rigidbody.angularVelocity = Vector3.zero;
            Entity.ReserveUpdatePosition();
        }

        #endregion
    }
}