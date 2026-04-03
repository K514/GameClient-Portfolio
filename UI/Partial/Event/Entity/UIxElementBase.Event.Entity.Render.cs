#if !SERVER_DRIVE

namespace k514.Mono.Common
{
    public partial class UIxElementBase
    {
        #region <Fields>

        /// <summary>
        /// 개체 렌더 이벤트 수신자
        /// </summary>
        private GameEntityRenderEventReceiver _EntityRenderEventReceiver;

        #endregion

        #region <Callbacks>

        protected virtual void OnEntityRenderEventTriggered(GameEntityTool.GameEntityRenderType p_EventType, GameEntityRenderEventParams p_Params)
        {
        }

        private void OnDisposeEntityRenderEventSender()
        {
            if (!ReferenceEquals(null, _EntityRenderEventReceiver))
            {
                _EntityRenderEventReceiver.Dispose();
                _EntityRenderEventReceiver = null;
            }
        }
        
        #endregion

        #region <Methods>

        public void SetEntityRenderEvent(GameEntityTool.GameEntityRenderType p_FlagMask)
        {
            RemoveEntityRenderEvent();

            if (ReferenceEquals(null, _EntityRenderEventReceiver))
            {
                _EntityRenderEventReceiver = new GameEntityRenderEventReceiver(p_FlagMask, OnEntityRenderEventTriggered);
            }

            _UIDynamicStateFlagMask.AddFlag(UIxTool.UIxDynamicStateType.HasEntityRenderEvent);
            _EventEntity?.AddReceiver(_EntityRenderEventReceiver);
        }

        public void RemoveEntityRenderEvent()
        {
            if (_UIDynamicStateFlagMask.HasAnyFlagExceptNone(UIxTool.UIxDynamicStateType.HasEntityRenderEvent))
            {
                _UIDynamicStateFlagMask.RemoveFlag(UIxTool.UIxDynamicStateType.HasEntityRenderEvent);
                _EventEntity?.RemoveReceiver(_EntityRenderEventReceiver);
            }
        }

        #endregion
    }
}

#endif