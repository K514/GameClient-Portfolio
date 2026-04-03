using System;
using System.Collections.Generic;

namespace k514
{
    public interface IMultiTableQuery<in Key, TableBridge, RecordBridge>
    {
        bool TryGetFirstTable(out TableBridge o_Table);
        bool TryGetTable(Key p_Key, out TableBridge o_Table);
        bool TryGetRecordBridge(Key p_Key, out RecordBridge o_Record);
        bool TryGetFirstRecordBridge(out RecordBridge o_Record);
        bool TryGetFallbackRecordBridge(out RecordBridge o_Record);
    }

    public interface IMultiTableLabelQuery<Key, Label, TableBridge, RecordBridge>
     where Label : struct, Enum
    {
        List<TableBridge> this[Label p_Label] { get; }
        bool IsLabel(Key p_Key, Label p_Label);
        bool TryGetLabel(Key p_Key, out Label o_Label);
        bool TryGetLabelTableKey(Label p_Label, out Key o_Key);
        bool TryGetLabelTable(Label p_Label, out TableBridge o_Table);
        bool TryGetLabelTable(Label p_Label, Key p_Key, out TableBridge o_Table);
        bool TryGetLabelTableRecord(Label p_Label, out RecordBridge o_Record);
        bool TryGetLabelTableRecord(Label p_Label, Key p_Key, out RecordBridge o_Record);
        bool TryGetLabelContext(Key p_Key, out Label o_Label, out TableBridge o_Table, out RecordBridge o_Record);
        bool TryGetLabelTableList(Label p_Label, out List<TableBridge> o_List);
    }
    
    public interface IMultiTableSubLabelQuery<Key, Label, TableBridge, RecordBridge>
        where Label : struct, Enum
    {
        bool IsSubLabel<SubLabel>(Key p_Key, SubLabel p_SubLabel) where SubLabel : struct, Enum;
        bool TryGetSubLabel<SubLabel>(Key p_Key, out SubLabel o_SubLabel) where SubLabel : struct, Enum;
        bool TryGetSubLabelTableKey<SubLabel>(Label p_Label, SubLabel p_SubLabel, out Key o_Key) where SubLabel : struct, Enum;
        bool TryGetSubLabelTable<SubLabel>(Label p_Label, SubLabel p_SubLabel, out TableBridge o_Table) where SubLabel : struct, Enum;
        bool TryGetSubLabelTable<SubLabel>(Label p_Label, Key p_Key, SubLabel p_SubLabel, out TableBridge o_Table) where SubLabel : struct, Enum;
        bool TryGetSubLabelTableRecord<SubLabel>(Label p_Label, SubLabel p_SubLabel, out RecordBridge o_Record) where SubLabel : struct, Enum;
        bool TryGetSubLabelTableRecord<SubLabel>(Label p_Label, Key p_Key, SubLabel p_SubLabel, out RecordBridge o_Record) where SubLabel : struct, Enum;
        bool TryGetSubLabelContext<SubLabel>(Key p_Key, out Label o_Label, out SubLabel o_SubLabel, out TableBridge o_Table, out RecordBridge o_Record) where SubLabel : struct, Enum;
    }
}