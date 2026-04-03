using Unity.Entities;
using UnityEngine;

namespace k514.Mono.Common
{
    public class DefaultInteractionEventHandler : InteractEventHandlerBase<DefaultInteractionEventHandler>
    {
        #region <Callbacks>

        public override void OnCreate(IObjectContent<ActionEventHandlerCreateParams> p_Wrapper, ActionEventHandlerCreateParams p_CreateParams)
        {
            base.OnCreate(p_Wrapper, p_CreateParams);

            ActionEventType = ActionEventTool.ActionEventType.None;
        }
        
        public override void OnInterruptSuccess()
        {
            base.OnInterruptSuccess();
            
            ActionModule.TryReleaseMainHandler(this);

            Test02();
        }

        public override void OnInterrupted()
        {
            base.OnInterrupted();
        }
        
        protected override void OnPress(CommandEventParams p_CommandPreset)
        {
            ActionModule.TryInterruptMainHandler(this);
        }

        protected override  void OnHolding(CommandEventParams p_CommandPreset)
        {
        }

        protected override  void OnRelease(CommandEventParams p_CommandPreset)
        {
        }

        #endregion

        #region <Methods>

        private void Test01()
        {
            var geo = Entity.GeometryModule;
            var navPreset = new GeometryTool.NavigateDestinationPreset(Entity.GetBottomPosition() + 5f * Vector3.forward, GeometryTool.NavigationAttributeFlag.ForceSurface);
            var result = geo.NavigateTo(navPreset);
        }

        private void Test02()
        {
            Entity.TryRunAffineEvent(AffineEventTool.AffineEventType.Forward, new AffineEventHandlerActivateParams(Entity, 2f, 100f * Entity.GetLookUV()));
        }
        
        protected override bool IsManualReleasable()
        {
            return false;
        }

        public override void PreloadEvent()
        {
        }
        
        #endregion
    }
}