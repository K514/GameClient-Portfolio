using k514.Mono.Common;

namespace k514.Mono.Feature
{
    public partial class PhaseUnit
    {
        #region <Fields>

        public new PhaseUnitComponentDataTable.TableRecord ComponentDataRecord { get; private set; }

        #endregion
        
        #region <Callbacks>

        protected override void OnBindRecord(IPrefabModelDataTableRecordBridge p_ModelRecord, IPrefabComponentDataTableRecordBridge p_ComponentRecord)
        {
            ComponentDataRecord = PhaseUnitComponentDataTable.GetInstanceUnsafe.CastRecord(p_ComponentRecord, true);
            
            OnBindRecordBubble(p_ModelRecord, ComponentDataRecord);
        }
        
        #endregion
    }
}