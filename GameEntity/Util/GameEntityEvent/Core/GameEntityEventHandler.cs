using System;

namespace k514.Mono.Common
{
    public interface IGameEntityEventHandler<out Key, CreateParams, in ActivateParams> : _IDisposable
        where CreateParams : IGameEntityEventHandlerCreateParams<Key>
        where ActivateParams : IGameEntityEventHandlerActivateParams
    {
        Key EventId { get; }
        IGameEntityBridge Entity { get; }
        void OnCreate(IObjectContent<CreateParams> p_Wrapper, CreateParams p_CreateParams);
        bool OnActivate(CreateParams p_CreateParams, ActivateParams p_ActivateParams, bool p_IsPooled);
        void OnRetrieve(CreateParams p_CreateParams);
        bool IsEnterable();
        void PreloadEvent();
        void Pooling();
    }
    
    public interface IGameEntityEventHandlerCreateParams<out Key> : IObjectCreateParams
    {
        Key EventId { get; }
        Type HandlerType { get; }
    }
    
    public interface IGameEntityEventHandlerActivateParams : IObjectActivateParams
    {
        IGameEntityBridge Entity { get; }
    }
    
    public abstract class GameEntityEventHandlerBase<This, Key, CreateParams, ActivateParams> : IGameEntityEventHandler<Key, CreateParams, ActivateParams>
        where This : GameEntityEventHandlerBase<This, Key, CreateParams, ActivateParams>, new()
        where CreateParams : IGameEntityEventHandlerCreateParams<Key>
        where ActivateParams : IGameEntityEventHandlerActivateParams
    {
        ~GameEntityEventHandlerBase()
        {
            Dispose();
        }

        #region <Fields>

        public Key EventId { get; private set; }
        public IGameEntityBridge Entity { get; private set; }
        private IObjectContent<CreateParams> _Wrapper;
        protected ActivateParams _ActivateParams;

        #endregion
        
        #region <Callbacks>

        public virtual void OnCreate(IObjectContent<CreateParams> p_Wrapper, CreateParams p_CreateParams)
        {
            EventId = p_CreateParams.EventId;
            
            _Wrapper = p_Wrapper;
        }

        public virtual bool OnActivate(CreateParams p_CreateParams, ActivateParams p_ActivateParams, bool p_IsPooled)
        {
            Entity = p_ActivateParams.Entity;
            _ActivateParams = p_ActivateParams;
            
            return !ReferenceEquals(null, Entity);
        }

        public virtual void OnRetrieve(CreateParams p_CreateParams)
        {
        }
        
        protected virtual void OnDisposeUnmanaged()
        {
        }

        #endregion

        #region <Methods>

        protected bool IsHandlerValid()
        {
            return _Wrapper.IsContentValid();
        }
        
        public abstract bool IsEnterable();
        public abstract void PreloadEvent();
        
        public void Pooling()
        {
            _Wrapper.Pooling();
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
