using UnityEngine;

namespace k514.Mono.Common
{
    public partial class GameEntityBase<Content, CreateParams, ActivateParams>
    {
        protected override void OnLiveSpanOver()
        {
            SetDead(false);
        }
        
        protected override void OnDeadSpanStarted()
        {
            TryRunReservedInstanceEvent();
        }
        
        protected override void OnDeadSpanOver()
        {
            if (IsUnitEntity)
            {
                TriggerDeadFadeOut();
            }
            else
            {
                SetLifeSpanPhase(GameEntityTool.EntityLifeSpanPhase.LifeSpanTerminate);
            }
        }
        
        protected override void OnLifeSpanTerminated()
        {
            ReservePooling();
        }
    }
}