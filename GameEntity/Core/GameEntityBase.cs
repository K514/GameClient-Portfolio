using xk514;

namespace k514.Mono.Common
{
    public abstract partial class GameEntityBase<Content, CreateParams, ActivateParams> : WorldObjectBase<Content, CreateParams, ActivateParams>, IGameEntityBridge
        where Content : GameEntityBase<Content, CreateParams, ActivateParams>
        where CreateParams : PrefabCreateParamsBase<CreateParams>, new()
        where ActivateParams : IGameEntityActivateParams
    {
        #region <Callbacks>

        protected override void OnCreate(CreateParams p_CreateParams)
        {
            base.OnCreate(p_CreateParams);

            OnCreateInteract();
            OnCreateStatus();
            OnCreateState();
            OnCreateVolume();
            OnCreateModule();
            OnCreateAttribute();
            OnCreateEventHandler();
            OnCreateEntityInventory();
            OnCreateParticleSystem();
            OnCreateMaterialControl();
        }

        protected override bool OnActivate(CreateParams CreateParams, ActivateParams p_ActivateParams, bool p_IsPooled)
        {
            if (base.OnActivate(CreateParams, p_ActivateParams, p_IsPooled) && OnActivateInteract(p_ActivateParams))
            {
                OnActivateStatus(p_ActivateParams);
                OnActivateState();
                OnActivateVolume();
                OnActivateModule();
                OnActivateAttribute(p_ActivateParams);
                OnActivateEventHandler(p_ActivateParams);
                OnActivateEntityInventory(p_ActivateParams);
                OnActivateParticleSystem();
                OnActivateMaterialControl(p_ActivateParams);

                return true;
            }
            else
            {
                return false;
            }
        }

        protected override void OnRetrieve(CreateParams p_CreateParams, bool p_IsPooled, bool p_IsDisposed)
        {
#if UNITY_EDITOR
            if (CustomDebug.AIStateName)
            {
                SetPostFix($"(Retrieved)"); 
            }
#endif 
            GameEntityBaseEventSender?.SendEvent(GameEntityTool.GameEntityBaseEventType.Retrieved, new GameEntityBaseEventParams(this));
            
            OnRetrieveMaterialControl();
            OnRetrieveParticleSystem();
            OnRetrieveEntityInventory();
            OnRetrieveEventHandler();
            OnRetrieveAttribute();
            OnRetrieveModule(p_IsPooled, p_IsDisposed);
            OnRetrieveVolume();
            OnRetrieveState();
            OnRetrieveStatus();
            OnRetrieveInteract();
            
            base.OnRetrieve(p_CreateParams, p_IsPooled, p_IsDisposed);
        }
        
        /// <summary>
        /// 인스턴스가 파기될 때 수행할 작업을 기술한다.
        /// </summary>
        protected override void OnDispose()
        {
            OnDisposeInteract();
            OnDisposeModule();
            OnDisposeStatus();
            
            base.OnDispose();
        }

        #endregion
        
        #region <Methods>

        /// <summary>
        /// 해당 유닛의 '시스템 사용불가' 상태를 지정하는 메서드
        /// </summary>
        public void SetDisable(bool p_Flag)
        {
            if (IsDisable != p_Flag)
            {
                if (p_Flag)
                {
#if UNITY_EDITOR
                    if (CustomDebug.AIStateName)
                    {
                        SetPostFix($"(SystemDisabled)");
                    }
#endif
                    AddState(GameEntityTool.EntityStateType.DISABLE);
                    gameObject.SetActiveSafe(false);
                    SleepAllModule();
                    TerminateAllEventHandler();
                    GameEntityBaseEventSender.SendEvent(GameEntityTool.GameEntityBaseEventType.Disabled, new GameEntityBaseEventParams(this));
                }
                else
                {
#if UNITY_EDITOR
                    if (CustomDebug.AIStateName)
                    {
                        SetPostFix($"(SystemEnabled)");
                    }
#endif
                    RemoveState(GameEntityTool.EntityStateType.DISABLE);
                    gameObject.SetActiveSafe(true);
                    AwakeAllModule();
                    GameEntityBaseEventSender.SendEvent(GameEntityTool.GameEntityBaseEventType.Enabled, new GameEntityBaseEventParams(this));
                }
            }
        }

        #endregion
    }
}