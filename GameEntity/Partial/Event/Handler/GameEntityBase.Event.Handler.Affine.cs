using System.Collections.Generic;
using xk514;

namespace k514.Mono.Common
{
    public partial class GameEntityBase<Content, CreateParams, ActivateParams>
    {
        #region <Fields>

        /// <summary>
        /// 아핀 이벤트 핸들러 리스트
        /// </summary>
        private List<IAffineEventHandler> _AffineEventHandlerList;

        /// <summary>
        /// 실행중인 아핀 이벤트 핸들러 리스트
        /// </summary>
        private List<IAffineEventHandler> _ProgressingAffineEventHandlerList;
        
        /// <summary>
        /// 종료된 아핀 이벤트 핸들러 리스트
        /// </summary>
        private List<IAffineEventHandler> _TerminatedAffineEventHandlerList;
        
        #endregion

        #region <Callbacks>

        private void OnCreateAffineEventHandler()
        {
            _AffineEventHandlerList = new List<IAffineEventHandler>();
            _ProgressingAffineEventHandlerList = new List<IAffineEventHandler>();
            _TerminatedAffineEventHandlerList = new List<IAffineEventHandler>();
        }

        private void OnActivateAffineEventHandler(ActivateParams p_ActivateParams)
        {
        }
  
        protected virtual void OnUpdateAffineEventHandler(float p_DeltaTime)
        {
            foreach (var eventHandler in _AffineEventHandlerList)
            {
                if (eventHandler.Progress(p_DeltaTime))
                {
                    _ProgressingAffineEventHandlerList.Add(eventHandler);
                }
                else
                {
                    _TerminatedAffineEventHandlerList.Add(eventHandler);
                }
            }

            foreach (var eventHandler in _TerminatedAffineEventHandlerList)
            {
                eventHandler.Pooling();
            }
            
            _TerminatedAffineEventHandlerList.Clear();
            (_AffineEventHandlerList, _ProgressingAffineEventHandlerList) = (_ProgressingAffineEventHandlerList, _AffineEventHandlerList);
            _ProgressingAffineEventHandlerList.Clear();
        }
              
        private void OnRetrieveAffineEventHandler()
        {
            TerminateAllAffineHandler();
        }

        #endregion

        #region <Methods>
        
        public List<IAffineEventHandler> GetAffineEventHandlerList()
        {
            return _AffineEventHandlerList;
        }
        
        public bool TryRunAffineEvent(AffineEventTool.AffineEventType p_Key, AffineEventHandlerActivateParams p_Params)
        {
            var eventHandler = AffineEventManager.GetInstanceUnsafe.GetHandler(p_Key, p_Params);
            
            if (eventHandler.IsEnterable())
            {
                _ProgressingAffineEventHandlerList.Add(eventHandler);
                return true;
            }
            else
            {
                eventHandler.Pooling();
                return false;
            }
        }

        public void TerminateAllAffineHandler()
        {
            foreach (var eventHandler in _AffineEventHandlerList)
            {
                eventHandler.Pooling();
            }
            
            _AffineEventHandlerList.Clear();
            _ProgressingAffineEventHandlerList.Clear();
            _TerminatedAffineEventHandlerList.Clear();
        }
        
#if UNITY_EDITOR
        public void PrintAffineHandlerList()
        {
            var affineList = GetAffineEventHandlerList();
            CustomDebug.LogError($"*** Affine Handler List (Count : {affineList.Count}) ***");
            foreach (var container in affineList)
            {
                CustomDebug.LogError($" - {container.EventId}");
            }
        }
#endif

        #endregion
    }
}