using System;

namespace k514
{
    public interface IMultiTable<Key, Meta, Label, TableBridge, RecordBridge> : IMultiTableQuery<Key, TableBridge, RecordBridge>, 
        IMultiTableLabelQuery<Key, Label, TableBridge, RecordBridge>, IMultiTableSubLabelQuery<Key, Label, TableBridge, RecordBridge>,
        ITableLoadCommon, ITableKeyQuery<Key>, ITableUnsafeRecordQuery<Key, RecordBridge>, ITableSafeRecordQuery<Key, object>
        where Meta : TableMetaData
        where Label : struct, Enum
        where TableBridge : ITableBridge<Key, Meta, RecordBridge>, ITableBridgeLabel<Label>
        where RecordBridge : ITableRecord
    {
        
    }
}