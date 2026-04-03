namespace k514.Mono.Common
{
    public abstract partial class ActionEventHandlerBase<This>
    {
        public InputEventTool.TriggerKeyType TriggerKey { get; private set; }
        
        public void SetTriggerKey(InputEventTool.TriggerKeyType p_Type)
        {
            TriggerKey = p_Type;
        }
    }
}