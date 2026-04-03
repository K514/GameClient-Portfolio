#if !SERVER_DRIVE

using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace k514.Mono.Common
{
    public class UIxSlider : UIxContainerBase
    {
        #region <Fields>

        private Slider _Slider;
        private Func<UIxSlider, CancellationToken, UniTask> _ChangeEvent;

        #endregion
        
        #region <Callbacks>

        protected override void OnCreate(UIPoolManager.CreateParams p_CreateParams)
        {
            base.OnCreate(p_CreateParams);

            _Slider = GetComponentInParent<Slider>();
            SetStateFlag(UIxTool.UIxStaticStateType.ReleaseInputEventIndependentHover);
        }
        
        protected override void OnRetrieve(UIPoolManager.CreateParams p_CreateParams, bool p_IsPooled, bool p_IsDisposed)
        {
            SetSliderChangeEvent(null);
            
            base.OnRetrieve(p_CreateParams, p_IsPooled, p_IsDisposed);
        }
        
        protected override void OnBlockStateChanged(bool p_Flag)
        {
            base.OnBlockStateChanged(p_Flag);
            
            _Slider.interactable = !p_Flag;
        }
        
        #endregion

        #region <Methods>

        protected override void CastPointerUpEvent(PointerEventData p_EventData)
        {
            TriggerSliderChangeEvent(p_EventData).Forget();
        }
        
        public void SetSliderChangeEvent(Func<UIxSlider, CancellationToken, UniTask> p_Event)
        {
            _ChangeEvent = p_Event;

            if (ReferenceEquals(null, _ChangeEvent))
            {
                _UIDynamicStateFlagMask.RemoveFlag(UIxTool.UIxDynamicStateType.InteractEvent);
            }
            else
            {
                _UIDynamicStateFlagMask.AddFlag(UIxTool.UIxDynamicStateType.InteractEvent);
            }
        }

        public async UniTask TriggerSliderChangeEvent(PointerEventData p_EventData)
        {
            if (!ReferenceEquals(null, _ChangeEvent))
            {
                await _ChangeEvent.Invoke(this, GetCancellationToken());
            }
            
            base.CastPointerUpEvent(p_EventData);
        }
        
        public void SetValue(float p_Rate)
        {
            _Slider.value = p_Rate;
        }
        
        public float GetValue()
        {
            return _Slider.value;
        }

        #endregion
    }
}

#endif