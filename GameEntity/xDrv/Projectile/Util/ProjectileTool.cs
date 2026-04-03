using System;
using k514.Mono.Common;

namespace k514.Mono.Feature
{
    public static class ProjectileTool
    {
        #region <Consts>

        public const AnimationModuleDataTableQuery.TableLabel __DefaultAnimationModuleType = AnimationModuleDataTableQuery.TableLabel.None;
        public const ActionModuleDataTableQuery.TableLabel __DefaultActionModuleType = ActionModuleDataTableQuery.TableLabel.Default;
        public const PhysicsModuleDataTableQuery.TableLabel __DefaultPhysicsModuleType = PhysicsModuleDataTableQuery.TableLabel.Kinematic;
        public const GeometryModuleDataTableQuery.TableLabel __DefaultGeometryModuleType = GeometryModuleDataTableQuery.TableLabel.Affine;
        public const MindModuleDataTableQuery.TableLabel __DefaultMindModuleType = MindModuleDataTableQuery.TableLabel.None;
        public const RenderModuleDataTableQuery.TableLabel __DefaultRenderModuleType = RenderModuleDataTableQuery.TableLabel.None;
        public const RoleModuleDataTableQuery.TableLabel __DefaultRoleModuleType = RoleModuleDataTableQuery.TableLabel.None;

        #endregion
        
        #region <Enums>

        [Flags]
        public enum ActivateParamsAttributeType
        {
            None = 0,
            
            GivePierce = 1 << 0,
            GiveKnockBack = 1 << 1,
            GiveNonCollision = 1 << 2,
        }

        
        [Flags]
        public enum ProjectileAttributeType
        {
            None = 0,
            
            Pierce = 1 << 0,
            KnockBack = 1 << 1,
            NonCollision = 1 << 2,
        }

        #endregion
    }
}