using UnityEngine;

namespace k514.Mono.Common
{
    public partial class ActionBase
    {
        #region <Fields>

        /// <summary>
        /// 현재 진행중인 액션 핸들러
        /// </summary>
        private IActionEventHandler _MainActionEventHandler;
        
        #endregion

        #region <Callbacks>

        private void OnAwakeEventHandler()
        {
            OnActionChanged();
        }

        private void OnSleepEventHandler()
        {
            ReleaseAllInput();
        }
        
        #endregion
        
        #region <Methods>

        public void TryInterruptMainHandler(IActionEventHandler p_Handler)
        {
            if (ReferenceEquals(null, _MainActionEventHandler) || _MainActionEventHandler.IsInterruptable(p_Handler))
            {
                _MainActionEventHandler?.OnInterrupted();
                _MainActionEventHandler = p_Handler;
                _MainActionEventHandler.OnInterruptSuccess();
            }
            else
            {
                p_Handler.OnInterruptFail();
            }
        }
        
        public void TryReleaseMainHandler(IActionEventHandler p_Handler)
        {
            if (!ReferenceEquals(null, _MainActionEventHandler) && ReferenceEquals(p_Handler, _MainActionEventHandler))
            {
                _MainActionEventHandler.OnInterrupted();
                _MainActionEventHandler = null;
            }
        }
        
        public void ReleaseAllInput()
        {
            foreach (var handlerListKV in _ActionEventHandlerListTableByLabel)
            {
                var handlerList = handlerListKV.Value;
                foreach (var handler in handlerList)
                {
                    handler.ManualInputRelease();
                }
            }
        }

        #endregion
    }
}