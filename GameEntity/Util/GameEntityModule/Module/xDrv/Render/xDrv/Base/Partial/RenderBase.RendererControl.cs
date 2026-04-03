#if !SERVER_DRIVE
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace k514.Mono.Common
{
    public partial class RenderBase
    {
        #region <Method/Shadow>

        /// <summary>
        /// 지정한 랜더러 그룹의 랜더러 그림자 투영 여부를 일괄변경한다.
        /// </summary>
        public void SetRendererShadow(RenderableTool.RenderGroupType p_RenderGroupType, bool p_Enable)
        {
            foreach (var renderGroupType in RenderableTool.RenderGroupTypeEnumerator)
            {
                if (p_RenderGroupType.HasAnyFlagExceptNone(renderGroupType))
                {
                    switch (renderGroupType)
                    {
                        case RenderableTool.RenderGroupType.OriginGroup:
                            ShaderTool.SetRenderShadow<SkinnedMeshRenderer>(_DefaultModelRendererControlPreset, p_Enable ? ShadowCastingMode.On : ShadowCastingMode.Off);
                            break;
                        case RenderableTool.RenderGroupType.AddedGroup:
                            ShaderTool.SetRenderShadow<SkinnedMeshRenderer>(_AddedModelRendererControlPreset, p_Enable ? ShadowCastingMode.On : ShadowCastingMode.Off);
                            break;
                    }
                }
            }
        }

        #endregion

        #region <Method/Layer>
        
        /// <summary>
        /// 지정한 랜더러 그룹의 랜더러의 게임 레이어 타입을 일괄변경한다.
        /// </summary>
        public void TurnRendererLayerTo(RenderableTool.RenderGroupType p_RenderGroupType, GameConst.GameLayerType p_LayerType)
        {
            foreach (var renderGroupType in RenderableTool.RenderGroupTypeEnumerator)
            {
                if (p_RenderGroupType.HasAnyFlagExceptNone(renderGroupType))
                {
                    switch (renderGroupType)
                    {
                        case RenderableTool.RenderGroupType.OriginGroup:
                            foreach (var renderer in (List<SkinnedMeshRenderer>)_DefaultModelRendererControlPreset)
                            {
                                renderer.gameObject.TurnLayerTo(p_LayerType, false);
                            }
                            break;
                        case RenderableTool.RenderGroupType.AddedGroup:
                            foreach (var renderer in (List<SkinnedMeshRenderer>)_AddedModelRendererControlPreset)
                            {
                                renderer.gameObject.TurnLayerTo(p_LayerType, false);
                            }
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 지정한 랜더러 그룹의 랜더러의 게임 레이어 타입을 게임 유닛의 레이어 타입으로 일괄변경한다.
        /// </summary>
        public void ResetRendererLayer(RenderableTool.RenderGroupType p_RenderGroupType)
        {
            /*var defaultLayer = _Entity.gameObject.layer;
            
            foreach (var renderGroupType in RenderableTool.RenderGroupTypeEnumerator)
            {
                if (p_RenderGroupType.HasAnyFlagExceptNone(renderGroupType))
                {
                    switch (renderGroupType)
                    {
                        case RenderableTool.RenderGroupType.OriginGroup:
                            foreach (var renderer in (List<SkinnedMeshRenderer>)_DefaultModelRendererControlPreset)
                            {
                                renderer.gameObject.layer = defaultLayer;
                            }               
                            break;
                        case RenderableTool.RenderGroupType.AddedGroup:
                            foreach (var renderer in (List<SkinnedMeshRenderer>)_AddedModelRendererControlPreset)
                            {
                                renderer.gameObject.layer = defaultLayer;
                            }                   
                            break;
                    }
                }
            }*/
        }

        #endregion
    }
}
#endif