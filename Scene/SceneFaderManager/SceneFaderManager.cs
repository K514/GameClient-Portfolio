using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace k514.Mono.Common
{
    public class SceneFaderManager : SceneChangeEventReceiveAsyncSingleton<SceneFaderManager>
    {
        #region <Fields>

        /// <summary>
        /// 현재 씬 페이더를 가진 컨테이너
        /// </summary>
        public ISceneFaderContainer SceneFaderContainer;

        #endregion

        #region <Callbacks>

        protected override void TryInitializeDependency()
        {
            base.TryInitializeDependency();

            Priority = 200;
        }
  
        protected override async UniTask OnInitiate(CancellationToken p_CancellationToken)
        {
            await UniTask.CompletedTask;
        }

        public override async UniTask OnScenePreload(CancellationToken p_CancellationToken)
        {
            SetBlackOut();

            await UniTask.CompletedTask;
        }

        public override async UniTask OnSceneStart(CancellationToken p_CancellationToken)
        {
            SetFadeIn();
            
            await UniTask.CompletedTask;
        }

        public override async UniTask OnSceneTerminate(CancellationToken p_CancellationToken)
        {
            SetFadeOut();
            
            await UniTask.CompletedTask;
        }

        public override async UniTask OnSceneTransition(CancellationToken p_CancellationToken)
        {
            ResetSceneFaderContainer();
            
            await UniTask.CompletedTask;
        }
        
        public void OnLateUpdate(float p_DeltaTime)
        {
            SceneFaderContainer?.GetSceneFader()?.OnLateUpdate(p_DeltaTime);
        }

        #endregion
        
        #region <Methods>

        /// <summary>
        /// 씬 페이더 컨테이너를 지정하는 메서드
        /// </summary>
        public void SetSceneFaderContainer(ISceneFaderContainer p_SceneFaderContainer, bool p_OverlapFlag)
        {
            if (ReferenceEquals(null, SceneFaderContainer) || p_OverlapFlag)
            {
                SceneFaderContainer = p_SceneFaderContainer;
            }
        }

        /// <summary>
        /// 씬 페이더 컨테이너를 지우는 메서드
        /// </summary>
        public void ResetSceneFaderContainer()
        {
            SetSceneFaderContainer(null, true);
        }
        
        /// <summary>
        /// 화면을 가리는 메서드
        /// </summary>
        public void SetBlackOut()
        {
            SceneFaderContainer?.GetSceneFader()?.SetInstantFadeOut();
        }
        
        /// <summary>
        /// 화면을 보아는 메서드
        /// </summary>
        public void CancelBlackOut()
        {
            SceneFaderContainer?.GetSceneFader()?.SetInstantFadeIn();
        }

        /// <summary>
        /// 페이드아웃 연출을 실행하는 메서드
        /// </summary>
        public void SetFadeOut(Action p_OnAnimationOver = null)
        {
            SceneFaderContainer?.GetSceneFader()?.SetFadeOut(UIxTool.UIAfterFadeType.None, false, p_OnAnimationOver);
        }

        /// <summary>
        /// 페이드인 연출을 실행하는 메서드
        /// </summary>
        public void SetFadeIn(Action p_OnAnimationOver = null)
        {
            SceneFaderContainer?.GetSceneFader()?.SetFadeIn(UIxTool.UIAfterFadeType.Hide, false, p_OnAnimationOver);
        }

        #endregion
    }
}