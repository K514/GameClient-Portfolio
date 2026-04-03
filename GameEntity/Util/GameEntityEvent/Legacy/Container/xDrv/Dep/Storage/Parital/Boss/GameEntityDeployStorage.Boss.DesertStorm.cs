using System.Threading;
using Cysharp.Threading.Tasks;
using k514.Mono.Feature;
using UnityEngine;

namespace k514.Mono.Common
{
    public partial class GameEntityDeployStorage 
    {
        #region <Callbacks>

        private void OnCreateDesertStorm()
        {
            _EntityEventTable.Add(20010000, new DesertStorm_00());
            _EntityEventTable.Add(20010010, new DesertStorm_01());
            _EntityEventTable.Add(20010020, new DesertStorm_02());
            _EntityEventTable.Add(20010030, new DesertStorm_03());
            _EntityEventTable.Add(20010040, new DesertStorm_04());
            _EntityEventTable.Add(20010050, new DesertStorm_05());
            _EntityEventTable.Add(20010059, new DesertStorm_05_ex());
            _EntityEventTable.Add(20010060, new DesertStorm_06());
            _EntityEventTable.Add(20010069, new DesertStorm_06_ex());
            _EntityEventTable.Add(20010070, new DesertStorm_07());
            _EntityEventTable.Add(20010080, new DesertStorm_08());
        }

        #endregion
        
        #region <Classess>
        
        public class DesertStorm_00 : GameEntityDeployEventBase
        {
            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                var createParams = param.GetShotCreateParams(1);
                var caster = param.Caster;
                var spawnPos = caster.GetCenterPosition();
                var shotScale = caster[StatusTool.ShotStatusGroupType.Total][ShotStatusTool.ShotStatusType.ShotScale];
                var affine = new AffinePreset(spawnPos, shotScale);
                var affineCorrect = new AffineCorrectionPreset(AffineTool.CorrectPositionType.ForceSurface, affine, GameConst.Terrain_LayerMask);
                var damageRate = 1f;
                var moveSpeedRate = 7f;

                for (int i = 0; i < 3; i++)
                {
                    param.TryGetAutoAimDirection(EntityQueryTool.FilterResultType.RemainNearestOne, out var o_UV);
                    ProjectilePoolManager.GetInstanceUnsafe
                        .SpawnSpreadProjectile(createParams, caster, affineCorrect, o_UV, moveSpeedRate, damageRate, 20f, 30f, 3);

                    await UniTask.Delay(300, cancellationToken: p_CancellationToken);
                }
            }
        }
        
        public class DesertStorm_01 : GameEntityDeployEventBase
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
                var affineCorrect = new AffineCorrectionPreset(AffineTool.CorrectPositionType.ForceSurface, affine, GameConst.Terrain_LayerMask);
                var damageRate = 1f;
                var moveSpeedRate = 7f;

