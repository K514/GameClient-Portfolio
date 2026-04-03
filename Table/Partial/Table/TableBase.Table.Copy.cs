using System.Collections.Generic;
using System.Linq;

namespace k514
{
    public partial class TableBase<Table, Meta, Key, Record>
    {
        #region <Methods>
                                
        /// <summary>
        /// 현재 테이블를 구성하는 메타데이터 및 레코드 컬렉션을 리턴하는 메서드
        /// </summary>
        public TableTool.TableDataImage<Meta, Key, Record> GetTableImage()
        {
            return new TableTool.TableDataImage<Meta, Key, Record>(GetMetaCopy(), GetTableCopy());
        }

        private Meta GetMetaCopy()
        {
            return _MetaData?.Clone() as Meta;
        }

        private Dictionary<Key, Record> GetTableCopy()
        {
            var tryTable = GetTable();
            return tryTable.ToDictionary(kpv => kpv.Key, kpv => kpv.Value);
        }

        #endregion
    }
}