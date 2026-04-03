using UnityEngine;
using UnityEngine.AI;

namespace k514.Mono.Common
{
    public partial class NavMeshGeometry
    {
        #region <Consts>

        /// <summary>
        /// 경로 계산 제한시간 상한
        /// </summary>
        private const float PendingBreakInterval = 10f;
           
        /// <summary>
        /// 길찾기 중 유닛이 Stuck 상태가 되었는지 판단하는 시간
        /// </summary>
        private const float StuckCheckInterval = 5f;
                    
        /// <summary>
        /// 길찾기가 더이상 불가능하다고 판단하는 시간
        /// </summary>
        private const float InvalidPathResetInterval = 3f;

        /// <summary>
        /// 경로 계산이 지체되는 경우, 관련 처리를 시작할 상한 시간 비율, PendingBreakInterval의 진행도 비율이다.
        /// </summary>
        private const float PendingDelayHandleUpperBoundProgressRate = 0.01f;

        #endregion

        #region <Fields>
        
        /// <summary>
        /// 현재 네브메쉬 에이전트의 목적지
        /// </summary>
        private Vector3 _NavMeshDestination;

        /// <summary>
        /// 현재 길찾기 에이전트 작업 페이즈
        /// </summary>
        private NavMeshAgentTaskPhase _NavMeshAgentTaskPhase;

        /// <summary>
        /// 현재 길찾기 에이전트가 목적지를 가지는지 표시하는 플래그
        /// </summary>
        private bool _IsNavMeshAgentDestinationValid => _NavMeshAgentTaskPhase != NavMeshAgentTaskPhase.HasNoDestination;
   
        /// <summary>
        /// 경로 계산 체크하는 타이머
        /// </summary>
        private ProgressTimer _PendingDeadlineCounter;

        /// <summary>
        /// 유닛 이동이 막혔는지 체크하는 타이머
        /// </summary>
        private ProgressTimer _StuckCheckCounter;
   
        /// <summary>
        /// 유효하지 않은 패스 이동 타이머
        /// </summary>
        private ProgressTimer _InvalidPathCounter;
           
        /// <summary>
        /// 유닛 이동이 막혔는지 체크하는 기준좌표
        /// </summary>
        private Vector3 _StuckCheckPivot;

        /// <summary>
        /// 이전 길찾기 패스가 불완전했거나, 오류가 있었던 경우 문제가 있는 해당 지점을 남겨놓기 위해
        /// 길찾기 목적지 위치 초기화를 막는 플래그
        /// </summary>
        private bool _BlockClearDestinationFlag;

        #endregion

        #region <Enums>

        public enum NavMeshAgentTaskPhase
        {
            HasNoDestination,
            Pending,
            PendingDelay,
            PendingOver,
            WaitForMove,
            BeforeRunPath,
            RunPath,
            Stuck,
        }

        #endregion

        #region <Callbacks>

        private void OnAwakeNavMeshAgentStateMachine()
        {
            _PendingDeadlineCounter = PendingBreakInterval;
            _StuckCheckCounter = StuckCheckInterval;
            _InvalidPathCounter = InvalidPathResetInterval;
        }

