using System;
using System.Collections.Generic;

namespace k514
{
    public partial class MultiTableBase<This, Key, Meta, Label, TableBridge, RecordBridge>
    {
        #region <Indexer>

        public List<TableBridge> this[Label p_Label]
        {
            get
            {
                if (TryGetLabelTableList(p_Label, out var o_List))
                {
                    return o_List;
                }
                else
                {
                    return null;
                }
            }
        }
        
        #endregion
        
        #region <Methods>
 
        public bool IsLabel(Key p_Key, Label p_Label)
        {
            return TryGetLabel(p_Key, out var o_Label) 
                   && EqualityComparer<Label>.Default.Equals(o_Label, p_Label);
        }
        
        public bool TryGetLabel(Key p_Key, out Label o_Label)
        {
            if (TryGetTable(p_Key, out var o_Table))
            {
                o_Label = o_Table.TableLabel;
                return true;
            }
            else
            {
                o_Label = default;
                return false;
            }
        }
        
        public bool TryGetLabelTableKey(Label p_Label, out Key o_Key)
        {
            if (TryGetLabelTable(p_Label, out var o_Table))
            {
                return o_Table.TryGetFirstKey(out o_Key);
            }
            else
            {
                o_Key = default;
                return false;
            }
        }
        
        public bool TryGetLabelTable(Label p_Label, out TableBridge o_Table)
        {
            if (TryGetLabelTableList(p_Label, out var o_List))
            {
                o_Table = o_List[0];
                return true;
            }
            else
            {
                o_Table = null;
                return false;
            }
        }
        
        public bool TryGetLabelTable(Label p_Label, Key p_Key, out TableBridge o_Table)
        {
            if (TryGetLabelTableList(p_Label, out var o_List))
            {
                foreach (var table in o_List)
                {
                    if (table.IsBridged(p_Key))
                    {
                        o_Table = table;
                        return true;
                    }
                }
            }
            
            o_Table = null;
            return false;
        }
        
        public bool TryGetLabelTableRecord(Label p_Label, out RecordBridge o_Record)
        {
            if (TryGetLabelTable(p_Label, out var o_Table))
            {
                return o_Table.TryGetFirstRecordBridge(out o_Record);
            }
            else
            {
                o_Record = null;
                return false;
            }
        }
        
        public bool TryGetLabelTableRecord(Label p_Label, Key p_Key, out RecordBridge o_Record)
        {
            if (TryGetLabelTableList(p_Label, out var o_List))
            {
                foreach (var table in o_List)
                {
                    if (table.IsBridged(p_Key)
                        && table.TryGetRecordBridge(p_Key, out o_Record))
                    {
                        return true;
                    }
                }
            }
            
            o_Record = null;
            return false;
        }
        
        public bool TryGetLabelContext(Key p_Key, out Label o_LabelType, out TableBridge o_Table, out RecordBridge o_Record)
        {
            foreach (var listKV in _LabelTableListTable)
            {
                var list = listKV.Value;
                foreach (var table in list)
                {
                    if (table.IsBridged(p_Key) 
                        && table.TryGetRecordBridge(p_Key, out o_Record))
                    {
                        o_LabelType = table.TableLabel;
                        o_Table = table;
                        
                        return true;
                    }
                }
            }

            o_LabelType = default;
            o_Table = null;
            o_Record = null;
            
            return false;
        }
        
        public bool TryGetLabelTableList(Label p_Label, out List<TableBridge> o_List)
        {
            if (_LabelTableListTable.TryGetValue(p_Label, out o_List))
            {
                return o_List.CheckCollectionSafe();
            }
            else
            {
                return false;
            }
        }

        #endregion
    }
}