using UnityEngine;

namespace k514.Mono.Common
{
    public partial class MindBase
    {
        #region <Fields>
        
        /// <summary>
        /// 공격 간격
        /// </summary>
        protected int _OrderInterval;
        
        #endregion
        
        #region <Methods>

        protected virtual void InitOrderInterval()
        {
            _OrderInterval = 1;
        }
        
        protected virtual void UpdateOrderInterval()
        {
            UpdateOrderInterval(1);
        }
        
        protected void UpdateOrderInterval(int p_Delay)
        {
            _OrderInterval = p_Delay;
        }
        
        protected virtual void ProgressOrderInterval()
        {
            _OrderInterval --;
        }

        public void ResetOrderInterval()
        {
            _OrderInterval = 0;
        }
        
        #endregion
    }
}