using System.Collections.Generic;
using UnityEngine;

namespace k514.Mono.Common
{
    public partial class GameEntityBase<Content, CreateParams, ActivateParams>
    {
        #region <Fields>

        /// <summary>
        /// 게임 개체 기본 이벤트 송신자
        /// </summary>
        protected GameEntityBaseEventSender GameEntityBaseEventSender;
        
        /// <summary>
        /// 게임 개체 UI 이벤트 송신자
        /// </summary>
        protected GameEntityUIEventSender GameEntityUIEventSender;

        /// <summary>
        /// 게임 개체 내부모듈 이벤트 송신자
        /// </summary>
        protected GameEntityModuleEventSender GameEntityInnerModuleEventSender;

#if !SERVER_DRIVE
        /// <summary>
        /// 게임 개체 랜더 상태 변화 이벤트 송신자
        /// </summary>
        protected GameEntityRenderEventSender GameEntityRenderEventSender;
#endif
        
        #endregion
        
        #region <Callbacks>

        protected override void OnCreateEventSender()
        {
            GameEntityBaseEventSender = new GameEntityBaseEventSender();
            GameEntityUIEventSender = new GameEntityUIEventSender();
            GameEntityInnerModuleEventSender = new GameEntityModuleEventSender();
#if !SERVER_DRIVE
            GameEntityRenderEventSender = new GameEntityRenderEventSender();
#endif
        }

        protected override void OnActivateEventSender(ActivateParams p_ActivateParams)
        {

        }
        
        protected override void OnRetrieveEventSender()
        {
            GameEntityBaseEventSender.ClearReceiver();
            GameEntityUIEventSender.ClearReceiver();
            GameEntityInnerModuleEventSender.ClearReceiver();
#if !SERVER_DRIVE
            GameEntityRenderEventSender.ClearReceiver();
#endif
        }

        protected override void OnDisposeEventSender()
        {
            if (!ReferenceEquals(null, GameEntityBaseEventSender))
            {
                GameEntityBaseEventSender.Dispose();
                GameEntityBaseEventSender = null;
            }
            if (!ReferenceEquals(null, GameEntityUIEventSender))
            {
                GameEntityUIEventSender.Dispose();
                GameEntityUIEventSender = null;
            }
            if (!ReferenceEquals(null, GameEntityInnerModuleEventSender))
            {
                GameEntityInnerModuleEventSender.Dispose();
                GameEntityInnerModuleEventSender = null;
            }
#if !SERVER_DRIVE
            if (!ReferenceEquals(null, GameEntityRenderEventSender))
            {
                GameEntityRenderEventSender.Dispose();
                GameEntityRenderEventSender = null;
            }
#endif
        }
        
        #endregion
        
        #region <Methods>

        public void AddReceiver(GameEntityBaseEventReceiver p_EventReceiver)
        {
            GameEntityBaseEventSender.AddReceiver(p_EventReceiver);
        }

        public void AddReceiver(GameEntityUIEventReceiver p_EventReceiver)
        {
            GameEntityUIEventSender.AddReceiver(p_EventReceiver);
        }

        public void AddReceiver(GameEntityModuleEventReceiver p_EventReceiver)
        {
            GameEntityInnerModuleEventSender.AddReceiver(p_EventReceiver);
        }

        public void SendEvent(GameEntityTool.GameEntityBaseEventType p_Type, GameEntityBaseEventParams p_Params)
        {
            GameEntityBaseEventSender.SendEvent(p_Type, p_Params);
        }

        public void SendEvent(GameEntityTool.GameEntityUIEventType p_Type, GameEntityUIEventParams p_Params)
        {
            GameEntityUIEventSender.SendEvent(p_Type, p_Params);
        }

        public void SendEvent(GameEntityTool.GameEntityModuleEventType p_Type, GameEntityModuleEventParams p_Params)
        {
            GameEntityInnerModuleEventSender.SendEvent(p_Type, p_Params);
        }
        
        public void RemoveReceiver(GameEntityBaseEventReceiver p_EventReceiver)
        {
            GameEntityBaseEventSender?.RemoveReceiver(p_EventReceiver);
        }
        
        public void RemoveReceiver(GameEntityUIEventReceiver p_EventReceiver)
        {
            GameEntityUIEventSender?.RemoveReceiver(p_EventReceiver);
        }
        
        public void RemoveReceiver(GameEntityModuleEventReceiver p_EventReceiver)
        {
            GameEntityInnerModuleEventSender?.RemoveReceiver(p_EventReceiver);
        }


#if !SERVER_DRIVE
        public void AddReceiver(GameEntityRenderEventReceiver p_EventReceiver)
        {
            GameEntityRenderEventSender.AddReceiver(p_EventReceiver);
        }

        public void SendEvent(GameEntityTool.GameEntityRenderType p_Type, GameEntityRenderEventParams p_Params)
        {
            GameEntityRenderEventSender.SendEvent(p_Type, p_Params);
        }
        
        public void RemoveReceiver(GameEntityRenderEventReceiver p_EventReceiver)
        {
            GameEntityRenderEventSender?.RemoveReceiver(p_EventReceiver);
        }
#endif

        #endregion
    }
}