using UnityEngine;

namespace k514.Mono.Common
{
    public class RisingUpperHandler : DefaultSkillEventHandlerBase<RisingUpperHandler>
    {
        #region <Consts>

        private const float _HitBoxDuration = 0.3f;

        #endregion

        #region <Fields>

        private EntityQueryTool.FilterSpaceConfig _FilterSpaceConfig;

        #endregion
        
        #region <Callbacks>

        public override void OnCreate(IObjectContent<ActionEventHandlerCreateParams> p_Wrapper, ActionEventHandlerCreateParams p_CreateParams)
        {
            base.OnCreate(p_Wrapper, p_CreateParams);

            ActionEventType = ActionEventTool.ActionEventType.SkillGroup0;
            InterruptableMask = ActionEventTool.ActionEventType.None;
        }
        
        public override bool OnActivate(ActionEventHandlerCreateParams p_CreateParams, ActionEventHandlerActivateParams p_ActivateParams, bool p_IsPooled)
        {
            if (base.OnActivate(p_CreateParams, p_ActivateParams, p_IsPooled))
            {
                _FilterSpaceConfig = 
                    new EntityQueryTool.FilterSpaceConfig
                    (
                        EntityQueryTool.FilterSpaceType.Box, 
                        Entity, 1f, 2f,
                        p_FilterParamsFlag: EntityQueryTool.FilterSpaceAttributeFlag.ForwardCast
                    );
                
                return true;
            }
            else
            {
                return false;
            }
        }
        
        public override void OnInterruptSuccess()
        {
            base.OnInterruptSuccess();

            Entity.AnimationModule.SwitchMotion(AnimationTool.MotionType.Kick, 1, AnimationTool.MotionTransitionType.Bypass_StateMachine);
        }
        
        protected override void OnProgressEventCue(int p_EventCueIndex)
        {
            switch (p_EventCueIndex)
            {
                case 0:
                    Entity.TryRunFilterEvent(FilterEventTool.FilterEventType.EnemyUpperFilter, new FilterEventHandlerActivateParams(Entity, _HitBoxDuration, _FilterSpaceConfig));
                    break;
            }
        }
        
        #endregion
        
        #region <Methods>

        public override void PreloadEvent()
        {
        }

        #endregion
    }
}