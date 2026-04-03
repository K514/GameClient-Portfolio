using System.Threading;
using Cysharp.Threading.Tasks;
using k514.Mono.Feature;
using UnityEngine;

namespace k514.Mono.Common
{
    public partial class GameEntityDeployStorage 
    {
        #region <Callbacks>

        private void OnCreateGlacierAnger()
        {
            _EntityEventTable.Add(20030000, new GlacierAnger_00());
            _EntityEventTable.Add(20030010, new GlacierAnger_01());
            _EntityEventTable.Add(20030020, new GlacierAnger_02()); 
            _EntityEventTable.Add(20030030, new GlacierAnger_03());
            _EntityEventTable.Add(20030040, new GlacierAnger_04());
            _EntityEventTable.Add(20030050, new GlacierAnger_00());
            _EntityEventTable.Add(20030058, new GlacierAnger_05_ex());
            _EntityEventTable.Add(20030059, new GlacierAnger_05_ex2());
            _EntityEventTable.Add(20030060, new GlacierAnger_06_00(3, 30f));
            _EntityEventTable.Add(20030068, new GlacierAnger_06_ex());
            _EntityEventTable.Add(20030069, new GlacierAnger_06_ex2());
            _EntityEventTable.Add(20030070, new GlacierAnger_07());
            _EntityEventTable.Add(20030080, new GlacierAnger_08());
        }

        #endregion
        
        #region <Classess>
        
        public class GlacierAnger_00 : GameEntityDeployEventBase
        {
            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                var createParams = param.GetShotCreateParams(0);
                var caster = param.Caster;
                var spawnPos = caster.GetCenterPosition();
                var shotScale = caster[StatusTool.ShotStatusGroupType.Total, ShotStatusTool.ShotStatusType.ShotScale, 1.5f];
                var affine = new AffinePreset(spawnPos, shotScale);
                var affineCorrect = new AffineCorrectionPreset(AffineTool.CorrectPositionType.CorrectSurface, affine, GameConst.Terrain_LayerMask);
                var damageRate = 0.3f;
                var moveSpeedRate = 9f;

                for (int j = 0; j < 6; j++)
                {
                    param.TryGetAutoAimDirection(EntityQueryTool.FilterResultType.RemainNearestOne, out var o_UV);
                    var shotDirection = o_UV.RotationVectorByPivot(Vector3.down, 15f * j);
                    for (int i = 0; i < 4; i++)
                    {
                        shotDirection = shotDirection.RotationVectorByPivot(Vector3.down, 90f);
                        var proj1 = ProjectilePoolManager.GetInstanceUnsafe.SpawnProjectile(createParams, caster, affineCorrect, shotDirection, moveSpeedRate, damageRate, 5f);
                        // proj1.RunAffineEvent(2001, default);
                        
                        var proj2 = ProjectilePoolManager.GetInstanceUnsafe.SpawnProjectile(createParams, caster, affineCorrect, shotDirection, moveSpeedRate, damageRate, 5f);
                        // proj2.RunAffineEvent(2002, default);
                    }
                    
                    await UniTask.Delay(150, cancellationToken: p_CancellationToken);
                }
            }
        }
        
        public class GlacierAnger_01 : GameEntityDeployEventBase
        {
            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                var createParams = param.GetShotCreateParams(0);
                param.TryGetAutoAimDirection(EntityQueryTool.FilterResultType.RemainNearestOne, out var o_UV);
                var caster = param.Caster;
                var shotScale = caster[StatusTool.ShotStatusGroupType.Total, ShotStatusTool.ShotStatusType.ShotScale, 1.5f];
                var damageRate = 0.3f;
                var moveSpeedRate = 7f;

