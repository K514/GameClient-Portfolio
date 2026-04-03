namespace k514.Mono.Common
{
    public partial class MindBase
    {
        #region <Callbacks>

        private void OnAwakeIdleOrder()
        {
            ReserveIdle(MindTool.MindOrderReserveType.Instant);
        }

        #endregion
        
        #region <Methods>
        
        /// <summary>
        /// 이동 명령을 취소하는 메서드
        /// </summary>
        public bool ReserveIdle(MindTool.MindOrderReserveType p_Type, float p_PreDelay = 0f)
        {
            return ReserveIdle(p_Type, AnimationModule.GetIdleState(), p_PreDelay);
        }

        /// <summary>
        /// 이동 명령을 취소하는 메서드
        /// </summary>
        public bool ReserveIdle(MindTool.MindOrderReserveType p_Type, AnimationTool.IdleMotionType p_IdleType, float p_PreDelay = 0f)
        {
            return ReserveOrder(p_Type, new MindTool.MindOrderActivateParams(p_IdleType, p_PreDelay));
        }

        #endregion
    }
}