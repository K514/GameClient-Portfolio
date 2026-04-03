using UnityEngine;

namespace k514.Mono.Common
{
    public abstract partial class GameSceneEnvironmentBase
    {
        #region <Fields>
        
        /*private AstarPath _AstarPath;*/
        
        #endregion
        
        #region <Callbacks>

        protected void OnCreateTerrain()
        {
            var terrainSet = FindObjectsByType<Terrain>(FindObjectsSortMode.None);
            if (terrainSet.CheckCollectionSafe())
            {
                foreach (var terrain in terrainSet)
                {
                    terrain.gameObject.TurnLayerTo(GameConst.GameLayerType.Terrain, false);
                    terrain.renderingLayerMask |= (uint) GameConst.RenderingLayerMaskType.DecalReceived;
                }
            }
            
            /*_AstarPath = FindAnyObjectByType<AstarPath>();
            if (_AstarPath != null)
            {
                AddAttribute(SceneTool.SceneEnvironmentAttribute.AStar);
            }*/
        }

        #endregion

        #region <Methods>

        /*
        public bool IsOnAStarPath(Vector3 p_Postion)
        {
            return HasAttribute(SceneTool.SceneEnvironmentAttribute.AStar) && _AstarPath.IsPointOnNavmesh(p_Postion);
        }
        */

        #endregion
    }
}