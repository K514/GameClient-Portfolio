using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using k514.Mono.Feature;
using UnityEngine;

namespace k514.Mono.Common
{
    public interface IActionDataTableBridge<out RecordBridge> : ITableIndexBridge<TableMetaData, RecordBridge>, ITableBridgeLabel<ActionDataTableQuery.TableLabel>
    {
    }

    public interface IActionDataTableRecordBridge : ITriggerEventHandlerRecord
    {
        /// <summary>
        /// 액션을 기술하는 언어 레코드 인덱스
        /// </summary>
        int ActionLanguageIndex { get; }
        
        /// <summary>
        /// 문자열 포맷을 기술하는 언어 레코드 인덱스
        /// </summary>
        int FormatLanguageIndex { get; }

        ActionLanguageDataTable.TableRecord GetActionLanguageRecordOrFallback();
        bool TryGetActionLanguageRecord(out ActionLanguageDataTable.TableRecord o_Record);
        FormatLanguageDataTable.TableRecord GeFormatLanguageRecordOrFallback();
        bool TryGetFormatLanguageRecord(out FormatLanguageDataTable.TableRecord o_Record);
    }
    
    public abstract class ActionDataTable<Table, Record, RecordBridge> : GameTableIndexBridge<Table, TableMetaData, Record, RecordBridge>, IActionDataTableBridge<RecordBridge>
        where Table : ActionDataTable<Table, Record, RecordBridge>, new() 
        where Record : ActionDataTable<Table, Record, RecordBridge>.ActionDataTableRecord, RecordBridge, new()
        where RecordBridge : class, IActionDataTableRecordBridge
    {
        #region <Fields>

        protected ActionDataTableQuery.TableLabel _ActionTableLabel;
        ActionDataTableQuery.TableLabel ITableBridgeLabel<ActionDataTableQuery.TableLabel>.TableLabel => _ActionTableLabel;

        #endregion

        #region <Record>

        [Serializable]
        public abstract class ActionDataTableRecord : GameTableRecord, IActionDataTableRecordBridge
        {
            #region <Fields>

            public Type EventHandlerType { get; protected set; }
            public float Cost { get; protected set; }
            public float Cooldown { get; protected set; }
            public int LevelUpperBound { get; protected set; }
            public int ActionLanguageIndex { get; protected set; }
            public int FormatLanguageIndex { get; protected set; }

            #endregion

            #region <Callbacks>

            public override async UniTask OnRecordAdded(Table p_Table, CancellationToken p_Cancellation)
            {
                await base.OnRecordAdded(p_Table, p_Cancellation);

                LevelUpperBound = Mathf.Max(LevelUpperBound, 1);
            }

            #endregion
            
            #region <Methods>

            public override async UniTask SetRecord(int p_Key, object[] p_RecordField, CancellationToken p_CancellationToken)
            {
                await base.SetRecord(p_Key, p_RecordField, p_CancellationToken);

                EventHandlerType = (Type) p_RecordField.GetElementSafe(0);
                Cost = (float) p_RecordField.GetElementSafe(1);
                Cooldown = (float) p_RecordField.GetElementSafe(2); 
                LevelUpperBound = (int) p_RecordField.GetElementSafe(3); 
                ActionLanguageIndex = (int) p_RecordField.GetElementSafe(4);
                FormatLanguageIndex = (int) p_RecordField. GetElementSafe(5);
            }

            public ActionLanguageDataTable.TableRecord GetActionLanguageRecordOrFallback()
            {
                return ActionLanguageDataTable.GetInstanceUnsafe.GetRecordOrFallback(ActionLanguageIndex);
            }
            
            public bool TryGetActionLanguageRecord(out ActionLanguageDataTable.TableRecord o_Record)
            {
                return ActionLanguageDataTable.GetInstanceUnsafe.TryGetRecord(ActionLanguageIndex, out o_Record);
            }
            
            public FormatLanguageDataTable.TableRecord GeFormatLanguageRecordOrFallback()
            {
                return FormatLanguageDataTable.GetInstanceUnsafe.GetRecordOrFallback(FormatLanguageIndex);
            }
            
            public bool TryGetFormatLanguageRecord(out FormatLanguageDataTable.TableRecord o_Record)
            {
                return FormatLanguageDataTable.GetInstanceUnsafe.TryGetRecord(FormatLanguageIndex, out o_Record);
            }

            #endregion
        }
        
        #endregion

        #region <Callbacks>

        protected override void TryInitializeDependency()
        {
            base.TryInitializeDependency();

            _Dependencies.Add(typeof(ActionLanguageDataTable));
        }

        #endregion
    }
}