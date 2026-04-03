using k514.Mono.Feature;
using UnityEngine;

namespace k514.Mono.Common
{
    public class NullPhysics : GameEntityModuleBase, IPhysicsModule
    {
        #region <Fields>

        public Vector3 _CurrentVelocity { get; }
        public CustomMath.Significant Current_Y_VelocityType { get; }

        #endregion
        
        #region <Constructor>

        public NullPhysics() : base(GameEntityModuleTool.ModuleType.None, default, default)
        {
        }

        #endregion

        #region <Callbacks>

        protected override void _OnAwakeModule()
        {
        }

        protected override void _OnSleepModule()
        {
        }

        protected override void _OnResetModule()
        {
        }

        public void OnFixedUpdate(float p_DeltaTime)
        {
        }

        public bool HasForce(PhysicsTool.ForceType p_Type)
        {
            return false;
        }

        public bool HasJumpForce()
        {
            return false;
        }

        public bool HasGravityForce()
        {
            return false;
        }

        public bool HasJumpForceBeforeFloat()
        {
            return false;
        }

        public void ClearForce(PhysicsTool.ForceType p_Type)
        {
        }

        public void ClearGravityForce()
        {
        }

        public void ClearForce()
        {
        }

        public void ClearForceExcept(PhysicsTool.ForceType p_ExceptMask)
        {
        }

        public void ClearForceExceptGravity()
        {
        }

        #endregion

        #region <Methods>

        public PhysicsModuleDataTableQuery.TableLabel GetPhysicsModuleType()
        {
            return PhysicsModuleDataTableQuery.TableLabel.None;
        }

        public void AddAcceleration(PhysicsTool.ForceType p_Type, Vector3 p_Acc)
        {
        }

        public void OverlapAcceleration(PhysicsTool.ForceType p_Type, Vector3 p_Acc)
        {
        }

        public void ClearAcceleration(PhysicsTool.ForceType p_Type)
        {
        }

        public void ClearGravityAcceleration()
        {
        }

        public void ClearAcceleration()
        {
        }

        public void ClearAccelerationExcept(PhysicsTool.ForceType p_ExceptMask)
        {
        }

        public void ClearAccelerationExceptGravity()
        {
        }

        public void AddAcceleration(PhysicsTool.ForceType p_Type, Vector3 p_Acc, float p_UpperBoundSqr)
        {
        }

        public void AddVelocity(Vector3 p_ForceVector)
        {
        }

        public void AddVelocity(PhysicsTool.ForceType p_Type, Vector3 p_Velocity)
        {
        }

        public void AddVelocity(Vector3 p_ForceVector, int p_AddForceIndex)
        {
        }

        public void AddVelocity(PhysicsTool.ForceType p_Type, Vector3 p_Velocity, PhysicsTool.UnitAddForceParams p_UnitAddForceParams)
        {
        }

        public void AddVelocity(PhysicsTool.UnitAddForceParams p_UnitAddForceParams)
        {
        }

        public void OverlapVelocity(PhysicsTool.ForceType p_Type, Vector3 p_Velocity)
        {
        }

        public void ClearGravityVelocity()
        {
        }

        public void ClearVelocity()
        {
        }

        public void ClearVelocityExcept(PhysicsTool.ForceType p_ExceptMask)
        {
        }

        public void ClearVelocityExceptGravity()
        {
        }

        public void ClearVelocity(PhysicsTool.ForceType p_Type)
        {
        }

        public void ClearVelocity(bool p_ClearAutonomyMove)
        {
        }

        public void TryUpdateCollision(Vector3 p_UnitVector)
        {
        }

        public bool IsGround()
        {
            return default;
        }

        public CapsuleCollider GetTriggerCollider()
        {
            return default;
        }

        public void SetGravityFlag(PhysicsTool.GravityType p_GravityFlag)
        {
        }
        
        public void SetAntiGravity(float p_Duration)
        {
        }
        
        public bool IsGrounded()
        {
            return false;
        }

        public void SetPhysicsCollideTrigger(bool p_Flag)
        {
        }

        public PhysicsTool.StampPreset UpdateStampPreset()
        {
            return default;
        }

        public PhysicsTool.StampPreset GetLatestStampPreset()
        {
            return default;
        }

        #endregion
    }
}