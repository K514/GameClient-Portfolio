#if !SERVER_DRIVE

namespace k514.Mono.Common
{
    public partial class UIxControlRoot
    {
        public void GoToLobby()
        {
            SceneChangeManager.GetInstanceUnsafe.TurnSceneTo(SceneTool.SceneShortCutType.LobbyScene);
        }
    }
}

#endif