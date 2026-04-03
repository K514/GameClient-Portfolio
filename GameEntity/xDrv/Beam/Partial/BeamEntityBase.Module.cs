using k514.Mono.Common;
using UnityEngine;

namespace k514.Mono.Feature
{
    public partial class BeamEntityBase
    {
        protected override AnimationModuleDataTableQuery.TableLabel GetFallbackAnimationModuleType() => BeamTool.__DefaultAnimationModuleType;
        protected override ActionModuleDataTableQuery.TableLabel GetFallbackActionModuleType() => BeamTool.__DefaultActionModuleType;
        protected override PhysicsModuleDataTableQuery.TableLabel GetFallbackPhysicsModuleType() => BeamTool.__DefaultPhysicsModuleType;
        protected override GeometryModuleDataTableQuery.TableLabel GetFallbackGeometryModuleType() => BeamTool.__DefaultGeometryModuleType;
        protected override MindModuleDataTableQuery.TableLabel GetFallbackMindModuleType() => BeamTool.__DefaultMindModuleType;
        protected override RenderModuleDataTableQuery.TableLabel GetFallbackRenderModuleType() => BeamTool.__DefaultRenderModuleType;
        protected override RoleModuleDataTableQuery.TableLabel GetFallbackRoleModuleType() => BeamTool.__DefaultRoleModuleType;
    }
}