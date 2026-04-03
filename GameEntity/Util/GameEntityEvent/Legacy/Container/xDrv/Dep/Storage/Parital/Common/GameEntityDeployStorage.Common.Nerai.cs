using System.Threading;
using Cysharp.Threading.Tasks;
using k514.Mono.Feature;
using UnityEngine;

namespace k514.Mono.Common
{
    public partial class GameEntityDeployStorage 
    {
        #region <Callbacks>

        private void OnCreateNerai()
        {
            _EntityEventTable.Add(30000, new Nerai_00());
            _EntityEventTable.Add(30010, new Nerai_01(3));
            _EntityEventTable.Add(30011, new Nerai_01(5));
            _EntityEventTable.Add(30020, new Nerai_02(3));
            _EntityEventTable.Add(30021, new Nerai_02(5));
            _EntityEventTable.Add(30030, new Nerai_03(3));
            _EntityEventTable.Add(30031, new Nerai_03(5));
            _EntityEventTable.Add(30040, new Nerai_04(5));
            _EntityEventTable.Add(30041, new Nerai_04(8));
            _EntityEventTable.Add(30049, new Nerai_04_ex());
            _EntityEventTable.Add(30050, new Nerai_05(5));
            _EntityEventTable.Add(30051, new Nerai_05(7));
            _EntityEventTable.Add(30060, new Nerai_06_0());
            _EntityEventTable.Add(30061, new Nerai_06_1(3, 0f, 36f));
            _EntityEventTable.Add(30062, new Nerai_06_1(5, 0f, 45f));
            _EntityEventTable.Add(30070, new Nerai_07(31020));
            _EntityEventTable.Add(30071, new Nerai_07(31000));
            _EntityEventTable.Add(30072, new Nerai_07(31012));
            _EntityEventTable.Add(30080, new Nerai_08());
        }

        #endregion
        
        #region <Classess>
        
        /// <summary>
        /// 전방에 직선탄 1발 발사
        /// </summary>
        public class Nerai_00 : GameEntityDeployEventBase
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
                var damageRate = 1f;
                var moveSpeedRate = 4f;
                
