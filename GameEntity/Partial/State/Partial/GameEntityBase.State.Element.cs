namespace k514.Mono.Common
{
    public partial class GameEntityBase<Content, CreateParams, ActivateParams>
    {
        #region <Field>

        /// <summary>
        /// 속성 타입
        /// </summary>
        private GameEntityTool.ElementType _ElementMask;

        #endregion
        
        #region <Callbacks>

        private void OnCreateElement()
        {
        }

        private void OnActivateElement()
        {
            _ElementMask = default;
        }
        
        private void OnRetrieveElement()
        {
        }

        #endregion

        #region <Methods>

        public void AddElement(GameEntityTool.ElementType p_Type)
        {
            _ElementMask.AddFlag(p_Type);
        }

        public void RemoveElement(GameEntityTool.ElementType p_Type)
        {
            _ElementMask.RemoveFlag(p_Type);
        }
        
        public void TurnElement(GameEntityTool.ElementType p_Type)
        {
            _ElementMask.TurnFlag(p_Type);
        }

        public void ClearElement()
        {
            TurnElement(GameEntityTool.ElementType.None);
        }

        public bool HasElement(GameEntityTool.ElementType p_Type)
        {
            return _ElementMask.HasAnyFlagExceptNone(p_Type);
        }

        public GameEntityTool.ElementType GetElement()
        {
            return _ElementMask;
        }

        #endregion
    }
}