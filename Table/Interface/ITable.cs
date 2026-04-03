using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace k514
{
    public interface ITable : ISingleton, ITableLoad, ITableWrite, ITableSerialize
    {
        TableTool.TableType TableType { get; }
        TableTool.TableSerializeType TableSerializeType { get; }
        TableTool.TableFileType GetTableFileType();
        string GetBranchHeader();
        string GetTableFileName(TableTool.TableNameQueryType p_TableNameQueryType);
        string GetTableFileFullPath(ResourceLoadType p_ResourceLoadType, AssetPathType p_PathType, TableTool.TableNameQueryType p_TableNameQueryType);
        string GetByteTableFullPath();
        string GetByteTableRelativePath();
        int GetRecordCount();
        void ClearTable(bool p_DefaultFlag);
    }

    public interface ITableLoad : ITableLoadCommon, ITableLoadWithName
    {
    }
    
    public interface ITableLoadCommon
    {
        UniTask<bool> LoadTable(CancellationToken p_CancellationToken);
    }
    
    public interface ITableLoadWithName
    {
        UniTask<bool> LoadTable(string p_TableName, CancellationToken p_CancellationToken);
    }
    
    public interface ITableWrite
    {
#if UNITY_EDITOR
        UniTask WriteTableTextFileToAutoPath(DataIOTool.WriteType p_WriteType, CancellationToken p_Cancellation);
#endif
    }
    
    public interface ITableSerialize
    {
#if UNITY_EDITOR
        UniTask SerializeTable(CancellationToken p_Token);
#endif
    }
    
    public interface ITable<Key> : ITable, ITableKeyQuery<Key>
    {
        UniTask<bool> AddRecord(Key p_Key, bool p_OverlapFlag, CancellationToken p_Cancellation, params object[] p_Params);
        void RemoveRecord(Key p_Key);
    }
    
    public interface ITable<Key, Record> : ITable<Key>, ITableUnsafeRecordQuery<Key, Record>, ITableSafeRecordQuery<Key, Record> where Record : ITableRecord
    {
        Dictionary<Key, Record> GetTable();
    }

    public interface ITable<Meta, Key, Record> : ITable<Key, Record>, ITableMetaQuery<Meta>
        where Meta : TableMetaData
        where Record : ITableRecord
    {
        UniTask<bool> ReplaceTable(TableTool.TableDataImage<Meta, Key, Record> p_TableImage, CancellationToken p_CancellationToken);
        void ReplaceMetaData(Meta p_MetaData);
        TableTool.TableDataImage<Meta, Key, Record> GetTableImage();
    }
}