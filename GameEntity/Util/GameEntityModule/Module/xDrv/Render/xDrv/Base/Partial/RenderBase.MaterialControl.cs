#if !SERVER_DRIVE
using UnityEngine;
using UnityEngine.Rendering;

namespace k514.Mono.Common
{
    public partial class RenderBase
    {
        #region <Consts>

        private const string WrappingTexturePropertyName = "_MatCap";
        private const string DissolveEdgeTexturePropertyName = "_EdgeAroundRamp";
        private const string DissolveTexturePropertyName = "_DissolveTex";
        private const string DissolveEdgeFloatPropertyName = "_DissolveEdge";
        private const string DissolveProgressFloatPropertyName = "_DissolveProgress";
        private const string DissolveEnableFlagPropertyName = "_UsingDissolve";
        
        private const string OutlineColorPropertyName = "_OutlineColor";
        private const string OutlineWidthPropertyName = "_OutlineWidth";
        private const string OutlineEnableFlagPropertyName = "_UsingOutline";
        
        private const string EradicationIntensityFloatPropertyName = "_EradicationIntensity";
        private const string EradicationEnableFlagPropertyName = "_UsingEradication";
        
        private const string SilhouetteColorPropertyName = "_SilhouetteColor";
        private const string SilhouetteEnableFlagPropertyName = "_UsingSilhouette";
        
        #endregion
        
        #region <Method/Silhouette>

        public void SetSilhouetteEnable(RenderableTool.RenderGroupType p_RenderGroupType, bool p_Flag)
        {
            if (ShaderTypeMask.HasAnyFlagExceptNone(RenderableTool.ShaderControlType.Silhouette))
            {
                foreach (var renderGroupType in RenderableTool.RenderGroupTypeEnumerator)
                {
                    if (p_RenderGroupType.HasAnyFlagExceptNone(renderGroupType))
                    {
                        switch (renderGroupType)
                        {
                            case RenderableTool.RenderGroupType.OriginGroup:
                                ShaderTool.SetFlag<SkinnedMeshRenderer>(_DefaultModelRendererControlPreset, SilhouetteEnableFlagPropertyName, p_Flag);
                                break;
                            case RenderableTool.RenderGroupType.AddedGroup:
                                ShaderTool.SetFlag<SkinnedMeshRenderer>(_AddedModelRendererControlPreset, SilhouetteEnableFlagPropertyName, p_Flag);
                                break;
                        }
                    }
                }
            }
        }

        public void SetSilhouetteColor(RenderableTool.RenderGroupType p_RenderGroupType, Color p_Color)
        {
            if (ShaderTypeMask.HasAnyFlagExceptNone(RenderableTool.ShaderControlType.Silhouette))
            {
                foreach (var renderGroupType in RenderableTool.RenderGroupTypeEnumerator)
                {
                    if (p_RenderGroupType.HasAnyFlagExceptNone(renderGroupType))
                    {
                        switch (renderGroupType)
                        {
                            case RenderableTool.RenderGroupType.OriginGroup:
                                ShaderTool.SetColor<SkinnedMeshRenderer>(_DefaultModelRendererControlPreset, SilhouetteColorPropertyName, p_Color);
                                break;
                            case RenderableTool.RenderGroupType.AddedGroup:
                                ShaderTool.SetColor<SkinnedMeshRenderer>(_AddedModelRendererControlPreset, SilhouetteColorPropertyName, p_Color);
                                break;
                        }
                    }
                }
            }
        }

        #endregion
        
        #region <Method/MatCap/Intensity>

        public void SetIntensityEnable(RenderableTool.RenderGroupType p_RenderGroupType, bool p_Flag)
        {
            if (ShaderTypeMask.HasAnyFlagExceptNone(RenderableTool.ShaderControlType.Intensity))
            {
                foreach (var renderGroupType in RenderableTool.RenderGroupTypeEnumerator)
                {
                    if (p_RenderGroupType.HasAnyFlagExceptNone(renderGroupType))
                    {
                        switch (renderGroupType)
                        {
                            case RenderableTool.RenderGroupType.OriginGroup:
                                ShaderTool.SetFlag<SkinnedMeshRenderer>(_DefaultModelRendererControlPreset, EradicationEnableFlagPropertyName, p_Flag);
                                break;
                            case RenderableTool.RenderGroupType.AddedGroup:
                                ShaderTool.SetFlag<SkinnedMeshRenderer>(_AddedModelRendererControlPreset, EradicationEnableFlagPropertyName, p_Flag);
                                break;
                        }
                    }
                }
            }
        }
        
