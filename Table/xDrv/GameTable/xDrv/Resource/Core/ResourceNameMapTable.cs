using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Object = UnityEngine.Object;

namespace k514
{
    public interface IResourceNameTable<Key, Asset> : ITable<Key> where Asset : Object
    {
        public AssetLoadResult<Asset> GetResource(Key p_Key, ResourceLifeCycleType p_ResourceLifeCycleType);
        public UniTask<AssetLoadResult<Asset>> GetResourceAsync(Key p_Key, ResourceLifeCycleType p_ResourceLifeCycleType, CancellationToken p_CancellationToken);
        public MultiAssetLoadResult<Asset> GetResources(Key p_Key, ResourceLifeCycleType p_ResourceLifeCycleType);
        public UniTask<MultiAssetLoadResult<Asset>> GetResourcesAsync(Key p_Key, ResourceLifeCycleType p_ResourceLifeCycleType, CancellationToken p_CancellationToken);
    }
    
    public interface IResourceNameTableRecord<Asset> : ITableRecord where Asset : Object
    {
        public AssetLoadResult<Asset> GetResource(ResourceLifeCycleType p_ResourceLifeCycleType);
        public UniTask<AssetLoadResult<Asset>> GetResourceAsync(ResourceLifeCycleType p_ResourceLifeCycleType, CancellationToken p_CancellationToken);
        public MultiAssetLoadResult<Asset> GetResources(ResourceLifeCycleType p_ResourceLifeCycleType);
        public UniTask<MultiAssetLoadResult<Asset>> GetResourcesAsync(ResourceLifeCycleType p_ResourceLifeCycleType, CancellationToken p_CancellationToken);
    }
    
    /// <summary>
    /// 리소스 이름 관리 테이블
    /// </summary>
    public abstract class ResourceNameMapTable<Table, Key, Record, RecordBridge, Asset> : GameTable<Table, TableMetaData, Key, Record, RecordBridge>, IResourceNameTable<Key, Asset>
        where Table : ResourceNameMapTable<Table, Key, Record, RecordBridge, Asset>, new() 
        where Record : ResourceNameMapTable<Table, Key, Record, RecordBridge, Asset>.ResourceNameTableRecord, RecordBridge, new()
        where RecordBridge : class, IResourceNameTableRecord<Asset>
        where Asset : Object
    {
        #region <Consts>

        private static readonly string ResourceFallbackName = "Null.null";

        #endregion

        #region <Callbacks>

        /*protected async override UniTask OnCreateTable(CancellationToken p_CancellationToken)
        {
            await base.OnCreateTable(p_CancellationToken);
            
            Debug.LogError($"R.N.Table : [{typeof(Table).Name} Loaded (Record : {GetRecordCount()})]");
        }*/

        #endregion
        
        #region <Record>

        [Serializable]
        public abstract class ResourceNameTableRecord : GameTableRecord, IResourceNameTableRecord<Asset>
        {
            #region <Fields>

            /// <summary>
            /// 해당 리소스의 이름
            /// </summary>
            public string ResourceName { get; protected set; }

            #endregion

            #region <Methods>

            public override async UniTask SetRecord(Key p_Key, object[] p_RecordField, CancellationToken p_CancellationToken)
            {
                await base.SetRecord(p_Key, p_RecordField, p_CancellationToken);
                
                ResourceName = (string) p_RecordField.GetElementSafe(0);
            }

            public AssetLoadResult<Asset> GetResource(ResourceLifeCycleType p_ResourceLifeCycleType)
            {
                return AssetLoaderManager.GetInstanceUnsafe.LoadAsset<Asset>((p_ResourceLifeCycleType, ResourceName));
            }
            
            public async UniTask<AssetLoadResult<Asset>> GetResourceAsync(ResourceLifeCycleType p_ResourceLifeCycleType, CancellationToken p_CancellationToken)
            {
                return await AssetLoaderManager.GetInstanceUnsafe.LoadAssetAsync<Asset>((p_ResourceLifeCycleType, ResourceName), p_CancellationToken);
            }
            
            public MultiAssetLoadResult<Asset> GetResources(ResourceLifeCycleType p_ResourceLifeCycleType)
            {
                return AssetLoaderManager.GetInstanceUnsafe.LoadMultiAsset<Asset>((p_ResourceLifeCycleType, ResourceName));
            }
            
            /// <summary>
            /// 지정한 키에 매핑된 이름을 가진 에셋을 비동기 로드하여 리턴하는 메서드 
            /// </summary>
            public async UniTask<MultiAssetLoadResult<Asset>> GetResourcesAsync(ResourceLifeCycleType p_ResourceLifeCycleType, CancellationToken p_CancellationToken)
            {
                return await AssetLoaderManager.GetInstanceUnsafe.LoadMultiAssetAsync<Asset>((p_ResourceLifeCycleType, ResourceName), p_CancellationToken);
            }
            
            #endregion
        }

        #endregion

        #region <Methods>
        
        public AssetLoadResult<Asset> GetResource(Key p_Key, ResourceLifeCycleType p_ResourceLifeCycleType)
        {
            if (TryGetRecord(p_Key, out var o_Record))
            {
                return o_Record.GetResource(p_ResourceLifeCycleType);
            }
            else
            {
                return default;
            }
        }
        
        public async UniTask<AssetLoadResult<Asset>> GetResourceAsync(Key p_Key, ResourceLifeCycleType p_ResourceLifeCycleType, CancellationToken p_CancellationToken)
        {
            if (TryGetRecord(p_Key, out var o_Record))
            {
                return await o_Record.GetResourceAsync(p_ResourceLifeCycleType, p_CancellationToken);
            }
            else
            {
                return default;
            }
        }
        
        public MultiAssetLoadResult<Asset> GetResources(Key p_Key, ResourceLifeCycleType p_ResourceLifeCycleType)
        {
            if (TryGetRecord(p_Key, out var o_Record))
            {
                return o_Record.GetResources(p_ResourceLifeCycleType);
            }
            else
            {
                return default;
            }
        }
        
        public async UniTask<MultiAssetLoadResult<Asset>> GetResourcesAsync(Key p_Key, ResourceLifeCycleType p_ResourceLifeCycleType, CancellationToken p_CancellationToken)
        {
            if (TryGetRecord(p_Key, out var o_Record))
            {
                return await o_Record.GetResourcesAsync(p_ResourceLifeCycleType, p_CancellationToken);
            }
            else
            {
                return default;
            }
        }
        
        #endregion
    }
    
