using k514.Mono.Feature;
using UnityEngine;

namespace k514.Mono.Common
{
    /// <summary>
    /// 유닛의 사고 법칙을 기술하는 인터페이스
    /// </summary>
    public interface IMindModule : IGameEntityModule
    {
        /* Default */
        MindModuleDataTableQuery.TableLabel GetMindModuleType();
        
        /* Order */
        void ResetOrderInterval();
        void ClearOrderQueue(bool p_CancelCurrentOrder);
    
        /* Reserve */
        bool ReserveIdle(MindTool.MindOrderReserveType p_Type, float p_PreDelay = 0f);
        bool ReserveIdle(MindTool.MindOrderReserveType p_Type, AnimationTool.IdleMotionType p_IdleType, float p_PreDelay = 0f);
        bool ReserveMove(MindTool.MindOrderReserveType p_Type, MindTool.MoveOrderParams p_Params, float p_PreDelay = 0f);
        bool ReserveReturnHome(MindTool.MindOrderReserveType p_Type, float p_PreDelay = 0f);
        bool ReserveDrawBackFrom(MindTool.MindOrderReserveType p_Type, IGameEntityBridge p_FromEntity, float p_PreDelay = 0f);
        bool ReserveInputCommand(MindTool.MindOrderReserveType p_Type, InputEventTool.TriggerKeyType p_CommandType, float p_PreDelay = 0f);
        bool ReserveDisable(MindTool.MindOrderReserveType p_Type, float p_PreDelay = 0f);
    }
}