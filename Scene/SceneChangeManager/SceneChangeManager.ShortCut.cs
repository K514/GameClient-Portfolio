using System.Threading;
using Cysharp.Threading.Tasks;
using xk514;

namespace k514.Mono.Common
{
    public partial class SceneChangeManager
    {
        #region <Methods>
            
        private async UniTask<bool> TurnSceneTo(SceneTool.SceneChangePreset p_SceneChangePreset, CancellationToken p_CancellationToken)
        {
            await UniTask.DelayFrame(1, cancellationToken: p_CancellationToken);
    
            switch (_CurrentPhase)
            {
                case SceneControlPhase.None:
                    ReserveSceneChangePreset(p_SceneChangePreset);
                    await OnSceneTerminate(p_CancellationToken);
                    await UniTask.Delay(GameConst.DefaultSceneTransitionDelayMsec, DelayType.UnscaledDeltaTime, cancellationToken: p_CancellationToken);
                    await OnSceneTransition(p_CancellationToken);
                    _CurrentPhase = SceneControlPhase.TransitionScene;
                    SceneTool.TurnToLoadingScene(CurrentSceneControlPreset.SceneControlPreset.LoadingSceneType);
                    return true;
                default:
                case SceneControlPhase.Reserved:
                case SceneControlPhase.TransitionScene:
#if APPLY_PRINT_LOG
                    CustomDebug.LogError($"Scene Change Failed : SceneChangeManager Phase now on [{_CurrentPhase}]");
#endif
                    return false;
            } 
        }
        
        private async UniTask<bool> TurnSceneTo(SceneTool.SceneInfo p_SceneInfo, int p_SceneVariableIndex, SceneTool.SceneControlPreset p_SceneControlPreset)
        {
            if (ReferenceEquals(null, p_SceneInfo))
            {
#if APPLY_PRINT_LOG
                CustomDebug.LogError($"Scene Change Failed : Invalid SceneInfo");
#endif
                return false;
            }
            else
            {
                await TurnSceneTo(new SceneTool.SceneChangePreset(p_SceneInfo, p_SceneVariableIndex, p_SceneControlPreset), GetCancellationToken());
       
                return true;
            }
        }

        /// <summary>
        /// 지정한 씬 이름을 가지는 씬을 지정한 VariableIndex 세팅으로 로드하는 메서드
        /// 씬 이름은 경로를 생략하고 파일명.unity 를 입력한다.
        /// </summary>
        public async UniTask<bool> TurnSceneTo(string p_SceneName, int p_VariableIndex, SceneTool.SceneControlPreset p_SceneControlPreset = default)
        {
            if (SceneInfoQueryTable.GetInstanceUnsafe.TryGetSceneInfoByName(p_SceneName, out var o_SceneNameInfo))
            {
                return await TurnSceneTo(o_SceneNameInfo, p_VariableIndex, p_SceneControlPreset);
            }
            else
            {
                return false;
            }
        }
        
        /// <summary>
        /// 지정한 인덱스를 가진 SceneEntry 테이블을 기준으로 씬을 로드하는 메서드
        /// </summary>
        public async UniTask<bool> TurnSceneTo(int p_EntryIndex, SceneTool.SceneControlPreset p_SceneControlPreset = default)
        {
            var sceneEntryRecord = SceneEntryDataTable.GetInstanceUnsafe[p_EntryIndex];
            return await TurnSceneTo(sceneEntryRecord.SceneName, sceneEntryRecord.SceneVariableIndex, p_SceneControlPreset);
        }
        
        /// <summary>
        /// 지정한 씬 타입에 대응하는 테이블의 서수에 따라 특정 인덱스의 씬을 로드시키는 메서드
        /// </summary>
        public async UniTask<bool> TurnSceneTo(SceneTool.SceneShortCutType p_SceneType, SceneTool.SceneControlPreset p_SceneControlPreset = default)
        {
            switch (p_SceneType)
            {
                case SceneTool.SceneShortCutType.EntryScene:
                    return await TurnSceneTo(SystemBoot.SystemEntryPreset.EntryIndex, p_SceneControlPreset);
                case SceneTool.SceneShortCutType.LobbyScene:
                    return await TurnSceneTo(SceneTool.LOBBY_SCENE_ENTRY_INDEX, p_SceneControlPreset);
                default:
                    return false;
            }
        }
        
        /// <summary>
        /// 현재 씬에 재진입시키는 메서드
        /// </summary>
        public async UniTask<bool> ReenterScene()
        {
            var currentScenePreset = CurrentSceneControlPreset;
            if (currentScenePreset.ValidFlag)
            {
                return await TurnSceneTo(currentScenePreset, GetCancellationToken());
            }
            else
            {
                return false;
            }
        }

#if UNITY_EDITOR
        public async UniTask<bool> TurnSceneTo(SceneTool.EditorSceneType p_SceneType, SceneTool.SceneControlPreset p_SceneControlPreset = default)
        {
            if (EditorScenePathTable.GetInstanceUnsafe.TryGetRecord(SceneTool.EditorSceneType.Dexter, out var o_Record))
            {
                return await TurnSceneTo(o_Record.SceneName, 0, p_SceneControlPreset);
            }
            else
            {
                return false;
            }
        }
#endif
        #endregion
    }
}