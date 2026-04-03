using k514.Mono.Common;
using UnityEngine;

namespace k514.Mono.Feature
{
    public class LineDrawer : GearEntityBase
    {
        #region <Fields>

        private ProgressTimer _EventInterval;
        private int _CurrentCount, _WaveCount;
        private float _WaveCountInv;
        private ProjectilePoolManager.CreateParams _CreateParams;
        private Vector3 _Pivot;
        
        #endregion

        #region <Callbacks>

        protected override bool OnActivate(GearPoolManager.CreateParams p_CreateParams, GearPoolManager.ActivateParams p_ActivateParams, bool p_IsPooled)
        {
            if (base.OnActivate(p_CreateParams, p_ActivateParams, p_IsPooled))
            {
                _EventInterval = p_ActivateParams.Duration;
                _CurrentCount = 0;
                _WaveCount = p_ActivateParams.Count;
                _WaveCountInv = 1f / _WaveCount;
            
                if (TryGetMaster(out var o_Master))
                {
                    if (o_Master.TryGetCurrentEnemy(out var o_Enemy))
                    {
                        _Pivot = o_Enemy.GetCenterPosition();
                    }
                    else
                    {
                        _Pivot = o_Master.GetCenterPosition();
                    }
                
                    AddStatus(StatusTool.BattleStatusGroupType.ExtraAdd, BattleStatusTool.BattleStatusType.Attack_Melee, o_Master[StatusTool.BattleStatusGroupType.Total].GetScaledAttackBase(DamageCalculator.DamageCalcType.Melee));
                    SetLifeSpan(999f, 1f);
                }
                else
                {
                    SetDead(false);
                }
                
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
                if (_CurrentCount < _WaveCount)
                {
                    _CurrentCount++;

                    if (TryGetMaster(out var o_Master))
                    {
                        TryRunReservedInstanceEvent();
                    }
                    else
                    {
                        SetDead(false);
                    }
                }
                else
                {
                    if (IsFreeEvent)
                    {
                        SetDead(false);
                    }
                }
            }
            else
            {
                _EventInterval.Progress(p_DeltaTime);
            }
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

        #endregion
    }
}