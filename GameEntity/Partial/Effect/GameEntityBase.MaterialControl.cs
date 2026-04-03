using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace k514.Mono.Common
{
    public partial class GameEntityBase<Content, CreateParams, ActivateParams>
    {
        #region <Consts>

        private const float _FlashIntensity = 8f;
        private static readonly Color _CombatFlashColor = new Color(_FlashIntensity, _FlashIntensity, _FlashIntensity);
        private static readonly Color _HealHPFlashColor = new Color(0.2f, 1f, 0f);
        private static readonly Color _HealMPFlashColor = new Color(0.5f, 0.7f, 1f);
        private static readonly Color _ShockFlashColor = new Color(0.7f, 1f, 0f);
        private static readonly Color _BleedFlashColor = new Color(1f, 0f, 0f);
        private static readonly Color _PoisonFlashColor = new Color(0.5f, 0f, 1f);
        private static readonly Color _BurnFlashColor = new Color(0.8f, 0.25f, 0.15f);
        private static readonly Color _ChillFlashColor = new Color(0.25f, 0.25f, 1f);
        
        #endregion
        
        #region <Fields>

        private MeshRenderer[] _RendererSet;
        private Light[] _LightSet;
        private RenderableTool.SkinnedMeshRendererPreset[] _SkinnedRendererPresetGroup;
        private ProgressTimer _FlashDuration;
        private float _TargetAlpha, _CurrentAlpha;
        private bool _ProgressFlag, _DeadFadeOutFlag;

        #endregion
        
        #region <Callbacks>

        private void OnCreateMaterialControl()
        {
            _RendererSet = GetComponentsInChildren<MeshRenderer>();
            _LightSet = GetComponentsInChildren<Light>();
            
            var skinnedMeshRendererSet = GetComponentsInChildren<SkinnedMeshRenderer>();
            if (skinnedMeshRendererSet.CheckCollectionSafe())
            {
                _SkinnedRendererPresetGroup = skinnedMeshRendererSet.Select(renderer => new RenderableTool.SkinnedMeshRendererPreset(renderer)).ToArray();
            }
            else
            {
                _SkinnedRendererPresetGroup = Array.Empty<RenderableTool.SkinnedMeshRendererPreset>();
            }
        }

        private void OnActivateMaterialControl(ActivateParams p_ActivateParams)
        {
            _TargetAlpha = 1f;
            _CurrentAlpha = 1f;
        }
        
        private void OnRetrieveMaterialControl()
        {
            _ProgressFlag = false;
            _DeadFadeOutFlag = false;
            _FlashDuration.Reset();
            
            ResetMaterialColor();
            SetMaterialOpacity(1f);
            SetRenderEnable(true);
        }

        private void OnUpdateMaterialControl(float p_DeltaTime)
        {
            if (_ProgressFlag)
            {
                if (_FlashDuration.IsOver())
                {
                    if (_DeadFadeOutFlag)
                    {
                        OnDeadFadeOutOver();
                    }
                    else
                    {
                        OnMaterialControlOver();
                    }
                }
                else
                {
                    _FlashDuration.Progress(p_DeltaTime);

                    _CurrentAlpha = Mathf.Lerp(_CurrentAlpha, _TargetAlpha, _FlashDuration.ProgressRate);
                    SetMaterialOpacity(_CurrentAlpha);
                }
            }
        }

        private void OnDeadFadeOutOver()
        {
            SetLifeSpanPhase(GameEntityTool.EntityLifeSpanPhase.LifeSpanTerminate);
            SetRenderEnable(false);
            
            _DeadFadeOutFlag = false;
            _ProgressFlag = false;
        }
        
        private void OnMaterialControlOver()
        {
            ResetMaterialColor();
            SetMaterialOpacity(_TargetAlpha);
            
            _ProgressFlag = false;
        }
        
        #endregion

        #region <Methods>

        private void TriggerDeadFadeOut()
        {
            SetAlphaLerp(0f, 2f);
            _DeadFadeOutFlag = true;
        }
        
        public void SetMaterialFlash()
        {
            SetAlphaLerp(1f, 0.1f);
        }

        public void SetAlphaLerp(float p_TargetAlpha, float p_LerpDuration)
        {
            if (!_DeadFadeOutFlag)
            {
                _TargetAlpha = p_TargetAlpha;
                _FlashDuration = p_LerpDuration;
                _ProgressFlag = true;
            }
        }

        private void SetMaterialColor(Color p_Color)
        {
            foreach (var rendererPreset in _SkinnedRendererPresetGroup)
            {
                rendererPreset.SetColor(p_Color);
            }
        }
        
        private void ResetMaterialColor()
        {
            foreach (var rendererPreset in _SkinnedRendererPresetGroup)
            {
                rendererPreset.ResetColor();
            }
        }

        private void SetMaterialOpacity(float p_Value)
        {
            foreach (var rendererPreset in _SkinnedRendererPresetGroup)
            {
                rendererPreset.SetOpacity(p_Value);
            }
        }

        public void SetRenderEnable(bool p_Flag)
        {
            if (_RendererSet.CheckCollectionSafe())
            {
                foreach (var renderer in _RendererSet)
                {
                    renderer.enabled = p_Flag;
                }
            }
                
            if (_LightSet.CheckCollectionSafe())
            {
                foreach (var light in _LightSet)
                {
                    light.enabled = p_Flag;
                }
            }
        }

        #endregion
    }
}