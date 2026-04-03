using k514.Mono.Common;
using UnityEngine;

namespace k514.Mono.Feature
{
    public partial class GearEntityBase
    {
        protected override AnimationModuleDataTableQuery.TableLabel GetFallbackAnimationModuleType() => GearTool.__DefaultAnimationModuleType;
        protected override ActionModuleDataTableQuery.TableLabel GetFallbackActionModuleType() => GearTool.__DefaultActionModuleType;
        protected override PhysicsModuleDataTableQuery.TableLabel GetFallbackPhysicsModuleType() => GearTool.__DefaultPhysicsModuleType;
        protected override GeometryModuleDataTableQuery.TableLabel GetFallbackGeometryModuleType() => GearTool.__DefaultGeometryModuleType;
        protected override MindModuleDataTableQuery.TableLabel GetFallbackMindModuleType() => GearTool.__DefaultMindModuleType;
        protected override RenderModuleDataTableQuery.TableLabel GetFallbackRenderModuleType() => GearTool.__DefaultRenderModuleType;
        protected override RoleModuleDataTableQuery.TableLabel GetFallbackRoleModuleType() => GearTool.__DefaultRoleModuleType;
    }
}