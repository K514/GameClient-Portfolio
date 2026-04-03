namespace k514.Mono.Common
{
    public abstract partial class TriggerEventHandlerBase<This, Key, CreateParams, ActivateParams, TriggerRecord>
    {
        #region <Fields>

        private float _Cooldown;
        private ProgressTimerWrap _CooldownTimer;
        private bool _HasCooldown;
        private bool _IsCooldown;

        #endregion

        #region <Callbacks>

        private void OnCreateCooldown()
        {
            _Cooldown = Record.Cooldown;
            
            if (_Cooldown > 0f)
            {
                _HasCooldown = true;
                _CooldownTimer = new ProgressTimerWrap(_Cooldown);
                _CooldownTimer.Terminate();
            }
        }

        protected abstract void OnCooldownStart();
        protected abstract void OnCooldownProgress(float p_Rate);
        protected abstract void OnCooldownOver();
        
        #endregion

        #region <Methods>
        
        public bool IsCooldown()
        {
            return _HasCooldown && _IsCooldown;
        }
        
        public float GetCooldownDuration() => _HasCooldown ? _Cooldown : 0f;
        public float GetCooldownElapsed() => _HasCooldown ? _CooldownTimer.GetElapsedTime() : 0f;
        public float GetCooldownRemained() => _HasCooldown ? _CooldownTimer.GetRemaindTime() : 0f;
        public float GetCooldownProgressRate() => _HasCooldown ? _CooldownTimer.GetProgressRate() : 0f;

        public void SetCooldown(float p_Value)
        {
            if (_HasCooldown)
            {
                _Cooldown = p_Value;
            }
        }

        public void RunCooldown()
        {
            if (_HasCooldown)
            {
                var coolDownRate = Entity[StatusTool.BattleStatusGroupType.Total, BattleStatusTool.BattleStatusType.CooldownRate];
                _CooldownTimer.SetDuration(coolDownRate * _Cooldown, true);
                _CooldownTimer.Reset();

                if(_CooldownTimer.IsProgressing())
                {
                    _IsCooldown = true;

                    OnCooldownStart();
                }
                else
                {
                    _IsCooldown = false;
                    
                    OnCooldownStart();
                    OnCooldownOver();
                }
            }
        }

        public void ProgressCooldown(float p_DeltaTime)
        {
            if (IsCooldown())
            {
                _CooldownTimer.Progress(p_DeltaTime);

                if (_CooldownTimer.IsOver())
                {
                    _IsCooldown = false;
                    
                    OnCooldownOver();
                }
                else
                {
                    OnCooldownProgress(_CooldownTimer.GetProgressRate());
                }
            }
        }

        public void ResetCooldown(bool p_CastEventFlag)
        {
            if (IsCooldown())
            {
                _CooldownTimer.Terminate();
                _IsCooldown = false;

                if (p_CastEventFlag)
                {
                    OnCooldownOver();
                }
            }
            else
            {   
                if (p_CastEventFlag)
                {
                    OnCooldownStart();
                    OnCooldownOver();
                }
            }
        }

        #endregion
    }
}