using System;

namespace k514.Mono.Common
{
    public class IceMonsterModelDataTable : MonsterModelDataTable<IceMonsterModelDataTable, IceMonsterModelDataTable.TableRecord, IceMonsterModelDataTable.TableRecord>
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

            _MonsterModelLabel = MonsterModelDataTableQuery.TableLabel.Ice;
            StartIndex = 1_330_000_000;
            EndIndex = 1_340_000_000;
        }

        #endregion
    }
}