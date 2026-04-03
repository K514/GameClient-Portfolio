using System.Collections.Generic;
using k514.Mono.Common;
using UnityEngine;

namespace k514.Mono.Feature
{
    public class Satellite : GearEntityBase
    {
        #region <Fields>

        private BeamPoolManager.CreateParams _CreateParams;
        private List<IGameEntityBridge> _BeamList;
        private Transform _Rotation;
        private GameEntityBaseEventReceiver _BeamEventReceiver;
        
        #endregion

        #region <Callbacks>

        protected override void OnCreate(GearPoolManager.CreateParams p_CreateParams)
        {
            base.OnCreate(p_CreateParams);

            _BeamList = new List<IGameEntityBridge>();
            _Rotation = new GameObject().transform;
            _Rotation.SetParent(Affine);
            _CreateParams = BeamPoolManager.GetInstanceUnsafe.GetCreateParams(2);
            _BeamEventReceiver = new GameEntityBaseEventReceiver(GameEntityTool.GameEntityBaseEventType.Retrieved, OnHandleBeamEvent);
        }

        protected override bool OnActivate(GearPoolManager.CreateParams p_CreateParams, GearPoolManager.ActivateParams p_ActivateParams, bool p_IsPooled)
        {
            if (base.OnActivate(p_CreateParams, p_ActivateParams, p_IsPooled))
            {
                var shotStatus = _Master[StatusTool.ShotStatusGroupType.Total];
                AddStatus(StatusTool.ShotStatusGroupType.ExtraAdd, ShotStatusTool.ShotStatusType.ShotCount, shotStatus.ShotCount);
                AddStatus(StatusTool.ShotStatusGroupType.ExtraAdd, ShotStatusTool.ShotStatusType.ShotSpeed, shotStatus.ShotSpeed);
                AddStatus(StatusTool.ShotStatusGroupType.ExtraAdd, ShotStatusTool.ShotStatusType.ShotScale, shotStatus.ShotScale);
                AddStatus(StatusTool.ShotStatusGroupType.ExtraAdd, ShotStatusTool.ShotStatusType.ShotDuration, shotStatus.ShotDuration);
                
                var battleStatus = _Master[StatusTool.BattleStatusGroupType.Total];
                AddStatus(StatusTool.BattleStatusGroupType.ExtraAdd, BattleStatusTool.BattleStatusType.Attack_Melee, 4f * battleStatus.GetScaledAttackBase(DamageCalculator.DamageCalcType.Melee));
                AddStatus(StatusTool.BattleStatusGroupType.ExtraAdd, BattleStatusTool.BattleStatusType.CriticalRate_Melee, battleStatus.GetCriticalRate(DamageCalculator.DamageCalcType.Melee));
                AddStatus(StatusTool.BattleStatusGroupType.CompoundMul, BattleStatusTool.BattleStatusType.DamageRate, p_ActivateParams.DamageRate * battleStatus.GetScaledDamageRate(DamageCalculator.DamageCalcType.Melee));
                AddStatus(StatusTool.BattleStatusGroupType.ExtraAdd, BattleStatusTool.BattleStatusType.Absorb, battleStatus.Absorb);
                AddStatus(StatusTool.BattleStatusGroupType.ExtraAdd, BattleStatusTool.BattleStatusType.MoveSpeedRate, shotStatus.ShotSpeed);
                AddStatus(StatusTool.BattleStatusGroupType.ExtraAdd, BattleStatusTool.BattleStatusType.MoveSpeedBasis, 4f);
                AddStatus(StatusTool.BattleStatusGroupType.ExtraAdd, BattleStatusTool.BattleStatusType.SightRange, 25f);
                
                AddState(GameEntityTool.EntityStateType.STABLE);
                
                SwitchActionModule(ActionModuleDataTableQuery.TableLabel.Default);
                SwitchPersona(AutonomyModuleDataTableQuery.TableLabel.Melee);
                SwitchGeometryModule(GeometryModuleDataTableQuery.TableLabel.Affine);
                SwitchPhysicsModule(PhysicsModuleDataTableQuery.TableLabel.Affine);
                
                var count = 5;
                var scale = this[StatusTool.ShotStatusGroupType.Total, ShotStatusTool.ShotStatusType.ShotScale, 0.5f];
                var hitCount = 16;
                var duration = 12f;
                var fadeInterval = 0.45f;
                
                switch (count)
                {
                    case 0 :
                        break;
                    case 1:
                    {
                        var spawnPos = GetTopPosition() + 50f * Vector3.up;
                        var beam = 
                            BeamPoolManager.GetInstanceUnsafe
                                .SpawnBeam
                                (
                                    _CreateParams, this,
                                    new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, new AffinePreset(spawnPos, scale)),
                                    Vector3.down, 
                                    1f, 100f, hitCount, duration, fadeInterval, 100
                                );
                        beam.Affine.SetParent(_Rotation);
                        _BeamList.Add(beam);
                        break;
                    }
                    default:
                    {
                        var theta = 360f / count;
                        var radius = 0.5f * scale;
                        for (var i = 0; i < count; i++)
                        {
                            var spawnPos = GetTopPosition() + 50f * Vector3.up + radius * Vector3.right.RotationVectorByPivot(Vector3.up, i * theta);
                            var beam = 
                                BeamPoolManager.GetInstanceUnsafe
                                    .SpawnBeam
                                    (
                                        _CreateParams, this,
                                        new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, new AffinePreset(spawnPos, scale)),
                                        Vector3.down, 
                                        1f, 100f, hitCount, duration, fadeInterval, 100
                                    );
                            beam.Affine.SetParent(_Rotation);
                            _BeamList.Add(beam);
                        }            
                        break;
                    }
                }

                foreach (var beam in _BeamList)
                {
                    beam.AddReceiver(_BeamEventReceiver);
                }
                
                return true;
            }
            else
            {
                return false;
            }
        }

        protected override void OnRetrieve(GearPoolManager.CreateParams p_CreateParams, bool p_IsPooled,
            bool p_IsDisposed)
        {
            base.OnRetrieve(p_CreateParams, p_IsPooled, p_IsDisposed);

            foreach (var beam in _BeamList)
            {
                if (beam.ContentState == PoolTool.ContentState.Active)
                {
                    beam.SetDead(false);
                }
            }
            
            _BeamEventReceiver.ClearSender();
            _BeamList.Clear();
        }

        protected override void OnDispose()
        {
            if (!ReferenceEquals(null, _BeamList))
            {
                _BeamList.Clear();
                _BeamList = default;
            }
            
            if (!ReferenceEquals(null, _BeamEventReceiver))
            {
                _BeamEventReceiver.Dispose();
                _BeamEventReceiver = default;
            }
            
            base.OnDispose();
        }

        private void OnHandleBeamEvent(GameEntityTool.GameEntityBaseEventType p_Type, GameEntityBaseEventParams p_Params)
        {
            switch (p_Type)
            {
                case GameEntityTool.GameEntityBaseEventType.Retrieved:
                {
                    _BeamList.Remove(p_Params.Trigger);
                    if (_BeamList.Count < 1)
                    {
                        SetDead(false);
                    }
                    break;
                }
            }
        }
        
        protected override void OnUpdateEntity(float p_DeltaTime)
        {
            base.OnUpdateEntity(p_DeltaTime);

            if (IsAlive)
            {
                _Rotation.Rotate(Vector3.up, 233f * p_DeltaTime, Space.Self);
            }
        }
        
        protected override void OnMasterFallen()
        {
            SetDead(true);
        }

        #endregion
    }
}