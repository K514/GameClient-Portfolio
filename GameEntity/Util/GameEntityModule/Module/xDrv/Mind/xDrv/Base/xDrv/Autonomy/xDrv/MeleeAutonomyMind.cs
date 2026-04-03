using System.Threading;
using Cysharp.Threading.Tasks;
using k514.Mono.Feature;

namespace k514.Mono.Common
{
    /// <summary>
    /// 지정한 적으로부터 일정 거리까지 접근한 후 공격하는 AI
    /// </summary>
    public class MeleeAutonomyMind : AutonomyMind
    {
        #region <Consts>

        public static (bool, MindModuleDataTableQuery.TableLabel, MeleeAutonomyMind) CreateModule(IMindModuleDataTableRecordBridge p_ModuleRecord, IGameEntityBridge p_Entity)
        {
            return AutonomyMind.CreateModule(new MeleeAutonomyMind(p_ModuleRecord, p_Entity));
        }
        
        public static async UniTask<(bool, MindModuleDataTableQuery.TableLabel, MeleeAutonomyMind)> CreateModule(IMindModuleDataTableRecordBridge p_ModuleRecord, IGameEntityBridge p_Entity, CancellationToken p_CancellationToken)
        {
            return await AutonomyMind.CreateModule(new MeleeAutonomyMind(p_ModuleRecord, p_Entity), p_CancellationToken);
        }
        
        #endregion
        
        #region <Constructor>

        private MeleeAutonomyMind(IMindModuleDataTableRecordBridge p_ModuleRecord, IGameEntityBridge p_TargetEntity) : base(AutonomyModuleDataTableQuery.TableLabel.Melee, p_ModuleRecord, p_TargetEntity)
        {
        }

        #endregion

        #region <Callbacks>

        protected override void OnUpdateAIState_TimeBlock()
        {
            if (Entity.TryGetCurrentEnemy(out var o_Enemy))
            {
                if (_OrderInterval < 1)
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
                    else
                    {
                        if (IsOrderEmpty)
                        {
                            ReserveMove(MindTool.MindOrderReserveType.Instant, new MindTool.MoveOrderParams(o_Enemy, GeometryTool.NavigationAttributeFlag.CalcRadius, AnimationTool.MoveMotionType.Run, p_MaxDistance: Entity.SightRange));
                        }
                    }
                    
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
                    Entity.UpdateEnemy();
                    if (Entity.TryGetCurrentEnemy(out var o_NextEnemy))
                    {
                        ClearOrderQueue(true);
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
    }
}