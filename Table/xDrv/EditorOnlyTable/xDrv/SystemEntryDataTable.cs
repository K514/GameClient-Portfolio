#if UNITY_EDITOR

using System.Threading;
using Cysharp.Threading.Tasks;

namespace k514
{
    public class SceneEntryDataMetaData : TableMetaData
    {
        public SystemEntryMode LatestEntryMode;
    }

    public class SystemEntryDataTable : EditorOnlyTable<SystemEntryDataTable, SceneEntryDataMetaData, SystemEntryMode, SystemEntryDataTable.TableRecord>
    {
        #region <Record>

        public class TableRecord : EditorOnlyTableRecord
        {
            public SystemEntryPreset SystemEntryPreset { get; private set; }
            
            public override async UniTask SetRecord(SystemEntryMode p_Key, object[] p_RecordField, CancellationToken p_CancellationToken)
            {
                await base.SetRecord(p_Key, p_RecordField, p_CancellationToken);
                
                SystemEntryPreset = (SystemEntryPreset) p_RecordField.GetElementSafe(0);
            }
        }

        #endregion

        #region <Methods>

        protected override void AddDefaultMetaData()
        {
            if (ReferenceEquals(null, _MetaData))
            {
                _MetaData = new SceneEntryDataMetaData();
                _MetaData.LatestEntryMode = SystemBoot.DEFAULT_ENTRY_MODE;
            }
        }

        public void SetLatestEntryMode(SystemEntryMode p_Type)
        {
            _MetaData.LatestEntryMode = p_Type;
        }

        public SystemEntryMode GetLatestMode()
        {
            return _MetaData.LatestEntryMode;
        }

        protected override async UniTask AddDefaultRecords(CancellationToken p_CancellationToken)
        {
            await base.AddDefaultRecords(p_CancellationToken);

            var enumerator = EnumFlag.GetEnumEnumerator<SystemEntryMode>(EnumFlag.GetEnumeratorType.ExceptNone);
            foreach (var entryMode in enumerator)
            {
                switch (entryMode)
                {
                    case SystemEntryMode.SelectMode:
                        await AddRecord(entryMode, false, p_CancellationToken, new SystemEntryPreset(entryMode, SystemBoot.DEFAULT_ENTRY_INDEX));
                        break; 
                    case SystemEntryMode.SingleMode:
                    case SystemEntryMode.MultiMode:
                    case SystemEntryMode.AttachMode:
                    case SystemEntryMode.DebugMode:
                        await AddRecord(entryMode, false, p_CancellationToken, new SystemEntryPreset(entryMode));
                        break; 
                }
            }
        }
        
        #endregion
    }
}

#endif