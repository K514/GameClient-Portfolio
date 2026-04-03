#if !SERVER_DRIVE

using System;
using UnityEngine;

namespace k514.Mono.Common
{
    public class ScreenSpaceCameraLayerDeployDataTable : UIxLayerDeployDataTable<ScreenSpaceCameraLayerDeployDataTable, ScreenSpaceCameraLayerDeployDataTable.TableRecord>
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
            _RenderModeLabel = RenderMode.ScreenSpaceCamera;
            StartIndex = 0;
            EndIndex = 50;
        }
        
        #endregion
    }
}

#endif