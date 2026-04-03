#if !SERVER_DRIVE

namespace k514.Mono.Common
{
    public partial class UIxElementBase
    {
        #region <Fields>

        /// <summary>
        /// 현재 추적중인 개체
        ///
        /// 개체와 UI 는 1 : N의 관계를 가지므로
        /// 개체 쪽에서는 EventSender를 통해서 UI의 EventReceiver와 통신하고
        /// UI 쪽에서는 해당 필드를 통해 개체와 직접 통신한다.
        /// </summary>
        protected IGameEntityBridge _EventEntity;

        #endregion

        #region <Callbacks>

        protected override void OnCreateEventSender()
        {
        }

        protected override void OnActivateEventSender(UIPoolManager.ActivateParams p_ActivateParams)
        {
            if (p_ActivateParams.ActivateParamType == UIPoolManager.ActivateParams.UIActivateParamType.EventEntity)
            {
                SetEventEntity(p_ActivateParams.EventEntity);
            }
        }

        protected override void OnRetrieveEventSender()
        {
            RemoveEntityBaseEvent();
            RemoveEntityUIEvent();
            RemoveEntityRenderEvent();
            RemoveEventEntity();
        }

        protected override void OnDisposeEventSender()
        {
            OnDisposeEntityBaseEventSender();
            OnDisposeEntityUIEventSender();
            OnDisposeEntityRenderEventSender();
        }

        #endregion
        
        #region <Methods>

        protected bool IsEventEntityValid()
        {
            return _EventEntity.IsEntityValid();
        }
        
        protected virtual void SetEventEntity(IGameEntityBridge p_Entity)
        {
            RemoveEventEntity();
            
            if (p_Entity.IsEntityValid())
            {
                _UIDynamicStateFlagMask.AddFlag(UIxTool.UIxDynamicStateType.HasEventEntity);
                _EventEntity = p_Entity;
                
                var enumerator = UIxTool._UIDynamicStateTypeEnumerator_EntityEvent;
                foreach (var stateType in enumerator)
                {
                    if (_UIDynamicStateFlagMask.HasAnyFlagExceptNone(stateType))
                    {
                        switch (stateType)
                        {
                            case UIxTool.UIxDynamicStateType.HasEntityBaseEvent:
                                _EventEntity.AddReceiver(_EntityBaseEventReceiver);
                                break;
                            case UIxTool.UIxDynamicStateType.HasEntityUIEvent:
                                _EventEntity.AddReceiver(_EntityUIEventReceiver);
                                break;
                            case UIxTool.UIxDynamicStateType.HasEntityRenderEvent:
                                _EventEntity.AddReceiver(_EntityRenderEventReceiver);
                                break;
                        }
                    }
                }
            }
        }
        
        public void RemoveEventEntity()
        {
            if (_UIDynamicStateFlagMask.HasAnyFlagExceptNone(UIxTool.UIxDynamicStateType.HasEventEntity))
            {
                _UIDynamicStateFlagMask.RemoveFlag(UIxTool.UIxDynamicStateType.HasEventEntity);
       
                var enumerator = UIxTool._UIDynamicStateTypeEnumerator_EntityEvent;
                foreach (var stateType in enumerator)
                {
                    if (_UIDynamicStateFlagMask.HasAnyFlagExceptNone(stateType))
                    {
                        switch (stateType)
                        {
                            case UIxTool.UIxDynamicStateType.HasEntityBaseEvent:
                                _EventEntity.RemoveReceiver(_EntityBaseEventReceiver);
                                break;
                            case UIxTool.UIxDynamicStateType.HasEntityUIEvent:
                                _EventEntity.RemoveReceiver(_EntityUIEventReceiver);
                                break;
                            case UIxTool.UIxDynamicStateType.HasEntityRenderEvent:
                                _EventEntity.RemoveReceiver(_EntityRenderEventReceiver);
                                break;
                        }
                    }
                }
                
                _EventEntity = null;
            }
        }

        #endregion
    }
}
#endif