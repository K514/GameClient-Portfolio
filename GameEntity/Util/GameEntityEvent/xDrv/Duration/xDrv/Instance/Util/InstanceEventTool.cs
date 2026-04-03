using System;

namespace k514.Mono.Common
{
    public static class InstanceEventTool
    {
        #region <Enums>

        public enum InstanceEventType
        {
            None,
            Hit,
            Launch,
        }

        #endregion

        #region <Structs>

        public struct InstanceEventPreset
        {
            #region <Fields>

            public readonly InstanceEventType EventId;
            public readonly InstanceEventHandlerActivateParams ActivateParams;
            public readonly bool ValidFlag;

            #endregion

            #region <Constructor>

            public InstanceEventPreset(InstanceEventTool.InstanceEventType p_Index, InstanceEventHandlerActivateParams p_Params)
            {
                EventId = p_Index;
                ActivateParams = p_Params;
                ValidFlag = true;
            }

            #endregion
        }

        #endregion
    }
}