#if !SERVER_DRIVE
using k514.Mono.Feature;
using UnityEngine;
using UnityEngine.Rendering;

namespace k514.Mono.Common
{
    public class NullRender : GameEntityModuleBase, IRenderModule
    {
        #region <Constructor>

        public NullRender() : base(GameEntityModuleTool.ModuleType.None, default, default)
        {
        }

        #endregion

        #region <Callbacks>

        protected override void _OnAwakeModule()
        {
        }

        protected override void _OnSleepModule()
        {
        }
        
        protected override void _OnResetModule()
        {
        }

        #endregion

        #region <Methods>

        public RenderModuleDataTableQuery.TableLabel GetRenderModuleType()
        {
            return RenderModuleDataTableQuery.TableLabel.None;
        }

        public void TurnRendererLayerTo(RenderableTool.RenderGroupType p_RenderGroupType, GameConst.GameLayerType p_LayerType)
        {
        }

        public void ResetRendererLayer(RenderableTool.RenderGroupType p_RenderGroupType)
        {
        }

        public void SetRenderEffect(RenderableTool.ShaderControlType p_ShaderControlType, int p_UpdateCount)
        {
        }

        public void AddRenderer(SkinnedMeshRenderer p_Renderer)
        {
        }

        public void SetSilhouetteEnable(RenderableTool.RenderGroupType p_RenderGroupType, bool p_Flag)
        {
        }

        public void SetSilhouetteColor(RenderableTool.RenderGroupType p_RenderGroupType, Color p_Color)
        {
        }

        public void SetOutlineEnable(RenderableTool.RenderGroupType p_RenderGroupType, bool p_Flag)
        {
        }

        public void SetOutlineColor(RenderableTool.RenderGroupType p_RenderGroupType, Color p_Color)
        {
        }

        public void SetOutlineWidth(RenderableTool.RenderGroupType p_RenderGroupType, float p_Width)
        {
        }

        public void SetIntensityEnable(RenderableTool.RenderGroupType p_RenderGroupType, bool p_Flag)
        {
        }

        public void SetIntensityValue(RenderableTool.RenderGroupType p_RenderGroupType, float p_Value)
        {
        }

        public void SetWrappingTexture(RenderableTool.RenderGroupType p_RenderGroupType, int p_TextureType)
        {
        }

        public void SetDissolveEnable(RenderableTool.RenderGroupType p_RenderGroupType, bool p_Flag)
        {
        }

        public void SetDissolveLerpProgress(RenderableTool.RenderGroupType p_RenderGroupType, float p_Progress01)
        {
        }

        public void SetDissolveEdge(RenderableTool.RenderGroupType p_RenderGroupType, float p_Float)
        {
        }

        public void SetDissolveTexture(RenderableTool.RenderGroupType p_RenderGroupType, int p_TextureType)
        {
        }

        public void SetDissolveEdgeTexture(RenderableTool.RenderGroupType p_RenderGroupType, int p_TextureType)
        {
        }

        public void SetRenderQueue(RenderableTool.RenderGroupType p_RenderGroupType, RenderQueue p_QueueType)
        {
        }

        public void SetRenderQueue(RenderableTool.RenderGroupType p_RenderGroupType, int p_QueueDepth)
        {
        }
        
        #endregion
    }
}
#endif