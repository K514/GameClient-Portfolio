using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace k514.Mono.Common
{
    public partial class KinematicPhysics : PhysicsBase
    {
        #region <Consts>

        public static (bool, PhysicsModuleDataTableQuery.TableLabel, KinematicPhysics) CreateModule(IPhysicsModuleDataTableRecordBridge p_ModuleRecord, IGameEntityBridge p_Entity)
        {
            return PhysicsBase.CreateModule(new KinematicPhysics(p_ModuleRecord, p_Entity));
        }
        
        public static async UniTask<(bool, PhysicsModuleDataTableQuery.TableLabel, KinematicPhysics)> CreateModule(IPhysicsModuleDataTableRecordBridge p_ModuleRecord, IGameEntityBridge p_Entity, CancellationToken p_CancellationToken)
        {
            return await PhysicsBase.CreateModule(new KinematicPhysics(p_ModuleRecord, p_Entity), p_CancellationToken);
        }

        #endregion
        
        #region <Fields>

        /// <summary>
        /// 물리 모듈 레코드
        /// </summary>
        private KinematicModuleDataTable.TableRecord _PhysicsRecord;

        /// <summary>
        /// 강체 컴포넌트
        /// </summary>
        protected Rigidbody _Rigidbody;

        #endregion

        #region <Constructor>
        
        private KinematicPhysics(IPhysicsModuleDataTableRecordBridge p_ModuleRecord, IGameEntityBridge p_Entity) : base(PhysicsModuleDataTableQuery.TableLabel.Kinematic, p_ModuleRecord, p_Entity)
        {
            _PhysicsRecord = (KinematicModuleDataTable.TableRecord) p_ModuleRecord;
            Affine.GetSafeComponent(ref _Rigidbody);
        }

        private void OnAwakeRigidbody()
        {
            _Rigidbody.isKinematic = true;
            _Rigidbody.useGravity = false;
            _Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            _Rigidbody.mass = Entity.Mass.CurrentValue;
        }
        
        #endregion
        
        #region <Callbacks>

        protected override void _OnAwakeModule()
        {
            OnAwakeRigidbody();
            
            base._OnAwakeModule();
        }
        
        #endregion
    }
}