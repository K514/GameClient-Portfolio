using System;
using UnityEngine;
using UnityEngine.AI;

namespace k514.Mono.Common
{
    public static class GeometryTool
    {
        #region <Consts>

        /// <summary>
        /// 기본 길찾기 종료 하한거리
        /// </summary>
        public const float __Default_Navigation_MinDistance = 0.1f;
            
        /// <summary>
        /// 기본 길찾기 종료 상한거리
        /// </summary>
        public const float __Default_Navigation_MaxDistance = CustomMath.MaxLineLength;
                
        /// <summary>
        /// 길찾기 Stuck 검출 거리 하한 기본값
        /// </summary>
        public const float __Default_StuckDistance_LowerBound = 0.001f;
            
        /// <summary>
        /// 길찾기 Stuck 상한값
        /// </summary>
        public const int __StuckCount_UpperBound = 10;
        
        /// <summary>
        /// 길찾기 Jitter 상한값
        /// </summary>
        public const int __JitterCount_UpperBound = 2;
            
        #endregion
        
        #region <Enums>

        /// <summary>
        /// 길찾기 페이즈
        /// </summary>
        public enum NavigatePhase
        {
            None,
            
            /// <summary>
            /// Interaction Manager 등에서 거리 테이블이 갱신되기까지 1프레임 쉬는 페이즈
            /// </summary>
            WaitSqrDistanceUpdate,
            
            Ready,
            Processing,
        }

        /// <summary>
        /// 길찾기 목적지 타입
        /// </summary>
        public enum NavigateDestinationType
        {
            /// <summary>
            /// 목적지 없음
            /// </summary>
            None,
            
            /// <summary>
            /// 목적지가 한 좌표인 경우
            /// </summary>
            Position,
            
            /// <summary>
            /// 목적지가 개체인 경우
            /// </summary>
            Entity,
        }

        [Flags]
        public enum NavigationAttributeFlag
        {
            None = 0,
            
            ForceSurface = 1 << 0,
            CalcRadius = 1 << 1,
            FallbackPosition = 1 << 2,
        }

        /// <summary>
        /// 길찾기 결과 타입
        /// </summary>
        public enum NavigateResultType
        {
            /// <summary>
            /// 유효하지 않은 요청
            /// </summary>
            None,
            
            /// <summary>
            /// 이동중
            /// </summary>
            Moving,
            
            /// <summary>
            /// 도착
            /// </summary>
            Reached,
            
            /// <summary>
            /// 범위 벗어남으로 인한 취소
            /// </summary>
            OutOfRange,
            
            /// <summary>
            /// 이동불가로 인한 취소
            /// </summary>
            Stuck,
            
            /// <summary>
            /// 진동으로 인한 취소
            /// </summary>
            Jitter,
            
            /// <summary>
            /// 외부에 의한 취소
            /// </summary>
            Canceled,
        }

        /// <summary>
        /// 길찾기 유닛이 적을 추적할 때, 추적할 위치를 선정하는 타입
        /// </summary>
        public enum TracePivotSelectType
        {
            /// <summary>
            /// 기본 타입.
            /// </summary>
            None,
            
            /// <summary>
            /// 적의 위치를 직접 참조하여 길찾기를 수행한다.
            /// </summary>
            TargetCenter,

            /// <summary>
            /// 타겟 유닛이 바라보는 방향으로 길찾기를 수행한다.
            ///
            /// 타겟이 이동한다고 해도, 그 방향쪽으로 길찾기를 수행하므로, DirectionToTarget 같은 버벅거림 없음
            /// </summary>
            TargetForward,
            
            /// <summary>
            /// 적의 위치로부터 해당 길찾기 에이전트 위치까지의 방향벡터를 구하여, 그 방향으로 적의 충돌 반경 + 공격 반큼만큼 멀어진 위치를 참조하여 길찾기를 수행한다.
            ///
            /// 타겟의 바로 뒤를 추적하기 때문에 추적자의 이동속도가 타겟보다 빠르고, 타겟이 계속 움직이는 경우 추적자가 금새 목적지에 도달하나
            /// 목적지도 금새 갱신되기 때문에 추적자의 움직임이 도달=>길찾기 를 반복하게 되어 버벅거림
            /// </summary>
            DirectionTargetToThis,
            
