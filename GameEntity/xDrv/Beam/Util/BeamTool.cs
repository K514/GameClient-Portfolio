using System;
using k514.Mono.Common;

namespace k514.Mono.Feature
{
    public static class BeamTool
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
        }

        
        [Flags]
        public enum BeamAttributeType
        {
            None = 0,
            
            Pierce = 1 << 0,
        }

        #endregion
    }
}