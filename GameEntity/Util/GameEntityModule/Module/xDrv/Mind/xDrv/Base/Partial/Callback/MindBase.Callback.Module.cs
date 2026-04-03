using UnityEngine;

namespace k514.Mono.Common
{
    public partial class MindBase
    {
        public override void OnNavigateEnd(GeometryTool.NavigationResultPreset p_Preset)
        {
            base.OnNavigateEnd(p_Preset);

            if (Entity.IsDrivingOrder)
            {
                if (_CurrentOrder?.IsOrderType(MindTool.MindOrderType.Navigate) ?? false)
                {
                    var navigateResult = p_Preset.NavigateResultType;
                    switch (navigateResult)
                    {
                        case GeometryTool.NavigateResultType.Reached:
                        {
                            _CurrentOrder.Terminate(true);
                            break;
                        }
                        case GeometryTool.NavigateResultType.None:
                        case GeometryTool.NavigateResultType.OutOfRange:
                        case GeometryTool.NavigateResultType.Stuck:
                        case GeometryTool.NavigateResultType.Jitter:
                        case GeometryTool.NavigateResultType.Canceled:
                        {
                            _CurrentOrder.Terminate(false);
                            Entity.ClearEnemy();
                            break;
                        }
                    }
                }
            }
        }
        
        public override void OnActionActivateSuccess(IActionEventHandler p_Handler)
        {
            base.OnActionActivateSuccess(p_Handler);
            
            if (Entity.IsDrivingOrder)
            {
                if (_CurrentOrder?.IsOrderType(MindTool.MindOrderType.InputCommand) ?? false)
                {
                    p_Handler.OnMindControl();
                }
            }
        }
        
        public override void OnActionActivateFail(IActionEventHandler p_Handler)
        {
            base.OnActionActivateFail(p_Handler);
            
            if (Entity.IsDrivingOrder)
            {
                if (_CurrentOrder?.IsOrderType(MindTool.MindOrderType.InputCommand) ?? false)
                {
                    if (_CurrentOrder.MindOrderParams.ReservedInputCommand == p_Handler.TriggerKey)
                    {
                        _CurrentOrder.Terminate(false);
                    }
                }
            }
        }
        
        public override void OnActionTerminated(IActionEventHandler p_Handler)
        {
            base.OnActionTerminated(p_Handler);
            
            if (Entity.IsDrivingOrder)
            {
                if (_CurrentOrder?.IsOrderType(MindTool.MindOrderType.InputCommand) ?? false)
                {
                    if (_CurrentOrder.MindOrderParams.ReservedInputCommand == p_Handler.TriggerKey)
                    {
                        _CurrentOrder.Terminate(true);
                    }
                }
            }
        }
    }
}