            /// <summary>
            /// 해당 길찾기 에이전트 위치에서 타겟으로의 방향벡터를 구하여, 그 방향으로 적의 충돌 반경 + 공격 반큼만큼 멀어진 위치를 참조하여 길찾기를 수행한다.
            /// </summary>
            DirectionThisToTarget,

            /// <summary>
            /// 적의 위치로부터 적의 충돌 반경 + 공격 반경만큼 랜덤한 방향으로 멀어진 위치를 참조하여 길찾기를 수행한다.
            /// </summary>
            RandomInRadius,
        }
        
        /// <summary>
        /// 길찾기 컴포넌트의 상태
        /// </summary>
        public enum NavMeshAgentState
        {
            /// <summary>
            /// 최초에 생성된 상태
            /// </summary>
            Spawned,
               
            /// <summary>
            /// 풀링되어 오브젝트가 활성화된 상태
            /// </summary>
            Pooled,
               
            /// <summary>
            /// 물리 에이전트가 활성화된 상태
            /// </summary>
            Enable,
               
            /// <summary>
            /// 물리 에이전트가 비활성화된 상태
            /// </summary>
            Disable
        }

        /// <summary>
        /// 네브메쉬 에이전트가 특정 위치를 pivot으로 설정했을 때, 해당 위치의 상태
        /// </summary>
        public enum NavMeshAgentDriveState
        {
            /// <summary>
            /// 네브 메쉬 에이전트를 활성화할 수 없는 경우
            /// </summary>
            NavMeshAgentInvalid,
            
            /// <summary>
            /// 지정한 위치에 도달 할 수 없는 경우
            /// </summary>
            Unreachable,
            
            /// <summary>
            /// 지정할 패스가 유효하지 않은 경우
            /// </summary>
            InvalidPath,
            
            /// <summary>
            /// 지정한 위치에 도달 할 수 있는 경우
            /// </summary>
            Reachable,
                        
            /// <summary>
            /// 지정한 위치가 현재 목표 위치와 같은 경우
            /// </summary>
            SameDestination,
                                    
            /// <summary>
            /// 지정한 위치가 현재 위치와 같은 경우
            /// </summary>
            SamePosition,
            
            /// <summary>
            /// 지정한 위치가 예약된 경우
            /// </summary>
            Reserved,
            
            /// <summary>
            /// 지정한 위치가 Off-Mesh Link 였던 경우
            /// </summary>
            JumpGateRecognition,
            
            /// <summary>
            /// 대상 유닛이 현재 움직일 수 있는 상태가 아닌 경우
            /// </summary>
            MasterNodeCantMove,
            
            /// <summary>
            /// 좌표 보정에 실패한 경우
            /// </summary>
            CorrectSurfaceFailed,
        }

        /// <summary>
        /// 길찾기 모듈에 의한 이동 적용 이후 결과 타입
        /// </summary>
        public enum UpdateAutonomyPhysicsResult
        {
            /// <summary>
            /// 다음 예약된 길찾기로 상태를 넘긴다.
            /// </summary>
            CheckNextMoveDestination,
            
            /// <summary>
            /// 길찾기를 수행하지 않는다.
            /// </summary>
            DoNothing,
            
            /// <summary>
            /// 길찾기를 속행한다.
            /// </summary>
            ProgressNavMeshControl,
        }
        
        #endregion

        #region <Methods>
        
        /// <summary>
        /// 지정한 게임 오브젝트에 네브메쉬 에이전트 컴포넌트를 비활성화 상태로 추가하는 메서드
        /// </summary>
        public static NavMeshAgent AddNavMeshAgentSafe(this GameObject p_GameObject)
        {
            // 네브메쉬 에이전트 생성시에는 위치가 가장 가까운 네브메쉬 서피스로 고정되므로
            // 컴포넌트를 추가하기 전에 현재 위치정보를 저장하고 게임오브젝트를 비활성화한 상태에서 컴포넌트 추가후 좌표를 복구시키고
            // 길찾기 에이전트를 비활성화 시킨 뒤, 게임오브젝트를 재활성화 시켜준다.
            var tryAffine = p_GameObject.transform;
            var prevPos = tryAffine.position;
            p_GameObject.SetActiveSafe(false);
            var result = p_GameObject.AddComponent<NavMeshAgent>();
            result.enabled = false;
            tryAffine.position = prevPos;
            p_GameObject.SetActiveSafe(true);

            return result;
        }

