using System.Threading;
using Cysharp.Threading.Tasks;
using k514.Mono.Feature;
using UnityEngine;

namespace k514.Mono.Common
{
    public partial class GameEntityDeployStorage 
    {
        #region <Callbacks>

        private void OnCreateDarkWing()
        {
            _EntityEventTable.Add(20020000, new DarkWing_00());
            _EntityEventTable.Add(20020010, new DarkWing_01());
            _EntityEventTable.Add(20020020, new DarkWing_02());
            _EntityEventTable.Add(20020030, new DarkWing_03_00());
            _EntityEventTable.Add(20020031, new DarkWing_03_01());
            _EntityEventTable.Add(20020032, new DarkWing_03_02());
            _EntityEventTable.Add(20020039, new DarkWing_03_ex());
            _EntityEventTable.Add(20020040, new DarkWing_04());
            _EntityEventTable.Add(20020049, new DarkWing_04_ex());
            _EntityEventTable.Add(20020050, new DarkWing_05());
            _EntityEventTable.Add(20020060, new DarkWing_06());
            _EntityEventTable.Add(20020069, new DarkWing_06_ex());
        }

        #endregion
        
        #region <Classess>
        
        public class DarkWing_00 : GameEntityDeployEventBase
        {
            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                var createParams = param.GetShotCreateParams(0);
                param.TryGetAutoAimDirection(EntityQueryTool.FilterResultType.RemainNearestOne, out var o_UV);
                var caster = param.Caster;
                var shotScale = caster[StatusTool.ShotStatusGroupType.Total][ShotStatusTool.ShotStatusType.ShotScale];
                var damageRate = 0.3f;
                var moveSpeedRate = 10f;

                for (var i = 3; i < 8; i++)
                {
                    var spawnPos = caster.GetCenterPosition();
                    var affine = new AffinePreset(spawnPos, shotScale);
                    var affineCorrect = new AffineCorrectionPreset(AffineTool.CorrectPositionType.CorrectSurface, affine, GameConst.Terrain_LayerMask);
           
                    ProjectilePoolManager.GetInstanceUnsafe.SpawnSpreadProjectile(createParams, caster, affineCorrect, o_UV, moveSpeedRate, damageRate, 10f, 30f + 8f * i, i);

                    await UniTask.Delay(150, cancellationToken: p_CancellationToken);
                }
            }
        }
        
        public class DarkWing_01 : GameEntityDeployEventBase
        {
            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                var createParams = param.GetShotCreateParams(1);
                param.TryGetAutoAimDirection(EntityQueryTool.FilterResultType.RemainNearestOne, out var o_UV);
                var caster = param.Caster;
                var spawnPos = caster.GetCenterPosition();
                var shotScale = caster[StatusTool.ShotStatusGroupType.Total][ShotStatusTool.ShotStatusType.ShotScale];
                var affine = new AffinePreset(spawnPos, shotScale);
                var affineCorrect = new AffineCorrectionPreset(AffineTool.CorrectPositionType.CorrectSurface, affine, GameConst.Terrain_LayerMask);
                var damageRate = 0.1f;
                var moveSpeedRate = 4f;
                var width = 0.3f * caster.Scale;
                
