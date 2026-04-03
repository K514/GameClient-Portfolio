using k514.Mono.Common;
using UnityEngine;

namespace k514.Mono.Feature
{
    public partial class ProjectileEntityBase
    {
        protected override AnimationModuleDataTableQuery.TableLabel GetFallbackAnimationModuleType() => ProjectileTool.__DefaultAnimationModuleType;
        protected override ActionModuleDataTableQuery.TableLabel GetFallbackActionModuleType() => ProjectileTool.__DefaultActionModuleType;
        protected override PhysicsModuleDataTableQuery.TableLabel GetFallbackPhysicsModuleType() => ProjectileTool.__DefaultPhysicsModuleType;
        protected override GeometryModuleDataTableQuery.TableLabel GetFallbackGeometryModuleType() => ProjectileTool.__DefaultGeometryModuleType;
        protected override MindModuleDataTableQuery.TableLabel GetFallbackMindModuleType() => ProjectileTool.__DefaultMindModuleType;
        protected override RenderModuleDataTableQuery.TableLabel GetFallbackRenderModuleType() => ProjectileTool.__DefaultRenderModuleType;
        protected override RoleModuleDataTableQuery.TableLabel GetFallbackRoleModuleType() => ProjectileTool.__DefaultRoleModuleType;
    }
}