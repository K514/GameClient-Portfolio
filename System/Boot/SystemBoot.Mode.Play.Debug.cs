using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using Cysharp.Threading.Tasks;
using k514.Mono.Common;
using k514.Mono.Feature;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace k514
{
    public partial class SystemBoot
    {
        #region <Callbacks>

        public static async UniTask HandleDebugMode()
        {
            {
                Debug.LogError("디버그 함수 시작");
            }
            
            await TryGameInitWithoutSceneChange(true, true, GetSystemCancellationToken());
            UIxControlRoot.GetInstanceUnsafe.MainHUD.SetHide(false);
            UIxControlRoot.GetInstanceUnsafe.SceneFader.SetFadeIn(UIxTool.UIAfterFadeType.Hide, false, null);

            await AudioManager.GetInstanceUnsafe.LoadClip(1, GetSystemCancellationToken());

            var audioPbject = AudioManager.GetInstanceUnsafe.PlayClip(AudioClipNameTableQuery.TableLabel.BGM, 1, DataAccessType.First);
            audioPbject.SetLoop(true);
        }

        private static async UniTask PrefabTableTest(CancellationToken p_CancellationToken)
        {
        }

        private static async UniTask UITest(CancellationToken p_CancellationToken)
        {
            var cameraManager = await CameraManager.GetInstanceSafe(p_CancellationToken);
            Debug.LogError(ReferenceEquals(null, cameraManager));
            
            var uiControlRoot = await UIxControlRoot.GetInstanceSafe(p_CancellationToken);
            await UniTask.Delay(1500);

            var tryFader = uiControlRoot.MainFader;
            Debug.LogError(tryFader.CanvasPreset.LayerIndex);
            tryFader.CanvasPreset.RootPreset.Dispose();
            await UniTask.Delay(3000);
        }

        #endregion
    }
}