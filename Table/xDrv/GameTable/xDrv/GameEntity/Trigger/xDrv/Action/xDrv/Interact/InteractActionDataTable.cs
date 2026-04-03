using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace k514.Mono.Common
{
    public class InteractActionDataTable : ActionDataTable<InteractActionDataTable, InteractActionDataTable.TableRecord, InteractActionDataTable.TableRecord>
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
            _ActionTableLabel = ActionDataTableQuery.TableLabel.Interact;
            StartIndex = 500;
            EndIndex = 1000;
        }

        #endregion
                
        #region <Methods>

        protected override async UniTask AddDefaultRecords(CancellationToken p_CancellationToken)
        {
            await base.AddDefaultRecords(p_CancellationToken);
            
            await AddRecord(GetDefaultKey(), false, p_CancellationToken, typeof(DefaultInteractionEventHandler), 0f, 0f, 1, 200000500, 10000000);
                
        }

        #endregion
    }
}