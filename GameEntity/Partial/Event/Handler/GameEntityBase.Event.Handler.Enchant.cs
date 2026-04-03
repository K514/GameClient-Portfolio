using System.Collections.Generic;
using k514.Mono.Feature;
using UnityEngine;
using xk514;

namespace k514.Mono.Common
{
    public partial class GameEntityBase<Content, CreateParams, ActivateParams>
    {
        #region <Fields>

        /// <summary>
        /// 인챈트 이벤트 핸들러 리스트
        /// </summary>
        private List<IEnchantEventHandler> _EnchantEventHandlerList;

        /// <summary>
        /// 실행중인 인챈트 이벤트 핸들러 리스트
        /// </summary>
        private List<IEnchantEventHandler> _ProgressingEnchantEventHandlerList;
        
        /// <summary>
        /// 종료된 인챈트 이벤트 핸들러 리스트
        /// </summary>
        private List<IEnchantEventHandler> _TerminatedEnchantEventHandlerList;
        
        #endregion
        
        #region <Callbacks>

        private void OnCreateEnchantEventHandler()
        {
            _EnchantEventHandlerList = new List<IEnchantEventHandler>();
            _ProgressingEnchantEventHandlerList = new List<IEnchantEventHandler>();
            _TerminatedEnchantEventHandlerList = new List<IEnchantEventHandler>();
        }

        private void OnActivateEnchantEventHandler(ActivateParams p_ActivateParams)
        {
        }
        
        private void OnUpdateEnchantEventHandler(float p_DeltaTime)
        {
            foreach (var eventHandler in _EnchantEventHandlerList)
            {
                if (eventHandler.Progress(p_DeltaTime))
                {
                    _ProgressingEnchantEventHandlerList.Add(eventHandler);
                }
                else
                {
                    _TerminatedEnchantEventHandlerList.Add(eventHandler);
                }
            }

            foreach (var eventHandler in _TerminatedEnchantEventHandlerList)
            {
                eventHandler.Pooling();
            }
            
            _TerminatedEnchantEventHandlerList.Clear();
            (_EnchantEventHandlerList, _ProgressingEnchantEventHandlerList) = (_ProgressingEnchantEventHandlerList, _EnchantEventHandlerList);
            _ProgressingEnchantEventHandlerList.Clear();
        }
        
        private void OnRetrieveEnchantEventHandler()
        {
            TerminateAllEnchantHandler();
        }

        #endregion

        #region <Methods>

        public List<IEnchantEventHandler> GetEnchantEventHandlerList()
        {
            return _EnchantEventHandlerList;
        }

        public bool TryRunEnchantEvent(int p_Key, EnchantEventHandlerActivateParams p_Params)
        {
            var eventHandler = EnchantEventManager.GetInstanceUnsafe.GetHandler(p_Key, p_Params);
            
            if (eventHandler.IsEnterable())
            {
                _ProgressingEnchantEventHandlerList.Add(eventHandler);
                return true;
            }
            else
            {
                eventHandler.Pooling();
                return false;
            }
        }

        public void TerminateAllEnchantHandler()
        {
            foreach (var eventHandler in _EnchantEventHandlerList)
            {
                eventHandler.Pooling();
            }
            
            _EnchantEventHandlerList.Clear();
            _ProgressingEnchantEventHandlerList.Clear();
            _TerminatedEnchantEventHandlerList.Clear();
        }

#if UNITY_EDITOR
        public void PrintEnchantHandlerList()
        {
            var enchantList = GetEnchantEventHandlerList();
            CustomDebug.LogError($"*** Enchant Handler List (Count : {enchantList.Count}) ***");
            foreach (var container in enchantList)
            {
                CustomDebug.LogError($" - {container.EventId}");
            }
        }
#endif
        
        #endregion
    }
}