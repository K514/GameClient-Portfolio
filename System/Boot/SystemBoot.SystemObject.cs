#if UNITY_EDITOR

using Cysharp.Threading.Tasks;
using k514.Mono.Common;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace k514
{
    public static partial class SystemBoot
    {
        #region <Fields>

        private const SceneTool.SystemSceneType __BootSceneType = SceneTool.SystemSceneType.BootScene;

        #endregion

        #region <Methods>

        public static void CheckSystemObject()
        {
            // 현재 씬이 BootScene인 경우 
            if (__BootSceneType.ToString() == SceneTool.GetActiveSceneName(false))
            {
                var systemEntry = Object.FindObjectOfType<SystemEntry>();
                if (ReferenceEquals(null, systemEntry))
                {
                    systemEntry = new GameObject("SystemEntry").AddComponent<SystemEntry>();
                }
                                                
                var systemLoop = Object.FindObjectOfType<SystemLoop>();
                if (ReferenceEquals(null, systemLoop))
                {
                    systemLoop = new GameObject("SystemLoop").AddComponent<SystemLoop>();
                }
                
                systemLoop.transform.SetParent(systemEntry.transform);
            }
        }
        
        #endregion
    }
}

#endif