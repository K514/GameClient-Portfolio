namespace k514.Mono.Common
{
    public abstract class DefaultPunchHandler<This> : DefaultSkillEventHandlerBase<This>
        where This : DefaultPunchHandler<This>, new()
    {
        #region <Consts>

        private const float _HitBoxDuration = 0.3f;

        #endregion
        
        #region <Fields>

        private EntityQueryTool.FilterSpaceConfig _FilterSpaceConfig;
        protected int _PunchMotionIndex;

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

            Entity.AnimationModule.SwitchMotion(AnimationTool.MotionType.Punch, _PunchMotionIndex, AnimationTool.MotionTransitionType.Bypass_StateMachine);
        }
        
        protected override void OnProgressEventCue(int p_EventCueIndex)
        {
            switch (p_EventCueIndex)
            {
                case 0:
                    Entity.TryRunFilterEvent(FilterEventTool.FilterEventType.EnemyFilter, new FilterEventHandlerActivateParams(Entity, _HitBoxDuration, _FilterSpaceConfig));
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
    
    public class DefaultPunch0Handler : DefaultPunchHandler<DefaultPunch0Handler>
    {
        public override void OnCreate(IObjectContent<ActionEventHandlerCreateParams> p_Wrapper, ActionEventHandlerCreateParams p_CreateParams)
        {
            base.OnCreate(p_Wrapper, p_CreateParams);

            _PunchMotionIndex = 0;
        }
    }
    
    public class DefaultPunch1Handler : DefaultPunchHandler<DefaultPunch1Handler>
    {
        public override void OnCreate(IObjectContent<ActionEventHandlerCreateParams> p_Wrapper, ActionEventHandlerCreateParams p_CreateParams)
        {
            base.OnCreate(p_Wrapper, p_CreateParams);

            _PunchMotionIndex = 1;
        }
    }
    
    public class DefaultPunch2Handler : DefaultPunchHandler<DefaultPunch2Handler>
    {
        public override void OnCreate(IObjectContent<ActionEventHandlerCreateParams> p_Wrapper, ActionEventHandlerCreateParams p_CreateParams)
        {
            base.OnCreate(p_Wrapper, p_CreateParams);

            _PunchMotionIndex = 2;
        }
    }
    
    public class DefaultPunch3Handler : DefaultPunchHandler<DefaultPunch3Handler>
    {
        public override void OnCreate(IObjectContent<ActionEventHandlerCreateParams> p_Wrapper, ActionEventHandlerCreateParams p_CreateParams)
        {
            base.OnCreate(p_Wrapper, p_CreateParams);

            _PunchMotionIndex = 3;
        }
    }
    
    public class DefaultPunch4Handler : DefaultPunchHandler<DefaultPunch4Handler>
    {
        public override void OnCreate(IObjectContent<ActionEventHandlerCreateParams> p_Wrapper, ActionEventHandlerCreateParams p_CreateParams)
        {
            base.OnCreate(p_Wrapper, p_CreateParams);

            _PunchMotionIndex = 4;
        }
    }
    
    public class DefaultPunch5Handler : DefaultPunchHandler<DefaultPunch5Handler>
    {
        public override void OnCreate(IObjectContent<ActionEventHandlerCreateParams> p_Wrapper, ActionEventHandlerCreateParams p_CreateParams)
        {
            base.OnCreate(p_Wrapper, p_CreateParams);

            _PunchMotionIndex = 5;
        }
    }
    
    public class DefaultPunch6Handler : DefaultPunchHandler<DefaultPunch6Handler>
    {
        public override void OnCreate(IObjectContent<ActionEventHandlerCreateParams> p_Wrapper, ActionEventHandlerCreateParams p_CreateParams)
        {
            base.OnCreate(p_Wrapper, p_CreateParams);

            _PunchMotionIndex = 6;
        }
    }
    
    public class DefaultPunch7Handler : DefaultPunchHandler<DefaultPunch7Handler>
    {
        public override void OnCreate(IObjectContent<ActionEventHandlerCreateParams> p_Wrapper, ActionEventHandlerCreateParams p_CreateParams)
        {
            base.OnCreate(p_Wrapper, p_CreateParams);

            _PunchMotionIndex = 7;
        }
    }
    
    public class DefaultPunch8Handler : DefaultPunchHandler<DefaultPunch8Handler>
    {
        public override void OnCreate(IObjectContent<ActionEventHandlerCreateParams> p_Wrapper, ActionEventHandlerCreateParams p_CreateParams)
        {
            base.OnCreate(p_Wrapper, p_CreateParams);

            _PunchMotionIndex = 8;
        }
    }
    
    public class DefaultPunch9Handler : DefaultPunchHandler<DefaultPunch9Handler>
    {
        public override void OnCreate(IObjectContent<ActionEventHandlerCreateParams> p_Wrapper, ActionEventHandlerCreateParams p_CreateParams)
        {
            base.OnCreate(p_Wrapper, p_CreateParams);

            _PunchMotionIndex = 9;
        }
    }
    
    public class DefaultPunch10Handler : DefaultPunchHandler<DefaultPunch10Handler>
    {
        public override void OnCreate(IObjectContent<ActionEventHandlerCreateParams> p_Wrapper, ActionEventHandlerCreateParams p_CreateParams)
        {
            base.OnCreate(p_Wrapper, p_CreateParams);

            _PunchMotionIndex = 10;
        }
    }
    
    public class DefaultPunch11Handler : DefaultPunchHandler<DefaultPunch11Handler>
    {
        public override void OnCreate(IObjectContent<ActionEventHandlerCreateParams> p_Wrapper, ActionEventHandlerCreateParams p_CreateParams)
        {
            base.OnCreate(p_Wrapper, p_CreateParams);

            _PunchMotionIndex = 11;
        }
    }
    
    public class DefaultPunch12Handler : DefaultPunchHandler<DefaultPunch12Handler>
    {
        public override void OnCreate(IObjectContent<ActionEventHandlerCreateParams> p_Wrapper, ActionEventHandlerCreateParams p_CreateParams)
        {
            base.OnCreate(p_Wrapper, p_CreateParams);

            _PunchMotionIndex = 12;
        }
    }
}