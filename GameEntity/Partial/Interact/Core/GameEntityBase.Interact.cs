using System.Collections.Generic;

namespace k514.Mono.Common
{
    public partial class GameEntityBase<Content, CreateParams, ActivateParams>
    {
        #region <Callbacks>

        private void OnCreateInteract()
        {
            OnCreateInteractManager();
            OnCreateInteractRelation();
        }

        private bool OnActivateInteract(ActivateParams p_ActivateParams)
        {
            if (OnActivateInteractManager())
            {
                OnActivateInteractRelation(p_ActivateParams);
                
                return true;
            }
            else
            {
                return false;
            }
        }

        private void OnRetrieveInteract()
        {
            OnRetrieveInteractManager();
            OnRetrieveInteractRelation();
        }

        private void OnDisposeInteract()
        {
            OnDisposeInteractRelation();
        }
        
        #endregion
    }
}