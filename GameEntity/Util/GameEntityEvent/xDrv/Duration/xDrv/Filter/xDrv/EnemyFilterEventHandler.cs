using System.Collections.Generic;
using UnityEngine;

namespace k514.Mono.Common
{
    public class EnemyFilterEventHandler : FilterEventHandlerBase<EnemyFilterEventHandler>
    {
        #region <Fields>

        private List<IGameEntityBridge> _FilterResultSet;
        private EntityQueryTool.FilterState _FilterState;
        private EntityQueryTool.FilterSpaceConfig _FilterSpaceConfig;

        #endregion

        #region <Callbacks>

        public override void OnCreate(IObjectContent<FilterEventHandlerCreateParams> p_Wrapper, FilterEventHandlerCreateParams p_CreateParams)
        {
            base.OnCreate(p_Wrapper, p_CreateParams);

            _FilterResultSet = new List<IGameEntityBridge>();
            _FilterState
                = new EntityQueryTool.FilterState
                (
                    EntityQueryTool.FilterStateQueryFlagType.FreeAll | EntityQueryTool.FilterStateQueryFlagType.ExceptMe, 
                    GameEntityTool.EntityStateType.DEAD,
                    GameEntityTool.GameEntityGroupRelateType.Enemy
                );
        }

        public override bool OnActivate(FilterEventHandlerCreateParams p_CreateParams, FilterEventHandlerActivateParams p_ActivateParams, bool p_IsPooled)
        {
            if (base.OnActivate(p_CreateParams, p_ActivateParams, p_IsPooled))
            {
                _FilterSpaceConfig = _ActivateParams.FilterSpaceParams;
                
                return true;
            }
            else
            {
                return false;
            }
        }

        protected override void OnEventStart()
        {
            FilterEnemy();
        }

        protected override void OnEventProgress(float p_DeltaTime, float p_Rate)
        {
        }

        protected override void OnEventTerminate()
        {
        }

        public override void OnRetrieve(FilterEventHandlerCreateParams p_CreateParams)
        {
            _FilterResultSet.Clear();

            base.OnRetrieve(p_CreateParams);
        }

        #endregion
        
        #region <Methods>

        public override void PreloadEvent()
        {
        }

        private void FilterEnemy()
        {
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

        #endregion
    }
}