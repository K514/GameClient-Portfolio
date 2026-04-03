using k514.Mono.Feature;

namespace k514.Mono.Common
{
    /// <summary>
    /// 모듈 계열 오브젝트를 제어하는 컨테이너 인터페이스
    /// </summary>
    public interface IGameEntityModuleContainer : IGameEntityModuleEventReceiver
    {
        /* Module : Common */
#if APPLY_PRINT_LOG
        void PrintModuleInfo();
#endif
        /* Module : Action */
        IActionModule ActionModule { get; }
        void SwitchDefaultActionModule();
        void SwitchNullActionModule();
        void SwitchActionModule(int p_Index);
        void SwitchActionModule(ActionModuleDataTableQuery.TableLabel p_ModuleType);
#if APPLY_PRINT_LOG
        void PrintActionInfo();
#endif
        /* Module : Animation */
        IAnimationModule AnimationModule { get; }
        void SwitchDefaultAnimationModule();
        void SwitchNullAnimationModule();
        void SwitchAnimationModule(int p_Index);
        void SwitchAnimationModule(AnimationModuleDataTableQuery.TableLabel p_ModuleType);

        /* Module : Geometry */
        IGeometryModule GeometryModule { get; }
        void SwitchDefaultGeometryModule();
        void SwitchNullGeometryModule();
        void SwitchGeometryModule(int p_Index);
        void SwitchGeometryModule(GeometryModuleDataTableQuery.TableLabel p_ModuleType);

        /* Module : Mind */
        IMindModule MindModule { get; }
        void SwitchDefaultPersona();
        void SwitchNullPersona();
        void SwitchPersona(int p_Index);
        void SwitchPersona(AutonomyModuleDataTableQuery.TableLabel p_ModuleType);
        void SwitchPersona(BoundedModuleDataTableQuery.TableLabel p_ModuleType);

        /* Module : Physics */
        IPhysicsModule PhysicsModule { get; }
        void SwitchDefaultPhysicsModule();
        void SwitchNullPhysicsModule();
        void SwitchPhysicsModule(int p_Index);
        void SwitchPhysicsModule(PhysicsModuleDataTableQuery.TableLabel p_ModuleType);
        float GetScaledMovementSpeed();
        
        /* Module : Render */
        IRenderModule RenderModule { get; }
        void SwitchDefaultRenderModule();
        void SwitchNullRenderModule();
        void SwitchRenderModule(int p_Index);
        void SwitchRenderModule(RenderModuleDataTableQuery.TableLabel p_ModuleType);

        /* Module : Role */
        IRoleModule RoleModule { get; }
        void SwitchDefaultRole();
        void SwitchNullRole();
        void SwitchRole(int p_Index);
        void SwitchRole(RoleModuleDataTableQuery.TableLabel p_ModuleType);
    }
}