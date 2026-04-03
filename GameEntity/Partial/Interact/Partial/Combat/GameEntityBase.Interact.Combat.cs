using System;
using UnityEngine;

namespace k514.Mono.Common
{
    public partial class GameEntityBase<Content, CreateParams, ActivateParams>
    {
        #region <Callbacks>
        
        private void OnHeal(StatusTool.StatusChangeParams p_Params)
        {
            SetMaterialFlash();
            
            var trigger = p_Params.Trigger;
            GameEntityBaseEventSender.SendEvent(GameEntityTool.GameEntityBaseEventType.Heal, new GameEntityBaseEventParams(trigger, this, p_Params));
        }
        
        private void OnHit(StatusTool.StatusChangeParams p_Params)
        {
            SetMaterialFlash();
            
            var trigger = p_Params.Trigger;
            GameEntityBaseEventSender.SendEvent(GameEntityTool.GameEntityBaseEventType.Hit, new GameEntityBaseEventParams(trigger, this, p_Params));

            if (p_Params.HasAttribute(StatusTool.StatusChangeAttribute.HasTrigger))
            {
                if (trigger.IsEntityValid())
                {
                    trigger.OnStrikeEntity(this, p_Params);
                }
            }
        }

        public virtual void OnStrikeEntity(IGameEntityBridge p_Entity, StatusTool.StatusChangeParams p_Params)
        {
            GameEntityBaseEventSender.SendEvent(GameEntityTool.GameEntityBaseEventType.Strike, new GameEntityBaseEventParams(this, p_Entity, p_Params));
        }

        private void OnMurdered(StatusTool.StatusChangeParams p_Params)
        {
            if (IsAlive)
            {
                SetDead(false);

                if (p_Params.HasAttribute(StatusTool.StatusChangeAttribute.HasTrigger))
                {
                    var trigger = p_Params.Trigger;
                    if (trigger.IsEntityValid())
                    {
                        trigger.OnKilledEntity(this, p_Params);
                    }
                }
            }
        }

        public void OnKilledEntity(IGameEntityBridge p_Entity, StatusTool.StatusChangeParams p_Params)
        {
            Absorb();
            
            GameEntityBaseEventSender.SendEvent(GameEntityTool.GameEntityBaseEventType.Kill, new GameEntityBaseEventParams(this, p_Entity, p_Params));
        }

        #endregion
    }
}