                for (int j = 0; j < 2; j++)
                {
                    var spawnPos = caster.GetCenterPosition();
                    var affine = new AffinePreset(spawnPos, shotScale);
                    var affineCorrect = new AffineCorrectionPreset(AffineTool.CorrectPositionType.CorrectSurface, affine, GameConst.Terrain_LayerMask);
                    for (int i = 0; i < 12; i++)
                    {
                        o_UV = o_UV.RotationVectorByPivot(Vector3.down, 30f);
                        var proj1 = ProjectilePoolManager.GetInstanceUnsafe.SpawnProjectile(createParams, caster, affineCorrect, o_UV, moveSpeedRate, damageRate, 5f);
                        // proj1.RunAffineEvent(2007, default);
                    }

                    await UniTask.Delay(300, cancellationToken: p_CancellationToken);
                }
            }
        }
        
        public class GlacierAnger_02 : GameEntityDeployEventBase
        {
            private GearPoolManager.CreateParams _GearCreateParams;

            public GlacierAnger_02()
            {
                _GearCreateParams = GearPoolManager.GetInstanceUnsafe.GetCreateParams(5);
            }

            public override void PreloadDeployEvent()
            {
                base.PreloadDeployEvent();
                
                GetInstanceUnsafe.PreloadDeployEvent(31060);
            }
            
            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                var createParams = param.GetShotCreateParams(0);
                param.TryGetAutoAimDirection(EntityQueryTool.FilterResultType.RemainNearestOne, out var o_UV);
                var caster = param.Caster;
                var spawnPos = caster.GetCenterPosition();

                var shotCount = 4;
                var thetaOffset = 75f;
                var wholeTheta = (shotCount + 1) * thetaOffset;
                o_UV = o_UV.RotationVectorByPivot(Vector3.down, -0.5f * wholeTheta);
                for (int i = 0; i < shotCount; i++)
                {
                    o_UV = o_UV.RotationVectorByPivot(Vector3.down, thetaOffset);
                    var gear = GearPoolManager.GetInstanceUnsafe.SpawnGear
                    (
                        _GearCreateParams, 
                        new GearPoolManager.ActivateParams
                        (
                            null, 
                            new AffineCorrectionPreset(AffineTool.CorrectPositionType.CorrectSurface, spawnPos, GameConst.Terrain_LayerMask), 
                            GameEntityTool.ActivateParamsAttributeType.GiveFollowFallenMaster, 
                            caster, default,
                            p_Duration: 0.2f, p_Count: 6
                        )
                    );
                
                    gear.SetLookUV(o_UV);
                    gear.SwitchActionModule(ActionModuleDataTableQuery.TableLabel.Default);
                    gear.SwitchPhysicsModule(PhysicsModuleDataTableQuery.TableLabel.Affine);
                    gear.AddStatus(StatusTool.BattleStatusGroupType.ExtraAdd, BattleStatusTool.BattleStatusType.MoveSpeedBasis, 12f);
                    // gear.RunAffineEvent(2000, default);
                }
            }
        }
        
        public class GlacierAnger_03 : GameEntityDeployEventBase
        {
            private GearPoolManager.CreateParams _GearCreateParams;

            public GlacierAnger_03()
            {
                _GearCreateParams = GearPoolManager.GetInstanceUnsafe.GetCreateParams(5);
            }

            public override void PreloadDeployEvent()
            {
                base.PreloadDeployEvent();
                
                GetInstanceUnsafe.PreloadDeployEvent(30010);
            }
            
            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                var createParams = param.GetShotCreateParams(0);
                param.TryGetAutoAimDirection(EntityQueryTool.FilterResultType.RemainNearestOne, out var o_UV);
                var caster = param.Caster;
                var spawnPos = caster.GetCenterPosition();

                var shotCount = 3;
                var thetaOffset = 45f;
                var wholeTheta = (shotCount + 1) * thetaOffset;
                o_UV = o_UV.RotationVectorByPivot(Vector3.down, -0.5f * wholeTheta);
                for (int i = 0; i < shotCount; i++)
                {
                    o_UV = o_UV.RotationVectorByPivot(Vector3.down, thetaOffset);
                    var gear = GearPoolManager.GetInstanceUnsafe.SpawnGear
                    (
                        _GearCreateParams, 
                        new GearPoolManager.ActivateParams
                        (
                            null, 
                            new AffineCorrectionPreset(AffineTool.CorrectPositionType.CorrectSurface, spawnPos, GameConst.Terrain_LayerMask), 
                            GameEntityTool.ActivateParamsAttributeType.GiveFollowFallenMaster, 
                            caster, default,
                            p_Duration: 0.12f, p_Count: 5
                        )
                    );

                    gear.AddStatus(StatusTool.BattleStatusGroupType.SimpleMul, BattleStatusTool.BattleStatusType.DamageRate, -0.7f);
                    gear.SetLookUV(o_UV);
                    gear.SwitchActionModule(ActionModuleDataTableQuery.TableLabel.Default);
                    gear.SwitchPhysicsModule(PhysicsModuleDataTableQuery.TableLabel.Affine);
                    gear.AddStatus(StatusTool.BattleStatusGroupType.ExtraAdd, BattleStatusTool.BattleStatusType.MoveSpeedBasis, 11f);
                    // gear.RunAffineEvent(2000, default);
                }
            }
        }
        
        public class GlacierAnger_04 : GameEntityDeployEventBase
        {
            private GearPoolManager.CreateParams _GearCreateParams;

            public GlacierAnger_04()
            {
                _GearCreateParams = GearPoolManager.GetInstanceUnsafe.GetCreateParams(5);
            }

            public override void PreloadDeployEvent()
            {
                base.PreloadDeployEvent();
                
                GetInstanceUnsafe.PreloadDeployEvent(32020);
            }
            
            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                var createParams = param.GetShotCreateParams(0);
                param.TryGetAutoAimDirection(EntityQueryTool.FilterResultType.RemainNearestOne, out var o_UV);
                var caster = param.Caster;
                var spawnPos = caster.GetCenterPosition();

                var gear = GearPoolManager.GetInstanceUnsafe.SpawnGear
                (
                    _GearCreateParams, 
                    new GearPoolManager.ActivateParams
                    (
                        null, 
                        new AffineCorrectionPreset(AffineTool.CorrectPositionType.CorrectSurface, spawnPos, GameConst.Terrain_LayerMask), 
                        GameEntityTool.ActivateParamsAttributeType.GiveFollowFallenMaster, 
                        caster, default,
                        p_Duration: 0.08f, p_Count: 15
                    )
                );
                
                gear.SetLookUV(o_UV);
                gear.SwitchActionModule(ActionModuleDataTableQuery.TableLabel.Default);
                gear.SwitchPhysicsModule(PhysicsModuleDataTableQuery.TableLabel.Affine);
                gear.AddStatus(StatusTool.BattleStatusGroupType.ExtraAdd, BattleStatusTool.BattleStatusType.MoveSpeedBasis, 18f);
                // gear.RunAffineEvent(2000, default);
            }
        }
        
        /// <summary>
        /// 레이저 경로 표시
        /// </summary>
        public class GlacierAnger_05 : GameEntityDeployEventBase
        {
            private ProjectorPoolManager.CreateParams _ProjectorCreateParams;

            public GlacierAnger_05()
            {
                _ProjectorCreateParams = ProjectorPoolManager.GetInstanceUnsafe.GetCreateParams(4);
            }

            public override void PreloadDeployEvent()
            {
                base.PreloadDeployEvent();
                
                GetInstanceUnsafe.PreloadDeployEvent(20030058);
                GetInstanceUnsafe.PreloadDeployEvent(20030059);
            }
            
            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                param.TryGetAutoAimDirection(EntityQueryTool.FilterResultType.RemainNearestOne, out var o_UV);
                var caster = param.Caster;

                caster.AddState(GameEntityTool.EntityStateType.BLOCK_MOVE);

                ProjectorPoolManager.GetInstanceUnsafe
                    .SpawnProjector
                    (
                        _ProjectorCreateParams, caster, 
                        new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, caster.GetCenterPosition()),
                        o_UV.RotationVectorByPivot(Vector3.up, 120f), 1f, 20030058, 
                        ProjectorTool.ActivateParamsAttributeType.GiveForwardPivotAttribute, 0.5f, 12f, 0.3f, 0.8f, 0.3f
                    );
                    
                ProjectorPoolManager.GetInstanceUnsafe
                    .SpawnProjector
                    (
                        _ProjectorCreateParams, caster, 
                        new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, caster.GetCenterPosition()),
                        o_UV.RotationVectorByPivot(Vector3.down, 120f), 1f, 20030059, 
                        ProjectorTool.ActivateParamsAttributeType.GiveForwardPivotAttribute, 0.5f, 12f, 0.3f, 0.8f, 0.3f
                    );
                
                await UniTask.Delay(3500, cancellationToken: p_CancellationToken);
                caster.RemoveState(GameEntityTool.EntityStateType.BLOCK_MOVE);
            }
        }
        
        /// <summary>
        /// 궤도 레이저 생성
        /// </summary>
        public class GlacierAnger_05_ex : GameEntityDeployEventBase
        {
            private BeamPoolManager.CreateParams _BeamCreateParams;

            public GlacierAnger_05_ex()
            {
                _BeamCreateParams = BeamPoolManager.GetInstanceUnsafe.GetCreateParams(2);
            }

            public override void PreloadDeployEvent()
            {
                base.PreloadDeployEvent();
                
            }
            
            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                var caster = param.Caster;
                var spawnPos = caster.GetCenterPosition();
                var affineCorrect = new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, new AffinePreset(spawnPos, 3f));

                var beam = BeamPoolManager.GetInstanceUnsafe.SpawnBeam(_BeamCreateParams, caster, affineCorrect, caster.GetLookUV(), 1f, 12f, 30, 3f, 0.2f);
                await UniTask.Delay(1000, cancellationToken: p_CancellationToken);
                // beam.RunAffineEvent(1003, default);
            }
        }
        
        public class GlacierAnger_05_ex2 : GameEntityDeployEventBase
        {
            private BeamPoolManager.CreateParams _BeamCreateParams;

            public GlacierAnger_05_ex2()
            {
                _BeamCreateParams = BeamPoolManager.GetInstanceUnsafe.GetCreateParams(2);
            }

            public override void PreloadDeployEvent()
            {
                base.PreloadDeployEvent();
                
            }
            
            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                var caster = param.Caster;
                var spawnPos = caster.GetCenterPosition();
                var affineCorrect = new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, new AffinePreset(spawnPos, 3f));

                var beam = BeamPoolManager.GetInstanceUnsafe.SpawnBeam(_BeamCreateParams, caster, affineCorrect, caster.GetLookUV(), 1f, 12f, 30, 3f, 0.2f);
                await UniTask.Delay(1000, cancellationToken: p_CancellationToken);
                // beam.RunAffineEvent(1004, default);
            }
        }
        
        /// <summary>
        /// 아이스 브레스 장판
        /// </summary>
        public class GlacierAnger_06_00 : GameEntityDeployEventBase
        {
            private ProjectorPoolManager.CreateParams _ProjectorCreateParams;
            private int _ShotCount;
            private float _Theta;
            
            public GlacierAnger_06_00(int p_ShotCount, float p_Theta)
            {
                _ProjectorCreateParams = ProjectorPoolManager.GetInstanceUnsafe.GetCreateParams(4);
                _ShotCount = p_ShotCount;
                _Theta = p_Theta;
            }

            public override void PreloadDeployEvent()
            {
                base.PreloadDeployEvent();
                
                GetInstanceUnsafe.PreloadDeployEvent(20030068);
            }
            
            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                param.TryGetAutoAimDirection(EntityQueryTool.FilterResultType.RemainNearestOne, out var o_UV);
                var caster = param.Caster;
                var projectDuration = 2f;
                
                var theta = _Theta / (_ShotCount - 1);

                o_UV = o_UV.RotationVectorByPivot(Vector3.up, 0.5f * _Theta);
                for (var i = 0; i < _ShotCount; i++)
                {
                    caster.AddState(GameEntityTool.EntityStateType.BLOCK_MOVE);
                    ProjectorPoolManager.GetInstanceUnsafe.SpawnProjector(_ProjectorCreateParams, caster, new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, caster.GetCenterPosition()), o_UV, 1f, 20030068, ProjectorTool.ActivateParamsAttributeType.GiveForwardPivotAttribute, 3.6f, 20f, 0.2f, projectDuration, 0.05f);

                    o_UV = o_UV.RotationVectorByPivot(Vector3.up, -theta);
                }
            }
        }
        
        /// <summary>
        /// 히트 필드 기어 생성
        /// </summary>
        public class GlacierAnger_06_ex : GameEntityDeployEventBase
        {
            private GearPoolManager.CreateParams _GearCreateParams;
            private VfxPoolManager.CreateParams _EffectCreateParams;

            public GlacierAnger_06_ex()
            {
                _GearCreateParams = GearPoolManager.GetInstanceUnsafe.GetCreateParams(13);
                _EffectCreateParams = VfxPoolManager.GetInstanceUnsafe.GetCreateParams(44);
            }

            public override void PreloadDeployEvent()
            {
                base.PreloadDeployEvent();
                
                GetInstanceUnsafe.PreloadDeployEvent(20030069);
            }
            
            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                var caster = param.Caster;
                var filterSpace = caster.GetSightRangeFilterConfig();

                var vfx = VfxPoolManager.GetInstanceUnsafe
                    .Pop
                    (
                        _EffectCreateParams, 
                        new VfxPoolManager.ActivateParams
                        (
                            null, 
                            new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, new AffinePreset(caster.GetCenterPosition(), caster.GetRotation(), 7f)) 
                        )
                    );
                
                GearPoolManager.GetInstanceUnsafe.SpawnGear
                (
                    _GearCreateParams,
                    new GearPoolManager.ActivateParams
                    (
                        null,
                        new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, new AffinePreset(caster.GetBottomPosition(), caster.GetRotation())),
                        GameEntityTool.ActivateParamsAttributeType.GiveFollowFallenMaster,
                        caster, default, p_Duration: vfx.GetLiveSpan(), p_FilterSpace: filterSpace
                    )
                );
            }
        }
        
        /// <summary>
        /// 아이스 브레스 종료
        /// </summary>
        public class GlacierAnger_06_ex2 : GameEntityDeployEventBase
        {
            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                var caster = param.Caster;
                
                if (caster.TryGetMaster(out var o_Master))
                {
                    o_Master.RemoveState(GameEntityTool.EntityStateType.BLOCK_MOVE);
                }
            }
        }
        
        public class GlacierAnger_07 : GameEntityDeployEventBase
        {
            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                var createParams = param.GetShotCreateParams(0);
                var caster = param.Caster;
                var spawnPos = caster.GetCenterPosition();
                var shotScale = caster[StatusTool.ShotStatusGroupType.Total, ShotStatusTool.ShotStatusType.ShotScale, 1.5f];
                var affine = new AffinePreset(spawnPos, shotScale);
                var affineCorrect = new AffineCorrectionPreset(AffineTool.CorrectPositionType.CorrectSurface, affine, GameConst.Terrain_LayerMask);
                var damageRate = 0.3f;
                var moveSpeedRate = 20f;
                var duration = 10f;
                var shotDirection = caster.GetLookUV();
                
                for (var j = 0; j < 25; j++)
                {
                    for (var i = 0; i < 4; i++)
                    {
                        shotDirection = shotDirection.RotationVectorByPivot(Vector3.down, Random.Range(0f, 360f));
                        var proj = ProjectilePoolManager.GetInstanceUnsafe.SpawnProjectile(createParams, caster, affineCorrect, shotDirection, moveSpeedRate, damageRate, duration);
                        // proj.RunAffineEvent(2006, default);
                    }

                    await UniTask.Delay(Random.Range(25, 150), cancellationToken: p_CancellationToken);
                }
            }
        }
        
        public class GlacierAnger_08 : GameEntityDeployEventBase
        {
            private UnitPoolManager.CreateParams _UnitCreateParams;
            private VfxPoolManager.CreateParams _EffectCreateParams;

            public GlacierAnger_08()
            {
                _UnitCreateParams = UnitPoolManager.GetInstanceUnsafe.GetCreateParams(4016);
                _EffectCreateParams = VfxPoolManager.GetInstanceUnsafe.GetCreateParams(81);
            }

            public override void PreloadDeployEvent()
            {
                base.PreloadDeployEvent();
                
                GetInstanceUnsafe.PreloadDeployEvent(20030000);
            }
            
            /*public override bool IsRunnable(GameEntityDeployEventContainer p_Handler)
            {
                var caster = p_Handler.Caster;
                return caster.GetCurrentStatusRate(BattleStatusTool.BattleStatusType.HP_Base) < 0.25f;
            }*/

            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                var caster = param.Caster;

                if (caster.GetCurrentStatusRate(BattleStatusTool.BattleStatusType.HP_Base) > 0.25f)
                {
                    await GetInstanceUnsafe._EntityEventTable[20030000].RunEvent(p_Handler, p_CancellationToken);
                    return;
                }

                var cnt = 5;
                
                for (var i = 0; i < cnt; i++)
                {
                    await UniTask.Delay(100, cancellationToken: p_CancellationToken);
                    var spawnPosition = caster.GetBottomPosition().GetRandomPosition(XYZType.ZX, 2f, 5f);
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
                                        new AffinePreset(spawnPosition, Quaternion.identity), 
                                        GameConst.Terrain_LayerMask
                                    ),
                                    p_GameEntityActivateParamsAttributeMask: GameEntityTool.ActivateParamsAttributeType.GiveFollowFallenMaster, caster
                                )
                            );
                                 
                    // entity.SwitchPersona(MindModuleDataTableQuery.TableLabel.Following); 
                    entity.SetLifeSpan(20f, 1f);
                    entity.SetGroupMask(caster.GetAllyMask(), caster.GetEnemyMask());

                    var activateParams = new VfxPoolManager.ActivateParams(null, new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, new AffinePreset(entity.GetBottomPosition(), 2f * entity.Scale)));
                    VfxPoolManager.GetInstanceUnsafe.Pop(_EffectCreateParams, activateParams);
                }
            }
        }
        
        #endregion
    }
}