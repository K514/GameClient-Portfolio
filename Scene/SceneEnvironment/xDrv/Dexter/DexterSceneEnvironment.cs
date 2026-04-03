using System.Threading;
using Cysharp.Threading.Tasks;
using k514.Mono.Feature;
using UnityEngine;

namespace k514.Mono.Common
{
    public class DexterSceneEnvironment : SceneEnvironment
    {
        private int _ComSpawnCount = 0;
        private UnitPoolManager.CreateParams _ICG_CreateParams;
        
        protected override void OnCreateAttribute()
        {
            CameraVariableIndex = 2;
        }
        
        public override async UniTask OnScenePreload(CancellationToken p_CancellationToken)
        {
            await base.OnScenePreload(p_CancellationToken);
            
            UIxControlRoot.GetInstanceUnsafe.MainHUD.SetHide(false);

            _ICG_CreateParams = UnitPoolManager.GetInstanceUnsafe.GetCreateParams(1, ResourceLifeCycleType.ManualUnload);

            var startPosPreset = SceneEnvironmentManager.GetInstanceUnsafe.CurrentSceneVariableDataRecord.SceneStartPreset;
            var playerICG = SpawnICG(startPosPreset.StartPosition);
            PlayerManager.GetInstanceUnsafe.SetPlayer(playerICG, PlayerTool.PlayerSetType.Overlap);
            playerICG.SetGroupMask(GameEntityTool.GameEntityGroupType.PlayerForce0, GameEntityTool.GameEntityGroupType.EnemyComputerForce);

            for (int i = 0; i < _ComSpawnCount; i++)
            {
                var spawnPos = CustomMath.GetRandomVector(XYZType.ZX, 4f, 10f) + playerICG.GetBottomPosition();
                var computerICG = SpawnICG(spawnPos);
                computerICG.SetGroupMask(GameEntityTool.GameEntityGroupType.EnemyComputerForce0, GameEntityTool.GameEntityGroupType.PlayerForce);
                computerICG.SwitchPersona(AutonomyModuleDataTableQuery.TableLabel.Coward);
            }
        }

        private UnitBase SpawnICG(Vector3 p_Pos)
        {
            return UnitPoolManager.GetInstanceUnsafe.Pop
            (
                _ICG_CreateParams,
                new UnitPoolManager.ActivateParams
                (
                    null,
                    new AffineCorrectionPreset
                    (
                        AffineTool.CorrectPositionType.ForceSurface, 
                        p_Pos + 10f * Vector3.up,
                        GameConst.EmptySurface_LayerMask
                    )
                )
            );
        }
    }
}