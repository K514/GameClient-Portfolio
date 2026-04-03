using System.Collections.Generic;
using System.Threading;

namespace k514.Mono.Common
{
    public interface IGameEntityEventContainer : ICancellationTokenSource
    {
        /// <summary>
        /// 이벤트 생성 개체
        /// </summary>
        IGameEntityBridge Caster { get; }

        /// <summary>
        /// 이벤트 공통 파라미터
        /// </summary>
        public GameEntityEventCommonParams CommonParams { get; }

        /// <summary>
        /// 레코드 인덱스
        /// </summary>
        int EventId { get; }

        /// <summary>
        /// 갱신 콜백
        /// </summary>
        bool OnUpdate(float p_DeltaTime);
        
        /// <summary>
        /// 테이블 레코드를 리턴하는 메서드
        /// </summary>
        bool TryGetRecord<T>(out T o_Record) where T : class, ITableRecord;

        /// <summary>
        /// 기본 핸들러를 리턴하는 메서드
        /// </summary>
        GameEntityExtraOptionHandler AddDefaultHandler(GameEntityExtraOptionTool.GameEntityExtraOptionParams p_Params = default);
  
        /// <summary>
        /// 수명을 갖는 핸들러를 리턴하는 메서드
        /// </summary>
        GameEntityExtraOptionHandler AddDurationHandler(float p_Duration, GameEntityExtraOptionTool.GameEntityExtraOptionParams p_Params = default);
        
        /// <summary>
        /// 옵션 핸들러를 리턴하는 메서드
        /// </summary>      
        bool TryAddExtraOptionHandler(int p_Index, GameEntityExtraOptionTool.GameEntityExtraOptionParams p_Params, out GameEntityExtraOptionHandler o_Handler);
        
        /// <summary>
        /// 현재 컨테이너에 존재하는 p_Index 번째 핸들러를 리턴하는 메서드
        /// </summary>
        GameEntityExtraOptionHandler GetHandler(int p_Index);
    }

    public interface IGameEntityEventContainer<TableRecord> : IGameEntityEventContainer where TableRecord : class, ITableRecord
    {
        public TableRecord Record { get; }
    }
    
    public interface IGameEntityEventContainerActivateParams : IObjectActivateParams
    {
        IGameEntityBridge Caster { get; }
        int EventId { get; }
        GameEntityEventCommonParams CommonParams { get; }
    }
    
    public struct GameEntityEventCommonParams
    {
        #region <Fields>

        public int Count;
        public IGameEntityBridge Trigger;
        
        #endregion
            
        #region <Constructor>

        public GameEntityEventCommonParams(int p_Count)
        {
            this = default;
            
            Count = p_Count;
        }
        
        public GameEntityEventCommonParams(IGameEntityBridge p_Trigger)
        {
            this = default;
            
            Trigger = p_Trigger;
        }

        #endregion
    }
    
