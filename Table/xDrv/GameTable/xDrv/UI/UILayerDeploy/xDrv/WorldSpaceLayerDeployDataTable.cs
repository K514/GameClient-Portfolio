#if !SERVER_DRIVE

using System;
using UnityEngine;

namespace k514.Mono.Common
{
    public class WorldSpaceLayerDeployDataTable : UIxLayerDeployDataTable<WorldSpaceLayerDeployDataTable, WorldSpaceLayerDeployDataTable.TableRecord>
    {
        #region <Record>
        
        [Serializable]
        public class TableRecord : UIxLayerDeployDataTableRecord
        {
        }

        #endregion
        
        #region <Callbacks>
        
        protected override void OnCreateTableBridge()
        {
            _RenderModeLabel = RenderMode.WorldSpace;
            StartIndex = 100;
            EndIndex = 150;
        }
        
        #endregion
    }
}

#endif