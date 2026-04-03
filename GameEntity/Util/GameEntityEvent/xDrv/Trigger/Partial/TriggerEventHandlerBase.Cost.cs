namespace k514.Mono.Common
{
    public abstract partial class TriggerEventHandlerBase<This, Key, CreateParams, ActivateParams, TriggerRecord>
    {
        #region <Fields>

        private float _Cost;
        private bool _HasCost;

        #endregion

        #region <Callbacks>

        private void OnCreateCost()
        {
            _Cost = Record.Cost;
            if (_Cost > 0f)
            {
                _HasCost = true;
            }
        }

        #endregion

        #region <Methods>
        
        public bool HasEnoughCost()
        {
            return !_HasCost || Entity.HasManaEnough(_Cost);
        }
        
        public void PayCost()
        {
            if (_HasCost)
            {
                Entity.CostMana(_Cost);
            }
        }
                
        #endregion
    }
}