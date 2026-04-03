#if !SERVER_DRIVE

using System.Collections.Generic;
using k514.Mono.Common;
using UnityEngine;

namespace k514.Mono.Feature
{
    public class UIxController : UIxControllerBase<UIxIndicatorJoystick>
    {
        #region <Fields>
        
        private PlayerEventReceiver _PlayerEventReceiver;
        private Dictionary<InputEventTool.TriggerKeyType, UIxSkillButton> _SkillButtonTable;

        #endregion
        
        #region <Callbacks>

        protected override void OnCreate(UIPoolManager.CreateParams p_CreateParams)
        {
            base.OnCreate(p_CreateParams);
  
            _PlayerEventReceiver = new PlayerEventReceiver(PlayerTool.PlayerEventType.PlayerChanged, OnHandlePlayerEvent);
            _SkillButtonTable = new Dictionary<InputEventTool.TriggerKeyType, UIxSkillButton>();

            _SkillButtonTable.Add(ActionTool.__DEFAULT_SKILL_TRIGGER_KEYSET[0], AddElement<UIxSkillButton>("A"));
            _SkillButtonTable.Add(ActionTool.__DEFAULT_SKILL_TRIGGER_KEYSET[1], AddElement<UIxSkillButton>("S"));
            _SkillButtonTable.Add(ActionTool.__DEFAULT_SKILL_TRIGGER_KEYSET[2], AddElement<UIxSkillButton>("D"));
            _SkillButtonTable.Add(ActionTool.__DEFAULT_SKILL_TRIGGER_KEYSET[3], AddElement<UIxSkillButton>("F"));
            _SkillButtonTable.Add(ActionTool.__DASH_TRIGGER, AddElement<UIxSkillButton>("LC"));
            _SkillButtonTable.Add(ActionTool.__SEQ_COMMAND_TRIGGER, AddElement<UIxSkillButton>("Z"));
            _SkillButtonTable.Add(ActionTool.__SEQ_COMMAND_TRIGGER2, AddElement<UIxSkillButton>("X"));
            _SkillButtonTable.Add(ActionTool.__GUARD_TRIGGER, AddElement<UIxSkillButton>("V"));
        }

        protected override void OnActivateEventSender(UIPoolManager.ActivateParams p_ActivateParams)
        {
            base.OnActivateEventSender(p_ActivateParams);

            SetEntityBaseEvent(GameEntityTool.GameEntityBaseEventType.Skill_Change); 
        }

        protected override void OnVisible()
        {
            base.OnVisible();
            
            SetEventEntity(PlayerManager.GetInstanceUnsafe?.Player);
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

        protected override void OnEntityBaseEventTriggered(GameEntityTool.GameEntityBaseEventType p_EventType, GameEntityBaseEventParams p_Params)
        {
            switch (p_EventType)
            {
                case GameEntityTool.GameEntityBaseEventType.Skill_Change:
                    OnUpdateSkillList();
                    break;
            }
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

        private void OnUpdateSkillList()
        {
            if (ReferenceEquals(null, _EventEntity))
            {
                foreach (var buttonKV in _SkillButtonTable)
                {
                    var button = buttonKV.Value;
                    button.ResetActionInfo();
                }
            }
            else
            {
                var actionModule = _EventEntity.ActionModule;
                var quickCommandTable = actionModule.GetQuickCommandHandlerTable();
                
                foreach (var buttonKV in _SkillButtonTable)
                {
                    var command = buttonKV.Key;
                    var button = buttonKV.Value;
                   
                    switch (command)
                    {
                        case InputEventTool.TriggerKeyType.A:
                        case InputEventTool.TriggerKeyType.S:
                        case InputEventTool.TriggerKeyType.D:
                        case InputEventTool.TriggerKeyType.F:
                        case InputEventTool.TriggerKeyType.LeftControl:
                        case InputEventTool.TriggerKeyType.Z:
                        case InputEventTool.TriggerKeyType.X:
                        case InputEventTool.TriggerKeyType.V:
                        {
                            if (quickCommandTable.TryGetValue(command, out var o_Handler))
                            {
                                button.SetActionInfo(o_Handler);
                            }
                            else
                            {
                                button.ResetActionInfo();
                            }
                            break;
                        }
                    }
                }
            }
        }
        
        #endregion

        #region <Methods>

        protected override void SetEventEntity(IGameEntityBridge p_Entity)
        {
            base.SetEventEntity(p_Entity);
            
            OnUpdateSkillList();
        }

        #endregion
    }
}

#endif