using System.Threading;
using Cysharp.Threading.Tasks;
using k514.Mono.Feature;
using UnityEngine;

namespace k514.Mono.Common
{
    public partial class GameEntityDeployStorage 
    {
        #region <Callbacks>

        private void OnCreateKnight()
        {
            _EntityEventTable.Add(10020000, new Knight_Main_00());
            _EntityEventTable.Add(10021000, new Knight_Sub_00());
            _EntityEventTable.Add(10021009, new Knight_Sub_00_ex());
        }

        #endregion

        #region <Classess>
        
        /// <summary>
        /// shotCount 종렬 발사. 적을 관통
        /// </summary>
        public class Knight_Main_00 : GameEntityDeployEventBase
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
                var moveSpeedRate = 18f;
                var shotCount = (int) caster[StatusTool.ShotStatusGroupType.Total, ShotStatusTool.ShotStatusType.ShotCount];
                var damageRate = caster[StatusTool.ShotStatusGroupType.Total][ShotStatusTool.ShotStatusType.ShotPower] / shotCount;
                var shotLifeSpan = 0.4f;

                var firstSpear = ProjectilePoolManager.GetInstanceUnsafe.SpawnForwardProjectile(createParams, caster, affineCorrect, o_UV, moveSpeedRate, damageRate, shotLifeSpan);
                firstSpear.SetPierce(3);

                var subShotCount = shotCount - 1;
                var perp = o_UV.GetXZPerpendicularUnitVector();
                for (int i = 0; i < subShotCount; i++)
                {
                    await UniTask.Delay(50, cancellationToken: p_CancellationToken);
                    var subSpawnPos = spawnPos + CustomMath.GetRandomSymmetric(0f, 1f) * perp;
                    var subAffine = new AffinePreset(subSpawnPos, shotScale);
                    var subAffineCorrect = new AffineCorrectionPreset(AffineTool.CorrectPositionType.CorrectSurface, subAffine, GameConst.Terrain_LayerMask);
                    var extraSpear = ProjectilePoolManager.GetInstanceUnsafe.SpawnForwardProjectile(createParams, caster, subAffineCorrect, o_UV, moveSpeedRate, damageRate, shotLifeSpan);
                    extraSpear.SetPierce(3);
                }
            }
        }
        
        /// <summary>
        /// 장판 생성
        /// </summary>
        public class Knight_Sub_00 : GameEntityDeployEventBase
        {
            private ProjectorPoolManager.CreateParams _ProjectorCreateParams;

            public Knight_Sub_00()
            {
                _ProjectorCreateParams = ProjectorPoolManager.GetInstanceUnsafe.GetCreateParams(9);
            }

            public override void PreloadDeployEvent()
            {
                base.PreloadDeployEvent();
                
                GetInstanceUnsafe.PreloadDeployEvent(10021009);
            }
            
            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                param.TryGetAutoAimDirection(EntityQueryTool.FilterResultType.RemainNearestOne, out var o_UV);
                var caster = param.Caster;

                ProjectorPoolManager.GetInstanceUnsafe.SpawnProjector(_ProjectorCreateParams, caster, new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, caster.GetBottomPosition()), o_UV, 1f, 10021009, ProjectorTool.ActivateParamsAttributeType.GiveForwardPivotAttribute, 10f, 4f, 0.05f, 0.1f, 0.1f);
            }
        }
        
        /// <summary>
        /// 장판 범위 내의 탄을 튕겨냄
        /// </summary>
        public class Knight_Sub_00_ex : GameEntityDeployEventBase
        {
            private VfxPoolManager.CreateParams _EffectCreateParams;

            public Knight_Sub_00_ex()
            {
                _EffectCreateParams = VfxPoolManager.GetInstanceUnsafe.GetCreateParams(137);
            }

            public override void PreloadDeployEvent()
            {
                base.PreloadDeployEvent();
                
            }
            
            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                var caster = param.Caster as ProjectorObjectBase;
                
                /*var queryParams 
                    = new EntityQueryTool.FilterQueryParams
                    (
                        default, GameEntityTool.GameEntityGroupRelateType.Enemy, 
                        EntityQueryTool.FilterQueryFlagType.ExceptMe | EntityQueryTool.FilterQueryFlagType.FreeAll, 
                        GameEntityTool.EntityStateType.DEAD
                    );*/
                var vfx = VfxPoolManager.GetInstanceUnsafe.Pop(_EffectCreateParams, new VfxPoolManager.ActivateParams(null, new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, new AffinePreset(caster.GetCenterPositionWithOffset(), 1.5f)), GameEntityTool.ActivateParamsAttributeType.None, null, default));
                vfx.SetLookUV(caster.GetLookUV());
                
                CameraManager.GetInstanceUnsafe.SetShake(Vector3.right, 7f, 0, 150, 3);

                /*if (caster.FilterFocusEntity(queryParams))
                {
                    var filterGroupSet = caster.FilterResultGroup;
                    foreach (var filtered in filterGroupSet)
                    {
                        var entityType = filtered.WorldObjectType;
                        switch (entityType)
                        {
                            case WorldObjectTool.WorldObjectType.Unit:
                            {
                                var uv = caster.GetCenterPosition().GetDirectionVectorTo(filtered).XZUVector();
                                filtered.PhysicsModule.AddVelocity(PhysicsTool.ForceType.Default, 125f * uv);
                                break;
                            }
                            case WorldObjectTool.WorldObjectType.Projectile:
                            {
                                if (caster.TryGetMaster(out var o_Master))
                                {
                                    o_Master.AddSlave(filtered);
                                    filtered.TerminateAllAffineHandler();
                                    filtered.SetLookUV(caster.GetCenterPosition().GetDirectionXZUnitVectorTo(filtered));
                                    filtered.AddStatus(StatusTool.BattleStatusGroupType.ExtraAdd, BattleStatusTool.BattleStatusType.MoveSpeedRate, 2f);
                                    // filtered.RunAffineEvent(2000, default);
                                }
                                break;
                            }
                        }
                    }
                }*/
            }
        }

        #endregion
    }
}