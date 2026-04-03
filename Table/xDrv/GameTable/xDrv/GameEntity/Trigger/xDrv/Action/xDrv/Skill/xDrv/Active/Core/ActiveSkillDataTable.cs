using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace k514.Mono.Common
{
    public interface IActiveSkillDataTableBridge<out RecordBridge> : ISkillDataTableBridge<RecordBridge>, ITableBridgeLabel<ActiveSkillDataTableQuery.TableLabel>
    {
    }

    public interface IActiveSkillDataTableRecordBridge : ISkillDataTableRecordBridge
    {
        /// <summary>
        /// 액션의 발동 커맨드 (방향키 + 커맨드키)
        /// </summary>
        ActionTool.SequenceCommand SequenceCommand { get; }

        /// <summary>
        /// 인공지능 스킬 발동 거리
        /// </summary>
        float EngageRange { get; }
    }
    
    public abstract class ActiveSkillDataTable<Table, Record, RecordBridge> : SkillDataTable<Table, Record, RecordBridge>, IActiveSkillDataTableBridge<RecordBridge>
        where Table : ActiveSkillDataTable<Table, Record, RecordBridge>, new() 
        where Record : ActiveSkillDataTable<Table, Record, RecordBridge>.ActiveSkillDataTableRecord, RecordBridge, new()
        where RecordBridge : class, IActiveSkillDataTableRecordBridge
    {
        #region <Fields>

        protected ActiveSkillDataTableQuery.TableLabel _ActiveSkillTableLabel;
        ActiveSkillDataTableQuery.TableLabel ITableBridgeLabel<ActiveSkillDataTableQuery.TableLabel>.TableLabel => _ActiveSkillTableLabel;

        #endregion
        
        #region <Record>

        [Serializable]
        public abstract class ActiveSkillDataTableRecord : SkillDataTableRecord, IActiveSkillDataTableRecordBridge
        {
            #region <Fields>

            public ActionTool.SequenceCommand SequenceCommand { get; protected set; }
            public float EngageRange { get; protected set; }

            #endregion

            #region <Methods>
            
            public override async UniTask SetRecord(int p_Key, object[] p_RecordField, CancellationToken p_CancellationToken)
            {
                await base.SetRecord(p_Key, p_RecordField, p_CancellationToken);

                SequenceCommand = p_RecordField.GetElementSafe<ActionTool.SequenceCommand>(6);
                EngageRange = p_RecordField.GetElementSafe<float>(7);
            }

            #endregion
        }
        
        #endregion
        
        #region <Callbacks>

        protected override void OnCreateTableBridge()
        {
            base.OnCreateTableBridge();
            
            _SkillTableLabel = SkillDataTableQuery.TableLabel.Active;
        }

        #endregion
    }
}