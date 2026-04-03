using System;
using System.Collections.Generic;
using xk514;

namespace k514
{
    /// <summary>
    /// 다수의 동일한 오브젝트를 제어하는 풀을 키 단위로 구분하여 보유하는 클래스
    /// </summary>
    public class ObjectPoolSocket<Key, Content, CreateParams, ActivateParams> : IObjectPoolSocket<Key, Content, CreateParams, ActivateParams>
        where Content : ObjectPoolContent<Content, CreateParams, ActivateParams>, new()
        where CreateParams : IObjectCreateParams
        where ActivateParams : IObjectActivateParams
    {
        #region <Finalizer>

        ~ObjectPoolSocket()
        {
            Dispose();
        }

        #endregion
        
        #region <Fields>

        /// <summary>
        /// [키, 오브젝트 풀]
        /// </summary>
        private Dictionary<Key, ObjectPool<Content, CreateParams, ActivateParams>> _poolSocket;

        private Func<Key, bool> IsAvailablePop;
        private Action<Key> OnPoolAdd;
        private Action<Key> OnPoolRemove;
        
        #endregion

        #region <Indexer>

        public ObjectPool<Content, CreateParams, ActivateParams> this[Key p_Key] => _poolSocket[p_Key];

        #endregion
        
        #region <Constructor>

        public ObjectPoolSocket(Func<Key, bool> p_IsAvailablePop = null, Action<Key> p_OnPoolAdd = null, Action<Key> p_OnPoolRemove = null)
        {
            _poolSocket = new Dictionary<Key, ObjectPool<Content, CreateParams, ActivateParams>>();
            IsAvailablePop = p_IsAvailablePop;
            OnPoolAdd = p_OnPoolAdd;
            OnPoolRemove = p_OnPoolRemove;
        }
        
        #endregion

        #region <Callbacks>
        
        /// <summary>
        /// 인스턴스가 파기될 때 수행할 작업을 기술한다.
        /// </summary>
        protected void OnDisposeUnmanaged()
        {
            if (_poolSocket.CheckCollectionSafe())
            {
                foreach (var poolKV in _poolSocket)
                {
                    var pool = poolKV.Value;
                    pool.Dispose();
                }
                _poolSocket.Clear();
            }

            _poolSocket = null;
        }

        #endregion
          
        #region <Methods>

        public bool HasKey(Key p_Key)
        {
            return _poolSocket.ContainsKey(p_Key);
        }
        
        public bool TryGetPool(Key p_Key, out ObjectPool<Content, CreateParams, ActivateParams> o_Pool)
        {
            return _poolSocket.TryGetValue(p_Key, out o_Pool);
        }
        
        /// <summary>
        /// 특정 키에 대응하는 풀을 추가하는 메서드
        /// </summary>
        public bool TryAddPool(Key p_Key, CreateParams p_CreateParams, int p_PreloadCount)
        {
            if (IsDisposed || _poolSocket.ContainsKey(p_Key))
            {
                return false;
            }
            else
            {
                var pool = new ObjectPool<Content, CreateParams, ActivateParams>(p_CreateParams);
                _poolSocket.Add(p_Key, pool);
                pool.Preload(p_PreloadCount);
                OnPoolAdd?.Invoke(p_Key);

                return true;
            }
        }

        /// <summary>
        /// 특정 키에 대응하는 풀을 제거하는 메서드
        /// </summary>
        public bool RemovePool(Key p_Key)
        {
            if (IsDisposed || !_poolSocket.Remove(p_Key, out var o_Pool))
            {
                return false;
            }
            else
            {
                OnPoolRemove?.Invoke(p_Key);
                o_Pool.Dispose();
                
                return true;
            }
        }
        
        /// <summary>
        /// 지정한 풀에 프리로드하는 메서드
        /// </summary>
        public void Preload(Key p_Key, int p_LoadCount)
        {
            var targetPool = _poolSocket[p_Key];
            targetPool.Preload(p_LoadCount);
        }

        /// <summary>
        /// 지정한 풀로부터 오브젝트를 가져오는 메서드
        /// </summary>
        public Content Pop(Key p_Key, ActivateParams p_ActivateParams)
        {
            if(IsAvailablePop?.Invoke(p_Key) ?? true)
            {
                var targetPool = _poolSocket[p_Key];
                return targetPool.Pop(p_ActivateParams);
            }
            else
            {
                return null;
            }
        }
        
        /// <summary>
        /// 지정한 풀로부터 오브젝트를 가져오는 메서드
        /// </summary>
        public Return Pop<Return>(Key p_Key, ActivateParams p_ActivateParams) where Return : Content
        {
            if(IsAvailablePop?.Invoke(p_Key) ?? true)
            {
                var targetPool = _poolSocket[p_Key];
                return targetPool.Pop(p_ActivateParams) as Return;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 지정한 풀로부터 오브젝트를 전부 회수하는 메서드
        /// </summary>
        public void RetrieveAll(Key p_Key)
        {
            var targetPool = _poolSocket[p_Key];
            targetPool.RetrieveAll();
        }
        
        /// <summary>
        /// 해당 풀 소켓의 모든 오브젝트를 회수하는 메서드
        /// </summary>
        public void RetrieveAll()
        {
            foreach (var poolKV in _poolSocket)
            {
                RetrieveAll(poolKV.Key);
            }
        }
        
        /// <summary>
        /// 지정한 풀을 초기화시키는 메서드
        /// </summary>
        public void ClearPool(Key p_Key)
        {
            var targetPool = _poolSocket[p_Key];
            targetPool.ClearPool();
        }

        /// <summary>
        /// 해당 풀 소켓의 모든 풀을 초기화 시키는 메서드
        /// </summary>
        public void ClearPool()
        {
            foreach (var poolKV in _poolSocket)
            {
                ClearPool(poolKV.Key);
            }
        }
        
        /// <summary>
        /// 해당 풀 소켓의 모든 풀의 활성화된 컨텐츠를 리턴하는 메서드
        /// </summary>
        public void GetActivateList(ref List<Content> r_Result)
        {
            r_Result.Clear();
            
            foreach (var poolKV in _poolSocket)
            {
                var pool = poolKV.Value;
                r_Result.AddRange(pool.GetActiveEnumerator());
            }
        }

#if APPLY_PRINT_LOG
        public void PrintPool()
        {
            foreach (var poolKV in _poolSocket)
            {
                var poolKey = poolKV.Key;
                var pool = poolKV.Value;
                CustomDebug.LogError($"[Key : {poolKey}] (Count : {pool.GetContentCount()})");
                pool.PrintPool();
            }
        }
#endif

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