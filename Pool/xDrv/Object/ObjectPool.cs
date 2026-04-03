using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using xk514;

namespace k514
{
    /// <summary>
    /// 오브젝트 풀
    /// </summary>
    public class ObjectPool<Content, CreateParams, ActivateParams> : PoolBase<Content, CreateParams, ActivateParams> 
        where Content : ObjectPoolContent<Content, CreateParams, ActivateParams>, new()
        where CreateParams : IObjectCreateParams
        where ActivateParams : IObjectActivateParams
    {
        #region <Constructor>
        
        public ObjectPool(CreateParams p_CreateParams = default) : base(p_CreateParams)
        {
        }

        #endregion

        #region <Callbacks>

        protected sealed override void OnCreateContent(Content p_Content)
        {
            p_Content.OnCreate(this);
        }
        
        protected sealed override bool OnActivateContent(Content p_Content, ActivateParams p_ActivateParams)
        {
            return p_Content.OnActivate(this, p_ActivateParams);
        }

        protected sealed override void OnRetrieveBegin(Content p_Content)
        {
            p_Content.OnRetrieveBegin(this);
        }

        protected sealed override void OnRetrieving(Content p_Content)
        {
            p_Content.OnRetrieving(this);
        }

        protected sealed override void OnRetrieveOver(Content p_Content)
        {
            p_Content.OnRetrieveOver(this);
        }
        
        #endregion
        
        #region <Methods>

        protected sealed override Content CreateContent(CreateParams p_CreateParams)
        {
            return ObjectPoolContent<Content, CreateParams, ActivateParams>.CreateContent(this, p_CreateParams);
        }

        #endregion
    }

