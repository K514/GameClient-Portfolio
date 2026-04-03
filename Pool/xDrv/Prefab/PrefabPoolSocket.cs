using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using xk514;

namespace k514
{
    /// <summary>
    /// 다수의 동일한 프리팹을 제어하는 풀을 키 단위로 구분하여 보유하는 클래스
    /// </summary>
    public class PrefabPoolSocket<Key, Content, CreateParams, ActivateParams> : IPrefabPoolSocket<Key, Content, CreateParams, ActivateParams>
        where Content : PrefabPoolContent<Content, CreateParams, ActivateParams>
        where CreateParams : IPrefabCreateParams<CreateParams>
        where ActivateParams : IPrefabActivateParams
    {
        #region <Finalizer>

        ~PrefabPoolSocket()
        {
            Dispose();
        }

        #endregion
        
        #region <Fields>

        /// <summary>
        /// [키, 오브젝트 풀]
        /// </summary>
        private Dictionary<Key, PrefabPool<Content, CreateParams, ActivateParams>> _poolSocket;
        
        /// <summary>
        /// [키, 풀 생성 Task]
        /// </summary>
        private Dictionary<Key, UniTask<PrefabPool<Content, CreateParams, ActivateParams>>> _poolLoadTasks;
        
        private Func<Key, bool> IsAvailablePop;
        private Action<Key> OnPoolAdd;
        private Action<Key> OnPoolRemove;
        
        #endregion

        #region <Indexer>

        public PrefabPool<Content, CreateParams, ActivateParams> this[Key p_Key] => _poolSocket[p_Key];

        #endregion
        
        #region <Constructor>

        public PrefabPoolSocket(Func<Key, bool> p_IsAvailablePop = null, Action<Key> p_OnPoolAdd = null, Action<Key> p_OnPoolRemove = null)
        {
            _poolSocket = new Dictionary<Key, PrefabPool<Content, CreateParams, ActivateParams>>();
            _poolLoadTasks = new Dictionary<Key, UniTask<PrefabPool<Content, CreateParams, ActivateParams>>>();
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
                _poolSocket = null;
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
            return _poolSocket.ContainsKey(p_Key);
        }
        
        public bool TryGetPool(Key p_Key, out PrefabPool<Content, CreateParams, ActivateParams> o_Pool)
        {
            return _poolSocket.TryGetValue(p_Key, out o_Pool);
        }
        
        /// <summary>
        /// 특정 키에 대응하는 풀을 추가하는 메서드
        /// </summary>
        public bool TryAddPool(Transform p_Affine, Key p_Key, CreateParams p_CreateParams, int p_PreloadCount)
        {
            if (IsDisposed || _poolSocket.ContainsKey(p_Key))
            {
                return false;
            }
            else
            {
                var pool = PrefabPool<Content, CreateParams, ActivateParams>.GetPool(p_CreateParams, p_Affine);
                _poolSocket.Add(p_Key, pool);
                pool.Preload(p_PreloadCount);
                OnPoolAdd?.Invoke(p_Key);

                return true;
            }
        }

        /// <summary>
        /// 특정 키에 대응하는 풀을 추가하는 메서드
        /// </summary>
        public async UniTask<bool> TryAddPool(Transform p_Affine, Key p_Key, CreateParams p_CreateParams, int p_PreloadCount, CancellationToken p_CancellationToken)
        {
            if (IsDisposed || _poolSocket.ContainsKey(p_Key))
            {
                return false;
            }
            else
            {
                if (_poolLoadTasks.TryGetValue(p_Key, out var o_Task))
                {
                    await o_Task;
                    
                    return false;
                }
                else
                {
                    var loadTask = PrefabPool<Content, CreateParams, ActivateParams>.GetPool(p_CreateParams, p_Affine, p_CancellationToken);
                    _poolLoadTasks.Add(p_Key, loadTask);
                    var prefabPool = await loadTask;
                    _poolLoadTasks.Remove(p_Key);
                    _poolSocket.Add(p_Key, prefabPool);
                    prefabPool.Preload(p_PreloadCount);
                    OnPoolAdd?.Invoke(p_Key);
                    
                    return true;
                }
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
        public Content Pop(Key p_Key, ActivateParams p_SpawnParams)
        {
            if(IsAvailablePop?.Invoke(p_Key) ?? true)
            {
                var targetPool = _poolSocket[p_Key];
                return targetPool.Pop(p_SpawnParams);
            }
            else
            {
                return null;
            }
        }
        
        /// <summary>
        /// 지정한 풀로부터 오브젝트를 가져오는 메서드
        /// </summary>
        public Return Pop<Return>(Key p_Key, ActivateParams p_SpawnParams) where Return : Content
        {
            if(IsAvailablePop?.Invoke(p_Key) ?? true)
            {
                var targetPool = _poolSocket[p_Key];
                return targetPool.Pop(p_SpawnParams) as Return;
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
                var createParams = pool.GetCreateParams();
                CustomDebug.LogError($"[Key : {poolKey}] [{createParams.ModelData.TableRecord?.PrefabName}] [{createParams.AssetLoadKey.ResourceLifeCycleType}] (Count : {pool.GetContentCount()})");
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