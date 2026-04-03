using System.Threading;
using Cysharp.Threading.Tasks;
using k514.Mono.Feature;
using UnityEngine;

namespace k514.Mono.Common
{
    public partial class GameEntityDeployStorage 
    {
        #region <Callbacks>

        private void OnCreateMelee()
        {
            _EntityEventTable.Add(34000, new Melee_00());
            _EntityEventTable.Add(34009, new Melee_00_ex());
            _EntityEventTable.Add(34010, new Melee_01());
            _EntityEventTable.Add(34019, new Melee_01_ex());
            _EntityEventTable.Add(34020, new Melee_02());
            _EntityEventTable.Add(34029, new Melee_02_ex());
            _EntityEventTable.Add(34030, new Melee_03_00());
            _EntityEventTable.Add(34031, new Melee_03_01());
            _EntityEventTable.Add(34039, new Melee_03_ex());
            
            _EntityEventTable.Add(34050, new Melee_05());
            _EntityEventTable.Add(34059, new Melee_05_ex());
            _EntityEventTable.Add(34060, new Melee_06_00());
            _EntityEventTable.Add(34061, new Melee_06_01());
            _EntityEventTable.Add(34062, new Melee_06_02());
            _EntityEventTable.Add(34069, new Melee_06_ex());
            
            _EntityEventTable.Add(34090, new Melee_09());
            _EntityEventTable.Add(34099, new Melee_09_ex());
            _EntityEventTable.Add(34100, new Melee_10());
            _EntityEventTable.Add(34109, new Melee_10_ex());
            _EntityEventTable.Add(34110, new Melee_11());
            _EntityEventTable.Add(34119, new Melee_11_ex());
            _EntityEventTable.Add(34120, new Melee_12());
            _EntityEventTable.Add(34129, new Melee_12_ex());
            _EntityEventTable.Add(34130, new Melee_13());
            _EntityEventTable.Add(34139, new Melee_13_ex());
            _EntityEventTable.Add(34140, new Melee_14_00(3));
            _EntityEventTable.Add(34141, new Melee_14_00(5));
            _EntityEventTable.Add(34142, new Melee_14_01(4));
            _EntityEventTable.Add(34149, new Melee_14_ex());
            _EntityEventTable.Add(34150, new Melee_15_00());
            _EntityEventTable.Add(34151, new Melee_15_01());
            _EntityEventTable.Add(34159, new Melee_15_ex());
        }

        #endregion
        
        #region <Classess>
        
        public class Melee_00 : GameEntityDeployEventBase
        {
            private const float __HitRangeWidth = 2f;
            private const float __HitRangeHeight = 3f;
            
            private ProjectorPoolManager.CreateParams _ProjectorCreateParams;

            public Melee_00()
            {
                _ProjectorCreateParams = ProjectorPoolManager.GetInstanceUnsafe.GetCreateParams(4);
            }

            public override void PreloadDeployEvent()
            {
                base.PreloadDeployEvent();
                
                GetInstanceUnsafe.PreloadDeployEvent(34009);
            }
            
            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                param.TryGetAutoAimDirection(EntityQueryTool.FilterResultType.RemainNearestOne, out var o_UV);
                var caster = param.Caster;
                var duration = 0.3f;
                var spawnPos = caster.GetBottomPosition();
                
