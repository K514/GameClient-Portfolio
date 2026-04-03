using System;
using System.Linq;
using UnityEngine;

namespace k514.Mono.Common
{
    public static partial class PhysicsTool
    {
        #region <Consts>

        /// <summary>
        /// NonAlloc 캐스트용 배열
        /// </summary>
        public static readonly RaycastHit[] _NonAllocRayCast;

        /// <summary>
        /// NonAlloc 오버랩용 배열
        /// </summary>
        public static readonly Collider[] _NonAllocCollider;

        /// <summary>
        /// None 타입을 제외한 힘 타입 순환자
        /// </summary>
        public static readonly ForceType[] _ForceTypeEnumeratorExceptNone;

        /// <summary>
        /// 모든 힘 타입 순환자
        /// </summary>
        public static readonly ForceType[] _ForceTypeEnumerator;

        /// <summary>
        /// 감쇄를 적용받지 않는 힘 타입 순환자
        /// </summary>
        public static readonly ForceType[] _ForceTypeEnumeratorExceptGravity;

        #endregion
        
        #region <Enums>

        /// <summary>
        /// 운동계 타입
        /// </summary>
        [Flags]
        public enum ForceType
        {
            None = 0,
            Default = 1 << 0,
            Jump = 1 << 1,
            SyncWithController = 1 << 2,
            Gravity = 1 << 3,
        }

        /// <summary>
        /// 중력 적용 방식
        /// </summary>
        public enum GravityType
        {
            /// <summary>
            /// 중력을 항상 적용
            /// </summary>
            Applied,
            
            /// <summary>
            /// 타격을 받지 않는 이상 중력 무시
            /// </summary>
            Anti_HitBreak,
            
            /// <summary>
            /// 일정 시간 중력 무시
            /// </summary>
            Anti_Duration,
            
            /// <summary>
            /// 중력을 항상 무시
            /// </summary>
            Anti_Perfect
        }

        /// <summary>
        /// 유닛 밟기 타입
        /// </summary>
        [Flags]
        public enum StampResultFlag
        {
            /// <summary>
            /// 밟은 오브젝트가 없음
            /// </summary>
            None = 0,
            
            /// <summary>
            /// 두 유닛이 일부분이라도 겹친 경우
            /// </summary>
            UnitOverlapped = 1 << 0,
            
            /// <summary>
            /// 한 유닛이 다른 유닛 위를 밟고 올라선 경우
            /// </summary>
            UnitStamped = 1 << 1,
            
            /// <summary>
            /// 한 유닛이 다르 유닛 위에 있지만, 듀 유닛이 충돌하지 않는 유닛이라
            /// 이후 중력에 의해 그대로 뚫고 떨어지는 경우
            /// </summary>
            UnitThroughIn = 1 << 2,
            
            /// <summary>
            /// 한 유닛이 지형 레이어 오브젝트를 밟고 올라선 경우
            /// </summary>
            TerrainStamped = 1 << 3,
            
            /// <summary>
            /// 한 유닛이 장해물 레이어 오브젝트를 밟고 올라선 경우
            /// </summary>
            ObstacleStamped = 1 << 4,
        }
        
        #endregion

        #region <Constructor>

        static PhysicsTool()
        {
            _NonAllocRayCast = new RaycastHit[512];
            _NonAllocCollider = new Collider[512];
            
            _ForceTypeEnumerator = EnumFlag.GetEnumEnumerator<ForceType>(EnumFlag.GetEnumeratorType.GetAll);
            _ForceTypeEnumeratorExceptNone = EnumFlag.GetEnumEnumerator<ForceType>(EnumFlag.GetEnumeratorType.ExceptNone);
            _ForceTypeEnumeratorExceptGravity = _ForceTypeEnumeratorExceptNone.Where(type => type != ForceType.Gravity).ToArray();
        }

        #endregion
        
        #region <Methods>

