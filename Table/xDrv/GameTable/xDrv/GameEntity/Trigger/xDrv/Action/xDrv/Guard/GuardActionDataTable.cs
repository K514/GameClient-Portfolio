using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace k514.Mono.Common
{
    public class GuardActionDataTable : ActionDataTable<GuardActionDataTable, GuardActionDataTable.TableRecord, GuardActionDataTable.TableRecord>
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
            _ActionTableLabel = ActionDataTableQuery.TableLabel.Guard;
            StartIndex = 400;
            EndIndex = 500;
        }

        #endregion
        
        #region <Methods>

        protected override async UniTask AddDefaultRecords(CancellationToken p_CancellationToken)
        {
            await base.AddDefaultRecords(p_CancellationToken);
            
            await AddRecord(GetDefaultKey(), false, p_CancellationToken, typeof(DefaultGuardEventHandler), 5f, 2f, 1, 200000400, 10000100);
                
        }

        #endregion
    }
}