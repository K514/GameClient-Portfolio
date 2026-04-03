#if !SERVER_DRIVE

using System.Collections.Generic;

namespace k514.Mono.Common
{
    public partial class RenderBase
    {
        #region <Consts>

        /// <summary>
        /// 랜더러 연출 기본 지속시간 단위 50ms
        /// </summary>
        private const float _RenderTimerInterval = 0.05f;
        
        /// <summary>
        /// 발광 연출 셰이더 밝기 배율
        /// </summary>
        private const float _Default_Eradication_Intensity = 1.5f;
        
        #endregion
        
        #region <Fields>
        
        /// <summary>
        /// 현재 적용중인 셰이더 연출 타입
        /// </summary>
        public RenderableTool.ShaderControlType ShaderTypeMask { get; private set; }
        
        /// <summary>
        /// 현재 셰이더 연출 중인 셰이더 컨트롤 타입별 스택
        /// </summary>
        private Dictionary<RenderableTool.ShaderControlType, CountDownIterator<RenderableTool.ShaderControlType>> _RenderTimer;

        #endregion

        #region <Callbacks>

        private void OnInitializeStackTimer()
        {
            ShaderTypeMask = _RenderModuleRecord.ShaderTypeMask;
            _RenderTimer = new Dictionary<RenderableTool.ShaderControlType, CountDownIterator<RenderableTool.ShaderControlType>>();

            foreach (var shaderControlType in RenderableTool.ShaderControlTypeEnumerator)
            {
                if (ShaderTypeMask.HasAnyFlagExceptNone(shaderControlType))
                {
                    _RenderTimer.Add(shaderControlType, new CountDownIterator<RenderableTool.ShaderControlType>(_RenderTimerInterval, shaderControlType, ClearRenderEffect, null));
                }
            }
        }

        #endregion

        #region <Methods>
        
        private bool HasRenderEffectStack(RenderableTool.ShaderControlType p_ShaderControlType)
        {
            return ShaderTypeMask.HasAnyFlagExceptNone(p_ShaderControlType) && _RenderTimer[p_ShaderControlType].ValidFlag;
        }

        public void SetRenderEffect(RenderableTool.ShaderControlType p_ShaderControlType, int p_UpdateCount)
        {
            if (p_UpdateCount > 0 && ShaderTypeMask.HasAnyFlagExceptNone(p_ShaderControlType))
            {
                switch (p_ShaderControlType)
                {
                    case RenderableTool.ShaderControlType.Outline:
                        SetOutlineEnable(RenderableTool.RenderGroupType.Whole, true);
                        break;
                    case RenderableTool.ShaderControlType.Silhouette:
                        SetSilhouetteEnable(RenderableTool.RenderGroupType.Whole, true);
                        break;
                    case RenderableTool.ShaderControlType.Intensity:
                        SetIntensityEnable(RenderableTool.RenderGroupType.Whole, true);
                        SetIntensityValue(RenderableTool.RenderGroupType.Whole, _Default_Eradication_Intensity);
                        break;
                    case RenderableTool.ShaderControlType.Dissolve:
                        SetDissolveEnable(RenderableTool.RenderGroupType.Whole, true);
                        break;
                }
                _RenderTimer[p_ShaderControlType].UpdateCount(p_UpdateCount);
            }
        }

        private void ClearRenderEffect(CountDownIterator<RenderableTool.ShaderControlType> p_StackTimer, RenderableTool.ShaderControlType p_ShaderControlType)
        {
            if (HasRenderEffectStack(p_ShaderControlType))
            {
                switch (p_ShaderControlType)
                {
                    case RenderableTool.ShaderControlType.Outline:
                        SetOutlineEnable(RenderableTool.RenderGroupType.Whole, false);
                        break;
                    case RenderableTool.ShaderControlType.Silhouette:
                        SetSilhouetteEnable(RenderableTool.RenderGroupType.Whole, false);
                        break;
                    case RenderableTool.ShaderControlType.Intensity:
                        SetIntensityEnable(RenderableTool.RenderGroupType.Whole, false);
                        break;
                    case RenderableTool.ShaderControlType.Dissolve:
                        SetDissolveEnable(RenderableTool.RenderGroupType.Whole, false);
                        break;
                }
            }
        }
        
        private void ClearRenderEffect(RenderableTool.ShaderControlType p_ShaderControlType)
        {
            if (HasRenderEffectStack(p_ShaderControlType))
            {
                _RenderTimer[p_ShaderControlType].CancelCountDown();
            }
        }
        
        private void ResetRenderEffect()
        {
            foreach (var shaderControlTypeKV in _RenderTimer)
            {
                shaderControlTypeKV.Value.CancelCountDown();
            }

            SetSilhouetteEnable(RenderableTool.RenderGroupType.Whole, false);
            SetIntensityEnable(RenderableTool.RenderGroupType.Whole, false);
            SetOutlineEnable(RenderableTool.RenderGroupType.Whole, false);
            SetDissolveEnable(RenderableTool.RenderGroupType.Whole, false);
            SetRendererShadow(RenderableTool.RenderGroupType.Whole, true);
        }

        #endregion
    }
}
#endif