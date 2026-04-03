using UnityEngine;

namespace k514.Mono.Common
{
    public partial class PatchScene : SceneController<PatchScene, PatchScene.PatchScenePhase, PatchSceneSequence, PatchSceneTaskHandler, DefaultAsyncTaskResult>
    {
        #region <Enums>

        public enum PatchScenePhase
        {
            None,
            
            GetPatchList,
            PatchFile,

            PatchTerminate,
        }

        #endregion
    }
}