        public override void OnModule_PreUpdate(float p_DeltaTime)
        {
            // 네브메쉬에이전트 상태를 체크해준다.
            switch (_NavMeshAgentState)
            {
                case GeometryTool.NavMeshAgentState.Spawned:
                    break;
                case GeometryTool.NavMeshAgentState.Pooled:
                    break;
                case GeometryTool.NavMeshAgentState.Enable:
                {
                    switch (_NavMeshAgentTaskPhase)
                    {
                        case NavMeshAgentTaskPhase.HasNoDestination:
                            break;
                        case NavMeshAgentTaskPhase.Pending:
                        {
                            if (_NavMeshAgent.pathPending)
                            {
                                // 일정시간이 지나도 경로 계산이 종료되지 않는다면, 경로 실패 이벤트를 호출한다.
                                if (_PendingDeadlineCounter.IsOver())
                                {
                                    InitializePathPendingPreset(true);

                                    var targetMind = Entity.MindModule;
                                    // targetMind.OnAutonomyPhysicsPendingDeadline();
                                }
                                else
                                {
                                    _PendingDeadlineCounter.Progress(p_DeltaTime);

                                    // 경로계산이 늦어질 거 같다면, 지연 이벤트를 호출한다.
                                    if (_PendingDeadlineCounter.ProgressRate > PendingDelayHandleUpperBoundProgressRate)
                                    {
                                        _NavMeshAgentTaskPhase = NavMeshAgentTaskPhase.PendingDelay;
                                        var targetMind = Entity.MindModule;
                                        // targetMind.OnAutonomyPhysicsPendingDelay();
                                    }
                                }
                            }
                            else
                            {
                                _NavMeshAgentTaskPhase = NavMeshAgentTaskPhase.PendingOver;
                            }
                            break;
                        }
                        case NavMeshAgentTaskPhase.PendingDelay:
                        {
                            if (_NavMeshAgent.pathPending)
                            {
                                // 일정시간이 지나도 경로 계산이 종료되지 않는다면, 경로 실패 이벤트를 호출한다.
                                if (_PendingDeadlineCounter.IsOver())
                                {
                                    InitializePathPendingPreset(true);

                                    var targetMind = Entity.MindModule;
                                    // targetMind.OnAutonomyPhysicsPendingDeadline();
                                }
                                else
                                {
                                    _PendingDeadlineCounter.Progress(p_DeltaTime);
                                }
                            }
                            else
                            {
                                _NavMeshAgentTaskPhase = NavMeshAgentTaskPhase.PendingOver;
                            }
                            break;
                        }
                        case NavMeshAgentTaskPhase.PendingOver:
                        {
                            switch (_NavMeshAgent.pathStatus)
                            {
                                case NavMeshPathStatus.PathComplete:
                                    _BlockClearDestinationFlag = false;
                                    break;
                                case NavMeshPathStatus.PathPartial:
                                case NavMeshPathStatus.PathInvalid:
                                    _BlockClearDestinationFlag = true;
                                    break;
                            }
                            
                            var targetMind = Entity.MindModule;
                            SetPhysicsAutonomySpeed(1f);
                            InitializePathStuckPreset();
                            _InvalidPathCounter.Reset();
                            _NavMeshAgentTaskPhase = NavMeshAgentTaskPhase.WaitForMove;
                            break;
                        }
                        case NavMeshAgentTaskPhase.WaitForMove:
                        {
                            if (_StuckCheckPivot.GetSqrDistanceTo(Affine.position) > 0f)
                            {                            
                                _NavMeshAgentTaskPhase = NavMeshAgentTaskPhase.BeforeRunPath;
                            }
                            else
                            {
                                TryCheckInvalidPathReset(p_DeltaTime);
                            }
                            break;
                        }
                        case NavMeshAgentTaskPhase.BeforeRunPath:
                        {
                            // 해당 값은 현재 위치로부터 목적지 위치까지의 거리와 같음.
                            var remainingDistance = _NavMeshAgent.remainingDistance;
                            var stoppingDistance = _NavMeshAgent.stoppingDistance;
                            var targetMind = Entity.MindModule;
                            // targetMind.OnAutonomyPhysicsPendingOver(remainingDistance.IsReachedValue(stoppingDistance));
                            SetPhysicsAutonomySpeed(1f);
                            _InvalidPathCounter.Reset();
                            _NavMeshAgentTaskPhase = NavMeshAgentTaskPhase.RunPath;
                            break;
                        }
                        case NavMeshAgentTaskPhase.RunPath:
                        {
                            // 해당 값은 현재 위치로부터 목적지 위치까지의 거리와 같음.
                            var remainingDistance = _NavMeshAgent.remainingDistance;
                            var stoppingDistance = _NavMeshAgent.stoppingDistance;

                            // 경로 계산 결과에 따라,
                            switch (_NavMeshAgent.pathStatus)
                            {
                                // 완전한 경로
                                case NavMeshPathStatus.PathComplete:
                                    /*// 경로 진행 중에는 유닛이 이동했으므로 플래그를 세운다.
                                    _Entity.ReserveUpdatePosition();
                                    
                                    // 사고 모듈로에 해당 이동을 종료해도 되는지 여부를 요청한다.
                                    switch (_Entity.MindModule.OnUpdateAutonomyPhysics(remainingDistance, stoppingDistance))
                                    {
                                        case GeometryTool.UpdateAutonomyPhysicsResult.CheckNextMoveDestination:
                                            // 현재 이동이 종료된 경우, 예약된 다음 이동에 관한 처리를 해준다.
                                            TryCastNextDestination();
                                            break;
                                        case GeometryTool.UpdateAutonomyPhysicsResult.DoNothing:
                                            break;
                                        case GeometryTool.UpdateAutonomyPhysicsResult.ProgressNavMeshControl:
                                            // 경로 이동 중에 해당 물리 모듈이 어딘가에 끼었는지 체크를 해준다.
                                            TryCheckAutonomyStuck(p_DeltaTime);
                                            break;
                                    }*/
                                    break;
                                // 불완전 경로
                                case NavMeshPathStatus.PathPartial:
                                    /*// 경로 진행 중에는 유닛이 이동했으므로 플래그를 세운다.
                                    _Entity.ReserveUpdatePosition();

                                    // 사고 모듈로에 해당 이동을 종료해도 되는지 여부를 요청한다.
                                    switch (_Entity.MindModule.OnUpdateAutonomyPhysics(remainingDistance, stoppingDistance))
                                    {
                                        case GeometryTool.UpdateAutonomyPhysicsResult.CheckNextMoveDestination:
                                            TryCastNextDestination();
                                            break;
                                        case GeometryTool.UpdateAutonomyPhysicsResult.DoNothing:
                                            break;
                                        case GeometryTool.UpdateAutonomyPhysicsResult.ProgressNavMeshControl:
                                            // 경로 이동 중에 해당 물리 모듈이 어딘가에 끼었는지 체크를 해준다.
                                            TryCheckAutonomyStuck(p_DeltaTime);
                                            break;
                                    }*/
                                    break;
                                // 베이킹된 맵에 문제가 있거나 하는 경우
                                case NavMeshPathStatus.PathInvalid:
                                    TryCheckInvalidPathReset(p_DeltaTime);
                                    break;
                            }
                            break;
                        }
                        case NavMeshAgentTaskPhase.Stuck:
                        {
                            TryCheckAutonomyStuckEscape(p_DeltaTime);
                            break;
                        }
                    }
                    break;
                }
                case GeometryTool.NavMeshAgentState.Disable:
                    break;
            }
               
            base.OnModule_PreUpdate(p_DeltaTime);
        }

