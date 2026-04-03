using System.Threading;
using Cysharp.Threading.Tasks;
using k514.Mono.Feature;
using UnityEngine;

namespace k514.Mono.Common
{
    public partial class GameEntityDeployStorage 
    {
        #region <Callbacks>

        private void OnCreateMage()
        {
            _EntityEventTable.Add(10030000, new Mage_Main_00());
            _EntityEventTable.Add(10031000, new Mage_Sub_00());
            _EntityEventTable.Add(10031009, new Mage_Sub_00_ex());
            _EntityEventTable.Add(10031010, new Mage_Sub_01());
        }

        #endregion

        #region <Classess>
        
        /// <summary>
        /// 오른손 shotCount 레이저 발사
        /// </summary>
        public class Mage_Main_00 : GameEntityDeployEventBase
        {
            private BeamPoolManager.CreateParams _BeamCreateParams;

            public Mage_Main_00()
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
                var scale = 2.5f * caster.Scale;
                var spawnPos = new AffinePreset(caster.GetCenterPosition(), scale);
                var isEnemyAimed = param.TryGetAutoAimDirection(EntityQueryTool.FilterResultType.SortDistanceAscendant, out var o_UV);
                var perpDirection = o_UV.GetXZPerpendicularUnitVector();
                var shotCount = (int) caster[StatusTool.ShotStatusGroupType.Total, ShotStatusTool.ShotStatusType.ShotCount];
                var damageRate = caster[StatusTool.ShotStatusGroupType.Total][ShotStatusTool.ShotStatusType.ShotPower] / shotCount;
                var pierceCount = 0;
 
                for (var i = 0; i < shotCount; i++)
                {
                    await UniTask.Delay(50, cancellationToken: p_CancellationToken);

                    var enemyGroup = caster.GetEnemyGroup();

                    // 첫번째 레이저
                    if (i == 0)
                    {
                        var subAffineCorrect = new AffineCorrectionPreset(AffineTool.CorrectPositionType.CorrectSurface, spawnPos + 0.5f * scale * o_UV, GameConst.Terrain_LayerMask);
                        BeamPoolManager.GetInstanceUnsafe.SpawnBeam(_BeamCreateParams, caster, subAffineCorrect, o_UV, damageRate, 20f, 5, 0.6f, 0.1f, pierceCount);
                    }
                    else
                    {
                        if (isEnemyAimed)
                        {
                            if (enemyGroup.TryGetRandomElement(out var o_Enemy))
                            {
                                var subSpawnPos = spawnPos + CustomMath.GetRandomSymmetric(0.5f, 2f) * perpDirection + Random.Range(3f, 4.5f) * Vector3.up;
                                var subAffineCorrect = new AffineCorrectionPreset(AffineTool.CorrectPositionType.CorrectSurface, subSpawnPos, GameConst.Terrain_LayerMask);
                                var subShotDirection = subSpawnPos.GetDirectionUnitVectorTo(o_Enemy.GetCenterPosition());
                                BeamPoolManager.GetInstanceUnsafe.SpawnBeam(_BeamCreateParams, caster, subAffineCorrect, subShotDirection, damageRate, 20f, 5, 0.6f, 0.1f, pierceCount);
                            }
                            else
                            {
                                var subSpawnPos = spawnPos + CustomMath.GetRandomSymmetric(0.5f, 2f) * perpDirection + Random.Range(3f, 4.5f) * Vector3.up;
                                var subAffineCorrect = new AffineCorrectionPreset(AffineTool.CorrectPositionType.CorrectSurface, subSpawnPos, GameConst.Terrain_LayerMask);
                                BeamPoolManager.GetInstanceUnsafe.SpawnBeam(_BeamCreateParams, caster, subAffineCorrect, o_UV, damageRate, 20f, 5, 0.6f, 0.1f, pierceCount);
                            }
                        }
                        else
                        {
                            var subSpawnPos = spawnPos + CustomMath.GetRandomSymmetric(0.5f, 2f) * perpDirection + Random.Range(3f, 4.5f) * Vector3.up;
                            var subAffineCorrect = new AffineCorrectionPreset(AffineTool.CorrectPositionType.CorrectSurface, subSpawnPos, GameConst.Terrain_LayerMask);
                            BeamPoolManager.GetInstanceUnsafe.SpawnBeam(_BeamCreateParams, caster, subAffineCorrect, o_UV, damageRate, 20f, 5, 0.6f, 0.1f, pierceCount);
                        }
                    }
                }
            }
        }
        
        /// <summary>
        /// 주변 랜덤한 위치에 장판 생성
        /// </summary>
        public class Mage_Sub_00 : GameEntityDeployEventBase
        {
            private ProjectorPoolManager.CreateParams _ProjectorCreateParams;

            public Mage_Sub_00()
            {
                _ProjectorCreateParams = ProjectorPoolManager.GetInstanceUnsafe.GetCreateParams(3);
            }

