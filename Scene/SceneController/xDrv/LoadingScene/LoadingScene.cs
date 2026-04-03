namespace k514.Mono.Common
{
    public partial class LoadingScene : SceneController<LoadingScene, LoadingScene.LoadingScenePhase, LoadingSceneSequence, LoadingSceneTaskHandler, DefaultAsyncTaskResult>
    {
        #region <Enums>

        public enum LoadingScenePhase
        {
            /// <summary>
            /// 동작을 하지 않는 상태
            /// </summary>
            None,

            /// <summary>
            /// 리소스 언로딩 중
            /// </summary>
            UnloadingResource,

            /// <summary>
            /// 리소스 로딩 중
            /// </summary>
            LoadingResource,

            /// <summary>
            /// 비동기 씬 로딩 중
            /// </summary>
            LoadingScene,
                                  
            /// <summary>
            /// 로딩 완료 시점, 로딩 씬과 로드된 씬이 동시에 존재함.
            /// </summary>
            LoadingSceneStageOn,

            /// <summary>
            /// MergeScene 메서드 참조, 로딩 씬과 로드된 씬이 병합됨
            /// </summary>
            MergeScene,
            
            /// <summary>
            /// 씬 병합 이후, 로딩 씬에 사용되었던 해당 오브젝트 및 관련 리소스를 제거하고
            /// 씬 종료 애니메이션을 수행하는 페이즈
            /// </summary>
            AsyncLoadTerminate,
        }

        #endregion
    }
}