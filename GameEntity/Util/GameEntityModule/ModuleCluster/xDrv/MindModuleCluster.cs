using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace k514.Mono.Common
{
    public class MindModuleCluster : GameEntityModuleClusterBase<MindModuleCluster, MindModuleDataTableQuery.TableLabel, IMindModuleDataTableBridge<IMindModuleDataTableRecordBridge>, IMindModuleDataTableRecordBridge, IMindModule>
    {
        #region <Consts>

        static MindModuleCluster()
        {
            _NullModule = new NullMind();
        }

        #endregion
                
        #region <Constructor>

        public MindModuleCluster(IGameEntityBridge p_Entity) : base(p_Entity, GameEntityModuleTool.ModuleType.Mind, MindModuleDataTableQuery.GetInstanceUnsafe)
        {
        }

        #endregion
        
        #region <Methods>

        protected override (bool, MindModuleDataTableQuery.TableLabel, IMindModule) SpawnModule(int p_Index)
        {
            if (MindModuleDataTableQuery.GetInstanceUnsafe.TryGetLabelContext(p_Index, out var o_Label, out var o_Table, out var o_Record))
            {
                switch (o_Label)
                {
                    case MindModuleDataTableQuery.TableLabel.Bounded:
                    {
                        if (BoundedModuleDataTableQuery.GetInstanceUnsafe.TryGetLabel(p_Index, out var o_SubLabel))
                        {
                            switch (o_SubLabel)
                            {
                                case BoundedModuleDataTableQuery.TableLabel.Dummy:
                                    return DummyBoundedMind.CreateModule(o_Record, _GameEntity);
                                case BoundedModuleDataTableQuery.TableLabel.Puppet:
                                    return PuppetBoundedMind.CreateModule(o_Record, _GameEntity);
                            }
                        }
                        break;
                    }
                    case MindModuleDataTableQuery.TableLabel.Autonomy:
                    {
                        if (AutonomyModuleDataTableQuery.GetInstanceUnsafe.TryGetLabel(p_Index, out var o_SubLabel))
                        {
                            switch (o_SubLabel)
                            {
                                case AutonomyModuleDataTableQuery.TableLabel.Melee:
                                    return MeleeAutonomyMind.CreateModule(o_Record, _GameEntity);
                                case AutonomyModuleDataTableQuery.TableLabel.Coward:
                                    return CowardAutonomyMind.CreateModule(o_Record, _GameEntity);
                                case AutonomyModuleDataTableQuery.TableLabel.Following:
                                    return FollowingAutonomyMind.CreateModule(o_Record, _GameEntity);
                            }
                        }
                        break;
                    }
                }
            }

            return default;
        }

        protected override async UniTask<(bool, MindModuleDataTableQuery.TableLabel, IMindModule)> SpawnModule(int p_Index, CancellationToken p_CancellationToken)
        {
            if (MindModuleDataTableQuery.GetInstanceUnsafe.TryGetLabelContext(p_Index, out var o_Label, out var o_Table, out var o_Record))
            {
                switch (o_Label)
                {
                    case MindModuleDataTableQuery.TableLabel.Bounded:
                    {
                        if (BoundedModuleDataTableQuery.GetInstanceUnsafe.TryGetLabel(p_Index, out var o_SubLabel))
                        {
                            switch (o_SubLabel)
                            {
                                case BoundedModuleDataTableQuery.TableLabel.Dummy:
                                    return await DummyBoundedMind.CreateModule(o_Record, _GameEntity, p_CancellationToken);
                                case BoundedModuleDataTableQuery.TableLabel.Puppet:
                                    return await PuppetBoundedMind.CreateModule(o_Record, _GameEntity, p_CancellationToken);
                            }
                        }
                        break;
                    }
                    case MindModuleDataTableQuery.TableLabel.Autonomy:
                    {
                        if (AutonomyModuleDataTableQuery.GetInstanceUnsafe.TryGetLabel(p_Index, out var o_SubLabel))
                        {
                            switch (o_SubLabel)
                            {
                                case AutonomyModuleDataTableQuery.TableLabel.Melee:
                                    return await MeleeAutonomyMind.CreateModule(o_Record, _GameEntity, p_CancellationToken);
                                case AutonomyModuleDataTableQuery.TableLabel.Coward:
                                    return await CowardAutonomyMind.CreateModule(o_Record, _GameEntity, p_CancellationToken);
                                case AutonomyModuleDataTableQuery.TableLabel.Following:
                                    return await FollowingAutonomyMind.CreateModule(o_Record, _GameEntity, p_CancellationToken);
                            }
                        }
                        break;
                    }
                }
            }

            return default;
        }
        
        #endregion
    }
}