namespace k514
{
    public struct SystemEntryPreset
    {
        #region <Fields>

        public SystemEntryMode EntryMode;
        public int EntryIndex;
            
        [TableTool.TableRecordAttribute(TableTool.TableRecordAttributeType.Runtime)] 
        public readonly bool ValidFlag;

        #endregion

        #region <Constructor>

        public SystemEntryPreset(SystemEntryMode p_EntryMode) : this(p_EntryMode, 0)
        {
        }
        
        public SystemEntryPreset(SystemEntryMode p_EntryMode, int p_EntryIndex)
        {
            EntryMode = p_EntryMode;
            EntryIndex = p_EntryIndex;
            ValidFlag = true;
        }

        #endregion

        #region <Operator>

        public override string ToString()
        {
            switch (EntryMode)
            {
                default:
                case SystemEntryMode.SelectMode:
                    return $"[{EntryMode}](Entry Scene Index : [{EntryIndex}])";
                case SystemEntryMode.SingleMode:
                case SystemEntryMode.MultiMode:
                case SystemEntryMode.AttachMode:
                case SystemEntryMode.DebugMode:
                    return $"[{EntryMode}]";
            }
        }

        #endregion
    }
}