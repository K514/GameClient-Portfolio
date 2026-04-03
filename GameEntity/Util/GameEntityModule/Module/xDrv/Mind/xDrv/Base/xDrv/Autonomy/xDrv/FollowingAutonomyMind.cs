using System.Threading;
using Cysharp.Threading.Tasks;
using k514.Mono.Feature;
using UnityEngine;

namespace k514.Mono.Common
{
    /// <summary>
    /// 지정한 적으로부터 일정 거리까지 접근한 후 공격하는 AI
    /// Master 개체가 있는 경우 해당 개체를 따라다닌다.
    /// </summary>
    public class FollowingAutonomyMind : AutonomyMind
    {
        #region <Consts>

        private const float __FollowStart_Distance = 12f;
        private const float __FollowStart_DistanceSqr = __FollowStart_Distance * __FollowStart_Distance;
        private const float __FollowRandomLowerBound_Distance = 4f;
        private const float __FollowRandomUpperBound_Distance = 8f;

        public static (bool, MindModuleDataTableQuery.TableLabel, FollowingAutonomyMind) CreateModule(IMindModuleDataTableRecordBridge p_ModuleRecord, IGameEntityBridge p_Entity)
        {
            return AutonomyMind.CreateModule(new FollowingAutonomyMind(p_ModuleRecord, p_Entity));
        }
        
        public static async UniTask<(bool, MindModuleDataTableQuery.TableLabel, FollowingAutonomyMind)> CreateModule(IMindModuleDataTableRecordBridge p_ModuleRecord, IGameEntityBridge p_Entity, CancellationToken p_CancellationToken)
        {
            return await AutonomyMind.CreateModule(new FollowingAutonomyMind(p_ModuleRecord, p_Entity), p_CancellationToken);
        }
        
        #endregion
        
        #region <Constructor>

        private FollowingAutonomyMind(IMindModuleDataTableRecordBridge p_ModuleRecord, IGameEntityBridge p_TargetEntity) : base(AutonomyModuleDataTableQuery.TableLabel.Following, p_ModuleRecord, p_TargetEntity)
        {
        }

        #endregion

        #region <Callbacks>
        
        protected override void _OnAwakeModule()
        {
            base._OnAwakeModule();

            Entity.SetStatus(StatusTool.BattleStatusGroupType.ExtraAdd, BattleStatusTool.BattleStatusType.SightRange, 500f);
        }
        
        protected override void OnUpdateAIState_TimeBlock()
        {
            if (Entity.TryGetCurrentEnemy(out var o_Enemy))
            {
                if (_OrderInterval < 1)
                {
                    var masterValid = Entity.TryGetMaster(out var o_Master);
                    if (masterValid)
                    {
                        InteractManager.GetInstanceUnsafe.ReserveUpdateInteraction(Entity, o_Master, true);
                        InteractManager.GetInstanceUnsafe.ReserveUpdateInteraction(Entity, o_Enemy, true);
                    }

                    /*
                    var masterSqrDistance = InteractManager.GetInstanceUnsafe.GetSqrDistanceBetween(Entity, o_Master);
                    var enemySqrDistance = InteractManager.GetInstanceUnsafe.GetSqrDistanceBetween(Entity, o_Enemy);
                    
                    switch (this)
                    {
                        case var _ when masterValid && masterSqrDistance > __FollowStart_DistanceSqr:
                        {
                            if (IsOrderEmpty)
                            {
                                ReserveMove(MindTool.MindOrderReserveType.Instant, new MindTool.MoveOrderParams(o_Master.GetRandomAroundPosition(__FollowRandomLowerBound_Distance, __FollowRandomUpperBound_Distance), GeometryTool.NavigationAttributeFlag.ForceSurface | GeometryTool.NavigationAttributeFlag.CalcRadius, AnimationTool.MoveMotionType.Run));
                            }
                            break;
                        }
                        case var _ when masterValid && enemySqrDistance > o_Master.SqrSightRange:
                        {
                            Entity.ClearEnemy();
                            ReserveMove(MindTool.MindOrderReserveType.Instant, new MindTool.MoveOrderParams(o_Master.GetRandomAroundPosition(__FollowRandomLowerBound_Distance, __FollowRandomUpperBound_Distance), GeometryTool.NavigationAttributeFlag.ForceSurface | GeometryTool.NavigationAttributeFlag.CalcRadius, AnimationTool.MoveMotionType.Run));
                            break;
                        }
                        default:
                        {
                            if (ActionModule.TryGetRandomValidQuickCommand(out var o_Command))
                            {
                                ClearOrderQueue(true);
                                
                                if (ActionModule.GetQuickCommandHandlerUnSafe(o_Command) is IActiveSkillEventHandler c_Handler)
                                {
                                    var engageRange = c_Handler.Record.EngageRange;
                                    ReserveMove(MindTool.MindOrderReserveType.Instant, new MindTool.MoveOrderParams(o_Enemy, GeometryTool.NavigationAttributeFlag.CalcRadius, AnimationTool.MoveMotionType.Run, engageRange, Entity.SightRange));
                                }
                          
                                ReserveInputCommand(MindTool.MindOrderReserveType.Add_RelayCancel, o_Command);
                            }
                            break;
                        }
                    }*/
                    
                    UpdateOrderInterval();
                }
                else
                {
                    ProgressOrderInterval();
                }
            }
            else
            {
                if (_OrderInterval < 1)
                {
                    var masterValid = Entity.TryGetMaster(out var o_Master);
                    if (masterValid)
                    {
                        InteractManager.GetInstanceUnsafe.ReserveUpdateInteraction(Entity, o_Master, true);
                    }
                    
                    switch (this)
                    {
                        /*case var _ when masterValid && InteractManager.GetInstanceUnsafe.GetSqrDistanceBetween(Entity, o_Master) > __FollowStart_DistanceSqr:
                        {
                            if (IsOrderEmpty)
                            {
                                ReserveMove(MindTool.MindOrderReserveType.Instant, new MindTool.MoveOrderParams(o_Master.GetRandomAroundPosition(__FollowRandomLowerBound_Distance, __FollowRandomUpperBound_Distance), GeometryTool.NavigationAttributeFlag.ForceSurface | GeometryTool.NavigationAttributeFlag.CalcRadius, AnimationTool.MoveMotionType.Run));
                            }
                            break;
                        }*/
                        default:
                        {
                            Entity.UpdateEnemy();
                            if (Entity.TryGetCurrentEnemy(out var o_NextEnemy))
                            {
                                ClearOrderQueue(true);
                            }
                            else
                            {
                                InteractManager.GetInstanceUnsafe.ReserveUpdateInteractionFrom(Entity, true);
                            }
                            break;
                        }
                    }
                          
                    UpdateOrderInterval();
                }
                else
                {
                    ProgressOrderInterval();
                }
            }
        }

        #endregion

        #region <Methods>

        protected override void UpdateOrderInterval()
        {
            UpdateOrderInterval(0);
        }

        #endregion
    }
}