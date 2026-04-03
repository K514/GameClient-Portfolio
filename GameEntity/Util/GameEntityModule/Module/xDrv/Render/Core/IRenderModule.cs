using k514.Mono.Feature;
using UnityEngine;
using UnityEngine.Rendering;

namespace k514.Mono.Common
{
    /// <summary>
    /// 유닛 랜더링에 필요한 기능 및 제어 등을 기술하는 모듈
    /// </summary>
    public interface IRenderModule : IGameEntityModule
    {
#if !SERVER_DRIVE
        #region <Methods>

        RenderModuleDataTableQuery.TableLabel GetRenderModuleType();
        void TurnRendererLayerTo(RenderableTool.RenderGroupType p_RenderGroupType, GameConst.GameLayerType p_LayerType);
        void ResetRendererLayer(RenderableTool.RenderGroupType p_RenderGroupType);
        void SetRenderEffect(RenderableTool.ShaderControlType p_ShaderControlType, int p_UpdateCount);

        #endregion

        #region <Method/Renderer>

        void AddRenderer(SkinnedMeshRenderer p_Renderer);

        #endregion

        #region <Method/ShaderControl/Silhouette>

        void SetSilhouetteEnable(RenderableTool.RenderGroupType p_RenderGroupType, bool p_Flag);
        void SetSilhouetteColor(RenderableTool.RenderGroupType p_RenderGroupType, Color p_Color);

        #endregion
        
        #region <Method/ShaderControl/Outline>

        void SetOutlineEnable(RenderableTool.RenderGroupType p_RenderGroupType, bool p_Flag);
        void SetOutlineColor(RenderableTool.RenderGroupType p_RenderGroupType, Color p_Color);
        void SetOutlineWidth(RenderableTool.RenderGroupType p_RenderGroupType, float p_Width);
        
        #endregion

        #region <Method/ShaderControl/Intensity>
        
        void SetIntensityEnable(RenderableTool.RenderGroupType p_RenderGroupType, bool p_Flag);
        void SetIntensityValue(RenderableTool.RenderGroupType p_RenderGroupType, float p_Value);

        #endregion

        #region <Method/ShaderControl/Texture>

        void SetWrappingTexture(RenderableTool.RenderGroupType p_RenderGroupType, int p_TextureType);

        #endregion
        
        #region <Method/ShaderControl/Dissolve>

        void SetDissolveEnable(RenderableTool.RenderGroupType p_RenderGroupType, bool p_Flag);
        void SetDissolveLerpProgress(RenderableTool.RenderGroupType p_RenderGroupType, float p_Progress01);
        void SetDissolveEdge(RenderableTool.RenderGroupType p_RenderGroupType, float p_Float);
        void SetDissolveTexture(RenderableTool.RenderGroupType p_RenderGroupType, int p_TextureType);
        void SetDissolveEdgeTexture(RenderableTool.RenderGroupType p_RenderGroupType, int p_TextureType);

        #endregion

        #region <Method/ShaderControl/RendererQueue>

        void SetRenderQueue(RenderableTool.RenderGroupType p_RenderGroupType, RenderQueue p_QueueType);
        void SetRenderQueue(RenderableTool.RenderGroupType p_RenderGroupType, int p_QueueDepth);

        #endregion

/*
        #region <AttachVfx>

        (bool, VFXUnit) AttachVfx(GameEntityTool.AttachingVfxType p_Type, int p_SpawnIndex,
            TransformTool.AffineCachePreset p_Affine, uint p_PreDelay = 0);
        void DetachVfx(GameEntityTool.AttachingVfxType p_Type);
        
        #endregion
*/
#endif
    }
}