using System;
using k514.Mono.Common;

namespace k514.Mono.Feature
{
    public static class GearTool
    {
        #region <Consts>

        public const AnimationModuleDataTableQuery.TableLabel __DefaultAnimationModuleType = AnimationModuleDataTableQuery.TableLabel.None;
        public const ActionModuleDataTableQuery.TableLabel __DefaultActionModuleType = ActionModuleDataTableQuery.TableLabel.None;
        public const PhysicsModuleDataTableQuery.TableLabel __DefaultPhysicsModuleType = PhysicsModuleDataTableQuery.TableLabel.None;
        public const GeometryModuleDataTableQuery.TableLabel __DefaultGeometryModuleType = GeometryModuleDataTableQuery.TableLabel.None;
        public const MindModuleDataTableQuery.TableLabel __DefaultMindModuleType = MindModuleDataTableQuery.TableLabel.None;
        public const RenderModuleDataTableQuery.TableLabel __DefaultRenderModuleType = RenderModuleDataTableQuery.TableLabel.None;
        public const RoleModuleDataTableQuery.TableLabel __DefaultRoleModuleType = RoleModuleDataTableQuery.TableLabel.None;

        #endregion
        
        #region <Enums>

        [Flags]
        public enum ActivateParamsAttributeType
        {
            None = 0,
        }

        #endregion
    }
}