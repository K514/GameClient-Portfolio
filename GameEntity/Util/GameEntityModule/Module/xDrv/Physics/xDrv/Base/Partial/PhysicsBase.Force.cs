using System.Collections.Generic;
using UnityEngine;

namespace k514.Mono.Common
{
    public partial class PhysicsBase
    {
        #region <Fields>
                
        /// <summary>
        /// 현재 총 속도
        /// </summary>
        public Vector3 _CurrentVelocity { get; protected set; }
        
        /// <summary>
        /// 타입별 물리계 테이블
        /// </summary>
        protected Dictionary<PhysicsTool.ForceType, PhysicsTool.PhysicsSystem> _PhysicsSystemTable;
   
        /// <summary>
        /// 점프 물리계
        /// </summary>
        private PhysicsTool.PhysicsSystem _JumpPhysicsSystem;
        
        /// <summary>
        /// 중력 물리계
        /// </summary>
        private PhysicsTool.PhysicsSystem _GravityPhysicsSystem;
        
        #endregion

        #region <Callbacks>

        private void OnAwakeForce()
        {
        }

        private void OnSleepForce()
        {
            ClearForce();
        }
        
        #endregion

        #region <Methods>

        public bool HasForce(PhysicsTool.ForceType p_Type)
        {
            return _PhysicsSystemTable[p_Type].IsVelocityValid;
        }

        public bool HasJumpForce()
        {
            return _JumpPhysicsSystem.IsVelocityValid;
        }
        
        public bool HasJumpForceBeforeFloat()
        {
            return _JumpPhysicsSystem.IsVelocityValid && !Entity.IsFloat;
        }

        public bool HasGravityForce()
        {
            return _GravityPhysicsSystem.IsVelocityValid;
        }

        public void ClearForce(PhysicsTool.ForceType p_Type)
        {
            var physicsSystem = _PhysicsSystemTable[p_Type];
            _CurrentVelocity -= physicsSystem.Velocity;
            physicsSystem.ClearForce();
            _AppliedForceFlagMask.RemoveFlag(p_Type);
            
            Update_Y_VelocityType();
        }
        
        public void ClearGravityForce()
        {
            _CurrentVelocity -= _GravityPhysicsSystem.Velocity;
            _GravityPhysicsSystem.ClearForce();
            _AppliedForceFlagMask.RemoveFlag(PhysicsTool.ForceType.Gravity);
            
            Update_Y_VelocityType();
        }
        
        public void ClearForce()
        {
            foreach (var forceType in PhysicsTool._ForceTypeEnumeratorExceptNone)
            {
                _PhysicsSystemTable[forceType].ClearForce();
            }
            
            _CurrentVelocity = Vector3.zero;
            _AppliedForceFlagMask = PhysicsTool.ForceType.None;
            
            Update_Y_VelocityType();
        }
        
        public void ClearForceExcept(PhysicsTool.ForceType p_ExceptMask)
        {
            foreach (var forceType in PhysicsTool._ForceTypeEnumeratorExceptNone)
            {
                if (!p_ExceptMask.HasAnyFlagExceptNone(forceType))
                {
                    var physicsSystem = _PhysicsSystemTable[forceType];
                    _CurrentVelocity -= physicsSystem.VelocityPreDamping;
                    physicsSystem.ClearForce();
                    _AppliedForceFlagMask.RemoveFlag(forceType);
                }
            }

            Update_Y_VelocityType();
        }
        
        public void ClearForceExceptGravity()
        {
            foreach (var forceType in PhysicsTool._ForceTypeEnumeratorExceptGravity)
            {
                var physicsSystem = _PhysicsSystemTable[forceType];
                _CurrentVelocity -= physicsSystem.VelocityPreDamping;
                physicsSystem.ClearForce();
                _AppliedForceFlagMask.RemoveFlag(forceType);
            }

            Update_Y_VelocityType();
        }
        
        #endregion
        
