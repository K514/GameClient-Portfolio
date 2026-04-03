using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace k514.Mono.Common
{
    public partial class GameEntityDeployStorage 
    {
        #region <Callbacks>

        private void OnCreateHazushi()
        {
            _EntityEventTable.Add(31000, new Hazushi_00(1, 4, 0f));
            _EntityEventTable.Add(31001, new Hazushi_00(2, 2, 90f));
            _EntityEventTable.Add(31010, new Hazushi_01(3, 4, 0f));
            _EntityEventTable.Add(31011, new Hazushi_01(4, 6, 20f));
            _EntityEventTable.Add(31012, new Hazushi_01(12, 3, 40f));
            _EntityEventTable.Add(31020, new Hazushi_02(2, 8, 22.5f));
            _EntityEventTable.Add(31021, new Hazushi_02(3, 6, 15f));
            _EntityEventTable.Add(31022, new Hazushi_02(1, 16, 0f));
            _EntityEventTable.Add(31030, new Hazushi_03(4, 40f));
            _EntityEventTable.Add(31031, new Hazushi_03(8, 60f));
            _EntityEventTable.Add(31032, new Hazushi_03(12, 80f));
            _EntityEventTable.Add(31040, new Hazushi_04(4, 40f));
            _EntityEventTable.Add(31041, new Hazushi_04(8, 60f));
            _EntityEventTable.Add(31042, new Hazushi_04(12, 80f));
            _EntityEventTable.Add(31050, new Hazushi_05());
            _EntityEventTable.Add(31060, new Hazushi_06());
        }

        #endregion

        #region <Classess>
        
        /// <summary>
        /// 전방위에 이중나선궤도탄을 발사
        /// </summary>
        public class Hazushi_00 : GameEntityDeployEventBase
        {
            private int _WaveCount;
            private int _ShotCount;
            private float _DeltaTheta;
            private float _RotateTheta;

            public Hazushi_00(int p_WaveCount, int p_ShotCount, float p_DeltaTheta)
            {
                _WaveCount = p_WaveCount;
                _ShotCount = p_ShotCount;
                _DeltaTheta = p_DeltaTheta;
                _RotateTheta = 360f / _ShotCount;
            }
            
            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                var createParams = param.GetShotCreateParams(0);
                param.TryGetAutoAimDirection(EntityQueryTool.FilterResultType.RemainNearestOne, out var o_UV);
                var caster = param.Caster;
                var spawnScale = caster[StatusTool.ShotStatusGroupType.Total][ShotStatusTool.ShotStatusType.ShotScale];
                var damageRate = 1f / _ShotCount;
                var moveSpeedRate = 8f;

                for (var j = 0; j < _WaveCount; j++)
                {
                    var spawnPos = caster.GetCenterPosition();
                    var affine = new AffinePreset(spawnPos, spawnScale);
                    var affineCorrect = new AffineCorrectionPreset(AffineTool.CorrectPositionType.CorrectSurface, affine, GameConst.Terrain_LayerMask);
                
                    for (int i = 0; i < _ShotCount; i++)
                    {
                        o_UV = o_UV.RotationVectorByPivot(Vector3.down, _RotateTheta);
                        var proj1 = ProjectilePoolManager.GetInstanceUnsafe.SpawnProjectile(createParams, caster, affineCorrect, o_UV, moveSpeedRate, damageRate, 10f);
                        // proj1.RunAffineEvent(2001, default);
                    
                        var proj2 = ProjectilePoolManager.GetInstanceUnsafe.SpawnProjectile(createParams, caster, affineCorrect, o_UV, moveSpeedRate, damageRate, 10f);
                        // proj2.RunAffineEvent(2002, default);
                    }
                    
                    o_UV = o_UV.RotationVectorByPivot(Vector3.down, _DeltaTheta);
                    await UniTask.Delay(500, cancellationToken: p_CancellationToken);
                }
            }
        }
   
        /// <summary>
        /// 바람개비 모양 탄막 생성
        /// </summary>
        public class Hazushi_01 : GameEntityDeployEventBase
        {
            private int _ShotCount;
            private int _LegCount;
            private float _Theta;

            public Hazushi_01(int p_ShotCount, int p_LegCount, float p_Theta)
            {
                _ShotCount = p_ShotCount;
                _LegCount = p_LegCount;
                _Theta = p_Theta;
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
                
                o_UV = o_UV.RotationVectorByPivot(Vector3.down, _Theta);
                
                for (int i = 0; i < _ShotCount; i++)
                {
                    var spawnPos = caster.GetCenterPosition();
                    var affine = new AffinePreset(spawnPos, spawnScale);
                    var affineCorrect = new AffineCorrectionPreset(AffineTool.CorrectPositionType.CorrectSurface, affine, GameConst.Terrain_LayerMask);
                    o_UV = o_UV.RotationVectorByPivot(Vector3.down, 10f);
                    ProjectilePoolManager.GetInstanceUnsafe
                        .SpawnRoundProjectile(createParams, caster, affineCorrect, o_UV, moveSpeedRate, damageRate, 20f, _LegCount);
                   
                    await UniTask.Delay(100, cancellationToken: p_CancellationToken);
                }
            }
        }
  
        /// <summary>
        /// 원 모양 탄막 생성
        /// </summary>
        public class Hazushi_02 : GameEntityDeployEventBase
        {
            private int _CircleCount;
            private int _ShotCount;
            private float _DeltaTheta;

            public Hazushi_02(int p_CircleCount, int p_ShotCount, float p_DeltaTheta)
            {
                _CircleCount = p_CircleCount;
                _ShotCount = p_ShotCount;
                _DeltaTheta = p_DeltaTheta;
            }
            
            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                var createParams = param.GetShotCreateParams(0);
                param.TryGetAutoAimDirection(EntityQueryTool.FilterResultType.RemainNearestOne, out var o_UV);
                var caster = param.Caster;
                var spawnScale = caster[StatusTool.ShotStatusGroupType.Total][ShotStatusTool.ShotStatusType.ShotScale];
                var damageRate = 2f / _ShotCount;
                var moveSpeedRate = 4f;

                for (int i = 0; i < _CircleCount; i++)
                {
                    var spawnPos = caster.GetCenterPosition();
                    var affine = new AffinePreset(spawnPos, spawnScale);
                    var affineCorrect = new AffineCorrectionPreset(AffineTool.CorrectPositionType.CorrectSurface, affine, GameConst.Terrain_LayerMask);
                    o_UV = o_UV.RotationVectorByPivot(Vector3.down, _DeltaTheta);
                    ProjectilePoolManager.GetInstanceUnsafe
                        .SpawnRoundProjectile(createParams, caster, affineCorrect, o_UV, moveSpeedRate, damageRate, 20f, _ShotCount);
                    
                    await UniTask.Delay(300, cancellationToken: p_CancellationToken);
                }
            }
        }  
 
        /// <summary>
        /// 반시계 나선 탄막 생성
        /// </summary>
        public class Hazushi_03 : GameEntityDeployEventBase
        {
            private int _ShotCount;
            private float _Theta;
            private float _DeltaTheta;
            
            public Hazushi_03(int p_ShotCount, float p_Theta)
            {
                _ShotCount = p_ShotCount;
                _Theta = p_Theta;
                _DeltaTheta = _Theta / _ShotCount;
            }
            
            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                var createParams = param.GetShotCreateParams(0);
                param.TryGetAutoAimDirection(EntityQueryTool.FilterResultType.RemainNearestOne, out var o_UV);
                var caster = param.Caster;
                var spawnScale = caster[StatusTool.ShotStatusGroupType.Total][ShotStatusTool.ShotStatusType.ShotScale];
                var damageRate = 2f / _ShotCount;
                var moveSpeedRate = 8f;
                
                for (int i = 0; i < _ShotCount; i++)
                {
                    var spawnPos = caster.GetCenterPosition();
                    var affine = new AffinePreset(spawnPos, spawnScale);
                    var affineCorrect = new AffineCorrectionPreset(AffineTool.CorrectPositionType.CorrectSurface, affine, GameConst.Terrain_LayerMask);
                    var direction = o_UV.RotationVectorByPivot(Vector3.up, _Theta - i * _DeltaTheta);
                    var proj = ProjectilePoolManager.GetInstanceUnsafe
                        .SpawnProjectile(createParams, caster, affineCorrect, direction, moveSpeedRate, damageRate, 10f);
                    // proj.RunAffineEvent(2001, default);
                    
                    await UniTask.Delay(75, cancellationToken: p_CancellationToken);
                }
            }
        }

        /// <summary>
        /// 시계 나선 탄막 생성
        /// </summary>
        public class Hazushi_04 : GameEntityDeployEventBase
        {
            private int _ShotCount;
            private float _Theta;
            private float _DeltaTheta;
            
            public Hazushi_04(int p_ShotCount, float p_Theta)
            {
                _ShotCount = p_ShotCount;
                _Theta = p_Theta;
                _DeltaTheta = _Theta / _ShotCount;
            }
            
            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                var createParams = param.GetShotCreateParams(0);
                param.TryGetAutoAimDirection(EntityQueryTool.FilterResultType.RemainNearestOne, out var o_UV);
                var caster = param.Caster;
                var spawnScale = caster[StatusTool.ShotStatusGroupType.Total][ShotStatusTool.ShotStatusType.ShotScale];
                var damageRate = 2f / _ShotCount;
                var moveSpeedRate = 8f;
                
                for (int i = 0; i < _ShotCount; i++)
                {
                    var spawnPos = caster.GetCenterPosition();
                    var affine = new AffinePreset(spawnPos, spawnScale);
                    var affineCorrect = new AffineCorrectionPreset(AffineTool.CorrectPositionType.CorrectSurface, affine, GameConst.Terrain_LayerMask);
                    var direction = o_UV.RotationVectorByPivot(Vector3.down, _Theta - i * _DeltaTheta);
                    var proj = ProjectilePoolManager.GetInstanceUnsafe
                        .SpawnProjectile(createParams, caster, affineCorrect, direction, moveSpeedRate, damageRate, 10f);
                    // proj.RunAffineEvent(2002, default);
                    
                    await UniTask.Delay(75, cancellationToken: p_CancellationToken);
                }
            }
        }
        
        /// <summary>
        /// 원 모양 탄막 생성
        /// </summary>
        public class Hazushi_05 : GameEntityDeployEventBase
        {
            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                var createParams = param.GetShotCreateParams(0);
                param.TryGetAutoAimDirection(EntityQueryTool.FilterResultType.RemainNearestOne, out var o_UV);
                var caster = param.Caster;
                var spawnScale = 0.8f * caster[StatusTool.ShotStatusGroupType.Total][ShotStatusTool.ShotStatusType.ShotScale];
                var moveSpeedRate = 30f;
                var shotCount = 6 + 3 * p_Handler.CommonParams.Count;
                var damageRate = 0.5f;
                
                var spawnPos = caster.GetCenterPosition();
                var affine = new AffinePreset(spawnPos, spawnScale);
                var affineCorrect = new AffineCorrectionPreset(AffineTool.CorrectPositionType.CorrectSurface, affine, GameConst.Terrain_LayerMask);
                ProjectilePoolManager.GetInstanceUnsafe
                    .SpawnRoundProjectile(createParams, caster, affineCorrect, o_UV, moveSpeedRate, damageRate, 20f, shotCount);

            }
        }  
        
        /// <summary>
        /// 좌우로 1발씩 탄막 생성
        /// </summary>
        public class Hazushi_06 : GameEntityDeployEventBase
        {
            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                var createParams = param.GetShotCreateParams(0);
                param.TryGetAutoAimDirection(EntityQueryTool.FilterResultType.RemainNearestOne, out var o_UV);
                var caster = param.Caster;
                var spawnScale = 0.8f * caster[StatusTool.ShotStatusGroupType.Total][ShotStatusTool.ShotStatusType.ShotScale];
                var moveSpeedRate = 8f;
                var damageRate = 0.2f;
                
                var spawnPos = caster.GetCenterPosition();
                var affine = new AffinePreset(spawnPos, spawnScale);
                var affineCorrect = new AffineCorrectionPreset(AffineTool.CorrectPositionType.CorrectSurface, affine, GameConst.Terrain_LayerMask);
                
                ProjectilePoolManager.GetInstanceUnsafe.SpawnForwardProjectile(createParams, caster, affineCorrect, o_UV.RotationVectorByPivot(Vector3.up, 90f), moveSpeedRate, damageRate, 8f);
                ProjectilePoolManager.GetInstanceUnsafe.SpawnForwardProjectile(createParams, caster, affineCorrect, o_UV.RotationVectorByPivot(Vector3.down, 90f), moveSpeedRate, damageRate, 8f);
            }
        }  
        
        #endregion
    }
}