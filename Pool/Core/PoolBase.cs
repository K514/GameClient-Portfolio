using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using xk514;

namespace k514
{
    /// <summary>
    /// 풀 기저 구현체
    /// </summary>
    public abstract class PoolBase<Content, CreateParams, ActivateParams> : IPool<Content, CreateParams, ActivateParams> 
        where Content : IContent
        where CreateParams : ICreateParams
        where ActivateParams : IActivateParams
    {
        #region <Finalizer>

        /// <summary>
        /// 소멸자
        /// </summary>
        ~PoolBase()
        {
            Dispose();
        }

        #endregion
        
        #region <Fields>

        private readonly CreateParams _createParams;
        private readonly List<Content> _activeContents;
        private readonly List<Content> _pooledContents;
        
        #endregion

        #region <Indexer>

        public Content this[int p_Index] => _activeContents[p_Index];

        #endregion
        
        #region <Constructors>

        protected PoolBase(CreateParams p_CreateParams)
        {
            _createParams = p_CreateParams;
            _activeContents = new List<Content>(GameConst.__CAPACITY_POOL);
            _pooledContents = new List<Content>(GameConst.__CAPACITY_POOL);
        }
        
        #endregion

        #region <Callbacks>
        
        /// <summary>
        /// Content가 Pop된 경우 호출되는 콜백
        /// </summary>
        private void OnPopContent(Content p_Content)
        {
            _activeContents.Add(p_Content);
            
            if (p_Content.GetPooledCount() < 1)
            {
                OnCreateContent(p_Content);
            }
        }

        /// <summary>
        /// Content가 신규 생성된 경우 호출되는 콜백
        /// </summary>
        protected abstract void OnCreateContent(Content p_Content);

        /// <summary>
        /// Content가 활성화되어야 하는 경우 호출되는 콜백
        /// </summary>
        protected abstract bool OnActivateContent(Content p_Content, ActivateParams p_ActivateParams);

        /// <summary>
        /// Content가 비활성화되어야 하는 경우 호출되는 콜백
        /// </summary>
        private void OnRetrieveContent(Content p_Content)
        {
            OnRetrieveBegin(p_Content);
            _activeContents.Remove(p_Content);
            OnRetrieving(p_Content);
            _pooledContents.Add(p_Content);
            OnRetrieveOver(p_Content);
        }

        /// <summary>
        /// Content 회수 로직 시작 콜백
        /// </summary>
        protected abstract void OnRetrieveBegin(Content p_Content);

        /// <summary>
        /// Content 회수 로직 메인 콜백
        /// </summary>
        protected abstract void OnRetrieving(Content p_Content);

        /// <summary>
        /// Content 회수 로직 종료 콜백
        /// </summary>
        protected abstract void OnRetrieveOver(Content p_Content);
        
        /// <summary>
        /// 풀이 파기될 때 수행할 작업을 기술한다.
        /// </summary>
        protected virtual void OnDisposeUnmanaged()
        {
            ClearPool();
        }

        #endregion
        
        #region <Methods>

        public CreateParams GetCreateParams()
        {
            return _createParams;
        }

        /// <summary>
        /// 상한을 넘지 않도록 풀에 Content를 신규 생성하여 격납시키는 메서드
        /// </summary>
        public void Preload(int p_LoadCount)
        {
            var currentPoolCapacity = GetContentCount();
            var preloadNumber = Mathf.Max(0, p_LoadCount - currentPoolCapacity);

            for (var i = 0; i < preloadNumber; i++)
            {
                var result = CreateContent(_createParams);
                if (ReferenceEquals(null, result))
                {
                    break;
                }
                else
                {
                    OnPopContent(result); 
                    Retrieve(result);
                }
            }
        }

        /// <summary>
        /// 풀로부터 Content를 가져오거나, 가져올 수 없다면 새로 생성하여 리턴하는 메서드
        /// </summary>
        protected Content PopContent()
        {
            var result = default(Content);
            var cnt = _pooledContents.Count;
            if (cnt > 0)
            {
#if APPLY_PRINT_LOG
                if (CustomDebug.CustomDebugLogFlag.PrintObjectPoolLog.HasOpen())
                {
                    CustomDebug.Log((this, $"Pooled Contents Pop"));
                }
#endif
                var lastIndex = cnt - 1;
                result = _pooledContents[lastIndex];
                _pooledContents.RemoveAt(lastIndex);
#if APPLY_PRINT_LOG
                if (CustomDebug.CustomDebugLogFlag.PrintObjectPoolLog.HasOpen(result.IsDisposed))
                {
                    CustomDebug.LogError((this, $"파기된 Content의 재활성화가 감지되었습니다. 오브젝트 : [{typeof(Content)}]"));
                }
#endif
                return result;
            }
            else
            {
#if APPLY_PRINT_LOG
                if (CustomDebug.CustomDebugLogFlag.PrintObjectPoolLog.HasOpen())
                {
                    CustomDebug.Log((this, $"New Contents Pop"));
                }
#endif
                result = CreateContent(_createParams);
                return result;
            }
        }

        /// <summary>
        /// Content를 생성하여 풀에 등록하는 메서드
        /// </summary>
        protected abstract Content CreateContent(CreateParams p_CreateParams);

        /// <summary>
        /// 풀에서 제어하는 Content 하나를 활성화시켜 리턴하는 메서드
        /// </summary>
        public virtual Content Pop(ActivateParams p_ActivateParams)
        {
            var result = PopContent();
            if (ReferenceEquals(null, result))
            {
                return default;
            }
            else
            {
                OnPopContent(result);
                if (OnActivateContent(result, p_ActivateParams))
                {
                    return result;
                }
                else
                {
                    Retrieve(result);
                    return default;
                }
            }
        }

        /// <summary>
        /// 지정한 Content를 비활성화시키고 풀로 회수시키는 메서드
        /// </summary>
        public void Retrieve(Content p_Content)
        {
            switch (p_Content.ContentState)
            {
                case PoolTool.ContentState.Created:
                case PoolTool.ContentState.Active:
                {
                    OnRetrieveContent(p_Content);
                    break;
                }
                case PoolTool.ContentState.Pooled:
                case PoolTool.ContentState.Retrieving:
                {
                    break;
                }
                case PoolTool.ContentState.Disposed:
                {
                    DisconnectContent(p_Content);
                    break;
                }
            }
        }

        /// <summary>
        /// 풀에서 관리하는 모든 Content를 비활성화시키고 풀로 회수시키는 메서드
        /// </summary>
        public void RetrieveAll()
        {
#if APPLY_PRINT_LOG
            if (CustomDebug.CustomDebugLogFlag.PrintObjectPoolLog.HasOpen())
            {
                CustomDebug.Log((this, $"Retrieve all Content"));
            }
#endif
            for (var i = _activeContents.Count - 1; i > -1; i--)
            {
                Retrieve(_activeContents[i]);
            }
            
            _activeContents.Clear();
        }

        /// <summary>
        /// 풀에서 관리하는 모든 Content를 비활성화시키고 풀로 회수시킨 후,
        /// 모든 Content를 파기시키는 메서드
        /// </summary>
        public virtual void ClearPool()
        {
#if APPLY_PRINT_LOG
            if (CustomDebug.CustomDebugLogFlag.PrintObjectPoolLog.HasOpen())
            {
                CustomDebug.Log((this, $"Clear Pool"));
            }
#endif
            RetrieveAll();
            _activeContents.Clear();

            for (var i = _pooledContents.Count - 1; i > -1; i--)
            {
                _pooledContents[i].Dispose();
            }

            _pooledContents.Clear();
        }

        /// <summary>
        /// 현재 활성화중인 Content 리스트를 리턴하는 메서드
        /// </summary>
        public List<Content> GetActiveEnumerator()
        {
            return _activeContents;
        }

        /// <summary>
        /// 지정한 Content의 인덱스를 리턴한다.
        /// </summary>
        public int GetIndex(Content p_Content) => _activeContents.IndexOf(p_Content);

        /// <summary>
        /// 현재 풀에서 관리하는 Content의 갯수를 리턴하는 메서드
        /// </summary>
        public int GetContentCount()
        {
            return _pooledContents.Count + _activeContents.Count;
        }
        
        /// <summary>
        /// 현재 활성화된 Content의 갯수를 리턴하는 메서드
        /// </summary>
        public int GetActivateContentCount()
        {
            return _activeContents.Count;
        }

        /// <summary>
        /// 지정한 Content를 해당 풀로부터 제외하는 메서드
        /// </summary>
        public void DisconnectContent(Content p_Content)
        {
            p_Content.DisconnectPool();
            _activeContents.Remove(p_Content);
            _pooledContents.Remove(p_Content);
        }

#if APPLY_PRINT_LOG
        public void PrintPool()
        {
            {
                if (_activeContents.Count > 0)
                {
                    CustomDebug.LogError("** Active **");
                    var cnt = 0;
                    foreach (var content in _activeContents)
                    {
                        CustomDebug.LogError($"[{cnt++} : {content.GetType()} (Active)]");
                    } 
                    CustomDebug.LogError("************");
                }
            }
            {
                if (_pooledContents.Count > 0)
                {
                    CustomDebug.LogError("** Pooled **");
                    var cnt = 0;
                    foreach (var content in _pooledContents)
                    {
                        CustomDebug.LogError($"[{cnt++} : {content.GetType()} (Pooled)]");
                    } 
                    CustomDebug.LogError("************");
                }
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