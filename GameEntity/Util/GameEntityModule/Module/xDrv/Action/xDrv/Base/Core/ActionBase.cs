using System.Threading;
using Cysharp.Threading.Tasks;
using k514.Mono.Feature;
using UnityEngine;

namespace k514.Mono.Common
{
    /// <summary>
    /// 액션 모듈 기본 클래스
    /// </summary>
    public abstract partial class ActionBase : GameEntityModuleBase, IActionModule
    {
        #region <Consts>

        protected static (bool, ActionModuleDataTableQuery.TableLabel, Module) CreateModule<Module>(Module p_Module)
            where Module : ActionBase
        {
            if (ReferenceEquals(null, p_Module))
            {
                return (false, ActionModuleDataTableQuery.TableLabel.None, default);
            }
            else
            {
                p_Module.BindAction();
                return (true, p_Module._ActionModuleType, p_Module);
            }
        }
        
        protected static async UniTask<(bool, ActionModuleDataTableQuery.TableLabel, Module)> CreateModule<Module>(Module p_Module, CancellationToken p_CancellationToken)
            where Module : ActionBase
        {
            if (ReferenceEquals(null, p_Module))
            {
                return (false, ActionModuleDataTableQuery.TableLabel.None, default);
            }
            else
            {
                p_Module.BindAction();
                return (true, p_Module._ActionModuleType, p_Module);
            }
        }

        #endregion
        
        #region <Fields>

        /// <summary>
        /// 해당 모듈의 타입
        /// </summary>
        private ActionModuleDataTableQuery.TableLabel _ActionModuleType;

        /// <summary>
        /// 해당 모듈을 기술하는 테이블 레코드
        /// </summary>
        private IActionModuleDataTableRecordBridge _ActionModuleRecord;

        #endregion

        #region <Constructor>

        protected ActionBase(ActionModuleDataTableQuery.TableLabel p_ModuleType, IActionModuleDataTableRecordBridge p_ModuleRecord, IGameEntityBridge p_Entity) : base(GameEntityModuleTool.ModuleType.Action, p_ModuleRecord, p_Entity)
        {
            _ActionModuleType = p_ModuleType;
            _ActionModuleRecord = p_ModuleRecord;

            OnCreateTable();
            OnCreateCommand();
        }

        #endregion
        
        #region <Callbacks>

        protected override void _OnAwakeModule()
        {
            OnAwakeCommand();
            OnAwakeEventHandler();
        }

        protected override void _OnSleepModule()
        {
            OnSleepEventHandler();
        }

        protected override void _OnResetModule()
        {
            BindAction();
        }
                
        protected override void OnDisposeUnmanaged()
        {
            ClearAction();
            
            base.OnDisposeUnmanaged();
        }
        
        public override void OnModule_Update(float p_DeltaTime)
        {
            ProgressCooldown(p_DeltaTime);
        }

        public override void OnModule_Dead(bool p_Instant)
        {
            ReleaseAllInput();
        }
        
        #endregion

        #region <Methods>

        public ActionModuleDataTableQuery.TableLabel GetActionModuleType()
        {
            return _ActionModuleType;
        }
        
        #endregion
    }
}