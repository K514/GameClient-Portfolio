using UnityEngine;

namespace k514.Mono.Common
{
    public partial class MindBase
    {
        #region <Methods>

        /// <summary>
        /// Pivot Position으로 이동 명령하는 메서드
        /// </summary>
        public bool ReserveReturnHome(MindTool.MindOrderReserveType p_Type, float p_PreDelay = 0f)
        {
            var pivotPosition = Entity.GetPivotPosition();
            return ReserveMove(p_Type, new MindTool.MoveOrderParams(pivotPosition, GeometryTool.NavigationAttributeFlag.ForceSurface), p_PreDelay);
        }

        /// <summary>
        /// 지정한 개체로부터 반대 방향으로 일정 거리를 이동시키는 메서드
        /// </summary>
        public bool ReserveDrawBackFrom(MindTool.MindOrderReserveType p_Type, IGameEntityBridge p_FromEntity, float p_PreDelay = 0f)
        {
            var runDV = p_FromEntity.GetDirectionUnitVectorTo(Entity).VectorRotationUsingQuaternion(Vector3.up, Random.Range(-30f, 30f));
            var runDist = Entity.GetRadius() * Random.Range(4f, 6f);
            var runPos = Entity.Affine.position + runDist * runDV;

            return ReserveMove(p_Type, new MindTool.MoveOrderParams(runPos, GeometryTool.NavigationAttributeFlag.ForceSurface), p_PreDelay);
        }

        /// <summary>
        /// 지정한 입력커맨드를 액션 모듈에 입력시키는 메서드
        /// </summary>
        public bool ReserveInputCommand(MindTool.MindOrderReserveType p_Type, InputEventTool.TriggerKeyType p_CommandType, float p_PreDelay = 0f)
        {
            return ReserveOrder(p_Type, new MindTool.MindOrderActivateParams(p_CommandType, p_PreDelay));
        }

        /// <summary>
        /// 대기 명령을 예약하는 메서드
        /// </summary>
        public bool ReserveDelay(MindTool.MindOrderReserveType p_Type, float p_PreDelay = 0f)
        {
            return ReserveOrder(p_Type, new MindTool.MindOrderActivateParams(MindTool.MindOrderType.Delay, p_PreDelay));
        }
        
        /// <summary>
        /// 동작 딜레이를 즉시 0으로 하는 명령을 예약하는 메서드
        /// </summary>
        public bool ReserveFreeDelay(MindTool.MindOrderReserveType p_Type, float p_PreDelay = 0f)
        {
            return ReserveOrder(p_Type, new MindTool.MindOrderActivateParams(MindTool.MindOrderType.FreeDelay, p_PreDelay));
        }
        
        /// <summary>
        /// 해당 모듈의 개체를 비활성화 시키는 메서드
        /// </summary>
        public bool ReserveDisable(MindTool.MindOrderReserveType p_Type, float p_PreDelay = 0)
        {
            return ReserveOrder(p_Type, new MindTool.MindOrderActivateParams(MindTool.MindOrderType.Disable, p_PreDelay));
        }

        #endregion
    }
}