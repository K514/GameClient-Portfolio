using System;
using System.Collections.Generic;

namespace k514.Mono.Common
{
    public interface IPassiveSkillDataTableBridge<out RecordBridge> : ISkillDataTableBridge<RecordBridge>, ITableBridgeLabel<PassiveSkillDataTableQuery.TableLabel>
    {
    }

    public interface IPassiveSkillDataTableRecordBridge : ISkillDataTableRecordBridge
    {
        public List<int> ExtraOptionIndexList { get; }
    }
    
    public abstract class PassiveSkillDataTable<Table, Record, RecordBridge> : SkillDataTable<Table, Record, RecordBridge>, IPassiveSkillDataTableBridge<RecordBridge>
        where Table : PassiveSkillDataTable<Table, Record, RecordBridge>, new() 
        where Record : PassiveSkillDataTable<Table, Record, RecordBridge>.PassiveSkillDataTableRecord, RecordBridge, new()
        where RecordBridge : class, IPassiveSkillDataTableRecordBridge
    {
        #region <Fields>

        protected PassiveSkillDataTableQuery.TableLabel _PassiveSkillTableLabel;
        PassiveSkillDataTableQuery.TableLabel ITableBridgeLabel<PassiveSkillDataTableQuery.TableLabel>.TableLabel => _PassiveSkillTableLabel;

        #endregion
        
        #region <Record>

        [Serializable]
        public abstract class PassiveSkillDataTableRecord : SkillDataTableRecord, IPassiveSkillDataTableRecordBridge
        {
            #region <Fields>

            public List<int> ExtraOptionIndexList { get; protected set; }

            #endregion
        }
        
        #endregion
        
        #region <Callbacks>

        protected override void OnCreateTableBridge()
        {
            base.OnCreateTableBridge();
            
            _SkillTableLabel = SkillDataTableQuery.TableLabel.Passive;
        }

        #endregion
    }
}