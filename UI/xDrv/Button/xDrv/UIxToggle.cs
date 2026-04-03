#if !SERVER_DRIVE

using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace k514.Mono.Common
{
    public class UIxToggle : UIxButton
    {
        #region <Fields>

        public bool Flag { get; private set; }
        private Image _ActivateImage, _DeactivateImage;
        private Func<CancellationToken, UniTask> _ActivateEvent, _DeativateEvent;
        
        #endregion
        
        #region <Callbacks>

        protected override void OnCreate(UIPoolManager.CreateParams p_CreateParams)
        {
            base.OnCreate(p_CreateParams);

            _ActivateImage = GetImageGroup()[0];
            _DeactivateImage = GetImageGroup()[1];
        }

        protected override bool OnActivate(UIPoolManager.CreateParams p_CreateParams, UIPoolManager.ActivateParams p_ActivateParams, bool p_IsPooled)
        {
            if (base.OnActivate(p_CreateParams, p_ActivateParams, p_IsPooled))
            {
                SetFlag(true);

                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion
        
        #region <Methods>

        protected override void CastPointerUpEvent(PointerEventData p_EventData)
        {
            SetFlag(!Flag);

            base.CastPointerUpEvent(p_EventData);
        }

        public void SetFlag(bool p_Flag)
        {
            if (Flag != p_Flag)
            {
                Flag = p_Flag;

                if (Flag)
                {
                    _Button.targetGraphic = _ActivateImage;
                    _ActivateImage.enabled = true;
                    _DeactivateImage.enabled = false;
                }
                else
                {
                    _Button.targetGraphic = _DeactivateImage;
                    _ActivateImage.enabled = false;
                    _DeactivateImage.enabled = true;
                }
            }
        }
        
        #endregion
    }
}

#endif