using System.Threading;
using Cysharp.Threading.Tasks;
using k514.Mono.Feature;
using UnityEngine;

namespace k514.Mono.Common
{
    public partial class GameEntityDeployStorage 
    {
        #region <Callbacks>

        private void OnCreateArcher()
        {
            _EntityEventTable.Add(10040000, new Archer_Main_00());
            _EntityEventTable.Add(10041000, new Archer_Sub_00());
            _EntityEventTable.Add(10041009, new Archer_Sub_00_ex());
        }

        #endregion

        #region <Classess>
        
        /// <summary>
        /// 왼손 1 + 2 * shotCount 방사
        /// </summary>
        public class Archer_Main_00 : GameEntityDeployEventBase
        {
            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                var createParams = param.GetShotCreateParams(0);
                param.TryGetAutoAimDirection(EntityQueryTool.FilterResultType.RemainNearestOne, out var o_UV);
                var caster = param.Caster;
                var spawnPos = caster.GetCenterPosition();
                var shotScale = 0.6f * caster[StatusTool.ShotStatusGroupType.Total][ShotStatusTool.ShotStatusType.ShotScale];
                var affine = new AffinePreset(spawnPos, shotScale);
                var affineCorrect = new AffineCorrectionPreset(AffineTool.CorrectPositionType.CorrectSurface, affine, GameConst.Terrain_LayerMask);
                var moveSpeedRate = 20f;
                var sideShotCount = (int) caster[StatusTool.ShotStatusGroupType.Total, ShotStatusTool.ShotStatusType.ShotCount] - 1;
                var shotCount = 1 + 2 * sideShotCount;
                var damageRate =  caster[StatusTool.ShotStatusGroupType.Total][ShotStatusTool.ShotStatusType.ShotPower] / shotCount;
                var shotLifeSpan = 0.5f;
                                
                ProjectilePoolManager.GetInstanceUnsafe.SpawnSpreadProjectile(createParams, caster, affineCorrect, o_UV, moveSpeedRate, damageRate, shotLifeSpan, 6f + 6f * sideShotCount, shotCount);
            }
        }

        /// <summary>
        /// 전방 하늘로 1발 사격
        /// </summary>
        public class Archer_Sub_00 : GameEntityDeployEventBase
        {
            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                param.TryGetAutoAimDirection(EntityQueryTool.FilterResultType.RemainNearestOne, out var o_UV);
                var caster = param.Caster;
                var spawnPos = caster.GetCenterPosition();
                var shotDirection = (o_UV + 2f * Vector3.up).normalized;
                var createParams = param.GetShotCreateParams(0);

                ProjectilePoolManager.GetInstanceUnsafe.SpawnForwardProjectile(createParams, caster, new AffineCorrectionPreset(AffineTool.CorrectPositionType.CorrectSurface, spawnPos, GameConst.Terrain_LayerMask), shotDirection, 45f, 1f, 0.3f, default, p_ProjectileActivateParamsAttributeType: ProjectileTool.ActivateParamsAttributeType.GiveNonCollision);
            }
        }
        
        /// <summary>
        /// 하늘로 올라간 화살이 분열하여 지상 폭격
        /// </summary>
        public class Archer_Sub_00_ex : GameEntityDeployEventBase
        {
            private GearPoolManager.CreateParams _GearCreateParams;

            public Archer_Sub_00_ex()
            {
                _GearCreateParams = GearPoolManager.GetInstanceUnsafe.GetCreateParams(2);
            }

            public override void PreloadDeployEvent()
            {
                base.PreloadDeployEvent();
                
            }
            
            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                // 쏘아올린 화살
                var param = p_Handler.DeployParams;
                var caster = param.Caster as ProjectileEntityBase;
                
                // 쏜 화살 주인
                var spawnPos = caster.GetBottomPosition();
                var damageRate = 0.25f;
                var createParams = caster.GetCreateParams();

                GearPoolManager.GetInstanceUnsafe.SpawnGear
                (
                    _GearCreateParams, 
                    new GearPoolManager.ActivateParams
                    (
                        null, 
                        new AffineCorrectionPreset(AffineTool.CorrectPositionType.CorrectSurface, spawnPos, GameConst.Terrain_LayerMask), 
                        GameEntityTool.ActivateParamsAttributeType.GiveFollowFallenMaster, 
                        caster, default, damageRate
                    )
                );
            }
        }

        #endregion
    }
}