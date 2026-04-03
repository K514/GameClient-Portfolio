using System.Threading;
using Cysharp.Threading.Tasks;
using k514.Mono.Common;
using TMPro;
using UnityEngine;

namespace k514.Mono.Feature
{
    public class UIxGameConfig : UIxContainerBase
    {
        #region <Consts>

        private const int __LANG_RECORD_INDEX = 20000100;
        private static readonly Color __SELECTED_BTN_COLOR = new Color(0.2f, 0.75f, 0.4f);

        #endregion
        
        #region <Fields>

        private LanguageEventReceiver _LanguageEventReceiver;
        private TextMeshProUGUI _LangColText;
        private UIxButton _EngBtn, _JpnBtn, _KorBtn;
        private TextMeshProUGUI _EngBtnText, _JpnBtnText, _KorBtnText;
        private TextMeshProUGUI _VolColText;
        private UIxSlider _MasterSlider, _BgmSlider, _EffectSlider;
        private TextMeshProUGUI _MasterSliderText, _BgmSliderText, _EffectSliderText;
        private UIxToggle _MasterMuteBtn, _BgmMuteBtn, _EffectMuteBtn;
        private UIxButton _DefaultBtn, _ResetBtn, _BackBtn;
        private TextMeshProUGUI _DefaultBtnText, _ResetBtnText, _BackBtnText;
        
        #endregion

        #region <Callbacks>

        protected override void OnCreate(UIPoolManager.CreateParams p_CreateParams)
        {
            base.OnCreate(p_CreateParams);

            _LanguageEventReceiver = new LanguageEventReceiver(LanguageDataTableQuery.LanguageEventType.LanguageChanged, OnHandleEvent);
            {
                var (valid, affine) = RectTransform.FindRecursive("Lang");
                if (valid)
                {
                    _LangColText = affine.GetComponentInChildren<TextMeshProUGUI>();
                }
                _EngBtn = AddElement<UIxButton>("EngBtn");
                _EngBtnText = _EngBtn.GetTextGroup()[0];
                _EngBtn.SetButtonClickEvent((button, token) => ChangeLanguage(LanguageType.ENG, token));
                _JpnBtn = AddElement<UIxButton>("JpnBtn");
                _JpnBtnText = _JpnBtn.GetTextGroup()[0];
                _JpnBtn.SetButtonClickEvent((button, token) => ChangeLanguage(LanguageType.JPN, token));
                _KorBtn = AddElement<UIxButton>("KorBtn");
                _KorBtnText = _KorBtn.GetTextGroup()[0];
                _KorBtn.SetButtonClickEvent((button, token) => ChangeLanguage(LanguageType.KOR, token));
            }
            {
                var (valid, affine) = RectTransform.FindRecursive("Vol");
                if (valid)
                {
                    _VolColText = affine.GetComponentInChildren<TextMeshProUGUI>();
                }
            }
            {
                var (valid, affine) = RectTransform.FindRecursive("Master");
                if (valid)
                {
                    _MasterSlider = AddElement<UIxSlider>(affine, "Slider");
                    _MasterSlider.SetSliderChangeEvent((slider, token) => ChangeVolume(AudioClipNameTableQuery.TableLabel.None, slider.GetValue(), token));
                    _MasterSliderText = affine.FindRecursive<TextMeshProUGUI>("Text").Item2;
                    _MasterMuteBtn = AddElement<UIxToggle>(affine, "MuteBtn");
                    _MasterMuteBtn.SetButtonClickEvent((button, token) => MuteVolume(button as UIxToggle, AudioClipNameTableQuery.TableLabel.None, token));
                }
            }
            {
                var (valid, affine) = RectTransform.FindRecursive("Bgm");
                if (valid)
                {
                    _BgmSlider = AddElement<UIxSlider>(affine, "Slider");
                    _BgmSlider.SetSliderChangeEvent((slider, token) => ChangeVolume(AudioClipNameTableQuery.TableLabel.BGM, slider.GetValue(), token));
                    _BgmSliderText = affine.FindRecursive<TextMeshProUGUI>("Text").Item2;
                    _BgmMuteBtn = AddElement<UIxToggle>(affine, "MuteBtn");
                    _BgmMuteBtn.SetButtonClickEvent((button, token) => MuteVolume(button as UIxToggle, AudioClipNameTableQuery.TableLabel.BGM, token));
                }
            }
            {
                var (valid, affine) = RectTransform.FindRecursive("Effect");
                if (valid)
                {
                    _EffectSlider = AddElement<UIxSlider>(affine, "Slider");
                    _EffectSlider.SetSliderChangeEvent((slider, token) => ChangeVolume(AudioClipNameTableQuery.TableLabel.Effect, slider.GetValue(), token));
                    _EffectSliderText = affine.FindRecursive<TextMeshProUGUI>("Text").Item2;
                    _EffectMuteBtn = AddElement<UIxToggle>(affine, "MuteBtn");
                    _EffectMuteBtn.SetButtonClickEvent((button, token) => MuteVolume(button as UIxToggle, AudioClipNameTableQuery.TableLabel.Effect, token));
                }
            }
            {
                _DefaultBtn = AddElement<UIxButton>("DefaultBtn");
                _DefaultBtnText = _DefaultBtn.GetTextGroup()[0];
                _DefaultBtn.SetButtonClickEvent(RevertGameConfig);
                _ResetBtn = AddElement<UIxButton>("ResetBtn");
                _ResetBtnText = _ResetBtn.GetTextGroup()[0];
                _ResetBtn.SetButtonClickEvent(ResetGameConfig);
                _BackBtn = AddElement<UIxButton>("BackBtn", new UIxTool.UIInputEventParams(InputEventTool.InputLayerType.ControlUI, KeyCode.Escape));
                _BackBtnText = _BackBtn.GetTextGroup()[0];
            }
        }

