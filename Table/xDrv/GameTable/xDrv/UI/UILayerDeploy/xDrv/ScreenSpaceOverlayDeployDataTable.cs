#if !SERVER_DRIVE

using System;
using UnityEngine;

namespace k514.Mono.Common
{
    public class ScreenSpaceOverlayDeployDataTable : UIxLayerDeployDataTable<ScreenSpaceOverlayDeployDataTable, ScreenSpaceOverlayDeployDataTable.TableRecord>
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
            _RenderModeLabel = RenderMode.ScreenSpaceOverlay;
            StartIndex = 50;
            EndIndex = 100;
        }
        
        #endregion
    }
}

#endif