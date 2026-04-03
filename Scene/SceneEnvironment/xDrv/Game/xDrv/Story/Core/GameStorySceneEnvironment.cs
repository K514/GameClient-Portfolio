using System.Threading;
using Cysharp.Threading.Tasks;
using k514.Mono.Common;

namespace k514.Mono.Feature
{
    public abstract class GameStorySceneEnvironment : GameSceneEnvironmentBase
    {
        #region <Callbacks>

        public override async UniTask OnScenePreload(CancellationToken p_CancellationToken)
        {
            await base.OnScenePreload(p_CancellationToken);
            
            GameManager.GetInstanceUnsafe.SetGameControl(GameManagerTool.GameControl.Dungeon);
            GameManager.GetInstanceUnsafe.SetStageType(GameManagerTool.StageType.Normal);
        }
        
        public override async UniTask OnSceneStart(CancellationToken p_CancellationToken)
        {
            await base.OnSceneStart(p_CancellationToken);

            SetLocationPhase(LocationBase.LocationPhase.Ready);
        }

        #endregion
    }
}