                caster.AddState(GameEntityTool.EntityStateType.BLOCK_MOVE);
                ProjectorPoolManager.GetInstanceUnsafe.SpawnProjector(_ProjectorCreateParams, caster, new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, spawnPos), o_UV, 1f, 34009, ProjectorTool.ActivateParamsAttributeType.GiveForwardPivotAttribute, __HitRangeWidth, __HitRangeHeight, 0.2f, duration, 0.05f);
            }
        }
        
        public class Melee_00_ex : GameEntityDeployEventBase
        {
            private const float __DamageRate = 0.5f;

            private VfxPoolManager.CreateParams _EffectCreateParams;

            public Melee_00_ex()
            {
                _EffectCreateParams = VfxPoolManager.GetInstanceUnsafe.GetCreateParams(10);
            }

            public override void PreloadDeployEvent()
            {
                base.PreloadDeployEvent();
                
            }
            
            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                var caster = param.Caster;
                var masterValid = caster.TryGetMaster(out var o_Master);
                if (masterValid)
                {
                    o_Master.PhysicsModule.AddVelocity(PhysicsTool.ForceType.Default, 25f * o_Master.GetLookUV());
                    o_Master.RemoveState(GameEntityTool.EntityStateType.BLOCK_MOVE);
                    
                    var activateParams = new VfxPoolManager.ActivateParams(null, new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, new AffinePreset(o_Master.GetCenterPosition(), o_Master.Scale)));
                    var vfx = VfxPoolManager.GetInstanceUnsafe.Pop(_EffectCreateParams, activateParams);
                    vfx.SetLookUV(caster.GetLookUV());
                
                    /*var queryParams 
                        = new EntityQueryTool.FilterQueryParams
                        (
                            default, GameEntityTool.GameEntityGroupRelateType.Enemy, 
                            EntityQueryTool.FilterQueryFlagType.ExceptMe | EntityQueryTool.FilterQueryFlagType.FreeAll, 
                            GameEntityTool.EntityStateType.DEAD
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
                                    filtered.GiveDamage(new StatusTool.StatusChangeParams(StatusTool.StatusChangeEventType.Combat, o_Master), __DamageRate);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
        
        public class Melee_01 : GameEntityDeployEventBase
        {
            private const float __HitRangeWidth = 2f;
            private const float __HitRangeHeight = 3f;
            
            private ProjectorPoolManager.CreateParams _ProjectorCreateParams;

            public Melee_01()
            {
                _ProjectorCreateParams = ProjectorPoolManager.GetInstanceUnsafe.GetCreateParams(4);
            }
            
            public override void PreloadDeployEvent()
            {
                base.PreloadDeployEvent();
                
                GetInstanceUnsafe.PreloadDeployEvent(34019);
            }
            
            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                param.TryGetAutoAimDirection(EntityQueryTool.FilterResultType.RemainNearestOne, out var o_UV);
                var caster = param.Caster;
                var duration = 0.3f;
                var spawnPos = caster.GetBottomPosition();
                
                caster.AddState(GameEntityTool.EntityStateType.BLOCK_MOVE);
                ProjectorPoolManager.GetInstanceUnsafe.SpawnProjector(_ProjectorCreateParams, caster, new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, spawnPos), o_UV, 1f, 34019, ProjectorTool.ActivateParamsAttributeType.GiveForwardPivotAttribute, __HitRangeWidth, __HitRangeHeight, 0.2f, duration, 0.05f);
            }
        }
        
        public class Melee_01_ex : GameEntityDeployEventBase
        {
            private const float __DamageRate = 0.5f;
            private const float __HealRate = 0.05f;
            
            private VfxPoolManager.CreateParams _EffectCreateParams;

            public Melee_01_ex()
            {
                _EffectCreateParams = VfxPoolManager.GetInstanceUnsafe.GetCreateParams(10);
            }

            public override void PreloadDeployEvent()
            {
                base.PreloadDeployEvent();
                
            }
            
            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                var caster = param.Caster;
                var masterValid = caster.TryGetMaster(out var o_Master);
                if (masterValid)
                {
                    o_Master.PhysicsModule.AddVelocity(PhysicsTool.ForceType.Default, 25f * o_Master.GetLookUV());
                    o_Master.RemoveState(GameEntityTool.EntityStateType.BLOCK_MOVE);
                    
                    var activateParams = new VfxPoolManager.ActivateParams(null, new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, new AffinePreset(o_Master.GetCenterPosition(), o_Master.Scale)));
                    var vfx = VfxPoolManager.GetInstanceUnsafe.Pop(_EffectCreateParams, activateParams);
                    vfx.SetLookUV(caster.GetLookUV());
                
                    /*var queryParams 
                        = new EntityQueryTool.FilterQueryParams
                        (
                            default, GameEntityTool.GameEntityGroupRelateType.Enemy, 
                            EntityQueryTool.FilterQueryFlagType.ExceptMe | EntityQueryTool.FilterQueryFlagType.FreeAll, 
                            GameEntityTool.EntityStateType.DEAD
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
                                    filtered.GiveDamage(new StatusTool.StatusChangeParams(StatusTool.StatusChangeEventType.Combat, o_Master), __DamageRate);
                                    o_Master.HealRateHP(__HealRate);

                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
      
        public class Melee_02 : GameEntityDeployEventBase
        {
            private const float __HitRangeWidth = 2f;
            private const float __HitRangeHeight = 7f;
                        
            private ProjectorPoolManager.CreateParams _ProjectorCreateParams;

            public Melee_02()
            {
                _ProjectorCreateParams = ProjectorPoolManager.GetInstanceUnsafe.GetCreateParams(4);
            }

            public override void PreloadDeployEvent()
            {
                base.PreloadDeployEvent();
                
                GetInstanceUnsafe.PreloadDeployEvent(34029);
            }

            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                param.TryGetAutoAimDirection(EntityQueryTool.FilterResultType.RemainNearestOne, out var o_UV);
                var caster = param.Caster;
                var duration = 0.3f;
                var spawnPos = caster.GetBottomPosition();
                
                caster.AddState(GameEntityTool.EntityStateType.BLOCK_MOVE);
                ProjectorPoolManager.GetInstanceUnsafe.SpawnProjector(_ProjectorCreateParams, caster, new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, spawnPos), o_UV, 1f, 34029, ProjectorTool.ActivateParamsAttributeType.GiveForwardPivotAttribute, __HitRangeWidth, __HitRangeHeight, 0.2f, duration, 0.05f);
            }
        }
        
        public class Melee_02_ex : GameEntityDeployEventBase
        {
            private const float __DamageRate = 0.5f;
            
            private VfxPoolManager.CreateParams _EffectCreateParams;

            public Melee_02_ex()
            {
                _EffectCreateParams = VfxPoolManager.GetInstanceUnsafe.GetCreateParams(24);
            }

            public override void PreloadDeployEvent()
            {
                base.PreloadDeployEvent();
                
            }

            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                var caster = param.Caster;
                var masterValid = caster.TryGetMaster(out var o_Master);
                if (masterValid)
                {
                    o_Master.PhysicsModule.AddVelocity(PhysicsTool.ForceType.Default, 100f * o_Master.GetLookUV());
                    o_Master.RemoveState(GameEntityTool.EntityStateType.BLOCK_MOVE);
                    
                    var scale = 4f * o_Master.Scale;
                    var activateParams = new VfxPoolManager.ActivateParams(null, new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, new AffinePreset(o_Master.GetCenterPosition(), scale)));
                    var vfx = VfxPoolManager.GetInstanceUnsafe.Pop(_EffectCreateParams, activateParams);
                    vfx.SetLookUV(caster.GetLookUV());
                
                    /*var queryParams 
                        = new EntityQueryTool.FilterQueryParams
                        (
                            default, GameEntityTool.GameEntityGroupRelateType.Enemy, 
                            EntityQueryTool.FilterQueryFlagType.ExceptMe | EntityQueryTool.FilterQueryFlagType.FreeAll, 
                            GameEntityTool.EntityStateType.DEAD
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
                                    filtered.GiveDamage(new StatusTool.StatusChangeParams(StatusTool.StatusChangeEventType.Combat, o_Master), __DamageRate);

                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
        
        public class Melee_03_00 : GameEntityDeployEventBase
        {
            private ProjectorPoolManager.CreateParams _ProjectorCreateParams;

            public Melee_03_00()
            {
                _ProjectorCreateParams = ProjectorPoolManager.GetInstanceUnsafe.GetCreateParams(4);
            }
            
            public override void PreloadDeployEvent()
            {
                base.PreloadDeployEvent();
                
                GetInstanceUnsafe.PreloadDeployEvent(34039);
            }
            
            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                param.TryGetAutoAimDirection(EntityQueryTool.FilterResultType.RemainNearestOne, out var o_UV);
                var caster = param.Caster;
                var duration = 0.5f;
                o_UV = o_UV.RotationVectorByPivot(Vector3.up, Random.Range(-45f, 0f));
                var perpDirection = o_UV.GetXZPerpendicularUnitVector();
                var gap = 0.2f;
                var damageRate = 0.33f;
                ProjectorPoolManager.GetInstanceUnsafe.SpawnProjector(_ProjectorCreateParams, caster, new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, caster.GetRelativePosition(2f) + gap * perpDirection), o_UV, damageRate, 34039, ProjectorTool.ActivateParamsAttributeType.None, 0.1f, 4f, 0.2f, duration, 0.05f);
                ProjectorPoolManager.GetInstanceUnsafe.SpawnProjector(_ProjectorCreateParams, caster, new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, caster.GetRelativePosition(2f)), o_UV, damageRate, 34039, ProjectorTool.ActivateParamsAttributeType.None, 0.1f, 4f, 0.2f, duration, 0.05f);
                ProjectorPoolManager.GetInstanceUnsafe.SpawnProjector(_ProjectorCreateParams, caster, new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, caster.GetRelativePosition(2f) - gap * perpDirection), o_UV, damageRate, 34039, ProjectorTool.ActivateParamsAttributeType.None, 0.1f, 4f, 0.2f, duration, 0.05f);
            }
        }
        
        public class Melee_03_01 : GameEntityDeployEventBase
        {
            private ProjectorPoolManager.CreateParams _ProjectorCreateParams;

            public Melee_03_01()
            {
                _ProjectorCreateParams = ProjectorPoolManager.GetInstanceUnsafe.GetCreateParams(4);
            }
            
            public override void PreloadDeployEvent()
            {
                base.PreloadDeployEvent();
                
                GetInstanceUnsafe.PreloadDeployEvent(34039);
            }

            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                param.TryGetAutoAimDirection(EntityQueryTool.FilterResultType.RemainNearestOne, out var o_UV);
                var caster = param.Caster;
                var duration = 0.5f;
                o_UV = o_UV.RotationVectorByPivot(Vector3.up, Random.Range(0f, 45f));
                var perpDirection = o_UV.GetXZPerpendicularUnitVector();
                var gap = 0.2f;
                var damageRate = 0.33f;
                ProjectorPoolManager.GetInstanceUnsafe.SpawnProjector(_ProjectorCreateParams, caster, new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, caster.GetRelativePosition(2f) + gap * perpDirection), o_UV, damageRate, 34039, ProjectorTool.ActivateParamsAttributeType.None, 0.1f, 4f, 0.2f, duration, 0.05f);
                ProjectorPoolManager.GetInstanceUnsafe.SpawnProjector(_ProjectorCreateParams, caster, new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, caster.GetRelativePosition(2f)), o_UV, damageRate, 34039, ProjectorTool.ActivateParamsAttributeType.None, 0.1f, 4f, 0.2f, duration, 0.05f);
                ProjectorPoolManager.GetInstanceUnsafe.SpawnProjector(_ProjectorCreateParams, caster, new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, caster.GetRelativePosition(2f) - gap * perpDirection), o_UV, damageRate, 34039, ProjectorTool.ActivateParamsAttributeType.None, 0.1f, 4f, 0.2f, duration, 0.05f);
            }
        }
        
        public class Melee_03_ex : GameEntityDeployEventBase
        {
            private VfxPoolManager.CreateParams _EffectCreateParams;

            public Melee_03_ex()
            {
                _EffectCreateParams = VfxPoolManager.GetInstanceUnsafe.GetCreateParams(200);
            }

            public override void PreloadDeployEvent()
            {
                base.PreloadDeployEvent();
                
            }

            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                await UniTask.Delay(100, cancellationToken: p_CancellationToken);
     
                var param = p_Handler.DeployParams;
                var caster = param.Caster;
                /*var queryParams 
                    = new EntityQueryTool.FilterQueryParams
                    (
                        default, GameEntityTool.GameEntityGroupRelateType.Enemy, 
                        EntityQueryTool.FilterQueryFlagType.ExceptMe | EntityQueryTool.FilterQueryFlagType.FreeAll, 
                        GameEntityTool.EntityStateType.DEAD
                    );
                var vfx = VfxPoolManager.GetInstanceUnsafe.Pop(_EffectCreateParams, new VfxPoolManager.ActivateParams(null, new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, new AffinePreset(caster.GetCenterPosition(), 1.5f)), GameEntityTool.ActivateParamsAttributeType.None, null, default));
                vfx.SetLookUV(caster.GetLookUV());
                
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
                                CameraManager.GetInstanceUnsafe.SetShake(Vector3.right, 7f, 0, 150, 3);
                                filtered.GiveDamage(caster[StatusTool.BattleStatusGroupType.Total].GetScaledAttackBase(DamageCalculator.DamageCalcType.Melee), new StatusTool.StatusChangeParams(StatusTool.StatusChangeEventType.Combat, caster));
                                break;
                            }
                        }
                    }
                }
            }
        }
        
        public class Melee_05 : GameEntityDeployEventBase
        {
            private ProjectorPoolManager.CreateParams _ProjectorCreateParams;

            public Melee_05()
            {
                _ProjectorCreateParams = ProjectorPoolManager.GetInstanceUnsafe.GetCreateParams(1);
            }
            
            public override void PreloadDeployEvent()
            {
                base.PreloadDeployEvent();
                
                GetInstanceUnsafe.PreloadDeployEvent(34059);
            }
            
            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                param.TryGetAutoAimDirection(EntityQueryTool.FilterResultType.RemainNearestOne, out var o_UV);
                var caster = param.Caster;
                var damageRate = 1f;
                var duration = 0.5f;
                var scale = 4f * caster.Scale;
                var spawnPos = caster.GetTopPosition();
                ProjectorPoolManager.GetInstanceUnsafe.SpawnProjector(_ProjectorCreateParams, caster, new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, spawnPos), caster.GetLookUV(), 1f, 34059, ProjectorTool.ActivateParamsAttributeType.None, scale, scale, 0.2f, duration, 0.05f);
            }
        }
        
        public class Melee_05_ex : GameEntityDeployEventBase
        {            
            private VfxPoolManager.CreateParams _EffectCreateParams;

            public Melee_05_ex()
            {
                _EffectCreateParams = VfxPoolManager.GetInstanceUnsafe.GetCreateParams(52);
            }

            public override void PreloadDeployEvent()
            {
                base.PreloadDeployEvent();
                
            }

            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                var caster = param.Caster;
                if (caster.TryGetSpawner(out var o_Spawner))
                {
                    var scale = 3f * caster.Scale;
                    var activateParams = new VfxPoolManager.ActivateParams(null, new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, new AffinePreset(caster.GetCenterPosition(), scale)));
                    var vfx = VfxPoolManager.GetInstanceUnsafe.Pop(_EffectCreateParams, activateParams);
                    
                    /*var queryParams 
                        = new EntityQueryTool.FilterQueryParams
                        (
                            default, GameEntityTool.GameEntityGroupRelateType.Enemy, 
                            EntityQueryTool.FilterQueryFlagType.ExceptMe | EntityQueryTool.FilterQueryFlagType.FreeAll, 
                            GameEntityTool.EntityStateType.DEAD
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
                                    CameraManager.GetInstanceUnsafe.SetShake(Vector3.right, 7f, 0, 150, 3);
                                    filtered.GiveDamage(caster[StatusTool.BattleStatusGroupType.Total].GetScaledAttackBase(DamageCalculator.DamageCalcType.Melee), new StatusTool.StatusChangeParams(StatusTool.StatusChangeEventType.Combat, caster));
                                    break;
                                }
                            }
                        }
                    }
                    
                    o_Spawner.SetDead(false);
                }
            }
        }
        
        public class Melee_06_00 : GameEntityDeployEventBase
        {            
            private ProjectorPoolManager.CreateParams _ProjectorCreateParams;

            public Melee_06_00()
            {
                _ProjectorCreateParams = ProjectorPoolManager.GetInstanceUnsafe.GetCreateParams(1);
            }
            
            public override void PreloadDeployEvent()
            {
                base.PreloadDeployEvent();
                
                GetInstanceUnsafe.PreloadDeployEvent(34069);
            }

            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                param.TryGetAutoAimDirection(EntityQueryTool.FilterResultType.RemainNearestOne, out var o_UV);
                var caster = param.Caster;
                var damageRate = 1f;
                var duration = 0.5f;
                var scale = 5f * caster.Scale;
                var spawnPos = caster.GetBottomPosition();
                ProjectorPoolManager.GetInstanceUnsafe.SpawnProjector(_ProjectorCreateParams, caster, new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, new AffinePreset(spawnPos, scale)), caster.GetLookUV(), 1f, 34069, ProjectorTool.ActivateParamsAttributeType.None, 1f, 1f, 0.2f, duration, 0.05f);
            }
        }
        
        public class Melee_06_01 : GameEntityDeployEventBase
        {
            private ProjectorPoolManager.CreateParams _ProjectorCreateParams;

            public Melee_06_01()
            {
                _ProjectorCreateParams = ProjectorPoolManager.GetInstanceUnsafe.GetCreateParams(1);
            }
            
            public override void PreloadDeployEvent()
            {
                base.PreloadDeployEvent();
                
                GetInstanceUnsafe.PreloadDeployEvent(34069);
            }

            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                param.TryGetAutoAimDirection(EntityQueryTool.FilterResultType.RemainNearestOne, out var o_UV);
                var caster = param.Caster;
                var damageRate = 1f;
                var duration = 0.5f;
                var scale = 10f * caster.Scale;
                var spawnPos = caster.GetBottomPosition();
                ProjectorPoolManager.GetInstanceUnsafe.SpawnProjector(_ProjectorCreateParams, caster, new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, new AffinePreset(spawnPos, scale)), caster.GetLookUV(), 1f, 34069, ProjectorTool.ActivateParamsAttributeType.None, 1f, 1f, 0.2f, duration, 0.05f);
            }
        }
        
        public class Melee_06_02 : GameEntityDeployEventBase
        {
            private ProjectorPoolManager.CreateParams _ProjectorCreateParams;

            public Melee_06_02()
            {
                _ProjectorCreateParams = ProjectorPoolManager.GetInstanceUnsafe.GetCreateParams(1);
            }
            
            public override void PreloadDeployEvent()
            {
                base.PreloadDeployEvent();
                
                GetInstanceUnsafe.PreloadDeployEvent(34069);
            }
            
            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                param.TryGetAutoAimDirection(EntityQueryTool.FilterResultType.RemainNearestOne, out var o_UV);
                var caster = param.Caster;
                var damageRate = 1f;
                var duration = 0.5f;
                var scale = 15f * caster.Scale;
                var spawnPos = caster.GetBottomPosition();
                ProjectorPoolManager.GetInstanceUnsafe.SpawnProjector(_ProjectorCreateParams, caster, new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, new AffinePreset(spawnPos, scale)), caster.GetLookUV(), 1f, 34069, ProjectorTool.ActivateParamsAttributeType.None, 1f, 1f, 0.2f, duration, 0.05f);
            }
        }
        
        public class Melee_06_ex : GameEntityDeployEventBase
        {          
            private VfxPoolManager.CreateParams _EffectCreateParams;

            public Melee_06_ex()
            {
                _EffectCreateParams = VfxPoolManager.GetInstanceUnsafe.GetCreateParams(135);
            }

            public override void PreloadDeployEvent()
            {
                base.PreloadDeployEvent();
                
            }

            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                var caster = param.Caster;
                var scale = 0.4f * caster.Scale;
                var activateParams = new VfxPoolManager.ActivateParams(null, new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, new AffinePreset(caster.GetBottomPosition(), scale)));
                var vfx = VfxPoolManager.GetInstanceUnsafe.Pop(_EffectCreateParams, activateParams);
                
                /*var queryParams 
                    = new EntityQueryTool.FilterQueryParams
                    (
                        default, GameEntityTool.GameEntityGroupRelateType.Enemy, 
                        EntityQueryTool.FilterQueryFlagType.ExceptMe | EntityQueryTool.FilterQueryFlagType.FreeAll, 
                        GameEntityTool.EntityStateType.DEAD
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
                                CameraManager.GetInstanceUnsafe.SetShake(Vector3.right, 7f, 0, 150, 3);
                                filtered.GiveDamage(caster[StatusTool.BattleStatusGroupType.Total].GetScaledAttackBase(DamageCalculator.DamageCalcType.Melee), new StatusTool.StatusChangeParams(StatusTool.StatusChangeEventType.Combat, caster));
                                break;
                            }
                        }
                    }
                }
            }
        }
        
        public class Melee_09 : GameEntityDeployEventBase
        {            
            private ProjectorPoolManager.CreateParams _ProjectorCreateParams;

            public Melee_09()
            {
                _ProjectorCreateParams = ProjectorPoolManager.GetInstanceUnsafe.GetCreateParams(4);
            }
            
            public override void PreloadDeployEvent()
            {
                base.PreloadDeployEvent();
                
                GetInstanceUnsafe.PreloadDeployEvent(34099);
            }

            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                param.TryGetAutoAimDirection(EntityQueryTool.FilterResultType.RemainNearestOne, out var o_UV);
                var caster = param.Caster;
                var duration = 0.5f;
                var gap = 0.2f;
                var damageRate = 0.33f;
                
                o_UV = o_UV.RotationVectorByPivot(Vector3.up, 45f);
                var perpDirection = o_UV.GetXZPerpendicularUnitVector();
                ProjectorPoolManager.GetInstanceUnsafe.SpawnProjector(_ProjectorCreateParams, caster, new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, caster.GetRelativePosition(1.2f) + gap * perpDirection), o_UV, damageRate, 34099, ProjectorTool.ActivateParamsAttributeType.None, 0.1f, 4f, 0.2f, duration, 0.05f);
                ProjectorPoolManager.GetInstanceUnsafe.SpawnProjector(_ProjectorCreateParams, caster, new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, caster.GetRelativePosition(1.2f) - gap * perpDirection), o_UV, damageRate, 34099, ProjectorTool.ActivateParamsAttributeType.None, 0.1f, 4f, 0.2f, duration, 0.05f);
                
                o_UV = o_UV.RotationVectorByPivot(Vector3.up, -90f);
                perpDirection = o_UV.GetXZPerpendicularUnitVector();
                ProjectorPoolManager.GetInstanceUnsafe.SpawnProjector(_ProjectorCreateParams, caster, new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, caster.GetRelativePosition(1.2f) + gap * perpDirection), o_UV, damageRate, 34099, ProjectorTool.ActivateParamsAttributeType.None, 0.1f, 4f, 0.2f, duration, 0.05f);
                ProjectorPoolManager.GetInstanceUnsafe.SpawnProjector(_ProjectorCreateParams, caster, new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, caster.GetRelativePosition(1.2f) - gap * perpDirection), o_UV, damageRate, 34099, ProjectorTool.ActivateParamsAttributeType.None, 0.1f, 4f, 0.2f, duration, 0.05f);
            }
        }
        
        public class Melee_09_ex : GameEntityDeployEventBase
        {          
            private VfxPoolManager.CreateParams _EffectCreateParams;

            public Melee_09_ex()
            {
                _EffectCreateParams = VfxPoolManager.GetInstanceUnsafe.GetCreateParams(200);
            }

            public override void PreloadDeployEvent()
            {
                base.PreloadDeployEvent();
                
            }

            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                await UniTask.Delay(100, cancellationToken: p_CancellationToken);
     
                var param = p_Handler.DeployParams;
                var caster = param.Caster;
                /*var queryParams 
                    = new EntityQueryTool.FilterQueryParams
                    (
                        default, GameEntityTool.GameEntityGroupRelateType.Enemy, 
                        EntityQueryTool.FilterQueryFlagType.ExceptMe | EntityQueryTool.FilterQueryFlagType.FreeAll, 
                        GameEntityTool.EntityStateType.DEAD
                    );
                var vfx = VfxPoolManager.GetInstanceUnsafe.Pop(_EffectCreateParams, new VfxPoolManager.ActivateParams(null, new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, new AffinePreset(caster.GetCenterPosition(), 1.5f)), GameEntityTool.ActivateParamsAttributeType.None, null, default));
                vfx.SetLookUV(caster.GetLookUV());
                
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
                                CameraManager.GetInstanceUnsafe.SetShake(Vector3.right, 7f, 0, 150, 3);
                                filtered.GiveDamage(caster[StatusTool.BattleStatusGroupType.Total].GetScaledAttackBase(DamageCalculator.DamageCalcType.Melee), new StatusTool.StatusChangeParams(StatusTool.StatusChangeEventType.Combat, caster));
                                // filtered.defaultnchant(13000002, new GameEntityEventCommonParams(caster));
                                break;
                            }
                        }
                    }
                }
            }
        }
        
        public class Melee_10 : GameEntityDeployEventBase
        {
            private const float __HitRangeWidth = 2f;
            private const float __HitRangeHeight = 7f;
                       
            private ProjectorPoolManager.CreateParams _ProjectorCreateParams;

            public Melee_10()
            {
                _ProjectorCreateParams = ProjectorPoolManager.GetInstanceUnsafe.GetCreateParams(4);
            }
            
            public override void PreloadDeployEvent()
            {
                base.PreloadDeployEvent();
                
                GetInstanceUnsafe.PreloadDeployEvent(34109);
            }

            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                var caster = param.Caster;
                param.TryGetAutoAimDirection(EntityQueryTool.FilterResultType.RemainNearestOne, out var o_UV);
                var duration = 0.3f;
                var spawnPos = caster.GetBottomPosition();
                
                caster.AddState(GameEntityTool.EntityStateType.BLOCK_MOVE);
                ProjectorPoolManager.GetInstanceUnsafe.SpawnProjector(_ProjectorCreateParams, caster, new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, spawnPos), o_UV, 1f, 34109, ProjectorTool.ActivateParamsAttributeType.GiveForwardPivotAttribute, __HitRangeWidth, __HitRangeHeight, 0.2f, duration, 0.05f);
            }
        }
        
        public class Melee_10_ex : GameEntityDeployEventBase
        {
            private const float __DamageRate = 0.5f;
         
            private VfxPoolManager.CreateParams _EffectCreateParams;

            public Melee_10_ex()
            {
                _EffectCreateParams = VfxPoolManager.GetInstanceUnsafe.GetCreateParams(24);
            }

            public override void PreloadDeployEvent()
            {
                base.PreloadDeployEvent();
                
            }

            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                var caster = param.Caster;
                if (caster.TryGetSpawner(out var o_Spawner))
                {
                    o_Spawner.PhysicsModule.AddVelocity(PhysicsTool.ForceType.Default, 100f * o_Spawner.GetLookUV());
                    o_Spawner.RemoveState(GameEntityTool.EntityStateType.BLOCK_MOVE);
                    
                    var scale = 4f * o_Spawner.Scale;
                    var activateParams = new VfxPoolManager.ActivateParams(null, new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, new AffinePreset(o_Spawner.GetCenterPosition(), scale)));
                    var vfx = VfxPoolManager.GetInstanceUnsafe.Pop(_EffectCreateParams, activateParams);
                    vfx.SetLookUV(caster.GetLookUV());
                
                    /*var queryParams 
                        = new EntityQueryTool.FilterQueryParams
                        (
                            default, GameEntityTool.GameEntityGroupRelateType.Enemy, 
                            EntityQueryTool.FilterQueryFlagType.ExceptMe | EntityQueryTool.FilterQueryFlagType.FreeAll, 
                            GameEntityTool.EntityStateType.DEAD
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
                                    filtered.GiveDamage(new StatusTool.StatusChangeParams(StatusTool.StatusChangeEventType.Combat, o_Spawner), __DamageRate);

                                    break;
                                }
                            }
                        }
                    }

                    await UniTask.Delay(350, cancellationToken: p_CancellationToken);
                    o_Spawner.SetDead(false);
                }
            }
        }
        
        public class Melee_11 : GameEntityDeployEventBase
        {
            private const float __HitRangeWidth = 2f;
            private const float __HitRangeHeight = 7f;
                     
            private ProjectorPoolManager.CreateParams _ProjectorCreateParams;

            public Melee_11()
            {
                _ProjectorCreateParams = ProjectorPoolManager.GetInstanceUnsafe.GetCreateParams(7);
            }
            
            public override void PreloadDeployEvent()
            {
                base.PreloadDeployEvent();
                
                GetInstanceUnsafe.PreloadDeployEvent(34119);
            }

            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                param.TryGetAutoAimDirection(EntityQueryTool.FilterResultType.RemainNearestOne, out var o_UV);
                var caster = param.Caster;
                var duration = 0.3f;
                var spawnPos = caster.GetBottomPosition();
                
                caster.AddState(GameEntityTool.EntityStateType.BLOCK_MOVE);
                ProjectorPoolManager.GetInstanceUnsafe.SpawnProjector(_ProjectorCreateParams, caster, new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, spawnPos), o_UV, 1f, 34119, ProjectorTool.ActivateParamsAttributeType.GiveForwardPivotAttribute, __HitRangeWidth, __HitRangeHeight, 0.2f, duration, 0.05f);
            }
        }
        
        public class Melee_11_ex : GameEntityDeployEventBase
        {
            private const float __DamageRate = 0.5f;
            
            private VfxPoolManager.CreateParams _EffectCreateParams;

            public Melee_11_ex()
            {
                _EffectCreateParams = VfxPoolManager.GetInstanceUnsafe.GetCreateParams(105);
            }

            public override void PreloadDeployEvent()
            {
                base.PreloadDeployEvent();
                
            }

            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                var caster = param.Caster;
                var masterValid = caster.TryGetMaster(out var o_Master);
                if (masterValid)
                {
                    o_Master.RemoveState(GameEntityTool.EntityStateType.BLOCK_MOVE);
                    
                    var scale = 4f * o_Master.Scale;
                    var activateParams = new VfxPoolManager.ActivateParams(null, new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, new AffinePreset(o_Master.GetCenterPosition(), scale)));
                    var vfx = VfxPoolManager.GetInstanceUnsafe.Pop(_EffectCreateParams, activateParams);
                    vfx.SetLookUV(caster.GetLookUV());
                
                    /*var queryParams 
                        = new EntityQueryTool.FilterQueryParams
                        (
                            default, GameEntityTool.GameEntityGroupRelateType.Enemy, 
                            EntityQueryTool.FilterQueryFlagType.ExceptMe | EntityQueryTool.FilterQueryFlagType.FreeAll, 
                            GameEntityTool.EntityStateType.DEAD
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
                                    filtered.GiveDamage(new StatusTool.StatusChangeParams(StatusTool.StatusChangeEventType.Combat, o_Master), __DamageRate);
                                    filtered.PhysicsModule.AddVelocity(PhysicsTool.ForceType.Default, 100f * o_Master.GetDirectionUnitVectorTo(filtered));

                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
                
        public class Melee_12 : GameEntityDeployEventBase
        {
            private const float __HitRangeWidth = 2f;
            private const float __HitRangeHeight = 7f;
            
            private ProjectorPoolManager.CreateParams _ProjectorCreateParams;

            public Melee_12()
            {
                _ProjectorCreateParams = ProjectorPoolManager.GetInstanceUnsafe.GetCreateParams(7);
            }
            
            public override void PreloadDeployEvent()
            {
                base.PreloadDeployEvent();
                
                GetInstanceUnsafe.PreloadDeployEvent(34129);
            }

            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                param.TryGetAutoAimDirection(EntityQueryTool.FilterResultType.RemainNearestOne, out var o_UV);
                var caster = param.Caster;
                var duration = 0.3f;
                var spawnPos = caster.GetBottomPosition();
                
                caster.AddState(GameEntityTool.EntityStateType.BLOCK_MOVE);
                ProjectorPoolManager.GetInstanceUnsafe.SpawnProjector(_ProjectorCreateParams, caster, new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, spawnPos), o_UV, 1f, 34129, ProjectorTool.ActivateParamsAttributeType.GiveForwardPivotAttribute | ProjectorTool.ActivateParamsAttributeType.GiveInverseAnimationAttribute, __HitRangeWidth, __HitRangeHeight, 0.2f, duration, 0.05f);
            }
        }
        
        public class Melee_12_ex : GameEntityDeployEventBase
        {
            private const float __DamageRate = 0.5f;
            
            private VfxPoolManager.CreateParams _EffectCreateParams;

            public Melee_12_ex()
            {
                _EffectCreateParams = VfxPoolManager.GetInstanceUnsafe.GetCreateParams(24);
            }

            public override void PreloadDeployEvent()
            {
                base.PreloadDeployEvent();
                
            }
            
            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                var caster = param.Caster;
                var masterValid = caster.TryGetMaster(out var o_Master);
                if (masterValid)
                {
                    o_Master.RemoveState(GameEntityTool.EntityStateType.BLOCK_MOVE);
                    
                    var scale = 4f * o_Master.Scale;
                    var activateParams = new VfxPoolManager.ActivateParams(null, new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, new AffinePreset(o_Master.GetCenterPosition(), scale)));
                    var vfx = VfxPoolManager.GetInstanceUnsafe.Pop(_EffectCreateParams, activateParams);
                    vfx.SetLookUV(caster.GetLookUV());
                
                    /*var queryParams 
                        = new EntityQueryTool.FilterQueryParams
                        (
                            default, GameEntityTool.GameEntityGroupRelateType.Enemy, 
                            EntityQueryTool.FilterQueryFlagType.ExceptMe | EntityQueryTool.FilterQueryFlagType.FreeAll, 
                            GameEntityTool.EntityStateType.DEAD
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
                                    filtered.GiveDamage(new StatusTool.StatusChangeParams(StatusTool.StatusChangeEventType.Combat, o_Master), __DamageRate);
                                    filtered.PhysicsModule.AddVelocity(PhysicsTool.ForceType.Default, 100f * filtered.GetDirectionUnitVectorTo(o_Master));

                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
        
        public class Melee_13 : GameEntityDeployEventBase
        {          
            private ProjectorPoolManager.CreateParams _ProjectorCreateParams;

            public Melee_13()
            {
                _ProjectorCreateParams = ProjectorPoolManager.GetInstanceUnsafe.GetCreateParams(1);
            }
            
            public override void PreloadDeployEvent()
            {
                base.PreloadDeployEvent();
                
                GetInstanceUnsafe.PreloadDeployEvent(34139);
            }
            
            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                param.TryGetAutoAimDirection(EntityQueryTool.FilterResultType.RemainNearestOne, out var o_UV);
                var caster = param.Caster;
                var duration = 0.3f;

                if (caster.TryGetCurrentEnemy(out var o_Enemy))
                {
                    var spawnPos = o_Enemy.GetBottomPosition();
                    var scale = caster.Scale;
                    ProjectorPoolManager.GetInstanceUnsafe.SpawnProjector(_ProjectorCreateParams, caster, new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, spawnPos), caster.GetLookUV(), 1f, 34139, ProjectorTool.ActivateParamsAttributeType.None, scale, scale, 0.2f, duration, 0.05f);
                }
            }
        }
        
        public class Melee_13_ex : GameEntityDeployEventBase
        {
            private const float __DamageRate = 0.5f;
            
            private VfxPoolManager.CreateParams _EffectCreateParams;

            public Melee_13_ex()
            {
                _EffectCreateParams = VfxPoolManager.GetInstanceUnsafe.GetCreateParams(177);
            }

            public override void PreloadDeployEvent()
            {
                base.PreloadDeployEvent();
                
            }

            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                var caster = param.Caster;
                var masterValid = caster.TryGetMaster(out var o_Master);
                if (masterValid)
                {
                    var scale = o_Master.Scale;
                    var activateParams = new VfxPoolManager.ActivateParams(null, new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, new AffinePreset(o_Master.GetCenterPosition(), scale)));
                    VfxPoolManager.GetInstanceUnsafe.Pop(_EffectCreateParams, activateParams);
                
                    /*var queryParams 
                        = new EntityQueryTool.FilterQueryParams
                        (
                            default, GameEntityTool.GameEntityGroupRelateType.Enemy, 
                            EntityQueryTool.FilterQueryFlagType.ExceptMe | EntityQueryTool.FilterQueryFlagType.FreeAll, 
                            GameEntityTool.EntityStateType.DEAD
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
                                    filtered.GiveDamage(new StatusTool.StatusChangeParams(StatusTool.StatusChangeEventType.Combat, o_Master), __DamageRate);

                                    var prevPos = filtered.GetTopPosition();
                                    if (filtered.TrySetTerrainSurfacePosition(o_Master.GetTopPosition()))
                                    {
                                        o_Master.TrySetTerrainSurfacePosition(prevPos);
                                    }
                                    activateParams = new VfxPoolManager.ActivateParams(null, new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, new AffinePreset(prevPos, scale)));
                                    VfxPoolManager.GetInstanceUnsafe.Pop(_EffectCreateParams, activateParams);

                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
        
        public class Melee_14_00 : GameEntityDeployEventBase
        {
            private ProjectorPoolManager.CreateParams _ProjectorCreateParams;
            private int _Count;
    
            public Melee_14_00(int p_Count)
            {
                _ProjectorCreateParams = ProjectorPoolManager.GetInstanceUnsafe.GetCreateParams(1);
                _Count = p_Count;
            }
            
            public override void PreloadDeployEvent()
            {
                base.PreloadDeployEvent();
                
                GetInstanceUnsafe.PreloadDeployEvent(34149);
            }
            
            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                param.TryGetAutoAimDirection(EntityQueryTool.FilterResultType.RemainNearestOne, out var o_UV);
                var caster = param.Caster;
                var duration = 0.3f;
                var spawnPos = caster.GetBottomPosition();

                caster.AddState(GameEntityTool.EntityStateType.BLOCK_MOVE);
                
                var offset = 2f * caster.GetLookUV();
                var scale = 2.5f * caster.Scale;
                for (var i = 0; i < _Count; i++)
                {
                    spawnPos += offset;
                    ProjectorPoolManager.GetInstanceUnsafe.SpawnProjector(_ProjectorCreateParams, caster, new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, spawnPos), caster.GetLookUV(), 1f, 34149, ProjectorTool.ActivateParamsAttributeType.None, scale, scale, 0.2f, duration, 0.05f);

                    await UniTask.Delay(100, cancellationToken: p_CancellationToken);
                }
                
                scale *= 1.5f;
                spawnPos += offset;
                ProjectorPoolManager.GetInstanceUnsafe.SpawnProjector(_ProjectorCreateParams, caster, new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, spawnPos), caster.GetLookUV(), 1f, 34149, ProjectorTool.ActivateParamsAttributeType.None, scale, scale, 0.2f, duration, 0.05f);
                
                await UniTask.Delay(200, cancellationToken: p_CancellationToken);
                caster.RemoveState(GameEntityTool.EntityStateType.BLOCK_MOVE);
            }
        }
        
        public class Melee_14_01 : GameEntityDeployEventBase
        {
            private ProjectorPoolManager.CreateParams _ProjectorCreateParams;
            private int _Count;
    
            public Melee_14_01(int p_Count)
            {
                _ProjectorCreateParams = ProjectorPoolManager.GetInstanceUnsafe.GetCreateParams(1);
                _Count = p_Count;
            }
            
            public override void PreloadDeployEvent()
            {
                base.PreloadDeployEvent();
                
                GetInstanceUnsafe.PreloadDeployEvent(34149);
            }

            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                param.TryGetAutoAimDirection(EntityQueryTool.FilterResultType.RemainNearestOne, out var o_UV);
                var caster = param.Caster;
                var duration = 0.3f;
                var spawnPos = caster.GetBottomPosition();
                var spawnPosL = caster.GetBottomPosition();
                var spawnPosR = caster.GetBottomPosition();

                caster.AddState(GameEntityTool.EntityStateType.BLOCK_MOVE);
                
                var scale = 2.5f * caster.Scale;
                ProjectorPoolManager.GetInstanceUnsafe.SpawnProjector(_ProjectorCreateParams, caster, new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, spawnPos), caster.GetLookUV(), 1f, 34149, ProjectorTool.ActivateParamsAttributeType.None, scale, scale, 0.2f, duration, 0.05f);
                await UniTask.Delay(100, cancellationToken: p_CancellationToken);
           
                var offset = 2f * caster.GetLookUV();
                var offsetL = offset.RotationVectorByPivot(Vector3.up, 30f);
                var offsetR = offset.RotationVectorByPivot(Vector3.up, -30f);

                for (var i = 0; i < _Count; i++)
                {
                    spawnPos += offset;
                    spawnPosL += offsetL;
                    spawnPosR += offsetR;
                    ProjectorPoolManager.GetInstanceUnsafe.SpawnProjector(_ProjectorCreateParams, caster, new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, spawnPos), offset, 1f, 34149, ProjectorTool.ActivateParamsAttributeType.None, scale, scale, 0.2f, duration, 0.05f);
                    ProjectorPoolManager.GetInstanceUnsafe.SpawnProjector(_ProjectorCreateParams, caster, new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, spawnPosL), offsetL, 1f, 34149, ProjectorTool.ActivateParamsAttributeType.None, scale, scale, 0.2f, duration, 0.05f);
                    ProjectorPoolManager.GetInstanceUnsafe.SpawnProjector(_ProjectorCreateParams, caster, new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, spawnPosR), offsetR, 1f, 34149, ProjectorTool.ActivateParamsAttributeType.None, scale, scale, 0.2f, duration, 0.05f);

                    await UniTask.Delay(100, cancellationToken: p_CancellationToken);
                }
                
                scale *= 1.5f;
                spawnPos += offset;
                spawnPosL += offsetL;
                spawnPosR += offsetR;
                ProjectorPoolManager.GetInstanceUnsafe.SpawnProjector(_ProjectorCreateParams, caster, new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, spawnPos), offset, 1f, 34149, ProjectorTool.ActivateParamsAttributeType.None, scale, scale, 0.2f, duration, 0.05f);
                ProjectorPoolManager.GetInstanceUnsafe.SpawnProjector(_ProjectorCreateParams, caster, new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, spawnPosL), offsetL, 1f, 34149, ProjectorTool.ActivateParamsAttributeType.None, scale, scale, 0.2f, duration, 0.05f);
                ProjectorPoolManager.GetInstanceUnsafe.SpawnProjector(_ProjectorCreateParams, caster, new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, spawnPosR), offsetR, 1f, 34149, ProjectorTool.ActivateParamsAttributeType.None, scale, scale, 0.2f, duration, 0.05f);

                await UniTask.Delay(200, cancellationToken: p_CancellationToken);
                caster.RemoveState(GameEntityTool.EntityStateType.BLOCK_MOVE);
            }
        }
        
        public class Melee_14_ex : GameEntityDeployEventBase
        {
            private const float __DamageRate = 0.5f;
            
            private VfxPoolManager.CreateParams _EffectCreateParams;

            public Melee_14_ex()
            {
                _EffectCreateParams = VfxPoolManager.GetInstanceUnsafe.GetCreateParams(178);
            }

            public override void PreloadDeployEvent()
            {
                base.PreloadDeployEvent();
                
            }

            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                var caster = param.Caster;
                var masterValid = caster.TryGetMaster(out var o_Master);
                if (masterValid)
                {
                    var scale = o_Master.Scale;
                    var activateParams = new VfxPoolManager.ActivateParams(null, new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, new AffinePreset(caster.GetCenterPosition(), scale)));
                    VfxPoolManager.GetInstanceUnsafe.Pop(_EffectCreateParams, activateParams);
                
                    /*var queryParams 
                        = new EntityQueryTool.FilterQueryParams
                        (
                            default, GameEntityTool.GameEntityGroupRelateType.Enemy, 
                            EntityQueryTool.FilterQueryFlagType.ExceptMe | EntityQueryTool.FilterQueryFlagType.FreeAll, 
                            GameEntityTool.EntityStateType.DEAD
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
                                    filtered.GiveDamage(new StatusTool.StatusChangeParams(StatusTool.StatusChangeEventType.Combat, o_Master), __DamageRate);
                                    filtered.PhysicsModule.AddVelocity(PhysicsTool.ForceType.Default, 40f * o_Master.GetDirectionUnitVectorTo(filtered));
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
        
        public class Melee_15_00 : GameEntityDeployEventBase
        {
            private const float __HitRangeWidth = 2f;
            private const float __HitRangeHeight = 7f;
                        
            private ProjectorPoolManager.CreateParams _ProjectorCreateParams;

            public Melee_15_00()
            {
                _ProjectorCreateParams = ProjectorPoolManager.GetInstanceUnsafe.GetCreateParams(4);
            }
            
            public override void PreloadDeployEvent()
            {
                base.PreloadDeployEvent();
                
                GetInstanceUnsafe.PreloadDeployEvent(34159);
            }

            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                param.TryGetAutoAimDirection(EntityQueryTool.FilterResultType.RemainNearestOne, out var o_UV);
                var caster = param.Caster;
                var duration = 0.3f;
                var spawnPos = caster.GetBottomPosition();

                caster.AddState(GameEntityTool.EntityStateType.BLOCK_MOVE);
                ProjectorPoolManager.GetInstanceUnsafe.SpawnProjector(_ProjectorCreateParams, caster, new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, spawnPos), o_UV, 1f, 34159, ProjectorTool.ActivateParamsAttributeType.GiveForwardPivotAttribute, __HitRangeWidth, __HitRangeHeight, 0.2f, duration, 0.05f);
            }
        }
        
        public class Melee_15_01 : GameEntityDeployEventBase
        {
            private const float __HitRangeWidth = 2f;
            private const float __HitRangeHeight = 7f;
                                    
            private ProjectorPoolManager.CreateParams _ProjectorCreateParams;

            public Melee_15_01()
            {
                _ProjectorCreateParams = ProjectorPoolManager.GetInstanceUnsafe.GetCreateParams(4);
            }
            
            public override void PreloadDeployEvent()
            {
                base.PreloadDeployEvent();
                
                GetInstanceUnsafe.PreloadDeployEvent(34159);
            }
            
            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                var caster = param.Caster;
                var shotDirection = caster.GetLookUV().RotationVectorByPivot(Vector3.up, Random.Range(0f, 360f));
                var duration = 0.3f;
                var spawnPos = caster.GetBottomPosition();

                caster.PhysicsModule.ClearVelocity(PhysicsTool.ForceType.Default);
                caster.SetLookUV(shotDirection);
                caster.AddState(GameEntityTool.EntityStateType.BLOCK_MOVE);
                ProjectorPoolManager.GetInstanceUnsafe.SpawnProjector(_ProjectorCreateParams, caster, new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, spawnPos), shotDirection, 1f, 34159, ProjectorTool.ActivateParamsAttributeType.GiveForwardPivotAttribute, __HitRangeWidth, __HitRangeHeight, 0.2f, duration, 0.05f);
            }
        }
        
        public class Melee_15_ex : GameEntityDeployEventBase
        {
            private const float __DamageRate = 0.5f;
            
            private VfxPoolManager.CreateParams _EffectCreateParams;

            public Melee_15_ex()
            {
                _EffectCreateParams = VfxPoolManager.GetInstanceUnsafe.GetCreateParams(24);
            }

            public override void PreloadDeployEvent()
            {
                base.PreloadDeployEvent();
                
            }

            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                var caster = param.Caster;
                var masterValid = caster.TryGetMaster(out var o_Master);
                if (masterValid)
                {
                    o_Master.PhysicsModule.AddVelocity(PhysicsTool.ForceType.Default, 100f * caster.GetLookUV());
                    o_Master.RemoveState(GameEntityTool.EntityStateType.BLOCK_MOVE);
                    
                    var scale = 4f * o_Master.Scale;
                    var activateParams = new VfxPoolManager.ActivateParams(null, new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, new AffinePreset(o_Master.GetCenterPosition(), scale)));
                    var vfx = VfxPoolManager.GetInstanceUnsafe.Pop(_EffectCreateParams, activateParams);
                    vfx.SetLookUV(caster.GetLookUV());
                
                    /*var queryParams 
                        = new EntityQueryTool.FilterQueryParams
                        (
                            default, GameEntityTool.GameEntityGroupRelateType.Enemy, 
                            EntityQueryTool.FilterQueryFlagType.ExceptMe | EntityQueryTool.FilterQueryFlagType.FreeAll, 
                            GameEntityTool.EntityStateType.DEAD
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
                                    filtered.GiveDamage(new StatusTool.StatusChangeParams(StatusTool.StatusChangeEventType.Combat, o_Master), __DamageRate);

                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
        
        #endregion
    }
}