        /// <summary>
        /// 레이캐스팅의 결과셋이 지정한 아핀객체 이외의 것을 포함하는지 검증하는 메서드
        /// </summary>
        public static bool IsAnyAffine_ExistAt_RayCastResult_Except(Transform p_Affine, int p_Count)
        {
            for (int i = 0; i < p_Count; i++)
            {
                if (!ReferenceEquals(p_Affine, _NonAllocRayCast[i].transform))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 오버랩의 결과셋이 지정한 아핀객체 이외의 것을 포함하는지 검증하는 메서드
        /// </summary>
        public static bool IsAnyAffine_ExistAt_OverlapResult_Except(Transform p_Affine, int p_Count)
        {
            for (int i = 0; i < p_Count; i++)
            {
                if (!ReferenceEquals(p_Affine, _NonAllocCollider[i].transform))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool TryGetTerrainSurfacePosition(this Vector3 p_Position, out Vector3 o_SurfacePosition)
        {
            var (valid, rayCastHit) = GetHighestObject_RayCast(p_Position, GameConst.EmptySurface_LayerMask, QueryTriggerInteraction.Ignore);
            if (valid)
            {
                if (rayCastHit.collider.IsLayerType(GameConst.GameLayerType.Terrain))
                {
                    o_SurfacePosition = rayCastHit.point;
                    return true;
                }
            }

            o_SurfacePosition = p_Position;
            return false; 
        }
        
        #endregion

        #region <Structs>

        public struct StampPreset
        {
            #region <Fields>

            /// <summary>
            /// 밟고 있는 유닛
            /// </summary>
            public IGameEntityBridge Stamping;

            /// <summary>
            /// 밟힌 유닛
            /// </summary>
            public IGameEntityBridge Stamped;

            /// <summary>
            /// 검증 결과
            /// </summary>
            public StampResultFlag StampStateFlag;

            #endregion
            
            #region <Constructors>

            public StampPreset(IGameEntityBridge p_Pivot)
            {
                StampStateFlag = StampResultFlag.None;
                Stamping = p_Pivot;
                Stamped = null;

                /*
                var count = PhysicsTool.GetCount_CapsuleOverlap(Stamping, GameManager.Obstacle_Terrain_UnitSet_ExceptC_LayerMask, QueryTriggerInteraction.Collide);
                var targetColliderSet = PhysicsTool._NonAllocCollider;
                var compareAffine = Stamping.Affine;
                var stampingUnitBottomPosition = Stamping.Affine.position;
                var stampingUnitRadius = Stamping.GetRadius();
                var pivotLayer = p_Pivot.gameObject.layer;
                
                for (var i = 0; i < count; i++)
                {
                    var tryCollider = targetColliderSet[i];
                    var tryGameObject = tryCollider.gameObject;
                    var tryLayer = tryGameObject.layer;
                    var tryLayerType = (GameManager.GameLayerType) tryLayer;
            
                    switch (tryLayerType)
                    {
                        case GameManager.GameLayerType.UnitA:    
                        case GameManager.GameLayerType.UnitB:    
                        case GameManager.GameLayerType.UnitD:
                        {
                            if (ReferenceEquals(null, Stamped))
                            {
                                var tryAffine = tryCollider.transform;
                                if (!ReferenceEquals(compareAffine, tryAffine))
                                {
                                    var (valid, unit) = InteractManager.GetInstanceUnsafe.TryGetUnit(tryGameObject);
                                    Stamped = valid ? unit : tryAffine.GetComponent<GameEntityBase>();

                                    if (!ReferenceEquals(null, Stamped))
                                    {
                                        var unitStampEventType = Physics.GetIgnoreLayerCollision(tryLayer, pivotLayer) ? StampResultFlag.UnitThroughIn : StampResultFlag.UnitStamped;
                                        var stampedUnitTopPosition = Stamped.GetTopPosition();
                                        var stampedUnitRadius = Stamped.GetRadius();
                                        if (stampedUnitTopPosition.y - stampingUnitBottomPosition.y > 0f
                                            && stampedUnitTopPosition.GetSqrDistanceTo(stampingUnitBottomPosition) > Mathf.Pow(Mathf.Min(stampedUnitRadius, stampingUnitRadius), 2))
                                        {
                                            StampStateFlag.AddFlag(StampResultFlag.UnitOverlapped | unitStampEventType);
                                        }
                                        else
                                        {
                                            StampStateFlag.AddFlag(unitStampEventType);
                                        }
                                    }
                                }
                            }
                        }
                            break;
                        case GameManager.GameLayerType.Terrain:
                        {
                            StampStateFlag.AddFlag(StampResultFlag.TerrainStamped);
                        }
                            break;
                        case GameManager.GameLayerType.Obstacle:
                        {
                            StampStateFlag.AddFlag(StampResultFlag.ObstacleStamped);
                        }
                            break;
                    }
                }*/
            }

            #endregion

            #region <Methods>

            public bool IsStampedTerrainOrObstacle() => StampStateFlag.HasAnyFlagExceptNone(StampResultFlag.TerrainStamped | StampResultFlag.ObstacleStamped);
            public bool IsStampedOtherUnit() => StampStateFlag.HasAnyFlagExceptNone(StampResultFlag.UnitStamped);
            public bool IsStampedOverlappedUnit() => StampStateFlag.HasAnyFlagExceptNone(StampResultFlag.UnitOverlapped);
            public (bool, IGameEntityBridge) TryGetStampedUnit() => IsStampedOtherUnit() ? (true, Stamped) : default;

            #endregion
        }

        /// <summary>
        /// 유닛 외력 프리셋
        /// </summary>
        public struct UnitAddForceParams
        {
            #region <Fields>

            public UnitAddForceParamsType _UnitAddForceParamsType;
            public int UnitAddForceRecordIndex;
            public AffinePreset PrevAffine;
            public IGameEntityBridge PivotTrigger;
            public Vector3 PivotPosition;
            public bool IsReentered;

            #endregion

            #region <Enums>

            public enum UnitAddForceParamsType
            {
                None,
                Forced,
                TriggeredUnit,
                TriggeredPivot,
            }

            #endregion
            
            #region <Constructors>

            public UnitAddForceParams(int p_UnitAddForceRecordIndex, IGameEntityBridge p_ForcedUnit)
            {
                _UnitAddForceParamsType = p_UnitAddForceRecordIndex != default
                    ? UnitAddForceParamsType.Forced
                    : UnitAddForceParamsType.None;
                
                UnitAddForceRecordIndex = p_UnitAddForceRecordIndex;
                PrevAffine = new AffinePreset(p_ForcedUnit, false);
                PivotTrigger = p_ForcedUnit;
                PivotPosition = p_ForcedUnit.Affine.position;
                IsReentered = default;
            }
            
            public UnitAddForceParams(int p_UnitAddForceRecordIndex, IGameEntityBridge p_ForcedUnit, IGameEntityBridge p_PivotTrigger)
            {
                _UnitAddForceParamsType = p_UnitAddForceRecordIndex != default
                    ? UnitAddForceParamsType.TriggeredUnit
                    : UnitAddForceParamsType.None;
                
                UnitAddForceRecordIndex = p_UnitAddForceRecordIndex;
                PrevAffine = new AffinePreset(p_ForcedUnit, false);
                PivotTrigger = p_PivotTrigger;
                PivotPosition = p_PivotTrigger.Affine.position;
                IsReentered = default;
            }
            
            public UnitAddForceParams(int p_UnitAddForceRecordIndex, IGameEntityBridge p_ForcedUnit, Vector3 p_PivotPosition)
            {
                _UnitAddForceParamsType = p_UnitAddForceRecordIndex != default
                    ? UnitAddForceParamsType.TriggeredPivot
                    : UnitAddForceParamsType.None;
                
                UnitAddForceRecordIndex = p_UnitAddForceRecordIndex;
                PrevAffine = new AffinePreset(p_ForcedUnit, false);
                PivotTrigger = default;
                PivotPosition = p_PivotPosition;
                IsReentered = default;
            }

            #endregion

            #region <Methods>

            public bool IsValid() => _UnitAddForceParamsType != UnitAddForceParamsType.None;
            
            public void UpdateAffine(IGameEntityBridge p_Master)
            {
                IsReentered = true;
                PrevAffine = new AffinePreset(p_Master, false);
            }

            public bool CheckBoundDistance(IGameEntityBridge p_MasterNode)
            {
                return false;

                /*switch (_UnitAddForceParamsType)
                {
                    case UnitAddForceParamsType.Forced:
                    case UnitAddForceParamsType.TriggeredPivot:
                    {
                        return p_MasterNode.GetSqrDistanceTo(PivotPosition) < Mathf.Pow(p_MasterNode.GetRadius() + Record.PivotDistanceBound, 2f);
                    }
                    case UnitAddForceParamsType.TriggeredUnit:
                    {
                        return p_MasterNode.GetSqrDistanceTo(PivotPosition) < Mathf.Pow(p_MasterNode.GetRadius() + Record.PivotDistanceBound + PivotTrigger.GetRadius(), 2f);
                    }
                    default:
                        return false;
                }*/
            }

            #endregion
        }

        #endregion

        #region <Classes>

        public class PhysicsSystem
        {
            #region <Fields>

            public readonly IGameEntityBridge Entity;
            public readonly ForceType ForceType;
            public Vector3 Acceleration;
            public Vector3 VelocityPreDamping;
            public Vector3 Velocity;
            public bool IsVelocityValid;

            #endregion

            #region <Constructor>

            public PhysicsSystem(IGameEntityBridge p_Entity, ForceType p_Type)
            {
                Entity = p_Entity;
                ForceType = p_Type;
                Acceleration = Vector3.zero;
                Velocity = Vector3.zero;
                IsVelocityValid = false;
            }

            #endregion
            
            #region <Operator>

            public static implicit operator bool(PhysicsSystem p_PhysicsSystem)
            {
                return p_PhysicsSystem.IsVelocityValid;
            }

            #endregion
            
            #region <Methods>

            public bool UpdateForce(float p_DeltaTime)
            {
                // v = _v + acc * dt
                Velocity += Acceleration * p_DeltaTime;
                
                if (Velocity.IsReachedZero())
                {
                    Velocity = Vector3.zero;
                    return IsVelocityValid = false;
                }
                else
                {
                    return IsVelocityValid = true;
                }
            }
            
            public void DampingForce()
            {
                if (IsVelocityValid)
                {
                    VelocityPreDamping = Velocity;
                    Velocity = (EnvironmentManager.__DampenRate * Velocity).FloorVectorValue(EnvironmentManager.__VelocityElementLowerBound);
                    
                    if (Velocity.IsReachedZero())
                    {
                        Velocity = Vector3.zero;
                        IsVelocityValid = false;
                    }
                }
                
                ClearAcceleration();
            }

            public void ClearForce()
            {
                ClearAcceleration();
                ClearVelocity();
            }
            
            #endregion
            
            #region <Methods/Acceleration>

            public void AddAcceleration(Vector3 p_Acc)
            {
                Acceleration += p_Acc;
            }
            
            public void OverlapAcceleration(Vector3 p_Acc)
            {
                Acceleration = p_Acc;
            }
            
            public void ClearAcceleration()
            {
                Acceleration = Vector3.zero;
            }
            
            #endregion
            
            #region <Methods/Velocity>

            public void AddVelocity(Vector3 p_Velocity)
            {
                Velocity += p_Velocity;
            }
            
            public void OverlapVelocity(Vector3 p_Velocity)
            {
                Velocity = p_Velocity;
            }
            
            public void ClearVelocity()
            {
                if (IsVelocityValid)
                {
                    IsVelocityValid = false;
                    VelocityPreDamping = Vector3.zero;
                    Velocity = Vector3.zero;
                }
            }
            
            public void ClearYLowerVelocity()
            {
                if (Velocity.y < 0f)
                {
                    ClearYVelocity();
                }
            }
            
            public void ClearYUpperVelocity()
            {
                if (Velocity.y > 0f)
                {
                    ClearYVelocity();
                }
            } 
            
            public void ClearYVelocity()
            {
                Velocity = Velocity.XZVector();

                if (IsVelocityValid)
                {
                    if (Velocity.IsReachedZero())
                    {
                        IsVelocityValid = false;
                    }
                }
            } 
            
            #endregion
        }

        #endregion
    }
}