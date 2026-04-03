using System.Threading;
using Cysharp.Threading.Tasks;
using k514.Mono.Feature;
using UnityEngine;

namespace k514.Mono.Common
{
    public partial class GameEntityDeployStorage 
    {
        #region <Callbacks>

        private void OnCreateDarkInvader()
        {
            _EntityEventTable.Add(20040000, new DarkInvader_00());
            _EntityEventTable.Add(20040010, new DarkInvader_01());
            _EntityEventTable.Add(20040020, new DarkInvader_02_00());
            _EntityEventTable.Add(20040021, new DarkInvader_02_01());
            _EntityEventTable.Add(20040029, new DarkInvader_02_ex());
            _EntityEventTable.Add(20040030, new DarkInvader_03_00());
            _EntityEventTable.Add(20040031, new DarkInvader_03_01());
            _EntityEventTable.Add(20040032, new DarkInvader_03_02());
            _EntityEventTable.Add(20040039, new DarkInvader_03_ex());
            _EntityEventTable.Add(20040040, new DarkInvader_04());
            _EntityEventTable.Add(20040049, new DarkInvader_04_ex());
            _EntityEventTable.Add(20040050, new DarkInvader_05());
        }

        #endregion
        
        #region <Classess>
        
        /// <summary>
        /// 전방으로 난사 2회 + 전방위 1회
        /// </summary>
        public class DarkInvader_00 : GameEntityDeployEventBase
        {
            private const float __DamageRate = 0.3f;
            private const float __ShotScale = 2f;
            private const float __ShotSpeed = 4f;
            private const float __ShotLifeSpan = 15f;
            private const float __ShotDegree = 30f;
            private const int __ShotWaveFirstStartFactor = 3;
            private const int __ShotWaveFirstFactor = 2;
            private const int __ShotWaveSecondStartFactor = 6;
            private const int __ShotWaveSecondFactor = 2;
            private const int __ShotWaveThirdFactor = 8;
            private const int __ShotWaveThirdStartFactor = 1;
            private const int __WaveInterval = 100;

            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                var createParams = param.GetShotCreateParams(0);
                param.TryGetAutoAimDirection(EntityQueryTool.FilterResultType.RemainNearestOne, out var o_UV);
                
                var caster = param.Caster;
                var spawnPos = caster.GetCenterPosition();
                var shotScale = caster[StatusTool.ShotStatusGroupType.Total, ShotStatusTool.ShotStatusType.ShotScale, __ShotScale];
                var affine = new AffinePreset(spawnPos, shotScale);
                var affineCorrect = new AffineCorrectionPreset(AffineTool.CorrectPositionType.CorrectSurface, affine, GameConst.Terrain_LayerMask);
         
                /*ProjectilePoolManager.GetInstanceUnsafe.SpawnSpreadProjectile(createParams, caster, affineCorrect, o_UV, __ShotSpeed, __DamageRate, __ShotLifeSpan, __ShotDegree, shotCount1);
                await UniTask.Delay(__WaveInterval, cancellationToken: p_CancellationToken);
                ProjectilePoolManager.GetInstanceUnsafe.SpawnSpreadProjectile(createParams, caster, affineCorrect, o_UV, __ShotSpeed, __DamageRate, __ShotLifeSpan, __ShotDegree, shotCount2);
                await UniTask.Delay(__WaveInterval, cancellationToken: p_CancellationToken);
                ProjectilePoolManager.GetInstanceUnsafe.SpawnRoundProjectile(createParams, caster, affineCorrect, o_UV, __ShotSpeed, __DamageRate, __ShotLifeSpan, shotCount3);*/
            }
        }
        
