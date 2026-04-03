namespace k514.Mono.Common
{
    public partial class GameEntityBase<Content, CreateParams, ActivateParams>
    {
        #region <Fields>

        /// <summary>
        /// 현재 경험치 값
        /// </summary>
        private float _CurrentExp;

        #endregion
        
        #region <Callbacks>

        private void OnCreateExp()
        {
        }
        
        private void OnActivateExp()
        {
            SetExp(0f);
        }
        
        private void OnRetrieveExp()
        {
        }

        private void OnExpChanged()
        {
            var neededExp = GetNeededExp();
            if (_CurrentExp >= neededExp)
            {
                AddLevel(1);

                if (IsLevelCounterStop())
                {
                    SetExp(0f);
                }
                else
                {
                    AddExp(-neededExp);
                }
            }
        }

        #endregion
        
        #region <Methods>

        public float GetNeededExp()
        {
            return _EnhanceRecord.GetNeededExp(_CurrentLevel);
        }
        
        public float GetNeededExpInv()
        {
            return _EnhanceRecord.GetNeededExpInv(_CurrentLevel);
        }
        
        public float GetCurrentExp()
        {
            return _CurrentExp;
        }
        
        public float GetCurrentExpRate()
        {
            return _CurrentExp * GetNeededExpInv();
        }
        
        public void SetExp(float p_Exp)
        {
            _CurrentExp = p_Exp;
            
            OnExpChanged();
        }
        
        public void AddExp(float p_Exp)
        {
            _CurrentExp += p_Exp;
            
            OnExpChanged();
        }

        public void SetExpRate(float p_Rate)
        {
            AddExp((int)(GetNeededExp() * p_Rate));
        }

        public void AddExpRate(float p_Rate)
        {
            AddExp((int)(GetNeededExp() * p_Rate));
        }

        #endregion
    }
}