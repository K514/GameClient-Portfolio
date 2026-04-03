using System;
using System.Collections.Generic;
using k514.Mono.Feature;
using UnityEngine;

namespace k514.Mono.Common
{
    public abstract partial class GameSceneEnvironmentBase
    {
        #region <Fields>

        private Transform _StartPos;
        private List<GameObject> _SceneDependencyObjects;

        #endregion

        #region <Callbacks>

        private void OnCreateSceneDependency()
        {
            var sceneVariableRecord = SceneEnvironmentManager.GetInstanceUnsafe.CurrentSceneVariableDataRecord;
            var startPreset = sceneVariableRecord.SceneStartPreset;
            
            /* 씬 시작지점 */
            _StartPos = new GameObject("StartPosition").transform;
            _StartPos.position = startPreset.StartPosition;
            _StartPos.rotation = Quaternion.Euler(startPreset.StartRotation);

            /* 씬 전용 오브젝트 생성 */
            _SceneDependencyObjects = new List<GameObject>();
            
            /* 씬에서 사용할 개체 프리로드 */
            PreloadSceneEntity();
        }

        #endregion

        #region <Methods>

        private void PreloadSceneEntity()
        {
            foreach (var locationListKV in LocationTable)
            {
                var locationType = locationListKV.Key;
                switch (locationType)
                {
                    case SceneTool.SceneLocationType.ZakoSpawner:
                    case SceneTool.SceneLocationType.ChampSpawner:
                    case SceneTool.SceneLocationType.BossSpawner:
                    case SceneTool.SceneLocationType.GearSpawner:
                    {
                        var locationList = locationListKV.Value;
                        foreach (var location in locationList)
                        {
                            location.OnLocationPreload();
                        }       
                        break;
                    }
                }
            }

            VfxTool.PreloadVfx();
        }
        
        private void TerminateSceneDependency()
        {
            foreach (var dependency in _SceneDependencyObjects)
            {
                DestroyImmediate(dependency);
            }
        }

        #endregion
    }
}