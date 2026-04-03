using System;

namespace k514.Mono.Common
{
    public class CaveMonsterModelDataTable : MonsterModelDataTable<CaveMonsterModelDataTable, CaveMonsterModelDataTable.TableRecord, CaveMonsterModelDataTable.TableRecord>
    {
        #region <Record>

        [Serializable]
        public class TableRecord : MonsterModelDataTableRecord
        {
        }

        #endregion

        #region <Callbacks>
        
        protected override void OnCreateTableBridge()
        {
            base.OnCreateTableBridge();

            _MonsterModelLabel = MonsterModelDataTableQuery.TableLabel.Cave;
            StartIndex = 1_300_000_000;
            EndIndex = 1_310_000_000;
        }

        #endregion
    }
}