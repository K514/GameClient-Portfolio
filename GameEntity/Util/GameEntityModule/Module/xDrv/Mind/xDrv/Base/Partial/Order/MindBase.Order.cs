using System.Collections.Generic;
using UnityEngine;

namespace k514.Mono.Common
{
    public partial class MindBase
    {
        #region <Fields>

        /// <summary>
        /// 오더 풀
        /// </summary>
        private ObjectPool<MindTool.MindOrder, MindTool.MindOrderCreateParams, MindTool.MindOrderActivateParams> _OrderPool;        
        
        /// <summary>
        /// 오더 큐
        /// </summary>
        private Queue<MindTool.MindOrder> _OrderQueue;
        
        /// <summary>
        /// 현재 오더
        /// </summary>
        protected MindTool.MindOrder _CurrentOrder;

        /// <summary>
        /// 현재 오더가 없는지 검증하는 논리 프로퍼티
        /// </summary>
        protected bool IsOrderEmpty => _OrderQueue.Count < 1 && ReferenceEquals(null, _CurrentOrder);
        
        #endregion
        
        #region <Callbacks>

        private void OnCreateOrder()
        {
            _OrderPool = new ObjectPool<MindTool.MindOrder, MindTool.MindOrderCreateParams, MindTool.MindOrderActivateParams>(new MindTool.MindOrderCreateParams(Entity, this));
            _OrderQueue = new Queue<MindTool.MindOrder>();
        }
        
        private void OnAwakeOrder()
        {
            OnAwakeIdleOrder();
        }
        
        private void OnSleepOrder()
        {
            Entity.RemoveState(GameEntityTool.EntityStateType.DRIVE_ORDER);
            ReleaseCurrentOrder();
            _OrderQueue.Clear();
            _OrderPool.RetrieveAll();
        }

        #endregion
        
        #region <Methods>

        private bool ReserveOrder(MindTool.MindOrderReserveType p_Type, MindTool.MindOrderActivateParams p_Params)
        {
            switch (p_Type)
            {
                default:
                case MindTool.MindOrderReserveType.Add:
                case MindTool.MindOrderReserveType.Add_RelayCancel:
                {
                    var order = _OrderPool.Pop(p_Params);
                    _OrderQueue.Enqueue(order);
                    order.OnAdded(p_Type);
                    Entity.AddState(GameEntityTool.EntityStateType.DRIVE_ORDER);
                    return true;
                }
                case MindTool.MindOrderReserveType.Overlap:
                {
                    ClearOrderQueue(false);
                    var order = _OrderPool.Pop(p_Params);
                    _OrderQueue.Enqueue(order);
                    order.OnAdded(p_Type);
                    Entity.AddState(GameEntityTool.EntityStateType.DRIVE_ORDER);
                    return true;
                }
                case MindTool.MindOrderReserveType.Instant:
                {
                    ClearOrderQueue(true);
                    var order = _OrderPool.Pop(p_Params);
                    _OrderQueue.Enqueue(order);
                    order.OnAdded(p_Type);
                    Entity.AddState(GameEntityTool.EntityStateType.DRIVE_ORDER);
                    return true;
                }
            }
        }

        private void DequeueNextOrder(MindTool.MindOrderPhase p_LatestOrderPhase)
        {
            if (_OrderQueue.Count > 0)
            {
                _CurrentOrder = _OrderQueue.Dequeue();
                _CurrentOrder.OnSelected(p_LatestOrderPhase);
            }
            else
            {
                Entity.RemoveState(GameEntityTool.EntityStateType.DRIVE_ORDER);
            }
        }

        private void ReleaseCurrentOrder()
        {
            if (!ReferenceEquals(null, _CurrentOrder))
            {
                _CurrentOrder.Pooling();
                _CurrentOrder = default;
            }
        }
        
        public void ClearOrderQueue(bool p_CancelCurrentOrder)
        {
            _OrderQueue.Clear();
            
            if (p_CancelCurrentOrder)
            {
                _CurrentOrder?.Terminate(false);
            }
        }
        
        #endregion
    }
}