    /// <summary>
    /// 단일 테이블용 슈퍼 클래스
    /// </summary>
    public abstract class ResourceNameMapTable<Table, Key, Record, Asset> : ResourceNameMapTable<Table, Key, Record, Record, Asset>
        where Table : ResourceNameMapTable<Table, Key, Record, Asset>, new() 
        where Record : ResourceNameMapTable<Table, Key, Record, Asset>.ResourceNameTableRecord, new()
        where Asset : Object
    {
    }
    
    public interface IResourceNameTableBridge<out RecordBridge, Asset> : IResourceNameTable<int, Asset>, ITableIndexBridge<TableMetaData, RecordBridge> where Asset : Object
        where RecordBridge : class, IResourceNameTableRecord<Asset>
    {
    }
    
    /// <summary>
    /// 멀티 테이블용 테이블 브릿지
    /// </summary>
    public abstract class ResourceNameMapTableBridge<Table, Record, RecordBridge, Asset> : ResourceNameMapTable<Table, int, Record, RecordBridge, Asset>, IResourceNameTableBridge<RecordBridge, Asset>
        where Table : ResourceNameMapTableBridge<Table, Record, RecordBridge, Asset>, new() 
        where Record : ResourceNameMapTableBridge<Table, Record, RecordBridge, Asset>.ResourceNameTableRecord, RecordBridge, new()
        where RecordBridge : class, IResourceNameTableRecord<Asset>
        where Asset : Object
    {
        #region <Fields>

        public int StartIndex { get; protected set; }
        public int EndIndex { get; protected set; }

        #endregion
   
        #region <Callbacks>

        protected override async UniTask OnCreated(CancellationToken p_CancellationToken)
        {
            await base.OnCreated(p_CancellationToken);
            
            OnCreateTableBridge();
        }

        protected abstract void OnCreateTableBridge();

        #endregion
        
        #region <Methods>
        
        protected override int GetDefaultKey()
        {
            return StartIndex;
        }
        
        public override bool IsAddibleKey(int p_Key)
        {
            return StartIndex <= p_Key && p_Key < EndIndex;
        }
        
        public bool IsBridged(int p_Key)
        {
            return IsAddibleKey(p_Key);
        }
        
        #endregion
    }
}