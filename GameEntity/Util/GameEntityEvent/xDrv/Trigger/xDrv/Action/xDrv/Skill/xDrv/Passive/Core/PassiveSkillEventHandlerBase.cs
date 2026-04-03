namespace k514.Mono.Common
{
    public interface IPassiveSkillEventHandler : ISkillEventHandler
    {
        IPassiveSkillDataTableRecordBridge Record { get; }
    }
    
    public abstract class PassiveSkillEventHandlerBase<This> : SkillEventHandlerBase<This>, IPassiveSkillEventHandler
        where This : PassiveSkillEventHandlerBase<This>, new()
    {
        #region <Fields>

        public new IPassiveSkillDataTableRecordBridge Record { get; private set; }

        #endregion

        #region <Callbacks>

        public override void OnCreate(IObjectContent<ActionEventHandlerCreateParams> p_Wrapper, ActionEventHandlerCreateParams p_CreateParams)
        {
            base.OnCreate(p_Wrapper, p_CreateParams);
            
            Record = base.Record as IPassiveSkillDataTableRecordBridge;
        }

        #endregion
    }
}