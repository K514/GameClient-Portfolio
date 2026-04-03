using System.Threading;
using Cysharp.Threading.Tasks;
using k514.Mono.Feature;
using UnityEngine;
using UnityEngine.AI;

namespace k514.Mono.Common
{
    public partial class NavMeshGeometry : GeometryBase
    {
        #region <Consts>
   
        /// <summary>
        /// 네브메쉬 에이전트 감속도. 감속도가 충분히 크지 않다면, 정지 기능에도 곧바로 멈추지 않고
        /// 네브메쉬 표면 위를 미끄러지게 된다.
        /// </summary>
        private const float Deacceleration = 1_000_000f;
   
        /// <summary>
        /// 네브메쉬 에이전트 초기 속도
        /// </summary>
        private const float DefaultNavMeshAgentDefaultSpeed = 1f;
           
        /// <summary>
        /// 네브메쉬 에이전트의 회전속도
        /// </summary>
        private const float AngularVel = 540f;

        public static (bool, GeometryModuleDataTableQuery.TableLabel, NavMeshGeometry) CreateModule(IGeometryModuleDataTableRecordBridge p_ModuleRecord, IGameEntityBridge p_Entity)
        {
            return GeometryBase.CreateModule(new NavMeshGeometry(p_ModuleRecord, p_Entity));
        }
        
        public static async UniTask<(bool, GeometryModuleDataTableQuery.TableLabel, NavMeshGeometry)> CreateModule(IGeometryModuleDataTableRecordBridge p_ModuleRecord, IGameEntityBridge p_Entity, CancellationToken p_CancellationToken)
        {
            return await GeometryBase.CreateModule(new NavMeshGeometry(p_ModuleRecord, p_Entity), p_CancellationToken);
        }

        #endregion
        
        #region <Fields>

        /// <summary>
        /// 네브메쉬 에이전트 고유 속도
        /// </summary>
        private float _NavMeshAgentDefaultSpeed;
   
        /// <summary>
        /// 네브 메쉬 에이전트의 활성화를 막는 키
        /// </summary>
        private bool _NavMeshAgentStateBlockFlag;
           
        /// <summary>
        /// 네브메쉬 에이전트 = 길찾기 에이전트
        /// </summary>
        public NavMeshAgent _NavMeshAgent { get; private set; }
           
        /// <summary>
        /// 길찾기 장해물
        /// 해당 물리모듈의 길찾기 에이전트가 비활성화된 상태에서 활성화되어
        /// 다른 길찾기 모듈의 경로 방해를 수행하여, 겹치는 일이 없게 한다.
        /// </summary>
        public NavMeshObstacle _NavMeshObstacle { get; private set; }

        /// <summary>
        /// 물리 에이전트의 상태를 표시하는 플래그
        /// </summary>
        protected GeometryTool.NavMeshAgentState _NavMeshAgentState;
   
        /// <summary>
        /// 네브메쉬 에이전트의 유효성을 표시하는 플래그
        /// </summary>
        public bool IsNavMeshAgentEnable => _NavMeshAgentState == GeometryTool.NavMeshAgentState.Enable;

        #endregion

        #region <Constructor>

