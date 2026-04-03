using System.Collections.Generic;
using UnityEngine;

namespace k514.Mono.Common
{
    /// <summary>
    /// 일정한 시간동안 특정한 방향으로 가속하는 아핀 이벤트 핸들러
    ///
    /// 지형이나 유닛에게 부딪히면 주변에 피해를 주는 충격파를 생성하고 즉시 이벤트를 종료한다.
    /// </summary>
    public class LaunchAffineEventHandler : AffineEventHandlerBase<LaunchAffineEventHandler>
    {
        #region <Fields>

        private List<IGameEntityBridge> _FilterResultSet;
        private EntityQueryTool.FilterState _FilterState;
        private EntityQueryTool.FilterSpaceConfig _FilterSpaceConfig;
        
        #endregion
        
        #region <Callbacks>

        public override void OnCreate(IObjectContent<AffineEventHandlerCreateParams> p_Wrapper, AffineEventHandlerCreateParams p_CreateParams)
        {
            base.OnCreate(p_Wrapper, p_CreateParams);

            _FilterResultSet = new List<IGameEntityBridge>();
            _FilterState = 
                new EntityQueryTool.FilterState
                (
                    EntityQueryTool.FilterStateQueryFlagType.FreeAll, 
                    GameEntityTool.EntityStateType.DEAD,
                    GameEntityTool.GameEntityGroupRelateType.Ally | GameEntityTool.GameEntityGroupRelateType.Neutral
                );
        }
        
        public override bool OnActivate(AffineEventHandlerCreateParams p_CreateParams, AffineEventHandlerActivateParams p_ActivateParams, bool p_IsPooled)
        {
            if (base.OnActivate(p_CreateParams, p_ActivateParams, p_IsPooled))
            {
                _FilterSpaceConfig = 
                    new EntityQueryTool.FilterSpaceConfig
                    (
                        EntityQueryTool.FilterSpaceType.Circle, 
                        Entity, 1.5f,
                        p_FilterParamsFlag: EntityQueryTool.FilterSpaceAttributeFlag.None
                    );
                
                return true;
            }
            else
            {
                return false;
            }
        }

        protected override void OnEventStart()
        {
            Entity.AddState(GameEntityTool.EntityStateType.LAUNCH);
            Entity.AddState(GameEntityTool.EntityStateType.STUCK);
        }

        protected override void OnEventProgress(float p_DeltaTime, float p_Rate)
        {
            Entity.PhysicsModule.AddVelocity(PhysicsTool.ForceType.Default, _ActivateParams.Vector);
        }

        protected override void OnEventTerminate()
        {
            Entity.RemoveState(GameEntityTool.EntityStateType.STUCK);
            Entity.RemoveState(GameEntityTool.EntityStateType.LAUNCH);
        }

        public override void OnRetrieve(AffineEventHandlerCreateParams p_CreateParams)
        {
            _FilterResultSet.Clear();
            
            base.OnRetrieve(p_CreateParams);
        }        
        
        public override void OnTriggerEnterWithBoundary(Collider p_Other)
        {
            OnTriggerEnterWithAnything(p_Other);
        }

        public override void OnTriggerEnterWithTerrain(Collider p_Other)
        {
            OnTriggerEnterWithAnything(p_Other);
        }

        public override void OnTriggerEnterWithObstacle(Collider p_Other)
        {
            OnTriggerEnterWithAnything(p_Other);
        }

        public override void OnTriggerEnterWithEntityObject(Collider p_Other, IGameEntityBridge p_Collided)
        {
            OnTriggerEnterWithAnything(p_Other);
        }

        private void OnTriggerEnterWithAnything(Collider p_Other)
        {
            if (Entity.IsLaunched)
            {
                TerminateEvent();
                
                var filterSpace = _FilterSpaceConfig.GetFilterSpace(Entity);
                if (InteractManager.GetInstanceUnsafe.FilterFocusEntity(Entity, ref _FilterState, ref filterSpace, ref _FilterResultSet))
                {
                    if (Entity.IsPlayer)
                    {
                        CameraManager.GetInstanceUnsafe.SetShake(Vector3.right, 7f, 0, 150, 3);
                    }
                
                    foreach (var filtered in _FilterResultSet)
                    {
                        var entityType = filtered.GameEntityType;
                        switch (entityType)
                        {
                            case GameEntityTool.GameEntityType.Unit:
                            {
                                var hitPosition = filtered.GetBottomPosition();
                                var damage = Entity[StatusTool.BattleStatusGroupType.Total].GetScaledAttackBase(DamageCalculator.DamageCalcType.Melee);
                                filtered.GiveDamage(damage, new StatusTool.StatusChangeParams(StatusTool.StatusChangeEventType.Combat, Entity, hitPosition));
                                filtered.TryRunInstanceEvent(InstanceEventTool.InstanceEventType.Hit, new InstanceEventHandlerActivateParams(filtered, 1f));
                                break;
                            }
                        }
                    }
                }
            }
        }

        #endregion
        
        #region <Methods>

        public override void PreloadEvent()
        {
        }

        #endregion
    }
}