            public override void PreloadDeployEvent()
            {
                base.PreloadDeployEvent();
                
                GetInstanceUnsafe.PreloadDeployEvent(10031009);
            }
            
            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                param.TryGetAutoAimDirection(EntityQueryTool.FilterResultType.RemainNearestOne, out var o_UV);
                var caster = param.Caster;

                ProjectorPoolManager.GetInstanceUnsafe.SpawnProjector(_ProjectorCreateParams, caster, new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, new AffinePreset(caster.GetBottomPosition(), 1f)), o_UV, 1f, 10031009, ProjectorTool.ActivateParamsAttributeType.None, 6f, 6f, 0.2f, 0.3f, 0.1f);
                for (var i = 0; i < 6; i++)
                {
                    await UniTask.Delay(100, cancellationToken: p_CancellationToken);
                    ProjectorPoolManager.GetInstanceUnsafe.SpawnProjector(_ProjectorCreateParams, caster, new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, new AffinePreset(caster.GetBottomPosition().GetRandomPosition(XYZType.ZX, 0f, 6f), 0.8f)), o_UV, 1f, 10031009, ProjectorTool.ActivateParamsAttributeType.None, 6f, 6f, 0.2f, 0.3f, 0.1f);

                    for (int j = 0; j < 3; j++)
                    {
                        await UniTask.Delay(50, cancellationToken: p_CancellationToken);
                        var rand = Random.Range(2, 4);
                        for (int k = 0; k < rand; k++)
                        {
                            ProjectorPoolManager.GetInstanceUnsafe.SpawnProjector(_ProjectorCreateParams, caster, new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, new AffinePreset(caster.GetBottomPosition().GetRandomPosition(XYZType.ZX, 0f, 9f), 0.6f)), o_UV, 1f, 10031009, ProjectorTool.ActivateParamsAttributeType.None, 6f, 6f, 0.2f, 0.3f, 0.1f);
                        } 
                    }
                }
            }
        }
        
        /// <summary>
        /// 장판에 낙뢰 공격
        /// </summary>
        public class Mage_Sub_00_ex : GameEntityDeployEventBase
        {
            private VfxPoolManager.CreateParams _EffectCreateParams;

            public Mage_Sub_00_ex()
            {
                _EffectCreateParams = VfxPoolManager.GetInstanceUnsafe.GetCreateParams(102);
            }

            public override void PreloadDeployEvent()
            {
                base.PreloadDeployEvent();
                
            }
            
            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                var caster = param.Caster;

                VfxPoolManager.GetInstanceUnsafe.Pop(_EffectCreateParams, new VfxPoolManager.ActivateParams(null, new AffineCorrectionPreset(AffineTool.CorrectPositionType.ForceSurface, new AffinePreset(caster.GetCenterPosition(), caster.Scale), GameConst.Terrain_LayerMask), GameEntityTool.ActivateParamsAttributeType.None, null, default));
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
                                var damage = caster[StatusTool.BattleStatusGroupType.Total].GetScaledAttackBase(DamageCalculator.DamageCalcType.Melee);
                                filtered.GiveDamage(damage, new StatusTool.StatusChangeParams(StatusTool.StatusChangeEventType.Combat, caster.GetMaster(), filtered.GetCenterPosition()));
                                break;
                            }
                        }
                    }
                }
            }
        }
        
        /// <summary>
        /// 적을 추적하며 지표면을 수직으로 폭격하는 레이저를 생성
        /// </summary>
        public class Mage_Sub_01 : GameEntityDeployEventBase
        {
            private GearPoolManager.CreateParams _GearCreateParams;

            public Mage_Sub_01()
            {
                _GearCreateParams = GearPoolManager.GetInstanceUnsafe.GetCreateParams(3);
            }

            public override void PreloadDeployEvent()
            {
                base.PreloadDeployEvent();
                
            }
            
            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                var caster = param.Caster;
                var hasEnemy = param.TryGetAutoAimDirection(EntityQueryTool.FilterResultType.SortDistanceAscendant, out var o_UV, out var o_Enemy);
                var spawnPos = hasEnemy ? o_Enemy.GetBottomPosition() : caster.GetBottomPosition();
                var affinePreset = new AffinePreset(spawnPos);
                var affineCorrection = new AffineCorrectionPreset(AffineTool.CorrectPositionType.ForceSurface, affinePreset, GameConst.Terrain_LayerMask);
                var damageRate = 0.5f;

                GearPoolManager.GetInstanceUnsafe.SpawnGear
                (
                    _GearCreateParams, 
                    new GearPoolManager.ActivateParams
                    (
                        null, affineCorrection, 
                        GameEntityTool.ActivateParamsAttributeType.GiveFollowFallenMaster, caster, 
                        p_DamageRate: damageRate
                    )
                );
            }
        }

        #endregion
    }
}