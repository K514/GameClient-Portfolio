namespace k514.Mono.Common
{
    public partial class UIxElementBase
    {
        #region <Fields>

        public new UIxModelDataTable.TableRecord ModelDataRecord { get; private set; }
        public new UIxComponentDataTable.TableRecord ComponentDataRecord { get; private set; }

        #endregion
        
        #region <Callbacks>

        protected override void OnBindRecord(IPrefabModelDataTableRecordBridge p_ModelRecord, IPrefabComponentDataTableRecordBridge p_ComponentRecord)
        {
            ModelDataRecord = UIxModelDataTable.GetInstanceUnsafe.CastRecord(p_ModelRecord, true);
            ComponentDataRecord = UIxComponentDataTable.GetInstanceUnsafe.CastRecord(p_ComponentRecord, true);
            
            OnBindRecordBubble(ModelDataRecord, ComponentDataRecord);
        }

        #endregion
    }
}