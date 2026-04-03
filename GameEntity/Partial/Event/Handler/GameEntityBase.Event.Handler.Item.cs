using System.Collections.Generic;
using xk514;

namespace k514.Mono.Common
{
    public partial class GameEntityBase<Content, CreateParams, ActivateParams>
    {
        #region <Fields>

        /// <summary>
        /// 아이템 이벤트 핸들러 리스트
        /// </summary>
        private List<IItemEventHandler> _ItemEventHandlerList;

        /// <summary>
        /// 실행중인 아이템 이벤트 핸들러 리스트
        /// </summary>
        private List<IItemEventHandler> _ProgressingItemEventHandlerList;
        
        /// <summary>
        /// 종료된 아이템 이벤트 핸들러 리스트
        /// </summary>
        private List<IItemEventHandler> _TerminatedItemEventHandlerList;
        
        #endregion
        
        #region <Callbacks>

        private void OnCreateItemEventHandler()
        {
            _ItemEventHandlerList = new List<IItemEventHandler>();
            _ProgressingItemEventHandlerList = new List<IItemEventHandler>();
            _TerminatedItemEventHandlerList = new List<IItemEventHandler>();
        }

        private void OnActivateItemEventHandler(ActivateParams p_ActivateParams)
        {
        }
        
        protected virtual void OnUpdateItemEventHandler(float p_DeltaTime)
        {
            foreach (var eventHandler in _ItemEventHandlerList)
            {
                if (eventHandler.Progress(p_DeltaTime))
                {
                    _ProgressingItemEventHandlerList.Add(eventHandler);
                }
                else
                {
                    _TerminatedItemEventHandlerList.Add(eventHandler);
                }
            }

            foreach (var eventHandler in _TerminatedItemEventHandlerList)
            {
                eventHandler.Pooling();
            }
            
            _TerminatedItemEventHandlerList.Clear();
            (_ItemEventHandlerList, _ProgressingItemEventHandlerList) = (_ProgressingItemEventHandlerList, _ItemEventHandlerList);
            _ProgressingItemEventHandlerList.Clear();
        }
              
        private void OnRetrieveItemEventHandler()
        {
            TerminateAllItemHandler();
        }
        
        #endregion

        #region <Methods>
        
        public List<IItemEventHandler> GetItemEventHandlerList()
        {
            return _ItemEventHandlerList;
        }
        
        public bool TryRunItemEvent(int p_Key, ItemEventHandlerActivateParams p_Params)
        {
            var eventHandler = ItemEventManager.GetInstanceUnsafe.GetHandler(p_Key, p_Params);
            
            if (eventHandler.IsEnterable())
            {
                _ProgressingItemEventHandlerList.Add(eventHandler);
                return true;
            }
            else
            {
                eventHandler.Pooling();
                return false;
            }
        }

        public void TerminateAllItemHandler()
        {
            foreach (var eventHandler in _ItemEventHandlerList)
            {
                eventHandler.Pooling();
            }
            
            _ItemEventHandlerList.Clear();
            _ProgressingItemEventHandlerList.Clear();
            _TerminatedItemEventHandlerList.Clear();
        }
        
#if UNITY_EDITOR
        public void PrintItemHandlerList()
        {
            var itemList = GetItemEventHandlerList();
            CustomDebug.LogError($"*** Item Handler List (Count : {itemList.Count}) ***");
            foreach (var container in itemList)
            {
                CustomDebug.LogError($" - {container.EventId}");
            }
        }
#endif
        
        #endregion
    }
}