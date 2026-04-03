using System;

namespace k514.Mono.Common
{
    public class WeaponModelDataTable : UnitModelDataTable<WeaponModelDataTable, WeaponModelDataTable.TableRecord, WeaponModelDataTable.TableRecord>
    {
        #region <Record>

        [Serializable]
        public class TableRecord : UnitModelDataTableRecord
        {
        }

        #endregion

        #region <Callbacks>
        
        protected override void OnCreateTableBridge()
        {
            base.OnCreateTableBridge();

            _UnitModelLabel = UnitModelDataTableQuery.TableLabel.Weapon;
            StartIndex = 1_100_000_000;
            EndIndex = 1_200_000_000;
        }

        #endregion
    }
}