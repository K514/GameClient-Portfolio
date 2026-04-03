using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace k514.Mono.Feature
{
    public partial class DefaultAccountManager
    {
        #region <Methods>

        public Dictionary<int, AccountCharacterData> GetCharacterTable()
        {
            return DataRecord.CharacterTable;
        }

        public bool TryGetCharacterData(int p_Index, out AccountCharacterData o_Data)
        {
            var charaList = GetCharacterTable();
            return charaList.TryGetValue(p_Index, out o_Data);
        }

        public async UniTask AddCharacterData(AccountCharacterData p_Data, bool p_SaveData)
        {
            var charaTable = GetCharacterTable();
            charaTable.Add(p_Data.UnitSpawnDataTableIndex, p_Data);
    
            if (p_SaveData)
            {
                await SaveData(GetCancellationToken());
            }
        }

        public async UniTask RemoveCharacterInfo(AccountCharacterData p_Characterinfo, bool p_SaveData)
        {
            var charaTable = GetCharacterTable();
            charaTable.Remove(p_Characterinfo.UnitSpawnDataTableIndex);
            
            if (p_SaveData)
            {
                await SaveData(GetCancellationToken());
            }
        }

        #endregion
    }
}