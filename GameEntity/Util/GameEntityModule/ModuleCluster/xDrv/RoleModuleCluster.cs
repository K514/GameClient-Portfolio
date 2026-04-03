using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using k514.Mono.Feature;

namespace k514.Mono.Common
{
    public class RoleModuleCluster : GameEntityModuleClusterBase<RoleModuleCluster, RoleModuleDataTableQuery.TableLabel, IRoleModuleDataTableBridge, IRoleModuleDataTableRecordBridge, IRoleModule>
    {
        #region <Consts>

        static RoleModuleCluster()
        {
            _NullModule = new NullRole();
        }

        #endregion
        
        #region <Constructor>

        public RoleModuleCluster(IGameEntityBridge p_Entity) : base(p_Entity, GameEntityModuleTool.ModuleType.Role, RoleModuleDataTableQuery.GetInstanceUnsafe)
        {
        }

        #endregion
        
        #region <Methods>

        protected override (bool, RoleModuleDataTableQuery.TableLabel, IRoleModule) SpawnModule(int p_Index)
        {
            if (RoleModuleDataTableQuery.GetInstanceUnsafe.TryGetLabelContext(p_Index, out var o_Label, out var o_Table, out var o_Record))
            {
                switch (o_Label)
                {
                    case RoleModuleDataTableQuery.TableLabel.Default:
                        return DefaultRole.CreateModule(o_Record, _GameEntity);
                    case RoleModuleDataTableQuery.TableLabel.Champion:
                        return ChampionRole.CreateModule(o_Record, _GameEntity);
                    case RoleModuleDataTableQuery.TableLabel.Slave:
                        return SlaveRole.CreateModule(o_Record, _GameEntity);
                }
            }

            return default;
        }

        protected override async UniTask<(bool, RoleModuleDataTableQuery.TableLabel, IRoleModule)> SpawnModule(int p_Index, CancellationToken p_CancellationToken)
        {
            if (RoleModuleDataTableQuery.GetInstanceUnsafe.TryGetLabelContext(p_Index, out var o_Label, out var o_Table, out var o_Record))
            {
                switch (o_Label)
                {
                    case RoleModuleDataTableQuery.TableLabel.Default:
                        return await DefaultRole.CreateModule(o_Record, _GameEntity, p_CancellationToken);
                    case RoleModuleDataTableQuery.TableLabel.Champion:
                        return await ChampionRole.CreateModule(o_Record, _GameEntity, p_CancellationToken);
                    case RoleModuleDataTableQuery.TableLabel.Slave:
                        return await SlaveRole.CreateModule(o_Record, _GameEntity, p_CancellationToken);
                }
            }

            return default;
        }
        
        #endregion
    }
}