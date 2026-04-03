using k514.Mono.Common;
using UnityEngine;

namespace k514.Mono.Feature
{
    public partial class VfxEntityBase
    {
        protected override AnimationModuleDataTableQuery.TableLabel GetFallbackAnimationModuleType() => VfxTool.__DefaultAnimationModuleType;
        protected override ActionModuleDataTableQuery.TableLabel GetFallbackActionModuleType() => VfxTool.__DefaultActionModuleType;
        protected override PhysicsModuleDataTableQuery.TableLabel GetFallbackPhysicsModuleType() => VfxTool.__DefaultPhysicsModuleType;
        protected override GeometryModuleDataTableQuery.TableLabel GetFallbackGeometryModuleType() => VfxTool.__DefaultGeometryModuleType;
        protected override MindModuleDataTableQuery.TableLabel GetFallbackMindModuleType() => VfxTool.__DefaultMindModuleType;
        protected override RenderModuleDataTableQuery.TableLabel GetFallbackRenderModuleType() => VfxTool.__DefaultRenderModuleType;
        protected override RoleModuleDataTableQuery.TableLabel GetFallbackRoleModuleType() => VfxTool.__DefaultRoleModuleType;
    }
}