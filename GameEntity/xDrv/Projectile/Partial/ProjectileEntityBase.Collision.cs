using System.Collections.Generic;
using k514.Mono.Common;
using UnityEngine;

namespace k514.Mono.Feature
{
    public partial class ProjectileEntityBase
    {
        #region <Fields>

        private int _CollisionEventId;

        #endregion
        
        #region <Callbacks>

        private void OnCreateCollision()
        {
        }

        private void OnRetrieveCollision()
        {
        }

        protected override void OnTriggerEnterWithTerrain(Collider p_Other)
        {
            base.OnTriggerEnterWithTerrain(p_Other);
            
            SetDead(false);
        }

        protected override void OnTriggerEnterWithBoundary(Collider p_Other)
        {
            base.OnTriggerEnterWithBoundary(p_Other);
       
            SetDead(false);
        }

        protected override void OnTriggerEnterWithObstacle(Collider p_Other)
        {
            base.OnTriggerEnterWithObstacle(p_Other);
    
            SetDead(false);
        }

        protected override void OnTriggerEnterWithEntityObject(Collider p_Other, IGameEntityBridge p_Trigger)
        {
            base.OnTriggerEnterWithEntityObject(p_Other, p_Trigger);
    
            if(p_Trigger.IsAvailableProjectileCollide())
            {
                var relateType = GetGroupRelate(p_Trigger);
                switch (relateType)
                {
                    case GameEntityTool.GameEntityGroupRelateType.None:
                    case GameEntityTool.GameEntityGroupRelateType.Ally:
                        break;
                    case GameEntityTool.GameEntityGroupRelateType.Neutral:
                    case GameEntityTool.GameEntityGroupRelateType.Enemy:
                    {
                        OnEntityHit(p_Trigger, GetBottomPosition());
                        break;
                    }
                }
            }
        }
        
        private void OnEntityHit(IGameEntityBridge p_Entity, Vector3 p_HitPosition)
        {
            if (_ProjectileAttribuetFlagMask.HasAnyFlagExceptNone(ProjectileTool.ProjectileAttributeType.KnockBack))
            {
                p_Entity.PhysicsModule.AddVelocity(PhysicsTool.ForceType.Default, _KnockBackForce * Affine.forward);
            }
                
            var damage = this[StatusTool.BattleStatusGroupType.Total].GetScaledAttackBase(DamageCalculator.DamageCalcType.Melee);
            p_Entity.GiveDamage(damage, new StatusTool.StatusChangeParams(StatusTool.StatusChangeEventType.Combat, GetMaster(), p_HitPosition));
                
            if (_ProjectileAttribuetFlagMask.HasAnyFlagExceptNone(ProjectileTool.ProjectileAttributeType.Pierce))
            {
                if (_PierceCount-- < 1)
                {
                    SetDead(false);
                }
            }
            else
            {
                SetDead(false);
            }
        }

        #endregion
    }
}