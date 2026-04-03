using k514.Mono.Common;

namespace k514.Mono.Feature
{
    public partial class ProjectileEntityBase
    {
        #region <Fields>

        /// <summary>
        /// 해당 투사체의 컴포넌트 데이터
        /// </summary>
        public new IProjectileComponentDataTableRecordBridge ComponentDataRecord { get; private set; }

        #endregion
        
        #region <Callbacks>

        protected override void OnBindRecordBubble(IPrefabModelDataTableRecordBridge p_ModelRecord, IPrefabComponentDataTableRecordBridge p_ComponentRecord)
        {
            ComponentDataRecord = ProjectileComponentDataTableQuery.GetInstanceUnsafe.CastRecord(p_ComponentRecord, true);
            
            base.OnBindRecordBubble(ModelDataRecord, ComponentDataRecord);
        }
        
        #endregion
    }
}