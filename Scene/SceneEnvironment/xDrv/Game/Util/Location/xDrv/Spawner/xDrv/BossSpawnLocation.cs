using System.Collections.Generic;
using k514.Mono.Feature;
using UnityEngine;

namespace k514.Mono.Common
{
    public class BossSpawnLocation : SpawnLocationBase
    {
        #region <Methods>

        protected override void PreloadEntity(int p_Index)
        {
        }

        protected override IGameEntityBridge SpawnEntity(int p_Index, AffinePreset p_AffinePreset)
        {
            var entity = 
                UnitPoolManager.GetInstanceUnsafe
                    .Pop
                    (
                        UnitPoolManager.GetInstanceUnsafe.GetCreateParams(p_Index), 
                        new UnitPoolManager.ActivateParams
                        (
                    null, 
                            new AffineCorrectionPreset
                            (
                                AffineTool.CorrectPositionType.ForceSurface, 
                                p_AffinePreset, 
                                GameConst.Terrain_LayerMask
                            ),
                            p_GameEntityActivateParamsAttributeMask: GameEntityTool.ActivateParamsAttributeType.GiveBoss
                        )
                    );
                   
            entity.SetGroupMask(3);
            entity.AddReceiver(_GameEntityEventReceiver);
            
            var createParams = VfxPoolManager.GetInstanceUnsafe.GetCreateParams(VfxTool.__MonsterSpawnVfxIndex);
            var activateParams =
                new VfxPoolManager.ActivateParams( 
                    null,
                    new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, new AffinePreset(entity.GetBottomPosition(), entity.Scale)));
            VfxPoolManager.GetInstanceUnsafe.Pop(createParams, activateParams);

            return entity;
        }

        #endregion
    }
}