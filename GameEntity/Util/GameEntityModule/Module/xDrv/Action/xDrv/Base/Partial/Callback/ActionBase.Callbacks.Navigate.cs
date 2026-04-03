using UnityEngine;

namespace k514.Mono.Common
{
    public partial class ActionBase
    {
        #region <Callbacks>
        
        public override void OnNavigateStart(GeometryTool.NavigationResultPreset p_Preset)
        {
            base.OnNavigateStart(p_Preset);

            var commandParams = 
                new CommandEventParams
                (
                    InputEventTool.InputLayerType.ControlUnit, ActionTool.__MOVE_TRIGGER, InputEventTool.InputStateType.Press, 
                    0, p_Preset.NavigationProgressPreset.UV, 
                    p_Preset.StartTimeStamp, p_Preset.DeltaTime,
                    MindTool.__MIND_COMMAND_PRIORITY_DEFAULT
                );

            InputCommand(commandParams);
        }
        
        public override void OnNavigateProgress(GeometryTool.NavigationResultPreset p_Preset)
        {
            base.OnNavigateProgress(p_Preset);

            var commandParams = 
                new CommandEventParams
                (
                    InputEventTool.InputLayerType.ControlUnit, ActionTool.__MOVE_TRIGGER, InputEventTool.InputStateType.Holding, 
                    0, p_Preset.NavigationProgressPreset.UV, 
                    p_Preset.StartTimeStamp, p_Preset.DeltaTime,
                    MindTool.__MIND_COMMAND_PRIORITY_DEFAULT
                );
            
            InputCommand(commandParams);
        }
        
        public override void OnNavigateEnd(GeometryTool.NavigationResultPreset p_Preset)
        {
            base.OnNavigateEnd(p_Preset);
            
            var commandParams = 
                new CommandEventParams
                (
                    InputEventTool.InputLayerType.ControlUnit, ActionTool.__MOVE_TRIGGER, InputEventTool.InputStateType.Release, 
                    0, p_Preset.NavigationProgressPreset.UV, 
                    p_Preset.StartTimeStamp, p_Preset.DeltaTime,
                    MindTool.__MIND_COMMAND_PRIORITY_DEFAULT
                );
            
            InputCommand(commandParams);
        }

        #endregion
    }
}