using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace k514.Mono.Common
{
    public partial class SceneChangeManager : AsyncSingleton<SceneChangeManager>
    {
        #region <Fields>

        /// <summary>
        /// 현재 페이즈
        /// </summary>
        private SceneControlPhase _CurrentPhase;

        /// <summary>
        /// 로딩 씬을 기준으로 마지막으로 로드했던 씬의 정보
        /// </summary>
        public SceneTool.SceneChangePreset PrevSceneControlPreset { get; private set; }

        /// <summary>
        /// 로딩 씬을 기준으로 현재 로드해야할 씬의 정보
        /// </summary>
        public SceneTool.SceneChangePreset CurrentSceneControlPreset { get; private set; }
  
        #endregion

        #region <Enums>

        private enum SceneControlPhase
        {
            None,
            Reserved,
            TransitionScene,
        }

        #endregion
        
        #region <Callbacks>

        protected override void TryInitializeDependency()
        {
            base.TryInitializeDependency();
            
            _Dependencies.Add(typeof(SceneEntryDataTable));
        }

        protected override async UniTask OnCreated(CancellationToken p_CancellationToken)
        {
            await UniTask.CompletedTask;
        }

        protected override async UniTask OnInitiate(CancellationToken p_CancellationToken)
        {
            await UniTask.CompletedTask;
        }

        #endregion

        #region <Methods>

        public bool IsManagingScene()
        {
            return CurrentSceneControlPreset.ValidFlag;
        }
        
        public bool IsFreeScene()
        {
            return !CurrentSceneControlPreset.ValidFlag;
        }

        public void SetSceneChangePreset(SceneTool.SceneChangePreset p_SceneChangePreset)
        {
            PrevSceneControlPreset = CurrentSceneControlPreset;
            CurrentSceneControlPreset = p_SceneChangePreset;
        }
        
        private void ReserveSceneChangePreset(SceneTool.SceneChangePreset p_SceneChangePreset)
        {
            _CurrentPhase = SceneControlPhase.Reserved;
            SetSceneChangePreset(p_SceneChangePreset);
        }

        #endregion
    }
}