                ProjectilePoolManager.GetInstanceUnsafe
                    .SpawnSpreadProjectile(createParams, caster, affineCorrect, o_UV, moveSpeedRate, damageRate, 20f, 45f, 1 + 2 * Random.Range(1,3));
            }
        }
        
        public class DesertStorm_02 : GameEntityDeployEventBase
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
                var affineCorrect = new AffineCorrectionPreset(AffineTool.CorrectPositionType.ForceSurface, affine, GameConst.Terrain_LayerMask);
                var damageRate = 1f;

                for (int i = 0; i < 4; i++)
                {
                    await UniTask.Delay(50, cancellationToken: p_CancellationToken);
                    for (int j = 0; j < 3; j++)
                    {
                        var moveSpeedRate = Random.Range(5f, 10f);
                        affineCorrect.SetScaleFactor(Random.Range(0.8f, 1.6f));
                        var shotDirection = o_UV.RotationVectorByPivot(Vector3.down, Random.Range(-66f, 66f));
                        ProjectilePoolManager.GetInstanceUnsafe
                            .SpawnForwardProjectile(createParams, caster, affineCorrect, shotDirection, Random.Range(1f, 3f) * moveSpeedRate, damageRate, 20f);
                    }
                }
            }
        }
        
        public class DesertStorm_03 : GameEntityDeployEventBase
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
                var affineCorrect = new AffineCorrectionPreset(AffineTool.CorrectPositionType.ForceSurface, affine, GameConst.Terrain_LayerMask);
                var damageRate = 1f;
                var moveSpeedRate = 7f;

                o_UV = o_UV.RotationVectorByPivot(Vector3.down, Random.Range(0f, 360f));
                var waveCount = Random.Range(2, 6);
                var shotCount = (6 - waveCount) * Random.Range(2, 7);
                for (int i = 0; i < shotCount; i++)
                {
                    await UniTask.Delay(50, cancellationToken: p_CancellationToken);
                    o_UV = o_UV.RotationVectorByPivot(Vector3.down, 10f);
                    ProjectilePoolManager.GetInstanceUnsafe.SpawnRoundProjectile(createParams, caster, affineCorrect, o_UV, moveSpeedRate, damageRate, 20f, waveCount);
                }
            }
        }
        
        public class DesertStorm_04 : GameEntityDeployEventBase
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
                var affineCorrect = new AffineCorrectionPreset(AffineTool.CorrectPositionType.ForceSurface, affine, GameConst.Terrain_LayerMask);
                var damageRate = 1f;
                var moveSpeedRate = 7f;
                
                o_UV = o_UV.RotationVectorByPivot(Vector3.down, Random.Range(0f, 360f));
                var waveCount = Random.Range(2, 6);
                var shotCount = (6 - waveCount) * Random.Range(2, 7);
                for (int i = 0; i < shotCount; i++)
                {
                    await UniTask.Delay(50, cancellationToken: p_CancellationToken);
                    o_UV = o_UV.RotationVectorByPivot(Vector3.down, -10f);
                    ProjectilePoolManager.GetInstanceUnsafe
                        .SpawnRoundProjectile(createParams, caster, affineCorrect, o_UV, moveSpeedRate, damageRate, 20f, waveCount);
                }
            }
        }
        
        public class DesertStorm_05 : GameEntityDeployEventBase
        {
            private ProjectorPoolManager.CreateParams _ProjectorCreateParams;

            public DesertStorm_05()
            {
                _ProjectorCreateParams = ProjectorPoolManager.GetInstanceUnsafe.GetCreateParams(1);
            }

            public override void PreloadDeployEvent()
            {
                base.PreloadDeployEvent();
                
                GetInstanceUnsafe.PreloadDeployEvent(20010059);
            }
            
            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                var caster = param.Caster;
                var shotCount = 4;
                for (int i = 0; i < shotCount; i++)
                {
                    var spawnPosition = param.TryGetAutoAimDirection(EntityQueryTool.FilterResultType.RemainNearestOne, out var o_UV, out var o_Enemy)
                        ? o_Enemy.GetCenterPosition()
                        : caster.GetCenterPosition();
                    ProjectorPoolManager.GetInstanceUnsafe.SpawnProjector(_ProjectorCreateParams, caster, new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, spawnPosition), o_UV, 1f, 20010059, ProjectorTool.ActivateParamsAttributeType.None, 8f, 8f, 0.2f, 1f, 0.05f);
              
                    await UniTask.Delay(800, cancellationToken: p_CancellationToken);
                }
            }
        }
        
        public class DesertStorm_05_ex : GameEntityDeployEventBase
        {
            private VfxPoolManager.CreateParams _EffectCreateParams;

            public DesertStorm_05_ex()
            {
                _EffectCreateParams = VfxPoolManager.GetInstanceUnsafe.GetCreateParams(136);
            }

            public override void PreloadDeployEvent()
            {
                base.PreloadDeployEvent();
                
            }
            
            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                var caster = param.Caster;
                var vfx = VfxPoolManager.GetInstanceUnsafe.Pop(_EffectCreateParams, new VfxPoolManager.ActivateParams(null, new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, caster.GetBottomPosition()), GameEntityTool.ActivateParamsAttributeType.None, null, default));
                vfx.SetScaleFactor(2.5f); 
                
                CameraManager.GetInstanceUnsafe.SetShake(Vector3.right, 7f, 0, 150, 3);

                if (caster.FilterFocusEntityWithSightRange(p_FilterStateMask: GameEntityTool.EntityStateType.EngageCandidateFilterMask))
                {
                    var filterGroupSet = caster.FilterResultGroup;
                    foreach (var filtered in filterGroupSet)
                    {
                        var entityType = filtered.WorldObjectType;
                        switch (entityType)
                        {
                            default:
                            {
                                var damage = caster[StatusTool.BattleStatusGroupType.Total].GetScaledAttackBase(DamageCalculator.DamageCalcType.Melee);
                                filtered.GiveDamage(damage, new StatusTool.StatusChangeParams(StatusTool.StatusChangeEventType.Combat, caster.GetMaster(), filtered.GetCenterPosition()));
                                break;
                            }
                        }
                    }
                }
            }
        }
        
        public class DesertStorm_06 : GameEntityDeployEventBase
        {
            private ProjectorPoolManager.CreateParams _ProjectorCreateParams;

            public DesertStorm_06()
            {
                _ProjectorCreateParams = ProjectorPoolManager.GetInstanceUnsafe.GetCreateParams(1);
            }

            public override void PreloadDeployEvent()
            {
                base.PreloadDeployEvent();
                
                GetInstanceUnsafe.PreloadDeployEvent(20010069);
            }
            
            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                param.TryGetAutoAimDirection(EntityQueryTool.FilterResultType.RemainNearestOne, out var o_UV);
                var caster = param.Caster;
                var spawnPos = caster.GetCenterPosition();

                for (int i = 0; i < 24; i++)
                {
                    spawnPos += 2f * o_UV;
                    ProjectorPoolManager.GetInstanceUnsafe.SpawnProjector(_ProjectorCreateParams, caster, new AffineCorrectionPreset(AffineTool.CorrectPositionType.CorrectSurface, spawnPos, GameConst.Terrain_LayerMask), o_UV, 1f, 20010069, ProjectorTool.ActivateParamsAttributeType.None, 8f, 8f, 0.2f, 1f, 0.05f);
                    await UniTask.Delay(50, cancellationToken: p_CancellationToken);
                }
            }
        }
        
        public class DesertStorm_06_ex : GameEntityDeployEventBase
        {
            private VfxPoolManager.CreateParams _EffectCreateParams;

            public DesertStorm_06_ex()
            {
                _EffectCreateParams = VfxPoolManager.GetInstanceUnsafe.GetCreateParams(136);
            }

            public override void PreloadDeployEvent()
            {
                base.PreloadDeployEvent();
                
            }
            
            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                var caster = param.Caster;
                var vfx = VfxPoolManager.GetInstanceUnsafe.Pop(_EffectCreateParams, new VfxPoolManager.ActivateParams(null, new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, caster.GetBottomPosition()), GameEntityTool.ActivateParamsAttributeType.None, null, default));
                vfx.SetScaleFactor(2.5f); 
                
                CameraManager.GetInstanceUnsafe.SetShake(Vector3.right, 7f, 0, 150, 3);

                /*var queryParams 
                    = new EntityQueryTool.FilterQueryParams
                    (
                        default, GameEntityTool.GameEntityGroupRelateType.Enemy, 
                        EntityQueryTool.FilterQueryFlagType.ExceptMe | EntityQueryTool.FilterQueryFlagType.FreeAll, 
                        GameEntityTool.EntityStateType.EngageCandidateFilterMask
                    );
            
                if (caster.FilterFocusEntity(queryParams))*/
                {
                    var filterGroupSet = caster.FilterResultGroup;
                    foreach (var filtered in filterGroupSet)
                    {
                        var entityType = filtered.WorldObjectType;
                        switch (entityType)
                        {
                            default:
                            {
                                var damage = 0.1f * caster[StatusTool.BattleStatusGroupType.Total].GetScaledAttackBase(DamageCalculator.DamageCalcType.Melee);
                                filtered.GiveDamage(damage, new StatusTool.StatusChangeParams(StatusTool.StatusChangeEventType.Combat, caster.GetMaster(), filtered.GetCenterPosition()));
                                // filtered.Enchant(12000000, new GameEntityEventCommonParams(caster.GetMaster()));
                                break;
                            }
                        }
                    }
                }
            }
        }
        
        public class DesertStorm_07 : GameEntityDeployEventBase
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
                var affineCorrect = new AffineCorrectionPreset(AffineTool.CorrectPositionType.ForceSurface, affine, GameConst.Terrain_LayerMask);
                var damageRate = 1f;
                var moveSpeedRate = 7f;

                ProjectilePoolManager.GetInstanceUnsafe
                    .SpawnSpreadProjectile(createParams, caster, affineCorrect, o_UV, moveSpeedRate, damageRate, 20f, 45f, 1 + 2 * Random.Range(1,3));
            }
        }
        
        public class DesertStorm_08 : GameEntityDeployEventBase
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
                var affineCorrect = new AffineCorrectionPreset(AffineTool.CorrectPositionType.ForceSurface, affine, GameConst.Terrain_LayerMask);
                var damageRate = 1f;
                var moveSpeedRate = 7f;

                ProjectilePoolManager.GetInstanceUnsafe
                    .SpawnSpreadProjectile(createParams, caster, affineCorrect, o_UV, moveSpeedRate, damageRate, 20f, 45f, 1 + 2 * Random.Range(1,3));
            }
        }
        
        #endregion
    }
}