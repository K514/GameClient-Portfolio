using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace k514
{
    public abstract class GameTable<Table, Meta, Key, Record> : TableBase<Table, Meta, Key, Record>
        where Table : GameTable<Table, Meta, Key, Record>, new()
        where Meta : TableMetaData, new()
        where Record : GameTable<Table, Meta, Key, Record>.GameTableRecord, new() 
    {
        [Serializable]
        public abstract class GameTableRecord : TableRecordBase
        {
        }

        protected override async UniTask OnCreated(CancellationToken p_CancellationToken)
        {
            await base.OnCreated(p_CancellationToken); 
     
            TableType = TableTool.TableType.GameTable;
            TableSerializeType = TableTool.TableSerializeType.SerializeBinaryTableImage;
        }
    }
    
    public abstract class GameTable<Table, Meta, Key, Record, RecordBridge> : GameTable<Table, Meta, Key, Record>, ITableRecordBridgeQuery<Key, RecordBridge>
        where Table : GameTable<Table, Meta, Key, Record, RecordBridge>, new()
        where Meta : TableMetaData, new()
        where Record : GameTable<Table, Meta, Key, Record, RecordBridge>.GameTableRecord, RecordBridge, new()
        where RecordBridge : class
    {
        #region <Methods>
        
        public RecordBridge GetRecordBridge(Key p_Key)
        {
            return GetRecord(p_Key);
        }
        
        public RecordBridge GetRecordBridgeOrFallback(Key p_Key)
        {
            return GetRecordOrFallback(p_Key);
        }

        public RecordBridge GetFirstRecordBridge()
        {
            return GetFirstRecord();
        }

        public RecordBridge GetFallbackRecordBridge()
        {
            return GetFallbackRecord();
        }

        public bool TryGetRecordBridge(Key p_Key, out object o_Record)
        {
            if (TryGetRecord(p_Key, out var o__Record))
            {
                o_Record = o__Record;
                return true;
            }
            else
            {
                o_Record = o__Record;
                return false;
            }
        }

        public bool TryGetRecordBridge<Bridge>(Key p_Key, out Bridge o_Record) where Bridge : class
        {
            o_Record = GetRecordBridge(p_Key) as Bridge;
            return !ReferenceEquals(null, o_Record);
        }

        public bool TryGetFirstRecordBridge<Bridge>(out Bridge o_Record) where Bridge : class
        {
            o_Record = GetFirstRecordBridge() as Bridge;
            return !ReferenceEquals(null, o_Record);
        }

        public bool TryGetFallbackRecordBridge<Bridge>(out Bridge o_Record) where Bridge : class
        {
            o_Record = GetFallbackRecordBridge() as Bridge;
            return !ReferenceEquals(null, o_Record);
        }

        #endregion
    }
}