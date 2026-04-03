#if UNITY_EDITOR

using System;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using xk514;

namespace k514
{
    public partial class SystemBoot
    {
        #region <Fields>

        public static Action OnLoadMethodOver;
        public static bool UpdateResourceListTableFlag;
        
        #endregion

        #region <Callbacks>

        /// <summary>
        /// 에디터를 켜거나, 컴파일 시 호출되는 메서드
        /// </summary>
        [InitializeOnLoadMethod]
        private static async void OnLoadMethod()
        {
            OnInitSystemEditor();

            // 에디터 모드에서만 동작한다. 
            if (!EditorApplication.isPlayingOrWillChangePlaymode)
            {
                await StartSystem();

                if (SystemFlagTable.IsSystemDevMode())
                {
                    await SystemMaintenance.UpdateBuiltInSceneSetting(_SystemTaskCancellationTokenSource.Token);
                    CheckSystemObject();
                }
            }
        }

        private static void OnInitSystemEditor()
        {
            EditorApplication.pauseStateChanged += OnEditorPauseStateChanged;
            EditorApplication.playModeStateChanged += OnPlayModeChanged;
            EditorSceneManager.sceneOpened += OnSceneOpened;
        }
        
        private static void OnReleaseSystemEditor()
        {
            EditorApplication.pauseStateChanged -= OnEditorPauseStateChanged;
            EditorApplication.playModeStateChanged -= OnPlayModeChanged;
            EditorSceneManager.sceneOpened -= OnSceneOpened;
        }

        private static async void OnEditorPauseStateChanged(PauseState p_Type)
        {
#if APPLY_PRINT_LOG
            CustomDebug.LogError(($"[SystemBoot] OnEditorPauseStateChanged {p_Type}", Color.blue));
#endif
            
            switch (p_Type)
            {
                case PauseState.Paused:
                    break;
                case PauseState.Unpaused:
                    break;
            }
            
            await UniTask.CompletedTask;
        }

        private static async void OnPlayModeChanged(PlayModeStateChange p_Type)
        {
#if APPLY_PRINT_LOG
            CustomDebug.LogError(($"[SystemBoot] OnPlayModeChanged {p_Type}", Color.yellow));
#endif
            switch (p_Type)
            {
                case PlayModeStateChange.EnteredEditMode:
                {
                    await StartSystem();
                    break;
                }
                case PlayModeStateChange.ExitingPlayMode:
                {
                    await BreakSystem();
                    break;
                }
                case PlayModeStateChange.ExitingEditMode:
                case PlayModeStateChange.EnteredPlayMode:
                {
                    break;
                }
            }
        }

        private static void OnSceneOpened(Scene scene, OpenSceneMode mode)
        {
            CheckSystemObject();
        }

        #endregion
    }
}

#endif