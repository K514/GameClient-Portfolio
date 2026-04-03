using System.Threading;
using Cysharp.Threading.Tasks;

namespace k514.Mono.Common
{
    public partial class GameEntityDeployStorage 
    {
        #region <Callbacks>

        private void OnCreateAssassin()
        {
            _EntityEventTable.Add(10010000, new Assassin_Main_00());
            _EntityEventTable.Add(10010001, new Assassin_Main_01());
            _EntityEventTable.Add(10010002, new Assassin_Main_02());
            _EntityEventTable.Add(10011000, new Assassin_Sub_00());
        }

        #endregion

        #region <Classess>
        
        /// <summary>
        /// 왼손 shotCount 횡렬 발사
        /// </summary>
        public class Assassin_Main_00 : GameEntityDeployEventBase
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
                var moveSpeedRate = 24f;
                var shotCount = (int) caster[StatusTool.ShotStatusGroupType.Total, ShotStatusTool.ShotStatusType.ShotCount];
                var damageRate = 0.5f * caster[StatusTool.ShotStatusGroupType.Total][ShotStatusTool.ShotStatusType.ShotPower] / shotCount;
                var shotLifeSpan = 0.35f;

                ProjectilePoolManager.GetInstanceUnsafe
                    .SpawnWidthProjectile(createParams, caster, affineCorrect, o_UV, moveSpeedRate, damageRate, shotLifeSpan, 0.25f, shotCount);
            }
        }
        
        /// <summary>
        /// 오른손 shotCount 횡렬 발사
        /// </summary>
        public class Assassin_Main_01 : GameEntityDeployEventBase
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
                var moveSpeedRate = 24f;
                var shotCount = (int) caster[StatusTool.ShotStatusGroupType.Total, ShotStatusTool.ShotStatusType.ShotCount];
                var damageRate = 0.5f * caster[StatusTool.ShotStatusGroupType.Total][ShotStatusTool.ShotStatusType.ShotPower] / shotCount;
                var shotLifeSpan = 0.35f;

                ProjectilePoolManager.GetInstanceUnsafe
                    .SpawnWidthProjectile(createParams, caster, affineCorrect, o_UV, moveSpeedRate, damageRate, shotLifeSpan, 0.25f, shotCount);
            }
        }
        
        /// <summary>
        /// 오른손 shotCount + 2 횡렬 발사
        /// </summary>
        public class Assassin_Main_02 : GameEntityDeployEventBase
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
                var moveSpeedRate = 24f;
                var shotCount = (int) caster[StatusTool.ShotStatusGroupType.Total, ShotStatusTool.ShotStatusType.ShotCount];
                var damageRate = 0.5f * caster[StatusTool.ShotStatusGroupType.Total][ShotStatusTool.ShotStatusType.ShotPower] / shotCount;
                var shotLifeSpan = 0.35f;

                shotCount += 2;
                ProjectilePoolManager.GetInstanceUnsafe
                    .SpawnWidthProjectile(createParams, caster, affineCorrect, o_UV, moveSpeedRate, damageRate, shotLifeSpan, 0.25f, shotCount);
            }
        }
        
        /// <summary>
        /// 전방 대시하며 반대방향으로 shotCount + 1 횡렬 발사
        ///
        /// 0.4초간 무적 부여
        /// </summary>
        public class Assassin_Sub_00 : GameEntityDeployEventBase
        {
            private VfxPoolManager.CreateParams _EffectCreateParams;

            public Assassin_Sub_00()
            {
                _EffectCreateParams = VfxPoolManager.GetInstanceUnsafe.GetCreateParams(23);
            }

            public override void PreloadDeployEvent()
            {
                base.PreloadDeployEvent();
                
            }
            
            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                var caster = param.Caster;
                
                if (caster.MindModule.GetMindModuleType().IsAutonomy())
                {
                    caster.SetLookUV(-caster.Affine.forward);
                }
  
                var leaderValid = caster.TryGetPartyLeader(out var o_Leader);
                var direction = param.GetActionTriggerUV();
                caster.PhysicsModule.AddAcceleration(PhysicsTool.ForceType.Default, 3500f * direction);
                caster.AddState(GameEntityTool.EntityStateType.CLOAK);

                if (leaderValid)
                {
                    o_Leader.AddState(GameEntityTool.EntityStateType.STABLE);
                    o_Leader.SetDisable(true);
                    
                    VfxPoolManager.GetInstanceUnsafe.Pop(_EffectCreateParams, new VfxPoolManager.ActivateParams(null, new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, new AffinePreset(o_Leader.GetCenterPosition(), 2f))));
                }
                
                await UniTask.Delay(100, cancellationToken: p_CancellationToken);
                var knifeCreateParams = param.GetShotCreateParams(0);
                var spawnPos = caster.GetCenterPosition();
                var shotScale = caster[StatusTool.ShotStatusGroupType.Total][ShotStatusTool.ShotStatusType.ShotScale];
                var affine = new AffinePreset(spawnPos, shotScale);
                var affineCorrect = new AffineCorrectionPreset(AffineTool.CorrectPositionType.CorrectSurface, affine, GameConst.Terrain_LayerMask);
                var moveSpeedRate = 30f;
                var shotCount = (int) caster[StatusTool.ShotStatusGroupType.Total, ShotStatusTool.ShotStatusType.ShotCount];
                var damageRate = 0.5f * caster[StatusTool.ShotStatusGroupType.Total][ShotStatusTool.ShotStatusType.ShotPower] / shotCount;
                var shotLifeSpan = 0.6f;

                shotCount += 1;
                ProjectilePoolManager.GetInstanceUnsafe
                    .SpawnWidthProjectile(knifeCreateParams, caster, affineCorrect, -direction, moveSpeedRate, damageRate, shotLifeSpan, 0.25f, shotCount);
          
                await UniTask.Delay(400, cancellationToken: p_CancellationToken);
                caster.RemoveState(GameEntityTool.EntityStateType.CLOAK);
                
                if (leaderValid)
                {
                    o_Leader.RemoveState(GameEntityTool.EntityStateType.STABLE);
                    o_Leader.TrySetTerrainSurfacePosition(caster.GetTopPosition());
                    o_Leader.SetDisable(false);
                    
                    VfxPoolManager.GetInstanceUnsafe.Pop(_EffectCreateParams, new VfxPoolManager.ActivateParams(null, new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, new AffinePreset(o_Leader.GetCenterPosition(), 2f))));
                }
            }
        }

        #endregion
    }
}