        /// <summary>
        /// 지정한 좌표가 네브메쉬 서피스인지 검증하는 메서드
        /// </summary>
        public static (bool, Vector3) IsNavMeshSurface(this Vector3 p_SourcePosition, float p_SampleRadius = 1f)
        {
            return (NavMesh.SamplePosition(p_SourcePosition, out var o_navMeshHit, p_SampleRadius, NavMesh.AllAreas), o_navMeshHit.position - p_SourcePosition);
        }
        
        /// <summary>
        /// 지정한 좌표를 기준으로 일정한 반경 안의 랜덤한 한 좌표를 샘플링하여 리턴한다.
        /// 2번째 파라미터가 참이라면 네브메쉬 서피스를 찾고 거짓이라면 Terrain 레이어를 찾는다.
        /// </summary>
        public static Vector3 GetRandomNavMeshSurfacePosition(this Vector3 p_SourcePosition, float p_SampleRadius, bool p_SampleNavMeshSurface)
        {
            return default;
            /*if (p_SampleNavMeshSurface)
            {
                NavMesh.SamplePosition(p_SourcePosition, out var o_Hit, p_SampleRadius, NavMesh.AllAreas);
                return o_Hit.position;
            }
            else
            {
                var correctedAffine
                    = AffineTool.CorrectPositionToSurface
                    (
                        p_SourcePosition + Random.insideUnitSphere * p_SampleRadius,
                        GameConst.Terrain_LayerMask
                    );
                
                return correctedAffine.Position;
            }*/
        }

        #endregion
        
        #region <Structs>

        /// <summary>
        /// 길찾기 목적지를 기술하는 프리셋
        /// </summary>
        public struct NavigateDestinationPreset : IEquatable<NavigateDestinationPreset>
        {
            #region <Fields>

            /// <summary>
            /// 길찾기 속성 플래그 마스크
            /// </summary>
            public readonly NavigationAttributeFlag NavigationAttributeFlagMask;

            /// <summary>
            /// 해당 프리셋 타입
            /// </summary>
            public NavigateDestinationType DestinationType;

            /// <summary>
            /// 목적지 좌표
            /// </summary>
            public Vector3 DestinationPosition;
            
            /// <summary>
            /// 목적지 개체
            /// </summary>
            public IGameEntityBridge DestinationEntity;

            /// <summary>
            /// 정지 거리 하한
            /// </summary>
            public float MinDistance;

            /// <summary>
            /// 정지 거리 하한
            /// </summary>
            public float MaxDistance;
 
            /// <summary>
            /// 이전 길찾기에서 목적지까지 경로 정보
            /// </summary>
            public NavigationStatePreset LatestProgress;
            
            /// <summary>
            /// 반경 계산 플래그
            /// </summary>
            private bool _CalcRadiusFlag;
                    
            /// <summary>
            /// 목적지까지의 거리가 변하지 않은 채로 길찾기 함수가 갱신된 횟수를 표시하는 카운터
            /// </summary>
            private int _StuckCount;
            
            /// <summary>
            /// 목적지까지의 방향이 정반대가 된 채로 길찾기 함수가 갱신된 횟수를 표시하는 카운터
            /// </summary>
            private int _JitterCount;
  
            /// <summary>
            /// 유효성 플래그
            /// </summary>
            public bool ValidFlag;
            
            #endregion

            #region <Constructors>

            public NavigateDestinationPreset(Vector3 p_Destination, NavigationAttributeFlag p_AttributeMask, float p_MinDistance = __Default_Navigation_MinDistance, float p_MaxDistance = __Default_Navigation_MaxDistance)
            {
                var valid = !p_AttributeMask.HasAnyFlagExceptNone(NavigationAttributeFlag.ForceSurface) || p_Destination.TryGetTerrainSurfacePosition(out p_Destination);
                if (valid)
                {
                    DestinationType = NavigateDestinationType.Position;
                    NavigationAttributeFlagMask = p_AttributeMask;
                    DestinationPosition = p_Destination;
                    DestinationEntity = default;
                    MinDistance = Mathf.Max(__Default_Navigation_MinDistance, p_MinDistance);
                    MaxDistance = Mathf.Min(__Default_Navigation_MaxDistance, p_MaxDistance);
                    LatestProgress = default;
                    _CalcRadiusFlag = NavigationAttributeFlagMask.HasAnyFlagExceptNone(NavigationAttributeFlag.CalcRadius);
                    _StuckCount = default;
                    _JitterCount = default;
                    ValidFlag = true;
                }
                else
                {
                    this = default;
                }
            }
            
