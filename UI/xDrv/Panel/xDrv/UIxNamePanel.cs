#if !SERVER_DRIVE

using k514.Mono.Common;
using UnityEngine;

namespace k514.Mono.Feature
{
    /// <summary>
    /// 문자열을 표시하는 판넬 UI 중에서 개체의 이름에 관련된 이벤트를 수신받고 처리하는 기능을 가지는 판넬 클래스
    /// </summary>
    public class UIxNamePanel : UIxPanelBase
    {
        #region <Callbacks>
        
        protected override void OnActivateEventSender(UIPoolManager.ActivateParams p_ActivateParams)
        {
            base.OnActivateEventSender(p_ActivateParams);

            SetEntityBaseEvent
            (
                GameEntityTool.GameEntityBaseEventType.PositionMoved | GameEntityTool.GameEntityBaseEventType.Dead 
                | GameEntityTool.GameEntityBaseEventType.Enabled | GameEntityTool.GameEntityBaseEventType.Disabled 
                | GameEntityTool.GameEntityBaseEventType.Retrieved
            ); 
            
            SetEntityUIEvent
            (
                GameEntityTool.GameEntityUIEventType.ChangeName | GameEntityTool.GameEntityUIEventType.ChangeNameColor
                | GameEntityTool.GameEntityUIEventType.ChangeNameSymbol
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
                SetMainTextColor(Color.white);
                SetMainText(_EventEntity.GetName());
                SetMainTextSize(25);
                SetPosition(GetPanelPosition());
                SetFadeDuration(0.3f, 0.8f, 0.3f);
                SetMainImageVisible(false);
                
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
                case GameEntityTool.GameEntityBaseEventType.Enabled:
                    if (p_Params.Trigger.IsObjectUIVisible)
                    {
                        LateEventMask.AddFlag(UIxTool.UIxLateEventType.TurnFadeIn);
                    }
                    break;
                case GameEntityTool.GameEntityBaseEventType.Disabled:
                    LateEventMask.AddFlag(UIxTool.UIxLateEventType.TurnFadeOut);
                    break;
                case GameEntityTool.GameEntityBaseEventType.Retrieved:
                    LateEventMask.AddFlag(UIxTool.UIxLateEventType.TurnFadeOut | UIxTool.UIxLateEventType.Retrieve);
                    break;
            }
        }

        protected override void OnEntityUIEventTriggered(GameEntityTool.GameEntityUIEventType p_EventType, GameEntityUIEventParams p_Params)
        {
            switch (p_EventType)
            {
                case GameEntityTool.GameEntityUIEventType.ChangeName:
                    SetLateUpdateText(_EventEntity.GetName());
                    break;
                case GameEntityTool.GameEntityUIEventType.ChangeNameColor:
                    SetLateUpdateColor(p_Params.Color);
                    break;
                case GameEntityTool.GameEntityUIEventType.ChangeNameSymbol:
                    SetLateUpdateImage(p_Params.Index);
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
            var weight = _EventEntity.GetRadius(1.5f);
            return _EventEntity.GetTopPosition() + weight * CameraManager.GetInstanceUnsafe._CameraCeilingVector;
        }

        #endregion
    }
}
#endif