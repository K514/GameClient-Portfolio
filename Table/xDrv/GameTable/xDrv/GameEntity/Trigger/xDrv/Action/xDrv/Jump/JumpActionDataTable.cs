using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace k514.Mono.Common
{
    public class JumpActionDataTable : ActionDataTable<JumpActionDataTable, JumpActionDataTable.TableRecord, JumpActionDataTable.TableRecord>
    {
        #region <Record>
        
        [Serializable]
        public class TableRecord : ActionDataTableRecord
        {
        }

        #endregion

        #region <Callbacks>

        protected override void OnCreateTableBridge()
        {
            _ActionTableLabel = ActionDataTableQuery.TableLabel.Jump;
            StartIndex = 200;
            EndIndex = 300;
        }

        #endregion
                        
        #region <Methods>

        protected override async UniTask AddDefaultRecords(CancellationToken p_CancellationToken)
        {
            await base.AddDefaultRecords(p_CancellationToken);
            
            await AddRecord(GetDefaultKey(), false, p_CancellationToken, typeof(DefaultJumpEventHandler), 0f, 0f, 1, 200000200, 10000000);
                
        }

        #endregion
    }
}