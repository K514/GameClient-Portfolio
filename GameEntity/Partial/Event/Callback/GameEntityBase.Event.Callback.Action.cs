namespace k514.Mono.Common
{
    public partial class GameEntityBase<Content, CreateParams, ActivateParams>
    {
        public void OnActionActivateSuccess(IActionEventHandler p_Handler)
        {
            OnModule_ActionActivateSuccess(p_Handler);
        }
        
        public void OnActionActivateFail(IActionEventHandler p_Handler)
        {
            OnModule_ActionActivateFail(p_Handler);
        }

        public void OnActionTerminated(IActionEventHandler p_Handler)
        {
            OnModule_ActionTerminated(p_Handler);
        }
    }
}