using k514.Mono.Common;

namespace k514.Mono.Feature
{
    public partial class VfxEntityBase
    {
        #region <Fields>

        public new IParticleModelDataTableRecordBridge ModelDataRecord { get; private set; }
        public new IVfxComponentDataTableRecordBridge ComponentDataRecord { get; private set; }

        #endregion
        
        #region <Callbacks>
        
        protected override void OnBindRecordBubble(IPrefabModelDataTableRecordBridge p_ModelRecord, IPrefabComponentDataTableRecordBridge p_ComponentRecord)
        {
            ModelDataRecord = ParticleModelDataTableQuery.GetInstanceUnsafe.CastRecord(p_ModelRecord, true);
            ComponentDataRecord = VfxComponentDataTableQuery.GetInstanceUnsafe.CastRecord(p_ComponentRecord, true);
            
            base.OnBindRecordBubble(p_ModelRecord, ComponentDataRecord);
        }
        
        #endregion
    }
}