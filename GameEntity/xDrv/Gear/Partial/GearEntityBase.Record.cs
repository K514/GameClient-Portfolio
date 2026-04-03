using k514.Mono.Common;

namespace k514.Mono.Feature
{
    public partial class GearEntityBase
    {
        #region <Fields>

        public new GearComponentDataTable.TableRecord ComponentDataRecord { get; private set; }

        #endregion
        
        #region <Callbacks>

        protected override void OnBindRecord(IPrefabModelDataTableRecordBridge p_ModelRecord, IPrefabComponentDataTableRecordBridge p_ComponentRecord)
        {
            ComponentDataRecord = GearComponentDataTable.GetInstanceUnsafe.CastRecord(p_ComponentRecord, true);
            
            OnBindRecordBubble(ModelDataRecord, ComponentDataRecord);
        }

        #endregion
    }
}