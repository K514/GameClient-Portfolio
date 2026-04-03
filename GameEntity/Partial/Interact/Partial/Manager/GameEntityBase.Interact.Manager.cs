using System.Collections.Generic;

namespace k514.Mono.Common
{
    public partial class GameEntityBase<Content, CreateParams, ActivateParams>
    {
        #region <Fields>

        /// <summary>
        /// 관리 키
        /// </summary>
        public EntityLocalId LocalId { get; private set; }
        
        /// <summary>
        /// Entity 쿼리 결과 리스트
        /// </summary>
        private List<IGameEntityBridge> _FilterResultGroup;

        #endregion

        #region <Callbacks>

        private void OnCreateInteractManager()
        {
            _FilterResultGroup = new List<IGameEntityBridge>();
            ResetLocalId();
        }

        private bool OnActivateInteractManager()
        {
            return InteractManager.GetInstanceUnsafe.AddEntity(this);
        }

        private void OnRetrieveInteractManager()
        {
            InteractManager.GetInstanceUnsafe.RemoveEntity(this);
            _FilterResultGroup.Clear();
        }

        public void OnInteractSceneStart()
        {
            SetDisable(false);
        }
        
        public void OnInteractSceneTransition()
        {
            SetDisable(true);
        }
        
        #endregion

        #region <Methods>

        public void SetLocalId(EntityLocalId p_LocalId)
        {
            LocalId = p_LocalId;
        }

        public void ResetLocalId()
        {
            LocalId = new EntityLocalId(-1);
        }
        
        #endregion
    }
}