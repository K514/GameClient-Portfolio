using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace k514.Mono.Common
{
    public partial class CharacterControllerPhysics : PhysicsBase
    {
        #region <Consts>

        private const float __Default_SlopeLimit = 45f;
        private const float __Default_MinDistance = 0.001f;
        private const float __Default_StepOffset = 0.01f;
        private const float __Default_GroundCheck_Distance_UpperBound = 0.02f;
        
        public static (bool, PhysicsModuleDataTableQuery.TableLabel, CharacterControllerPhysics) CreateModule(IPhysicsModuleDataTableRecordBridge p_ModuleRecord, IGameEntityBridge p_Entity)
        {
            return PhysicsBase.CreateModule(new CharacterControllerPhysics(p_ModuleRecord, p_Entity));
        }
        
        public static async UniTask<(bool, PhysicsModuleDataTableQuery.TableLabel, CharacterControllerPhysics)> CreateModule(IPhysicsModuleDataTableRecordBridge p_ModuleRecord, IGameEntityBridge p_Entity, CancellationToken p_CancellationToken)
        {
            return await PhysicsBase.CreateModule(new CharacterControllerPhysics(p_ModuleRecord, p_Entity), p_CancellationToken);
        }

        #endregion
        
        #region <Fields>

        /// <summary>
        /// 물리 모듈 레코드
        /// </summary>
        private CharacterControllerModuleDataTable.TableRecord _PhysicsRecord;

        /// <summary>
        /// 기본 이동 및 경사 이동을 담당하는 컬라이더
        /// </summary>
        protected CharacterController _CharacterController;

        #endregion

        #region <Constructor>
        
        private CharacterControllerPhysics(IPhysicsModuleDataTableRecordBridge p_ModuleRecord, IGameEntityBridge p_Entity) : base(PhysicsModuleDataTableQuery.TableLabel.CharacterController, p_ModuleRecord, p_Entity)
        {
            _PhysicsRecord = (CharacterControllerModuleDataTable.TableRecord) p_ModuleRecord;
            Affine.GetSafeComponent(ref _CharacterController);
        }

        private void OnAwakeCharacterController()
        {
            OnUpdateVolume();
            OnUpdateSlope();
            OnUpdateLowerMoveThresholdVector();
        }

        private void OnUpdateVolume()
        {
            _CharacterController.radius = Entity.Radius.DefaultValue;
            _CharacterController.height = Entity.Height.DefaultValue;
            _CharacterController.skinWidth = Entity.Radius.DefaultOffset;
            _CharacterController.center = Entity.ColliderCenterOffset.DefaultValue;
        }

        private void OnUpdateSlope()
        {
            _CharacterController.slopeLimit = __Default_SlopeLimit;
            _CharacterController.minMoveDistance = __Default_MinDistance;
            _CharacterController.stepOffset = Mathf.Min(__Default_StepOffset, _CharacterController.height);
        }
        
        private void OnUpdateLowerMoveThresholdVector()
        {
            _GroundCheckVector = Mathf.Max(Mathf.Max(_CharacterController.minMoveDistance + 0.001f, _CharacterController.skinWidth * 2f), __Default_GroundCheck_Distance_UpperBound) * Vector3.down;
        }

        #endregion
        
        #region <Callbacks>

        protected override void _OnAwakeModule()
        {
            OnAwakeCharacterController();

            base._OnAwakeModule();
        }
        
        public override void OnModule_Update_Scale()
        {
            base.OnModule_Update_Scale();

            OnUpdateLowerMoveThresholdVector();
        }
        
        #endregion
    }
}