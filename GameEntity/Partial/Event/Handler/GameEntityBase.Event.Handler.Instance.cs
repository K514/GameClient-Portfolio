using System.Collections.Generic;
using xk514;

namespace k514.Mono.Common
{
    public partial class GameEntityBase<Content, CreateParams, ActivateParams>
    {
        #region <Fields>

        /// <summary>
        /// 인스턴스 이벤트 핸들러 리스트
        /// </summary>
        private List<IInstanceEventHandler> _InstanceEventHandlerList;

        /// <summary>
        /// 실행중인 인스턴스 이벤트 핸들러 리스트
        /// </summary>
        private List<IInstanceEventHandler> _ProgressingInstanceEventHandlerList;
        
        /// <summary>
        /// 종료된 인스턴스 이벤트 핸들러 리스트
        /// </summary>
        private List<IInstanceEventHandler> _TerminatedInstanceEventHandlerList;

        /// <summary>
        /// 개체 초기화시 예약된 인스턴스 이벤트 프리셋
        /// </summary>
        private InstanceEventTool.InstanceEventPreset _ReservedInstanceEventPreset;
        
        #endregion
        
        #region <Callbacks>

        protected virtual void OnCreateInstanceEventHandler()
        {
            _InstanceEventHandlerList = new List<IInstanceEventHandler>();
            _ProgressingInstanceEventHandlerList = new List<IInstanceEventHandler>();
            _TerminatedInstanceEventHandlerList = new List<IInstanceEventHandler>();
        }

        protected virtual void OnActivateInstanceEventHandler(ActivateParams p_ActivateParams)
        {
            _ReservedInstanceEventPreset = p_ActivateParams.ReservedEventPreset;
        }

        private void OnUpdateInstanceEventHandler(float p_DeltaTime)
        {
            foreach (var eventHandler in _InstanceEventHandlerList)
            {
                if (eventHandler.Progress(p_DeltaTime))
                {
                    _ProgressingInstanceEventHandlerList.Add(eventHandler);
                }
                else
                {
                    _TerminatedInstanceEventHandlerList.Add(eventHandler);
                }
            }

            foreach (var eventHandler in _TerminatedInstanceEventHandlerList)
            {
                eventHandler.Pooling();
            }
            
            _TerminatedInstanceEventHandlerList.Clear();
            (_InstanceEventHandlerList, _ProgressingInstanceEventHandlerList) = (_ProgressingInstanceEventHandlerList, _InstanceEventHandlerList);
            _ProgressingInstanceEventHandlerList.Clear();
        }
        
        private void OnRetrieveInstanceEventHandler()
        {
            ResetReservedInstanceEvent();
            TerminateAllInstanceHandler();
        }

        #endregion

        #region <Methods>
        
        public List<IInstanceEventHandler> GetInstanceEventHandlerList()
        {
            return _InstanceEventHandlerList;
        }
   
        public bool TryRunInstanceEvent(InstanceEventTool.InstanceEventType p_Key, InstanceEventHandlerActivateParams p_Params)
        {
            var eventHandler = InstanceEventManager.GetInstanceUnsafe.GetHandler(p_Key, p_Params);
            
            if (eventHandler.IsEnterable())
            {
                _ProgressingInstanceEventHandlerList.Add(eventHandler);
                return true;
            }
            else
            {
                eventHandler.Pooling();
                return false;
            }
        }

        public bool TryRunReservedInstanceEvent()
        {
            if (_ReservedInstanceEventPreset.ValidFlag)
            {
                return TryRunInstanceEvent(_ReservedInstanceEventPreset.EventId, _ReservedInstanceEventPreset.ActivateParams);
            }
            else
            {
                return false;
            }
        }

        public void ResetReservedInstanceEvent()
        {
            _ReservedInstanceEventPreset = default;
        }
    
        public void TerminateAllInstanceHandler()
        {
            foreach (var eventHandler in _InstanceEventHandlerList)
            {
                eventHandler.Pooling();
            }
            
            _InstanceEventHandlerList.Clear();
            _ProgressingInstanceEventHandlerList.Clear();
            _TerminatedInstanceEventHandlerList.Clear();
        }
        
#if UNITY_EDITOR
        public void PrintInstanceHandlerList()
        {
            var InstanceList = GetInstanceEventHandlerList();
            CustomDebug.LogError($"*** Instance Handler List (Count : {InstanceList.Count}) ***");
            foreach (var container in InstanceList)
            {
                CustomDebug.LogError($" - {container.EventId}");
            }
        }
#endif
        
        #endregion
    }
}