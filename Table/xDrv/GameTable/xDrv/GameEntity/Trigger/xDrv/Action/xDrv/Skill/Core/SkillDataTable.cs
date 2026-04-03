using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using k514.Mono.Feature;
using UnityEngine;

namespace k514.Mono.Common
{
    public interface ISkillDataTableBridge<out RecordBridge> : IActionDataTableBridge<RecordBridge>, ITableBridgeLabel<SkillDataTableQuery.TableLabel>
    {
    }

    public interface ISkillDataTableRecordBridge : IActionDataTableRecordBridge
    {
    }
    
    public abstract class SkillDataTable<Table, Record, RecordBridge> : ActionDataTable<Table, Record, RecordBridge>, ISkillDataTableBridge<RecordBridge>
        where Table : SkillDataTable<Table, Record, RecordBridge>, new() 
        where Record : SkillDataTable<Table, Record, RecordBridge>.SkillDataTableRecord, RecordBridge, new()
        where RecordBridge : class, ISkillDataTableRecordBridge
    {
        #region <Fields>

        protected SkillDataTableQuery.TableLabel _SkillTableLabel;
        SkillDataTableQuery.TableLabel ITableBridgeLabel<SkillDataTableQuery.TableLabel>.TableLabel => _SkillTableLabel;

        #endregion

        #region <Record>

        [Serializable]
        public abstract class SkillDataTableRecord : ActionDataTableRecord, ISkillDataTableRecordBridge
        {
        }
        
        #endregion
        
        #region <Callbacks>

        protected override void OnCreateTableBridge()
        {
            _ActionTableLabel = ActionDataTableQuery.TableLabel.Skill;
        }

        #endregion
    }
}