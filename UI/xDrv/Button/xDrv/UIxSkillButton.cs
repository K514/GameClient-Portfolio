#if !SERVER_DRIVE

using System;
using k514.Mono.Common;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace k514.Mono.Feature
{
    public class UIxSkillButton : UIxButton
    {
        #region <Consts>

        private const float _LockedAlpha = 0.9f;
        private const float _CooldownOverAnimationDuration = 0.5f;

        #endregion
        
        #region <Fields>

        private IActionEventHandler _Handler;
        private HandlerChangeDirtyFlag _HandlerChangeDirtyFlag;
        private Image _SkillIcon;
        private TextMeshProUGUI _CommandText;
        private Image _CooldownFill;
        private TextMeshProUGUI _CooldownText;
        private Image _Lock;
        private Image _CooldownOver;
        private AssetLoadResult<Sprite> _IconSpritePreset;
        private LockPhase _LockPhase;
        private CooldownPhase _CooldownPhase;
        private ProgressTimer _CooldownOverAnimationTimer;
        
        #endregion

        #region <Enums>

        private enum HandlerChangeDirtyFlag
        {
            Invalid,
            Valid,
            Dirty,
        }
        
        private enum LockPhase
        {
            None,
            Free,
            Locked,
        }
        
        private enum CooldownPhase
        {
            None,
            CooldownFree,
            CooldownProgress,
            CooldownOverAnimation,
        }

        #endregion
        
        #region <Callbacks>

        protected override void OnCreate(UIPoolManager.CreateParams p_CreateParams)
        {
            base.OnCreate(p_CreateParams);

            _SkillIcon = _Button.transform.FindRecursive<Image>("Icon").Item2;
            _CommandText = _Button.transform.FindRecursive<TextMeshProUGUI>("CommandText").Item2;
            _CooldownFill = _Button.transform.FindRecursive<Image>("CooldownFill").Item2;
            _CooldownText = _Button.transform.FindRecursive<TextMeshProUGUI>("CooldownText").Item2;
            _Lock = _Button.transform.FindRecursive<Image>("Lock").Item2;
            _CooldownOver = _Button.transform.FindRecursive<Image>("CooldownOver").Item2;

            _CooldownFill.type = Image.Type.Filled;
            _CooldownFill.fillMethod = Image.FillMethod.Vertical;
            _CooldownFill.fillOrigin = 1;
            _CooldownOverAnimationTimer = _CooldownOverAnimationDuration;
            
            ResetKeyCode();
            ResetCooldownUI();
        }

        protected override void OnRetrieve(UIPoolManager.CreateParams p_CreateParams, bool p_IsPooled, bool p_IsDisposed)
        {
            base.OnRetrieve(p_CreateParams, p_IsPooled, p_IsDisposed);
            
            ResetActionInfo();
        }

        protected override void OnUpdateUIDeferredEvent(float p_DeltaTime)
        {
            base.OnUpdateUIDeferredEvent(p_DeltaTime);

            switch (_HandlerChangeDirtyFlag)
            {
                case HandlerChangeDirtyFlag.Dirty:
                {
                    if (ReferenceEquals(null, _Handler))
                    {
                        ResetActionInfo();
                    }
                    else
                    {
                        _HandlerChangeDirtyFlag = HandlerChangeDirtyFlag.Valid;
                        
                        LoadIcon();
                        SetKeyCode(_Handler.TriggerKey.ConvertToKeyCode());
                    }
                    break;
                }
                case HandlerChangeDirtyFlag.Valid:
                {
                    switch (_LockPhase)
                    {
                        case LockPhase.None:
                        {
                            var enterable = _Handler.IsEnterableExceptCooldown();
                            if (enterable)
                            {
                                _Lock.SetImageAlpha(0f);
                                _LockPhase = LockPhase.Free;
                            }
                            else
                            {
                                _Lock.SetImageAlpha(_LockedAlpha);
                                _LockPhase = LockPhase.Locked;
                            }
                            break;
                        }
                        case LockPhase.Free:
                        {
                            var enterable = _Handler.IsEnterableExceptCooldown();
                            if (!enterable)
                            {
                                _Lock.SetImageAlpha(_LockedAlpha);
                                _LockPhase = LockPhase.Locked;
                            }
                            break;
                        }
                        case LockPhase.Locked:
                        {
                            var enterable = _Handler.IsEnterableExceptCooldown();
                            if (enterable)
                            {
                                _Lock.SetImageAlpha(0f);
                                _LockPhase = LockPhase.Free;
                            }
                            break;
                        }
                    }
                    
                    switch (_CooldownPhase)
                    {
                        case CooldownPhase.None:
                        {
                            if (_Handler.IsCooldown())
                            {
                                _CooldownPhase = CooldownPhase.CooldownProgress;
                                _CooldownFill.fillAmount = 1f - _Handler.GetCooldownProgressRate();
                                _CooldownText.text = $"{_Handler.GetCooldownRemained():N1}";
                            }
                            else
                            {
                                if (_LockPhase != LockPhase.Locked)
                                {
                                    SetCooldownOverAnimation();
                                }
                                else
                                {
                                    _CooldownPhase = CooldownPhase.CooldownFree;
                                    ResetCooldownUI();
                                }
                            }
                            break;
                        }
                        case CooldownPhase.CooldownFree:
                        {
                            if (_Handler.IsCooldown())
                            {
                                _CooldownPhase = CooldownPhase.CooldownProgress;
                                _CooldownFill.fillAmount = 1f - _Handler.GetCooldownProgressRate();
                                _CooldownText.text = $"{_Handler.GetCooldownRemained():N1}";
                            }
                            break;
                        }
                        case CooldownPhase.CooldownProgress:
                        {
                            if (_Handler.IsCooldown())
                            {
                                _CooldownFill.fillAmount = 1f - _Handler.GetCooldownProgressRate();
                                _CooldownText.text = $"{_Handler.GetCooldownRemained():N1}";
                            }
                            else
                            {
                                if (_LockPhase != LockPhase.Locked)
                                {
                                    SetCooldownOverAnimation();
                                }
                                else
                                {
                                    _CooldownPhase = CooldownPhase.CooldownFree;
                                    ResetCooldownUI();
                                }
                            }
                            break;
                        }
                        case CooldownPhase.CooldownOverAnimation:
                        {
                            if (_CooldownOverAnimationTimer.IsOver())
                            {
                                _CooldownPhase = CooldownPhase.CooldownFree;
                                ResetCooldownOverAnimation();
                            }
                            else
                            {
                                _CooldownOverAnimationTimer.Progress(p_DeltaTime);
                                _CooldownOver.SetImageAlpha(1f - _CooldownOverAnimationTimer.ProgressRate);
                            }
                            break;
                        }
                    }
                    break;
                }
            }
        }
        
        #endregion
        
        #region <Methods>

        public void SetActionInfo(IActionEventHandler p_Handler)
        {
            if (!ReferenceEquals(p_Handler, _Handler))
            {
                _HandlerChangeDirtyFlag = HandlerChangeDirtyFlag.Dirty;
                _Handler = p_Handler;
            }
        }
        
        public void ResetActionInfo()
        {
            _HandlerChangeDirtyFlag = HandlerChangeDirtyFlag.Invalid;
            _Handler = null;
                        
            ReleaseIcon();
            ResetKeyCode();
            ResetCooldownUI();
            ResetCooldownOverAnimation();
        }

        private void SetKeyCode(KeyCode p_KeyCode)
        {
            _CommandText.text = p_KeyCode.ConvertToKeyName();
            SetInputEvent(new UIxTool.UIInputEventParams(InputEventTool.InputLayerType.ControlUnit, p_KeyCode));
        }

        private void ResetKeyCode()
        {
            _CommandText.text = string.Empty;
            RemoveInputEvent();
        }

        private void ResetCooldownUI()
        {
            _CooldownFill.fillAmount = 0f;
            _CooldownText.text = string.Empty;
        }

        private void SetCooldownOverAnimation()
        {
            _CooldownPhase = CooldownPhase.CooldownOverAnimation;
            ResetCooldownUI();
            _CooldownOverAnimationTimer.Reset();
        }
        
        private void ResetCooldownOverAnimation()
        {
            _CooldownOver.SetImageAlpha(0f);
        }

        private void LoadIcon()
        {
        }
        
        private void ReleaseIcon()
        {
            if (_IconSpritePreset.ValidFlag)
            {
                AssetLoaderManager.GetInstanceUnsafe.UnloadAsset(ref _IconSpritePreset);
            }
        }

        #endregion
    }
}

#endif