        /// <summary>
        /// 전방으로 난사 2회
        /// </summary>
        public class DarkInvader_01 : GameEntityDeployEventBase
        {
            private const float __DamageRate = 0.3f;
            private const float __ShotScale = 1f;
            private const float __ShotSpeed = 5f;
            private const float __ShotLifeSpan = 15f;
            
            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                var createParams = param.GetShotCreateParams(0);
                param.TryGetAutoAimDirection(EntityQueryTool.FilterResultType.RemainNearestOne, out var o_UV);

                var caster = param.Caster;
                var spawnPos = caster.GetCenterPosition();
                var shotScale = caster[StatusTool.ShotStatusGroupType.Total, ShotStatusTool.ShotStatusType.ShotScale, __ShotScale];
                var affine = new AffinePreset(spawnPos, shotScale);
                var affineCorrect = new AffineCorrectionPreset(AffineTool.CorrectPositionType.CorrectSurface, affine, GameConst.Terrain_LayerMask);
                var shotDirection1 = o_UV;
                var shotDirection2 = o_UV;
                var moveSpeedRate = __ShotSpeed;
                var shotCount = 6;
                
                for (int i = 0; i < shotCount; i++)
                {
                    await UniTask.Delay(100, cancellationToken: p_CancellationToken);
                    moveSpeedRate *= 1.05f;
                    shotDirection1 = shotDirection1.RotationVectorByPivot(Vector3.down, 13f + 0.6f * i);
                    ProjectilePoolManager.GetInstanceUnsafe.SpawnSpreadProjectile(createParams, caster, affineCorrect, shotDirection1, moveSpeedRate, __DamageRate, __ShotLifeSpan, 30f, 3, 2002);
                    shotDirection2 = shotDirection2.RotationVectorByPivot(Vector3.down, -13f - 0.6f * i);
                    ProjectilePoolManager.GetInstanceUnsafe.SpawnSpreadProjectile(createParams, caster, affineCorrect, shotDirection2, moveSpeedRate, __DamageRate, __ShotLifeSpan, 30f, 3, 2001);
                }
            }
        }
        
        /// <summary>
        /// 원형 장판 생성
        /// </summary>
        public class DarkInvader_02_00 : GameEntityDeployEventBase
        {
            private const float __DamageRate = 1f;
            private const float __ProjectScale = 26f;
            private const float __ProjectDuration = 4f;
            
            private VfxPoolManager.CreateParams _EffectCreateParams;
            private ProjectorPoolManager.CreateParams _ProjectorCreateParams;

            public DarkInvader_02_00()
            {
                _EffectCreateParams = VfxPoolManager.GetInstanceUnsafe.GetCreateParams(1000);
                _ProjectorCreateParams = ProjectorPoolManager.GetInstanceUnsafe.GetCreateParams(1);
            }

            public override void PreloadDeployEvent()
            {
                base.PreloadDeployEvent();
                
                GetInstanceUnsafe.PreloadDeployEvent(20040029);
            }

            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                var isAimed = param.TryGetAutoAimDirection(EntityQueryTool.FilterResultType.RemainNearestOne, out var o_UV, out var o_Enemy);
                var caster = param.Caster;
                var spawnPos = isAimed
                    ? o_Enemy.GetBottomPosition()
                    : caster.GetBottomPosition() + caster.GetRadius(2f) * o_UV;
                
                VfxPoolManager.GetInstanceUnsafe.Pop(_EffectCreateParams, new VfxPoolManager.ActivateParams(null, new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, new AffinePreset(spawnPos, __ProjectScale)), GameEntityTool.ActivateParamsAttributeType.None, null, default));
                ProjectorPoolManager.GetInstanceUnsafe.SpawnProjector(_ProjectorCreateParams, caster, new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, spawnPos), caster.GetLookUV(), __DamageRate, 20040029, ProjectorTool.ActivateParamsAttributeType.None, __ProjectScale, __ProjectScale, 0.2f, __ProjectDuration, 0.05f);
            }
        }
        
        /// <summary>
        /// 원형 장판 여러개 생성
        /// </summary>
        public class DarkInvader_02_01 : GameEntityDeployEventBase
        {
            private const float __ProjectDuration = 4f;
                        
            private VfxPoolManager.CreateParams _EffectCreateParams;
            private ProjectorPoolManager.CreateParams _ProjectorCreateParams;

            public DarkInvader_02_01()
            {
                _EffectCreateParams = VfxPoolManager.GetInstanceUnsafe.GetCreateParams(1000);
                _ProjectorCreateParams = ProjectorPoolManager.GetInstanceUnsafe.GetCreateParams(1);
            }

            public override void PreloadDeployEvent()
            {
                base.PreloadDeployEvent();
                
                GetInstanceUnsafe.PreloadDeployEvent(20040029);
            }
            
            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                var caster = param.Caster;
                var shotCount = 8;

                for (var i = 0; i < shotCount; i++)
                {
                    await UniTask.Delay(100, cancellationToken: p_CancellationToken);
                  
                    var spawnPos = caster.TryUpdateAndGetCurrentEnemy(out var o_Enemy)
                        ? o_Enemy.GetBottomPosition() 
                        : caster.GetBottomPosition();
                    spawnPos = spawnPos.GetRandomPosition(XYZType.ZX, 2f, 10f + 2f);
                    var scale = (6f + 0.5f) * Random.Range(1f, 1.5f);
                    VfxPoolManager.GetInstanceUnsafe.Pop(_EffectCreateParams, new VfxPoolManager.ActivateParams(null, new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, new AffinePreset(spawnPos, scale)), GameEntityTool.ActivateParamsAttributeType.None, null, default));
                    ProjectorPoolManager.GetInstanceUnsafe.SpawnProjector(_ProjectorCreateParams, caster, new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, spawnPos), caster.GetLookUV(), 1f, 20040029, ProjectorTool.ActivateParamsAttributeType.None, scale, scale, 0.2f, __ProjectDuration, 0.05f);
                }
            }
        }
        
        /// <summary>
        /// 원형 장판 범위 내 적 출혈 + 실명
        /// </summary>
        public class DarkInvader_02_ex : GameEntityDeployEventBase
        {
            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                var caster = param.Caster;
                
                if (caster.FilterFocusEntityWithSightRange())
                {
                    var filterGroupSet = caster.FilterResultGroup;
                    foreach (var filtered in filterGroupSet)
                    {
                        var entityType = filtered.WorldObjectType;
                        switch (entityType)
                        {
                            default:
                            {
                                CameraManager.GetInstanceUnsafe.SetShake(Vector3.right, 7f, 0, 150, 3);
                                filtered.GiveDamage(caster[StatusTool.BattleStatusGroupType.Total].GetScaledAttackBase(DamageCalculator.DamageCalcType.Melee), new StatusTool.StatusChangeParams(StatusTool.StatusChangeEventType.Combat, caster));
                                // filtered.Enchant(13000002, new GameEntityEventCommonParams(caster));
                                // filtered.Enchant(19000003, new GameEntityEventCommonParams(caster));
                                break;
                            }
                        }
                    }
                }
            }
        }
        
        /// <summary>
        /// 좌상향 사각 장판 3줄 생성
        /// </summary>
        public class DarkInvader_03_00 : GameEntityDeployEventBase
        {
            private const float __DamageRate = 0.3f;
            private const float __ProjectDuration = 0.5f;
            
            private ProjectorPoolManager.CreateParams _ProjectorCreateParams;

            public DarkInvader_03_00()
            {
                _ProjectorCreateParams = ProjectorPoolManager.GetInstanceUnsafe.GetCreateParams(4);
            }

            public override void PreloadDeployEvent()
            {
                base.PreloadDeployEvent();
                
                GetInstanceUnsafe.PreloadDeployEvent(20040039);
            }
            
            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                var caster = param.Caster;
                var isAimed = param.TryGetAutoAimDirection(EntityQueryTool.FilterResultType.RemainNearestOne, out var o_UV, out var o_Enemy);
                var spawnPos = isAimed
                    ? o_Enemy.GetBottomPosition()
                    : caster.GetRelativePosition(caster.Scale);
                var shotDirection = o_UV.RotationVectorByPivot(Vector3.down, Random.Range(-45f, 0f));
                var perpDirection = shotDirection.GetXZPerpendicularUnitVector();

                ProjectorPoolManager.GetInstanceUnsafe.SpawnProjector(_ProjectorCreateParams, caster, new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, spawnPos + perpDirection), shotDirection, __DamageRate, 20040039, ProjectorTool.ActivateParamsAttributeType.None, 0.5f, 11f, 0.2f, __ProjectDuration, 0.05f);
                ProjectorPoolManager.GetInstanceUnsafe.SpawnProjector(_ProjectorCreateParams, caster, new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, spawnPos), shotDirection, __DamageRate, 20040039, ProjectorTool.ActivateParamsAttributeType.None, 0.5f, 11f, 0.2f, __ProjectDuration, 0.05f);
                ProjectorPoolManager.GetInstanceUnsafe.SpawnProjector(_ProjectorCreateParams, caster, new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, spawnPos - perpDirection), shotDirection, __DamageRate, 20040039, ProjectorTool.ActivateParamsAttributeType.None, 0.5f, 11f, 0.2f, __ProjectDuration, 0.05f);
            }
        }
        
        /// <summary>
        /// 우상향 사각 장판 3줄 생성
        /// </summary>
        public class DarkInvader_03_01 : GameEntityDeployEventBase
        {
            private const float __DamageRate = 0.3f;
            private const float __ProjectDuration = 0.5f;
            
            private ProjectorPoolManager.CreateParams _ProjectorCreateParams;

            public DarkInvader_03_01()
            {
                _ProjectorCreateParams = ProjectorPoolManager.GetInstanceUnsafe.GetCreateParams(4);
            }

            public override void PreloadDeployEvent()
            {
                base.PreloadDeployEvent();
                
                GetInstanceUnsafe.PreloadDeployEvent(20040039);
            }
            
            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                var caster = param.Caster;
                var isAimed = param.TryGetAutoAimDirection(EntityQueryTool.FilterResultType.RemainNearestOne, out var o_UV, out var o_Enemy);
                var spawnPos = isAimed
                    ? o_Enemy.GetBottomPosition()
                    : caster.GetRelativePosition(caster.Scale);
                var shotDirection = o_UV.RotationVectorByPivot(Vector3.down, Random.Range(0f, 45f));
                var perpDirection = shotDirection.GetXZPerpendicularUnitVector();

                ProjectorPoolManager.GetInstanceUnsafe.SpawnProjector(_ProjectorCreateParams, caster, new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, spawnPos + perpDirection), shotDirection, __DamageRate, 20040039, ProjectorTool.ActivateParamsAttributeType.None, 0.5f, 11f, 0.2f, __ProjectDuration, 0.05f);
                ProjectorPoolManager.GetInstanceUnsafe.SpawnProjector(_ProjectorCreateParams, caster, new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, spawnPos), shotDirection, __DamageRate, 20040039, ProjectorTool.ActivateParamsAttributeType.None, 0.5f, 11f, 0.2f, __ProjectDuration, 0.05f);
                ProjectorPoolManager.GetInstanceUnsafe.SpawnProjector(_ProjectorCreateParams, caster, new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, spawnPos - perpDirection), shotDirection, __DamageRate, 20040039, ProjectorTool.ActivateParamsAttributeType.None, 0.5f, 11f, 0.2f, __ProjectDuration, 0.05f);
            }
        }
        
        /// <summary>
        /// [사각 장판 3줄] 여러번 생성
        /// </summary>
        public class DarkInvader_03_02 : GameEntityDeployEventBase
        {
            private const float __DamageRate = 0.3f;
            private const float __ProjectDuration = 0.5f;
            
            private ProjectorPoolManager.CreateParams _ProjectorCreateParams;

            public DarkInvader_03_02()
            {
                _ProjectorCreateParams = ProjectorPoolManager.GetInstanceUnsafe.GetCreateParams(4);
            }

            public override void PreloadDeployEvent()
            {
                base.PreloadDeployEvent();
                
                GetInstanceUnsafe.PreloadDeployEvent(20040039);
            }
            
            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                var caster = param.Caster;
                var isAimed = param.TryGetAutoAimDirection(EntityQueryTool.FilterResultType.RemainNearestOne, out var o_UV, out var o_Enemy);
                var pivotPos = isAimed
                    ? o_Enemy.GetBottomPosition()
                    : caster.GetRelativePosition(caster.Scale);
                var waveFactor = 2;
                var shotCount = 3 + waveFactor;

                for (var i = 0; i < shotCount; i++)
                {
                    await UniTask.Delay(100, cancellationToken: p_CancellationToken);
              
                    var spawnPos = pivotPos.GetRandomPosition(XYZType.ZX, 1f, 5f);
                    var shotDirection = spawnPos.GetDirectionUnitVectorTo(pivotPos);
                    var perpDirection = shotDirection.GetXZPerpendicularUnitVector();
                
                    ProjectorPoolManager.GetInstanceUnsafe.SpawnProjector(_ProjectorCreateParams, caster, new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, spawnPos + perpDirection), shotDirection, __DamageRate, 20040039, ProjectorTool.ActivateParamsAttributeType.None, 0.5f, 11f, 0.2f, __ProjectDuration, 0.05f);
                    ProjectorPoolManager.GetInstanceUnsafe.SpawnProjector(_ProjectorCreateParams, caster, new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, spawnPos), shotDirection, __DamageRate, 20040039, ProjectorTool.ActivateParamsAttributeType.None, 0.5f, 11f, 0.2f, __ProjectDuration, 0.05f);
                    ProjectorPoolManager.GetInstanceUnsafe.SpawnProjector(_ProjectorCreateParams, caster, new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, spawnPos - perpDirection), shotDirection, __DamageRate, 20040039, ProjectorTool.ActivateParamsAttributeType.None, 0.5f, 11f, 0.2f, __ProjectDuration, 0.05f);
                }
            }
        }
        
        /// <summary>
        /// 사각 장판 내의 적 출혈 + 실명
        /// </summary>
        public class DarkInvader_03_ex : GameEntityDeployEventBase
        {
            private VfxPoolManager.CreateParams _EffectCreateParams;

            public DarkInvader_03_ex()
            {
                _EffectCreateParams = VfxPoolManager.GetInstanceUnsafe.GetCreateParams(121);
            }

            public override void PreloadDeployEvent()
            {
                base.PreloadDeployEvent();
                
            }
            
            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                await UniTask.Delay(100, cancellationToken: p_CancellationToken);
     
                var param = p_Handler.DeployParams;
                var caster = param.Caster;
                var vfx = VfxPoolManager.GetInstanceUnsafe.Pop(_EffectCreateParams, new VfxPoolManager.ActivateParams(null, new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, new AffinePreset(caster.GetCenterPosition(), 1.5f)), GameEntityTool.ActivateParamsAttributeType.None, null, default));
                vfx.SetLookUV(caster.GetLookUV());
                
                if (caster.FilterFocusEntityWithSightRange())
                {
                    var filterGroupSet = caster.FilterResultGroup;
                    foreach (var filtered in filterGroupSet)
                    {
                        var entityType = filtered.WorldObjectType;
                        switch (entityType)
                        {
                            default:
                            {
                                CameraManager.GetInstanceUnsafe.SetShake(Vector3.right, 7f, 0, 150, 3);
                                filtered.GiveDamage(caster[StatusTool.BattleStatusGroupType.Total].GetScaledAttackBase(DamageCalculator.DamageCalcType.Melee), new StatusTool.StatusChangeParams(StatusTool.StatusChangeEventType.Combat, caster));
                                // filtered.Enchant(13000002, new GameEntityEventCommonParams(caster));
                                // filtered.Enchant(19000002, new GameEntityEventCommonParams(caster));
                                break;
                            }
                        }
                    }
                }
            }
        }
        
        /// <summary>
        /// 사각 장판 생성
        /// </summary>
        public class DarkInvader_04 : GameEntityDeployEventBase
        {
            private const float __ProjectDuration = 0.5f;
            
            private ProjectorPoolManager.CreateParams _ProjectorCreateParams;

            public DarkInvader_04()
            {
                _ProjectorCreateParams = ProjectorPoolManager.GetInstanceUnsafe.GetCreateParams(4);
            }

            public override void PreloadDeployEvent()
            {
                base.PreloadDeployEvent();
                
                GetInstanceUnsafe.PreloadDeployEvent(20040049);
            }
            
            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                var caster = param.Caster;
                param.TryGetAutoAimDirection(EntityQueryTool.FilterResultType.RemainNearestOne, out var o_UV);
                var spawnPos = caster.GetBottomPosition();
                caster.AddState(GameEntityTool.EntityStateType.BLOCK_MOVE);
                ProjectorPoolManager.GetInstanceUnsafe.SpawnProjector(_ProjectorCreateParams, caster, new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, spawnPos), o_UV, 1f, 20040049, ProjectorTool.ActivateParamsAttributeType.GiveForwardPivotAttribute, 5f, 18f, 0.2f, __ProjectDuration, 0.05f);
            }
        }
        
        /// <summary>
        /// 사각 장판 내의 적 출혈 + 실명
        ///
        /// 적이 있었을 경우, 체력 10% 회복
        /// </summary>
        public class DarkInvader_04_ex : GameEntityDeployEventBase
        {
            private VfxPoolManager.CreateParams _EffectCreateParams;

            public DarkInvader_04_ex()
            {
                _EffectCreateParams = VfxPoolManager.GetInstanceUnsafe.GetCreateParams(121);
            }

            public override void PreloadDeployEvent()
            {
                base.PreloadDeployEvent();
                
            }
            
            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                /*if (p_Handler.Caster is ProjectorObjectBase o_Projector)
                {
                    var masterValid = o_Projector.TryGetMaster(out var o_Master);
                    if (masterValid)
                    {
                        o_Master.PhysicsModule.AddVelocity(PhysicsTool.ForceType.Default, 250f * o_Master.GetLookUV());
                        o_Master.RemoveState(GameEntityTool.EntityStateType.BLOCK_MOVE);
                    }
            
                    var vfx = VfxPoolManager.GetInstanceUnsafe.Pop(_EffectCreateParams, new VfxPoolManager.ActivateParams(null, new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, new AffinePreset(o_Projector.GetCenterPositionWithOffset(), 4f)), GameEntityTool.ActivateParamsAttributeType.None, null, default));
                    vfx.SetLookUV(o_Projector.GetLookUV());
                    
                    if (o_Projector.FilterFocusEntityWithSightRange())
                    {
                        var filterGroupSet = o_Projector.FilterResultGroup;
                        foreach (var filtered in filterGroupSet)
                        {
                            var entityType = filtered.WorldObjectType;
                            switch (entityType)
                            {

                                default:
                                {
                                    CameraManager.GetInstanceUnsafe.SetShake(Vector3.right, 7f, 0, 150, 3);
                                    filtered.GiveDamage(o_Projector[StatusTool.BattleStatusGroupType.Total].GetScaledAttackBase(DamageCalculator.DamageCalcType.Melee), new StatusTool.StatusChangeParams(StatusTool.StatusChangeEventType.Combat, o_Projector));
                                    // filtered.Enchant(13000002, new GameEntityEventCommonParams(o_Projector));
                                    // filtered.Enchant(19000002, new GameEntityEventCommonParams(o_Projector));

                                    if (masterValid)
                                    {
                                        o_Master.HealRateHP(0.1f);
                                    }
                                    
                                    var uv = o_Projector.GetCenterPosition().GetDirectionVectorTo(filtered).XZUVector();
                                    filtered.PhysicsModule.AddVelocity(PhysicsTool.ForceType.Default, 50f * uv);
                                    break;
                                }
                            }
                        }
                    }
                }*/
            }
        }
        
        /// <summary>
        /// 체력이 50% 이하일 때, 자신과 동일한 패턴을 사용하는 분신 생성
        /// 1. 분신은 20초간 활동
        /// 2. 분신은 현재 자신의 체력을 가지고 생성
        /// 3. 체력 외의 모든 스탯은 원본 스탯을 그대로 따라감
        /// </summary>
        public class DarkInvader_05 : GameEntityDeployEventBase
        {
            private UnitPoolManager.CreateParams _UnitCreateParams;
            private VfxPoolManager.CreateParams _EffectCreateParams;

            public DarkInvader_05()
            {
                _UnitCreateParams = UnitPoolManager.GetInstanceUnsafe.GetCreateParams(12101);
                _EffectCreateParams = VfxPoolManager.GetInstanceUnsafe.GetCreateParams(VfxTool.__MonsterSpawnVfxIndex);
            }

            public override void PreloadDeployEvent()
            {
                base.PreloadDeployEvent();
                
                UnitPoolManager.GetInstanceUnsafe.Preload(_UnitCreateParams, 2);
                GetInstanceUnsafe.PreloadDeployEvent(20040000);
            }
            
            /*public override bool IsRunnable(GameEntityDeployEventContainer p_Handler)
            {
                var caster = p_Handler.Caster;
                return caster.GetCurrentStatusRate(BattleStatusTool.BattleStatusType.HP_Base) < 0.25f;
            }*/

            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var caster = p_Handler.Caster;

                if (caster.GetCurrentStatusRate(BattleStatusTool.BattleStatusType.HP_Base) > 0.25f)
                {
                    await GetInstanceUnsafe._EntityEventTable[20040000].RunEvent(p_Handler, p_CancellationToken);
                    return;
                }
                
                var entity = 
                    UnitPoolManager.GetInstanceUnsafe
                        .Pop
                        (
                            _UnitCreateParams, 
                            new UnitPoolManager.ActivateParams
                            (
                        null, 
                                new AffineCorrectionPreset
                                (
                                    AffineTool.CorrectPositionType.ForceSurface, 
                                    new AffinePreset(caster.GetBottomPosition(), Quaternion.identity), 
                                    GameConst.Terrain_LayerMask
                                ),
                                p_GameEntityActivateParamsAttributeMask: GameEntityTool.ActivateParamsAttributeType.GiveFollowFallenMaster, caster
                            )
                        );
                             
                // entity.SwitchPersona(MindModuleDataTableQuery.TableLabel.Following); 
                entity.SetLifeSpan(20f, 1f);
                entity.SetGroupMask(caster.GetAllyMask(), caster.GetEnemyMask());

                var activateParams = new VfxPoolManager.ActivateParams(null, new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, new AffinePreset(entity.GetBottomPosition(), entity.Scale)));
                VfxPoolManager.GetInstanceUnsafe.Pop(_EffectCreateParams, activateParams);
                
                entity.SetStatus(StatusTool.BattleStatusGroupType.Current, BattleStatusTool.BattleStatusType.HP_Base, caster[StatusTool.BattleStatusGroupType.Current][BattleStatusTool.BattleStatusType.HP_Base]);
            }
        }
        
        #endregion
    }
}