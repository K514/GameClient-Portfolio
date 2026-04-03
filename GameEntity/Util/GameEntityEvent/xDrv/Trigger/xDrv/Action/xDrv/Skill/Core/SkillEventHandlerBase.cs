using UnityEngine;

namespace k514.Mono.Common
{
    public interface ISkillEventHandler : IActionEventHandler
    {
    }
    
    public abstract class SkillEventHandlerBase<This> : ActionEventHandlerBase<This>, ISkillEventHandler
        where This : SkillEventHandlerBase<This>, new()
    {
        #region <Fields>

        protected new ISkillDataTableRecordBridge Record { get; private set; }

        #endregion

        #region <Callbacks>

        public override void OnCreate(IObjectContent<ActionEventHandlerCreateParams> p_Wrapper, ActionEventHandlerCreateParams p_CreateParams)
        {
            base.OnCreate(p_Wrapper, p_CreateParams);
            
            Record = base.Record as ISkillDataTableRecordBridge;
        }
        
        public override void OnInterruptSuccess()
        {
            base.OnInterruptSuccess();

            Entity.AddState(GameEntityTool.EntityStateType.DRIVE_SKILL);
        }

        public override void OnInterrupted()
        {
            base.OnInterrupted();
            
            Entity.RemoveState(GameEntityTool.EntityStateType.DRIVE_SKILL);
        }

        protected override void OnActionLevelChanged(int p_Prev, int p_Cur)
        {
        }
        
        #endregion
        
        #region <Methods>
        
        public override bool IsEnterableExceptCooldown()
        {
            return base.IsEnterableExceptCooldown() && IsAvailableActivateSkill();
        }

        protected virtual bool IsAvailableActivateSkill()
        {
            return Entity.HasState_Only(GameEntityTool.EntityStateType.SkillPassMask)
                   && !Entity.PhysicsModule.HasJumpForceBeforeFloat();
        }
        
        #endregion
    }
}