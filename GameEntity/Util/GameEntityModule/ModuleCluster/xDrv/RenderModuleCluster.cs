using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using k514.Mono.Feature;

#if !SERVER_DRIVE
namespace k514.Mono.Common
{
    public class RenderModuleCluster : GameEntityModuleClusterBase<RenderModuleCluster, RenderModuleDataTableQuery.TableLabel, IRenderModuleDataTableBridge, IRenderModuleDataTableRecordBridge, IRenderModule>
    {
        #region <Consts>

        static RenderModuleCluster()
        {
            _NullModule = new NullRender();
        }
        
        #endregion
                
        #region <Constructor>

        public RenderModuleCluster(IGameEntityBridge p_Entity) : base(p_Entity, GameEntityModuleTool.ModuleType.Render, RenderModuleDataTableQuery.GetInstanceUnsafe)
        {
        }

        #endregion
        
        #region <Methods>

        protected override (bool, RenderModuleDataTableQuery.TableLabel, IRenderModule) SpawnModule(int p_Index)
        {
            if (RenderModuleDataTableQuery.GetInstanceUnsafe.TryGetLabelContext(p_Index, out var o_Label, out var o_Table, out var o_Record))
            {
                switch (o_Label)
                {
                    case RenderModuleDataTableQuery.TableLabel.Default:
                        return DefaultRender.CreateModule(o_Record, _GameEntity);
                }
            }

            return default;
        }

        protected override async UniTask<(bool, RenderModuleDataTableQuery.TableLabel, IRenderModule)> SpawnModule(int p_Index, CancellationToken p_CancellationToken)
        {
            if (RenderModuleDataTableQuery.GetInstanceUnsafe.TryGetLabelContext(p_Index, out var o_Label, out var o_Table, out var o_Record))
            {
                switch (o_Label)
                {
                    case RenderModuleDataTableQuery.TableLabel.Default:
                        return await DefaultRender.CreateModule(o_Record, _GameEntity, p_CancellationToken);
                }
            }

            return default;
        }
        
        #endregion
    }
}
#endif