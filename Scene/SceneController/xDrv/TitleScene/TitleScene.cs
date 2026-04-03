namespace k514.Mono.Common
{
    public partial class TitleScene : SceneController<TitleScene, TitleScene.TitleScenePhase, TitleSceneSequence, TitleSceneTaskHandler, DefaultAsyncTaskResult>
    {
        #region <Enums>

        public enum TitleScenePhase
        {
            /// <summary>
            /// 동작을 하지 않는 상태
            /// </summary>
            None,
            
            /// <summary>
            /// 테이틀 씬 오프닝
            /// </summary>
            TitleOpen,
            
            /// <summary>
            /// Start Button 클릭
            /// </summary>
            StartButton,
                        
            /// <summary>
            /// Option Button 클릭
            /// </summary>
            OptionButton,
                        
            /// <summary>
            /// Replay Button 클릭
            /// </summary>
            ReplayButton,
                        
            /// <summary>
            /// Exit Button 클릭
            /// </summary>
            ExitButton,
        }

        #endregion
    }
}