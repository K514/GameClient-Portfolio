using System.Threading;
using Cysharp.Threading.Tasks;
using k514.Mono.Feature;

namespace k514.Mono.Common
{
    /// <summary>
    /// 개체 인공지능 모듈 기저 클래스
    /// </summary>
    public abstract partial class MindBase : GameEntityModuleBase, IMindModule
    {
        #region <Consts>
        
        protected static (bool, MindModuleDataTableQuery.TableLabel, Module) CreateModule<Module>(Module p_Module)
            where Module : MindBase
        {
            if (ReferenceEquals(null, p_Module))
            {
                return (false, MindModuleDataTableQuery.TableLabel.None, default);
            }
            else
            {
                return (true, p_Module._MindModuleType, p_Module);
            }
        }
        
        protected static async UniTask<(bool, MindModuleDataTableQuery.TableLabel, Module)> CreateModule<Module>(Module p_Module, CancellationToken p_CancellationToken)
            where Module : MindBase
        {
            if (ReferenceEquals(null, p_Module))
            {
                return (false, MindModuleDataTableQuery.TableLabel.None, default);
            }
            else
            {
                return (true, p_Module._MindModuleType, p_Module);
            }
        }
        
        #endregion

        #region <Fields>

        /// <summary>
        /// 해당 모듈의 타입
        /// </summary>
        private MindModuleDataTableQuery.TableLabel _MindModuleType;

        /// <summary>
        /// 해당 모듈을 기술하는 테이블 레코드
        /// </summary>
        private IMindModuleDataTableRecordBridge _MindModuleRecord;
        
        #endregion

        #region <Constructor>
        
        protected MindBase(MindModuleDataTableQuery.TableLabel p_ModuleType, IMindModuleDataTableRecordBridge p_ModuleRecord, IGameEntityBridge p_Entity) : base(GameEntityModuleTool.ModuleType.Mind, p_ModuleRecord, p_Entity)
        {
            _MindModuleType = p_ModuleType;
            _MindModuleRecord = p_ModuleRecord;

            OnCreateOrder();
        }
        
        #endregion
        
        #region <Callbacks>

        protected override void _OnAwakeModule()
        {
            OnAwakeOrder();
        }
        
        protected override void _OnSleepModule()
        {
            OnSleepOrder();
        }

        protected override void _OnResetModule()
        {
        }

        #endregion

        #region <Methods>

        public MindModuleDataTableQuery.TableLabel GetMindModuleType()
        {
            return _MindModuleType;
        }

        #endregion
    }
}