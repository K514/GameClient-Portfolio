using System.Collections.Generic;
using k514.Mono.Feature;
using UnityEngine;
using xk514;

namespace k514.Mono.Common
{
    public class SceneBootstrap : MonoBehaviour
    {
        #region <Fields>
        
        /// <summary>
        /// 비정규 방식으로 씬 실행시 사용할 씬 변수 레코드 인덱스
        /// </summary>
        public int SceneVariableIndex;
        
        /// <summary>
        /// 비정규 방식으로 씬 실행시 사용할 던전 레코드 인덱스
        /// </summary>
        public int DungeonIndex;
        
        /// <summary>
        /// 비정규 방식으로 씬 실행시 사용할 파티 멤버 인덱스 리스트
        /// </summary>
        public List<int> FallbackPartyMemberIndexList = new List<int>{ 11, 21, 31, 41 };
   
        #endregion

        #region <Callbacks>

        /// <summary>
        /// 해당 씬이 SceneChangeManager 외의 방법으로 열린 경우에 씬 이벤트를 처리하기 위한 로직
        /// </summary>
        protected async void Awake()
        {
            // 시스템 초기화
            await SystemBoot.StartSystem();
            
            // 게임 매니저 초기화
            await SystemBoot.TryGameInitWithoutSceneChange(false, false, SystemBoot.GetSystemCancellationToken());

#if UNITY_EDITOR && APPLY_TEST_MANAGER
            // 테스트 매니저 초기화
            await SingletonTool.CreateSingletonAsync(typeof(TestManager), SystemBoot.GetSystemCancellationToken());
#endif
            
            // 현재 씬이 씬 매니저에 의해 제어받지 않는 경우
            if (SceneChangeManager.GetInstanceUnsafe.IsFreeScene())
            {
                // 수동으로 씬 이벤트를 호출해준다.
                var activeScenePath = SceneTool.GetActiveScenePath();
                if (SceneInfoQueryTable.GetInstanceUnsafe.TryGetSceneInfoByPath(activeScenePath, out var o_SceneInfo))
                {
                    SceneChangeManager.GetInstanceUnsafe.SetSceneChangePreset(new SceneTool.SceneChangePreset(o_SceneInfo, SceneVariableIndex, default));
                    GameManager.GetInstanceUnsafe.StartFallbackDungeon(this);
                    await SceneChangeManager.GetInstanceUnsafe.OnSceneTerminate(SystemBoot.GetSystemCancellationToken());
                    await SceneChangeManager.GetInstanceUnsafe.OnSceneTransition(SystemBoot.GetSystemCancellationToken());
                    await SceneChangeManager.GetInstanceUnsafe.OnScenePreload(SystemBoot.GetSystemCancellationToken());
                    await SceneChangeManager.GetInstanceUnsafe.OnSceneStart(SystemBoot.GetSystemCancellationToken());
                }
            }
        }
        
        #endregion
    }
}