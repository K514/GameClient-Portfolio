using System;

namespace k514
{
    public interface IDataStorageTable<Record> : ITable<DataStorageTableMetaData, int, Record>
        where Record : IDataStorageTableRecord<int, Record>
    {
    }

    public interface IDataStorageTableRecord<Key, Record> : ITableRecord<Key, Record> 
        where Record : IDataStorageTableRecord<Key, Record>
    {
    }
    
    [Serializable]
    public class DataStorageTableMetaData : TableMetaData
    {
    }

    public abstract class DataStorageTable<Table, Record> : OptionalTable<Table, DataStorageTableMetaData, int, Record>, IDataStorageTable<Record>
        where Table : DataStorageTable<Table, Record>, new() 
        where Record : DataStorageTable<Table, Record>.DataStorageTableRecordBase, new()
    {
        #region <Record>

        [Serializable]
        public abstract class DataStorageTableRecordBase : OptionalTableRecord, IDataStorageTableRecord<int, Record>
        {
        }

        #endregion
    }
}