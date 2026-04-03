using UnityEngine;

namespace k514.Mono.Common
{
    public abstract partial class TriggerEventHandlerBase<This, Key, CreateParams, ActivateParams, TriggerRecord>
    {
        #region <Fields>

        private int _Level;
        public int Level => Mathf.Min(_Level, Record.LevelUpperBound);
        public bool IsMasterLevel => Level == Record.LevelUpperBound;

        #endregion

        #region <Callbacks>

        protected abstract void OnActionLevelChanged(int p_Prev, int p_Cur);
        
        #endregion

        #region <Methods>

        public bool AddLevel(int p_Value)
        {
            var prev = _Level;
            _Level = Mathf.Clamp(_Level + p_Value, 1, Record.LevelUpperBound);

            if (prev != _Level)
            {
                OnActionLevelChanged(prev, _Level);

                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion
    }
}