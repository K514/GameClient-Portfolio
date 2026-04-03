using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using xk514;

namespace k514
{
    public static partial class SystemBoot
    {
        #region <Methods>

        /// <summary>
        /// 게임 시스템을 시작하는 메서드
        /// </summary>
        public static async UniTask<bool> StartSystem()
        {
            if (IsSystemOpenProgressing)
            {
#if APPLY_PRINT_LOG
                CustomDebug.LogError(($"[SystemBoot] StartSystem BusyWaiting Start", Color.blue));
#endif
                await WaitSystemOpen();
#if APPLY_PRINT_LOG
                CustomDebug.LogError(($"[SystemBoot] StartSystem BusyWaiting Release", Color.blue));
#endif
                return true;
            }
            else
            {
                if (IsSystemOpen)
                {
#if APPLY_PRINT_LOG
                    CustomDebug.LogError(($"[SystemBoot] StartSystem Free Pass", Color.blue));
#endif
                    return true;
                }
                else
                {
#if APPLY_PRINT_LOG
                    CustomDebug.LogError(($"[SystemBoot] StartSystem", Color.blue));
#endif
                    SystemMaintenance.InitDirectory();
                    SystemMaintenance.InitPlatformSetting();

                    OnBeginStartSystem();
                    OnInitSystemTime();
                    OnInitSystemToken();
                    OnInitSystemSingleton();
                    OnInitSystemConfig();
                    
                    if (await LoadSystemBasisSingleton(_SystemTaskCancellationTokenSource.Token))
                    {
                        await OnInitSystemEntry();
#if APPLY_PRINT_LOG
                        CustomDebug.LogError(($"[SystemBoot] StartSystem Success", Color.blue));
#endif
                        OnSuccessStartSystem();
                        return true;
                    }
                    else
                    {
#if APPLY_PRINT_LOG
                        if (_SystemTaskCancellationTokenSource.IsCancellationRequested)
                        {
                            CustomDebug.LogError(($"[SystemBoot] StartSystem Cancelled", Color.red));
                        }
                        else
                        {
                            CustomDebug.LogError(($"[SystemBoot] StartSystem Fail", Color.red));
                        }
#endif
                        OnFailStartSystem();
                        return false;
                    }
                }
            }
        }

        /// <summary>
        /// 게임 시스템을 종료하는 메서드
        /// </summary>
        public static async UniTask BreakSystem()
        {
#if APPLY_PRINT_LOG
            CustomDebug.LogWarning(($"[SystemBoot] ExitSystem", Color.blue));
#endif
            if (IsSystemOpen)
            {
#if UNITY_EDITOR
                if (UpdateResourceListTableFlag)
                {
                    UpdateResourceListTableFlag = false;
#if APPLY_PRINT_LOG
                    CustomDebug.LogError(($"[SystemBoot] ResourceList Update...", Color.cyan));
#endif
                    await UniTask.DelayFrame(1);
                    await ResourceListTable.GetInstanceUnsafe.WriteDefaultTable(true, _SystemTaskCancellationTokenSource.Token);
                }
#endif
                OnReleaseSystemEntry();
                OnReleaseSystemSingleton();
                OnReleaseSystemToken();
                OnReleaseSystemConfig();
                OnReleaseSystemTime();
                OnSuccessReleaseSystem();
            }
#if APPLY_PRINT_LOG
            CustomDebug.LogWarning(($"[SystemBoot] ExitSystem Success", Color.blue));
#endif
        }

        public static async UniTask<bool> RestartSystem()
        {
            await BreakSystem();
            
            return await StartSystem();
        }

        public static void QuitSystem()
        {
#if UNITY_EDITOR
            // 에디터에서 플레이 모드 중지
            EditorApplication.isPlaying = false;
#else
            // 실제 빌드에서는 앱 종료
            Application.Quit();
#endif
        }

        #endregion
    }
}