using k514.Mono.Common;

namespace k514.Mono.Feature
{
    public partial class UnitBase
    {
        #region <Callbacks>

        protected override void OnCreateInstanceEventHandler()
        {
            base.OnCreateInstanceEventHandler();

            PreloadShotCreateParams();
        }

        
        protected override void OnActivateInstanceEventHandler(UnitPoolManager.ActivateParams p_ActivateParams)
        {
            base.OnActivateInstanceEventHandler(p_ActivateParams);
            
            LoadShotCreateParams();
        }

        #endregion

        #region <Methods>

        private void PreloadShotCreateParams()
        {
            var projectileSpawnIndexList = ComponentDataRecord.ProjectileSpawnIndexList;
            if (projectileSpawnIndexList.CheckCollectionSafe())
            {
                foreach (var spawnIndex in projectileSpawnIndexList)
                {
                }
            }
            else
            {
                var fallbackIndex = 1;
            }
        }
        
        private void LoadShotCreateParams()
        {
            var projectileSpawnIndexList = ComponentDataRecord.ProjectileSpawnIndexList;
            if (projectileSpawnIndexList.CheckCollectionSafe())
            {
                foreach (var spawnIndex in projectileSpawnIndexList)
                {
                    // var shotCreateParams = ProjectilePoolManager.GetInstanceUnsafe.GetCreateParams(spawnIndex, ResourceLifeCycleType.ManualUnload);
                    // _InstanceProjectileCreateParamList.Add(shotCreateParams);
                }
            }
            else
            {
                var fallbackIndex = 1;
                var shotCreateParams = ProjectilePoolManager.GetInstanceUnsafe.GetCreateParams(fallbackIndex, ResourceLifeCycleType.ManualUnload);
                // _InstanceProjectileCreateParamList.Add(shotCreateParams);
            }
        }

        #endregion
    }
}