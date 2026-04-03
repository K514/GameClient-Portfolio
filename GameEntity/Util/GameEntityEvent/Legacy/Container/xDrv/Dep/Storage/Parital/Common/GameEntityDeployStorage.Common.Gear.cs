using System.Threading;
using Cysharp.Threading.Tasks;
using k514.Mono.Feature;
using UnityEngine;

namespace k514.Mono.Common
{
    public partial class GameEntityDeployStorage 
    {
        #region <Callbacks>

        private void OnCreateGear()
        {
            _EntityEventTable.Add(35000, new Gear_00_00());
            _EntityEventTable.Add(35001, new Gear_00_01(3, 25f));
            _EntityEventTable.Add(35008, new Gear_00_ex());
            _EntityEventTable.Add(35009, new Gear_00_ex2());
            _EntityEventTable.Add(35010, new Gear_01_00());
            _EntityEventTable.Add(35011, new Gear_01_01(3, 45f));
            _EntityEventTable.Add(35018, new Gear_01_ex());
            _EntityEventTable.Add(35019, new Gear_01_ex2());
            _EntityEventTable.Add(35050, new Gear_05());
        }

        #endregion
        
        #region <Classess>
        
        /// <summary>
        /// 아이스 브레스 장판
        /// </summary>
        public class Gear_00_00 : GameEntityDeployEventBase
        {
            private ProjectorPoolManager.CreateParams _ProjectorCreateParams;

            public Gear_00_00()
            {
                _ProjectorCreateParams = ProjectorPoolManager.GetInstanceUnsafe.GetCreateParams(4);
            }

            public override void PreloadDeployEvent()
            {
                base.PreloadDeployEvent();
                
                GetInstanceUnsafe.PreloadDeployEvent(35008);
            }
            
            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                param.TryGetAutoAimDirection(EntityQueryTool.FilterResultType.RemainNearestOne, out var o_UV);
                var caster = param.Caster;
                var projectDuration = 0.8f;
                
