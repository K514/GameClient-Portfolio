using System.Linq;

namespace k514.Mono.Common
{
    public static partial class ActionTool
    {
        #region <Consts>

        public const float __RUN_SPEED_RATE = 1.67f;
        public const float __SCUD_COMMAND_INPUT_THRESHOLD = 0.3f;

        public const InputEventTool.TriggerKeyType __MOVE_TRIGGER = InputEventTool.TriggerKeyType.Move;
        public const InputEventTool.TriggerKeyType __JUMP_TRIGGER = InputEventTool.TriggerKeyType.C;
        public const InputEventTool.TriggerKeyType __DASH_TRIGGER = InputEventTool.TriggerKeyType.LeftControl;
        public const InputEventTool.TriggerKeyType __GUARD_TRIGGER = InputEventTool.TriggerKeyType.V;
        public const InputEventTool.TriggerKeyType __INTERACT_TRIGGER = InputEventTool.TriggerKeyType.Space;
        public const InputEventTool.TriggerKeyType __SEQ_COMMAND_TRIGGER = InputEventTool.TriggerKeyType.Z;
        public const InputEventTool.TriggerKeyType __SEQ_COMMAND_TRIGGER2 = InputEventTool.TriggerKeyType.X;
        
        public static readonly InputEventTool.TriggerKeyType[] __DEFAULT_SEQ_COMMAND_TRIGGER_KEYSET = 
            new[]
            {
                __SEQ_COMMAND_TRIGGER, __SEQ_COMMAND_TRIGGER2
            };
        
        public static readonly InputEventTool.TriggerKeyType[] __DEFAULT_SKILL_TRIGGER_KEYSET = 
            new[]
            {
                InputEventTool.TriggerKeyType.A, InputEventTool.TriggerKeyType.S, InputEventTool.TriggerKeyType.D, InputEventTool.TriggerKeyType.F
            };

        public static readonly InputEventTool.TriggerKeyType[] __DEFAULT_TRIGGER_KEYSET = 
            new[]
            {
                __MOVE_TRIGGER, __JUMP_TRIGGER, __DASH_TRIGGER, __GUARD_TRIGGER, __INTERACT_TRIGGER
            }
            .Concat(__DEFAULT_SEQ_COMMAND_TRIGGER_KEYSET)
            .Concat(__DEFAULT_SKILL_TRIGGER_KEYSET)
            .ToArray();


        #endregion
    }
}