        private NavMeshGeometry(IGeometryModuleDataTableRecordBridge p_ModuleRecord, IGameEntityBridge p_Entity) : base(GeometryModuleDataTableQuery.TableLabel.NavMesh, p_ModuleRecord, p_Entity)
        {
            /*_GeometryRecord = (NavMeshGeometryModuleDataTable.TableRecord) p_ModuleRecord;
            
            _NavMeshAgent = p_Entity.GetComponent<NavMeshAgent>();
            if (null == _NavMeshAgent)
            {
                _NavMeshAgent = _Entity.gameObject.AddNavMeshAgentSafe();
                _NavMeshAgent.speed = _NavMeshAgentDefaultSpeed = DefaultNavMeshAgentDefaultSpeed;
                _NavMeshAgent.acceleration = Deacceleration;
                _NavMeshAgent.angularSpeed = AngularVel;
                _NavMeshAgent.avoidancePriority = _GeometryRecord.NavMeshAgentPriority;
                _NavMeshAgent.autoBraking = true;
                _NavMeshAgent.updatePosition = false;
            }
            
            var modelRecord = _Entity.ModelDataRecord;
            if (!ReferenceEquals(null, modelRecord))
            {
                var radius = modelRecord.PrefabColliderRadius;
                var height = modelRecord.PrefabColliderHeight;
                var center = modelRecord.PrefabColliderCenterOffset;
                
                _NavMeshAgent.radius = radius;
                _NavMeshAgent.height = height;
                
                _NavMeshObstacle = p_Entity.gameObject.AddComponent<NavMeshObstacle>();
                _NavMeshObstacle.shape = NavMeshObstacleShape.Capsule;
                _NavMeshObstacle.radius = radius;
                _NavMeshObstacle.height = height;
                _NavMeshObstacle.center = center;
            }
            
            OnAwakePath();
            OnAwakeOffMeshLink();
            OnAwakeNavMeshAgentStateMachine();

            SetNavMeshAgentState(GeometryTool.NavMeshAgentState.Spawned);*/
        }

        #endregion
        
        #region <Callbacks>

        protected override void _OnAwakeModule()
        {
            base._OnAwakeModule();
               
            SetNavMeshAgentEnableBlockFlag(false);
            SetNavMeshAgentState(GeometryTool.NavMeshAgentState.Pooled);
            OnPoolingOffMeshLink();
            ClearPhysicsAutonomySpeed();
        }
   
        protected override void _OnSleepModule()
        {
            base._OnSleepModule();
   
            _BlockClearDestinationFlag = false;
            SetBreakingDistance(0f);
            ClearPhysicsAutonomyMove();
        }

        protected override void _OnResetModule()
        {
        }

        /// <summary>
        /// 유닛 pivot 이 변경되는 경우 호출되는 콜백
        /// </summary>
        public override void OnModule_PivotChanged(PositionTracer p_PositionStatePreset)
        {
            base.OnModule_PivotChanged(p_PositionStatePreset);

            // 경로를 초기화 시켜준다.
            ResetNavigatePath();
        }

        /// <summary>
        /// 피격 액션이 시작되는 경우 호출되는 콜백
        /// </summary>
        public override void OnModule_HitMotion_Start()
        {
            base.OnModule_HitMotion_Start();
               
            ClearPhysicsAutonomyMove();
        }

        /// <summary>
        /// 유닛 액션이 시작되는 경우 호출되는 콜백
        /// </summary>
        public override void OnModule_HitMotion_Over()
        {
            base.OnModule_HitMotion_Over();
               
            ClearPhysicsAutonomyMove();
        }

        /// <summary>
        /// 유닛 사망시 호출되는 콜백
        /// </summary>
        public override void OnModule_Dead(bool p_Instant)
        {
            base.OnModule_Dead(p_Instant);
            
            ClearPhysicsAutonomyMove();
            _NavMeshObstacle.enabled = false;
        }
        
        #endregion
           
        #region <Methods>

        protected override GeometryTool.NavigationStatePreset GetNavigationState()
        {
            return default;
        }
        
        /// <summary>
        /// 현재 물리 모듈이 길찾기를 포함하는지 여부를 리턴하도록 구현
        /// </summary>
        public bool IsAutonomyValid(bool p_CheckEnable)
        {
            return !p_CheckEnable || IsNavMeshAgentEnable;
        }
        
        /// <summary>
        /// 네브메쉬 에이전트 활성화를 블록하는 키를 세트하는 메서드
        /// </summary>
        public void SetNavMeshAgentEnableBlockFlag(bool p_Flag)
        {
            _NavMeshAgentStateBlockFlag = p_Flag;
        }
   
        /// <summary>
        /// 현재 네브메쉬 에이전트를 활성화 시킬 수 있는지 검증하는 논리메서드
        /// </summary>
        private bool IsTransitionableToEnableNavMeshAgent()
        {
            return Entity.PhysicsModule.IsGrounded() && !_NavMeshAgentStateBlockFlag;
        }
   
