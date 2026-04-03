using UnityEngine;

namespace k514.Mono.Common
{
    public partial class CharacterControllerPhysics
    {
        #region <Fields>

        private Vector3 _GroundCheckVector;

        #endregion

        #region <Callbacks>

        protected override void OnAwakeMainCalc()
        {
            _GroundCheckVector = Mathf.Max(2f * _CharacterController.skinWidth, __Default_GroundCheck_Distance_UpperBound) * Vector3.down;
            _CharacterController.Move(_GroundCheckVector);
            
            base.OnAwakeMainCalc();
        }

        #endregion
        
        #region <Methods>

        protected override bool GetGrounded()
        {
            return _CharacterController.isGrounded;
        }

        protected override void ApplyVelocity(float p_DeltaTime)
        {
            var deltaVel = p_DeltaTime * _CurrentVelocity;
            if (Current_Y_VelocityType == CustomMath.Significant.Zero)
            {
                deltaVel += _GroundCheckVector;
            }
            
            _CharacterController.Move(deltaVel);
            Entity.ReserveUpdatePosition();
        }

        #endregion
    }
}