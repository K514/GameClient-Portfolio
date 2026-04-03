using UnityEngine;

namespace k514
{
    public partial class DataStorageBase<This, Table, Record>
    {
        public string GetKey()
        {
            return $"{_DataKey}";
        }

        public bool HasData()
        {
            var dataKey = GetKey();
#if STEAMWORKS_NET
            return SteamRemoteStorage.FileExists(accountDataKey);
#elif BACK_END && !UNITY_EDITOR
            return BackEndManager.GetInstanceUnsafe.GetHasTable(accountDataKey);
#else
            return PlayerPrefs.HasKey(dataKey);
#endif
        }
        
        public string GetData()
        {
            var dataKey = GetKey();
#if STEAMWORKS_NET
            return SteamManager.FileLoad<string>(accountDataKey);
#elif BACK_END && !UNITY_EDITOR
            return BackEndManager.GetInstanceUnsafe.GetTableData(accountDataKey);
#else
            return PlayerPrefs.GetString(dataKey);
#endif
        }
    }
}