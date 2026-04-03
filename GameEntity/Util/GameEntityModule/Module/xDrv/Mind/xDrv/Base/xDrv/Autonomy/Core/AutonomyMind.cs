using System.Threading;
using Cysharp.Threading.Tasks;
using k514.Mono.Feature;

namespace k514.Mono.Common
{
    /// <summary>
    /// 파생된 논리 플로우에 의해, 유닛이 자율적으로 행동하는 사고회로 모듈 기저 클래스
    /// </summary>
    public abstract class AutonomyMind : MindBase
    {
        #region <Consts>

        protected new static (bool, MindModuleDataTableQuery.TableLabel, Module) CreateModule<Module>(Module p_Module)
            where Module : AutonomyMind
        {
            return MindBase.CreateModule(p_Module);
        }
        
        protected new static async UniTask<(bool, MindModuleDataTableQuery.TableLabel, Module)> CreateModule<Module>(Module p_Module, CancellationToken p_CancellationToken)
            where Module : AutonomyMind
        {
            return await MindBase.CreateModule(p_Module, p_CancellationToken);
        }
        
        #endregion
        
        #region <Fields>

        /// <summary>
        /// 해당 모듈의 타입
        /// </summary>
        private AutonomyModuleDataTableQuery.TableLabel _AutonomyModuleType;
        
        /// <summary>
        /// 해당 모듈을 기술하는 테이블 레코드
        /// </summary>
        protected IAutonomyModuleDataTableRecordBridge _AutonomyModuleRecord;
        
        #endregion

        #region <Constructors>

        protected AutonomyMind(AutonomyModuleDataTableQuery.TableLabel p_ModuleType, IMindModuleDataTableRecordBridge p_ModuleRecord, IGameEntityBridge p_Entity) : base(MindModuleDataTableQuery.TableLabel.Autonomy, p_ModuleRecord, p_Entity)
        {
            _AutonomyModuleType = p_ModuleType;
            _AutonomyModuleRecord = (IAutonomyModuleDataTableRecordBridge) p_ModuleRecord;
        }

        #endregion
        
        #region <Callbacks>
        
        protected override void _OnAwakeModule()
        {
            base._OnAwakeModule();

            InitOrderInterval();
        }

        protected override void OnUpdateAIState(float p_DeltaTime)
        {
        }
        
        #endregion
    }
}