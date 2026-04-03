using System.Collections.Generic;

namespace k514
{
    public partial class TableBase<Table, Meta, Key, Record>
    {
        #region <Methods>

        /// <summary>
        /// 해당 테이블 타입과 동일한 텅빈 컬렉션 하나를 인스턴스화 하여 리턴하는 메서드
        /// </summary>
        private Dictionary<Key, Record> SpawnEmptyTable()
        {
            return new Dictionary<Key, Record>();
        }

        /// <summary>
        /// 테이블 컬렉션의 생성을 보장하는 메서드
        /// </summary>
        protected void CheckTable()
        {
            if (ReferenceEquals(null, _Table))
            {
                _Table = SpawnEmptyTable();
            }
        }

        /// <summary>
        /// 테이블을 비우는 메서드
        /// </summary>
        public void ClearTable(bool p_DefaultFlag)
        {
            _Table = null;
            
            if (p_DefaultFlag)
            {
                CheckTable();
            }
            
            OnTableCleared();
        }

        #endregion
    }
}