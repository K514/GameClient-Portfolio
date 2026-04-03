using System;
using System.Collections.Generic;
using k514.Mono.Feature;

namespace k514
{
    public partial class MultiTableBase<This, Key, Meta, Label, TableBridge, RecordBridge>
    {
        #region <Methods>
          
        public bool IsSubLabel<SubLabel>(Key p_Key, SubLabel p_SubLabel) where SubLabel : struct, Enum
        {
            return TryGetSubLabel<SubLabel>(p_Key, out var o_SubLabel) 
                   && EqualityComparer<SubLabel>.Default.Equals(o_SubLabel, p_SubLabel);
        }
        
        public bool TryGetSubLabel<SubLabel>(Key p_Key, out SubLabel o_SubLabel) where SubLabel : struct, Enum
        {
            if (TryGetTable(p_Key, out var o_Table) && o_Table is ITableBridgeLabel<SubLabel> c_Table)
            {
                o_SubLabel = c_Table.TableLabel;
                return true;
            }
            else
            {
                o_SubLabel = default;
                return false;
            }
        }
        
        public bool TryGetSubLabelTableKey<SubLabel>(Label p_Label, SubLabel p_SubLabel, out Key o_Key) where SubLabel : struct, Enum
        {
            if (TryGetSubLabelTable(p_Label, p_SubLabel, out var o_Table))
            {
                return o_Table.TryGetFirstKey(out o_Key);
            }
            else
            {
                o_Key = default;
                return false;
            }
        }
        
        public bool TryGetSubLabelTable<SubLabel>(Label p_Label, SubLabel p_SubLabel, out TableBridge o_Table) where SubLabel : struct, Enum
        {
            if (TryGetLabelTableList(p_Label, out var o_List))
            {
                foreach (var table in o_List)
                {
                    if (table is ITableBridgeLabel<SubLabel> c_Table
                        && EqualityComparer<SubLabel>.Default.Equals(c_Table.TableLabel, p_SubLabel))
                    {
                        o_Table = table;
                        return true;
                    }
                }
            }
            
            o_Table = null;
            return false;
        }
        
        public bool TryGetSubLabelTable<SubLabel>(Label p_Label, Key p_Key, SubLabel p_SubLabel, out TableBridge o_Table) where SubLabel : struct, Enum
        {
            if (TryGetLabelTableList(p_Label, out var o_List))
            {
                foreach (var table in o_List)
                {
                    if (table.IsBridged(p_Key)
                        && table is ITableBridgeLabel<SubLabel> c_Table
                        && EqualityComparer<SubLabel>.Default.Equals(c_Table.TableLabel, p_SubLabel))
                    {
                        o_Table = table;
                        return true;
                    }
                }
            }
            
            o_Table = null;
            return false;
        }
        
        public bool TryGetSubLabelTableRecord<SubLabel>(Label p_Label, SubLabel p_SubLabel, out RecordBridge o_Record) where SubLabel : struct, Enum
        {
            if (TryGetSubLabelTable(p_Label, p_SubLabel, out var o_Table))
            {
                return o_Table.TryGetFirstRecordBridge(out o_Record);
            }
            else
            {
                o_Record = null;
                return false;
            }
        }
        
        public bool TryGetSubLabelTableRecord<SubLabel>(Label p_Label, Key p_Key, SubLabel p_SubLabel, out RecordBridge o_Record) where SubLabel : struct, Enum
        {
            if (TryGetLabelTableList(p_Label, out var o_List))
            {
                foreach (var table in o_List)
                {
                    if (table.IsBridged(p_Key)
                        && table is ITableBridgeLabel<SubLabel> c_Table
                        && EqualityComparer<SubLabel>.Default.Equals(c_Table.TableLabel, p_SubLabel)
                        && table.TryGetRecordBridge(p_Key, out o_Record))
                    {
                        return true;
                    }
                }
            }
            
            o_Record = null;
            return false;
        }

        public bool TryGetSubLabelContext<SubLabel>(Key p_Key, out Label o_Label, out SubLabel o_SubLabel, out TableBridge o_Table, out RecordBridge o_Record) where SubLabel : struct, Enum
        {
            foreach (var listKV in _LabelTableListTable)
            {
                var list = listKV.Value;
                foreach (var table in list)
                {
                    if (table.IsBridged(p_Key)
                        && table is ITableBridgeLabel<SubLabel> c_Table
                        && table.TryGetRecordBridge(p_Key, out o_Record))
                    {
                        o_Label = table.TableLabel;
                        o_SubLabel = c_Table.TableLabel;
                        o_Table = table;
                        
                        return true;
                    }
                }
            }

            o_Label = default;
            o_SubLabel = default;
            o_Table = null;
            o_Record = null;
            
            return false;
        }

        #endregion
    }
}