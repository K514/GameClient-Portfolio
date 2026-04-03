#if !SERVER_DRIVE

namespace k514.Mono.Common
{
    public partial class UIxElementBase
    {
        #region <Fields>

        /// <summary>
        /// 개체 UI 이벤트 수신자
        /// </summary>
        private GameEntityUIEventReceiver _EntityUIEventReceiver;

        #endregion

        #region <Callbacks>

        protected virtual void OnEntityUIEventTriggered(GameEntityTool.GameEntityUIEventType p_EventType, GameEntityUIEventParams p_Params)
        {
        }

        private void OnDisposeEntityUIEventSender()
        {
            if (!ReferenceEquals(null, _EntityUIEventReceiver))
            {
                _EntityUIEventReceiver.Dispose();
                _EntityUIEventReceiver = null;
            }
        }
        
        #endregion

        #region <Methods>

        public void SetEntityUIEvent(GameEntityTool.GameEntityUIEventType p_FlagMask)
        {
            RemoveEntityUIEvent();
            
            if (ReferenceEquals(null, _EntityUIEventReceiver))
            {
                _EntityUIEventReceiver = new GameEntityUIEventReceiver(p_FlagMask, OnEntityUIEventTriggered);
            }
            
            _EventEntity?.AddReceiver(_EntityUIEventReceiver);
            _UIDynamicStateFlagMask.AddFlag(UIxTool.UIxDynamicStateType.HasEntityUIEvent);
        }

        public void RemoveEntityUIEvent()
        {
            if (_UIDynamicStateFlagMask.HasAnyFlagExceptNone(UIxTool.UIxDynamicStateType.HasEntityUIEvent))
            {
                _UIDynamicStateFlagMask.RemoveFlag(UIxTool.UIxDynamicStateType.HasEntityUIEvent);
                _EventEntity?.RemoveReceiver(_EntityUIEventReceiver);
            }
        }

        #endregion
    }
}

#endif