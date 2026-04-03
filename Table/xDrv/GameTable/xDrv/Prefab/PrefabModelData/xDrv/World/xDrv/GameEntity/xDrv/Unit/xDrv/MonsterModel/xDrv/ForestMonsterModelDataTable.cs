using System;

namespace k514.Mono.Common
{
    public class ForestMonsterModelDataTable : MonsterModelDataTable<ForestMonsterModelDataTable, ForestMonsterModelDataTable.TableRecord, ForestMonsterModelDataTable.TableRecord>
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

            _MonsterModelLabel = MonsterModelDataTableQuery.TableLabel.Forest;
            StartIndex = 1_320_000_000;
            EndIndex = 1_330_000_000;
        }

        #endregion
    }
}