using UnityEngine;

namespace k514.Mono.Common
{
    /// <summary>
    /// 게임 개체에 포함되는 모듈을 기술하는 인터페이스
    /// </summary>
    public interface IGameEntityModule : IGameEntityModuleEventReceiver, _IDisposable
    {
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
        public IActionModule ActionModule { get; }
                
        /// <summary>
        /// 주인 개체가 가지고 있는 애니메이션 모듈 접근 프로퍼티
        /// </summary>
        public IAnimationModule AnimationModule { get; }
                
        /// <summary>
        /// 주인 개체가 가지고 있는 지리 모듈 접근 프로퍼티
        /// </summary>
        public IGeometryModule GeometryModule { get; }
                
        /// <summary>
        /// 주인 개체가 가지고 있는 사고 모듈 접근 프로퍼티
        /// </summary>
        public IMindModule MindModule { get; }
                       
        /// <summary>
        /// 주인 개체가 가지고 있는 물리 모듈 접근 프로퍼티
        /// </summary>
        public IPhysicsModule PhysicsModule { get; }

#if !SERVER_DRIVE
        /// <summary>
        /// 주인 개체가 가지고 있는 랜더 모듈 접근 프로퍼티
        /// </summary>
        public IRenderModule RenderModule { get; }
#endif
                       
        /// <summary>
        /// 주인 개체가 가지고 있는 역할 모듈 접근 프로퍼티
        /// </summary>
        public IRoleModule RoleModule { get; }
        
        /// <summary>
        /// 해당 모듈의 타입을 리턴하는 메서드
        /// </summary>
        GameEntityModuleTool.ModuleType GetModuleType();
        
        /// <summary>
        /// 해당 모듈을 구성하는 레코드의 키값을 리턴하는 메서드
        /// </summary>
        int GetRecordKey();

        /// <summary>
        /// 해당 모듈을 구성하는 레코드를 리턴하는 메서드
        /// </summary>
        IGameEntityModuleDataTableRecordBridge GetRecord();
        
        /// <summary>
        /// 해당 모듈을 활성화 시키는 메서드
        /// </summary>
        void AwakeModule();
        
        /// <summary>
        /// 해당 모듈을 비활성화 시키는 메서드
        /// </summary>
        void SleepModule();
        
        /// <summary>
        /// 해당 모듈을 초기화 시키는 메서드
        /// </summary>
        void ResetModule();
    }
}