            public NavigateDestinationPreset(IGameEntityBridge p_Destination, NavigationAttributeFlag p_AttributeMask, float p_MinDistance = __Default_Navigation_MinDistance, float p_MaxDistance = __Default_Navigation_MaxDistance)
            {
                if (p_Destination.IsEntityValid())
                {
                    DestinationType = NavigateDestinationType.Entity;
                    NavigationAttributeFlagMask = p_AttributeMask;
                    DestinationPosition = p_Destination.GetBottomPosition();
                    DestinationEntity = p_Destination;
                    MinDistance = Mathf.Max(__Default_Navigation_MinDistance, p_MinDistance);
                    MaxDistance = Mathf.Min(__Default_Navigation_MaxDistance, p_MaxDistance);
                    MaxDistance = p_MaxDistance;
                    LatestProgress = default;
                    _CalcRadiusFlag = NavigationAttributeFlagMask.HasAnyFlagExceptNone(NavigationAttributeFlag.CalcRadius);
                    _StuckCount = default;
                    _JitterCount = default;
                    ValidFlag = true;
                }
                else
                {
                    this = default;
                }
            }

            #endregion

            #region <Operator>

            /// <summary>
            /// IEquatable 동등성 검증 메서드
            /// </summary>
            public override bool Equals(object p_Right)
            {
                return p_Right is NavigateDestinationPreset c_Right && Equals(c_Right);
            }
        
            /// <summary>
            /// IEquatable<T>동등성 검증 메서드
            /// </summary>
            public bool Equals(NavigateDestinationPreset p_RightValue)
            {
                switch (DestinationType)
                {
                    default:
                    case NavigateDestinationType.None:
                        return DestinationType == p_RightValue.DestinationType;
                    case NavigateDestinationType.Position:
                        return
                            DestinationType == p_RightValue.DestinationType
                            && (DestinationPosition - p_RightValue.DestinationPosition).IsReachedZero();
                    case NavigateDestinationType.Entity:
                        return DestinationType == p_RightValue.DestinationType
                               && ReferenceEquals(DestinationEntity, p_RightValue.DestinationEntity);
                }
            }

            /// <summary>
            /// 해쉬코드는 불변값이므로 미리 계산해서 캐싱한 값을 리턴한다.
            /// </summary>
            public override int GetHashCode()
            {
                return DestinationType switch
                {
                    NavigateDestinationType.None => HashCode.Combine(DestinationType),
                    NavigateDestinationType.Position => HashCode.Combine(DestinationType, NavigationAttributeFlagMask, DestinationPosition),
                    NavigateDestinationType.Entity => HashCode.Combine(DestinationType, NavigationAttributeFlagMask, DestinationEntity),
                };
            }

            /// <summary>
            /// 동등연산자 == 재정의
            /// </summary>
            public static bool operator ==(NavigateDestinationPreset p_Left, NavigateDestinationPreset p_Right)
            {
                return p_Left.Equals(p_Right);
            }

            /// <summary>
            /// 동등연산자 != 재정의
            /// </summary>
            public static bool operator !=(NavigateDestinationPreset p_Left, NavigateDestinationPreset p_Right)
            {
                return !p_Left.Equals(p_Right);
            }

            #endregion

            #region <Callbacks>

            public bool OnSelected(IGameEntityBridge p_Entity)
            {
                switch (DestinationType)
                {
                    default:
                    case NavigateDestinationType.None:
                    case NavigateDestinationType.Position:
                    {
                        return false;
                    }
                    case NavigateDestinationType.Entity:
                    {
                        if (DestinationEntity.IsEntityValid())
                        {
                            DestinationPosition = DestinationEntity.GetBottomPosition();
                            InteractManager.GetInstanceUnsafe.ReserveUpdateInteraction(p_Entity, DestinationEntity, true);

                            return true;
                        }
                        else
                        {
                            OnDestinationEntityInvalid();

                            return false;
                        }
                    }
                } 
            }

            private void OnDestinationEntityInvalid()
            {
                if (NavigationAttributeFlagMask.HasAnyFlagExceptNone(NavigationAttributeFlag.FallbackPosition))
                {
                    DestinationEntity = default;
                    DestinationType = NavigateDestinationType.Position;
                }
                else
                {
                    this = default;
                }
            }
            
