using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace k514.Mono.Common
{
    public partial class NavMeshGeometry
    {
        #region <Fields>

        /// <summary>
        /// 예약된 이동 목적지 리스트
        /// </summary>
        private List<Vector3> _ReservedNavMeshDestinationList;

        /// <summary>
        /// 길찾기 에이전트가 비활성화 상태에서 활성화 상태가 되었을 때 좌표
        /// </summary>
        private Vector3 _EnableStatePosition;

        /// <summary>
        /// 길찾기 에이전트 활성화 시, 원본 아핀객체와 위상차가 생기는데 그 값을 기록하는 필드
        /// </summary>
        private Vector3 _EnableOffsetVector;
        
        /// <summary>
        /// 길찾기 에이전트가 활성화 상태에서 비활성화 상태가 되었을 때 좌표
        /// </summary>
        private Vector3 _DisableStatePosition;

        /// <summary>
        /// 길찾기 에이전트 비활성화 시, 원본 아핀객체와 위상차가 생기는데 그 값을 기록하는 필드
        /// </summary>
        private Vector3 _DisableOffsetVector;
        
        #endregion
        
        #region <Callbacks>

        private void OnAwakePath()
        {
            _ReservedNavMeshDestinationList = new List<Vector3>();
        }

        private void OnDestinationUpdated(Vector3 p_Destination, float p_StoppingDistance, bool p_StateTransitioned)
        {
            /*_NavMeshAgentTaskPhase = NavMeshAgentTaskPhase.Pending;
            
            SetNavMeshDestinationPivotPosition(p_Destination);
            SetBreakingDistance(p_StoppingDistance);
            _Entity.OnSelectDestination();
            _NavMeshAgent.updatePosition = true;
            
#if SERVER_DRIVE
            HeadlessServerManager.GetInstanceUnsafe.OnUnitAutonomyPathRequested(_Entity, _NavMeshDestination);
#endif
            if (p_StateTransitioned)
            {
                OnUpdateLatestPassivePosition();
            }*/
        }

        /// <summary>
        /// 길찾기 에이전트가 활성화된 경우에는
        /// 다음 프레임에 물리엔진이 보정되는 과정에서 아핀 좌표의 위상차가 생긴다.
        /// 해당 값을 캐싱한다. 
        /// </summary>
        private async UniTaskVoid OnUpdateLatestPassivePosition()
        {
            _EnableStatePosition = Affine.position;
            
            await UniTask.NextFrame();

            if (Entity.IsEntityValid())
            {
                _EnableOffsetVector = _EnableStatePosition - Affine.position;
            }
        }
        
        /// <summary>
        /// 길찾기 에이전트가 비활성화된 경우에는
        /// 다음 프레임에 물리엔진이 보정되는 과정에서 아핀 좌표의 위상차가 생긴다.
        /// 해당 값을 캐싱한다. 
        /// </summary>
        private async UniTaskVoid OnUpdateLatestAutonomyPosition()
        {
            _DisableStatePosition = Affine.position;
                
            await UniTask.NextFrame();
                
            if (Entity.IsEntityValid())
            {
                _DisableOffsetVector = _DisableStatePosition - Affine.position;
                NavigateTo(_DisableOffsetVector, 1f);
            }
        }

        #endregion
        
        #region <Method/AutoPath>

        /// <summary>
        /// 길찾기 에이전트에 등록되어 있는 경로 오브젝트를 리턴하는 메서드
        /// 경로 오브젝트는 길찾기 함수(SetDestination)로 NavMeshAgent.path 필드에 초기화되며
        /// 해당 값을 다른 길찾기 에이전트에 SetPath해주면 해당 경로를 그대로 이동한다.
        /// </summary>
        public NavMeshPath GetPath()
        {
            return _NavMeshAgent.path;
        }

        private bool IsSameDestination(Vector3 p_TryPosition)
        {
            return (p_TryPosition - _NavMeshDestination).sqrMagnitude.IsReachedZero(1f);
        }

        /// <summary>
        /// 길찾기 경로 오브젝트를 해당 물리 모듈의 길찾기 에이전트에 세트해주는 메서드
        /// </summary>
        public (bool, GeometryTool.NavMeshAgentDriveState) SetPath(NavMeshPath p_Path, GeometryTool.NavigateDestinationPreset p_AutonomyPathStoppingDistancePreset)
        {
            /*// 마스터 노드 유닛이 움직일 수 있는 경우에,
            if (_Entity.HasState_Only(GameEntityTool.StateType.DefaultMoveAvailableMask))
            {
                var corners = p_Path.corners;
                var cornerCount = corners.Length;
                if (cornerCount < 1)
                {
                    // 경로 객체 내부의 정점 갯수가 부족한 경우
                    return (false, GeometryTool.NavMeshAgentDriveState.InvalidPath);
                }
                else
                {
                    // 경로 세트를 위해 길찾기 에이전트를 활성화시킨다.
                    SetNavMeshAgentState(GeometryTool.NavMeshAgentState.Enable);
                    var (result, resultMessage) = (false, GeometryTool.NavMeshAgentDriveState.Unreachable);

                    // 에이전트가 활성화 되어 있는 경우
                    if (IsNavMeshAgentEnable)
                    {
                        // 이미 목적지가 세트되어 있는 경우
                        if (_IsNavMeshAgentDestinationValid)
                        {
                            // 경로에 정점이 단 하나이고, 해당 위치가 기존의 목적지와 같은 경우 경로를 세트하지 않는다.
                            if (cornerCount == 1 && IsSameDestination(corners[0]))
                            {
                                (result, resultMessage) = (true, GeometryTool.NavMeshAgentDriveState.SameDestination);
                            }
                            else
                            {
                                if (_NavMeshAgent.SetPath(p_Path))
                                {
                                    OnDestinationUpdated(corners.Last(), p_AutonomyPathStoppingDistancePreset.IsNavigateOver(_Entity), false);
                                    (result, resultMessage) = (true, GeometryTool.NavMeshAgentDriveState.Reachable);
                                }
                                else
                                {
                                    (result, resultMessage) = (false, GeometryTool.NavMeshAgentDriveState.Unreachable);
                                }
                            }
                        }
                        else
                        {
                            // 정지거리를 가져온다.
                            var tryStoppingDistance = p_AutonomyPathStoppingDistancePreset.IsNavigateOver(_Entity);
                            
                            // 경로에 정점이 단 하나이고, 해당 위치가 현재 위치와 같은 경우 경로를 세트하지 않는다.
                            if (cornerCount == 1 && (corners[0] - _Transform.position).sqrMagnitude <= Mathf.Pow(tryStoppingDistance, 2))
                            {
                                (result, resultMessage) = (false, GeometryTool.NavMeshAgentDriveState.SamePosition);
                            }
                            else
                            {
                                if (_NavMeshAgent.SetPath(p_Path))
                                {
                                    OnDestinationUpdated(corners.Last(), tryStoppingDistance, true);
                                    (result, resultMessage) = (true, GeometryTool.NavMeshAgentDriveState.Reachable);
                                }
                                else
                                {
                                    (result, resultMessage) = (false, GeometryTool.NavMeshAgentDriveState.Unreachable);
                                }
                            }
                        }
                    }
                    else
                    {
                        (result, resultMessage) = (false, GeometryTool.NavMeshAgentDriveState.NavMeshAgentInvalid);
                    }
                    
                    switch (resultMessage)
                    {
                        case GeometryTool.NavMeshAgentDriveState.CorrectSurfaceFailed:
                        case GeometryTool.NavMeshAgentDriveState.NavMeshAgentInvalid:
                        case GeometryTool.NavMeshAgentDriveState.Unreachable:
                        case GeometryTool.NavMeshAgentDriveState.SamePosition:
                            break;
                    }

                    return (result, resultMessage);
                }
            }
            else
            {
                SetNavMeshAgentState(GeometryTool.NavMeshAgentState.Disable);
                return (false, GeometryTool.NavMeshAgentDriveState.MasterNodeCantMove);
            }*/

            return default;
        }

        /// <summary>
        /// 지정한 좌표로의 경로를 계산하고, 길찾기 에이전트가 해당 경로를 따라 이동하도록 하는 메서드
        /// </summary>
        private (bool, GeometryTool.NavMeshAgentDriveState) SetDestination(Vector3 p_Destination, bool p_OverlapCurrentDestination, GeometryTool.NavigateDestinationPreset p_AutonomyPathStoppingDistancePreset)
        {
            // 좌표를 터레인 표면에 세트한다.
            // if (AffineTool.TryCorrectPositionToSurface(p_Destination, GameConst.Block_LayerMask, out var resultAffinePreset))
            {
                // var position = resultAffinePreset.Position;
                // 추가하려는 좌표가 기존 좌표를 덮어써야하는 경우
                if (p_OverlapCurrentDestination)
                {
                    // 기존 좌표가 있던 경우
                    if (_IsNavMeshAgentDestinationValid)
                    {
                        // 새로 경로를 갱신하려는 지점과 기존의 목적지가 같은 경우
                        /*if (IsSameDestination(position))
                        {
                            return (true, GeometryTool.NavMeshAgentDriveState.SameDestination);
                        }
                        else
                        {
                            if (_NavMeshAgent.SetDestination(position))
                            {
                                OnDestinationUpdated(position, p_AutonomyPathStoppingDistancePreset.IsNavigateOver(_Entity), false);
                                return (true, GeometryTool.NavMeshAgentDriveState.Reachable);
                            }
                            else
                            {
                                return (false, GeometryTool.NavMeshAgentDriveState.Unreachable);
                            }
                        }*/
                    }
                    else
                    {
                        goto SEG_CALC_NEW_PATH;
                    }
                }
                // 이동 큐에 좌표를 추가하는 경우
                else
                {
                    // 기존 좌표가 있던 경우
                    if (_IsNavMeshAgentDestinationValid)
                    {
                        // 이동 큐에 다른 좌표가 있던 경우
                        /*var reservedCount = _ReservedNavMeshDestinationList.Count;
                        if (reservedCount > 0)
                        {
                            var lastAddedDestination = _ReservedNavMeshDestinationList[reservedCount - 1];
                            // 새로 경로를 갱신하려는 지점과 마지막에 예약된 지점이 같은 경우
                            if ((position - lastAddedDestination).sqrMagnitude.IsReachedZero())
                            {
                                return (true, GeometryTool.NavMeshAgentDriveState.SameDestination);
                            }
                            else
                            {
                                _ReservedNavMeshDestinationList.Add(position);
                                return (true, GeometryTool.NavMeshAgentDriveState.Reserved);
                            }
                        }
                        else
                        {
                            // 새로 경로를 갱신하려는 지점과 기존의 목적지가 같은 경우
                            if (IsSameDestination(position))
                            {
                                return (true, GeometryTool.NavMeshAgentDriveState.SameDestination);
                            }
                            else
                            {
                                _ReservedNavMeshDestinationList.Add(position);
                                return (true, GeometryTool.NavMeshAgentDriveState.Reserved);
                            }
                        }*/
                    }
                    else
                    {
                        goto SEG_CALC_NEW_PATH;
                    }
                }
                
                // 현재 길찾기 에이전트가 이동중이지 않고, 지정한 위치로의 경로 계산 및 이동이 필요한 경우 도달하는 구역
                SEG_CALC_NEW_PATH:
                {
                    /*// 정지거리를 가져온다.
                    var tryStoppingDistance = p_AutonomyPathStoppingDistancePreset.IsNavigateOver();
                    if ((position - _Transform.position).sqrMagnitude <= Mathf.Pow(tryStoppingDistance, 2))
                    {
                        return (false, GeometryTool.NavMeshAgentDriveState.SamePosition);
                    }
                    else
                    {
                        if (_NavMeshAgent.SetDestination(position))
                        {
                            OnDestinationUpdated(position, tryStoppingDistance, true);
                            return (true, GeometryTool.NavMeshAgentDriveState.Reachable);
                        }
                        else
                        {
                            return (false, GeometryTool.NavMeshAgentDriveState.Unreachable);
                        }
                    }*/
                }
            }
            /*else
            {
                return (false, GeometryTool.NavMeshAgentDriveState.CorrectSurfaceFailed);
            }*/

            return default;
        }

        /// <summary>
        /// 지정한 좌표로의 경로를 계산하고, 길찾기 에이전트가 해당 경로를 따라 이동하도록 하는 메서드
        /// 해당 메서드는 현재 이동중인 경로가 있는 경우 그 경로를 덮어쓰는 지정한 목적지에 대한 경로를 새로 계산한다.
        /// </summary>
        public (bool, GeometryTool.NavMeshAgentDriveState) SetPhysicsAutonomyMove(Vector3 p_Position, GeometryTool.NavigateDestinationPreset p_AutonomyPathStoppingDistancePreset)
        {
            // 마스터 노드 유닛이 움직일 수 있는 경우에,
            if (Entity.HasState_Only(GameEntityTool.EntityStateType.NavigatePassMask))
            {
                SetNavMeshAgentState(GeometryTool.NavMeshAgentState.Enable);
                var (result, resultMessage) = (false, GeometryTool.NavMeshAgentDriveState.Unreachable);
                
                // 에이전트가 활성화 되어 있는 경우
                if (IsNavMeshAgentEnable)
                {
                    // 에이전트가 현재 offMeshLink를 지나는 경우
                    if (false && _NavMeshAgent.currentOffMeshLinkData.valid)
                    {
                        // 오프 메쉬 관련 로직은 추후 재구현
                        /*
                        // 해당 위치에 정확히 도달하기 위해, 정지거리를 0으로 한다.
                        SetBreakingDistance(0f);
                        On_Off_Mesh_LinkBegin();
                        resultState = GeometryTool.NavMeshAgentDriveState.JumpGateRecognition;
                        */
                    }
                    // 그 외의 경우
                    else
                    {
                        _OfflinkProcessPhase = OfflinkProcessPhase.ReadyToOfflink;
                        if (_NavMeshAgent.isOnNavMesh)
                        {
                            (result, resultMessage) = SetDestination(p_Position, true, p_AutonomyPathStoppingDistancePreset);
                        }
                        else
                        {
                            (result, resultMessage) = (false, GeometryTool.NavMeshAgentDriveState.Unreachable);
                        }
                    }
                }
                else
                {
                    (result, resultMessage) = (false, GeometryTool.NavMeshAgentDriveState.NavMeshAgentInvalid);
                }

                switch (resultMessage)
                {
                    case GeometryTool.NavMeshAgentDriveState.JumpGateRecognition:
                        // 오프 메쉬 관련 로직은 추후 재구현
                        /*
                        switch (_OfflinkProcessPhase)
                        {
                            case OfflinkProcessPhase.ReadyToOfflink:
                            {
                                _MasterNode.OnJumpUpAreal();
                                
                                var offLinkData = _NavMeshAgent.currentOffMeshLinkData;
                                _OffMeshLink_StartPosition = offLinkData.startPos;
                                _OffMeshLink_EndPosition = _NavMeshAgent.pathEndPosition;
                                
                                var offMeshLinkOffset =
                                    _OffMeshLink_StartPosition.GetDirectionVectorTo(_OffMeshLink_EndPosition);
                                var distanceFactor = offMeshLinkOffset.magnitude * 0.2f;
                                var offMeshLinkOffsetY = offMeshLinkOffset.y;
                                
                                _OffMeshLink_SpeedRate = Mathf.Clamp(distanceFactor, 0.6f, 6f);
                                if (offMeshLinkOffsetY < 0f)
                                {
                                    _JumpHeight = (-offMeshLinkOffsetY + _MasterNode._BattleStatusPreset.t_Current.GetJumpForce()) * 2f;
                                    _OffMeshLink_JumpOrbitEquation = 0.75f;
                                }
                                else
                                {
                                    _JumpHeight = (offMeshLinkOffsetY + _MasterNode._BattleStatusPreset.t_Current.GetJumpForce()) * 2f;
                                    if (offMeshLinkOffsetY > Height.CurrentValue)
                                    {
                                        _OffMeshLink_JumpOrbitEquation = 0.75f;
                                    }
                                    else
                                    {
                                        _OffMeshLink_JumpOrbitEquation = 0.5f;
                                    }
                                }
    
                                _OfflinkProcessPhase = OfflinkProcessPhase.OfflinkProgressing;
                            }
                                break;
                            case OfflinkProcessPhase.OfflinkProgressing:
                                break;
                            case OfflinkProcessPhase.OfflinkTerminate:
                                break;
                        }
                        */
                        break;
                    case GeometryTool.NavMeshAgentDriveState.CorrectSurfaceFailed:
                    case GeometryTool.NavMeshAgentDriveState.NavMeshAgentInvalid:
                    case GeometryTool.NavMeshAgentDriveState.Unreachable:
                    case GeometryTool.NavMeshAgentDriveState.SamePosition:
                        break;
                }

                return (result, resultMessage);
            }
            else
            {
                SetNavMeshAgentState(GeometryTool.NavMeshAgentState.Disable);
                return (false, GeometryTool.NavMeshAgentDriveState.MasterNodeCantMove);
            }
        }

        #endregion

        #region <Method/ManualPath>

        /// <summary>
        /// 길찾기 이동 큐에 다음 이동 목적지를 예약하는 메서드
        /// </summary>
        public (bool, GeometryTool.NavMeshAgentDriveState) AddManualMovePathEdge(Vector3 p_Edge)
        {
            if (IsNavMeshAgentEnable)
            {
                return SetDestination(p_Edge, false, default);
            }
            else
            {
                return SetPhysicsAutonomyMove(p_Edge, default);
            }
        }

        /// <summary>
        /// 길찾기 이동 큐에 다음 이동 목적지를 예약하는 메서드
        /// </summary>
        public (bool, GeometryTool.NavMeshAgentDriveState) SetManualMovePath(Vector3[] p_Path, GeometryTool.NavigateDestinationPreset p_AutonomyPathStoppingDistancePreset)
        {
            var isReserved = false;
            foreach (var edge in p_Path)
            {
                var (result, _) = AddManualMovePathEdge(edge);
                if (!isReserved && result)
                {
                    isReserved = true;
                }
            }

            return (isReserved, GeometryTool.NavMeshAgentDriveState.Reserved);
        }

        #endregion

        #region <Methods>

        /// <summary>
        /// 예약된 목적지가 있는지 리턴하는 메서드
        /// </summary>
        public bool HasReservedDestination()
        {
            return _ReservedNavMeshDestinationList.Count > 0;
        }
        
        /// <summary>
        /// 다음 목적지 위치로의 경로 계산을 수행하는 메서드
        /// </summary>
        private void TryCastNextDestination()
        {
            while (_ReservedNavMeshDestinationList.Count > 0)
            {
                var tryDestination = _ReservedNavMeshDestinationList[0];
                _ReservedNavMeshDestinationList.RemoveAt(0);
                
                // 지정한 위치로 경로계산을 수행한다.
                var (result, resultMessage) = SetDestination(tryDestination, true, default);
                if (result)
                {
                    /*switch (_Entity.MindModule.OnUpdateAutonomyPhysics(0f, _NavMeshAgent.stoppingDistance))
                    {
                        case GeometryTool.UpdateAutonomyPhysicsResult.CheckNextMoveDestination:
                        case GeometryTool.UpdateAutonomyPhysicsResult.DoNothing:
                            break;
                        case GeometryTool.UpdateAutonomyPhysicsResult.ProgressNavMeshControl:
                            // 지정한 위치로의 경로 계산에 성공한 경우
                            _StuckCheckCounter.Reset();
                            return;
                    }*/
                }
            }

            // 더 이상 경로가 없는 경우, 이동 종료 콜백을 사고모듈에 던진다.
            // _Entity.MindModule.OnAutonomyPhysicsPathOver();
        }

        /// <summary>
        /// 네브메쉬 에이전트의 속도를
        /// 0으로 하는 메서드
        /// </summary>
        public void ClearPhysicsAutonomyMove()
        {
            /*if (_IsNavMeshAgentDestinationValid)
            {
                _ReservedNavMeshDestinationList.Clear();
                _NavMeshAgent.updatePosition = false;
            
                _NavMeshAgentTaskPhase = NavMeshAgentTaskPhase.HasNoDestination; 
                SetNavMeshDestinationPivotPosition(_BlockClearDestinationFlag ? _NavMeshDestination : _Transform.position);
            
                _Entity.OnReachedDestination();
                ClearPhysicsAutonomySpeed();
                
                _InvalidPathCounter.Reset();
                _StuckCheckCounter.Reset();

                SetNavMeshAgentState(GeometryTool.NavMeshAgentState.Disable);
                OnUpdateLatestAutonomyPosition();
            }
            else
            {
                _InvalidPathCounter.Reset();
                _StuckCheckCounter.Reset();

                SetNavMeshAgentState(GeometryTool.NavMeshAgentState.Disable);
            }*/
        }

        /// <summary>
        /// 목적지를 지정하는 메서드
        /// </summary>
        private void SetNavMeshDestinationPivotPosition(Vector3 p_Pivot)
        {
            InitializePathPendingPreset(false);
            _NavMeshDestination = p_Pivot;
            InitializePathStuckPreset();
        }
        
        /// <summary>
        /// 네브메쉬 에이전트의 이동속도를
        /// 네브메쉬 에이전트의 고유속도에 파라미터로 받은 속도 비율 값
        /// 및 유닛 이동속도를 곱한 값으로 업데이트하는 메서드
        /// </summary>
        public void SetPhysicsAutonomySpeed(float p_Rate)
        {
            /*_NavMeshAgent.speed = _NavMeshAgentDefaultSpeed * p_Rate * _Entity.GetScaledMovementSpeed();*/
        }

        private void ClearPhysicsAutonomySpeed()
        {
            _NavMeshAgent.velocity = Vector3.zero;
            _NavMeshAgent.speed = 0f;
        }

        /// <summary>
        /// 네브메쉬 에이전트의 정지거리를 세트하는 메서드
        /// </summary>
        public void SetBreakingDistance(float p_Distance)
        {
            _NavMeshAgent.stoppingDistance = p_Distance;
        }
        
        /// <summary>
        /// 해당 네브메쉬 에이전트의 경로정보를 초기화시키는 메서드
        /// </summary>
        public void ResetNavigatePath()
        {
            if (_NavMeshAgent.isOnNavMesh)
            {
                _NavMeshAgent.ResetPath();
                ClearPhysicsAutonomyMove();
            }
        }

        /// <summary>
        /// 해당 네브메쉬 에이전트 활성화 여부를 리턴하는 논리 메서드
        /// </summary>
        public bool IsOnNavigate()
        {
            return IsNavMeshAgentEnable;
        }

        public (bool, Vector3) TryGetAutonomyDestination()
        {
            if (IsOnNavigate())
            {
                return (true, _NavMeshDestination);
            }
            else
            {
                return default;
            }
        }

        #endregion
    }
}