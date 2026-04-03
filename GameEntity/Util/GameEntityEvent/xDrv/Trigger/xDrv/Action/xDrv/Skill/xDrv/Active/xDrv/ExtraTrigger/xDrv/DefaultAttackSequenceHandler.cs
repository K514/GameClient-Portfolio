using UnityEngine;

namespace k514.Mono.Common
{
    public class DefaultAttackSequenceHandler : ExtraTriggerSkillEventHandlerBase<DefaultAttackSequenceHandler>
    {
        #region <Consts>

        private const float _HitBoxDuration = 0.3f;
        private const int _MaxAirborneCount = 3;
        
        static DefaultAttackSequenceHandler()
        {
            _ExtraMainTriggerTimeStampValidDuration = 0.3f;
        }

        #endregion

        #region <Fields>

        private EntityQueryTool.FilterSpaceConfig _FilterSpaceConfig;
        private int _AirborneCount;

        #endregion
        
        #region <Callbacks>

        public override void OnCreate(IObjectContent<ActionEventHandlerCreateParams> p_Wrapper, ActionEventHandlerCreateParams p_CreateParams)
        {
            base.OnCreate(p_Wrapper, p_CreateParams);

            ActionEventType = ActionEventTool.ActionEventType.SkillGroup0;
            InterruptableMask = ActionEventTool.ActionEventType.Jump; // 점프로 캔슬 가능
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
        
        public override void OnReachedGround()
        {
            _AirborneCount = 0;
            
            ActionModule.TryReleaseMainHandler(this);
        }
        
        protected override void OnProgressEventCue(int p_EventCueIndex)
        {
            Entity.TryRunFilterEvent(FilterEventTool.FilterEventType.EnemyFilter, new FilterEventHandlerActivateParams(Entity, _HitBoxDuration, _FilterSpaceConfig));
        }

        protected override void OnProgressSequence(int p_SequenceIndex)
        {
            if (_AirborneCount < _MaxAirborneCount)
            {
                if (Entity.IsFloat)
                {
                    _AirborneCount++;
                    Entity.AnimationModule.SwitchMotion(AnimationTool.MotionType.Punch, 9, AnimationTool.MotionTransitionType.Bypass_StateMachine);
                
                    var physicsModule = Entity.PhysicsModule;
                    physicsModule.SetAntiGravity(0.5f);
                    physicsModule.AddVelocity(PhysicsTool.ForceType.Default, 2f * Vector3.up);
                }
                else
                {
                    switch (p_SequenceIndex)
                    {
                        case 0 :
                            Entity.AnimationModule.SwitchMotion(AnimationTool.MotionType.Punch, 0, AnimationTool.MotionTransitionType.Bypass_StateMachine);
                            break;
                        case 1 :
                            Entity.AnimationModule.SwitchMotion(AnimationTool.MotionType.Punch, 1, AnimationTool.MotionTransitionType.Bypass_StateMachine);
                            break;
                        case 2 :
                            Entity.AnimationModule.SwitchMotion(AnimationTool.MotionType.Punch, 2, AnimationTool.MotionTransitionType.Bypass_StateMachine);
                            break;
                        default :
                            ActionModule.TryReleaseMainHandler(this);
                            break;
                    }
                }
            }
        }

        protected override void OnPressExtraMainTrigger()
        {
        }

        protected override void OnPressExtraJumpTrigger()
        {
        }

        #endregion
        
        #region <Methods>
        
        public override void PreloadEvent()
        {
        }
        
        protected override bool IsAvailableActivateSkill()
        {
            return Entity.HasState_Only(GameEntityTool.EntityStateType.AerialSkillPassMask)
                   && !Entity.PhysicsModule.HasJumpForceBeforeFloat();
        }
        
        #endregion
    }
}