        public void SetIntensityValue(RenderableTool.RenderGroupType p_RenderGroupType, float p_Value)
        {
            if (ShaderTypeMask.HasAnyFlagExceptNone(RenderableTool.ShaderControlType.Intensity))
            {
                foreach (var renderGroupType in RenderableTool.RenderGroupTypeEnumerator)
                {
                    if (p_RenderGroupType.HasAnyFlagExceptNone(renderGroupType))
                    {
                        switch (renderGroupType)
                        {
                            case RenderableTool.RenderGroupType.OriginGroup:
                                ShaderTool.SetFloat<SkinnedMeshRenderer>(_DefaultModelRendererControlPreset, EradicationIntensityFloatPropertyName, p_Value);
                                break;
                            case RenderableTool.RenderGroupType.AddedGroup:
                                ShaderTool.SetFloat<SkinnedMeshRenderer>(_AddedModelRendererControlPreset, EradicationIntensityFloatPropertyName, p_Value);
                                break;
                        }
                    }
                }
            }
        }

        #endregion
        
        #region <Method/MatCap/Outline>

        public void SetOutlineEnable(RenderableTool.RenderGroupType p_RenderGroupType, bool p_Flag)
        {
            if (ShaderTypeMask.HasAnyFlagExceptNone(RenderableTool.ShaderControlType.Outline))
            {
                foreach (var renderGroupType in RenderableTool.RenderGroupTypeEnumerator)
                {
                    if (p_RenderGroupType.HasAnyFlagExceptNone(renderGroupType))
                    {
                        switch (renderGroupType)
                        {
                            case RenderableTool.RenderGroupType.OriginGroup:
                                ShaderTool.SetFlag<SkinnedMeshRenderer>(_DefaultModelRendererControlPreset, OutlineEnableFlagPropertyName, p_Flag);
                                break;
                            case RenderableTool.RenderGroupType.AddedGroup:
                                ShaderTool.SetFlag<SkinnedMeshRenderer>(_AddedModelRendererControlPreset, OutlineEnableFlagPropertyName, p_Flag);
                                break;
                        }
                    }
                }
            }
        }

        public void SetOutlineColor(RenderableTool.RenderGroupType p_RenderGroupType, Color p_Color)
        {
            if (ShaderTypeMask.HasAnyFlagExceptNone(RenderableTool.ShaderControlType.Outline))
            {
                foreach (var renderGroupType in RenderableTool.RenderGroupTypeEnumerator)
                {
                    if (p_RenderGroupType.HasAnyFlagExceptNone(renderGroupType))
                    {
                        switch (renderGroupType)
                        {
                            case RenderableTool.RenderGroupType.OriginGroup:
                                ShaderTool.SetColor<SkinnedMeshRenderer>(_DefaultModelRendererControlPreset, OutlineColorPropertyName, p_Color);
                                break;
                            case RenderableTool.RenderGroupType.AddedGroup:
                                ShaderTool.SetColor<SkinnedMeshRenderer>(_AddedModelRendererControlPreset, OutlineColorPropertyName, p_Color);
                                break;
                        }
                    }
                }
            }
        }

        public void SetOutlineWidth(RenderableTool.RenderGroupType p_RenderGroupType, float p_Width)
        {
            if (ShaderTypeMask.HasAnyFlagExceptNone(RenderableTool.ShaderControlType.Outline))
            {
                foreach (var renderGroupType in RenderableTool.RenderGroupTypeEnumerator)
                {
                    if (p_RenderGroupType.HasAnyFlagExceptNone(renderGroupType))
                    {
                        switch (renderGroupType)
                        {
                            case RenderableTool.RenderGroupType.OriginGroup:
                                ShaderTool.SetFloat<SkinnedMeshRenderer>(_DefaultModelRendererControlPreset, OutlineWidthPropertyName, p_Width);
                                break;
                            case RenderableTool.RenderGroupType.AddedGroup:
                                ShaderTool.SetFloat<SkinnedMeshRenderer>(_AddedModelRendererControlPreset, OutlineWidthPropertyName, p_Width);
                                break;
                        }
                    }
                }
            }
        }

