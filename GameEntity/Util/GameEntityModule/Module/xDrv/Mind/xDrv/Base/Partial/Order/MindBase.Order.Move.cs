namespace k514.Mono.Common
{
    public partial class MindBase
    {
        #region <Methods>

        /// <summary>
        /// 지정한 목적지로 이동 명령을 내리는 메서드
        /// </summary>
        public bool ReserveMove(MindTool.MindOrderReserveType p_Type, MindTool.MoveOrderParams p_Params, float p_PreDelay = 0f)
        {
            return ReserveOrder(p_Type, new MindTool.MindOrderActivateParams(p_Params, p_PreDelay));
        }

        #endregion
    }
}