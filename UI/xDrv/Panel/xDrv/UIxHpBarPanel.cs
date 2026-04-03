#if !SERVER_DRIVE

using k514.Mono.Common;
using UnityEngine;

namespace k514.Mono.Feature
{
    /// <summary>
    /// 문자열을 표시하는 판넬 UI 중에서 개체의 체력에 관련된 이벤트를 수신받고 처리하는 기능을 가지는 판넬 클래스
    /// </summary>
    public class UIxHpBarPanel : UIxPanelBase
    {
        #region <Fields>

        private float _CurrentTargetHpBarRate;

        #endregion
        
        #region <Callbacks>
        
        protected override void OnActivateEventSender(UIPoolManager.ActivateParams p_ActivateParams)
        {
            base.OnActivateEventSender(p_ActivateParams);

            SetEntityBaseEvent
            (
                GameEntityTool.GameEntityBaseEventType.PositionMoved | GameEntityTool.GameEntityBaseEventType.Dead 
                | GameEntityTool.GameEntityBaseEventType.Enabled | GameEntityTool.GameEntityBaseEventType.Disabled 
                | GameEntityTool.GameEntityBaseEventType.Retrieved | GameEntityTool.GameEntityBaseEventType.BattleStatus_Change
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
                SetHpBar(1f);
                
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
                case GameEntityTool.GameEntityBaseEventType.BattleStatus_Change:
                    _CurrentTargetHpBarRate = Mathf.Clamp01(p_Params.Trigger.GetCurrentStatusRate(BattleStatusTool.BattleStatusType.HP_Base));
                    SetLateUpdateRate();
                    break;
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
            return _EventEntity.GetBottomPosition() - weight * CameraManager.GetInstanceUnsafe._CameraCeilingVector;
        }

        protected void SetHpBar(float p_Rate)
        {
            _CurrentTargetHpBarRate = p_Rate;
            
            var mainImageScale = _MainImage.transform.localScale;
            _MainImage.transform.localScale = _CurrentTargetHpBarRate * Vector3.right + mainImageScale.YZVector();
        }

        protected override bool UpdateRate(float p_DeltaTime)
        {
            var mainImageScale = _MainImage.transform.localScale;
            var mainImageScaleX = Mathf.MoveTowards(mainImageScale.x, _CurrentTargetHpBarRate, p_DeltaTime);
            _MainImage.transform.localScale = mainImageScaleX * Vector3.right + mainImageScale.YZVector();

            return mainImageScaleX.IsReachedValue(_CurrentTargetHpBarRate);
        }
        
        #endregion
    }
}
#endif