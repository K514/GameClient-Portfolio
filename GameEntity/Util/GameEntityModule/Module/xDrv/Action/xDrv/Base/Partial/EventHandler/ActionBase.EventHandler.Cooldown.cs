namespace k514.Mono.Common
{
    public partial class ActionBase
    {
        #region <Methods>

        /// <summary>
        /// 모든 액션의 쿨타임을 풀로 채운다.
        /// </summary>
        public void RunCooldown()
        {
            foreach (var handlerListKV in _ActionEventHandlerListTableByLabel)
            {
                var handlerList = handlerListKV.Value;
                foreach (var handler in handlerList)
                {
                    handler.RunCooldown();
                }
            }
        }

        /// <summary>
        /// 모든 액션의 쿨타임을 진행시킨다.
        /// </summary>
        public void ProgressCooldown(float p_DeltaTime)
        {
            var coolDownSpeedRate = Entity[StatusTool.BattleStatusGroupType.Total, BattleStatusTool.BattleStatusType.CooldownRecoverySpeedRate];
            var scaledDeltaTime = p_DeltaTime * coolDownSpeedRate;

            foreach (var handlerListKV in _ActionEventHandlerListTableByLabel)
            {
                var handlerList = handlerListKV.Value;
                foreach (var handler in handlerList)
                {
                    handler.ProgressCooldown(scaledDeltaTime);
                }
            }
        }
        
        /// <summary>
        /// 모든 액션의 쿨타임을 0으로 한다.
        /// </summary>
        public void ResetCooldown()
        {
            foreach (var handlerListKV in _ActionEventHandlerListTableByLabel)
            {
                var handlerList = handlerListKV.Value;
                foreach (var handler in handlerList)
                {
                    handler.ResetCooldown(true);
                }
            }
        }

        #endregion
    }
}