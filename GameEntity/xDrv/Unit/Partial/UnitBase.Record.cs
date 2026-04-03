using k514.Mono.Common;
using UnityEngine;

namespace k514.Mono.Feature
{
    public partial class UnitBase
    {
        #region <Fields>

        public new IUnitModelDataTableRecordBridge ModelDataRecord { get; private set; }
        public new IUnitComponentDataTableRecordBridge ComponentDataRecord { get; private set; }

        #endregion
        
        #region <Callbacks>

        protected override void OnBindRecordBubble(IPrefabModelDataTableRecordBridge p_ModelRecord, IPrefabComponentDataTableRecordBridge p_ComponentRecord)
        {
            ModelDataRecord = UnitModelDataTableQuery.GetInstanceUnsafe.CastRecord(p_ModelRecord, true);
            ComponentDataRecord = UnitComponentDataTableQuery.GetInstanceUnsafe.CastRecord(p_ComponentRecord, true);
            
            base.OnBindRecordBubble(p_ModelRecord, p_ComponentRecord);
        }

        #endregion
    }
}