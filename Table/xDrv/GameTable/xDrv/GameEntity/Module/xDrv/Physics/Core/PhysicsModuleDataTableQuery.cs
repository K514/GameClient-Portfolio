namespace k514.Mono.Common
{
    public class PhysicsModuleDataTableQuery : MultiTableIndexBase<PhysicsModuleDataTableQuery, TableMetaData, PhysicsModuleDataTableQuery.TableLabel, IPhysicsModuleDataTableBridge, IPhysicsModuleDataTableRecordBridge>
    {
        #region <Enums>

        /// <summary>
        /// 물리 모듈 연산자 타입
        /// </summary>
        public enum TableLabel
        {
            None,
            Affine,
            Kinematic,
            Rigidbody,
            CharacterController,
        }

        #endregion
    }
}