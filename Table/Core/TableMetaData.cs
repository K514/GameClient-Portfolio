using System;

namespace k514
{
    [Serializable]
    public class TableMetaData : ICloneable
    {
        #region <Methods>

        public object Clone()
        {
            return MemberwiseClone();
        }

        #endregion
    }
}