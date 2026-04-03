using System.Collections.Generic;
using k514.Mono.Common;
using UnityEngine;

namespace k514.Mono.Feature
{
    public partial class BeamEntityBase
    {
        #region <Fields>

        private List<(IGameEntityBridge, Vector3)> _CollidedEntityGroup;

        #endregion
        
        #region <Callbacks>

        private void OnCreateCollision()
        {
            _CollidedEntityGroup = new List<(IGameEntityBridge, Vector3)>();
        }

        private void OnRetrieveCollision()
        {
            _CollidedEntityGroup.Clear();
        }
        
        private void OnEntityHit(IGameEntityBridge p_Entity, Vector3 p_HitPosition)
        {
            var damage = this[StatusTool.BattleStatusGroupType.Total].GetScaledAttackBase(DamageCalculator.DamageCalcType.Melee);
            p_Entity.GiveDamage(damage, new StatusTool.StatusChangeParams(StatusTool.StatusChangeEventType.Combat, GetMaster(), p_HitPosition));
        }

        #endregion

        #region <Methods>

        private void CastBeam(float p_DeltaTime, int p_HitCount)
        {
            _CollidedEntityGroup.Clear();
            _Laser.SetPosition(0, Affine.position);
            
            var isHitIntervalOver = _HitInterval.IsOver();
            
            // 캡슐 쓰지말고 레이캐스트 사용해야함. 여러개를 엮어서 부피 체크 구현
            /*var rayHitCount
                = PhysicsTool.GetCapsuleCastSort
                (
                    this, Affine.forward, _EmitDistance, 
                    GameConst.VisibleBlock_Unit_LayerMask, QueryTriggerInteraction.Collide, ref _RayCastHits
                );

            var targetDistance = _EmitDistance;
            var entityCount = 0;
            for (var i = 0; i < rayHitCount && entityCount <= _PierceCount; i++)
            {
                var rayCastHit = _RayCastHits[i];
                var tryObject = rayCastHit.collider.gameObject;

                switch (tryObject)
                {
                    case var _ when tryObject.IsLayerType(GameConst.GameLayerMaskType.BlockSet) :
                    {
                        targetDistance = rayCastHit.distance;
                        goto OUT_LOOP;
                    }
                    case var _ when tryObject.IsLayerType(GameConst.GameLayerMaskType.UnitSet) :
                    {
                        if (InteractManager.GetInstanceUnsafe.TryGetEntity(tryObject, out var o_Entity))
                        {
                            if (!ReferenceEquals(this, o_Entity)
                                && GetGroupRelate(o_Entity) != GameEntityTool.GameEntityGroupRelateType.Ally 
                                && o_Entity.IsAvailableBeamCollide())
                            {
                                targetDistance = rayCastHit.distance;
                                entityCount++;

                                if (isHitIntervalOver)
                                {
                                    _CollidedEntityGroup.Add((o_Entity, rayCastHit.point));
                                }
                            }
                        }
                        break;
                    }
                }
            }
            
            OUT_LOOP :
            var endPos = Affine.position + targetDistance * Affine.forward;
            _Laser.SetPosition(1, endPos);
            _HitEffectControl.Affine.position = endPos;

            _LaserNoise[0] = targetDistance * _MainTextureLength;
            _LaserNoise[2] = targetDistance * _NoiseTextureLength;
   
            if (isHitIntervalOver)
            {
                _CurrentHitCount += p_HitCount;
                _HitInterval.RelayReset();
            
                if (entityCount > 0)
                {
                    for (var i = 0; i < p_HitCount; i++)
                    {
                        foreach (var collidedEntity in _CollidedEntityGroup)
                        {
                            OnEntityHit(collidedEntity.Item1, collidedEntity.Item2);
                        }
                    }
                }
            }
            else
            {
                _HitInterval.Progress(p_DeltaTime);
            }*/
        }

        #endregion
    }
}