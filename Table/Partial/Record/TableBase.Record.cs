using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace k514
{
    public partial class TableBase<Table, Meta, Key, Record>
    {
        #region <Methods>

        private async UniTask<Record> SpawnRecord(Key p_Key, CancellationToken p_Cancellation, params object[] p_Params)
        {
            var spawnedRecord = new Record();
            await spawnedRecord.SetRecord(p_Key, p_Params, p_Cancellation);
            await spawnedRecord.OnRecordDecoded(p_Cancellation);
            return spawnedRecord;
        }

        private async UniTask<bool> AddRecord(Record p_Record, bool p_OverlapFlag, CancellationToken p_Cancellation)
        {
            var tryKey = p_Record.KEY;
            if (IsAddibleKey(tryKey) && (p_OverlapFlag || !_Table.ContainsKey(tryKey)))
            {
                _Table.Add(tryKey, p_Record);
                await p_Record.OnRecordAdded(this as Table, p_Cancellation);

                if (_Table.Count == 1)
                {
                    _FirstRecord = p_Record;
                }
                
                return true;
            }
            else
            {
                return false;
            }
        }
        
        public async UniTask<bool> AddRecord(Key p_Key, bool p_OverlapFlag, CancellationToken p_Cancellation, params object[] p_Params)
        {
            if (IsAddibleKey(p_Key))
            {
                if (_Table.ContainsKey(p_Key))
                {
                    if (p_OverlapFlag)
                    {
                        var spawnedRecord = await SpawnRecord(p_Key, p_Cancellation, p_Params);
                        _Table[p_Key] = spawnedRecord;
                        await spawnedRecord.OnRecordAdded(this as Table, p_Cancellation);
                        
                        if (_Table.Count == 1)
                        {
                            _FirstRecord = spawnedRecord;
                        }
                
                        return true;
                    }
                }
                else
                {
                    var spawnedRecord = await SpawnRecord(p_Key, p_Cancellation, p_Params);
                    _Table.Add(p_Key, spawnedRecord);
                    await spawnedRecord.OnRecordAdded(this as Table, p_Cancellation);
                    
                    if (_Table.Count == 1)
                    {
                        _FirstRecord = spawnedRecord;
                    }
                
                    return true;
                }
            }
            
            return false;
        }

        public void RemoveRecord(Key p_Key)
        {
            if (_Table.TryGetValue(p_Key, out var o_Record))
            {
                _Table.Remove(p_Key);

                if (ReferenceEquals(_FirstRecord, o_Record))
                {
                    _FirstRecord = _Table.Count > 0 ? _Table.First().Value : default;
                }

                if (ReferenceEquals(_FallbackRecord, o_Record))
                {
                    _FallbackRecord = default;
                }
            }
        }

        public Record CastRecord(ITableRecord p_Record, bool p_FallbackFlag)
        {
            if (p_FallbackFlag)
            {
                return p_Record as Record ?? GetFallbackRecord();
            }
            else
            {
                return p_Record as Record;
            }
        }

        #endregion
    }
}