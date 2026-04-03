using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using xk514;

namespace k514.Mono.Common
{
    /// <summary>
    /// 게임 개체 모듈 클러스터 인터페이스
    /// </summary>
    public interface IGameEntityModuleCluster : _IDisposable
    {
        GameEntityModuleTool.ModuleType ModuleType { get; }
        IGameEntityModule CurrentModule { get; }
        IGameEntityModule GetNullModule();
        void OnActivateCluster();
        void OnRetrieveCluster();
        int GetModuleCount();
#if  APPLY_PRINT_LOG
        void PrintModuleClusterInfo();
#endif
    }

    /// <summary>
    /// 게임 개체는 동일한 모듈이라도 여러 종류를 가지며 필요에 따라 모듈을 스위칭하는데
    /// 해당 모듈들을 제어하는 배열 클래스
    /// </summary>
    public abstract partial class GameEntityModuleClusterBase<This, ModuleLabelType, TableBridge, RecordBridge, ModuleBase> : IGameEntityModuleCluster 
        where This : GameEntityModuleClusterBase<This, ModuleLabelType, TableBridge, RecordBridge, ModuleBase>
        where ModuleLabelType : struct, Enum
        where TableBridge : IGameEntityModuleDataTableBridge<RecordBridge>, ITableBridgeLabel<ModuleLabelType>
        where RecordBridge : IGameEntityModuleDataTableRecordBridge
        where ModuleBase : class, IGameEntityModule
    {
        #region <Consts>

        ~GameEntityModuleClusterBase()
        {
            Dispose();
        }

        protected static ModuleBase _NullModule;

        #endregion

        #region <Fields>

        /// <summary>
        /// 해당 모듈 클러스터를 포함하고 있는 게임 개체
        /// </summary>
        protected IGameEntityBridge _GameEntity;

        /// <summary>
        /// 해당 모듈 클러스터가 다루는 모듈 타입
        /// </summary>
        public GameEntityModuleTool.ModuleType ModuleType { get; protected set; }

        /// <summary>
        /// 모듈 쿼리
        /// </summary>
        private IMultiTable<int, TableMetaData, ModuleLabelType, TableBridge, RecordBridge> _ModuleTableQuery;

        /// <summary>
        /// [모듈 인덱스, 클러스터에 포함된 모듈] 컬렉션
        /// </summary>
        private Dictionary<int, ModuleBase> _ModuleIndexTable;

        /// <summary>
        /// [모듈 라벨, 클러스터에 포함된 모듈] 컬렉션
        /// </summary>
        private Dictionary<ModuleLabelType, List<ModuleBase>> _ModuleLabelTable;

        /// <summary>
        /// 현재 선정된 모듈
        /// </summary>
        public ModuleBase _DefaultModule, _CurrentModule;

        /// <summary>
        /// 현재 선정된 모듈 참조 프로퍼티
        /// </summary>
        public IGameEntityModule CurrentModule => _CurrentModule;
        
        #endregion

        #region <Constructor>

        protected GameEntityModuleClusterBase(IGameEntityBridge p_Entity, GameEntityModuleTool.ModuleType p_ModuleType, IMultiTable<int, TableMetaData, ModuleLabelType, TableBridge, RecordBridge> p_ModuleTableQuery)
        {
            _GameEntity = p_Entity;
            ModuleType = p_ModuleType;
            _ModuleTableQuery = p_ModuleTableQuery;
            _ModuleIndexTable = new Dictionary<int, ModuleBase>();
            _ModuleLabelTable = new Dictionary<ModuleLabelType, List<ModuleBase>>();
            
            _CurrentModule = _DefaultModule = _NullModule;
        }

        #endregion

        #region <Callbacks>

        public void OnActivateCluster()
        {
            SwitchDefaultModule();
        }

        public void OnRetrieveCluster()
        {
            SwitchNullModule();

            foreach (var moduleKV in _ModuleIndexTable)
            {
                var module = moduleKV.Value;
                module.ResetModule();
            }
        }
        
        private void OnModuleSwitched(ModuleBase p_Transition)
        {
            var currentNull = ReferenceEquals(_NullModule, _CurrentModule);
            var targetNull = ReferenceEquals(_NullModule, p_Transition);

            if (currentNull)
            {
                if (targetNull)
                {
                    // do nothing
                }
                else
                {
                    _CurrentModule = p_Transition;
                    _CurrentModule.AwakeModule();
                } 
            }
            else
            {
                if (targetNull)
                {
                    _CurrentModule.SleepModule();
                    _CurrentModule = _NullModule;
                }
                else
                {
                    _CurrentModule.SleepModule();
                    _CurrentModule = p_Transition;
                    _CurrentModule.AwakeModule();
                }
            }
        }

        /// <summary>
        /// 인스턴스가 파기될 때 수행할 작업을 기술한다.
        /// </summary>
        private void OnDisposeUnmanaged()
        {
            if (_ModuleIndexTable != null)
            {
                ResetCluster();

                _ModuleIndexTable = null;
                _ModuleLabelTable = null;
            }

            _ModuleTableQuery = null;
            ModuleType = default;
            _GameEntity = default;
        }

        #endregion
        
        #region <Methods>
        
        /// <summary>
        /// 현재 모듈이 Null 모듈인지 검증하는 메서드
        /// </summary>
        public bool IsNullModule()
        {
            return ReferenceEquals(_CurrentModule, _NullModule);
        }        
        
        /// <summary>
        /// Null 모듈을 리턴하는 메서드
        /// </summary>
        public IGameEntityModule GetNullModule()
        {
            return _NullModule;
        }
        
        /// <summary>
        /// 현재 모듈을 리턴하는 메서드
        /// </summary>
        public ModuleBase GetCurrentModule()
        {
            return _CurrentModule;
        }

        /// <summary>
        /// 해당 클러스터에 포함되어 있는 모듈 테이블을 리턴하는 메서드
        /// </summary>
        public Dictionary<int, ModuleBase> GetModuleTable() => _ModuleIndexTable;
        
        /// <summary>
        /// 클러스터에 등록된 모듈 갯수를 리턴하는 메서드
        /// </summary>
        public int GetModuleCount()
        {
            return _ModuleIndexTable.Count;
        }

        /// <summary>
        /// 지정한 인덱스 리스트로부터 모듈을 생성하여 등록하는 메서드
        /// </summary>
        public void InitModules(List<int> p_IndexList, ModuleLabelType p_FallbackType)
        {
            ResetCluster();
            
            if (p_IndexList.CheckCollectionSafe())
            {
                foreach (var moduleIndex in p_IndexList)
                {
                    var tryModule = AddModule(moduleIndex);
                    
                    if (ReferenceEquals(_DefaultModule, _NullModule))
                    {
                        _DefaultModule = tryModule;
                    }
                }
            }
            
            if (ReferenceEquals(_DefaultModule, _NullModule))
            {
                var tryModule = AddModule(p_FallbackType);
                _DefaultModule = tryModule;
            }
        }
        
        /// <summary>
        /// 클러스터를 초기화시키는 메서드
        /// </summary>
        public void ResetCluster()
        {
            SwitchNullModule();
            _DefaultModule = _NullModule;
            
            foreach (var moduleTableKV in _ModuleIndexTable)
            {
                var module = moduleTableKV.Value;
                module.Dispose();
            }
            _ModuleIndexTable.Clear();
            _ModuleLabelTable.Clear();
        }

#if  APPLY_PRINT_LOG
        /// <summary>
        /// 클러스터 상태를 출력하는 메서드
        /// </summary>
        public void PrintModuleClusterInfo()
        {
            CustomDebug.LogError($"* {ModuleType} (current : {_CurrentModule})");

            if (_ModuleIndexTable.Count > 0)
            {
                CustomDebug.LogError($"- ModuleIndexTable");
                foreach (var indexTypeKv in _ModuleIndexTable)
                {
                    CustomDebug.LogError($" ({indexTypeKv.Key} : {indexTypeKv.Value})");
                }
            }

            if (_ModuleLabelTable.Count > 0)
            {
                CustomDebug.LogError($"- ModuleLabelBaseTable");
                foreach (var labelBaseTypeKV in _ModuleLabelTable)
                {
                    CustomDebug.LogError($" ({labelBaseTypeKV.Key} : {string.Join(",", labelBaseTypeKV.Value?.Select(type => $"{type}"))})");
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