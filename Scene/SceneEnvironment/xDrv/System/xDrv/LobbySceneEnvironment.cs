using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using k514.Mono.Common;
using UnityEngine;
using xk514;

namespace k514.Mono.Feature
{
    public class LobbySceneEnvironment : SystemSceneEnvironmentBase, ISceneFaderContainer
    {
        #region <Fields>

        public UIxFadePanel Fader;
        private bool _IsFaderValid; 
        public UIxButton PlayButton;

        #endregion
        
        #region <Callbacks>

        public override async UniTask OnScenePreload(CancellationToken p_CancellationToken)
        {
            await base.OnScenePreload(p_CancellationToken);
            
            GameManager.GetInstanceUnsafe.SetGameControl(GameManagerTool.GameControl.Lobby);
            UnitPoolManager.GetInstanceUnsafe.ClearPool();            
            ProjectilePoolManager.GetInstanceUnsafe.ClearPool();

            _IsFaderValid = !ReferenceEquals(null, Fader);
            if (_IsFaderValid)
            {
                Fader.CheckAwake();
                Fader.SetFadeDuration(GameConst.DefaultSceneTransitionDelay, 0f, GameConst.DefaultSceneTransitionDelay);
                Fader.SetInstantFadeOut();

                SceneFaderManager.GetInstanceUnsafe.SetSceneFaderContainer(this, true);
            }
            
            PlayButton.CheckAwake();
            PlayButton.SetButtonClickEvent(PlayGame);
        }

        #endregion
        
        #region <Methods>
        
        public UIxFadePanel GetSceneFader()
        {
            return Fader;
        }
        
        private async UniTask PlayGame(UIxButton p_Button, CancellationToken p_Token)
        {
            if (!await SceneChangeManager.GetInstanceUnsafe.TurnSceneTo(10000, new SceneTool.SceneControlPreset(SceneTool.LoadingSceneType.Black)))
            {
#if APPLY_PRINT_LOG
                CustomDebug.LogError($"[LobbyScene] Failed turn to Play Game Scene");
#endif
            }
        }        

        #endregion
    }
}