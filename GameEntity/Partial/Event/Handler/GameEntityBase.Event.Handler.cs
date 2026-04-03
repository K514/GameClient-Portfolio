namespace k514.Mono.Common
{
    public partial class GameEntityBase<Content, CreateParams, ActivateParams>
    {
        #region <Callbacks>

        private void OnCreateEventHandler()
        {
            OnCreateAffineEventHandler();
            OnCreateInstanceEventHandler();
            OnCreateFilterEventHandler();
            OnCreateEnchantEventHandler();
            OnCreateItemEventHandler();
        }

        private void OnActivateEventHandler(ActivateParams p_ActivateParams)
        {
            OnActivateAffineEventHandler(p_ActivateParams);
            OnActivateInstanceEventHandler(p_ActivateParams);
            OnActivateFilterEventHandler(p_ActivateParams);
            OnActivateEnchantEventHandler(p_ActivateParams);
            OnActivateItemEventHandler(p_ActivateParams);
        }
        
        private void OnUpdateEventHandler(float p_DeltaTime)
        {
            OnUpdateAffineEventHandler(p_DeltaTime);
            OnUpdateInstanceEventHandler(p_DeltaTime);
            OnUpdateFilterEventHandler(p_DeltaTime);
            OnUpdateEnchantEventHandler(p_DeltaTime);
            OnUpdateItemEventHandler(p_DeltaTime);
        }
        
        private void OnRetrieveEventHandler()
        {
            OnRetrieveItemEventHandler();
            OnRetrieveEnchantEventHandler();
            OnRetrieveFilterEventHandler();
            OnRetrieveInstanceEventHandler();
            OnRetrieveAffineEventHandler();
        }

        #endregion

        #region <Methods>

        public void TerminateAllEventHandler()
        {
            TerminateAllAffineHandler();
            TerminateAllInstanceHandler();
            TerminateAllFilterHandler();
            TerminateAllEnchantHandler();
            TerminateAllItemHandler();
        }

        #endregion
    }
}
