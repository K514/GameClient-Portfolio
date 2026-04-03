using System.Collections.Generic;

namespace k514
{
    public partial class MultiTableBase<This, Key, Meta, Label, TableBridge, RecordBridge>
    {
        #region <Methods>

        public bool HasKey(Key p_Key)
        {
            if (!ReferenceEquals(null, _LabelTableListTable))
            {
                foreach (var listKV in _LabelTableListTable)
                {
                    var list = listKV.Value;
                    foreach (var table in list)
                    {
                        if (table.HasKey(p_Key))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public bool IsAddibleKey(Key p_Key)
        {
            if (!ReferenceEquals(null, _LabelTableListTable))
            {
                foreach (var listKV in _LabelTableListTable)
                {
                    var list = listKV.Value;
                    foreach (var labelTable in list)
                    {
                        if (labelTable.IsAddibleKey(p_Key))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public List<Key> GetCurrentKeyEnumerator()
        {
            var result = new List<Key>();
            if (!ReferenceEquals(null, _LabelTableListTable))
            {
                foreach (var listKV in _LabelTableListTable)
                {
                    var list = listKV.Value;
                    foreach (var labelTable in list)
                    {
                        var subKeySet = labelTable.GetCurrentKeyEnumerator();
                        result.AddRange(subKeySet);
                    }
                }
            }
            return result;
        }

        public bool TryGetFirstKey(out Key o_Key)
        {
            if (!ReferenceEquals(null, _LabelTableListTable))
            {
                foreach (var listKV in _LabelTableListTable)
                {
                    var list = listKV.Value;
                    foreach (var labelTable in list)
                    {
                        if (labelTable.TryGetFirstKey(out o_Key))
                        {
                            return true;
                        }
                    }
                }
            }

            o_Key = default;
            return false;
        }

        public bool TryGetFallbackKey(out Key o_Key)
        {
            if (!ReferenceEquals(null, _LabelTableListTable))
            {
                foreach (var listKV in _LabelTableListTable)
                {
                    var list = listKV.Value;
                    foreach (var labelTable in list)
                    {
                        if (labelTable.TryGetFallbackKey(out o_Key))
                        {
                            return true;
                        }
                    }
                }
            }

            o_Key = default;
            return false;
        }
        
        public bool TryGetRecordObject<Record>(Key p_Key, out Record o_Record) where Record : ITableRecord<Key>
        {
            if (TryGetRecordBridge(p_Key, out var o_RecorBridge) && o_RecorBridge is Record c_Record)
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