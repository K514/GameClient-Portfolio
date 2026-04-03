using k514.Mono.Common;

namespace k514.Mono.Feature
{
    public class UIxPlayerState : UIxContainerBase
    {
        #region <Fields>

        private PlayerEventReceiver _PlayerEventReceiver;
        private UIxTraceGauge _HpBar;
        private UIxGauge _MpBar;

        #endregion

        #region <Callbacks>

        protected override void OnCreate(UIPoolManager.CreateParams p_CreateParams)
        {
            base.OnCreate(p_CreateParams);

            _PlayerEventReceiver = new PlayerEventReceiver(PlayerTool.PlayerEventType.PlayerChanged, OnHandlePlayerEvent);

            _HpBar = AddElement<UIxTraceGauge>("Hp");
            _MpBar = AddElement<UIxGauge>("Mp");
        }

        protected override void OnActivateEventSender(UIPoolManager.ActivateParams p_ActivateParams)
        {
            base.OnActivateEventSender(p_ActivateParams);

            SetEntityBaseEvent
            (
                GameEntityTool.GameEntityBaseEventType.BattleStatus_Change
            ); 
        }

        protected override void OnDispose()
        {
            if (!ReferenceEquals(null, _PlayerEventReceiver))
            {
                _PlayerEventReceiver.Dispose();
                _PlayerEventReceiver = null;
            }
            
            base.OnDispose();
        }
        
        private void OnHandlePlayerEvent(PlayerTool.PlayerEventType p_Type, PlayerEventParams p_Params)
        {
            switch (p_Type)
            {
                case PlayerTool.PlayerEventType.PlayerChanged:
                    SetEventEntity(p_Params.Player);
                    break;
            }
        }
        
        protected override void OnEntityBaseEventTriggered(GameEntityTool.GameEntityBaseEventType p_EventType, GameEntityBaseEventParams p_Params)
        {
            switch (p_EventType)
            {
                case GameEntityTool.GameEntityBaseEventType.BattleStatus_Change:
                    UpdateEntityStatus(p_Params.BattleStatusType);
                    break;
            }
        }
        
        private void UpdateEntityStatus(BattleStatusTool.BattleStatusType p_Mask)
        {
            if (p_Mask.HasAnyFlagExceptNone(BattleStatusTool.BattleStatusType.HP_Base))
            {
                OnUpdateHpBar();
            }
            if (p_Mask.HasAnyFlagExceptNone(BattleStatusTool.BattleStatusType.MP_Base))
            {
                OnUpdateMpBar();
            }
        }
        
        private void OnUpdateHpBar()
        {
            if (IsEventEntityValid())
            {
                _HpBar.SetValue(_EventEntity.GetCurrentStatusRate(BattleStatusTool.BattleStatusType.HP_Base));
            }
            else
            {
                // _HpBar.SetValue(0f);
            }
        }
        
        private void OnUpdateMpBar()
        {
            if (IsEventEntityValid())
            {
                _MpBar.SetValue(_EventEntity.GetCurrentStatusRate(BattleStatusTool.BattleStatusType.MP_Base));
            }
            else
            {
                // _MpBar.SetValue(0f);
            }
        }

        private void OnUpdatePlayer()
        {
            OnUpdateHpBar();
            OnUpdateMpBar();
        }
        
        #endregion
        
        #region <Methods>

        protected override void SetEventEntity(IGameEntityBridge p_Entity)
        {
            base.SetEventEntity(p_Entity);

            OnUpdatePlayer();
        }

        #endregion
    }
}