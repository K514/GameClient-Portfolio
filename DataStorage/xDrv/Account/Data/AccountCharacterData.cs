using System;
using System.Collections.Generic;
using System.Linq;
using k514.Mono.Common;
using UnityEngine;

namespace k514.Mono.Feature
{
    [Serializable]
    public class AccountCharacterData
    {
        #region <Fields>
        
        public int UnitSpawnDataTableIndex;
        public GameEntityItemTool.ItemRankType RankType;
        public int Level;
        public float Exp;
        public Dictionary<int, int> PassiveSkillIndexTable;
        public Dictionary<int, bool> EinArmsOpenFlag;

        #endregion

        #region <Constructor>

        public AccountCharacterData(int p_Index, GameEntityItemTool.ItemRankType p_RankType = GameEntityItemTool.ItemRankType.None, int p_Value = 1, float p_Rate = 0f)
        {
            UnitSpawnDataTableIndex = p_Index;
            RankType = p_RankType;
            Level = p_Value;
            Exp = p_Rate;
            PassiveSkillIndexTable = new Dictionary<int, int>();
            EinArmsOpenFlag = new Dictionary<int, bool>();

            var componentRecord = GetSpawnRecord()?.GetComponentRecordBridge();
            if (!ReferenceEquals(null, componentRecord))
            {
                var lowerBound = p_RankType switch
                {
                    GameEntityItemTool.ItemRankType.None => 1,
                    GameEntityItemTool.ItemRankType.Normal => 1,
                    GameEntityItemTool.ItemRankType.Magic => 1,
                    GameEntityItemTool.ItemRankType.Rare => 1,
                    GameEntityItemTool.ItemRankType.Unique => 2,
                    GameEntityItemTool.ItemRankType.Legendary => 2,
                    GameEntityItemTool.ItemRankType.Epic => 3,
                    GameEntityItemTool.ItemRankType.Fantasm => 4,
                };
                
                var extraLevel = p_RankType switch
                {
                    GameEntityItemTool.ItemRankType.None => 0,
                    GameEntityItemTool.ItemRankType.Normal => 0,
                    GameEntityItemTool.ItemRankType.Magic => 1,
                    GameEntityItemTool.ItemRankType.Rare => 2,
                    GameEntityItemTool.ItemRankType.Unique => 4,
                    GameEntityItemTool.ItemRankType.Legendary => 8,
                    GameEntityItemTool.ItemRankType.Epic => 16,
                    GameEntityItemTool.ItemRankType.Fantasm => 32,
                };
                  
                var possibilityA = p_RankType switch
                {
                    GameEntityItemTool.ItemRankType.None => 0.8,
                    GameEntityItemTool.ItemRankType.Normal => 0.8,
                    GameEntityItemTool.ItemRankType.Magic => 0.75,
                    GameEntityItemTool.ItemRankType.Rare => 0.7,
                    GameEntityItemTool.ItemRankType.Unique => 0.5,
                    GameEntityItemTool.ItemRankType.Legendary => 0.3,
                    GameEntityItemTool.ItemRankType.Epic => 0.1,
                    GameEntityItemTool.ItemRankType.Fantasm => 0,
                };
                
                var possibilityB = p_RankType switch
                {
                    GameEntityItemTool.ItemRankType.None => 0.1,
                    GameEntityItemTool.ItemRankType.Normal => 0.1,
                    GameEntityItemTool.ItemRankType.Magic => 0.15,
                    GameEntityItemTool.ItemRankType.Rare => 0.18,
                    GameEntityItemTool.ItemRankType.Unique => 0.21,
                    GameEntityItemTool.ItemRankType.Legendary => 0.24,
                    GameEntityItemTool.ItemRankType.Epic => 0.27,
                    GameEntityItemTool.ItemRankType.Fantasm => 0.3,
                };
                
                var possibilityC = p_RankType switch
                {
                    GameEntityItemTool.ItemRankType.None => 0,
                    GameEntityItemTool.ItemRankType.Normal => 0,
                    GameEntityItemTool.ItemRankType.Magic => 0.01,
                    GameEntityItemTool.ItemRankType.Rare => 0.02,
                    GameEntityItemTool.ItemRankType.Unique => 0.04,
                    GameEntityItemTool.ItemRankType.Legendary => 0.08,
                    GameEntityItemTool.ItemRankType.Epic => 0.10,
                    GameEntityItemTool.ItemRankType.Fantasm => 0.15,
                };
                
                var actionList = componentRecord.GetActionBindPresetList();
                if (actionList.CheckCollectionSafe())
                {
                    foreach (var actionBindPreset in actionList)
                    {
                        var actionKey = actionBindPreset.Index;
                        if (SkillDataTableQuery.GetInstanceUnsafe.IsLabel(actionKey, SkillDataTableQuery.TableLabel.Passive))
                        {
                            var upperBound = PassiveSkillDataTableQuery.GetInstanceUnsafe.GetRecord(actionKey).LevelUpperBound;
                            PassiveSkillIndexTable.Add(actionKey, Mathf.Min(lowerBound, upperBound));
                        }
                    }
                }
                
                /*
                var passiveKeySet1 = CommonA_PassiveActionDataTable.GetInstanceUnsafe.GetCurrentKeyEnumerator();
                var passiveKeySet2 = CommonB_PassiveActionDataTable.GetInstanceUnsafe.GetCurrentKeyEnumerator();
                var passiveKeySet3 = CommonC_PassiveActionDataTable.GetInstanceUnsafe.GetCurrentKeyEnumerator();

                while (PassiveSkillIndexTable.Count < GameEntityTool.DefaultPassiveCountUpperBound)
                {
                    if (PassiveSkillIndexTable.Count < GameEntityTool.DefaultPassiveCountUpperBound
                        && Random.value < possibilityA
                        && passiveKeySet1.TryGetRandomElement(out var o_RandomKey1)
                        && !PassiveSkillIndexTable.ContainsKey(o_RandomKey1))
                    {
                        var upperBound = PassiveActionDataTableQuery.GetInstanceUnsafe.GetRecord(o_RandomKey1).LevelUpperBound;
                        PassiveSkillIndexTable.Add(o_RandomKey1, Mathf.Min(lowerBound, upperBound));
                    }

                    if (PassiveSkillIndexTable.Count < GameEntityTool.DefaultPassiveCountUpperBound
                        && Random.value < possibilityB
                        && passiveKeySet2.TryGetRandomElement(out var o_RandomKey2)
                        && !PassiveSkillIndexTable.ContainsKey(o_RandomKey2))
                    {
                        var upperBound = PassiveActionDataTableQuery.GetInstanceUnsafe.GetRecord(o_RandomKey2).LevelUpperBound;
                        PassiveSkillIndexTable.Add(o_RandomKey2, Mathf.Min(lowerBound, upperBound));
                    }

                    if (PassiveSkillIndexTable.Count < GameEntityTool.DefaultPassiveCountUpperBound
                        && Random.value < possibilityC
                        && passiveKeySet3.TryGetRandomElement(out var o_RandomKey3)
                        && !PassiveSkillIndexTable.ContainsKey(o_RandomKey3))
                    {
                        var upperBound = PassiveActionDataTableQuery.GetInstanceUnsafe.GetRecord(o_RandomKey3).LevelUpperBound;
                        PassiveSkillIndexTable.Add(o_RandomKey3, Mathf.Min(lowerBound, upperBound));
                    }
                }
                */

                var flagBuff = PassiveSkillIndexTable.ToDictionary(kv => kv.Key, kv => false);
                for (var i = 0; i < extraLevel; i++)
                {
                    var randomKey = PassiveSkillIndexTable.GetRandomElement().Key;
                    var upperBound = PassiveSkillDataTableQuery.GetInstanceUnsafe.GetRecord(randomKey).LevelUpperBound;
                    if (PassiveSkillIndexTable[randomKey] < upperBound)
                    {
                        PassiveSkillIndexTable[randomKey]++;
                    }
                    else
                    {
                        i--;
                        flagBuff[randomKey] = true;

                        if (flagBuff.All(kv => kv.Value))
                        {
                            break;
                        }
                    }
                }
                
                var latentList = componentRecord.LatentAbilityIndexList;
                if (latentList.CheckCollectionSafe())
                {
                    foreach (var latentIndex in latentList)
                    {
                        EinArmsOpenFlag.Add(latentIndex, false);
                    }
                }
            }
        }
        
        public AccountCharacterData(int p_Index, GameEntityItemTool.ItemRankType p_Type, int p_Value, float p_Rate, Dictionary<int, int> p_SkillTable, Dictionary<int, bool> p_EinTable)
        {
            UnitSpawnDataTableIndex = p_Index;
            RankType = p_Type;
            Level = p_Value;
            Exp = p_Rate;
            PassiveSkillIndexTable = p_SkillTable??new Dictionary<int, int>();
            EinArmsOpenFlag = p_EinTable??new Dictionary<int, bool>();
        }

        #endregion
        
        #region <Methods>

        public UnitSpawnDataTable.TableRecord GetSpawnRecord()
        {
            return UnitSpawnDataTable.GetInstanceUnsafe.GetRecord(UnitSpawnDataTableIndex);
        }
        
        #endregion
    }
}