        protected override bool OnActivate(UIPoolManager.CreateParams p_CreateParams, UIPoolManager.ActivateParams p_ActivateParams, bool p_IsPooled)
        {
            if (base.OnActivate(p_CreateParams, p_ActivateParams, p_IsPooled))
            {
                _LanguageEventReceiver.SetReceiverBlock(false);
                
                return true;
            }
            else
            {
                return false;
            }
        }

        protected override void OnRetrieve(UIPoolManager.CreateParams p_CreateParams, bool p_IsPooled, bool p_IsDisposed)
        {
            _LanguageEventReceiver.SetReceiverBlock(true);
            
            base.OnRetrieve(p_CreateParams, p_IsPooled, p_IsDisposed);
        }

        protected override void OnDispose()
        {
            if (!ReferenceEquals(null, _LanguageEventReceiver))
            {
                _LanguageEventReceiver.Dispose();
                _LanguageEventReceiver = null;
            }
            
            base.OnDispose();
        }
        
        protected override void OnVisible()
        {
            base.OnVisible();
            
            SetControlBlock(false);
            DefaultGameConfigureManager.GetInstanceUnsafe.BackUpData();
            
            OnUpdateControls();
            OnUpdateText();
        }

        public override void OnControlInput(InputLayerEventParams p_Params)
        {
            var keyCode = p_Params.KeyCode;
            switch (keyCode)
            {
                case KeyCode.Escape:
                case KeyCode.G:
                {
                    if (p_Params.IsTouched)
                    {
                        UIxControlRoot.GetInstanceUnsafe.PopFromControlStack(this);
                    }
                    break;
                }
            }
        }

        private void OnHandleEvent(LanguageDataTableQuery.LanguageEventType p_Type, LanguageEventParams p_Params)
        {
            OnUpdateText();
        }

        private void OnUpdateText()
        {
            var record = UILanguageDataTable.GetInstanceUnsafe[__LANG_RECORD_INDEX];
            _LangColText.text = record.TextList[0];
            _EngBtnText.text = record.TextList[1];
            _JpnBtnText.text = record.TextList[2];
            _KorBtnText.text = record.TextList[3];
            _VolColText.text = record.TextList[4];
            _MasterSliderText.text = record.TextList[5];
            _BgmSliderText.text = record.TextList[6];
            _EffectSliderText.text = record.TextList[7];
            _DefaultBtnText.text = record.TextList[8];
            _ResetBtnText.text = record.TextList[9];
            _BackBtnText.text = record.TextList[10];
        }

        private void OnUpdateControls()
        {
            OnUpdateLanguageButton();
            OnUpdateVolumeSlider();
            OnUpdateVolumeMuteButton();
        }
        
        private void OnUpdateLanguageButton()
        {
            var currentLanguage = SystemBoot.SystemLanguageType;
            switch (currentLanguage)
            {
                case LanguageType.ENG:
                    _EngBtn.SetImageColor(__SELECTED_BTN_COLOR);
                    _JpnBtn.ResetImage();
                    _KorBtn.ResetImage();
                    break;
                case LanguageType.JPN:
                    _EngBtn.ResetImage();
                    _JpnBtn.SetImageColor(__SELECTED_BTN_COLOR);
                    _KorBtn.ResetImage();
                    break;
                case LanguageType.KOR:
                    _EngBtn.ResetImage();
                    _JpnBtn.ResetImage();
                    _KorBtn.SetImageColor(__SELECTED_BTN_COLOR);
                    break;
                default:
                    _EngBtn.ResetImage();
                    _JpnBtn.ResetImage();
                    _KorBtn.ResetImage();
                    break;
            }
        }

        private void OnUpdateVolumeSlider()
        {
            _MasterSlider.SetValue(DefaultGameConfigureManager.GetInstanceUnsafe.DataRecord.MasterVolume);
            _BgmSlider.SetValue(DefaultGameConfigureManager.GetInstanceUnsafe.DataRecord.BgmVolume);
            _EffectSlider.SetValue(DefaultGameConfigureManager.GetInstanceUnsafe.DataRecord.EffectVolume);
        }
        
