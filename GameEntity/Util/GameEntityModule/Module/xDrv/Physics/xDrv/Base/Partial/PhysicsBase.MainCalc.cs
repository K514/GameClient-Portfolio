using UnityEngine;

namespace k514.Mono.Common
{
    public partial class PhysicsBase
    {
        #region <Consts>

        /// <summary>
        /// 체공 상태 변화 허용 카운트 하한
        /// </summary>
        private const int _AerialStateTransitionStackUpperBound = 2;
        
        #endregion
        
        #region <Fields>
        
        /// <summary>
        /// Aerial상태를 전이시키는 용도로 사용하는 스택
        /// </summary>
        private int _AerialStateTransitionStack;

        /// <summary>
        /// 몰리모듈이 착지중인지 표시하는 플래그
        /// </summary>
        private bool _IsGrounded;

        #endregion

        #region <Callbacks>

        protected virtual void OnAwakeMainCalc()
        {
            _IsGrounded = GetGrounded();
        }

        private void OnSleepMainCalc()
        {
            _IsGrounded = false;
        }

        #endregion
        
        #region <Methods>

        public bool IsGrounded() => _IsGrounded;
        
        private bool UpdateGrounded()
        {
            switch (_GravityFlag)
            {
                default:
                case PhysicsTool.GravityType.Applied:
                {
                    return GetGrounded();
                }
                case PhysicsTool.GravityType.Anti_HitBreak:
                case PhysicsTool.GravityType.Anti_Perfect:
                {
                    return false;
                }
            }
        }

        protected virtual bool GetGrounded()
        {
            return _CurrentVelocity.y <= 0f && PhysicsTool.CheckEntityLowerCollision(Entity);
        }

        private void UpdateAerialState(float p_DeltaTime)
        {
            var isGroundedPrev = _IsGrounded;
            var isGroundedCur = UpdateGrounded();

            if (isGroundedCur)
            {
                _AerialStateTransitionStack = 0;
                _IsGrounded = true;
            }
            else
            {
                if (_AerialStateTransitionStack > _AerialStateTransitionStackUpperBound)
                {
                    _IsGrounded = false;
                }
                else
                {
                    _AerialStateTransitionStack++;
                }
            }

            switch (this)
            {
                case var _ when isGroundedPrev && !_IsGrounded:
                    OnBeginFloat(_AppliedForceFlagMask, _CurrentVelocity);
                    break;
                case var _ when !isGroundedPrev && _IsGrounded: 
                    OnReachedGround();
                    break;
            }
        }
    
        private void UpdateVelocity(float p_DeltaTime)
        {
            _CurrentVelocity = Vector3.zero;
            _AppliedForceFlagMask = PhysicsTool.ForceType.None;

            foreach (var forceType in PhysicsTool._ForceTypeEnumeratorExceptNone)
            {
                var physicsSystem = _PhysicsSystemTable[forceType];
                if (physicsSystem.UpdateForce(p_DeltaTime))
                {
                    _CurrentVelocity += physicsSystem.Velocity;
                    _AppliedForceFlagMask.AddFlag(forceType);
                }
            }

            Update_Y_VelocityType();
        }
        
        protected abstract void ApplyVelocity(float p_DeltaTime);
        
        #endregion
    }
}