        #endregion
        
        #region <Method/MatCap/Dissolve>

        public void SetDissolveEnable(RenderableTool.RenderGroupType p_RenderGroupType, bool p_Flag)
        {
            if (ShaderTypeMask.HasAnyFlagExceptNone(RenderableTool.ShaderControlType.Dissolve))
            {
                foreach (var renderGroupType in RenderableTool.RenderGroupTypeEnumerator)
                {
                    if (p_RenderGroupType.HasAnyFlagExceptNone(renderGroupType))
                    {
                        switch (renderGroupType)
                        {
                            case RenderableTool.RenderGroupType.OriginGroup:
                                ShaderTool.SetFlag<SkinnedMeshRenderer>(_DefaultModelRendererControlPreset, DissolveEnableFlagPropertyName, p_Flag);
                                break;
                            case RenderableTool.RenderGroupType.AddedGroup:
                                ShaderTool.SetFlag<SkinnedMeshRenderer>(_AddedModelRendererControlPreset, DissolveEnableFlagPropertyName, p_Flag);
                                break;
                        }
                    }
                }
            }
        }
        
        public void SetDissolveLerpProgress(RenderableTool.RenderGroupType p_RenderGroupType, float p_Progress01)
        {
            if (ShaderTypeMask.HasAnyFlagExceptNone(RenderableTool.ShaderControlType.Dissolve))
            {
                foreach (var renderGroupType in RenderableTool.RenderGroupTypeEnumerator)
                {
                    if (p_RenderGroupType.HasAnyFlagExceptNone(renderGroupType))
                    {
                        switch (renderGroupType)
                        {
                            case RenderableTool.RenderGroupType.OriginGroup:
                                ShaderTool.SetFloat<SkinnedMeshRenderer>(_DefaultModelRendererControlPreset, DissolveProgressFloatPropertyName, p_Progress01);
                                break;
                            case RenderableTool.RenderGroupType.AddedGroup:
                                ShaderTool.SetFloat<SkinnedMeshRenderer>(_AddedModelRendererControlPreset, DissolveProgressFloatPropertyName, p_Progress01);
                                break;
                        }
                    }
                }
            }
        }
        
        public void SetDissolveEdge(RenderableTool.RenderGroupType p_RenderGroupType, float p_Float)
        {
            if (ShaderTypeMask.HasAnyFlagExceptNone(RenderableTool.ShaderControlType.Dissolve))
            {
                foreach (var renderGroupType in RenderableTool.RenderGroupTypeEnumerator)
                {
                    if (p_RenderGroupType.HasAnyFlagExceptNone(renderGroupType))
                    {
                        switch (renderGroupType)
                        {
                            case RenderableTool.RenderGroupType.OriginGroup:
                                ShaderTool.SetFloat<SkinnedMeshRenderer>(_DefaultModelRendererControlPreset, DissolveEdgeFloatPropertyName, p_Float);
                                break;
                            case RenderableTool.RenderGroupType.AddedGroup:
                                ShaderTool.SetFloat<SkinnedMeshRenderer>(_AddedModelRendererControlPreset, DissolveEdgeFloatPropertyName, p_Float);
                                break;
                        }
                    }
                }
            }
        }
        
        public void SetDissolveTexture(RenderableTool.RenderGroupType p_RenderGroupType, int p_TextureType)
        {
            if (ShaderTypeMask.HasAnyFlagExceptNone(RenderableTool.ShaderControlType.Dissolve))
            {
                foreach (var renderGroupType in RenderableTool.RenderGroupTypeEnumerator)
                {
                    if (p_RenderGroupType.HasAnyFlagExceptNone(renderGroupType))
                    {
                        switch (renderGroupType)
                        {
                            case RenderableTool.RenderGroupType.OriginGroup:
                                ShaderTool.SetTexture<SkinnedMeshRenderer>(_DefaultModelRendererControlPreset, DissolveTexturePropertyName, p_TextureType);
                                break;
                            case RenderableTool.RenderGroupType.AddedGroup:
                                ShaderTool.SetTexture<SkinnedMeshRenderer>(_AddedModelRendererControlPreset, DissolveTexturePropertyName, p_TextureType);
                                break;
                        }
                    }
                }
            }
        }
        
