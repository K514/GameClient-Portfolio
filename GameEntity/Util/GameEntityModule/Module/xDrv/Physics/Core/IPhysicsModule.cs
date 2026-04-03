using k514.Mono.Feature;
using UnityEngine;
using UnityEngine.AI;

namespace k514.Mono.Common
{
    /// <summary>
    /// 유닛의 물리 법칙을 기술하는 모듈
    /// </summary>
    public interface IPhysicsModule : IGameEntityModule
    {
        /* Default */
        PhysicsModuleDataTableQuery.TableLabel GetPhysicsModuleType();
        Vector3 _CurrentVelocity { get; }
        CustomMath.Significant Current_Y_VelocityType { get; }
        
#if ADD_FIXED_UPDATE_GAME_ENTITY
        void OnFixedUpdate(float p_DeltaTime);
#endif

        bool HasForce(PhysicsTool.ForceType p_Type);
        bool HasJumpForce();
        bool HasGravityForce();
        bool HasJumpForceBeforeFloat();
        void ClearForce(PhysicsTool.ForceType p_Type);
        void ClearGravityForce();
        void ClearForce();
        void ClearForceExcept(PhysicsTool.ForceType p_ExceptMask);
        void ClearForceExceptGravity();
        
        void AddAcceleration(PhysicsTool.ForceType p_Type, Vector3 p_Acc);
        void OverlapAcceleration(PhysicsTool.ForceType p_Type, Vector3 p_Acc);
        void ClearAcceleration(PhysicsTool.ForceType p_Type);
        void ClearGravityAcceleration();
        void ClearAcceleration();
        void ClearAccelerationExcept(PhysicsTool.ForceType p_ExceptMask);
        void ClearAccelerationExceptGravity();
        
        void AddVelocity(PhysicsTool.ForceType p_Type, Vector3 p_Velocity);
        void OverlapVelocity(PhysicsTool.ForceType p_Type, Vector3 p_Velocity);
        void ClearVelocity(PhysicsTool.ForceType p_Type);
        void ClearGravityVelocity();
        void ClearVelocity();
        void ClearVelocityExcept(PhysicsTool.ForceType p_ExceptMask);
        void ClearVelocityExceptGravity();
        
        bool IsGrounded();
        void SetGravityFlag(PhysicsTool.GravityType p_GravityFlag);
        void SetAntiGravity(float p_Duration);
    }
}