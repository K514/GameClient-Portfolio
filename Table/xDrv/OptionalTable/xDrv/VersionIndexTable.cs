using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using xk514;

namespace k514
{
    [Serializable]
    public class VersionIndexTableMetaData : TableMetaData
    {
    }
    
    /// <summary>
    /// 에셋 번들이 생성되었을 때, 해당 번들의 버전 및 상세정보를 기술하는 테이블 클래스
    /// </summary>
    public class VersionIndexTable : OptionalTable<VersionIndexTable, VersionIndexTableMetaData, int, VersionIndexTable.TableRecord>
    {
        #region <Fields>

        /// <summary>
        /// 버전의 해쉬값을 기준으로 정렬된 테이블
        /// </summary>
        private Dictionary<string, TableRecord> HashSortedTable;

        #endregion
        
        #region <Record>
        
        public class TableRecord : OptionalTableRecord
        {
            public string APKVersion;
            public int BundleVersion;
            public string VersionDescription;
            public string VersionDriveDate;
            public string PrevHash;
            public string CurrentHash;

            public override async UniTask SetRecord(int p_Key, object[] p_RecordField, CancellationToken p_CancellationToken)
            {
                await base.SetRecord(p_Key, p_RecordField, p_CancellationToken);

                APKVersion = (string) p_RecordField.GetElementSafe(0);
                BundleVersion = (int) p_RecordField.GetElementSafe(1);
                VersionDescription = (string) p_RecordField.GetElementSafe(2);
                VersionDriveDate = (string) p_RecordField.GetElementSafe(3);
                PrevHash = (string) p_RecordField.GetElementSafe(4);
                CurrentHash = (string) p_RecordField.GetElementSafe(5);
            }

#if UNITY_EDITOR
            public override string ToString()
            {
                return
                    $"[APKVersion] : {APKVersion}\n$[BundleVersion] : {BundleVersion}\n$[VersionDriveDate] : {VersionDriveDate}\n"
                    + $"$[VersionDescription] : {VersionDescription}";
            }
#endif
        }

        #endregion

        #region <Callbacks>

        protected override async UniTask OnCreated(CancellationToken p_CancellationToken)
        {
            await base.OnCreated(p_CancellationToken);
            
            HashSortedTable = new Dictionary<string, TableRecord>();
        }

        protected override async UniTask OnInitiate(CancellationToken p_CancellationToken)
        {
            await base.OnInitiate(p_CancellationToken);

            HashSortedTable.Clear();
            
            var table = GetTable();
            foreach (var tableRecordKV in table)
            {
                var tryRecord = tableRecordKV.Value;
                var hash = tryRecord.CurrentHash;

                HashSortedTable[hash] = tryRecord;
            }            
        }
        
        #endregion

        #region <Methods>
        
        public (bool, TableRecord) GetLatestVersionRecord()
        {
            if (GetTable().Count > 0)
            {
                return (true, this[GetTable().Keys.Max()]);
            }
            else
            {
                return default;
            }
        }

        public TableRecord GetVersionRecord(int p_Version)
        {
            return this[GetTable().Keys.Where(key => key == p_Version).Max()];
        }
        
        public (bool, TableRecord) GetHashRecord(string p_Hash)
        {
            if (HashSortedTable.TryGetValue(p_Hash, out var o_Record))
            {
                return (true, o_Record);
            }
            else
            {
                return default;
            }
        }
        
        public bool IsValidBundle(int p_TryVersion, string p_Hash)
        {
#if APPLY_PRINT_LOG
            CustomDebug.LogWarning($"[버전 테이블] : [클라이언트 버전 : {p_TryVersion}] [번들 해시 : {p_Hash}]");
#endif
            if (HashSortedTable.TryGetValue(p_Hash, out var o_Record))
            {
#if APPLY_PRINT_LOG
                CustomDebug.LogWarning($"[버전 테이블] : [일치하는 해시 버전 : {o_Record.BundleVersion}]");
#endif
                return o_Record.BundleVersion == p_TryVersion;
            }
            else
            {
#if APPLY_PRINT_LOG
                CustomDebug.LogWarning($"[버전 테이블] : 일치하는 해시가 없습니다.");
#endif
                return false;
            }
        }

        /// <summary>
        /// 서버의 최신 버전 프리셋을 로드하는 메서드
        /// </summary>
        public async UniTask TryLoadVersionIndexFromServer(CancellationToken p_CancellationToken, int p_TimeOut = NetworkTool.DefaultTimeOutSecond, string p_SaveDirectory = "")
        {
/*#if APPLY_PRINT_LOG
            CustomDebug.LogWarning("[버전 테이블] : 버젼 프리셋 정보 로드를 시작합니다.");
#endif
            var URI = await NetworkTool.GetPatchNode_VersionIndex_URL_From_Server(p_CancellationToken);
#if APPLY_PRINT_LOG
            CustomDebug.LogWarning($"[버전 테이블] : 대상 URI {URI}");
#endif
            var webRequestParams = new NetworkTool.UnityWebRequestParams(URI, p_TimeOut, p_SaveDirectory);
            var (valid, webRequestHandler) = await DownloadManager.GetInstanceUnsafe.RunHandler(webRequestParams, 1f);
            if (valid)
            {
#if APPLY_PRINT_LOG
                CustomDebug.LogWarning($"[버전 테이블] : 버젼 테이블이 서버로부터 클래스에 로드되었습니다. {webRequestHandler.WebRequest.result}");
#endif
                var webReqeust = webRequestHandler.WebRequest;
                var text = webReqeust.downloadHandler.text;
                await ReadTextAssetTable(text, p_CancellationToken);
                DownloadManager.GetInstanceUnsafe.CancelRequest(webRequestParams);
#if APPLY_PRINT_LOG
                CustomDebug.LogWarning($"[버전 테이블] : 로드된 레코드 개수 [{GetTable().Count}]");
#endif
            }*/
        }
        
#if UNITY_EDITOR
        protected override async UniTask AddDefaultRecords(CancellationToken p_CancellationToken)
        {
            await base.AddDefaultRecords(p_CancellationToken);

            await AddRecord(0, false, p_CancellationToken, new object[]{Application.version, -1, "Table Spawned", $"{DateTime.Now.ToLongDateString()}, {DateTime.Now.ToLongTimeString()}", "114", "514"});
        }

        public async UniTask AddVersionDescription(int p_BundleVersion, string p_Description, string p_PrevHash, string p_CurrentHash, CancellationToken p_CancellationToken)
        {
            var nextKey = _Table.Keys.Count;
            await AddRecord(nextKey, true, p_CancellationToken, new object[]{Application.version, p_BundleVersion, p_Description, $"{DateTime.Now.ToLongDateString()}, {DateTime.Now.ToLongTimeString()}", p_PrevHash, p_CurrentHash});
        }
#endif
        
        #endregion
    }
}