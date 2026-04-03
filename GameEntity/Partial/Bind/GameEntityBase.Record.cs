namespace k514.Mono.Common
{
    public partial class GameEntityBase<Content, CreateParams, ActivateParams>
    {
        #region <Fields>

        public new IGameEntityModelDataTableRecordBridge ModelDataRecord { get; private set; }
        public new IGameEntityComponentDataTableRecordBridge ComponentDataRecord { get; private set; }

        #endregion
        
        #region <Callbacks>

        protected override void OnBindRecordBubble(IPrefabModelDataTableRecordBridge p_ModelRecord, IPrefabComponentDataTableRecordBridge p_ComponentRecord)
        {
            ModelDataRecord = GameEntityModelDataTableQuery.GetInstanceUnsafe.CastRecord(p_ModelRecord, true);
            ComponentDataRecord = GameEntityComponentDataTableQuery.GetInstanceUnsafe.CastRecord(p_ComponentRecord, true);
            
            base.OnBindRecordBubble(p_ModelRecord, p_ComponentRecord);
        }

        #endregion
    }
}