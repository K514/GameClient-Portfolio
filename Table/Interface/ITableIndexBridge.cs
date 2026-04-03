namespace k514
{
    public interface ITableIndexBridge<Meta, out RecordBridge> : ITableBridge<int, Meta, RecordBridge>
        where Meta : TableMetaData
    {
        int StartIndex { get; }
        int EndIndex { get; }
    }
}