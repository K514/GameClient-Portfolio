using System;
using k514.Mono.Feature;

namespace k514.Mono.Common
{
    public class ProjectorModelDataTable : WorldObjectModelDataTable<ProjectorModelDataTable, ProjectorModelDataTable.TableRecord, IWorldObjectModelDataTableRecordBridge>
    {
        #region <Record>

        [Serializable]
        public class TableRecord : WorldObjectModelDataTableRecord
        {
            public ProjectorTool.ProjectorDecalType ProjectorDecalType { get; private set; }
        }

        #endregion

        #region <Callbacks>

        protected override void OnCreateTableBridge()
        {
            base.OnCreateTableBridge();

            _WorldObjectModelLabel = WorldObjectModelDataTableQuery.TableLabel.Projector;
            StartIndex = 40_000_000;
            EndIndex = 50_000_000;
        }

        #endregion
    }
}