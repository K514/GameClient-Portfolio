#if !SERVER_DRIVE

using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using System.Threading;

namespace k514
{
    public class LanguageDataTableQuery : MultiTableIndexBase<LanguageDataTableQuery, TableMetaData, LanguageDataTableQuery.TableLabel, ILanguageDataTableBridge, ILanguageDataTableRecordBridge>
    {
        #region <Enums>

        public enum TableLabel
        {
            SystemLanguage,         // 0 ~ 1
            
            FormatLanguage,         // 1 ~ 2
            UILanguage,             // 2 ~ 3
            GameContentLanguage,    // 3 ~ 4
            GameEntityLanguage,     // 5 ~ 6
            
            ItemLanguage,           // 10 ~ 11
            ExtraOptionLanguage,    // 11 ~ 12
            
            ActionLanguage,         // 20 ~ 21
            
            StoreLanguage,          // 30 ~ 31
            QuestLanguage,          // 31 ~ 32
            
            DungeonLanguage,        // 40 ~ 41
            ScenarioLanguage,       // 41 ~ 42
        }

        [Flags]
        public enum LanguageEventType
        {
            None = 0,
            
            LanguageChanged = 1 << 0,
        }

        #endregion

        #region <Methods>

        public async UniTask<List<ILanguageDataTableBridge>> ReloadLanguageTable(CancellationToken p_Token)
        {
            return await ReloadLanguageTable(SystemBoot.SystemLanguageType, p_Token);
        }
        
        public async UniTask<List<ILanguageDataTableBridge>> ReloadLanguageTable(LanguageType p_Type, CancellationToken p_Token)
        {
            var result = new List<ILanguageDataTableBridge>();
            foreach (var tableSetKV in _LabelTableListTable)
            {
                var tableSet = tableSetKV.Value;
                foreach (var table in tableSet)
                {
                    var alterName = table.GetLanguageTableFileName(p_Type);
                    await table.LoadTable(alterName, p_Token);
                    result.Add(table);
                }
            }
            
            return result;
        }
        
        public static string GetContent(int p_Key)
        {
            if (GetInstanceUnsafe.TryGetRecordBridge(p_Key, out var o_Record))
            {
                return o_Record.Text;
            }
            else
            {
                return $"* No Contents ({p_Key}) *";
            }
        }

        #endregion
    }
}

#endif