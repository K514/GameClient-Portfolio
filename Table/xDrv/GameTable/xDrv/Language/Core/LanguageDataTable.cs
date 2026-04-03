#if !SERVER_DRIVE

using System;

namespace k514
{
    public interface ILanguageDataTableBridge : ITableIndexBridge<TableMetaData, ILanguageDataTableRecordBridge>, ITableBridgeLabel<LanguageDataTableQuery.TableLabel>
    {
        string GetLanguageTableFileName(LanguageType p_Type);
    }

    public interface ILanguageDataTableRecordBridge : ITableRecord
    {
        string Text { get; }
    }
    
    public abstract class LanguageDataTable<Table, Record> : GameTableIndexBridge<Table, TableMetaData, Record, ILanguageDataTableRecordBridge>, ILanguageDataTableBridge
        where Table : LanguageDataTable<Table, Record>, new()
        where Record : LanguageDataTable<Table, Record>.LanguageDataTableRecord, new()
    {
        #region <Fields>

        protected LanguageDataTableQuery.TableLabel _LanguageTableLabel;

        LanguageDataTableQuery.TableLabel ITableBridgeLabel<LanguageDataTableQuery.TableLabel>.TableLabel => _LanguageTableLabel;

        #endregion
        
        #region <Record>
        
        [Serializable]
        public abstract class LanguageDataTableRecord : GameTableRecord, ILanguageDataTableRecordBridge
        {
            #region <Fields>

            public string Text { get; protected set; }
            
            #endregion
        }

        #endregion

        #region <Methods>

        protected override string GetMainTableFileName()
        {
            return GetLanguageTableFileName(SystemBoot.SystemLanguageType);
        }
        
        public string GetLanguageTableFileName(LanguageType p_Type)
        {
            return $"{base.GetMainTableFileName()}.{p_Type}";
        }

        #endregion
    }
}

#endif