        /// <summary>
        /// 네브메쉬 에이전트의 활성화 상태를 세트하는 메서드
        /// </summary>
        public void SetNavMeshAgentState(GeometryTool.NavMeshAgentState p_Flag, bool p_NavMeshAgentBlockKey)
        {
            SetNavMeshAgentEnableBlockFlag(p_NavMeshAgentBlockKey);
            SetNavMeshAgentState(p_Flag);
        }
           
        /// <summary>
        /// 네브메쉬 에이전트의 활성화 상태를 세트하는 메서드
        /// </summary>
        public void SetNavMeshAgentState(GeometryTool.NavMeshAgentState p_Flag)
        {
            if (_NavMeshAgentState != p_Flag)
            {
                switch (_NavMeshAgentState)
                {
                    case GeometryTool.NavMeshAgentState.Spawned:
                        switch (p_Flag)
                        {
                            // Pooled 외의 전이는 허용하지 않는다.
                            case GeometryTool.NavMeshAgentState.Pooled:
                                break;
                            default:
                                return;
                        }
                        break;
                    case GeometryTool.NavMeshAgentState.Pooled:
                        switch (p_Flag)
                        {
                            // Spawned 로의 전이는 허용하지 않는다.
                            case GeometryTool.NavMeshAgentState.Spawned:
                                return;
                            case GeometryTool.NavMeshAgentState.Enable:
                                if (!IsTransitionableToEnableNavMeshAgent())
                                {
                                    p_Flag = GeometryTool.NavMeshAgentState.Disable;
                                }
                                break;
                        }
                        break;
                    case GeometryTool.NavMeshAgentState.Enable:
                        switch (p_Flag)
                        {
                            // Spawned 로의 전이는 허용하지 않는다.
                            case GeometryTool.NavMeshAgentState.Spawned:
                                return;
                        }
                        break;
                    case GeometryTool.NavMeshAgentState.Disable:
                        switch (p_Flag)
                        {
                            // Spawned 로의 전이는 허용하지 않는다.
                            case GeometryTool.NavMeshAgentState.Spawned:
                                return;
                            case GeometryTool.NavMeshAgentState.Enable:
                                if (!IsTransitionableToEnableNavMeshAgent())
                                {
                                    return;
                                }
                                break;
                        }
                        break;
                }

                _NavMeshAgentState = p_Flag;
                   
                switch (_NavMeshAgentState)
                {
                    // 에이전트가 활성화 된 경우,
                    case GeometryTool.NavMeshAgentState.Enable:
                        _NavMeshObstacle.enabled = false;
                        _NavMeshAgent.enabled = true;
                        break;
                    // 에이전트가 비활성화된 경우, baseOffset만큼 현재 위치를 보정시켜준다.
                    case GeometryTool.NavMeshAgentState.Spawned:
                    case GeometryTool.NavMeshAgentState.Pooled:
                    case GeometryTool.NavMeshAgentState.Disable:
                        _NavMeshAgent.enabled = false;
                        _NavMeshObstacle.enabled = true;
                        ClearNavMeshBaseLocalYOffset();
                        break;
                }
            }
        }
   
        /// <summary>
        /// 네브메쉬 에이전트의 baseOffset을 제외한 현재 네브메쉬 에이전트의 좌표(표면 위의)를 리턴하는 메서드
        /// </summary>
        public Vector3 GetNavMeshAgentPosition()
        {
            return Affine.position + Vector3.down * _NavMeshAgent.baseOffset;
        }
           
        /// <summary>
        /// 네브메쉬 에이전트의 baseOffset을 0으로하고, 그값만큼 높이를 보정하는 메서드
        /// </summary>
        public void ClearNavMeshBaseLocalYOffset()
        {
            var baseOffset = _NavMeshAgent.baseOffset;
            if (baseOffset > 0f)
            {
                Affine.position += Vector3.up * baseOffset;
                _NavMeshAgent.baseOffset = 0f;
            }
        }
           
