using System.Collections.Generic;
using System.Linq;
using k514.Mono.Feature;
using UnityEngine;

namespace k514.Mono.Common
{
    public abstract partial class GameSceneEnvironmentBase
    {
        #region <Fields>

        private Dictionary<SceneTool.SceneLocationType, List<LocationBase>> LocationTable;
      
        #endregion

        #region <Callbacks>

        private void OnCreateSceneLocation()
        {
            LocationTable = new Dictionary<SceneTool.SceneLocationType, List<LocationBase>>();
            var enumerator = EnumFlag.GetEnumEnumerator<SceneTool.SceneLocationType>(EnumFlag.GetEnumeratorType.ExceptNone);
            foreach (var locationType in enumerator)
            {
                LocationTable.Add(locationType, new List<LocationBase>());
            }
            
            var sceneVariableRecord = SceneEnvironmentManager.GetInstanceUnsafe.CurrentSceneVariableDataRecord;
            var sceneLocationMetaSet = sceneVariableRecord.SceneLocationMetaSet;
            if (sceneLocationMetaSet.CheckCollectionSafe())
            {
                foreach (var sceneLocationMeta in sceneLocationMetaSet)
                {
                    var spawnType = sceneLocationMeta.LocationType;
                    switch (spawnType)
                    {
                        default:
                        case SceneTool.SceneLocationType.None:
                            break;
                        case SceneTool.SceneLocationType.ZakoSpawner:
                        {
                            var location = new GameObject("ZSpawner").AddComponent<ZakoSpawnLocation>();
                            location.SetSceneLocationMeta(this, SceneTool.SceneLocationType.ZakoSpawner, sceneLocationMeta);
                            LocationTable[SceneTool.SceneLocationType.ZakoSpawner].Add(location);
                            break;
                        }
                        case SceneTool.SceneLocationType.ChampSpawner:
                        {
                            var location = new GameObject("CSpawner").AddComponent<ChampSpawnLocation>();
                            location.SetSceneLocationMeta(this, SceneTool.SceneLocationType.ChampSpawner, sceneLocationMeta);
                            LocationTable[SceneTool.SceneLocationType.ChampSpawner].Add(location);
                            break;
                        }
                        case SceneTool.SceneLocationType.BossSpawner:
                        {
                            var location = new GameObject("BSpawner").AddComponent<BossSpawnLocation>();
                            location.SetSceneLocationMeta(this, SceneTool.SceneLocationType.BossSpawner, sceneLocationMeta);
                            LocationTable[SceneTool.SceneLocationType.BossSpawner].Add(location);
                            break;
                        }
                        case SceneTool.SceneLocationType.GearSpawner:
                        {
                            var location = new GameObject("GSpawner").AddComponent<GearSpawnLocation>();
                            location.SetSceneLocationMeta(this, SceneTool.SceneLocationType.GearSpawner, sceneLocationMeta);
                            LocationTable[SceneTool.SceneLocationType.GearSpawner].Add(location);
                            break;
                        }
                        case SceneTool.SceneLocationType.MessageSpawner:
                        {
                            var location = new GameObject("MsgPop").AddComponent<MessagePopLocation>();
                            location.SetSceneLocationMeta(this, SceneTool.SceneLocationType.MessageSpawner, sceneLocationMeta);
                            LocationTable[SceneTool.SceneLocationType.MessageSpawner].Add(location);
                            break;
                        }
                        case SceneTool.SceneLocationType.VictoryLocation:
                        {
                            var location = new GameObject("VictoryLocation").AddComponent<VictoryLocation>();
                            location.SetSceneLocationMeta(this, SceneTool.SceneLocationType.VictoryLocation, sceneLocationMeta);
                            LocationTable[SceneTool.SceneLocationType.VictoryLocation].Add(location);
                            break;
                        }
                    }
                }
            }
        }

        public void OnLocationActivated(LocationBase p_Location)
        {
        }
        
        #endregion

        #region <Methods>

        public bool IsLocationEliminate(SceneTool.SceneLocationType p_Type)
        {
            var locationList = LocationTable[p_Type];
            if (locationList.Count < 1)
            {
                return true;
            }
            else
            {
                foreach (var location in locationList)
                {
                    if (!location.IsEliminate())
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        public List<LocationBase> GetLocationList(SceneTool.SceneLocationType p_Type)
        {
            return LocationTable[p_Type];
        }

        protected void SetLocationPhase(LocationBase.LocationPhase p_Phase)
        {
            foreach (var locationKV in LocationTable)
            {
                var locationList = locationKV.Value;
                foreach (var location in locationList)
                {
                    location.SetPhase(p_Phase);
                }
            } 
            
            CheckStageClear();
        }
        
        public virtual void CheckStageClear()
        {
            if (IsLocationEliminate(SceneTool.SceneLocationType.VictoryLocation) 
                && IsLocationEliminate(SceneTool.SceneLocationType.ZakoSpawner) 
                && IsLocationEliminate(SceneTool.SceneLocationType.ChampSpawner) 
                && IsLocationEliminate(SceneTool.SceneLocationType.BossSpawner))
            {
                GameManager.GetInstanceUnsafe.ClearStage();
            }
        }
        
        #endregion
    }
}