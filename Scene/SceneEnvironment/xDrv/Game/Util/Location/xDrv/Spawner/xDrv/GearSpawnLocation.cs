using UnityEngine;

namespace k514.Mono.Common
{
    public class GearSpawnLocation : SpawnLocationBase
    {
        #region <Methods>

        protected override void PreloadEntity(int p_Index)
        {
        }
        
        protected override IGameEntityBridge SpawnEntity(int p_Index, AffinePreset p_AffinePreset)
        {
            var entity = 
                GearPoolManager.GetInstanceUnsafe
                    .Pop
                    (
                        GearPoolManager.GetInstanceUnsafe.GetCreateParams(p_Index, ResourceLifeCycleType.ManualUnload), 
                        new GearPoolManager.ActivateParams
                        (
                            null, 
                            new AffineCorrectionPreset
                            (
                                AffineTool.CorrectPositionType.ForceSurface, 
                                p_AffinePreset, 
                                GameConst.Terrain_LayerMask
                            ),
                            GameEntityTool.ActivateParamsAttributeType.None,
                            null
                        )
                    );
            
            return entity;
        }

        #endregion
    }
}