namespace k514.Mono.Common
{
    public partial class GameEntityBase<Content, CreateParams, ActivateParams>
    {
        #region <Field>

        /// <summary>
        /// 재질 타입
        /// </summary>
        private GameEntityTool.MaterialType _MaterialType;

        #endregion
        
        #region <Callbacks>

        private void OnCreateMaterial()
        {
        }

        private void OnActivateMaterial()
        {
            _MaterialType = default;
        }
        
        private void OnRetrieveMaterial()
        {
        }

        #endregion

        #region <Methods>

        public void SetMaterialType(GameEntityTool.MaterialType p_Type)
        {
            _MaterialType = p_Type;
        }
        
        public GameEntityTool.MaterialType GetMaterialType()
        {
            return _MaterialType;
        }

        #endregion
    }
}