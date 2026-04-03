namespace k514.Mono.Common
{
    public interface IActiveSkillEventHandler : ISkillEventHandler
    {
        IActiveSkillDataTableRecordBridge Record { get; }
    }
    
    public abstract class ActiveSkillEventHandlerBase<This> : SkillEventHandlerBase<This>, IActiveSkillEventHandler
        where This : ActiveSkillEventHandlerBase<This>, new()
    {
        #region <Fields>

        public new IActiveSkillDataTableRecordBridge Record { get; private set; }

        #endregion

        #region <Callbacks>

        public override void OnCreate(IObjectContent<ActionEventHandlerCreateParams> p_Wrapper, ActionEventHandlerCreateParams p_CreateParams)
        {
            base.OnCreate(p_Wrapper, p_CreateParams);
            
            Record = base.Record as IActiveSkillDataTableRecordBridge;
        }

        #endregion
    }
}