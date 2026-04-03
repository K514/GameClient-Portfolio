using UnityEngine;

namespace k514.Mono.Common
{
    public partial class GameEntityBase<Content, CreateParams, ActivateParams>
    {
        #region <Fields>

        private GameEntityTool.LateEventType _LateEventFlagMask;
        
        #endregion

        #region <Callbacks>
        
        protected override void OnCreateUpdate()
        {
        }
        
        protected override void OnActivateUpdate()
        {
            _LateEventFlagMask = GameEntityTool.LateEventType.UpdateCameraInteract;
        }

        protected override void OnRetrieveUpdate()
        {
            _LateEventFlagMask = GameEntityTool.LateEventType.None;
        }

        public void OnPreUpdate(float p_DeltaTime)
        {
            if (IsFunctional)
            {
                OnModule_PreUpdate(p_DeltaTime);
            }
        }

        /// <summary>
        /// 매 프레임당 호출되는 콜백
        /// </summary>
        public override void OnUpdate(float p_DeltaTime)
        {
            if (IsFunctional)
            {
                OnModule_Update(p_DeltaTime);
                OnUpdateBattleStatusRegenerate(p_DeltaTime);
                base.OnUpdate(p_DeltaTime);
                OnUpdateEventHandler(p_DeltaTime);
                OnUpdateInventory(p_DeltaTime);
                OnUpdateMaterialControl(p_DeltaTime);
                OnUpdateEntity(p_DeltaTime);
            }
        }

        protected virtual void OnUpdateEntity(float p_DeltaTime)
        {
        }

        /// <summary>
        /// UnitInteractManager 로부터 일정 주기(0.1초)로 호출되는 콜백
        /// </summary>
        public void OnUpdate_TimeBlock()
        {
            if (IsFunctional)
            {
                OnModule_Update_TimeBlock();
                OnUpdateEntity_TimeBlock();
            }
        }
        
        protected virtual void OnUpdateEntity_TimeBlock()
        {
        }

        public override void OnLateUpdate(float p_DeltaTime)
        {
            var nextFrameFlagMask = GameEntityTool.LateEventType.None;
            foreach (var eventType in GameEntityTool.LateEventTypeEnumeraotr)
            {
                if (_LateEventFlagMask.HasAnyFlagExceptNone(eventType))
                {
                    switch (eventType)
                    {
                        case GameEntityTool.LateEventType.UpdateEntityPosition:
                        {
                            if (OnPositionMoved())
                            {
                                // 해당 프레임에서 이동이 감지된 경우, 다음 프레임에서 이동이 멈췄는지 한번 더 체크해야하므로 플래그를 세워준다.
                                nextFrameFlagMask.AddFlag(GameEntityTool.LateEventType.UpdateEntityPosition);
                            }
                            break;
                        }
                        case GameEntityTool.LateEventType.UpdateCameraInteract:
                        {
#if !SERVER_DRIVE
                            OnUpdateCameraInteractState();
#endif
                            break;
                        }
                        case GameEntityTool.LateEventType.UpdateSkillChange:
                        {
                            GameEntityBaseEventSender.SendEvent(GameEntityTool.GameEntityBaseEventType.Skill_Change, default);
                            break;
                        }
                        case GameEntityTool.LateEventType.BaseStatusChange:
                        {
                            OnBaseStatusChanged();
                            break;
                        }
                        case GameEntityTool.LateEventType.BattleStatusChange:
                        {
                            OnBattleStatusChanged();
                            break;
                        }
                        case GameEntityTool.LateEventType.ShotStatusChange:
                        {
                            OnShotStatusChanged();
                            break;
                        }
                        case GameEntityTool.LateEventType.Pooling:
                        {
                            switch (this)
                            {
                                case var _ when AnimationModule.IsReservedMotion(AnimationTool.MotionType.Dead):
                                case var _ when AnimationModule.IsCurrentMotion(AnimationTool.MotionType.Dead) 
                                                && AnimationModule.IsCurrentMotionProgressing():
                                case var _ when HasState_Or(GameEntityTool.EntityStateType.DRIVE_EVENT):
                                case var _ when HasAttribute(GameEntityTool.GameEntityAttributeType.PreserveCorpse):
                                case var _ when !IsLifeSpanTerminated():
                                case var _ when _DeadFadeOutFlag:
                                {
                                    nextFrameFlagMask.AddFlag(GameEntityTool.LateEventType.Pooling);
                                    break;
                                }
                                default:
                                {
                                    Pooling();
                                    break;
                                }
                            }
                            break;
                        }
                        case GameEntityTool.LateEventType.InventoryUpdate:
                        {
                            GameEntityBaseEventSender?.SendEvent(GameEntityTool.GameEntityBaseEventType.Inventory_Change, new GameEntityBaseEventParams(this));
                            break;
                        }
                    }
                }
            }
            
            _LateEventFlagMask = nextFrameFlagMask;
        }

        #endregion
        
        #region <Methods>

        private void SetLateEventFlag(GameEntityTool.LateEventType p_EventType)
        {
            _LateEventFlagMask.AddFlag(p_EventType);
        }

        public void ReserveUpdatePosition()
        {
            SetLateEventFlag(GameEntityTool.LateEventType.UpdateEntityPosition);
        }
        
        public void ReserveUpdateCameraInteract()
        {
            SetLateEventFlag(GameEntityTool.LateEventType.UpdateCameraInteract);
        }
        
        public void ReserveSkillChange()
        {
            SetLateEventFlag(GameEntityTool.LateEventType.UpdateSkillChange);
        }

        private void ReserveBaseStatusChange()
        {
            SetLateEventFlag(GameEntityTool.LateEventType.BaseStatusChange);
        }

        private void ReserveBattleStatusChange()
        {
            SetLateEventFlag(GameEntityTool.LateEventType.BattleStatusChange);
        }

        private void ReserveShotStatusChange()
        {
            SetLateEventFlag(GameEntityTool.LateEventType.ShotStatusChange);
        }
        
        public void ReservePooling()
        {
            SetLateEventFlag(GameEntityTool.LateEventType.Pooling);
        }

        public void ReserveInventoryUpdate()
        {
            SetLateEventFlag(GameEntityTool.LateEventType.InventoryUpdate);
        }

        #endregion
    }
}