        #endregion

        #region <Methods>
        
        /// <summary>
        /// 경로 계산 지연시 사용할 변수를 초기화시키는 메서드
        /// </summary>
        private void InitializePathPendingPreset(bool p_ResetFirstCount)
        {
            _PendingDeadlineCounter.Reset();
        }

        /// <summary>
        /// 길찾기 끼임 체크 카운터를 초기화시키는 메서드
        /// </summary>
        private void InitializePathStuckPreset()
        {
            _StuckCheckCounter.Reset();
            _StuckCheckPivot = Affine.position;
        }
           
        /// <summary>
        /// 이전 위치와 현재 위치 간의 위상차를 비교하여, 해당 유닛이 이동중 상태인데
        /// 한곳에 머물고 있는지 검증하는 타이머 함수
        /// </summary>
        private void TryCheckAutonomyStuck(float p_DeltaTime)
        {
            if (_StuckCheckCounter.IsOver())
            {
                InitializePathStuckPreset();
                _NavMeshAgentTaskPhase = NavMeshAgentTaskPhase.Stuck;
                
                var targetMind = Entity.MindModule;
                // targetMind.OnAutonomyPhysicsStuck();
            }
            else
            {
                /*if (_StuckCheckPivot.GetSqrDistanceTo(_Transform.position) < _Entity.Radius.CurrentSqrValue)
                {
                    _StuckCheckCounter.Progress(p_DeltaTime);
                }
                else
                {
                    // 움직인 경우 카운터를 초기화시켜준다.
                    InitializePathStuckPreset();
                    
                    // 경로 진행 중에는 유닛이 이동했으므로 플래그를 세운다.
                    _Entity.ReserveUpdatePosition();
                }*/
            }
        }
        
        /// <summary>
        /// 이전 위치와 현재 위치 간의 위상차를 비교하여 해당 유닛이 Stuck 상태를 벗어났는지 체크하는 메서드
        /// </summary>
        private void TryCheckAutonomyStuckEscape(float p_DeltaTime)
        {
            /*if (_StuckCheckPivot.GetSqrDistanceTo(_Transform.position) > 0f)
            {
                // 움직인 경우 카운터를 초기화시켜준다.
                InitializePathStuckPreset();
                    
                // 경로 진행 중에는 유닛이 이동했으므로 플래그를 세운다.
                _Entity.ReserveUpdatePosition();

                _NavMeshAgentTaskPhase = NavMeshAgentTaskPhase.BeforeRunPath;
            }
            else
            {
                TryCheckInvalidPathReset(p_DeltaTime);
            }*/
        }
           
        /// <summary>
        /// 길찾기 결과가 유효하지 않은 경로 였던 경우, 즉시 길찾기를 재개하는 것 보다는
        /// 일정 간격을 두고 하는게 과부하를 막기 때문에
        /// 경로 재계산 까지의 간격을 재는 타이머 함수
        /// </summary>
        private void TryCheckInvalidPathReset(float p_DeltaTime)
        {
            if (_InvalidPathCounter.IsOver())
            {
                _InvalidPathCounter.Reset();
                
                var targetMind = Entity.MindModule;
                // targetMind.OnAutonomyPhysicsPendingDeadline();
            }
            else
            {
                _InvalidPathCounter.Progress(p_DeltaTime);
            }
        }

        #endregion
    }
}