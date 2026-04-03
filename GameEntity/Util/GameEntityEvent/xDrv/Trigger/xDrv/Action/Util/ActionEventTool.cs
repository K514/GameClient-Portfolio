using System;

namespace k514.Mono.Common
{
    public static class ActionEventTool
    {
        #region <Enums>

        [Flags]
        public enum ActionEventType
        {
            None = 0,
            
            Move = 1 << 0,
            Jump = 1 << 1,
            Dash = 1 << 2,
            Guard = 1 << 3,
            
            SkillGroup0 = 1 << 4,
            SkillGroup1 = 1 << 5,
            SkillGroup2 = 1 << 6,
            SkillGroup3 = 1 << 7,
            
            SkillMask = SkillGroup0 | SkillGroup1 | SkillGroup2 | SkillGroup3,
            All = Move | Jump | Dash | Guard | SkillMask,
        }

        [Flags]
        public enum ActionEventState
        {
            None = 0,
            Holding = 1 << 0,
            Selected = 1 << 1,
        }

        #endregion
    }
}