    public abstract class GameEntityEventContainer<This, CreateParams, ActivateParams, TableRecord> : ObjectPoolContent<This, CreateParams, ActivateParams>, IGameEntityEventContainer<TableRecord>
        where This : ObjectPoolContent<This, CreateParams, ActivateParams>, new() 
        where CreateParams : IObjectCreateParams 
        where ActivateParams : IGameEntityEventContainerActivateParams
        where TableRecord : class, ITableRecord
    {
        #region <Fields>

        private PhaseType _CurrentPhase;
        protected ActivateParams _ActivateParams { get; private set; }
        public IGameEntityBridge Caster => _ActivateParams.Caster;
        public int EventId => _ActivateParams.EventId;
        public GameEntityEventCommonParams CommonParams => _ActivateParams.CommonParams;
        public TableRecord Record { get; protected set; }
        private ObjectPool<GameEntityExtraOptionHandler, GameEntityExtraOptionHandlerCreateParam, GameEntityExtraOptionHandlerActivateParam> _ExtraOptionHandlerPool;
        private List<GameEntityExtraOptionHandler> _TerminatedHandlerGroup;
        protected CancellationToken _CancellationToken;
        protected CancellationTokenSource _CancellationTokenSource;
        
        #endregion
        
        #region <Callbacks>

        protected override void OnCreate(CreateParams p_CreateParams)
        {
            _ExtraOptionHandlerPool = new ObjectPool<GameEntityExtraOptionHandler, GameEntityExtraOptionHandlerCreateParam, GameEntityExtraOptionHandlerActivateParam>(new GameEntityExtraOptionHandlerCreateParam(this));
            _TerminatedHandlerGroup = new List<GameEntityExtraOptionHandler>();
        }

        protected override bool OnActivate(CreateParams p_CreateParams, ActivateParams p_ActivateParams, bool p_IsPooled)
        {
            _ActivateParams = p_ActivateParams;
            
            InitCancellationToken();

            return true;
        }

        protected override void OnRetrieve(CreateParams p_CreateParams)
        {
            CancelContainer();
            
            _TerminatedHandlerGroup.Clear();
            _ExtraOptionHandlerPool.RetrieveAll();

            _CurrentPhase = PhaseType.None;
            Record = default;

            if (!ReferenceEquals(null, _CancellationTokenSource))
            {
                _CancellationTokenSource.Cancel();
                _CancellationTokenSource = default;
                _CancellationToken = default;
            }
        }

        protected abstract void OnContainerTerminate();

        public virtual bool OnUpdate(float p_DeltaTime)
        {
            switch (_CurrentPhase)
            {
                default:
                case PhaseType.None:
                    return false;
                case PhaseType.Running:
                {
                    var poolEnumerator = _ExtraOptionHandlerPool.GetActiveEnumerator();
                    foreach (var handler in poolEnumerator)
                    {
                        if (handler.OnUpdate(p_DeltaTime))
                        {
                            _TerminatedHandlerGroup.Add(handler);
                        }
                    }

                    UpdateTerminatedHandler();

                    return CheckContainerTerminate();
                }
                case PhaseType.Terminate:
                    return true;
            }
        }
   
        protected override void OnDispose()
        {
        }

        #endregion

        #region <Methods>
                
        private void UpdateTerminatedHandler()
        {
            if (_TerminatedHandlerGroup.CheckCollectionSafe())
            {
                foreach (var eventHandler in _TerminatedHandlerGroup)
                {
                    eventHandler.Pooling();
                }
                _TerminatedHandlerGroup.Clear();
            }
        }

        protected virtual bool CheckContainerTerminate()
        {
            return _ExtraOptionHandlerPool.GetActivateContentCount() < 1;
        }
        
        protected abstract void InitCancellationToken();

        public void PreloadHandler()
        {
            var poolEnumerator = _ExtraOptionHandlerPool.GetActiveEnumerator();
            foreach (var handler in poolEnumerator)
            {
                handler.PreloadHandler();
            }   
        }
        
        public bool RunContainer()
        {
            switch (_CurrentPhase)
            {
                default:
                {
                    return false;
                }
                case PhaseType.None:
                {
                    _CurrentPhase = PhaseType.Running;
                    
                    var poolEnumerator = _ExtraOptionHandlerPool.GetActiveEnumerator();
                    foreach (var handler in poolEnumerator)
                    {
                        if (!handler.RunHandler())
                        {
                            _TerminatedHandlerGroup.Add(handler);
                        }
                    }   
                    
                    UpdateTerminatedHandler();

                    if (CheckContainerTerminate())
                    {
                        Pooling();
                        
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
        }
        
        public bool CancelContainer()
        {
            switch (_CurrentPhase)
            {
                default:
                {
                    _CurrentPhase = PhaseType.Terminate;

                    return false;
                }
                case PhaseType.Running:
                {
                    _CurrentPhase = PhaseType.Terminate;
                    
                    var poolEnumerator = _ExtraOptionHandlerPool.GetActiveEnumerator();
                    foreach (var handler in poolEnumerator)
                    {
                        handler.CancelHandler(false);
                        _TerminatedHandlerGroup.Add(handler);
                    }

                    UpdateTerminatedHandler();
                    OnContainerTerminate();
                    
                    return true;
                }
            }
        }
        
        public bool TryGetRecord<T>(out T o_Record) where T : class, ITableRecord
        {
            if (ReferenceEquals(null, Record))
            {
                o_Record = default;
                return false;
            }
            else
            {
                o_Record = Record as T;
                return !ReferenceEquals(null, o_Record);
            }
        }

        public GameEntityExtraOptionHandler AddDefaultHandler(GameEntityExtraOptionTool.GameEntityExtraOptionParams p_Params = default)
        {
            return _ExtraOptionHandlerPool.Pop(new GameEntityExtraOptionHandlerActivateParam(p_Params));
        }
        
        public GameEntityExtraOptionHandler AddDurationHandler(float p_Duration, GameEntityExtraOptionTool.GameEntityExtraOptionParams p_Params = default)
        {
            var handler = _ExtraOptionHandlerPool.Pop(new GameEntityExtraOptionHandlerActivateParam(p_Params));
            handler.SetLifeSpan(p_Duration);

            return handler;
        }

        public bool TryAddExtraOptionHandler(int p_Index, GameEntityExtraOptionTool.GameEntityExtraOptionParams p_Params, out GameEntityExtraOptionHandler o_Handler)
        {
            if (ExtraOptionDataTable.GetInstanceUnsafe.TryGetRecord(p_Index, out var o_Record))
            {
                if (GameEntityExtraOptionStorage.GetInstanceUnsafe.TryGetExtraOption(o_Record.ExtraOptionType, out var o_Option))
                {
                    o_Handler = _ExtraOptionHandlerPool.Pop(new GameEntityExtraOptionHandlerActivateParam(o_Record, o_Option, p_Params));

                    return true;
                }
            }

            o_Handler = default;
            return false;
        }

        public GameEntityExtraOptionHandler GetHandler(int p_Index)
        {
            var poolEnumerator = _ExtraOptionHandlerPool.GetActiveEnumerator();
            return poolEnumerator.GetElementSafe(p_Index);
        }

        public CancellationToken GetCancellationToken()
        {
            return _CancellationTokenSource.Token;
        }

        public void GetLinkedCancellationTokenSource(ref CancellationTokenSource r_Token)
        {
            if (r_Token.IsValid())
            {
                r_Token.Cancel();
            }

            r_Token = CancellationTokenSource.CreateLinkedTokenSource(_CancellationTokenSource.Token);
        }

        #endregion
    }
}