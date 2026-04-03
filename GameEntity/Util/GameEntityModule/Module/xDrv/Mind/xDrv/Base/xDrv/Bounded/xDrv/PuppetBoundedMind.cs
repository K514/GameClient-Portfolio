using System.Threading;
using Cysharp.Threading.Tasks;
using k514.Mono.Feature;

namespace k514.Mono.Common
{
    public class PuppetBoundedMind : BoundedMind
    {
        #region <Consts>

        public static (bool, MindModuleDataTableQuery.TableLabel, PuppetBoundedMind) CreateModule(IMindModuleDataTableRecordBridge p_ModuleRecord, IGameEntityBridge p_Entity)
        {
            return MindBase.CreateModule(new PuppetBoundedMind(p_ModuleRecord, p_Entity));
        }
        
        public static async UniTask<(bool, MindModuleDataTableQuery.TableLabel, PuppetBoundedMind)> CreateModule(IMindModuleDataTableRecordBridge p_ModuleRecord, IGameEntityBridge p_Entity, CancellationToken p_CancellationToken)
        {
            return await MindBase.CreateModule(new PuppetBoundedMind(p_ModuleRecord, p_Entity), p_CancellationToken);
        }

        #endregion
        
        #region <Fields>

        /// <summary>
        /// 시스템 입력 이벤트 수신자
        /// </summary>
        private CommandEventReceiver _CommandEventReceiver;

        #endregion

        #region <Constructor>

        private PuppetBoundedMind(IMindModuleDataTableRecordBridge p_ModuleRecord, IGameEntityBridge p_Entity) : base(BoundedModuleDataTableQuery.TableLabel.Puppet, p_ModuleRecord, p_Entity)
        {
            _CommandEventReceiver =
                new CommandEventReceiver
                (
                    InputEventTool.InputLayerType.ControlUnit,
                    InputEventTool.InputKeyType.ArrowKey | InputEventTool.InputKeyType.TriggerKey,
                    OnHandleKeyCode
                );
        }

        #endregion
        
        #region <Callbacks>
        
        protected override void _OnAwakeModule()
        {
            base._OnAwakeModule();
            
            _CommandEventReceiver.SetReceiverBlock(false);
        }

        protected override void _OnSleepModule()
        {
            base._OnSleepModule();
            
            _CommandEventReceiver.SetReceiverBlock(true);
        }
        
        protected override void _OnResetModule()
        {
        }

        /// <summary>
        /// 인스턴스가 파기될 때 수행할 작업을 기술한다.
        /// </summary>
        protected override void OnDisposeUnmanaged()
        {
            base.OnDisposeUnmanaged();

            if (!ReferenceEquals(null, _CommandEventReceiver))
            {
                _CommandEventReceiver.Dispose();
                _CommandEventReceiver = null;
            }
        }

        private void OnHandleKeyCode(InputEventTool.InputKeyType p_Type, CommandEventParams p_Params)
        {
            ActionModule.InputCommand(p_Params);
        }

        #endregion
    }
}