

using System;
#if !SERVER_DRIVE
using System.Collections.Generic;
using UnityEngine;

namespace k514.Mono.Common
{
    public interface IUIxLayerDeployDataTableBridge : ITableIndexBridge<TableMetaData, IUIxLayerDeployDataTableRecordBridge>, ITableBridgeLabel<RenderMode>
    {
    }

    public interface IUIxLayerDeployDataTableRecordBridge : ITableRecord
    {
        List<UIxTool.UIxElementType> UIxElementTypeList { get; }
    }
    
    public abstract class UIxLayerDeployDataTable<Table, Record> : GameTableIndexBridge<Table, TableMetaData, Record, IUIxLayerDeployDataTableRecordBridge>, IUIxLayerDeployDataTableBridge
        where Table : UIxLayerDeployDataTable<Table, Record>, new()
        where Record : UIxLayerDeployDataTable<Table, Record>.UIxLayerDeployDataTableRecord, new()
    {
        #region <Fields>

        protected RenderMode _RenderModeLabel;
        RenderMode ITableBridgeLabel<RenderMode>.TableLabel => _RenderModeLabel;

        #endregion

        #region <Record>

        [Serializable]
        public abstract class UIxLayerDeployDataTableRecord : GameTableRecord, IUIxLayerDeployDataTableRecordBridge
        {
            public List<UIxTool.UIxElementType> UIxElementTypeList { get; protected set; }
        }

        #endregion
    }
}

#endif