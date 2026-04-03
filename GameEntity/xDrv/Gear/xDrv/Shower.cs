using k514.Mono.Common;
using UnityEngine;

namespace k514.Mono.Feature
{
    public class Shower : GearEntityBase
    {
        #region <Fields>

        private ProgressTimer _EventInterval;
        private int _CurrentCount, _WaveCount;
        private ProjectilePoolManager.CreateParams _CreateParams;
        
        #endregion

        #region <Callbacks>

        protected override bool OnActivate(GearPoolManager.CreateParams p_CreateParams, GearPoolManager.ActivateParams p_ActivateParams, bool p_IsPooled)
        {
            if (base.OnActivate(p_CreateParams, p_ActivateParams, p_IsPooled))
            {
                _EventInterval = 0.08f;
                _CurrentCount = 0;
                _WaveCount = 25;
                AddStatus(StatusTool.BattleStatusGroupType.CompoundMul, BattleStatusTool.BattleStatusType.DamageRate_Melee, p_ActivateParams.DamageRate);
            
                SetLifeSpan(999f, 1f);
                
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
                        var damageRate = this[StatusTool.BattleStatusGroupType.Total, BattleStatusTool.BattleStatusType.DamageRate_Melee];
                        var arrowCount = 3;

                        for (int i = 0; i < arrowCount; i++)
                        {
                            var pos = GetCenterPosition().GetRandomPosition(XYZType.ZX, 0f, Radius.CurrentValue);
                            var affine = new AffinePreset(pos, 0.6f);
                            var uv = -Affine.up.RotationVectorByPivot(Vector3.right, Random.Range(-25f, 25f)).RotationVectorByPivot(Vector3.forward, Random.Range(-25f, 25f)) ;
                        
                            ProjectilePoolManager.GetInstanceUnsafe
                                .SpawnForwardProjectile
                                (
                                    _CreateParams,
                                    o_Master, new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, affine), uv, 25f, damageRate, 5f
                                );
                        }
                    }
                    else
                    {
                        SetDead(false);
                    }
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

        #endregion
    }
}