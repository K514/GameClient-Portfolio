using System.Threading;
using Cysharp.Threading.Tasks;

namespace k514
{
    public abstract class GameTableBridge<Table, Meta, Key, Record, RecordBridge> : GameTable<Table, Meta, Key, Record, RecordBridge>, ITableBridge<Key, Meta, RecordBridge>
        where Table : GameTableBridge<Table, Meta, Key, Record, RecordBridge>, new()
        where Meta : TableMetaData, new()
        where Record : GameTableBridge<Table, Meta, Key, Record, RecordBridge>.GameTableRecord, RecordBridge, new()
        where RecordBridge : class
    {
        #region <Callbacks>

        protected override async UniTask OnCreated(CancellationToken p_CancellationToken)
        {
            await base.OnCreated(p_CancellationToken);
            
            OnCreateTableBridge();
        }

        protected abstract void OnCreateTableBridge();

        #endregion

        #region <Methods>
        
        public virtual bool IsBridged(Key p_Key)
        {
            return HasKey(p_Key);
        }
        
        #endregion
    }
    
    public abstract class GameTableIndexBridge<Table, Meta, Record, RecordBridge> : GameTableBridge<Table, Meta, int, Record, RecordBridge>, ITableIndexBridge<Meta, RecordBridge>
        where Table : GameTableIndexBridge<Table, Meta, Record, RecordBridge>, new()
        where Meta : TableMetaData, new()
        where Record : GameTableIndexBridge<Table, Meta, Record, RecordBridge>.GameTableRecord, RecordBridge, new()
        where RecordBridge : class
    {
        #region <Fields>

        public int StartIndex { get; protected set; }
        public int EndIndex { get; protected set; }

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
        
        public override bool IsBridged(int p_Key)
        {
            return IsAddibleKey(p_Key);
        }
        
        #endregion
    }
}