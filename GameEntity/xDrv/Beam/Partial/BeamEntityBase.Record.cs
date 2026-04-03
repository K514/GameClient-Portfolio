using k514.Mono.Common;

namespace k514.Mono.Feature
{
    public partial class BeamEntityBase
    {
        #region <Fields>

        public new BeamModelDataTable.TableRecord ModelDataRecord { get; private set; }
        public new IBeamComponentDataTableRecordBridge ComponentDataRecord { get; private set; }
        
        #endregion
        
        #region <Callbacks>
        
        protected override void OnBindRecordBubble(IPrefabModelDataTableRecordBridge p_ModelRecord, IPrefabComponentDataTableRecordBridge p_ComponentRecord)
        {
            ModelDataRecord = BeamModelDataTable.GetInstanceUnsafe.CastRecord(p_ModelRecord, true);
            ComponentDataRecord = BeamComponentDataTableQuery.GetInstanceUnsafe.CastRecord(p_ComponentRecord, true);
            
            base.OnBindRecordBubble(p_ModelRecord, p_ComponentRecord);
        }

        #endregion
    }
}