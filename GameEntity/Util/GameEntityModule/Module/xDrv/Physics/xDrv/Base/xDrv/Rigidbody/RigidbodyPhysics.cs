using System.Threading;
using Cysharp.Threading.Tasks;
using k514.Mono.Feature;
using UnityEngine;

namespace k514.Mono.Common
{
    public partial class RigidbodyPhysics : PhysicsBase
    {
        #region <Consts>

        public static (bool, PhysicsModuleDataTableQuery.TableLabel, RigidbodyPhysics) CreateModule(IPhysicsModuleDataTableRecordBridge p_ModuleRecord, IGameEntityBridge p_Entity)
        {
            return PhysicsBase.CreateModule(new RigidbodyPhysics(p_ModuleRecord, p_Entity));
        }
        
        public static async UniTask<(bool, PhysicsModuleDataTableQuery.TableLabel, RigidbodyPhysics)> CreateModule(IPhysicsModuleDataTableRecordBridge p_ModuleRecord, IGameEntityBridge p_Entity, CancellationToken p_CancellationToken)
        {
            return await PhysicsBase.CreateModule(new RigidbodyPhysics(p_ModuleRecord, p_Entity), p_CancellationToken);
        }

        #endregion
        
        #region <Fields>

        /// <summary>
        /// 물리 모듈 레코드
        /// </summary>
        private RigidbodyModuleDataTable.TableRecord _PhysicsRecord;

        /// <summary>
        /// 강체 컴포넌트
        /// </summary>
        protected Rigidbody _Rigidbody;

        #endregion

        #region <Constructor>
        
        private RigidbodyPhysics(IPhysicsModuleDataTableRecordBridge p_ModuleRecord, IGameEntityBridge p_Entity) : base(PhysicsModuleDataTableQuery.TableLabel.Rigidbody, p_ModuleRecord, p_Entity)
        {
            _PhysicsRecord = (RigidbodyModuleDataTable.TableRecord) p_ModuleRecord;
            Affine.GetSafeComponent(ref _Rigidbody);
        }

        private void OnAwakeRigidbody()
        {
            _Rigidbody.isKinematic = false;
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