                caster.AddState(GameEntityTool.EntityStateType.BLOCK_MOVE);
                ProjectorPoolManager.GetInstanceUnsafe.SpawnProjector(_ProjectorCreateParams, caster, new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, caster.GetCenterPosition()), caster.GetLookUV(), 1f, 35008, ProjectorTool.ActivateParamsAttributeType.GiveForwardPivotAttribute, 2.2f, 15f, 0.2f, projectDuration, 0.05f);
            }
        }
        
        /// <summary>
        /// 아이스 브레스 장판
        /// </summary>
        public class Gear_00_01 : GameEntityDeployEventBase
        {
            private ProjectorPoolManager.CreateParams _ProjectorCreateParams;
            private int _ShotCount;
            private float _Theta;

            public Gear_00_01(int p_ShotCount, float p_Theta)
            {
                _ProjectorCreateParams = ProjectorPoolManager.GetInstanceUnsafe.GetCreateParams(4);
                _ShotCount = p_ShotCount;
                _Theta = p_Theta;
            }
            
            public override void PreloadDeployEvent()
            {
                base.PreloadDeployEvent();
                
                GetInstanceUnsafe.PreloadDeployEvent(35008);
            }

            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                param.TryGetAutoAimDirection(EntityQueryTool.FilterResultType.RemainNearestOne, out var o_UV);
                var caster = param.Caster;
                var projectDuration = 0.8f;
                
                var theta = _Theta / (_ShotCount - 1);

                o_UV = o_UV.RotationVectorByPivot(Vector3.up, 0.5f * _Theta);
                for (var i = 0; i < _ShotCount; i++)
                {
                    caster.AddState(GameEntityTool.EntityStateType.BLOCK_MOVE);
                    ProjectorPoolManager.GetInstanceUnsafe.
                        SpawnProjector
                        (
                            _ProjectorCreateParams, caster, 
                            new AffineCorrectionPreset
                            (
                                AffineTool.CorrectPositionType.None, caster.GetRelativePosition(0.33f * caster.Scale)
                            ), 
                            o_UV, 1f, 35008, 
                            ProjectorTool.ActivateParamsAttributeType.GiveForwardPivotAttribute, 
                            2.2f, 15f, 0.2f, projectDuration, 0.05f
                        );
                    o_UV = o_UV.RotationVectorByPivot(Vector3.up, -theta);
                }
            }
        }
        
        /// <summary>
        /// 아이스 브레스 히트 필드 기어 생성
        /// </summary>
        public class Gear_00_ex : GameEntityDeployEventBase
        {
            private GearPoolManager.CreateParams _GearCreateParams;
            private VfxPoolManager.CreateParams _EffectCreateParams;

            public Gear_00_ex()
            {
                _GearCreateParams = GearPoolManager.GetInstanceUnsafe.GetCreateParams(13);
                _EffectCreateParams = VfxPoolManager.GetInstanceUnsafe.GetCreateParams(44);
            }

            public override void PreloadDeployEvent()
            {
                base.PreloadDeployEvent();
                
                GetInstanceUnsafe.PreloadDeployEvent(35009);
            }
            
            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                var caster = param.Caster;
                var filterSpace = caster.GetSightRangeFilterConfig();

                var vfx = VfxPoolManager.GetInstanceUnsafe
                    .Pop
                    (
                        _EffectCreateParams, 
                        new VfxPoolManager.ActivateParams
                        (
                            null, 
                            new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, new AffinePreset(caster.GetCenterPosition(), caster.GetRotation(), 3.5f)) 
                        )
                    );
                
                GearPoolManager.GetInstanceUnsafe.SpawnGear
                (
                    _GearCreateParams,
                    new GearPoolManager.ActivateParams
                    (
                        null,
                        new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, new AffinePreset(caster.GetBottomPosition(), caster.GetRotation())),
                        GameEntityTool.ActivateParamsAttributeType.GiveFollowFallenMaster,
                        caster, default, p_Duration: vfx.GetLiveSpan(), p_FilterSpace: filterSpace
                    )
                );
            }
        }
        
        /// <summary>
        /// 아이스 브레스 종료
        /// </summary>
        public class Gear_00_ex2 : GameEntityDeployEventBase
        {
            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                var caster = param.Caster;
                
                if (caster.TryGetMaster(out var o_Master))
                {
                    o_Master.RemoveState(GameEntityTool.EntityStateType.BLOCK_MOVE);
                }
            }
        }

        /// <summary>
        /// 파이어 브레스 장판
        /// </summary>
        public class Gear_01_00 : GameEntityDeployEventBase
        {
            private ProjectorPoolManager.CreateParams _ProjectorCreateParams;

            public Gear_01_00()
            {
                _ProjectorCreateParams = ProjectorPoolManager.GetInstanceUnsafe.GetCreateParams(4);
            }

            public override void PreloadDeployEvent()
            {
                base.PreloadDeployEvent();
                
                GetInstanceUnsafe.PreloadDeployEvent(35018);
            }
            
            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                param.TryGetAutoAimDirection(EntityQueryTool.FilterResultType.RemainNearestOne, out var o_UV);
                var caster = param.Caster;
                var projectDuration = 0.8f;
                
                caster.AddState(GameEntityTool.EntityStateType.BLOCK_MOVE);
                ProjectorPoolManager.GetInstanceUnsafe.SpawnProjector(_ProjectorCreateParams, caster, new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, caster.GetCenterPosition()), caster.GetLookUV(), 1f, 35018, ProjectorTool.ActivateParamsAttributeType.GiveForwardPivotAttribute, 2.2f, 15f, 0.2f, projectDuration, 0.05f);
            }
        }
        
        /// <summary>
        /// 파이어 브레스 장판
        /// </summary>
        public class Gear_01_01 : GameEntityDeployEventBase
        {
            private ProjectorPoolManager.CreateParams _ProjectorCreateParams;
            private int _ShotCount;
            private float _Theta;
            
            public Gear_01_01(int p_ShotCount, float p_Theta)
            {
                _ProjectorCreateParams = ProjectorPoolManager.GetInstanceUnsafe.GetCreateParams(4);
                _ShotCount = p_ShotCount;
                _Theta = p_Theta;
            }
            
            public override void PreloadDeployEvent()
            {
                base.PreloadDeployEvent();
                
                GetInstanceUnsafe.PreloadDeployEvent(35018);
            }
            
            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                param.TryGetAutoAimDirection(EntityQueryTool.FilterResultType.RemainNearestOne, out var o_UV);
                var caster = param.Caster;
                var projectDuration = 0.8f;
                
                var theta = _Theta / (_ShotCount - 1);

                o_UV = o_UV.RotationVectorByPivot(Vector3.up, 0.5f * _Theta);
                for (var i = 0; i < _ShotCount; i++)
                {
                    caster.AddState(GameEntityTool.EntityStateType.BLOCK_MOVE);
                    ProjectorPoolManager.GetInstanceUnsafe.
                        SpawnProjector
                        (
                            _ProjectorCreateParams, caster, 
                            new AffineCorrectionPreset
                            (
                                AffineTool.CorrectPositionType.None, caster.GetRelativePosition(0.33f * caster.Scale)
                            ), 
                            o_UV, 1f, 35018, 
                            ProjectorTool.ActivateParamsAttributeType.GiveForwardPivotAttribute, 
                            2.2f, 15f, 0.2f, projectDuration, 0.05f
                        );
                    o_UV = o_UV.RotationVectorByPivot(Vector3.up, -theta);
                }
            }
        }
        
        /// <summary>
        /// 파이어 브레스 히트 필드 기어 생성
        /// </summary>
        public class Gear_01_ex : GameEntityDeployEventBase
        {
            private GearPoolManager.CreateParams _GearCreateParams;
            private VfxPoolManager.CreateParams _EffectCreateParams;

            public Gear_01_ex()
            {
                _GearCreateParams = GearPoolManager.GetInstanceUnsafe.GetCreateParams(14);
                _EffectCreateParams = VfxPoolManager.GetInstanceUnsafe.GetCreateParams(43);
            }

            public override void PreloadDeployEvent()
            {
                base.PreloadDeployEvent();
                
                GetInstanceUnsafe.PreloadDeployEvent(35019);
            }
            
            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                var caster = param.Caster;
                var filterSpace = caster.GetSightRangeFilterConfig();

                var vfx = VfxPoolManager.GetInstanceUnsafe
                    .Pop
                    (
                        _EffectCreateParams, 
                        new VfxPoolManager.ActivateParams
                        (
                            null, 
                            new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, new AffinePreset(caster.GetCenterPosition(), caster.GetRotation(), 3.5f)) 
                        )
                    );
                
                GearPoolManager.GetInstanceUnsafe.SpawnGear
                (
                    _GearCreateParams,
                    new GearPoolManager.ActivateParams
                    (
                        null,
                        new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, new AffinePreset(caster.GetBottomPosition(), caster.GetRotation())),
                        GameEntityTool.ActivateParamsAttributeType.GiveFollowFallenMaster,
                        caster, default, p_Duration: vfx.GetLiveSpan(), p_FilterSpace: filterSpace
                    )
                );
            }
        }
        
        /// <summary>
        /// 파이어 브레스 종료
        /// </summary>
        public class Gear_01_ex2 : GameEntityDeployEventBase
        {
            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                var caster = param.Caster;
                
                if (caster.TryGetMaster(out var o_Master))
                {
                    o_Master.RemoveState(GameEntityTool.EntityStateType.BLOCK_MOVE);
                }
            }
        }
        
        /// <summary>
        /// 회전 기어 생성
        /// </summary>
        public class Gear_05 : GameEntityDeployEventBase
        {
            private GearPoolManager.CreateParams _GearCreateParams;

            public Gear_05()
            {
                _GearCreateParams = GearPoolManager.GetInstanceUnsafe.GetCreateParams(12);
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
                var projectileCreateParams = param.GetShotCreateParams(0);

                var gear = 
                    GearPoolManager.GetInstanceUnsafe.SpawnGear
                    (
                        _GearCreateParams,
                        new GearPoolManager.ActivateParams
                        (
                            null,
                            new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, new AffinePreset(caster.GetCenterPosition(), caster.GetRotation())),
                            GameEntityTool.ActivateParamsAttributeType.GiveFollowFallenMaster,
                            caster, default
                        )
                    );
                
                gear.SwitchPhysicsModule(PhysicsModuleDataTableQuery.TableLabel.Affine); 
                gear.SwitchActionModule(ActionModuleDataTableQuery.TableLabel.Default);
                gear.SwitchGeometryModule(GeometryModuleDataTableQuery.TableLabel.Affine);
                gear.AddStatus(StatusTool.BattleStatusGroupType.ExtraAdd, BattleStatusTool.BattleStatusType.MoveSpeedBasis, 8f);
                // gear.RunAffineEvent(2000, default);
            }
        }
        
        #endregion
    }
}