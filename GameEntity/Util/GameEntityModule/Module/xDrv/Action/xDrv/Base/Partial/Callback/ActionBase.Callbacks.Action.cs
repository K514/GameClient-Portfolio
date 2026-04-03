namespace k514.Mono.Common
{
    public partial class ActionBase
    {
        private void OnActionAdded(IActionEventHandler p_Action)
        {
            p_Action.PreloadEvent();
            
            OnActionChanged();
        }
        
        private void OnActionChanged()
        {
            Entity.ReserveSkillChange();
        }
    }
}