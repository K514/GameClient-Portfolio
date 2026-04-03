using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using xk514;

namespace k514
{
    /// <summary>
    /// 다수의 상이한 프리팹을 제어하는 풀을 키 단위로 구분하여 보유하는 클래스
    /// </summary>
    public class PrefabPoolCluster<Key, CreateParams, ActivateParams> : IPrefabPoolCluster<Key, CreateParams, ActivateParams>
        where CreateParams : IPrefabCreateParams<CreateParams>
        where ActivateParams : IPrefabActivateParams
    {
        #region <Finalizer>

        ~PrefabPoolCluster()
        {
            Dispose();
        }

        #endregion
        
        #region <Fields>

        /// <summary>
        /// [키, 오브젝트 풀]
        /// </summary>
        private Dictionary<Key, IPool> _poolCluster;
        
        /// <summary>
        /// [키, 풀 생성 Task]
        /// </summary>
        private Dictionary<Key, UniTask<IPool>> _poolLoadTasks;

        private Func<Key, bool> IsAvailablePop;
        private Action<Key> OnPoolAdd;
        private Action<Key> OnPoolRemove;
        
        #endregion

        #region <Indexer>

        public IPool this[Key p_Key] => _poolCluster[p_Key];

        #endregion
        
        #region <Constructor>

        public PrefabPoolCluster(Func<Key, bool> p_IsAvailablePop = null, Action<Key> p_OnPoolAdd = null, Action<Key> p_OnPoolRemove = null)
        {
            _poolCluster = new Dictionary<Key, IPool>();
            _poolLoadTasks = new Dictionary<Key, UniTask<IPool>>();
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
            if (_poolCluster.CheckCollectionSafe())
            {
                foreach (var poolKV in _poolCluster)
                {
                    var pool = poolKV.Value;
                    pool.Dispose();
                }
                _poolCluster.Clear();
                _poolCluster = null;
            }

            if (_poolLoadTasks is not null)
            {
                _poolLoadTasks.Clear();
                _poolLoadTasks = null;
            }
        }
 
        #endregion
          
        #region <Methods>

        public bool HasKey(Key p_Key)
        {
            return _poolCluster.ContainsKey(p_Key);
        }
        
        public bool TryGetPool(Key p_Key, out IPool o_Pool)
        {
            return _poolCluster.TryGetValue(p_Key, out o_Pool);
        }
        
        /// <summary>
        /// 특정 키에 대응하는 풀을 추가하는 메서드
        /// </summary>
        public bool TryAddPool(Key p_Key, IPool p_Pool, int p_PreloadCount)
        {
            if (IsDisposed || !_poolCluster.TryAdd(p_Key, p_Pool))
            {
                return false;
            }
            else
            {
                p_Pool.Preload(p_PreloadCount);
                OnPoolAdd?.Invoke(p_Key);
                
                return true;
            }
        }

        /// <summary>
        /// 특정 키에 대응하는 풀을 제거하는 메서드
        /// </summary>
        public bool RemovePool(Key p_Key)
        {
            if (IsDisposed || !_poolCluster.Remove(p_Key, out var o_Pool))
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
            var targetPool = _poolCluster[p_Key];
            targetPool.Preload(p_LoadCount);
        }

        /// <summary>
        /// 지정한 풀로부터 오브젝트를 가져오는 메서드
        /// </summary>
        public Return Pop<Return>(Key p_Key, ActivateParams p_ActivateParams) where Return : PrefabPoolContent<Return, CreateParams, ActivateParams>
        {
            if(IsAvailablePop?.Invoke(p_Key) ?? true)
            {
                var targetPool = _poolCluster[p_Key] as PrefabPool<Return, CreateParams, ActivateParams>;
                return targetPool.Pop(p_ActivateParams);
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
            var targetPool = _poolCluster[p_Key];
            targetPool.RetrieveAll();
        }
        
        /// <summary>
        /// 해당 풀 소켓의 모든 오브젝트를 회수하는 메서드
        /// </summary>
        public void RetrieveAll()
        {
            foreach (var poolKV in _poolCluster)
            {
                RetrieveAll(poolKV.Key);
            }
        }
        
        /// <summary>
        /// 지정한 풀을 초기화시키는 메서드
        /// </summary>
        public void ClearPool(Key p_Key)
        {
            var targetPool = _poolCluster[p_Key];
            targetPool.ClearPool();
        }

        /// <summary>
        /// 해당 풀 소켓의 모든 풀을 초기화 시키는 메서드
        /// </summary>
        public void ClearPool()
        {
            foreach (var poolKV in _poolCluster)
            {
                ClearPool(poolKV.Key);
            }
        }
 
#if APPLY_PRINT_LOG
        public void PrintPool()
        {
            foreach (var poolKV in _poolCluster)
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