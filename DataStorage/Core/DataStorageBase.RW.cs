using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace k514
{
    public partial class DataStorageBase<This, Table, Record>
    {
        /// <summary>
        /// 테이블 데이터를 PlayerPref에 문자열 직렬화해서 저장하는 메서드
        /// </summary>
        public async UniTask SaveData(CancellationToken p_Token)
        {
            var accountDataKey = GetKey();
            var tableData = _DataTable.GetTableImage();
            var lexicalData = await TableLoader.GetInstanceUnsafe.Extract(tableData, p_Token);
            var serializedLexicalData = lexicalData.EncodeValue(typeof(TableTool.TableLexicalData<int>));
#if STEAMWORKS_NET
            SteamRemoteStorage.BeginFileWriteBatch();
            SteamManager.FileSave(accountDataKey, serializedLexicalData);
            SteamRemoteStorage.EndFileWriteBatch();
#elif BACK_END && !UNITY_EDITOR
            BackEndManager.GetInstanceUnsafe.SaveTableData(accountDataKey, serializedLexicalData);
#else
            PlayerPrefs.SetString(accountDataKey, serializedLexicalData);
            PlayerPrefs.Save();
#endif
        }

        /// <summary>
        /// 테이블 데이터를 PlayerPref로부터 로드하는 메서드
        /// </summary>
        public async UniTask<bool> LoadData(CancellationToken p_Token)
        {
            if (HasData())
            {
                var serializedTableData = GetData();
                if (string.IsNullOrEmpty(serializedTableData))
                {
                    return false;
                }
                else
                {
                    var lexicalData = serializedTableData.DecodeValue<TableTool.TableLexicalData<int>>();
                    var tableImage = await TableLoader.GetInstanceUnsafe.ParseTableLexicalData<DataStorageTableMetaData, int, Record>(lexicalData, GetCancellationToken());
                    return await _DataTable.ReplaceTable(tableImage, p_Token);
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 테이블 데이터를 PlayerPref에 문자열 직렬화해서 저장하는 메서드
        /// </summary>
        public void RemoveData(bool p_ClearTableFlag)
        {
            if (p_ClearTableFlag)
            {
                _DataTable.ClearTable(true);
            }
            
            var dataKey = GetKey();
#if STEAMWORKS_NET
            SteamManager.FileDelete(SystemBoot.LanguageStorageName);
            SteamManager.FileDelete(accountDataKey);
#elif BACK_END && !UNITY_EDITOR
            BackEndManager.GetInstanceUnsafe.DeleteTableData(accountDataKey);
#else
            PlayerPrefs.DeleteKey(dataKey);
            PlayerPrefs.Save();
#endif
        }
    }
}