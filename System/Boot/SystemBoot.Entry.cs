using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using xk514;

namespace k514
{
    public partial class SystemBoot
    {
        #region <Consts>

        public const SystemEntryMode DEFAULT_ENTRY_MODE = SystemEntryMode.SingleMode;
        public const int DEFAULT_ENTRY_INDEX = 0;
        public static readonly SystemEntryPreset DefaultSystemEntryPreset = new SystemEntryPreset(DEFAULT_ENTRY_MODE, DEFAULT_ENTRY_INDEX);  
        
        #endregion
        
        #region <Fields>

        public static SystemEntryPreset SystemEntryPreset;
        
        #endregion
        
        #region <Callbacks>

        private static async UniTask OnInitSystemEntry()
        {
#if UNITY_EDITOR
            if (SystemFlagTable.IsSystemDevMode())
            {
                SetLatestSystemEntryPreset();
            }
            else
            {
                SetSystemEntryPreset(DefaultSystemEntryPreset);
            }
#else
            SetSystemEntryPreset(DefaultSystemEntryPreset);
#endif
        }

        private static void OnReleaseSystemEntry()
        {
        }

        #endregion
        
        #region <Methods>

#if UNITY_EDITOR
        public static void SetLatestSystemEntryPreset()
        {
            var latestMode = GetLatestMode();
            var entryTable = SystemEntryDataTable.GetInstanceUnsafe;
            var tryMode = entryTable.HasKey(latestMode) ? latestMode : DEFAULT_ENTRY_MODE;
#if APPLY_PRINT_LOG
            CustomDebug.LogError(($"{tryMode} 모드로 세트되었습니다.", Color.blue));
#endif
            SetSystemEntryPreset(entryTable[tryMode].SystemEntryPreset);
        }

        private static SystemEntryMode GetLatestMode()
        {
            return SystemEntryDataTable.GetInstanceUnsafe.GetLatestMode();
        }
#endif
        
        public static void SetSystemEntryPreset(SystemEntryPreset p_SystemEntryPreset)
        {
            SystemEntryPreset = p_SystemEntryPreset;
        }
        
        public static SystemEntryMode GetEntryMode()
        {
            return SystemEntryPreset.EntryMode;
        }
        
        public static int GetEntryIndex()
        {
            return SystemEntryPreset.EntryIndex;
        }

        public static bool IsDebugMode()
        {
            return GetEntryMode() == SystemEntryMode.DebugMode;
        }

        #endregion
    }
}
