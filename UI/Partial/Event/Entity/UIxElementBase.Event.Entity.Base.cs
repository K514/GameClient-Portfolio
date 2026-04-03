#if !SERVER_DRIVE

namespace k514.Mono.Common
{
    public partial class UIxElementBase
    {
        #region <Fields>

        /// <summary>
        /// 개체 기본 이벤트 수신자
        /// </summary>
        private GameEntityBaseEventReceiver _EntityBaseEventReceiver;

        #endregion

        #region <Callbacks>

        protected virtual void OnEntityBaseEventTriggered(GameEntityTool.GameEntityBaseEventType p_EventType, GameEntityBaseEventParams p_Params)
        {
        }

        private void OnDisposeEntityBaseEventSender()
        {
            if (!ReferenceEquals(null, _EntityBaseEventReceiver))
            {
                _EntityBaseEventReceiver.Dispose();
                _EntityBaseEventReceiver = null;
            }
        }
        
        #endregion

        #region <Methods>
        
        protected void SetEntityBaseEvent(GameEntityTool.GameEntityBaseEventType p_FlagMask)
        {
            RemoveEntityBaseEvent();
            
            if (ReferenceEquals(null, _EntityBaseEventReceiver))
            {
                _EntityBaseEventReceiver = new GameEntityBaseEventReceiver(p_FlagMask, OnEntityBaseEventTriggered);
            }

            _EventEntity?.AddReceiver(_EntityBaseEventReceiver);
            _UIDynamicStateFlagMask.AddFlag(UIxTool.UIxDynamicStateType.HasEntityBaseEvent);
        }

        private void RemoveEntityBaseEvent()
        {
            if (_UIDynamicStateFlagMask.HasAnyFlagExceptNone(UIxTool.UIxDynamicStateType.HasEntityBaseEvent))
            {
                _UIDynamicStateFlagMask.RemoveFlag(UIxTool.UIxDynamicStateType.HasEntityBaseEvent);
                _EventEntity?.RemoveReceiver(_EntityBaseEventReceiver);
            }
        }

        #endregion
    }
}

#endif