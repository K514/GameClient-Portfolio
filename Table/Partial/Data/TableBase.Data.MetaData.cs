namespace k514
{
    public partial class TableBase<Table, Meta, Key, Record>
    {
        #region <Fields>

        /// <summary>
        /// 테이블 메타데이터
        /// </summary>
        protected Meta _MetaData;

        #endregion
        
        #region <Methods>
                
        /// <summary>
        /// 메타 데이터의 기본값을 생성하는 메서드
        /// </summary>
        protected virtual void AddDefaultMetaData()
        {
        }

        /// <summary>
        /// 메타 데이터를 기본값으로 되돌리는 메서드
        /// </summary>
        protected void ClearMetaData(bool p_DefaultFlag)
        {
            _MetaData = null;

            if (p_DefaultFlag)
            {
                AddDefaultMetaData();
            }
        }

        #endregion
    }
}