using System.Collections.Generic;
using xk514;

namespace k514.Mono.Common
{
    public partial class GameEntityBase<Content, CreateParams, ActivateParams>
    {
        #region <Fields>

        /// <summary>
        /// 필터 이벤트 핸들러 리스트
        /// </summary>
        private List<IFilterEventHandler> _FilterEventHandlerList;

        /// <summary>
        /// 실행중인 필터 이벤트 핸들러 리스트
        /// </summary>
        private List<IFilterEventHandler> _ProgressingFilterEventHandlerList;
        
        /// <summary>
        /// 종료된 필터 이벤트 핸들러 리스트
        /// </summary>
        private List<IFilterEventHandler> _TerminatedFilterEventHandlerList;
        
        #endregion
        
        #region <Callbacks>

        private void OnCreateFilterEventHandler()
        {
            _FilterEventHandlerList = new List<IFilterEventHandler>();
            _ProgressingFilterEventHandlerList = new List<IFilterEventHandler>();
            _TerminatedFilterEventHandlerList = new List<IFilterEventHandler>();
        }

        private void OnActivateFilterEventHandler(ActivateParams p_ActivateParams)
        {
        }

        private void OnUpdateFilterEventHandler(float p_DeltaTime)
        {
            foreach (var eventHandler in _FilterEventHandlerList)
            {
                if (eventHandler.Progress(p_DeltaTime))
                {
                    _ProgressingFilterEventHandlerList.Add(eventHandler);
                }
                else
                {
                    _TerminatedFilterEventHandlerList.Add(eventHandler);
                }
            }

            foreach (var eventHandler in _TerminatedFilterEventHandlerList)
            {
                eventHandler.Pooling();
            }
            
            _TerminatedFilterEventHandlerList.Clear();
            (_FilterEventHandlerList, _ProgressingFilterEventHandlerList) = (_ProgressingFilterEventHandlerList, _FilterEventHandlerList);
            _ProgressingFilterEventHandlerList.Clear();
        }
        
        private void OnRetrieveFilterEventHandler()
        {
            TerminateAllFilterHandler();
        }

        #endregion

        #region <Methods>
        
        public List<IFilterEventHandler> GetFilterEventHandlerList()
        {
            return _FilterEventHandlerList;
        }
   
        public bool TryRunFilterEvent(FilterEventTool.FilterEventType p_Key, FilterEventHandlerActivateParams p_Params)
        {
            var eventHandler = FilterEventManager.GetInstanceUnsafe.GetHandler(p_Key, p_Params);
            
            if (eventHandler.IsEnterable())
            {
                _ProgressingFilterEventHandlerList.Add(eventHandler);
                return true;
            }
            else
            {
                eventHandler.Pooling();
                return false;
            }
        }
    
        public void TerminateAllFilterHandler()
        {
            foreach (var eventHandler in _FilterEventHandlerList)
            {
                eventHandler.Pooling();
            }
            
            _FilterEventHandlerList.Clear();
            _ProgressingFilterEventHandlerList.Clear();
            _TerminatedFilterEventHandlerList.Clear();
        }
        
#if UNITY_EDITOR
        public void PrintFilterHandlerList()
        {
            var FilterList = GetFilterEventHandlerList();
            CustomDebug.LogError($"*** Filter Handler List (Count : {FilterList.Count}) ***");
            foreach (var container in FilterList)
            {
                CustomDebug.LogError($" - {container.EventId}");
            }
        }
#endif
        
        #endregion
    }
}