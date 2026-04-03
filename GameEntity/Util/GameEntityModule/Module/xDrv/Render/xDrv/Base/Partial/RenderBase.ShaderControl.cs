#if !SERVER_DRIVE
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace k514.Mono.Common
{
    public partial class RenderBase
    {
        #region <Methods>

        /// <summary>
        /// 지정한 랜더러 그룹의 랜더러 셰이더를 일괄변경한다.
        /// </summary>
        public void SetShader(RenderableTool.RenderGroupType p_RenderGroupType, RenderableTool.ShaderControlType p_ShaderType)
        {
            foreach (var renderGroupType in RenderableTool.RenderGroupTypeEnumerator)
            {
                if (p_RenderGroupType.HasAnyFlagExceptNone(renderGroupType))
                {
                    switch (renderGroupType)
                    {
                        case RenderableTool.RenderGroupType.OriginGroup:
                            _DefaultModelRendererControlPreset.SwitchShaderSet(p_ShaderType);
                            break;
                        case RenderableTool.RenderGroupType.AddedGroup:
                            _AddedModelRendererControlPreset.SwitchShaderSet(p_ShaderType);
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 신규 랜더러를 추가한다.
        /// </summary>
        public void AddRenderer(SkinnedMeshRenderer p_Renderer)
        {
            _AddedModelRendererControlPreset.SetRendererGroup(p_Renderer);
        }
        
        #endregion
    }
}
#endif