using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace k514.Mono.Common
{
    public abstract partial class AnimationBase : GameEntityModuleBase, IAnimationModule
    {
        #region <Consts>

        protected static (bool, AnimationModuleDataTableQuery.TableLabel, Module) CreateModule<Module>(Module p_Module)
            where Module : AnimationBase
        {
            if (ReferenceEquals(null, p_Module))
            {
                return (false, AnimationModuleDataTableQuery.TableLabel.None, default);
            }
            else
            {
                return (true, p_Module._AnimationModuleType, p_Module);
            }
        }
        
        protected static async UniTask<(bool, AnimationModuleDataTableQuery.TableLabel, Module)> CreateModule<Module>(Module p_Module, CancellationToken p_CancellationToken)
            where Module : AnimationBase
        {
            if (ReferenceEquals(null, p_Module))
            {
                return (false, AnimationModuleDataTableQuery.TableLabel.None, default);
            }
            else
            {
                return (true, p_Module._AnimationModuleType, p_Module);
            }
        }

        #endregion
        
        #region <Fields>

        private AnimationModuleDataTableQuery.TableLabel _AnimationModuleType;
        protected IAnimationModuleDataTableRecordBridge _AnimationModuleRecord;
        
        #endregion

        #region <Constructor>

        protected AnimationBase(AnimationModuleDataTableQuery.TableLabel p_ModuleType, IAnimationModuleDataTableRecordBridge p_ModuleRecord, IGameEntityBridge p_Entity) : base(GameEntityModuleTool.ModuleType.Animation, p_ModuleRecord, p_Entity)
        {
            _AnimationModuleType = p_ModuleType;
            _AnimationModuleRecord = p_ModuleRecord;
        }

        #endregion
        
        #region <Callbacks>

        protected override void _OnAwakeModule()
        {
            ResetAnimationSpeed();
            SetAnimationResume();
        }
        
        public override void OnModule_Dead(bool p_Instant)
        {
            if (!p_Instant)
            {
                AnimationModule.SwitchMotion(AnimationTool.MotionType.Dead, AnimationTool.MotionTransitionType.Restrict);
            }
        }
        
        #endregion
       
        #region <Methods>
        
        public AnimationModuleDataTableQuery.TableLabel GetAnimationModuleType()
        {
            return _AnimationModuleType;
        }

        #endregion
    }
}