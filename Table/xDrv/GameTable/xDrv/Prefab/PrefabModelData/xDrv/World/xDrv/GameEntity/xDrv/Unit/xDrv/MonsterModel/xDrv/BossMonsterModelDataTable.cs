using System;

namespace k514.Mono.Common
{
    public class BossMonsterModelDataTable : MonsterModelDataTable<BossMonsterModelDataTable, BossMonsterModelDataTable.TableRecord, BossMonsterModelDataTable.TableRecord>
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

            _MonsterModelLabel = MonsterModelDataTableQuery.TableLabel.Boss;
            StartIndex = 1_380_000_000;
            EndIndex = 1_390_000_000;
        }

        #endregion
    }
}