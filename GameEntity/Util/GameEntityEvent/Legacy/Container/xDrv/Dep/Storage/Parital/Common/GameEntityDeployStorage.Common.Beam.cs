using System.Threading;
using Cysharp.Threading.Tasks;
using k514.Mono.Feature;
using UnityEngine;

namespace k514.Mono.Common
{
    public partial class GameEntityDeployStorage 
    {
        #region <Callbacks>

        private void OnCreateBeam()
        {
            _EntityEventTable.Add(32000, new Beam_00_0());
            _EntityEventTable.Add(32001, new Beam_00_1(3, 0f, 30f));
            _EntityEventTable.Add(32002, new Beam_00_1(4, 45f, 90f));
            _EntityEventTable.Add(32003, new Beam_00_1(4, 0f, 90f));
            _EntityEventTable.Add(32009, new Beam_00_ex());
            _EntityEventTable.Add(32010, new Beam_01_0());
            _EntityEventTable.Add(32011, new Beam_01_1(3, 0f, 30f));
            _EntityEventTable.Add(32012, new Beam_01_1(4, 45f, 90f));
            _EntityEventTable.Add(32013, new Beam_01_1(4, 0f, 90f));
            _EntityEventTable.Add(32019, new Beam_01_ex());
            _EntityEventTable.Add(32020, new Beam_02(3, 2f));
            _EntityEventTable.Add(32021, new Beam_02(8, 4f));
            _EntityEventTable.Add(32029, new Beam_02_ex());
            _EntityEventTable.Add(32030, new Beam_03(3, 2f));
            _EntityEventTable.Add(32031, new Beam_03(8, 4f));
            _EntityEventTable.Add(32039, new Beam_03_ex());
        }

        #endregion
        
        #region <Classess>
        
        /// <summary>
        /// 레이저 경로 표시
        /// </summary>
        public class Beam_00_0 : GameEntityDeployEventBase
        {
            private ProjectorPoolManager.CreateParams _ProjectorCreateParams;

            public Beam_00_0()
            {
                _ProjectorCreateParams = ProjectorPoolManager.GetInstanceUnsafe.GetCreateParams(4);
            }

            public override void PreloadDeployEvent()
            {
                base.PreloadDeployEvent();
                
                GetInstanceUnsafe.PreloadDeployEvent(32009);
            }
            
            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                param.TryGetAutoAimDirection(EntityQueryTool.FilterResultType.RemainNearestOne, out var o_UV);
                var caster = param.Caster;

                caster.AddState(GameEntityTool.EntityStateType.BLOCK_MOVE);
                
                ProjectorPoolManager.GetInstanceUnsafe
                    .SpawnProjector
                    (
                        _ProjectorCreateParams, caster, 
                        new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, caster.GetCenterPosition()),
                        o_UV, 1f, 32009, 
                        ProjectorTool.ActivateParamsAttributeType.GiveForwardPivotAttribute, 0.5f, 12f, 0.3f, 1.2f, 0.3f
                    );

