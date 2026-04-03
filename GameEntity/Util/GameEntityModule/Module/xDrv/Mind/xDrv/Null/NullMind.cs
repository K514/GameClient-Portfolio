using k514.Mono.Feature;

namespace k514.Mono.Common
{
    public class NullMind : GameEntityModuleBase, IMindModule
    {
        public NullMind() : base(GameEntityModuleTool.ModuleType.None, default, default)
        {
        }

        protected override void _OnAwakeModule()
        {
        }

        protected override void _OnSleepModule()
        {
        }

        protected override void _OnResetModule()
        {
        }

        public MindModuleDataTableQuery.TableLabel GetMindModuleType()
        {
            return MindModuleDataTableQuery.TableLabel.None;
        }

        public void ResetOrderInterval()
        {
        }

        public void ClearOrderQueue(bool p_CancelCurrentOrder)
        {
        }

        public bool ReserveIdle(MindTool.MindOrderReserveType p_Type, float p_PreDelay = 0)
        {
            return default;
        }

        public bool ReserveIdle(MindTool.MindOrderReserveType p_Type, AnimationTool.IdleMotionType p_IdleType, float p_PreDelay = 0)
        {
            return default;
        }

        public bool ReserveMove(MindTool.MindOrderReserveType p_Type, MindTool.MoveOrderParams p_Params, float p_PreDelay = 0)
        {
            return default;
        }

        public bool ReserveReturnHome(MindTool.MindOrderReserveType p_Type, float p_PreDelay = 0)
        {
            return default;
        }

        public bool ReserveDrawBackFrom(MindTool.MindOrderReserveType p_Type, IGameEntityBridge p_FromEntity, float p_PreDelay = 0)
        {
            return default;
        }

        public bool ReserveInputCommand(MindTool.MindOrderReserveType p_Type, InputEventTool.TriggerKeyType p_CommandType, float p_PreDelay = 0)
        {
            return default;
        }

        public bool ReserveDisable(MindTool.MindOrderReserveType p_Type, float p_PreDelay = 0)
        {
            return default;
        }
    }
}