using System.Threading;
using Cysharp.Threading.Tasks;
using k514.Mono.Feature;
using UnityEngine;

namespace k514.Mono.Common
{
    /// <summary>
    /// 지정한 적으로부터 일정 거리까지 접근한 후 공격하는 AI
    /// 일정 확률로 공격 대신 뒤로 후퇴한다.
    /// </summary>
    public class CowardAutonomyMind : AutonomyMind
    {
        #region <Consts>

        public static (bool, MindModuleDataTableQuery.TableLabel, CowardAutonomyMind) CreateModule(IMindModuleDataTableRecordBridge p_ModuleRecord, IGameEntityBridge p_Entity)
        {
            return AutonomyMind.CreateModule(new CowardAutonomyMind(p_ModuleRecord, p_Entity));
        }
        
        public static async UniTask<(bool, MindModuleDataTableQuery.TableLabel, CowardAutonomyMind)> CreateModule(IMindModuleDataTableRecordBridge p_ModuleRecord, IGameEntityBridge p_Entity, CancellationToken p_CancellationToken)
        {
            return await AutonomyMind.CreateModule(new CowardAutonomyMind(p_ModuleRecord, p_Entity), p_CancellationToken);
        }
        
        #endregion
        
        #region <Constructor>

        private CowardAutonomyMind(IMindModuleDataTableRecordBridge p_ModuleRecord, IGameEntityBridge p_TargetEntity) : base(AutonomyModuleDataTableQuery.TableLabel.Coward, p_ModuleRecord, p_TargetEntity)
        {
        }

        #endregion

        #region <Callbacks>
        
        protected override void OnUpdateAIState_TimeBlock()
        {
            if (Entity.TryGetCurrentEnemy(out var o_Enemy))
            {
                if (IsOrderEmpty)
                {
                    Entity.SetLook(o_Enemy);
                    
                    if (_OrderInterval < 1)
                    {
                        var rand = Random.value;
                        switch (rand)
                        {
                            case var _ when rand > 0.15f:
                            {
                                if (ActionModule.TryGetRandomValidQuickCommand(out var o_Command))
                                {
                                    if (ActionModule.GetQuickCommandHandlerUnSafe(o_Command) is IActiveSkillEventHandler c_Handler)
                                    {
                                        Debug.LogError($"{o_Command} 예약");
                                        var engageRange = c_Handler.Record.EngageRange;
                                        ReserveMove(MindTool.MindOrderReserveType.Instant, new MindTool.MoveOrderParams(o_Enemy, GeometryTool.NavigationAttributeFlag.CalcRadius, AnimationTool.MoveMotionType.Run, engageRange, Entity.SightRange));
                                        ReserveInputCommand(MindTool.MindOrderReserveType.Add_RelayCancel, o_Command);
                                        ReserveFreeDelay(MindTool.MindOrderReserveType.Add, 0.3f);
                                    }
                                }
                                break;
                            }
                            case var _ when rand > 0f:
                            {
                                Debug.LogError($"후퇴 예약");
                                ReserveDrawBackFrom(MindTool.MindOrderReserveType.Instant, o_Enemy);
                                ReserveFreeDelay(MindTool.MindOrderReserveType.Add, 0.3f);
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
            else
            {
                if (_OrderInterval < 1)
                {
                    Entity.UpdateEnemy();
                    if (Entity.TryGetCurrentEnemy(out var o_NextEnemy))
                    {
                        InteractManager.GetInstanceUnsafe.ReserveUpdateInteraction(Entity, o_NextEnemy, true);
                    }
                    else
                    {
                        InteractManager.GetInstanceUnsafe.ReserveUpdateInteractionFrom(Entity, true);
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

        protected override void InitOrderInterval()
        {
            _OrderInterval = 0;
        }
        
        protected override void UpdateOrderInterval()
        {
            UpdateOrderInterval(4);
        }
        
        protected override void ProgressOrderInterval()
        {
            _OrderInterval -= 1;
        }

        #endregion
    }
}