using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using k514.Mono.Feature;
using UnityEngine;

namespace k514.Mono.Common
{
    public abstract partial class PhysicsBase : GameEntityModuleBase, IPhysicsModule
    {
        #region <Consts>

        protected static (bool, PhysicsModuleDataTableQuery.TableLabel, Module) CreateModule<Module>(Module p_Module)
            where Module : PhysicsBase
        {
            if (ReferenceEquals(null, p_Module))
            {
                return (false, PhysicsModuleDataTableQuery.TableLabel.None, default);
            }
            else
            {
                return (true, p_Module._PhysicsModuleType, p_Module);
            }
        }
        
        protected static async UniTask<(bool, PhysicsModuleDataTableQuery.TableLabel, Module)> CreateModule<Module>(Module p_Module, CancellationToken p_CancellationToken)
            where Module : PhysicsBase
        {
            if (ReferenceEquals(null, p_Module))
            {
                return (false, PhysicsModuleDataTableQuery.TableLabel.None, default);
            }
            else
            {
                return (true, p_Module._PhysicsModuleType, p_Module);
            }
        }

        #endregion
        
        #region <Fields>

        /// <summary>
        /// 물리 모듈 타입
        /// </summary>
        protected PhysicsModuleDataTableQuery.TableLabel _PhysicsModuleType;

        /// <summary>
        /// 물리 모듈 레코드
        /// </summary>
        protected IPhysicsModuleDataTableRecordBridge _PhysicsModuleRecord;
        
        #endregion

        #region <Constructor>

        protected PhysicsBase(PhysicsModuleDataTableQuery.TableLabel p_ModuleType, IPhysicsModuleDataTableRecordBridge p_ModuleRecord, IGameEntityBridge p_Entity) : base(GameEntityModuleTool.ModuleType.Physics, p_ModuleRecord, p_Entity)
        {
            _PhysicsModuleType = p_ModuleType;
            _PhysicsModuleRecord = p_ModuleRecord;
            _PhysicsSystemTable = PhysicsTool._ForceTypeEnumerator.ToDictionary(forceType => forceType, forceType => new PhysicsTool.PhysicsSystem(Entity, forceType));
            _JumpPhysicsSystem = _PhysicsSystemTable[PhysicsTool.ForceType.Jump];
            _GravityPhysicsSystem = _PhysicsSystemTable[PhysicsTool.ForceType.Gravity];
        }

        #endregion
        
        #region <Callbacks>

        protected override void _OnAwakeModule()
        {
            OnAwakeMainCalc();
            OnAwakeForce();
            OnAwakeState();
            OnAwakeStamp();
        }

        protected override void _OnSleepModule()
        {
            OnSleepStamp();
            OnSleepState();
            OnSleepForce();
            OnSleepMainCalc();
        }
        
        protected override void _OnResetModule()
        {
        }

#if ADD_FIXED_UPDATE_GAME_ENTITY
        public void OnFixedUpdate(float p_DeltaTime)
        {
            OnPhysicsUpdate(p_DeltaTime);
        }
#else
        public override void OnModule_Update(float p_DeltaTime)
        {
            base.OnModule_Update(p_DeltaTime);
         
            OnPhysicsUpdate(p_DeltaTime);
        }
#endif

        private void OnPhysicsUpdate(float p_DeltaTime)
        {
            UpdateAerialState(p_DeltaTime);
            UpdateGravity(p_DeltaTime);
            UpdateVelocity(p_DeltaTime);
            ApplyVelocity(p_DeltaTime);
            ApplyDampingForce();
        }

        #endregion

        #region <Methods>

        public PhysicsModuleDataTableQuery.TableLabel GetPhysicsModuleType()
        {
            return _PhysicsModuleType;
        }

        #endregion
    }
}