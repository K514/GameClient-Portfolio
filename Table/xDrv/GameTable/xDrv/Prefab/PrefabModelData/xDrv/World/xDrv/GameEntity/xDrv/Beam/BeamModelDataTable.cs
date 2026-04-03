using System;

namespace k514.Mono.Common
{
    public class BeamModelDataTable : GameEntityModelDataTable<BeamModelDataTable, BeamModelDataTable.TableRecord, IGameEntityModelDataTableRecordBridge>
    {
        #region <Record>

        [Serializable]
        public class TableRecord : GameEntityModelDataTableRecord
        {
            public float MainTextureLength { get; private set; }
            public float NoiseTextureLength { get; private set; }
        }

        #endregion

        #region <Callbacks>

        protected override void OnCreateTableBridge()
        {
            base.OnCreateTableBridge();

            _GameEntityModelLabel = GameEntityModelDataTableQuery.TableLabel.Beam;
            StartIndex = 300_000_000;
            EndIndex = 500_000_000;
        }

        #endregion
    }
}