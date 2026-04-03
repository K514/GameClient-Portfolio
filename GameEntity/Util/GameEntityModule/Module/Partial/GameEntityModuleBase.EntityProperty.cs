using UnityEngine;

namespace k514.Mono.Common
{
    public partial class GameEntityModuleBase
    {
        #region <Fields>

        /// <summary>
        /// 해당 모듈을 소유하는 주인 개체 접근 프로퍼티
        /// </summary>
        public IGameEntityBridge Entity { get; }

        /// <summary>
        /// 해당 모듈을 소유하는 주인 개체 아핀 접근 프로퍼티
        /// </summary>
        public Transform Affine { get; }
        
        /// <summary>
        /// 주인 개체가 가지고 있는 액션 모듈 접근 프로퍼티
        /// </summary>
        public IActionModule ActionModule => Entity.ActionModule;
                
        /// <summary>
        /// 주인 개체가 가지고 있는 애니메이션 모듈 접근 프로퍼티
        /// </summary>
        public IAnimationModule AnimationModule => Entity.AnimationModule;
                
        /// <summary>
        /// 주인 개체가 가지고 있는 지리 모듈 접근 프로퍼티
        /// </summary>
        public IGeometryModule GeometryModule => Entity.GeometryModule;
                
        /// <summary>
        /// 주인 개체가 가지고 있는 사고 모듈 접근 프로퍼티
        /// </summary>
        public IMindModule MindModule => Entity.MindModule;
                       
        /// <summary>
        /// 주인 개체가 가지고 있는 물리 모듈 접근 프로퍼티
        /// </summary>
        public IPhysicsModule PhysicsModule => Entity.PhysicsModule;

#if !SERVER_DRIVE
        /// <summary>
        /// 주인 개체가 가지고 있는 랜더 모듈 접근 프로퍼티
        /// </summary>
        public IRenderModule RenderModule => Entity.RenderModule;
#endif
                       
        /// <summary>
        /// 주인 개체가 가지고 있는 역할 모듈 접근 프로퍼티
        /// </summary>
        public IRoleModule RoleModule => Entity.RoleModule;

        #endregion
    }
}