            #endregion
            
            #region <Methods>
            
            public void SetMinDistance(float p_MinDistance)
            {
                MinDistance = p_MinDistance;
            }
            
            public void SetMaxDistance(float p_MaxDistance)
            {
                MaxDistance = p_MaxDistance;
            }
            
            public void SetDistance(float p_MinDistance, float p_MaxDistance)
            {
                SetMinDistance(p_MinDistance);
                SetMaxDistance(p_MaxDistance);
            }
                    
            public NavigationResultPreset CalcNavigate(IGameEntityBridge p_Trigger, NavigationStatePreset p_Preset, float p_StartTimeStamp, float p_DeltaTime)
            {
                if (p_Preset.IsOver)
                {
                    return new NavigationResultPreset(NavigateResultType.Reached, p_StartTimeStamp, p_DeltaTime, LatestProgress = p_Preset);
                }
                else
                {
                    switch (DestinationType)
                    {
                        default:
                        case NavigateDestinationType.None:
                        {
                            return new NavigationResultPreset(NavigateResultType.None, p_StartTimeStamp, p_DeltaTime, LatestProgress = p_Preset);
                        }
                        case NavigateDestinationType.Position:
                        {
                            var radius = _CalcRadiusFlag ? p_Trigger.GetRadius() : 0f;
                            var correctedMinDistance = radius + MinDistance;
                            var correctedMinSqrDistance = correctedMinDistance * correctedMinDistance;
                            var correctedMaxDistance = radius + MaxDistance;
                            var correctedMaxSqrDistance = correctedMaxDistance * correctedMaxDistance;
                            var sqrDistance = p_Preset.SqrDistance;
                            
                            switch (this)
                            {
                                case var _ when correctedMinSqrDistance > sqrDistance:
                                {
                                    return new NavigationResultPreset(NavigateResultType.Reached, p_StartTimeStamp, p_DeltaTime, LatestProgress = p_Preset);
                                }
                                case var _ when sqrDistance > correctedMaxSqrDistance:
                                {
                                    return new NavigationResultPreset(NavigateResultType.OutOfRange, p_StartTimeStamp, p_DeltaTime, LatestProgress = p_Preset);
                                }
                                default:
                                {
                                    if (sqrDistance.IsReachedValue(LatestProgress.SqrDistance, __Default_StuckDistance_LowerBound))
                                    {
                                        if (_StuckCount++ > __StuckCount_UpperBound)
                                        {
                                            return new NavigationResultPreset(NavigateResultType.Stuck, p_StartTimeStamp, p_DeltaTime, LatestProgress = p_Preset);
                                        }
                                        else
                                        {
                                            return new NavigationResultPreset(NavigateResultType.Moving, p_StartTimeStamp, p_DeltaTime, LatestProgress = p_Preset);
                                        }
                                    }
                                    else
                                    {
                                        if ((p_Preset.UV + LatestProgress.UV).IsReachedZero())
                                        {
                                            if (_JitterCount++ > __JitterCount_UpperBound)
                                            {
                                                return new NavigationResultPreset(NavigateResultType.Jitter, p_StartTimeStamp, p_DeltaTime, LatestProgress = p_Preset);
                                            }
                                        }
                                        else
                                        {
                                            _JitterCount = 0;
                                        }
                                        
                                        _StuckCount = 0;
                                        return new NavigationResultPreset(NavigateResultType.Moving, p_StartTimeStamp, p_DeltaTime, LatestProgress = p_Preset);
                                    }
                                }
                            }
                        }
                        case NavigateDestinationType.Entity:
                        {
                            if (DestinationEntity.IsEntityValid())
                            {
                                var radius = _CalcRadiusFlag ? p_Trigger.GetRadiusWithSkinOffset() + DestinationEntity.GetRadiusWithSkinOffset() : 0f;
                                var correctedMinDistance = radius + MinDistance;
                                var correctedMinSqrDistance = correctedMinDistance * correctedMinDistance;
                                var correctedMaxDistance = radius + MaxDistance;
                                var correctedMaxSqrDistance = correctedMaxDistance * correctedMaxDistance;
                                var sqrDistance = p_Preset.SqrDistance;

                                switch (this)
                                {
                                    case var _ when correctedMinSqrDistance > sqrDistance:
                                    {
                                        DestinationPosition = DestinationEntity.GetBottomPosition();
                                        return new NavigationResultPreset(NavigateResultType.Reached, p_StartTimeStamp, p_DeltaTime, LatestProgress = p_Preset);
                                    }
                                    case var _ when sqrDistance > correctedMaxSqrDistance:
                                    {
                                        DestinationPosition = DestinationEntity.GetBottomPosition();
                                        return new NavigationResultPreset(NavigateResultType.OutOfRange, p_StartTimeStamp, p_DeltaTime, LatestProgress = p_Preset);
                                    }
                                    default:
                                    {
                                        if (sqrDistance.IsReachedValue(LatestProgress.SqrDistance, __Default_StuckDistance_LowerBound))
                                        {
                                            if (_StuckCount++ > __StuckCount_UpperBound)
                                            {
                                                DestinationPosition = DestinationEntity.GetBottomPosition();
                                                return new NavigationResultPreset(NavigateResultType.Stuck, p_StartTimeStamp, p_DeltaTime, LatestProgress = p_Preset);
                                            }
                                            else
                                            {
                                                InteractManager.GetInstanceUnsafe.ReserveUpdateInteraction(p_Trigger, DestinationEntity, true);
                                                DestinationPosition = DestinationEntity.GetBottomPosition();
                                                return new NavigationResultPreset(NavigateResultType.Moving, p_StartTimeStamp, p_DeltaTime, LatestProgress = p_Preset);
                                            }
                                        }
                                        else
                                        {
                                            if ((p_Preset.UV + LatestProgress.UV).IsReachedZero())
                                            {
                                                if (_JitterCount++ > __JitterCount_UpperBound)
                                                {
                                                    DestinationPosition = DestinationEntity.GetBottomPosition();
                                                    return new NavigationResultPreset(NavigateResultType.Jitter, p_StartTimeStamp, p_DeltaTime, LatestProgress = p_Preset);
                                                }
                                            }
                                            else
                                            {
                                                _JitterCount = 0;
                                            }
                                            
                                            _StuckCount = 0;
                                            InteractManager.GetInstanceUnsafe.ReserveUpdateInteraction(p_Trigger, DestinationEntity, true);
                                            DestinationPosition = DestinationEntity.GetBottomPosition();
                                            return new NavigationResultPreset(NavigateResultType.Moving, p_StartTimeStamp, p_DeltaTime, LatestProgress = p_Preset);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                OnDestinationEntityInvalid();

                                if (DestinationType == NavigateDestinationType.Position)
                                {
                                    goto case NavigateDestinationType.Position;
                                }
                                else
                                {
                                    return new NavigationResultPreset(NavigateResultType.None, p_StartTimeStamp, p_DeltaTime, LatestProgress = p_Preset);
                                }
                            }
                        }
                    }
                }
            }

            #endregion
        }

        public struct NavigationStatePreset
        {
            #region <Fields>

            public readonly Vector3 UV;
            public readonly float SqrDistance;
            public readonly bool IsOver;

            #endregion

            #region <Constructor>

            public NavigationStatePreset(Vector3 p_DestinationUV, float p_SqrDistance, bool p_IsOver)
            {
                UV = p_DestinationUV;
                SqrDistance = p_SqrDistance;
                IsOver = p_IsOver;
            }
            
            #endregion
        }

        
        public struct NavigationResultPreset
        {
            #region <Fields>

            public readonly NavigateResultType NavigateResultType;
            public readonly float StartTimeStamp;
            public readonly float DeltaTime;
            public readonly NavigationStatePreset NavigationProgressPreset;
            public readonly bool IsOver;
            public readonly bool IsMoving;

            #endregion

            #region <Constructors>

            public NavigationResultPreset(NavigateResultType p_NavigateResultType, float p_StartTimeStamp, float p_DeltaTime, NavigationStatePreset p_Preset)
            {
                NavigateResultType = p_NavigateResultType;
                StartTimeStamp = p_StartTimeStamp;
                DeltaTime = p_DeltaTime;
                NavigationProgressPreset = p_Preset;
                IsOver = p_NavigateResultType switch
                {
                    NavigateResultType.None => true,
                    NavigateResultType.Reached => true,
                    NavigateResultType.OutOfRange => true,
                    NavigateResultType.Stuck => true,
                    NavigateResultType.Jitter => true,
                    _ => false,
                };
                IsMoving = NavigateResultType == NavigateResultType.Moving;
            }

            #endregion
        }

        #endregion
    }
}