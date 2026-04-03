using System.Threading;
using Cysharp.Threading.Tasks;
using k514.Mono.Feature;
using xk514;

namespace k514.Mono.Common
{
    /// <summary>
    /// 유닛의 행동이 어떤 논리 플로우에 의해 결정되는 것이 아닌 외부 입력에 의해 결정되는 인공지능 클래스
    /// </summary>
    public abstract partial class BoundedMind : MindBase
    {
        #region <Consts>

        protected new static (bool, MindModuleDataTableQuery.TableLabel, Module) CreateModule<Module>(Module p_Module)
            where Module : BoundedMind
        {
            return MindBase.CreateModule(p_Module);
        }
        
        protected new static async UniTask<(bool, MindModuleDataTableQuery.TableLabel, Module)> CreateModule<Module>(Module p_Module, CancellationToken p_CancellationToken)
            where Module : BoundedMind
        {
            return await MindBase.CreateModule(p_Module, p_CancellationToken);
        }
        
        #endregion

        #region <Fields>

        /// <summary>
        /// 해당 모듈의 타입
        /// </summary>
        private BoundedModuleDataTableQuery.TableLabel _BoundedModuleType;

        /// <summary>
        /// 해당 모듈을 기술하는 테이블 레코드
        /// </summary>
        private IBoundedModuleDataTableRecordBridge _BoundedModuleRecord;
        
        #endregion

        #region <Constructor>

        protected BoundedMind(BoundedModuleDataTableQuery.TableLabel p_ModuleType, IMindModuleDataTableRecordBridge p_ModuleRecord, IGameEntityBridge p_Entity) : base(MindModuleDataTableQuery.TableLabel.Bounded, p_ModuleRecord, p_Entity)
        {
            _BoundedModuleType = p_ModuleType;
            _BoundedModuleRecord = (IBoundedModuleDataTableRecordBridge) p_ModuleRecord;
        }

        #endregion
        
        #region <Callbacks>
        
        protected override void _OnAwakeModule()
        {
            base._OnAwakeModule();
#if UNITY_EDITOR
            if (CustomDebug.AIStateName)
            {
                Entity.SetPostFix("[Passivity]");
            }
#endif
        }
        
        protected override void OnUpdateAIState(float p_DeltaTime)
        {
        }

        protected override void OnUpdateAIState_TimeBlock()
        {
        }
        
        #endregion
    }
}