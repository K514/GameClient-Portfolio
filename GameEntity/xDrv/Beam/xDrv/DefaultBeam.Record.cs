using k514.Mono.Common;

namespace k514.Mono.Feature
{
    public partial class DefaultBeam : BeamEntityBase
    {
        public new DefaultBeamComponentDataTable.TableRecord ComponentDataRecord { get; private set; }
        
        protected override void OnBindRecord(IPrefabModelDataTableRecordBridge p_ModelRecord, IPrefabComponentDataTableRecordBridge p_ComponentRecord)
        {
            ComponentDataRecord = DefaultBeamComponentDataTable.GetInstanceUnsafe.CastRecord(p_ComponentRecord, true);
        
            OnBindRecordBubble(p_ModelRecord, ComponentDataRecord);
        }
    }
}