        private void OnUpdateVolumeMuteButton()
        {
            if (DefaultGameConfigureManager.GetInstanceUnsafe.DataRecord.MasterVolumeMuteFlag)
            {
                _MasterMuteBtn.SetFlag(false);
                _MasterSlider.SetControlBlock(true);
            }
            else
            {
                _MasterMuteBtn.SetFlag(true);
                _MasterSlider.SetControlBlock(false);
            }
            
            if (DefaultGameConfigureManager.GetInstanceUnsafe.DataRecord.BgmVolumeMuteFlag)
            {
                _BgmMuteBtn.SetFlag(false);
                _BgmSlider.SetControlBlock(true);
            }
            else
            {
                _BgmMuteBtn.SetFlag(true);
                _BgmSlider.SetControlBlock(false);
            }
            
            if (DefaultGameConfigureManager.GetInstanceUnsafe.DataRecord.EffectVolumeMuteFlag)
            {
                _EffectMuteBtn.SetFlag(false);
                _EffectSlider.SetControlBlock(true);
            }
            else
            {
                _EffectMuteBtn.SetFlag(true);
                _EffectSlider.SetControlBlock(false);
            }
        }
        
        #endregion

        #region <Methods>
        
        private async UniTask RevertGameConfig(UIxButton p_Button, CancellationToken p_Token)
        {
            SetControlBlock(true);
            await DefaultGameConfigureManager.GetInstanceUnsafe.TurnToDefault(true, p_Token);
            SetControlBlock(false);
            
            OnUpdateControls();
        }
        
        private async UniTask ResetGameConfig(UIxButton p_Button, CancellationToken p_Token)
        {
            SetControlBlock(true);
            await DefaultGameConfigureManager.GetInstanceUnsafe.TurnToBackUp(true, p_Token);
            SetControlBlock(false);
            
            OnUpdateControls();
        }

        #endregion
        
        #region <Method/Lang>
        
        private async UniTask ChangeLanguage(LanguageType p_Type, CancellationToken p_Token)
        {
            SetControlBlock(true);
            DefaultGameConfigureManager.GetInstanceUnsafe.DataRecord.LanguageType = p_Type;
            await DefaultGameConfigureManager.GetInstanceUnsafe.ApplyConfig(true, p_Token);
            SetControlBlock(false);
            
            OnUpdateLanguageButton();
        }

        #endregion

        #region <Method/Vol>

        private async UniTask ChangeVolume(AudioClipNameTableQuery.TableLabel p_Type, float p_Value, CancellationToken p_Token)
        {
            SetControlBlock(true);
            switch (p_Type)
            {
                case AudioClipNameTableQuery.TableLabel.None:
                    DefaultGameConfigureManager.GetInstanceUnsafe.DataRecord.MasterVolume = p_Value;
                    break;
                case AudioClipNameTableQuery.TableLabel.BGM:
                    DefaultGameConfigureManager.GetInstanceUnsafe.DataRecord.BgmVolume = p_Value;
                    break;
                case AudioClipNameTableQuery.TableLabel.Effect:
                    DefaultGameConfigureManager.GetInstanceUnsafe.DataRecord.EffectVolume = p_Value;
                    break;
            }
            await DefaultGameConfigureManager.GetInstanceUnsafe.ApplyConfig(true, p_Token);
            SetControlBlock(false);
        }
        
        private async UniTask MuteVolume(UIxToggle p_Toggle, AudioClipNameTableQuery.TableLabel p_Type, CancellationToken p_Token)
        {
            SetControlBlock(true);

            var toggleFlag = p_Toggle.Flag;
            switch (p_Type)
            {
                case AudioClipNameTableQuery.TableLabel.None:
                {
                    if (toggleFlag)
                    {
                        DefaultGameConfigureManager.GetInstanceUnsafe.DataRecord.MasterVolumeMuteFlag = false;
                        _MasterSlider.SetControlBlock(false);
                    }
                    else
                    {
                        DefaultGameConfigureManager.GetInstanceUnsafe.DataRecord.MasterVolumeMuteFlag = true;
                        _MasterSlider.SetControlBlock(true);
                    }
                    break;
                }
                case AudioClipNameTableQuery.TableLabel.BGM:
                {
                    if (toggleFlag)
                    {
                        DefaultGameConfigureManager.GetInstanceUnsafe.DataRecord.BgmVolumeMuteFlag = false;
                        _BgmSlider.SetControlBlock(false);
                    }
                    else
                    {
                        DefaultGameConfigureManager.GetInstanceUnsafe.DataRecord.BgmVolumeMuteFlag = true;
                        _BgmSlider.SetControlBlock(true);
                    }
                    break;
                }
                case AudioClipNameTableQuery.TableLabel.Effect:
                {
                    if (toggleFlag)
                    {
                        DefaultGameConfigureManager.GetInstanceUnsafe.DataRecord.EffectVolumeMuteFlag = false;
                        _EffectSlider.SetControlBlock(false);
                    }
                    else
                    {
                        DefaultGameConfigureManager.GetInstanceUnsafe.DataRecord.EffectVolumeMuteFlag = true;
                        _EffectSlider.SetControlBlock(true);
                    }
                    break;
                }
            }
            await DefaultGameConfigureManager.GetInstanceUnsafe.ApplyConfig(true, p_Token);
            SetControlBlock(false);
        }

        #endregion
    }
}