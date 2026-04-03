using UnityEngine;

namespace k514.Mono.Common
{
    public partial class InitScene : SceneController<InitScene, InitScene.InitScenePhase, InitSceneSequence, InitSceneTaskHandler, DefaultAsyncTaskResult>
    {
        #region <Enums>

        public enum InitScenePhase
        {
            /// <summary>
            /// 동작을 하지 않는 상태
            /// </summary>
            None,
            
            /// <summary>
            /// 부팅 시작
            /// </summary>
            InitSceneStart,
            
            /// <summary>
            /// 부팅에 필요한 시스템 초기화 작업
            /// </summary>
            InitSceneProcess,
            InitSceneProcess2,
            
            /// <summary>
            /// 부팅 종료 및 해당 씬 파기
            /// </summary>
            InitSceneTerminate,
        }

        #endregion
    }
}