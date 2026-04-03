using System;
using k514.Mono.Common;

namespace k514.Mono.Feature
{
    public static class UnitTool
    {
        #region <Consts>

        public const AnimationModuleDataTableQuery.TableLabel __DefaultAnimationModuleType = AnimationModuleDataTableQuery.TableLabel.AnimatorAnimation;
        public const ActionModuleDataTableQuery.TableLabel __DefaultActionModuleType = ActionModuleDataTableQuery.TableLabel.Default;
        public const PhysicsModuleDataTableQuery.TableLabel __DefaultPhysicsModuleType = PhysicsModuleDataTableQuery.TableLabel.Affine;
        public const GeometryModuleDataTableQuery.TableLabel __DefaultGeometryModuleType = GeometryModuleDataTableQuery.TableLabel.AStar;
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
        
        public enum ClassType
        {
            None,
            
            Assassin,
            Knight,
            Archer,
            Mage,
        }
        
        public enum AttachPoint
        {
            /// <summary>
            /// 스크립트 컴포넌트가 붙어있는 래퍼
            /// </summary>
            MainTransform,
            
            /// <summary>
            /// 애니메이션의 주체가 되는 래퍼
            /// </summary>
            AnimationRootNode,

            /// <summary>
            /// 애니메이션 모델의 중심 본
            /// </summary>
            BoneCenterNode,
            
            /// <summary>
            /// 왼손 무기 모델을 표시하는 래퍼
            /// </summary>
            LeftArm,
                        
            /// <summary>
            /// 오른손 무기 모델을 표시하는 래퍼
            /// </summary>
            RightArm,
        }
        
        [Flags]
        public enum UnitPhaseTerminateConditionType
        {
            None = 0,
            
            Duration,
            HitCount,
            HpRate,
        }
        
        [Flags]
        public enum UnitPhaseAttribute
        {
            None = 0,
            
            HasDuration = 1 << 0,
            HasHitCount = 1 << 1,
            HasHpRate = 1 << 2,
            
            HasPhaseSuccessEvent = 1 << 7,
            HasPhaseFailEvent = 1 << 8,
        }

        public enum UnitPhaseEventType
        {
            PhaseStart,
            PhaseSuccess,
            PhaseFail,
        }
        
        #endregion
    }
}