    /// <summary>
    /// 풀 오브젝트
    /// </summary>
    public abstract class ObjectPoolContent<Content, CreateParams, ActivateParams> : IObjectContent<CreateParams>
        where Content : ObjectPoolContent<Content, CreateParams, ActivateParams>, new()
        where CreateParams : IObjectCreateParams
        where ActivateParams : IObjectActivateParams
    {
        #region <Finalizer>

        ~ObjectPoolContent()
        {
            Dispose();
        }

        #endregion

        #region <Consts>

        public static Content CreateContent(ObjectPool<Content, CreateParams, ActivateParams> p_Pool, CreateParams p_CreateParams)
        {
            var result = new Content { _Pool = p_Pool };
            return result;
        }

        #endregion
        
        #region <Fields>

        public PoolTool.ContentState ContentState { get; private set; }
        public bool IsContentActive => ContentState == PoolTool.ContentState.Active;
        public bool IsContentPooled => ContentState == PoolTool.ContentState.Pooled;
        private ObjectPool<Content, CreateParams, ActivateParams> _Pool;
        private int _pooledCount;

        #endregion

        #region <Callbacks>

        public void OnCreate(ObjectPool<Content, CreateParams, ActivateParams> p_Pool)
        {
            if (ReferenceEquals(_Pool, p_Pool) && ContentState == PoolTool.ContentState.None)
            {
                ContentState = PoolTool.ContentState.Created;
                
                try
                {
                    OnCreate(_Pool.GetCreateParams());
                }
#if APPLY_PRINT_LOG
                catch (Exception e)
                {
                    CustomDebug.LogError((this, "OnCreateError!", e, Color.red));
                    Dispose();
                }
#else
                catch
                {
                    Dispose();
                }
#endif
            }
        }

        public bool OnActivate(ObjectPool<Content, CreateParams, ActivateParams> p_Pool, ActivateParams p_ActivateParams)
        {
            if (ReferenceEquals(_Pool, p_Pool))
            {
                switch (ContentState)
                {
                    default:
                    {
                        return false;
                    }
                    case PoolTool.ContentState.Created:
                    {
                        ContentState = PoolTool.ContentState.Active;
                        _pooledCount++;

                        try
                        {
                            return OnActivate(_Pool.GetCreateParams(), p_ActivateParams, false);
                        }
#if APPLY_PRINT_LOG
                        catch (Exception e)
                        {
                            CustomDebug.LogError((this, "OnActivateError!", e, Color.red));
                            return false;
                        }
#else
                        catch
                        {
                            return false;
                        }
#endif
                        break;
                    }
                    case PoolTool.ContentState.Pooled:
                    {
                        ContentState = PoolTool.ContentState.Active;
                        _pooledCount++;

                        try
                        {
                            return OnActivate(_Pool.GetCreateParams(), p_ActivateParams, true);
                        }
#if APPLY_PRINT_LOG
                        catch (Exception e)
                        {
                            CustomDebug.LogError((this, "OnActivateError!", e, Color.red));
                            return false;
                        }
#else
                        catch
                        {
                            return false;
                        }
#endif
                        break;
                    }
                }
            }
            else
            {
                return false;
            }
        }

        public void OnRetrieveBegin(ObjectPool<Content, CreateParams, ActivateParams> p_Pool)
        {
            if (ReferenceEquals(_Pool, p_Pool))
            {
                switch (ContentState)
                {
                    case PoolTool.ContentState.Created:
                    case PoolTool.ContentState.Active:
                        ContentState = PoolTool.ContentState.Retrieving;
                        break;
                }
            }
        }

        public void OnRetrieving(ObjectPool<Content, CreateParams, ActivateParams> p_Pool)
        {
            if (ReferenceEquals(_Pool, p_Pool) && ContentState == PoolTool.ContentState.Retrieving)
            {
                try
                {
                    OnRetrieve(_Pool.GetCreateParams());
                }
#if APPLY_PRINT_LOG
                catch (Exception e)
                {
                    CustomDebug.LogError((this, "OnRetrieveError!", e, Color.red));
                    Dispose();
                }
#else
                catch
                {
                    Dispose();
                }
#endif
            }
        }

        public void OnRetrieveOver(ObjectPool<Content, CreateParams, ActivateParams> p_Pool)
        {
            if (ReferenceEquals(_Pool, p_Pool) && ContentState == PoolTool.ContentState.Retrieving)
            {
                ContentState = PoolTool.ContentState.Pooled;
            }
        }

        protected abstract void OnCreate(CreateParams p_CreateParams);
        protected abstract bool OnActivate(CreateParams p_CreateParams, ActivateParams p_ActivateParams, bool p_IsPooled);
        protected abstract void OnRetrieve(CreateParams p_CreateParams);
        protected abstract void OnDispose();

        /// <summary>
        /// 인스턴스가 파기될 때 수행할 작업을 기술한다.
        /// </summary>
        protected void OnDisposeUnmanaged()
        {
            switch (ContentState)
            {
                case PoolTool.ContentState.None:
                case PoolTool.ContentState.Created:
                case PoolTool.ContentState.Active:
                case PoolTool.ContentState.Pooled:
                case PoolTool.ContentState.Retrieving:
                {
                    ContentState = PoolTool.ContentState.Disposed;

                    Pooling();
                    OnDispose();

                    break;
                }
                case PoolTool.ContentState.Disposed :
                    break;
            }
        }

        #endregion
        
        #region <Methods>

        /// <summary>
        /// 생성 파라미터를 리턴하는 메서드
        /// </summary>
        public CreateParams GetCreateParams() => _Pool.GetCreateParams();

        /// <summary>
        /// 해당 오브젝트가 오브젝트 풀러 이외의 방법으로 생성된 경우, 생성 콜백을 수동 호출해주는 메서드
        /// </summary>
        public void CheckAwake(CreateParams p_CreateParams = default)
        {
            if (ContentState == PoolTool.ContentState.None)
            {
                ContentState = PoolTool.ContentState.Active;
                OnCreate(p_CreateParams);
                OnActivate(p_CreateParams, default, false);
            }
        }
        
        public int GetPooledCount()
        {
            return _pooledCount;
        }

        public void DisconnectPool()
        {
            if (!ReferenceEquals(null, _Pool))
            {
                var _pool = _Pool;
                _Pool = null;
                _pool.DisconnectContent(this as Content);
            }
        }

        public void Pooling()
        {
            _Pool?.Retrieve(this as Content);
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