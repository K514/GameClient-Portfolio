using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using k514.Mono.Common;
using k514.Mono.Feature;
using UnityEngine;

namespace k514.Mono.Common
{
    public class ExtraOptionDataTable : GameTable<ExtraOptionDataTable, TableMetaData, int, ExtraOptionDataTable.TableRecord>
    {
        #region <Record>

        [Serializable]
        public class TableRecord : GameTableRecord, ITriggerEventHandlerRecord
        {
            #region <Fields>

            public Type EventHandlerType { get; private set; }
            public float Cost { get; private set; }
            public float Cooldown { get; private set; }
            public int LevelUpperBound { get; private set; }
            public GameEntityExtraOptionTool.ExtraOptionType ExtraOptionType { get; private set; }
            public int Index { get; private set; }
            public int Count { get; private set; }
            public float Value { get; private set; }
            public float Value2 { get; private set; }
            public BattleStatusTool.BattleStatusType BattleStatusType { get; private set; }
            public ShotStatusTool.ShotStatusType ShotStatusType { get; private set; }
            [TableTool.TableRecordAttribute(TableTool.TableRecordAttributeType.Runtime)] 
            public BattleStatusPreset BattleStatusPreset { get; private set; }
            [TableTool.TableRecordAttribute(TableTool.TableRecordAttributeType.Runtime)] 
            public BattleStatusPreset BattleStatusPreset2 { get; private set; }
            [TableTool.TableRecordAttribute(TableTool.TableRecordAttributeType.Runtime)] 
            public ShotStatusPreset ShotStatusPreset { get; private set; }
            [TableTool.TableRecordAttribute(TableTool.TableRecordAttributeType.Runtime)] 
            public ShotStatusPreset ShotStatusPreset2 { get; private set; }
            
            #endregion

            #region <Callbacks>

            public override async UniTask OnRecordAdded(ExtraOptionDataTable p_Table, CancellationToken p_Cancellation)
            {
                await base.OnRecordAdded(p_Table, p_Cancellation);

                Count = Mathf.Max(Count, 1);

                if (BattleStatusType != BattleStatusTool.BattleStatusType.None)
                {
                    BattleStatusPreset = new BattleStatusPreset(BattleStatusType, Value);
                    BattleStatusPreset2 = new BattleStatusPreset(BattleStatusType, Value2);
                }
                
                if (ShotStatusType != ShotStatusTool.ShotStatusType.None)
                {
                    ShotStatusPreset = new ShotStatusPreset(ShotStatusType, Value);
                    ShotStatusPreset2 = new ShotStatusPreset(ShotStatusType, Value2);
                }
            }

            #endregion

            #region <Methods>

            public bool TryGetLanguageRecord(out ExtraOptionLanguageDataTable.TableRecord o_Record)
            {
                if (ExtraOptionMetaDataTable.GetInstanceUnsafe.TryGetRecord(ExtraOptionType, out var o_EventRecord))
                {
                    return o_EventRecord.TryGetLanguageRecord(out o_Record);
                }
                else
                {
                    o_Record = default;
                
                    return false;
                }
            }

            #endregion
        }

        #endregion
        
        #region <Callbacks>

        protected override void TryInitializeDependency()
        {
            base.TryInitializeDependency();

            _Dependencies.Add(typeof(ExtraOptionMetaDataTable)); 
        }
        
        #endregion

    }
}