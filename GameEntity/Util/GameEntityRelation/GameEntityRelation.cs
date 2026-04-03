using System;
using System.Collections.Generic;
using UnityEngine;

namespace k514.Mono.Common
{
    /// <summary>
    /// 게임 개체간의 1 : N 관계를 기술하는 클래스
    /// </summary>
    public class GameEntityRelation : _IDisposable
    {
        #region <Finalizer>

        ~GameEntityRelation()
        {
            Dispose();
        }

        #endregion

        #region <Fields>

        public GameEntityRelationTool.GameEntityRelationType GameEntityRelateType { get; private set; }
        public IGameEntityBridge MasterNode { get; private set; }
        private Action<GameEntityRelationTool.GameEntityRelationEventType, GameEntityRelationTool.GameEntityRelationEventParams> _MasterNodeEventHandler;
        public IGameEntityBridge CurrentSubNode { get; private set; }
        private HashSet<IGameEntityBridge> _SubNodeGroup;
        private GameEntityBaseEventReceiver _SubNodeEventReceiver;

        #endregion

        #region <Constructor>

        public GameEntityRelation(GameEntityRelationTool.GameEntityRelationType p_Type, IGameEntityBridge p_MasterNode, Action<GameEntityRelationTool.GameEntityRelationEventType, GameEntityRelationTool.GameEntityRelationEventParams> p_EventHandler)
        {
            GameEntityRelateType = p_Type;
            MasterNode = p_MasterNode;
            _MasterNodeEventHandler = p_EventHandler;
            _SubNodeGroup = new HashSet<IGameEntityBridge>(GameConst.__CAPACITY_RELATION);
            _SubNodeEventReceiver = new GameEntityBaseEventReceiver(GameEntityTool.GameEntityBaseEventType.Dead | GameEntityTool.GameEntityBaseEventType.Retrieved, OnSubNodeEventTriggered);
        }

        #endregion

        #region <Callbacks>
        
        private void OnSubNodeEventTriggered(GameEntityTool.GameEntityBaseEventType p_EventType, GameEntityBaseEventParams p_EventParams)
        {
            switch (p_EventType)
            {
                case GameEntityTool.GameEntityBaseEventType.Dead:
                {
                    var targetNode = p_EventParams.Trigger;
                    if (_SubNodeGroup.Contains(targetNode))
                    {
                        _MasterNodeEventHandler.Invoke(GameEntityRelationTool.GameEntityRelationEventType.SubNodeDead, new GameEntityRelationTool.GameEntityRelationEventParams(this, targetNode));
                    }

                    break;
                }
                case GameEntityTool.GameEntityBaseEventType.Retrieved:
                {
                    RemoveNode(p_EventParams.Trigger);
                    break;
                }
            }
        }
        
        /// <summary>
        /// 인스턴스가 파기될 때 수행할 작업을 기술한다.
        /// </summary>
        private void OnDisposeUnmanaged()
        {
            MasterNode = default;
            ResetNode();

            if (!ReferenceEquals(null, _SubNodeGroup))
            {
                _SubNodeGroup.Clear();
                _SubNodeGroup = default;
            }

            if (!ReferenceEquals(null, _SubNodeEventReceiver))
            {
                _SubNodeEventReceiver.Dispose();
                _SubNodeEventReceiver = default;
            }
        }

        #endregion

        #region <Methods>

        public void AddNode(IGameEntityBridge p_Node)
        {
            if (p_Node.IsEntityValid())
            {
                if (_SubNodeGroup.Add(p_Node))
                {
                    p_Node.AddReceiver(_SubNodeEventReceiver);
                    _MasterNodeEventHandler.Invoke(GameEntityRelationTool.GameEntityRelationEventType.SubNodeAdded, new GameEntityRelationTool.GameEntityRelationEventParams(this, p_Node));
                }
            }
        }

        public void SelectNode(IGameEntityBridge p_Node)
        {
            AddNode(p_Node);

            var prevNode = CurrentSubNode;
            CurrentSubNode = p_Node;

            _MasterNodeEventHandler.Invoke(GameEntityRelationTool.GameEntityRelationEventType.SubNodeSelected, new GameEntityRelationTool.GameEntityRelationEventParams(this, prevNode, CurrentSubNode));
        }

        public void RemoveNode(IGameEntityBridge p_Node)
        {
            if (_SubNodeGroup.Remove(p_Node))
            {
                p_Node.RemoveReceiver(_SubNodeEventReceiver);
                _MasterNodeEventHandler.Invoke(GameEntityRelationTool.GameEntityRelationEventType.SubNodeRemoved, new GameEntityRelationTool.GameEntityRelationEventParams(this, p_Node));
                
                if (ReferenceEquals(CurrentSubNode, p_Node))
                {
                    SelectNode(null);
                }
            }
        }

        public void ResetNode()
        {
            if (_SubNodeGroup.CheckGenericCollectionSafe())
            {
                foreach (var subNode in _SubNodeGroup)
                {
                    _MasterNodeEventHandler.Invoke(GameEntityRelationTool.GameEntityRelationEventType.SubNodeRemoved, new GameEntityRelationTool.GameEntityRelationEventParams(this, subNode));
                }
                _SubNodeGroup.Clear();
         
                SelectNode(null);
            }
            
            _SubNodeEventReceiver.ClearSender();
        }

        public HashSet<IGameEntityBridge> GetSubNodes()
        {
            return _SubNodeGroup;
        }

        #endregion

        #region <Disposable>

        /// <summary>
        /// dispose 패턴 onceFlag
        /// </summary>
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// dispose 플래그를 초기화 시키는 메서드
        /// </summary>
        public void Rejuvenate()
        {
            IsDisposed = false;
        }

        /// <summary>
        /// 인스턴스 파기 메서드
        /// </summary>
        public void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }
            else
            {
                IsDisposed = true;
                OnDisposeUnmanaged();
            }
        }

        #endregion
    }
}