using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace k514.Mono.Common
{
    public interface IPrefabSpawnDataTable<Key> : ITable<Key>
    {
    }
    
    public interface IPrefabSpawnDataTable<Key, Record, ModelTableRecord, ComponentTableRecord> : IPrefabSpawnDataTable<Key>, ITable<Key, Record>
        where Record : IPrefabSpawnDataTableRecord<Key, Record, ModelTableRecord, ComponentTableRecord>
        where ModelTableRecord : class, IPrefabModelDataTableRecordBridge
        where ComponentTableRecord : class, IPrefabComponentDataTableRecordBridge
    {
    }

    public interface IPrefabSpawnDataTableRecord<Key> : ITableRecord<Key>
    {
        /// <summary>
        /// 프리팹 모델 레코드 인덱스
        /// </summary>
        int ModelIndex { get; set;  }
        
        /// <summary>
        /// 프리팹에 붙일 컴포넌트 레코드 인덱스
        /// </summary>
        int ComponentIndex { get; set;  }
    }
    
    public interface IPrefabSpawnDataTableRecord<Key, Record, out ModelTableRecord, out ComponentTableRecord> : IPrefabSpawnDataTableRecord<Key>
        where ModelTableRecord : class, IPrefabModelDataTableRecordBridge
        where ComponentTableRecord : class, IPrefabComponentDataTableRecordBridge
    {
        /// <summary>
        /// 프리팹 모델 레코드를 리턴하는 메서드
        /// </summary>
        public ModelTableRecord GetModelRecordBridge();
                
        /// <summary>
        /// 프리팹에 붙일 컴포넌트 레코드를 리턴하는 메서드
        /// </summary>
        public ComponentTableRecord GetComponentRecordBridge();
    }
    
    public abstract class PrefabSpawnDataTable<Table, Key, Record, ModelTable, ModelTableRecord, ComponentTable, ComponentTableRecord> : GameTable<Table, TableMetaData, Key, Record>, IPrefabSpawnDataTable<Key, Record, ModelTableRecord, ComponentTableRecord>
        where Table : PrefabSpawnDataTable<Table, Key, Record, ModelTable, ModelTableRecord, ComponentTable, ComponentTableRecord>, new() 
        where Record : PrefabSpawnDataTable<Table, Key, Record, ModelTable, ModelTableRecord, ComponentTable, ComponentTableRecord>.PrefabSpawnDataTableRecord, new()
        where ModelTable : class, ITableUnsafeRecordQuery<int, ModelTableRecord>
        where ModelTableRecord : class, IPrefabModelDataTableRecordBridge
        where ComponentTable : class, ITableUnsafeRecordQuery<int, ComponentTableRecord>
        where ComponentTableRecord : class, IPrefabComponentDataTableRecordBridge
    {
        #region <Fields>

        private ModelTable _ModelTable;
        private ComponentTable _ComponentTable;

        #endregion
        
        #region <Record>

        [Serializable]
        public abstract class PrefabSpawnDataTableRecord : GameTableRecord, IPrefabSpawnDataTableRecord<Key, Record, ModelTableRecord, ComponentTableRecord>
        {
            #region <Fields>

            public int ModelIndex { get; set; }
            public int ComponentIndex { get; set; }

            #endregion
            
            #region <Methods>
            
            public override async UniTask SetRecord(Key p_Key, object[] p_RecordField, CancellationToken p_CancellationToken)
            {
                await base.SetRecord(p_Key, p_RecordField, p_CancellationToken);

                ModelIndex = p_RecordField.As<int>(0);
                ComponentIndex = p_RecordField.As<int>(1);
            }
            
            public ModelTableRecord GetModelRecordBridge()
            {
                return GetInstanceUnsafe._ModelTable.GetRecordOrFallback(ModelIndex);
            }
  
            public ComponentTableRecord GetComponentRecordBridge()
            {
                return GetInstanceUnsafe._ComponentTable.GetRecordOrFallback(ComponentIndex);
            }

            public float GetPrefabScale() => GetModelRecordBridge().ModelScale * GetComponentRecordBridge().ComponentScaleFactor;

            #endregion
            
            #region <Operator>
#if UNITY_EDITOR
            public override string ToString()
            {
                return GetComponentRecordBridge().ToString();
            }
#endif
            #endregion
        }

        #endregion

        #region <Callbacks>

        protected override void TryInitializeDependency()
        {
            base.TryInitializeDependency();
            
            _Dependencies.Add(typeof(ModelTable));
            _Dependencies.Add(typeof(ComponentTable));
        }

        protected override async UniTask OnCreated(CancellationToken p_CancellationToken)
        {
            await base.OnCreated(p_CancellationToken);

            _ModelTable = SingletonTool.GetSingletonUnsafe<ModelTable>();
            _ComponentTable = SingletonTool.GetSingletonUnsafe<ComponentTable>();
        }

        #endregion
    }
}