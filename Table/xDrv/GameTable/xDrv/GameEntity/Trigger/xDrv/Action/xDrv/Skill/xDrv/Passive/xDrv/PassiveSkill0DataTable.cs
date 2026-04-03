using System;

namespace k514.Mono.Common
{
    public class PassiveSkill0DataTable : PassiveSkillDataTable<PassiveSkill0DataTable, PassiveSkill0DataTable.TableRecord, PassiveSkill0DataTable.TableRecord>
    {
        #region <Record>
        
        [Serializable]
        public class TableRecord : PassiveSkillDataTableRecord
        {
        }

        #endregion

        #region <Callbacks>

        protected override void OnCreateTableBridge()
        {
            base.OnCreateTableBridge();

            _PassiveSkillTableLabel = PassiveSkillDataTableQuery.TableLabel.Group_0;
            StartIndex = 2_000_000_000;
            EndIndex = 2_100_000_000;
        }

        #endregion
    }
}