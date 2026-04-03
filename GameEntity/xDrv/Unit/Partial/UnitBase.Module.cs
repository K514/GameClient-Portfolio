using k514.Mono.Common;

namespace k514.Mono.Feature
{
    public partial class UnitBase
    {
        protected override AnimationModuleDataTableQuery.TableLabel GetFallbackAnimationModuleType() => UnitTool.__DefaultAnimationModuleType;
        protected override ActionModuleDataTableQuery.TableLabel GetFallbackActionModuleType() => UnitTool.__DefaultActionModuleType;
        protected override PhysicsModuleDataTableQuery.TableLabel GetFallbackPhysicsModuleType() => UnitTool.__DefaultPhysicsModuleType;
        protected override GeometryModuleDataTableQuery.TableLabel GetFallbackGeometryModuleType() => UnitTool.__DefaultGeometryModuleType;
        protected override MindModuleDataTableQuery.TableLabel GetFallbackMindModuleType() => UnitTool.__DefaultMindModuleType;
        protected override RenderModuleDataTableQuery.TableLabel GetFallbackRenderModuleType() => UnitTool.__DefaultRenderModuleType;
        protected override RoleModuleDataTableQuery.TableLabel GetFallbackRoleModuleType() => UnitTool.__DefaultRoleModuleType;
    }
}