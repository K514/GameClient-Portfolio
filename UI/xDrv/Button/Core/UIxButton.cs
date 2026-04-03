#if !SERVER_DRIVE

using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace k514.Mono.Common
{
    public class UIxButton : UIxElementBase
    {
        #region <Fields>

        protected Button _Button;
        protected Func<UIxButton, CancellationToken, UniTask> _ClickEvent;

        #endregion

        #region <Callbacks>

        protected override void OnCreate(UIPoolManager.CreateParams p_CreateParams)
        {
            base.OnCreate(p_CreateParams);
            
            _Button = GetComponentInChildren<Button>();
        }

        protected override void OnRetrieve(UIPoolManager.CreateParams p_CreateParams, bool p_IsPooled, bool p_IsDisposed)
        {
            SetButtonClickEvent(null);
            
            base.OnRetrieve(p_CreateParams, p_IsPooled, p_IsDisposed);
        }
               
        protected override void OnBlockStateChanged(bool p_Flag)
        {
            base.OnBlockStateChanged(p_Flag);
            
            _Button.interactable = !p_Flag;
        }

        #endregion
        
        #region <Methods>

        protected override void CastPointerUpEvent(PointerEventData p_EventData)
        {
            TriggerButtonClickEvent(p_EventData).Forget();
        }

        public void SetButtonClickEvent(Func<UIxButton, CancellationToken, UniTask> p_Event)
        {
            _ClickEvent = p_Event;

            if (ReferenceEquals(null, _ClickEvent))
            {
                _UIDynamicStateFlagMask.RemoveFlag(UIxTool.UIxDynamicStateType.InteractEvent);
            }
            else
            {
                _UIDynamicStateFlagMask.AddFlag(UIxTool.UIxDynamicStateType.InteractEvent);
            }
        }

        private async UniTask TriggerButtonClickEvent(PointerEventData p_EventData)
        {
            if (!ReferenceEquals(null, _ClickEvent))
            {
                await _ClickEvent.Invoke(this, GetCancellationToken());
            }
            
            base.CastPointerUpEvent(p_EventData);
        }

        #endregion
    }
}

#endif