        #region <Methods/Acceleration>

        public void AddAcceleration(PhysicsTool.ForceType p_Type, Vector3 p_Acc)
        {
            _PhysicsSystemTable[p_Type].AddAcceleration(p_Acc);
        }
        
        public void OverlapAcceleration(PhysicsTool.ForceType p_Type, Vector3 p_Acc)
        {
            _PhysicsSystemTable[p_Type].OverlapAcceleration(p_Acc);
        }

        public void ClearAcceleration(PhysicsTool.ForceType p_Type)
        {
            _PhysicsSystemTable[p_Type].ClearAcceleration();
        }
        
        public void ClearGravityAcceleration()
        {
            _GravityPhysicsSystem.ClearAcceleration();
        }

        public void ClearAcceleration()
        {
            foreach (var forceType in PhysicsTool._ForceTypeEnumeratorExceptNone)
            {
                _PhysicsSystemTable[forceType].ClearAcceleration();
            }
        }
        
        public void ClearAccelerationExcept(PhysicsTool.ForceType p_ExceptMask)
        {
            foreach (var forceType in PhysicsTool._ForceTypeEnumeratorExceptNone)
            {
                if (!p_ExceptMask.HasAnyFlagExceptNone(forceType))
                {
                    _PhysicsSystemTable[forceType].ClearAcceleration();
                }
            }
        }
        
        public void ClearAccelerationExceptGravity()
        {
            foreach (var forceType in PhysicsTool._ForceTypeEnumeratorExceptGravity)
            {
                _PhysicsSystemTable[forceType].ClearAcceleration();
            }
        }

        #endregion
        
        #region <Methods/Velocity>
        
        public void AddVelocity(PhysicsTool.ForceType p_Type, Vector3 p_Velocity)
        {
            _PhysicsSystemTable[p_Type].AddVelocity(p_Velocity);
        }

        public void OverlapVelocity(PhysicsTool.ForceType p_Type, Vector3 p_Velocity)
        {
            _PhysicsSystemTable[p_Type].OverlapVelocity(p_Velocity);
        }

        public void ClearVelocity(PhysicsTool.ForceType p_Type)
        {
            _PhysicsSystemTable[p_Type].ClearVelocity();
        }
        
        public void ClearGravityVelocity()
        {
            _GravityPhysicsSystem.ClearVelocity();
        }

        public void ClearVelocity()
        {
            foreach (var forceType in PhysicsTool._ForceTypeEnumeratorExceptNone)
            {
                _PhysicsSystemTable[forceType].ClearVelocity();
            }
        }

        public void ClearVelocityExcept(PhysicsTool.ForceType p_ExceptMask)
        {
            foreach (var forceType in PhysicsTool._ForceTypeEnumeratorExceptNone)
            {
                if (!p_ExceptMask.HasAnyFlagExceptNone(forceType))
                {
                    _PhysicsSystemTable[forceType].ClearVelocity();
                }
            }
        }
        
        public void ClearVelocityExceptGravity()
        {
            foreach (var forceType in PhysicsTool._ForceTypeEnumeratorExceptGravity)
            {
                _PhysicsSystemTable[forceType].ClearVelocity();
            }
        }
        
        private void ClearYLowerVelocity()
        {
            foreach (var forceType in PhysicsTool._ForceTypeEnumeratorExceptNone)
            {
                _PhysicsSystemTable[forceType].ClearYLowerVelocity();
            }
        }

        private void ClearYUpperVelocity()
        {
            foreach (var forceType in PhysicsTool._ForceTypeEnumeratorExceptNone)
            {
                _PhysicsSystemTable[forceType].ClearYUpperVelocity();
            }
        }      
        
        private void ClearYVelocity()
        {
            foreach (var forceType in PhysicsTool._ForceTypeEnumeratorExceptNone)
            {
                _PhysicsSystemTable[forceType].ClearYVelocity();
            }
        }
        
        #endregion
    }
} 