                for (var i = 0; i < 4; i++)
                {
                    await UniTask.Delay(50, cancellationToken: p_CancellationToken);
                    for (var j = 0; j < 3; j++)
                    {
                        affineCorrect.SetScaleFactor(Random.Range(0.8f, 1.6f));
                        var shotDirection = o_UV.RotationVectorByPivot(Vector3.down, Random.Range(-80f, 80f));
                        ProjectilePoolManager.GetInstanceUnsafe
                            .SpawnWidthProjectile(createParams, caster, affineCorrect, shotDirection, Random.Range(0.8f, 1.2f) * moveSpeedRate, damageRate, 10f, width, Random.Range(1, 4));
                    }
                }
            }
        }
        
        public class DarkWing_02 : GameEntityDeployEventBase
        {
            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                var createParams = param.GetShotCreateParams(0);
                param.TryGetAutoAimDirection(EntityQueryTool.FilterResultType.RemainNearestOne, out var o_UV);
                var caster = param.Caster;
                var spawnPos = caster.GetCenterPosition();
                var shotScale = caster[StatusTool.ShotStatusGroupType.Total][ShotStatusTool.ShotStatusType.ShotScale];
                var affine = new AffinePreset(spawnPos, shotScale);
                var affineCorrect = new AffineCorrectionPreset(AffineTool.CorrectPositionType.CorrectSurface, affine, GameConst.Terrain_LayerMask);
                var damageRate = 0.1f;
                var moveSpeedRate = 7f;
                var duration = 2f;
                var shotCount = 24;
                var theta = 360f / shotCount;
                for (int i = 0; i < shotCount; i++)
                {
                    o_UV = o_UV.RotationVectorByPivot(Vector3.down, theta);
                    var proj = ProjectilePoolManager.GetInstanceUnsafe.SpawnProjectile(createParams, caster, affineCorrect, o_UV, moveSpeedRate, damageRate, duration);
                    // proj.RunAffineEvent(2010, default);
                }
            }
        }
        
        public class DarkWing_03_00 : GameEntityDeployEventBase
        {
            private GearPoolManager.CreateParams _GearCreateParams;

            public DarkWing_03_00()
            {
                _GearCreateParams = GearPoolManager.GetInstanceUnsafe.GetCreateParams(5);
            }

            public override void PreloadDeployEvent()
            {
                base.PreloadDeployEvent();
                
                GetInstanceUnsafe.PreloadDeployEvent(20020039);
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
                        p_Duration: 0.2f, p_Count: 5
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
        /// 원형 장판 여러개 생성
        /// </summary>
        public class DarkWing_03_01 : GameEntityDeployEventBase
        {
            private const float __ProjectDuration = 4f;
            
            private ProjectorPoolManager.CreateParams _ProjectorCreateParams;

            public DarkWing_03_01()
            {
                _ProjectorCreateParams = ProjectorPoolManager.GetInstanceUnsafe.GetCreateParams(1);
            }

            public override void PreloadDeployEvent()
            {
                base.PreloadDeployEvent();
                
                GetInstanceUnsafe.PreloadDeployEvent(20020039);
            }
            
            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                var caster = param.Caster;
                var waveFactor = 2;
                var shotCount = 8 + 2 * waveFactor;

                for (var i = 0; i < shotCount; i++)
                {
                    await UniTask.Delay(50, cancellationToken: p_CancellationToken);
                  
                    var spawnPos = caster.TryUpdateAndGetCurrentEnemy(out var o_Enemy)
                        ? o_Enemy.GetBottomPosition() 
                        : caster.GetBottomPosition();
                    spawnPos = spawnPos.GetRandomPosition(XYZType.ZX, 2f, 10f + 2f * waveFactor);
                    var scale = (6f + 0.5f * waveFactor) * Random.Range(1f, 1.5f);
                    ProjectorPoolManager.GetInstanceUnsafe.SpawnProjector(_ProjectorCreateParams, caster, new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, spawnPos), caster.GetLookUV(), 1f, 20020039, ProjectorTool.ActivateParamsAttributeType.None, scale, scale, 0.2f, __ProjectDuration, 0.05f);
                }
            }
        }
        
        public class DarkWing_03_02 : GameEntityDeployEventBase
        {
            public override void PreloadDeployEvent()
            {
                base.PreloadDeployEvent();
                
                GetInstanceUnsafe.PreloadDeployEvent(20020039);
            }
            
            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                var createParams = param.GetShotCreateParams(0);
                param.TryGetAutoAimDirection(EntityQueryTool.FilterResultType.RemainNearestOne, out var o_UV);
                var caster = param.Caster;
                var spawnScale = caster[StatusTool.ShotStatusGroupType.Total][ShotStatusTool.ShotStatusType.ShotScale];
                var damageRate = 0.1f;
                var moveSpeedRate = 6f;
                var duration = 2f;
                var shotCount = 9;
                var theta = 360f / shotCount;
                for (int i = 0; i < shotCount; i++)
                {
                    var spawnPos = caster.GetCenterPosition();
                    var affine = new AffinePreset(spawnPos, spawnScale);
                    var affineCorrect = new AffineCorrectionPreset(AffineTool.CorrectPositionType.CorrectSurface, affine, GameConst.Terrain_LayerMask);
                    ProjectilePoolManager.GetInstanceUnsafe
                        .SpawnForwardProjectile(createParams, caster, affineCorrect, o_UV, moveSpeedRate, damageRate, duration);
                    
                    await UniTask.Delay(75, cancellationToken: p_CancellationToken);
                    o_UV = o_UV.RotationVectorByPivot(Vector3.down, theta);
                }
            }
        }
        
        public class DarkWing_03_ex : GameEntityDeployEventBase
        {
            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                var createParams = param.GetShotCreateParams(0);
                param.TryGetAutoAimDirection(EntityQueryTool.FilterResultType.RemainNearestOne, out var o_UV);
                var caster = param.Caster;
                var spawnPos = caster.GetCenterPosition();
                var shotScale = caster[StatusTool.ShotStatusGroupType.Total][ShotStatusTool.ShotStatusType.ShotScale];
                var affine = new AffinePreset(spawnPos, shotScale);
                var affineCorrect = new AffineCorrectionPreset(AffineTool.CorrectPositionType.CorrectSurface, affine, GameConst.Terrain_LayerMask);
                var damageRate = 0.1f;
                var moveSpeedRate = 7f;
                var duration = 2f;
                var shotCount = 12;
                var theta = 360f / shotCount;
                for (int i = 0; i < shotCount; i++)
                {
                    o_UV = o_UV.RotationVectorByPivot(Vector3.down, theta);
                    var proj = ProjectilePoolManager.GetInstanceUnsafe.SpawnProjectile(createParams, caster, affineCorrect, o_UV, moveSpeedRate, damageRate, duration);
                    // proj.RunAffineEvent(2009, default);
                }
            }
        }

        /// <summary>
        /// 큰 원형 장판 생성
        /// </summary>
        public class DarkWing_04 : GameEntityDeployEventBase
        {
            private const float __ProjectDuration = 4f;

            private ProjectorPoolManager.CreateParams _ProjectorCreateParams;

            public DarkWing_04()
            {
                _ProjectorCreateParams = ProjectorPoolManager.GetInstanceUnsafe.GetCreateParams(1);
            }

            public override void PreloadDeployEvent()
            {
                base.PreloadDeployEvent();
                
                GetInstanceUnsafe.PreloadDeployEvent(20020049);
            }
            
            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                var caster = param.Caster;
                var spawnPos = param.TryGetAutoAimDirection(EntityQueryTool.FilterResultType.RemainNearestOne, out var o_UV, out var o_Enemy)
                    ? o_Enemy.GetBottomPosition() 
                    : caster.GetBottomPosition();
                var scale = 8f;
                ProjectorPoolManager.GetInstanceUnsafe.SpawnProjector(_ProjectorCreateParams, caster, new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, spawnPos), caster.GetLookUV(), 1f, 20020049, ProjectorTool.ActivateParamsAttributeType.None, scale, scale, 0.2f, __ProjectDuration, 0.05f);
            }
        }
        
        public class DarkWing_04_ex : GameEntityDeployEventBase
        {
            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                var createParams = param.GetShotCreateParams(0);
                param.TryGetAutoAimDirection(EntityQueryTool.FilterResultType.RemainNearestOne, out var o_UV);
                var caster = param.Caster;
                var spawnPos = caster.GetCenterPosition();
                var shotScale = caster[StatusTool.ShotStatusGroupType.Total][ShotStatusTool.ShotStatusType.ShotScale];
                var affine = new AffinePreset(spawnPos, shotScale);
                var affineCorrect = new AffineCorrectionPreset(AffineTool.CorrectPositionType.CorrectSurface, affine, GameConst.Terrain_LayerMask);
                var damageRate = 0.1f;
                var moveSpeedRate = 7f;
                var duration = 2f;
                var shotCount = 12;
                var theta = 360f / shotCount;
                for (int i = 0; i < shotCount; i++)
                {
                    o_UV = o_UV.RotationVectorByPivot(Vector3.down, theta);
                    var proj = ProjectilePoolManager.GetInstanceUnsafe.SpawnProjectile(createParams, caster, affineCorrect, o_UV, moveSpeedRate, damageRate, duration);
                    // proj.RunAffineEvent(2004, default);
                }
            }
        }
        
        public class DarkWing_05 : GameEntityDeployEventBase
        {
            private GearPoolManager.CreateParams _GearCreateParams;

            public DarkWing_05()
            {
                _GearCreateParams = GearPoolManager.GetInstanceUnsafe.GetCreateParams(5);
            }

            public override void PreloadDeployEvent()
            {
                base.PreloadDeployEvent();
                
                GetInstanceUnsafe.PreloadDeployEvent(32030);
            }
            
            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler,
                CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                var createParams = param.GetShotCreateParams(0);
                param.TryGetAutoAimDirection(EntityQueryTool.FilterResultType.RemainNearestOne, out var o_UV);
                var caster = param.Caster;
                var spawnPos = caster.GetCenterPosition();
                var shotCount = 2;
                var thetaOffset = 180f;
                var wholeTheta = (shotCount + 1) * thetaOffset;
                o_UV = o_UV.RotationVectorByPivot(Vector3.down, -0.5f * wholeTheta);
                for (var i = 0; i < shotCount; i++)
                {
                    o_UV = o_UV.RotationVectorByPivot(Vector3.down, thetaOffset);
                    var gear = GearPoolManager.GetInstanceUnsafe.SpawnGear
                    (
                        _GearCreateParams,
                        new GearPoolManager.ActivateParams
                        (
                            null,
                            new AffineCorrectionPreset(AffineTool.CorrectPositionType.CorrectSurface, spawnPos,
                                GameConst.Terrain_LayerMask),
                            GameEntityTool.ActivateParamsAttributeType.GiveFollowFallenMaster,
                            caster, default,
                            p_Duration: 0.12f, p_Count: 8
                        )
                    );

                    gear.SetLookUV(o_UV); 
                    gear.SwitchActionModule(ActionModuleDataTableQuery.TableLabel.Default);
                    gear.SwitchPhysicsModule(PhysicsModuleDataTableQuery.TableLabel.Affine);
                    gear.AddStatus(StatusTool.BattleStatusGroupType.ExtraAdd, BattleStatusTool.BattleStatusType.MoveSpeedBasis, 18f);
                    // gear.RunAffineEvent(2010, default);
                }
            }
        }
        
        public class DarkWing_06 : GameEntityDeployEventBase
        {
            private ProjectorPoolManager.CreateParams _ProjectorCreateParams;

            public DarkWing_06()
            {
                _ProjectorCreateParams = ProjectorPoolManager.GetInstanceUnsafe.GetCreateParams(4);
            }

            public override void PreloadDeployEvent()
            {
                base.PreloadDeployEvent();
                
                GetInstanceUnsafe.PreloadDeployEvent(20020010);
                GetInstanceUnsafe.PreloadDeployEvent(20020069);
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

                await GetInstanceUnsafe._EntityEventTable[20020010].RunEvent(p_Handler, p_CancellationToken);
                
                if (caster.GetCurrentStatusRate(BattleStatusTool.BattleStatusType.HP_Base) > 0.25f)
                {
                    return; 
                }
                
                param.TryGetAutoAimDirection(EntityQueryTool.FilterResultType.RemainNearestOne, out var o_UV);
                caster.AddStateMask(GameEntityTool.EntityStateType.BLOCK);

                var projectorSpawnPos = caster.GetCenterPosition() + 20f * o_UV;
                ProjectorPoolManager.GetInstanceUnsafe.SpawnProjector(_ProjectorCreateParams, caster, new AffineCorrectionPreset(AffineTool.CorrectPositionType.CorrectSurface, projectorSpawnPos, GameConst.Terrain_LayerMask), o_UV, 1f, 20020069, ProjectorTool.ActivateParamsAttributeType.None, 6f, 48f, 0.4f, 1f, 0.05f);

                await UniTask.Delay(2500, cancellationToken: p_CancellationToken);
                caster.RemoveStateMask(GameEntityTool.EntityStateType.BLOCK);
            }
        }
        
        public class DarkWing_06_ex : GameEntityDeployEventBase
        {
            private BeamPoolManager.CreateParams _BeamCreateParams;

            public DarkWing_06_ex()
            {
                _BeamCreateParams = BeamPoolManager.GetInstanceUnsafe.GetCreateParams(4);
            }

            public override void PreloadDeployEvent()
            {
                base.PreloadDeployEvent();
                
            }
            
            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                var caster = param.Caster;
                var master = caster.GetMaster();
                var spawnPos = master.GetCenterPosition() + master.GetLookUV();
                var affine = new AffinePreset(spawnPos, 2f);
                var affineCorrect = new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, affine);
                var shotDirection = caster.GetLookUV();
 
                BeamPoolManager.GetInstanceUnsafe.SpawnBeam(_BeamCreateParams, master, affineCorrect, shotDirection, 10f, 100f, 10, 2f, 0.1f);
            }
        }
        
        #endregion
    }
}