namespace k514.Mono.Common
{
    public partial class GameEntityAffineControlStorage 
    {
        #region <Callbacks>

        private void OnCreateCommon()
        {
            _EntityEventTable.Add(0, new NoneControl());
        }

        #endregion

        #region <Classess>

        public class NoneControl : GameEntityAffineControlEventBase
        {
            public override bool InitializeAffineControl(GameEntityAffineControlEventContainer p_Handler)
            {
                return false;
            }

            public override bool UpdateAffineControl(GameEntityAffineControlEventContainer p_Handler, float p_DeltaTime)
            {
                return true;
            }
        }
        #endregion
    }
}