using k514.Mono.Common;

namespace k514.Mono.Feature
{
    public partial class ArmedUnit
    {
        #region <Fields>

        public new ArmedUnitComponentDataTable.TableRecord ComponentDataRecord { get; private set; }

        #endregion
        
        #region <Callbacks>
        
        protected override void OnBindRecord(IPrefabModelDataTableRecordBridge p_ModelRecord, IPrefabComponentDataTableRecordBridge p_ComponentRecord)
        {
            ComponentDataRecord = ArmedUnitComponentDataTable.GetInstanceUnsafe.CastRecord(p_ComponentRecord, true);
            
            OnBindRecordBubble(p_ModelRecord, ComponentDataRecord);
        }
        
        #endregion
    }
}