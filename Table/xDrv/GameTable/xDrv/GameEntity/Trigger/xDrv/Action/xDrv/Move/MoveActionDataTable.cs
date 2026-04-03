using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace k514.Mono.Common
{
    public class MoveActionDataTable : ActionDataTable<MoveActionDataTable, MoveActionDataTable.TableRecord, MoveActionDataTable.TableRecord>
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
            _ActionTableLabel = ActionDataTableQuery.TableLabel.Move;
            StartIndex = 100;
            EndIndex = 200;
        }

        #endregion
                                
        #region <Methods>

        protected override async UniTask AddDefaultRecords(CancellationToken p_CancellationToken)
        {
            await base.AddDefaultRecords(p_CancellationToken);
            
            await AddRecord(GetDefaultKey(), false, p_CancellationToken, typeof(D3MoveEventHandler), 0f, 0f, 1, 200000100, 10000000);
            await AddRecord(GetDefaultKey() + 1, false, p_CancellationToken, typeof(D2MoveEventHandler), 0f, 0f, 1, 200000100, 10000000);
        }

        #endregion
    }
}