using k514.Mono.Feature;
using UnityEngine;
using UnityEngine.AI;

namespace k514.Mono.Common
{
    public class NullGeometry : GameEntityModuleBase, IGeometryModule
    {
        #region <Constructor>
        
        public NullGeometry() : base(GameEntityModuleTool.ModuleType.None, default, default)
        {
        }
        
        #endregion

        #region <Callbacks>

        protected override void _OnAwakeModule()
        {
        }

        protected override void _OnSleepModule()
        {
        }

        protected override void _OnResetModule()
        {
        }

        #endregion

        #region <Methods>

        public GeometryModuleDataTableQuery.TableLabel GetGeometryModuleType()
        {
            return default;
        }

        public bool IsOnNavigate()
        {
            return default;
        }

        public bool NavigateTo(GeometryTool.NavigateDestinationPreset p_Preset)
        {
            return default;
        }

        public void StopNavigate()
        {
        }

        public (bool, GeometryTool.NavigateDestinationPreset) TryGetDestination()
        {
            return default;
        }

        #endregion
    }
}
