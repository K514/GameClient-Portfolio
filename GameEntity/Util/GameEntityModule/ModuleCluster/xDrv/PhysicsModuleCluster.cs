using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using k514.Mono.Feature;

namespace k514.Mono.Common
{
    public class PhysicsModuleCluster : GameEntityModuleClusterBase<PhysicsModuleCluster, PhysicsModuleDataTableQuery.TableLabel, IPhysicsModuleDataTableBridge, IPhysicsModuleDataTableRecordBridge, IPhysicsModule>
    {
        #region <Consts>

        static PhysicsModuleCluster()
        {
            _NullModule = new NullPhysics();
        }
        
        #endregion
                
        #region <Constructor>

        public PhysicsModuleCluster(IGameEntityBridge p_Entity) : base(p_Entity, GameEntityModuleTool.ModuleType.Physics, PhysicsModuleDataTableQuery.GetInstanceUnsafe)
        {
        }

        #endregion
        
        #region <Methods>

        protected override (bool, PhysicsModuleDataTableQuery.TableLabel, IPhysicsModule) SpawnModule(int p_Index)
        {
            if (PhysicsModuleDataTableQuery.GetInstanceUnsafe.TryGetLabelContext(p_Index, out var o_Label, out var o_Table, out var o_Record))
            {
                switch (o_Label)
                {
                    case PhysicsModuleDataTableQuery.TableLabel.Affine:
                        return AffinePhysics.CreateModule(o_Record, _GameEntity);
                    case PhysicsModuleDataTableQuery.TableLabel.Kinematic:
                        return KinematicPhysics.CreateModule(o_Record, _GameEntity);
                    case PhysicsModuleDataTableQuery.TableLabel.Rigidbody:
                        return RigidbodyPhysics.CreateModule(o_Record, _GameEntity);
                    case PhysicsModuleDataTableQuery.TableLabel.CharacterController:
                        return CharacterControllerPhysics.CreateModule(o_Record, _GameEntity);
                }
            }

            return default;
        }

        protected override async UniTask<(bool, PhysicsModuleDataTableQuery.TableLabel, IPhysicsModule)> SpawnModule(int p_Index, CancellationToken p_CancellationToken)
        {
            if (PhysicsModuleDataTableQuery.GetInstanceUnsafe.TryGetLabelContext(p_Index, out var o_Label, out var o_Table, out var o_Record))
            {
                switch (o_Label)
                {
                    case PhysicsModuleDataTableQuery.TableLabel.Affine:
                        return await AffinePhysics.CreateModule(o_Record, _GameEntity, p_CancellationToken);
                    case PhysicsModuleDataTableQuery.TableLabel.Kinematic:
                        return await KinematicPhysics.CreateModule(o_Record, _GameEntity, p_CancellationToken);
                    case PhysicsModuleDataTableQuery.TableLabel.Rigidbody:
                        return await RigidbodyPhysics.CreateModule(o_Record, _GameEntity, p_CancellationToken);
                    case PhysicsModuleDataTableQuery.TableLabel.CharacterController:
                        return await CharacterControllerPhysics.CreateModule(o_Record, _GameEntity, p_CancellationToken);
                }
            }

            return default;
        }
        
        #endregion
    }
}