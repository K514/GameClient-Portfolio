using k514.Mono.Common;

namespace k514.Mono.Feature
{
    public class ForwardHitField : GearEntityBase
    {
        #region <Consts>

        private const int _HitCount = 25;
        private const float _HitInterval = 1f / _HitCount;

        #endregion
        
        #region <Fields>

        private EntityQueryTool.FilterState _QueryParams;
        private ProgressTimer _EventInterval;
        private int _CurrentCount;
        
        #endregion

        #region <Callbacks>

        protected override bool OnActivate(GearPoolManager.CreateParams p_CreateParams, GearPoolManager.ActivateParams p_ActivateParams, bool p_IsPooled)
        {
            if (base.OnActivate(p_CreateParams, p_ActivateParams, p_IsPooled))
            {
                var filterSpace = p_ActivateParams.FilterSpace;
                _QueryParams
                    = new EntityQueryTool.FilterState
                    (
                        EntityQueryTool.FilterStateQueryFlagType.ExceptMe | EntityQueryTool.FilterStateQueryFlagType.FreeAll, 
                        GameEntityTool.EntityStateType.EngageCandidateFilterMask,
                        GameEntityTool.GameEntityGroupRelateType.Enemy
                    );
            
                _EventInterval = p_ActivateParams.Duration * _HitInterval;
                _CurrentCount = 0;

                AddStatus(StatusTool.BattleStatusGroupType.ExtraAdd, BattleStatusTool.BattleStatusType.SightRange, filterSpace.FloatValue1);

                if (TryGetMaster(out var o_Master))
                {
                    AddStatus(StatusTool.BattleStatusGroupType.ExtraAdd, BattleStatusTool.BattleStatusType.Attack_Melee, o_Master[StatusTool.BattleStatusGroupType.Total, BattleStatusTool.BattleStatusType.Attack_Melee, _HitInterval]);
                }
            
                InteractManager.GetInstanceUnsafe.ReserveUpdateInteractionFrom(this, true);
                SetLifeSpan(999f, 0f);
                
                return true;
            }
            else
            {
                return false;
            }
        }

        protected override void OnLiveSpanProgress(float p_DeltaTime)
        {
            base.OnLiveSpanProgress(p_DeltaTime);

            if (_EventInterval.IsOver())
            {
                _EventInterval.Reset();
                if (_CurrentCount < _HitCount)
                {
                    _CurrentCount++;

                    // if (InteractManager.GetInstanceUnsafe.FilterFocusEntity(this, _QueryParams, Affine))
                    {
                        foreach (var entity in FilterResultGroup)
                        {
                            OnHitEntity(entity);
                        }
                    }
                    
                    InteractManager.GetInstanceUnsafe.ReserveUpdateInteractionFrom(this, true);
                }
                else
                {
                    SetDead(false);
                }
            }
            else
            {
                _EventInterval.Progress(p_DeltaTime);
            }
        }

        protected virtual void OnHitEntity(IGameEntityBridge p_Entity)
        {
            p_Entity.GiveDamage(new StatusTool.StatusChangeParams(StatusTool.StatusChangeEventType.Combat, this));
        }

        #endregion
    }
}