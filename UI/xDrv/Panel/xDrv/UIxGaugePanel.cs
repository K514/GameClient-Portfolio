#if !SERVER_DRIVE

using k514.Mono.Common;
using UnityEngine;

namespace k514.Mono.Feature
{
    /// <summary>
    /// ProgressBar를 표시하는 판넬
    /// </summary>
    public class UIxGaugePanel : UIxPanelBase
    {
        #region <Callbacks>
        
        protected override void OnActivateEventSender(UIPoolManager.ActivateParams p_ActivateParams)
        {
            base.OnActivateEventSender(p_ActivateParams);

            SetEntityBaseEvent
            (
                GameEntityTool.GameEntityBaseEventType.PositionMoved | GameEntityTool.GameEntityBaseEventType.Dead 
                | GameEntityTool.GameEntityBaseEventType.Disabled | GameEntityTool.GameEntityBaseEventType.Retrieved
            ); 

            SetEntityRenderEvent
            (
                GameEntityTool.GameEntityRenderType.UI
            );
        }
        
        protected override bool OnActivate(UIPoolManager.CreateParams p_CreateParams, UIPoolManager.ActivateParams p_ActivateParams, bool p_IsPooled)
        {
            if (base.OnActivate(p_CreateParams, p_ActivateParams, p_IsPooled))
            {
                SetPosition(GetPanelPosition());
                SetFadeDuration(0.3f, 0.8f, 0.3f);
                SetGaugeBar(0f);
                
                return true;
            }
            else
            {
                return false;
            }
        }

        protected override void OnEntityBaseEventTriggered(GameEntityTool.GameEntityBaseEventType p_EventType, GameEntityBaseEventParams p_Params)
        {
            switch (p_EventType)
            {
                case GameEntityTool.GameEntityBaseEventType.PositionMoved:
                    SetLateUpdatePosition(GetPanelPosition());
                    break;
                case GameEntityTool.GameEntityBaseEventType.Dead:
                    LateEventMask.AddFlag(UIxTool.UIxLateEventType.TurnFadeOut | UIxTool.UIxLateEventType.Retrieve);
                    break;
                case GameEntityTool.GameEntityBaseEventType.Disabled:
                    LateEventMask.AddFlag(UIxTool.UIxLateEventType.TurnFadeOut | UIxTool.UIxLateEventType.Retrieve);
                    break;
                case GameEntityTool.GameEntityBaseEventType.Retrieved:
                    LateEventMask.AddFlag(UIxTool.UIxLateEventType.TurnFadeOut | UIxTool.UIxLateEventType.Retrieve);
                    break;
            }
        }
        
        protected override void OnEntityRenderEventTriggered(GameEntityTool.GameEntityRenderType p_EventType, GameEntityRenderEventParams p_Params)
        {
            switch (p_Params.ValidStateType)
            {
                case ValidStateType.Added:
                    LateEventMask.AddFlag(UIxTool.UIxLateEventType.TurnFadeIn | UIxTool.UIxLateEventType.TurnVisible);
                    SetLateUpdatePosition(GetPanelPosition());
                    break;
                case ValidStateType.KeepAdd:
                    SetLateUpdatePosition(GetPanelPosition());
                    break;
                case ValidStateType.Removed:
                    LateEventMask.AddFlag(UIxTool.UIxLateEventType.TurnHide);
                    break;
            }
        }

        #endregion

        #region <Methods>

        private Vector3 GetPanelPosition()
        {
            var weight = _EventEntity.GetRadius(1.75f);
            return _EventEntity.GetTopPosition() + weight * CameraManager.GetInstanceUnsafe._CameraCeilingVector;
        }

        public void SetGaugeBar(float p_Rate)
        {
            _MainImage.fillAmount = p_Rate;
        }
        
        #endregion
    }
}
#endif