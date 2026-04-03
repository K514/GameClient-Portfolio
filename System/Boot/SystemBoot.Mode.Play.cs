using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using k514.Mono.Common;
using UnityEngine;
using UnityEngine.SceneManagement;
using xk514;

namespace k514
{
    public partial class SystemBoot
    {
        #region <Callbacks>
        
        public static void OnPatchSuccess()
        {
            TryGameInit();
        }

        public static void OnGameInitSuccess()
        {
            var trySceneEntryPreset = SystemEntryPreset;
#if APPLY_PRINT_LOG
            CustomDebug.LogError($"System begins with {trySceneEntryPreset}!");
#endif
            switch (trySceneEntryPreset.EntryMode)
            {
                case SystemEntryMode.SingleMode:
                case SystemEntryMode.MultiMode:
                    TryTitleScene();
                    break;
                case SystemEntryMode.SelectMode:
                    TryEntryScene();
                    break;
                // unreachable
                case SystemEntryMode.AttachMode:
                case SystemEntryMode.DebugMode:
#if UNITY_EDITOR
                    CustomDebug.LogError("unreachable");
#endif
                    break;
            }
        }

        public static void OnGameStart()
        {
            SceneChangeManager.GetInstanceUnsafe.TurnSceneTo(SceneTool.SceneShortCutType.LobbyScene);
        }
        
        #endregion

        #region <Methods>

        public static void EnterPlayMode()
        {
#if UNITY_EDITOR
    #if SERVER_DRIVE
            TryGameInit();
    #else
            if (IsDebugMode())
            {
                TryGameInit();
            }
            else
            {
                TryPatchSystem();
            }
    #endif
#else
    #if SERVER_DRIVE
            TryGameInit();
    #else
            TryPatchSystem();
    #endif
#endif
        }
        
        private static void TryPatchSystem()
        {
            // 번들에서 로드하는 리소스 타입이 설정되어 있는 경우, 번들 패치를 위해 패치 씬으로 넘어간다.
            if (SystemMaintenance.HasBundleLoadResourceType())
            {
                SceneTool.TurnToSystemScene(SceneTool.SystemSceneType.PatchScene);
            }
            else
            {
                TryGameInit();
            }            
        }
        
        private static void TryGameInit()
        {
            switch (SystemEntryPreset.EntryMode)
            {
                case SystemEntryMode.SingleMode:
                case SystemEntryMode.MultiMode:
                case SystemEntryMode.SelectMode:
                    SceneTool.TurnToSystemScene(SceneTool.SystemSceneType.InitScene);
                    break;
                case SystemEntryMode.AttachMode:
                    TryGameInitWithoutSceneChange(true, false, GetSystemCancellationToken());
                    break;
                case SystemEntryMode.DebugMode:
                    TryDebugScene();
                    break;
            }
        }
        
        private static void TryTitleScene()
        {
            SceneTool.TurnToSystemScene(SceneTool.SystemSceneType.TitleScene);
        }

        private static void TryEntryScene()
        {
            SceneChangeManager.GetInstanceUnsafe.TurnSceneTo(SceneTool.SceneShortCutType.EntryScene);
        }

        public static async UniTask TryGameInitWithoutSceneChange(bool p_RunLoopFlag, bool p_CameraOpenFlag, CancellationToken p_CancellationToken)
        {
            await LoadSystemLoop(p_CancellationToken);
            await LoadSceneManager(p_CancellationToken);
            await LoadGameManager(p_CancellationToken);
            await LoadCameraManager(p_CancellationToken);
            await LoadUI(p_CancellationToken);
            
            if (p_RunLoopFlag)
            {
                SystemLoop.GetInstanceUnsafe.SetGameLoopStart();
            }

            if (p_CameraOpenFlag)
            {
                CameraManager.GetInstanceUnsafe.SetCameraDefaultConfig();
                CameraManager.GetInstanceUnsafe.OpenMainCamera();
                
                await LoadAudioManager(p_CancellationToken);
            }
        }
        
        private static void TryDebugScene()
        {
            HandleDebugMode();
        }
        
        #endregion
    }
}