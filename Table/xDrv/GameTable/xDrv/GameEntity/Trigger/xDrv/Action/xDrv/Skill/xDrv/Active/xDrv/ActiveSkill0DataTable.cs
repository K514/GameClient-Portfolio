using System;

namespace k514.Mono.Common
{
    public class ActiveSkill0DataTable : ActiveSkillDataTable<ActiveSkill0DataTable, ActiveSkill0DataTable.TableRecord, ActiveSkill0DataTable.TableRecord>
    {
        #region <Record>
        
        [Serializable]
        public class TableRecord : ActiveSkillDataTableRecord
        {
        }

        #endregion

        #region <Callbacks>

        protected override void OnCreateTableBridge()
        {
            base.OnCreateTableBridge();

            _ActiveSkillTableLabel = ActiveSkillDataTableQuery.TableLabel.Group_0;
            StartIndex = 1_000_000_000;
            EndIndex = 1_100_000_000;
        }

        #endregion
    }
}