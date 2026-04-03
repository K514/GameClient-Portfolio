using System.Threading;
using Cysharp.Threading.Tasks;
using k514.Mono.Feature;
using UnityEngine;

namespace k514.Mono.Common
{
    public partial class GameEntityDeployStorage 
    {
        #region <Callbacks>

        private void OnCreateProjector()
        {
            _EntityEventTable.Add(33000, new Projector_00());
            _EntityEventTable.Add(33009, new Projector_00_ex());
            _EntityEventTable.Add(33010, new Projector_01());
            _EntityEventTable.Add(33019, new Projector_01_ex());
            _EntityEventTable.Add(33020, new Projector_02());
            _EntityEventTable.Add(33029, new Projector_02_ex());
            _EntityEventTable.Add(33030, new Projector_03());
            _EntityEventTable.Add(33039, new Projector_03_ex());
        }

        #endregion
        
        #region <Classess>
        
        /// <summary>
        /// 장판 1개 생성
        /// </summary>
        public class Projector_00 : GameEntityDeployEventBase
        {
            private ProjectorPoolManager.CreateParams _ProjectorCreateParams;

            public Projector_00()
            {
                _ProjectorCreateParams = ProjectorPoolManager.GetInstanceUnsafe.GetCreateParams(1);
            }

            public override void PreloadDeployEvent()
            {
                base.PreloadDeployEvent();
                
                GetInstanceUnsafe.PreloadDeployEvent(33009);
            }
            
            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                if (param.TryGetAutoAimDirection(EntityQueryTool.FilterResultType.RemainNearestOne, out var o_UV, out var o_Enemy))
                {
                    var caster = param.Caster;
                    var duration = 3f;
                    ProjectorPoolManager.GetInstanceUnsafe.SpawnProjector(_ProjectorCreateParams, caster, new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, o_Enemy.GetBottomPosition()), o_UV, 1f, 33009, ProjectorTool.ActivateParamsAttributeType.None, 8f, 8f, 0.2f, duration, 0.05f);
                }
            }
        }
        
        /// <summary>
        /// 장판 폭발
        /// </summary>
        public class Projector_00_ex : GameEntityDeployEventBase
        {
            private VfxPoolManager.CreateParams _EffectCreateParams;

            public Projector_00_ex()
            {
                _EffectCreateParams = VfxPoolManager.GetInstanceUnsafe.GetCreateParams(110);
            }

            public override void PreloadDeployEvent()
            {
                base.PreloadDeployEvent();
                
            }
            
            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                var caster = param.Caster;
                var vfx = VfxPoolManager.GetInstanceUnsafe.Pop(_EffectCreateParams, new VfxPoolManager.ActivateParams(null, new AffineCorrectionPreset(AffineTool.CorrectPositionType.CorrectSurface, caster.GetBottomPosition())));
                vfx.SetScaleFactor(2f); 
                
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
                                var damage = 5f * caster[StatusTool.BattleStatusGroupType.Total].GetScaledAttackBase(DamageCalculator.DamageCalcType.Melee);
                                filtered.GiveDamage(damage, new StatusTool.StatusChangeParams(StatusTool.StatusChangeEventType.Combat, caster.GetMaster(), filtered.GetCenterPosition()));
                                break;
                            }
                        }
                    }
                }
            }
        }
        
        /// <summary>
        /// 장판 8개 생성
        /// </summary>
        public class Projector_01 : GameEntityDeployEventBase
        {
            private ProjectorPoolManager.CreateParams _ProjectorCreateParams;

            public Projector_01()
            {
                _ProjectorCreateParams = ProjectorPoolManager.GetInstanceUnsafe.GetCreateParams(1);
            }

            public override void PreloadDeployEvent()
            {
                base.PreloadDeployEvent();
                
                GetInstanceUnsafe.PreloadDeployEvent(33019);
            }

            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                param.TryGetAutoAimDirection(EntityQueryTool.FilterResultType.RemainNearestOne, out var o_UV);
                var caster = param.Caster;
                var duration = 2f;

                var burstCount = 8;
                for (int i = 0; i < burstCount; i++)
                {
                    if (caster.TryGetCurrentEnemy(out var o_Enemy))
                    {
                        await UniTask.Delay(150, cancellationToken: p_CancellationToken);

                        var burstPosition = o_Enemy.GetBottomPosition().GetRandomPosition(XYZType.ZX, 0f, 2f);
                        ProjectorPoolManager.GetInstanceUnsafe.SpawnProjector(_ProjectorCreateParams, caster, new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, burstPosition), o_UV, 1f, 33019, ProjectorTool.ActivateParamsAttributeType.None, 2f, 2f, 0.2f, 2f, 0.05f);
                    }
                }
            }
        }
        
        /// <summary>
        /// 장판 폭발
        /// </summary>
        public class Projector_01_ex: GameEntityDeployEventBase
        {
            private VfxPoolManager.CreateParams _EffectCreateParams;

            public Projector_01_ex()
            {
                _EffectCreateParams = VfxPoolManager.GetInstanceUnsafe.GetCreateParams(110);
            }

            public override void PreloadDeployEvent()
            {
                base.PreloadDeployEvent();
                
            }
            
            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                var caster = param.Caster;
                var vfx = VfxPoolManager.GetInstanceUnsafe.Pop(_EffectCreateParams, new VfxPoolManager.ActivateParams(null, new AffineCorrectionPreset(AffineTool.CorrectPositionType.CorrectSurface, caster.GetBottomPosition())));

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
        /// 장판 1개 생성
        /// </summary>
        public class Projector_02 : GameEntityDeployEventBase
        {
            private ProjectorPoolManager.CreateParams _ProjectorCreateParams;

            public Projector_02()
            {
                _ProjectorCreateParams = ProjectorPoolManager.GetInstanceUnsafe.GetCreateParams(1);
            }

            public override void PreloadDeployEvent()
            {
                base.PreloadDeployEvent();
                
                GetInstanceUnsafe.PreloadDeployEvent(33029);
            }

            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                if (param.TryGetAutoAimDirection(EntityQueryTool.FilterResultType.RemainNearestOne, out var o_UV, out var o_Enemy))
                {
                    var caster = param.Caster;
                    var duration = 2f;
                    var burstPosition = o_Enemy.GetBottomPosition();
                    ProjectorPoolManager.GetInstanceUnsafe.SpawnProjector(_ProjectorCreateParams, caster, new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, burstPosition), o_UV, 1f, 33029, ProjectorTool.ActivateParamsAttributeType.None, 2f, 2f, 0.2f, duration, 0.05f);
                }
            }
        }
        
        /// <summary>
        /// 8 * 2 원형 탄막
        /// </summary>
        public class Projector_02_ex: GameEntityDeployEventBase
        {
            private VfxPoolManager.CreateParams _EffectCreateParams;
            private ProjectilePoolManager.CreateParams _ProjectileCreateParams;

            public Projector_02_ex()
            {
                _EffectCreateParams = VfxPoolManager.GetInstanceUnsafe.GetCreateParams(110);
                _ProjectileCreateParams = ProjectilePoolManager.GetInstanceUnsafe.GetCreateParams(6);
            }

            public override void PreloadDeployEvent()
            {
                base.PreloadDeployEvent();
                
            }

            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                param.TryGetAutoAimDirection(EntityQueryTool.FilterResultType.RemainNearestOne, out var o_UV);
                var caster = param.Caster;
                var spawnScale = caster[StatusTool.ShotStatusGroupType.Total][ShotStatusTool.ShotStatusType.ShotScale];
                var vfx = VfxPoolManager.GetInstanceUnsafe.Pop(_EffectCreateParams, new VfxPoolManager.ActivateParams(null, new AffineCorrectionPreset(AffineTool.CorrectPositionType.CorrectSurface, caster.GetBottomPosition())));
                vfx.SetScaleFactor(1.5f);
                
                var moveSpeedRate = 4f;
                var circleCount = 2;
                var shotCount = 8;
                var damageRate = 1f / shotCount;

                for (int i = 0; i < circleCount; i++)
                {
                    var spawnPos = caster.GetCenterPosition();
                    var affine = new AffinePreset(spawnPos, spawnScale);
                    var affineCorrect = new AffineCorrectionPreset(AffineTool.CorrectPositionType.CorrectSurface, affine, GameConst.Terrain_LayerMask);
                    o_UV = o_UV.RotationVectorByPivot(Vector3.down, 22.5f + 11.25f * i);
                    ProjectilePoolManager.GetInstanceUnsafe
                        .SpawnRoundProjectile(_ProjectileCreateParams, caster, affineCorrect, o_UV, moveSpeedRate, damageRate, 20f, shotCount);
                    await UniTask.Delay(500, cancellationToken: p_CancellationToken);
                }
            }
        }
        
        /// <summary>
        /// 장판 1개 생성
        /// </summary>
        public class Projector_03 : GameEntityDeployEventBase
        {
            private ProjectorPoolManager.CreateParams _ProjectorCreateParams;

            public Projector_03()
            {
                _ProjectorCreateParams = ProjectorPoolManager.GetInstanceUnsafe.GetCreateParams(2);
            }

            public override void PreloadDeployEvent()
            {
                base.PreloadDeployEvent();
                
                GetInstanceUnsafe.PreloadDeployEvent(33039);
            }
            
            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                param.TryGetAutoAimDirection(EntityQueryTool.FilterResultType.RemainNearestOne, out var o_UV);
                var caster = param.Caster;
                var duration = 2f;
                
                caster.AddState(GameEntityTool.EntityStateType.BLOCK_MOVE);
                ProjectorPoolManager.GetInstanceUnsafe.SpawnProjector(_ProjectorCreateParams, caster, new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, caster.GetBottomPosition()), o_UV, 1f, 33039, ProjectorTool.ActivateParamsAttributeType.None, 24f, 24f, 0.2f, duration, 0.05f);
            }
        }
        
        /// <summary>
        /// 장판 회복
        /// </summary>
        public class Projector_03_ex : GameEntityDeployEventBase
        {
            private VfxPoolManager.CreateParams _EffectCreateParams;

            public Projector_03_ex()
            {
                _EffectCreateParams = VfxPoolManager.GetInstanceUnsafe.GetCreateParams(165);
            }

            public override void PreloadDeployEvent()
            {
                base.PreloadDeployEvent();
                
            }
            
            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                var caster = param.Caster;
                var vfx = VfxPoolManager.GetInstanceUnsafe.Pop(_EffectCreateParams, new VfxPoolManager.ActivateParams(null, new AffineCorrectionPreset(AffineTool.CorrectPositionType.CorrectSurface, caster.GetBottomPosition())));
                vfx.SetScaleFactor(5f);
                
                var masterValid = caster.TryGetMaster(out var o_Master);
                if (masterValid)
                {
                    o_Master.RemoveState(GameEntityTool.EntityStateType.BLOCK_MOVE);
                }
  
                /*var queryParams 
                    = new EntityQueryTool.FilterQueryParams
                    (
                        default, GameEntityTool.GameEntityGroupRelateType.Ally, 
                        EntityQueryTool.FilterQueryFlagType.FreeAll, 
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
                                var heal = 5f * caster[StatusTool.BattleStatusGroupType.Total].GetScaledAttackBase(DamageCalculator.DamageCalcType.Melee);
                                filtered.HealHP(heal);
                                break;
                            }
                        }
                    }
                }
            }
        }
        
        #endregion
    }
}