                ProjectilePoolManager.GetInstanceUnsafe.SpawnForwardProjectile(createParams, caster, affineCorrect, o_UV, moveSpeedRate, damageRate, 10f);
            }
        }
        
        /// <summary>
        /// 전방에 방사탄 발사
        /// </summary>
        public class Nerai_01 : GameEntityDeployEventBase
        {
            private int _ShotCount;
            
            public Nerai_01(int p_ShotCount)
            {
                _ShotCount = p_ShotCount;
            }
            
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
                var damageRate = 1f / _ShotCount;
                var moveSpeedRate = 4f;
                
                ProjectilePoolManager.GetInstanceUnsafe
                    .SpawnSpreadProjectile(createParams, caster, affineCorrect, o_UV, moveSpeedRate, damageRate, 10f, 30f, _ShotCount);
            }
        }
        
        /// <summary>
        /// 전방에 횡렬탄 발사
        /// </summary>
        public class Nerai_02 : GameEntityDeployEventBase
        {
            private int _ShotCount;
            
            public Nerai_02(int p_ShotCount)
            {
                _ShotCount = p_ShotCount;
            }
            
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
                var damageRate = 1f / _ShotCount;
                var moveSpeedRate = 4f;

                ProjectilePoolManager.GetInstanceUnsafe
                    .SpawnWidthProjectile(createParams, caster, affineCorrect, o_UV, moveSpeedRate, damageRate, 10f, 0.5f, _ShotCount);
            }
        }
        
        /// <summary>
        /// 랜덤한 방향에 횡렬탄 발사
        /// </summary>
        public class Nerai_03 : GameEntityDeployEventBase
        {
            private int _ShotCount;
            
            public Nerai_03(int p_ShotCount)
            {
                _ShotCount = p_ShotCount;
            }
            
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
                var damageRate = 1f / _ShotCount;
                var moveSpeedRate = 4f;

                o_UV = o_UV.RotationVectorByPivot(Vector3.up, Random.Range(-22f, 22f));

                ProjectilePoolManager.GetInstanceUnsafe
                    .SpawnWidthProjectile(createParams, caster, affineCorrect, o_UV, moveSpeedRate, damageRate, 10f, 0.5f, _ShotCount);
            }
        }
 
        /// <summary>
        /// 폭격 탄막 생성
        /// </summary>
        public class Nerai_04 : GameEntityDeployEventBase
        {
            private int _ShotCount;
            
            public Nerai_04(int p_ShotCount)
            {
                _ShotCount = p_ShotCount;
            }
            
            public override void PreloadDeployEvent()
            {
                base.PreloadDeployEvent();
                
                GetInstanceUnsafe.PreloadDeployEvent(30049);
            }
            
            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                var createParams = param.GetShotCreateParams(0);
                param.TryGetAutoAimDirection(EntityQueryTool.FilterResultType.RemainNearestOne, out var o_UV);
                var caster = param.Caster;
                var spawnScale = caster[StatusTool.ShotStatusGroupType.Total][ShotStatusTool.ShotStatusType.ShotScale];
                var damageRate = 1f;
                var moveSpeedRate = 25f;
                
                for (int i = 0; i < _ShotCount; i++)
                {
                    await UniTask.Delay(100, cancellationToken: p_CancellationToken);
                  
                    var spawnPos = caster.GetCenterPosition();
                    var randPos = spawnPos.GetRandomPosition(XYZType.ZX, 0f, 1f);
                    var affine = new AffinePreset(randPos, spawnScale);
                    var affineCorrect = new AffineCorrectionPreset(AffineTool.CorrectPositionType.CorrectSurface, affine, GameConst.Terrain_LayerMask);
                    ProjectilePoolManager.GetInstanceUnsafe
                        .SpawnForwardProjectile(createParams, caster, affineCorrect, Vector3.up, moveSpeedRate, damageRate, 1f, default);
                }
            }
        }  
        
        /// <summary>
        /// 낙하 탄막 생성
        /// </summary>
        public class Nerai_04_ex : GameEntityDeployEventBase
        {
            private ProjectorPoolManager.CreateParams _ProjectorCreateParams;

            public Nerai_04_ex()
            {
                _ProjectorCreateParams = ProjectorPoolManager.GetInstanceUnsafe.GetCreateParams(1);
            }

            public override void PreloadDeployEvent()
            {
                base.PreloadDeployEvent();
                
            }
            
            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                var projectileCreateParams = param.GetShotCreateParams(0);
                var caster = param.Caster.GetMaster();
                var spawnScale = caster[StatusTool.ShotStatusGroupType.Total][ShotStatusTool.ShotStatusType.ShotScale];
                var damageRate = 0.5f;
                var moveSpeedRate = 20f;
                var duration = 2f;
                var spawnPos = caster.TryGetCurrentEnemy(out var o_Enemy) ? o_Enemy.GetBottomPosition() : caster.GetBottomPosition();
                spawnPos += CustomMath.GetRandomVector(XYZType.ZX, 0f, 2f);
                ProjectorPoolManager.GetInstanceUnsafe.SpawnProjector(_ProjectorCreateParams, caster, new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, spawnPos), Vector3.forward, 1f, 0, ProjectorTool.ActivateParamsAttributeType.GiveForwardPivotAttribute, 1f, 1f, 0.2f, duration, 0.05f);

                spawnPos += 50f * Vector3.up;
                var direction = Vector3.down;
                var affine = new AffinePreset(spawnPos, spawnScale);
                var affineCorrect = new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, affine, GameConst.Terrain_LayerMask);
                ProjectilePoolManager.GetInstanceUnsafe
                    .SpawnForwardProjectile(projectileCreateParams, caster, affineCorrect, direction, moveSpeedRate, damageRate, 10f);
            }
        }  
        
        /// <summary>
        /// V대열탄 발사
        /// </summary>
        public class Nerai_05 : GameEntityDeployEventBase
        { 
            private int _ShotCount;
            
            public Nerai_05(int p_ShotCount)
            {
                _ShotCount = p_ShotCount - 1;
            }
            
            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                var createParams = param.GetShotCreateParams(0);
                param.TryGetAutoAimDirection(EntityQueryTool.FilterResultType.RemainNearestOne, out var o_UV);
                var caster = param.Caster;
                var spawnPos = caster.GetCenterPosition();
                var spawnScale = caster[StatusTool.ShotStatusGroupType.Total][ShotStatusTool.ShotStatusType.ShotScale];
                var affine = new AffinePreset(spawnPos, spawnScale);
                var affineCorrect = new AffineCorrectionPreset(AffineTool.CorrectPositionType.CorrectSurface, affine, GameConst.Terrain_LayerMask);
                var damageRate = 1f / _ShotCount;
                var moveSpeedRate = 10f;
                
                ProjectilePoolManager.GetInstanceUnsafe
                    .SpawnForwardProjectile(createParams, caster, affineCorrect, o_UV, moveSpeedRate, damageRate, 20f);
                
                var widthFactor = 0.5f * caster.Scale;
                for (int i = 0; i < _ShotCount; i++)
                {
                    await UniTask.Delay(100, cancellationToken: p_CancellationToken);
                  
                    spawnPos = caster.GetCenterPosition();
                    affine = new AffinePreset(spawnPos, spawnScale);
                    affineCorrect = new AffineCorrectionPreset(AffineTool.CorrectPositionType.CorrectSurface, affine, GameConst.Terrain_LayerMask);
                    ProjectilePoolManager.GetInstanceUnsafe
                        .SpawnWidthProjectile(createParams, caster, affineCorrect, o_UV, moveSpeedRate, damageRate, 10f, widthFactor * (i + 1), 2);
                }
            }
        }  
        
        /// <summary>
        /// 전방에 지그재그탄 1발 발사
        /// </summary>
        public class Nerai_06_0 : GameEntityDeployEventBase
        {
            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                var createParams = param.GetShotCreateParams(0);
                param.TryGetAutoAimDirection(EntityQueryTool.FilterResultType.RemainNearestOne, out var o_UV);
                var caster = param.Caster;
                var spawnPos = caster.GetCenterPosition();
                var spawnScale = caster[StatusTool.ShotStatusGroupType.Total][ShotStatusTool.ShotStatusType.ShotScale];
                var affine = new AffinePreset(spawnPos, spawnScale);
                var affineCorrect = new AffineCorrectionPreset(AffineTool.CorrectPositionType.CorrectSurface, affine, GameConst.Terrain_LayerMask);
                var damageRate = 1f;
                var moveSpeedRate = 4f;
                
                var proj = ProjectilePoolManager.GetInstanceUnsafe
                    .SpawnProjectile(createParams, caster, affineCorrect, o_UV, moveSpeedRate, damageRate, 20f);
                // proj.RunAffineEvent(2003, default);
            }
        }
        
        /// <summary>
        /// 전방에 지그재그 방사탄 발사
        /// </summary>
        public class Nerai_06_1 : GameEntityDeployEventBase
        {
            private int _ShotCount;
            private float _StartTheta;
            private float _DeltaTheta;
            private float _WholeThetaHalf;
            
            public Nerai_06_1(int p_ShotCount, float p_StartTheta, float p_DeltaTheta)
            {
                _ShotCount = Mathf.Max(2, p_ShotCount);
                _StartTheta = p_StartTheta;
                _DeltaTheta = p_DeltaTheta;
                _WholeThetaHalf = 0.5f * _DeltaTheta * (_ShotCount - 1);
            }
            
            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                var createParams = param.GetShotCreateParams(0);
                param.TryGetAutoAimDirection(EntityQueryTool.FilterResultType.RemainNearestOne, out var o_UV);
                var caster = param.Caster;
                var spawnScale = caster[StatusTool.ShotStatusGroupType.Total][ShotStatusTool.ShotStatusType.ShotScale];
                var damageRate = 1f / _ShotCount;
                var moveSpeedRate = 4f;

                o_UV = o_UV.RotationVectorByPivot(Vector3.up, _StartTheta + _WholeThetaHalf);
                for (int i = 0; i < _ShotCount; i++)
                {
                    var spawnPos = caster.GetCenterPosition();
                    var affine = new AffinePreset(spawnPos, spawnScale);
                    var affineCorrect = new AffineCorrectionPreset(AffineTool.CorrectPositionType.CorrectSurface, affine, GameConst.Terrain_LayerMask);
                    var proj = ProjectilePoolManager.GetInstanceUnsafe
                        .SpawnProjectile(createParams, caster, affineCorrect, o_UV, moveSpeedRate, damageRate, 20f);
                    // proj.RunAffineEvent(2003, default);
                  
                    o_UV = o_UV.RotationVectorByPivot(Vector3.down, _DeltaTheta);
                    await UniTask.Delay(75, cancellationToken: p_CancellationToken);
                }
            }
        }  

        /// <summary>
        /// 전방에 프렉탈 탄막 발사
        /// </summary>
        public class Nerai_07 : GameEntityDeployEventBase
        {
            private int _FractalIndex;

            public Nerai_07(int p_Index)
            {
                _FractalIndex = p_Index;
            }
            
            public override void PreloadDeployEvent()
            {
                base.PreloadDeployEvent();
                
                GetInstanceUnsafe.PreloadDeployEvent(_FractalIndex);
            }
            
            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                var createParams = param.GetShotCreateParams(0);
                param.TryGetAutoAimDirection(EntityQueryTool.FilterResultType.RemainNearestOne, out var o_UV);
                var caster = param.Caster;
                var spawnPos = caster.GetCenterPosition();
                var spawnScale = 2f * caster[StatusTool.ShotStatusGroupType.Total][ShotStatusTool.ShotStatusType.ShotScale];
                var affine = new AffinePreset(spawnPos, spawnScale);
                var affineCorrect = new AffineCorrectionPreset(AffineTool.CorrectPositionType.CorrectSurface, affine, GameConst.Terrain_LayerMask);
                var damageRate = 0.5f;
                var moveSpeedRate = 3f;
                
                ProjectilePoolManager.GetInstanceUnsafe.SpawnForwardProjectile(createParams, caster, affineCorrect, o_UV, moveSpeedRate, damageRate, 3f, default);
            }
        } 
        
        /// <summary>
        /// 전방에 추적 탄막 발사
        /// </summary>
        public class Nerai_08 : GameEntityDeployEventBase
        {
            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                var createParams = param.GetShotCreateParams(0);
                param.TryGetAutoAimDirection(EntityQueryTool.FilterResultType.RemainNearestOne, out var o_UV);
                var caster = param.Caster;
                var spawnPos = caster.GetCenterPosition();
                var spawnScale = 0.8f * caster[StatusTool.ShotStatusGroupType.Total][ShotStatusTool.ShotStatusType.ShotScale];
                var affine = new AffinePreset(spawnPos, spawnScale);
                var affineCorrect = new AffineCorrectionPreset(AffineTool.CorrectPositionType.CorrectSurface, affine, GameConst.Terrain_LayerMask);
                var moveSpeedRate = 12f;
                var shotCount = 2 * p_Handler.CommonParams.Count;
                var damageRate = 1f / shotCount;
                var duration = 8f;
                
                switch (shotCount)
                {
                    case < 1 :
                        break;
                    case 1:
                    {
                        var proj = ProjectilePoolManager.GetInstanceUnsafe.SpawnForwardProjectile(createParams, caster, affineCorrect, o_UV, moveSpeedRate, damageRate, duration);
                        proj.AddStatus(StatusTool.BattleStatusGroupType.ExtraAdd, BattleStatusTool.BattleStatusType.SightRange, caster.SightRange);
                        proj.SetEnemySelectType(GameEntityTool.EnemySelectType.Random);
                        // proj.SwitchPersona(MindModuleDataTableQuery.TableLabel.Tracker);
                        break;
                    }
                    default:
                    {

                        var halfDegree = 30f;
                        var deltaTheta = 2f * halfDegree / (shotCount - 1);
                        o_UV = o_UV.RotationVectorByPivot(Vector3.up, halfDegree + deltaTheta);
                        for (int i = 0; i < shotCount; i++)
                        {
                            o_UV = o_UV.RotationVectorByPivot(Vector3.down, deltaTheta);
                            var proj = ProjectilePoolManager.GetInstanceUnsafe.SpawnForwardProjectile(createParams, caster, affineCorrect, o_UV, moveSpeedRate, damageRate, duration);
                            proj.AddStatus(StatusTool.BattleStatusGroupType.ExtraAdd, BattleStatusTool.BattleStatusType.SightRange, caster.SightRange);
                            proj.SetEnemySelectType(GameEntityTool.EnemySelectType.Random);
                            // proj.SwitchPersona(MindModuleDataTableQuery.TableLabel.Tracker);
                        }
                        break;
                    }
                }
            }
        } 
        
        #endregion
    }
}