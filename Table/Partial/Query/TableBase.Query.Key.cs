using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace k514
{
    public partial class TableBase<Table, Meta, Key, Record>
    {
        #region <Methods>
        
        /// <summary>
        /// 기본키 값을 지정하는 용도의 가상 메서드
        ///
        /// Key 타입이 전부 Value라면 default 값으로 테이블에 넣을 수 있지만
        /// 문자열 등의 Reference도 Key 타입으로 사용해야하기 때문에 null 외의 값을
        /// 지정하는 용도로 사용한다.
        /// </summary>
        protected virtual Key GetDefaultKey()
        {
            return default;
        }
        
        public bool HasKey(Key p_Key)
        {
            return _Table.ContainsKey(p_Key);
        }
        
        public virtual bool IsAddibleKey(Key p_Key)
        {
            return true;
        }
        
        public List<Key> GetCurrentKeyEnumerator()
        {
            return GetTable().Keys.ToList();
        }
        
        public bool TryGetRecordObject<Record1>(Key p_Key, out Record1 o_Record) where Record1 : ITableRecord<Key>
        {
            if (TryGetRecord(p_Key, out var o_ThisRecord) && o_ThisRecord is Record1 c_Record)
            {
                o_Record = c_Record;
                return true;
            }
            else
            {
                o_Record = default;
                return false;
            }
        }
        
        #endregion
    }
}