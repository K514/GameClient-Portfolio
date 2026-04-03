using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace k514.Mono.Common
{
    public class GeometryModuleCluster : GameEntityModuleClusterBase<GeometryModuleCluster, GeometryModuleDataTableQuery.TableLabel, IGeometryModuleDataTableBridge, IGeometryModuleDataTableRecordBridge, IGeometryModule>
    {
        #region <Consts>

        static GeometryModuleCluster()
        {
            _NullModule = new NullGeometry();
        }

        #endregion
        
        #region <Constructor>

        public GeometryModuleCluster(IGameEntityBridge p_Entity) : base(p_Entity, GameEntityModuleTool.ModuleType.Geometry, GeometryModuleDataTableQuery.GetInstanceUnsafe)
        {
        }

        #endregion
        
        #region <Methods>

        protected override (bool, GeometryModuleDataTableQuery.TableLabel, IGeometryModule) SpawnModule(int p_Index)
        {
            if (GeometryModuleDataTableQuery.GetInstanceUnsafe.TryGetLabelContext(p_Index, out var o_Label, out var o_Table, out var o_Record))
            {
                switch (o_Label)
                {
                    case GeometryModuleDataTableQuery.TableLabel.Affine:
                        return AffineGeometry.CreateModule(o_Record, _GameEntity);
                    case GeometryModuleDataTableQuery.TableLabel.NavMesh:
                        return NavMeshGeometry.CreateModule(o_Record, _GameEntity);
                    case GeometryModuleDataTableQuery.TableLabel.AStar:
                        return AStarGeometry.CreateModule(o_Record, _GameEntity);
                }
            }

            return default;
        }

        protected override async UniTask<(bool, GeometryModuleDataTableQuery.TableLabel, IGeometryModule)> SpawnModule(int p_Index, CancellationToken p_CancellationToken)
        {
            if (GeometryModuleDataTableQuery.GetInstanceUnsafe.TryGetLabelContext(p_Index, out var o_Label, out var o_Table, out var o_Record))
            {
                switch (o_Label)
                {
                    case GeometryModuleDataTableQuery.TableLabel.Affine:
                        return await AffineGeometry.CreateModule(o_Record, _GameEntity, p_CancellationToken);
                    case GeometryModuleDataTableQuery.TableLabel.NavMesh:
                        return await NavMeshGeometry.CreateModule(o_Record, _GameEntity, p_CancellationToken);
                    case GeometryModuleDataTableQuery.TableLabel.AStar:
                        return await AStarGeometry.CreateModule(o_Record, _GameEntity, p_CancellationToken);
                }
            }

            return default;
        }
        
        #endregion
    }
}