                await UniTask.Delay(2800, cancellationToken: p_CancellationToken);
                caster.RemoveState(GameEntityTool.EntityStateType.BLOCK_MOVE);
            }
        }
        
        /// <summary>
        /// 전방위 레이저 경로 표시
        /// </summary>
        public class Beam_00_1 : GameEntityDeployEventBase
        {
            private ProjectorPoolManager.CreateParams _ProjectorCreateParams;
            private int _ShotCount;
            private float _StartTheta;
            private float _DeltaTheta;
            private float _WholeThetaHalf;
            
            public Beam_00_1(int p_ShotCount, float p_StartTheta, float p_DeltaTheta)
            {
                _ProjectorCreateParams = ProjectorPoolManager.GetInstanceUnsafe.GetCreateParams(4);
                _ShotCount = Mathf.Max(2, p_ShotCount);
                _StartTheta = p_StartTheta;
                _DeltaTheta = p_DeltaTheta;
                _WholeThetaHalf = 0.5f * _DeltaTheta * (_ShotCount - 1);
            }
  
            public override void PreloadDeployEvent()
            {
                base.PreloadDeployEvent();
                
                GetInstanceUnsafe.PreloadDeployEvent(32009);
            }
     
            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                param.TryGetAutoAimDirection(EntityQueryTool.FilterResultType.RemainNearestOne, out var o_UV);
                var caster = param.Caster;

                caster.AddState(GameEntityTool.EntityStateType.BLOCK_MOVE);

                o_UV = o_UV.RotationVectorByPivot(Vector3.up, _StartTheta + _WholeThetaHalf);
                for (int i = 0; i < _ShotCount; i++)
                {
                    ProjectorPoolManager.GetInstanceUnsafe
                        .SpawnProjector
                        (
                            _ProjectorCreateParams, caster, 
                            new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, caster.GetCenterPosition()),
                            o_UV, 1f, 32009, 
                            ProjectorTool.ActivateParamsAttributeType.GiveForwardPivotAttribute, 0.5f, 12f, 0.3f, 1.2f, 0.3f
                        );
                    
                    o_UV = o_UV.RotationVectorByPivot(Vector3.down, _DeltaTheta);
                }
       
                await UniTask.Delay(2800, cancellationToken: p_CancellationToken);
                caster.RemoveState(GameEntityTool.EntityStateType.BLOCK_MOVE);
            }
        }
        
        /// <summary>
        /// 직선 레이저 생성
        /// </summary>
        public class Beam_00_ex : GameEntityDeployEventBase
        {
            private BeamPoolManager.CreateParams _BeamCreateParams;

            public Beam_00_ex()
            {
                _BeamCreateParams = BeamPoolManager.GetInstanceUnsafe.GetCreateParams(1);
            }

            public override void PreloadDeployEvent()
            {
                base.PreloadDeployEvent();
                
            }
         
            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                var caster = param.Caster;
                var scale = caster.Scale;
                var spawnPos = caster.GetCenterRelativePosition(scale);
                var affineCorrect = new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, new AffinePreset(spawnPos, 3f));

                BeamPoolManager.GetInstanceUnsafe.SpawnBeam(_BeamCreateParams, caster, affineCorrect, caster.GetLookUV(), 3f, 12f - scale, 12, 2.4f, 0.2f);
            }
        }
        
        /// <summary>
        /// 레이저 경로 표시
        /// </summary>
        public class Beam_01_0 : GameEntityDeployEventBase
        {
            private ProjectorPoolManager.CreateParams _ProjectorCreateParams;

            public Beam_01_0()
            {
                _ProjectorCreateParams = ProjectorPoolManager.GetInstanceUnsafe.GetCreateParams(4);
            }

            public override void PreloadDeployEvent()
            {
                base.PreloadDeployEvent();
                
                GetInstanceUnsafe.PreloadDeployEvent(32019);
            }
            
            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                param.TryGetAutoAimDirection(EntityQueryTool.FilterResultType.RemainNearestOne, out var o_UV);
                var caster = param.Caster;

                caster.AddState(GameEntityTool.EntityStateType.BLOCK_MOVE);
                
                ProjectorPoolManager.GetInstanceUnsafe
                    .SpawnProjector
                    (
                        _ProjectorCreateParams, caster, 
                        new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, caster.GetCenterPosition()),
                        o_UV, 1f, 32019, 
                        ProjectorTool.ActivateParamsAttributeType.GiveForwardPivotAttribute, 0.5f, 12f, 0.3f, 1.2f, 0.3f
                    );

                await UniTask.Delay(2800, cancellationToken: p_CancellationToken);
                caster.RemoveState(GameEntityTool.EntityStateType.BLOCK_MOVE);
            }
        }
        
        /// <summary>
        /// 전방위 레이저 경로 표시
        /// </summary>
        public class Beam_01_1 : GameEntityDeployEventBase
        {
            private ProjectorPoolManager.CreateParams _ProjectorCreateParams;
            private int _ShotCount;
            private float _StartTheta;
            private float _DeltaTheta;
            private float _WholeThetaHalf;
            
            public Beam_01_1(int p_ShotCount, float p_StartTheta, float p_DeltaTheta)
            {
                _ProjectorCreateParams = ProjectorPoolManager.GetInstanceUnsafe.GetCreateParams(4);
                _ShotCount = Mathf.Max(2, p_ShotCount);
                _StartTheta = p_StartTheta;
                _DeltaTheta = p_DeltaTheta;
                _WholeThetaHalf = 0.5f * _DeltaTheta * (_ShotCount - 1);
            }
            
            public override void PreloadDeployEvent()
            {
                base.PreloadDeployEvent();
                
                GetInstanceUnsafe.PreloadDeployEvent(32019);
            }
            
            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                param.TryGetAutoAimDirection(EntityQueryTool.FilterResultType.RemainNearestOne, out var o_UV);
                var caster = param.Caster;

                caster.AddState(GameEntityTool.EntityStateType.BLOCK_MOVE);

                o_UV = o_UV.RotationVectorByPivot(Vector3.up, _StartTheta + _WholeThetaHalf);
                for (int i = 0; i < _ShotCount; i++)
                {
                    ProjectorPoolManager.GetInstanceUnsafe
                        .SpawnProjector
                        (
                            _ProjectorCreateParams, caster, 
                            new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, caster.GetCenterPosition()),
                            o_UV, 1f, 32019, 
                            ProjectorTool.ActivateParamsAttributeType.GiveForwardPivotAttribute, 0.5f, 12f, 0.3f, 1.2f, 0.3f
                        );
                    
                    o_UV = o_UV.RotationVectorByPivot(Vector3.down, _DeltaTheta);
                }
       
                await UniTask.Delay(2800, cancellationToken: p_CancellationToken);
                caster.RemoveState(GameEntityTool.EntityStateType.BLOCK_MOVE);
            }
        }
        
        /// <summary>
        /// 궤도 레이저 생성
        /// </summary>
        public class Beam_01_ex : GameEntityDeployEventBase
        {
            private BeamPoolManager.CreateParams _BeamCreateParams;

            public Beam_01_ex()
            {
                _BeamCreateParams = BeamPoolManager.GetInstanceUnsafe.GetCreateParams(5);
            }

            public override void PreloadDeployEvent()
            {
                base.PreloadDeployEvent();
                
            }
   
            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                var caster = param.Caster;
                var scale = caster.DoubleScale;
                var spawnPos = caster.GetTopUpPosition(scale);
                var affineCorrect = new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, new AffinePreset(spawnPos, 3f));

                var beam = BeamPoolManager.GetInstanceUnsafe.SpawnBeam(_BeamCreateParams, caster, affineCorrect, caster.GetLookUV(), 3f, 12f, 12, 2.4f, 0.2f);
                // beam.RunAffineEvent(1000, default);
            }
        }
        
        /// <summary>
        /// 레이저 폭격 위치 표시
        /// </summary>
        public class Beam_02 : GameEntityDeployEventBase
        {
            private ProjectorPoolManager.CreateParams _ProjectorCreateParams;
            private int _ShotCount;
            private float _ErrorRange;
            private float _DeltaTheta;
            private float _WholeThetaHalf;
            
            public Beam_02(int p_ShotCount, float p_ErrorRange)
            {
                _ProjectorCreateParams = ProjectorPoolManager.GetInstanceUnsafe.GetCreateParams(1);
                _ShotCount = Mathf.Max(2, p_ShotCount);
                _ErrorRange = p_ErrorRange;
            }
            
            public override void PreloadDeployEvent()
            {
                base.PreloadDeployEvent();
                
                GetInstanceUnsafe.PreloadDeployEvent(32029);
            }
            
            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                param.TryGetAutoAimDirection(EntityQueryTool.FilterResultType.RemainNearestOne, out var o_UV);
                var caster = param.Caster;

                for (int i = 0; i < _ShotCount; i++)
                {
                    var spawnPos = caster.TryGetCurrentEnemy(out var o_Enemy)
                        ? o_Enemy.GetBottomPosition()
                        : caster.GetBottomPosition();
                    spawnPos += CustomMath.GetRandomVector(XYZType.ZX, 0f, _ErrorRange);
                    
                    ProjectorPoolManager.GetInstanceUnsafe
                        .SpawnProjector
                        (
                            _ProjectorCreateParams, caster, 
                            new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, spawnPos),
                            o_UV, 1f, 32029, 
                            ProjectorTool.ActivateParamsAttributeType.GiveForwardPivotAttribute, 1f, 1f, 0.3f, 1.2f, 0.3f
                        );

                    await UniTask.Delay(200, cancellationToken: p_CancellationToken);
                }
            }
        }
        
        /// <summary>
        /// 폭격 레이저 생성
        /// </summary>
        public class Beam_02_ex : GameEntityDeployEventBase
        {
            private BeamPoolManager.CreateParams _BeamCreateParams;

            public Beam_02_ex()
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
                var pivotPos = caster.GetCenterPosition();
                var spawnPos = caster.GetTopUpPosition(Random.Range(10, 15f)).GetRandomPosition(XYZType.ZX, 0f, 5f);
                var affineCorrect = new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, new AffinePreset(spawnPos, 3f));
                var direction = spawnPos.GetDirectionUnitVectorTo(pivotPos);
                BeamPoolManager.GetInstanceUnsafe.SpawnBeam(_BeamCreateParams, caster, affineCorrect, direction, 3f, 100f, 12, 2.4f, 0.2f, 100);
            }
        }
        
        /// <summary>
        /// 레이저 폭격 위치 표시
        /// </summary>
        public class Beam_03 : GameEntityDeployEventBase
        {
            private ProjectorPoolManager.CreateParams _ProjectorCreateParams;
            private int _ShotCount;
            private float _ErrorRange;
            private float _DeltaTheta;
            private float _WholeThetaHalf;
            
            public Beam_03(int p_ShotCount, float p_ErrorRange)
            {
                _ProjectorCreateParams = ProjectorPoolManager.GetInstanceUnsafe.GetCreateParams(1);
                _ShotCount = Mathf.Max(2, p_ShotCount);
                _ErrorRange = p_ErrorRange;
            }
            
            public override void PreloadDeployEvent()
            {
                base.PreloadDeployEvent();
                   
                GetInstanceUnsafe.PreloadDeployEvent(32039);
            }
            
            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                param.TryGetAutoAimDirection(EntityQueryTool.FilterResultType.RemainNearestOne, out var o_UV);
                var caster = param.Caster;

                for (int i = 0; i < _ShotCount; i++)
                {
                    var spawnPos = caster.TryGetCurrentEnemy(out var o_Enemy)
                        ? o_Enemy.GetBottomPosition()
                        : caster.GetBottomPosition();
                    spawnPos += CustomMath.GetRandomVector(XYZType.ZX, 0f, _ErrorRange);
                    
                    ProjectorPoolManager.GetInstanceUnsafe
                        .SpawnProjector
                        (
                            _ProjectorCreateParams, caster, 
                            new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, spawnPos),
                            o_UV, 1f, 32039, 
                            ProjectorTool.ActivateParamsAttributeType.GiveForwardPivotAttribute, 1f, 1f, 0.3f, 1.2f, 0.3f
                        );

                    await UniTask.Delay(200, cancellationToken: p_CancellationToken);
                }
            }
        }
        
        /// <summary>
        /// 폭격 레이저 생성
        /// </summary>
        public class Beam_03_ex : GameEntityDeployEventBase
        {
            private BeamPoolManager.CreateParams _BeamCreateParams;

            public Beam_03_ex()
            {
                _BeamCreateParams = BeamPoolManager.GetInstanceUnsafe.GetCreateParams(9);
            }

            public override void PreloadDeployEvent()
            {
                base.PreloadDeployEvent();
                
            }
            
            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                var caster = param.Caster;
                var pivotPos = caster.GetCenterPosition();
                var spawnPos = caster.GetTopUpPosition(Random.Range(10, 15f)).GetRandomPosition(XYZType.ZX, 0f, 5f);
                var affineCorrect = new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, new AffinePreset(spawnPos, 3f));
                var direction = spawnPos.GetDirectionUnitVectorTo(pivotPos);
                BeamPoolManager.GetInstanceUnsafe.SpawnBeam(_BeamCreateParams, caster, affineCorrect, direction, 3f, 100f, 12, 2.4f, 0.2f, 100);
            }
        }
      
        #endregion
    }
}