        /// <summary>
        /// 네브메쉬 에이전트의 baseOffset으로 세트하는 메서드
        /// 지정할 수 있는 실수구간은 ( 절대값 _VelocityYLowerVectorFactor, +무한 )
        /// </summary>
        public void SetNavMeshBaseLocalYOffset(float p_Offset)
        {
            _NavMeshAgent.baseOffset = p_Offset;
        }
   
        /// <summary>
        /// 지정한 값 만큼 네브메쉬 에이전트의 baseOffset을 더하는 메서드
        /// 이 때, baseoffset은 0이하가 되지 않으며 0이하가 되도록 값을 입력했다면
        /// 적용되지 않은 나머지 음수 값을 리턴한다.
        /// </summary>
        public float AddNavMeshBaseLocalYOffset(float p_Offset)
        {
            var targetOffset = _NavMeshAgent.baseOffset + p_Offset;
            SetNavMeshBaseLocalYOffset(targetOffset);
            return targetOffset;
        }

        /// <summary>
        /// 기존의 캐릭터 컨트롤러의 이동과는 다르게, 네브메쉬 에이전트는 네브메쉬 표면으로부터
        /// y축으로 멀어지는 경우, 네브메쉬 엔진에 의해 곧바로 표면으로 돌아오도록 좌표를 수정당하기 때문에
        /// 같은 방법으로는 체공 연출을 구현할 수 없다.
        ///
        /// 네브메쉬 에이전트는 y축 위상에 대응하는 baseOffset이라는 멤버를 제공하는데, 해당 값을
        /// 제어하는 것으로 에이전트 자체는 네브메쉬 표면에 고정된 상태로
        /// 나머지 컬라이더를 포함한 게임오브젝트는 y축 이동이 가능하다.
        ///
        /// 아래 메서드는 기존의 캐릭터 컨트롤러 이동 메서드를 오버라이드하여
        /// y축에 대해서만 baseOffset을 변형시키도록 구현되어 있다.
        /// </summary>       
        public void NavigateTo(Vector3 p_DeltaVelocity, float p_DeltaTime)
        {
            // 네브메쉬 에이전트가 활성화되어 있는 경우
            if (IsNavMeshAgentEnable)
            {
                return;
                // offlink load 중에는 피격 당한 경우를 제외하고 외력의 영향을 받지 않는다.
//                if (_InOnNavMesh_Off_MeshLink && !_MasterNode.HasState_Or(Unit.UnitStateType.STUCK))
//                {
//                    p_DeltaVelocity = Vector3.zero;
//                }
//   
//                // 속도벡터에서 Y값은 baseOffset으로 처리하므로 제거해준다.
//                var deltaVelocityY = p_DeltaVelocity.y;
//                p_DeltaVelocity = p_DeltaVelocity.XZVector();
//                       
//                /* 네브메쉬 에이전트 길찾기 방향을 기준으로 */
//                // +y축으로 속도가 작용중인 경우
//                if (deltaVelocityY > 0f)
//                {
//                    p_DeltaVelocity.y += PhysicsTool._VelocityYLowerVectorFactor_Negative;
//                    deltaVelocityY += PhysicsTool._VelocityYLowerVectorFactor;
//                }
//                // -y축으로 속도가 작용중인 경우
//                else if (deltaVelocityY < 0f)
//                {
//                    p_DeltaVelocity.y += PhysicsTool._VelocityYLowerVectorFactor;
//                    deltaVelocityY += PhysicsTool._VelocityYLowerVectorFactor_Negative;
//                }
//                   
//                // base offset 값을 보정하고 속도를 적용시킨다.
//                p_DeltaVelocity.y += AddNavMeshBaseLocalYOffset(deltaVelocityY);
//                _CharacterController.Move(p_DeltaVelocity);
//                _MasterNode.OnUnitPositionChangeDetected();
            }
        }

        #endregion
    }
}