using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace k514.Mono.Common
{
    public class DashActionDataTable : ActionDataTable<DashActionDataTable, DashActionDataTable.TableRecord, DashActionDataTable.TableRecord>
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
            _ActionTableLabel = ActionDataTableQuery.TableLabel.Dash;
            StartIndex = 300;
            EndIndex = 400;
        }

        #endregion

        #region <Methods>

        protected override async UniTask AddDefaultRecords(CancellationToken p_CancellationToken)
        {
            await base.AddDefaultRecords(p_CancellationToken);
            
            await AddRecord(GetDefaultKey(), false, p_CancellationToken, typeof(DefaultDashEventHandler), 5f, 2f, 1, 200000300, 10000100);
                
        }

        #endregion
    }
}