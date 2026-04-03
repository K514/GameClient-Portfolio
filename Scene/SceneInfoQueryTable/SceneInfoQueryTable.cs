using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using k514.Mono.Common;
using UnityEngine;
using UnityEngine.SceneManagement;
using xk514;

namespace k514
{
    /// <summary>
    /// 씬 리스트 테이블 레코드에 대한 쿼리를 제공하는 싱글톤
    /// </summary>
    public class SceneInfoQueryTable : AsyncSingleton<SceneInfoQueryTable>
    {
        #region <Fields>

        /// <summary>
        /// 이름 DB, 씬 이름을 통해 씬 프리셋을 관리하는 컬렉션
        /// </summary>
        private Dictionary<string, SceneTool.SceneInfo> _SceneNameQueryTable;
        private Dictionary<string, SceneTool.SceneInfo> _ScenePathQueryTable;

        #endregion

        #region <Callbacks>

        protected override void TryInitializeDependency()
        {
            base.TryInitializeDependency();

            _Dependencies.Add(typeof(SceneConstantDataTable));
            _Dependencies.Add(typeof(SceneVariableDataTable));
        }

        protected override async UniTask OnCreated(CancellationToken p_CancellationToken)
        {
            _SceneNameQueryTable = new Dictionary<string, SceneTool.SceneInfo>();
            _ScenePathQueryTable = new Dictionary<string, SceneTool.SceneInfo>();

            await UniTask.CompletedTask;
        }

        protected override async UniTask OnInitiate(CancellationToken p_CancellationToken)
        {
            _SceneNameQueryTable.Clear();
            _ScenePathQueryTable.Clear();

            // Built-In에 등록된 씬들을 읽어온다.
            var builtInSceneCount = SceneManager.sceneCountInBuildSettings;
            for (var i = 0; i < builtInSceneCount; i++)
            {
                var sceneFullPath = SceneUtility.GetScenePathByBuildIndex(i);
                if (!string.IsNullOrEmpty(sceneFullPath))
                {
                    var sceneInfo = new SceneTool.SceneInfo(sceneFullPath);
                    _SceneNameQueryTable.Add(sceneInfo.SceneName, sceneInfo);
                    _ScenePathQueryTable.Add(sceneInfo.SceneFullPath, sceneInfo);
                }
            }
            
            // SceneConstant 테이블에 등록된 씬들을 읽어온다.
            var sceneConstantTable = SceneConstantDataTable.GetInstanceUnsafe.GetTable();
            if (sceneConstantTable.CheckCollectionSafe())
            {
                var sceneLoadType = ResourceType.Scene.GetResourceLoadType();
                switch (sceneLoadType)
                {
                    // 번들로부터 씬을 로드해야하는 경우 해당 씬들은 BuiltIn씬에는 포함되지 않고, 에셋번들 생성 당시 위치했던
                    // 리소스 폴더 내부의 경로 위치로 접근이 가능한데, 해당 위치는 리소스 리스트에 저장되어 있으므로 해당 값을 참조한다.
                    case ResourceLoadType.FromAssetBundle:
                    {
                        foreach (var recordKV in sceneConstantTable)
                        {
                            var sceneName = recordKV.Key;
                            if (ResourceListTable.GetInstanceUnsafe.TryGetRecord(sceneName, out var o_Record))
                            {
                                // 씬 이름이 이미 BuiltIn에 등록되어 있는 경우
                                if (_SceneNameQueryTable.ContainsKey(sceneName))
                                {
#if APPLY_PRINT_LOG
                                    if (Application.isPlaying)
                                    {
                                        CustomDebug.LogError($"BuiltIn에 등록된 씬이름이 검색되었습니다. [{sceneName}]");
                                    }
#endif
                                }
                                else
                                {
                                    var sceneFullPath = o_Record.GetAssetResourcePathFormat();
                                    var sceneInfo = new SceneTool.SceneInfo(sceneFullPath);
                                    _SceneNameQueryTable.Add(sceneInfo.SceneName, sceneInfo);
                                    _ScenePathQueryTable.Add(sceneInfo.SceneFullPath, sceneInfo);
                                }
                            }
#if APPLY_PRINT_LOG
                            // 씬 이름이 리소스 리스트에 존재하지 않는 경우,
                            else
                            {
                                if (Application.isPlaying)
                                {
                                    CustomDebug.LogError($"리소스리스트에 등록되지 않은 씬이름이 검색되었습니다. [{sceneName}]");
                                }
                            }
#endif
                        }
                        break;
                    }
                    // 유니티 리소스 폴더에 포함되어 있는 씬들은 유니티 엔진 정책 상 BuiltIn에 등록되어 있어야하므로
                    // 해당 블록 까지 진행되었을때, _SceneNameQueryTable에는 이미 이름이 저장되어있다.
                    case ResourceLoadType.FromUnityResource:
                    {
                        foreach (var recordKV in sceneConstantTable)
                        {
                            var sceneName = recordKV.Key;
                            if (_SceneNameQueryTable.TryGetValue(sceneName, out var o_SceneInfo))
                            {
                            }
#if APPLY_PRINT_LOG
                            // BuiltIn에는 없고 SceneConstant에만 있는 씬 이름인 경우
                            else
                            {
                                CustomDebug.LogError($"BuiltIn에 등록되지 않은 씬이름이 검색되었습니다. [{sceneName}]");
                            }
#endif
                        }
                        break;
                    }    
                }
            }
            
            await UniTask.CompletedTask;
        }

        #endregion

        #region <Methods>

        public bool TryGetSceneInfoByName(string p_Key, out SceneTool.SceneInfo p_SceneInfo)
        {
            return _SceneNameQueryTable.TryGetValue(p_Key, out p_SceneInfo);
        }
        
        public bool TryGetSceneInfoByPath(string p_Path, out SceneTool.SceneInfo p_SceneInfo)
        {
            return _ScenePathQueryTable.TryGetValue(p_Path, out p_SceneInfo);
        }
        
#if UNITY_EDITOR
        public void PrintTable()
        {
            foreach (var sceneInfoKV in _SceneNameQueryTable)
            {
                Debug.LogError($"[SceneName : {sceneInfoKV.Key}], [SceneInfo : {sceneInfoKV.Value}]");
            }
        }
#endif

        #endregion
    }
}