        public void SetDissolveEdgeTexture(RenderableTool.RenderGroupType p_RenderGroupType, int p_TextureType)
        {
            if (ShaderTypeMask.HasAnyFlagExceptNone(RenderableTool.ShaderControlType.Dissolve))
            {
                foreach (var renderGroupType in RenderableTool.RenderGroupTypeEnumerator)
                {
                    if (p_RenderGroupType.HasAnyFlagExceptNone(renderGroupType))
                    {
                        switch (renderGroupType)
                        {
                            case RenderableTool.RenderGroupType.OriginGroup:
                                ShaderTool.SetTexture<SkinnedMeshRenderer>(_DefaultModelRendererControlPreset, DissolveEdgeTexturePropertyName, p_TextureType);
                                break;
                            case RenderableTool.RenderGroupType.AddedGroup:
                                ShaderTool.SetTexture<SkinnedMeshRenderer>(_AddedModelRendererControlPreset, DissolveEdgeTexturePropertyName, p_TextureType);
                                break;
                        }
                    }
                }
            }
        }

        #endregion
        
        #region <Method/MatCap/Texture>

        /// <summary>
        /// 지정한 랜더러 그룹의 _MatCap 텍스처를 일괄변경한다.
        /// </summary>
        public void SetWrappingTexture(RenderableTool.RenderGroupType p_RenderGroupType, int p_TextureType)
        {
            foreach (var renderGroupType in RenderableTool.RenderGroupTypeEnumerator)
            {
                if (p_RenderGroupType.HasAnyFlagExceptNone(renderGroupType))
                {
                    switch (renderGroupType)
                    {
                        case RenderableTool.RenderGroupType.OriginGroup:
                            ShaderTool.SetTexture<SkinnedMeshRenderer>(_DefaultModelRendererControlPreset, WrappingTexturePropertyName, p_TextureType);
                            break;
                        case RenderableTool.RenderGroupType.AddedGroup:
                            ShaderTool.SetTexture<SkinnedMeshRenderer>(_AddedModelRendererControlPreset, WrappingTexturePropertyName, p_TextureType);
                            break;
                    }
                }
            }
        }

        #endregion
        
        #region <Method/RendererQueue>

        /// <summary>
        /// 지정한 랜더러 그룹의 랜더 큐 값을 일괄변경한다.
        /// </summary>
        public void SetRenderQueue(RenderableTool.RenderGroupType p_RenderGroupType, RenderQueue p_QueueType)
        {
            foreach (var renderGroupType in RenderableTool.RenderGroupTypeEnumerator)
            {
                if (p_RenderGroupType.HasAnyFlagExceptNone(renderGroupType))
                {
                    switch (renderGroupType)
                    {
                        case RenderableTool.RenderGroupType.OriginGroup:
                            ShaderTool.SetRenderQueue<SkinnedMeshRenderer>(_DefaultModelRendererControlPreset, p_QueueType);
                            break;
                        case RenderableTool.RenderGroupType.AddedGroup:
                            ShaderTool.SetRenderQueue<SkinnedMeshRenderer>(_AddedModelRendererControlPreset, p_QueueType);
                            break;
                    }
                }
            }
        }
        
        /// <summary>
        /// 지정한 랜더러 그룹의 랜더 큐 값을 일괄변경한다.
        /// </summary>
        public void SetRenderQueue(RenderableTool.RenderGroupType p_RenderGroupType, int p_QueueDepth)
        {
            foreach (var renderGroupType in RenderableTool.RenderGroupTypeEnumerator)
            {
                if (p_RenderGroupType.HasAnyFlagExceptNone(renderGroupType))
                {
                    switch (renderGroupType)
                    {
                        case RenderableTool.RenderGroupType.OriginGroup:
                            ShaderTool.SetRenderQueue(_DefaultModelRendererControlPreset.RendererGroup, p_QueueDepth);
                            break;
                        case RenderableTool.RenderGroupType.AddedGroup:
                            ShaderTool.SetRenderQueue(_AddedModelRendererControlPreset.RendererGroup, p_QueueDepth);
                            break;
                    }
                }
            }
        }

        #endregion
    }
}
#endif