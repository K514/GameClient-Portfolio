namespace k514.Mono.Common
{
    public interface IGameEntityActionEventReceiver
    {
        void OnActionActivateSuccess(IActionEventHandler p_Handler);
        void OnActionActivateFail(IActionEventHandler p_Handler);
        void OnActionTerminated(IActionEventHandler p_Handler);
    }
}