using System.Collections.Generic;
using k514.Mono.Feature;

namespace k514.Mono.Common
{
    public class VictoryLocation : LocationBase
    {
        protected override void PreloadLocation(List<SceneTool.SceneLocationPivotMeta> p_MetaSet)
        {
        }

        protected override void ActivateLocation(SceneTool.SceneLocationWaveType p_Type, List<SceneTool.SceneLocationPivotMeta> p_MetaSet)
        {
            switch (p_Type)
            {
                case SceneTool.SceneLocationWaveType.Default:
                    GameManager.GetInstanceUnsafe.ClearStage();
                    break;
                default:
                case SceneTool.SceneLocationWaveType.FirstOnce:
                    break;
            }
        }
    }
}