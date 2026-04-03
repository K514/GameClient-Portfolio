using UnityEngine;

namespace k514.Mono.Common
{
    public partial class ActionBase
    {
        #region <Fields>
        
        /// <summary>
        /// 현재 누적된 점프 횟수
        /// </summary>
        protected int CurrentJumpCount;
        
        #endregion

        #region <Callbacks>

        public override void OnModule_ManualJump()
        {
            CurrentJumpCount++;
        }

        public override void OnModule_ReachedGround(PhysicsTool.StampPreset p_UnitStampPreset)
        {
            CurrentJumpCount = 0;

            _MainActionEventHandler?.OnReachedGround();
        }
        
        #endregion
        
        #region <Methods>

        public bool IsAvailableManualJump() => Entity[StatusTool.BattleStatusGroupType.Total][BattleStatusTool.BattleStatusType3.JumpCount] > CurrentJumpCount;
        public bool IsManualJumped() => CurrentJumpCount > 0;
            
        #endregion
    }
}