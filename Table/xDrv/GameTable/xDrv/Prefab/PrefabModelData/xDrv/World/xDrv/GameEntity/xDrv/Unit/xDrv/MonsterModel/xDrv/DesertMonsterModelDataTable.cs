using System;

namespace k514.Mono.Common
{
    public class DesertMonsterModelDataTable : MonsterModelDataTable<DesertMonsterModelDataTable, DesertMonsterModelDataTable.TableRecord, DesertMonsterModelDataTable.TableRecord>
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

            _MonsterModelLabel = MonsterModelDataTableQuery.TableLabel.Desert;
            StartIndex = 1_310_000_000;
            EndIndex = 1_320_000_000;
        }

        #endregion
    }
}