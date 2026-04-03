using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace k514.Mono.Common
{
    public class EnhanceTable : GameTable<EnhanceTable, TableMetaData, int, EnhanceTable.TableRecord>
    {
        [Serializable]
        public class TableRecord : GameTableRecord
        {
            #region <Fields>

            /// <summary>
            /// 최대 레벨
            /// </summary>
            public int MaxLevel { get; private set; }
            
            /// <summary>
            /// 레벨업 당 강화율
            /// </summary>
            public float LevelUpBonusRate { get; private set; }
            
            /// <summary>
            /// 레벨업 당 강화율 역수
            /// </summary>
            [TableTool.TableRecordAttribute(TableTool.TableRecordAttributeType.Runtime)] 
            public float LevelUpBonusRateInv { get; private set; }
            
            /// <summary>
            /// 경험치 테이블
            /// </summary>
            public Dictionary<int, float> ExpTable { get; private set; }
            
            /// <summary>
            /// 경험치 역수 테이블
            /// </summary>
            [TableTool.TableRecordAttribute(TableTool.TableRecordAttributeType.Runtime)] 
            public Dictionary<int, float> InvExpTable { get; private set; }
            
            /// <summary>
            /// 경험치 누적 테이블
            /// </summary>
            [TableTool.TableRecordAttribute(TableTool.TableRecordAttributeType.Runtime)] 
            public Dictionary<int, float> AccumulateExpTable { get; private set; }
            
            #endregion

            #region <Callbacks>

            public override async UniTask OnRecordAdded(EnhanceTable p_Table, CancellationToken p_Cancellation)
            {
                await base.OnRecordAdded(p_Table, p_Cancellation);

                MaxLevel = Mathf.Max(1, MaxLevel);
                LevelUpBonusRateInv = 1f / LevelUpBonusRate;
   
                if (ExpTable.CheckCollectionSafe())
                {
                    var expTable = new Dictionary<int, float>();
                    var keyEnumerator = ExpTable.Keys;
                    var targetExp = 0f;
                    var tryLevel = 0;
                    foreach (var key in keyEnumerator)
                    {
                        while (tryLevel++ < MaxLevel + 1)
                        {
                            targetExp += ExpTable[key];
                            if (ExpTable.ContainsKey(tryLevel))
                            {
                                expTable.Add(tryLevel, targetExp);
                                break;
                            }
                            else
                            {
                                expTable.Add(tryLevel, targetExp);
                            }
                            
                        }
                    }
                    ExpTable = expTable;
                    InvExpTable = new Dictionary<int, float>();
                    foreach (var expKV in ExpTable)
                    {
                        var level = expKV.Key;
                        var neededExp = expKV.Value.ApplyLowerBound();
                        
                        InvExpTable.Add(level, 1f / neededExp);
                    }
                    
                    AccumulateExpTable = new Dictionary<int, float>();
                    var accExp = 0f;
                    foreach (var expKV in ExpTable)
                    {
                        var level = expKV.Key;
                        accExp += expKV.Value;
                        AccumulateExpTable.Add(level, accExp);
                    }
                }
            }
            
            #endregion

            #region <Methods>

            public override async UniTask SetRecord(int p_Key, object[] p_RecordField, CancellationToken p_Cancellation)
            {
                await base.SetRecord(p_Key, p_RecordField, p_Cancellation);
                
                MaxLevel = p_RecordField.GetElementSafe<int>(0);
                LevelUpBonusRate = p_RecordField.GetElementSafe<float>(1);
                ExpTable = p_RecordField.GetElementSafe<Dictionary<int, float>>(2);
            }

            public float GetNeededExp(int p_Level)
            {
                var level = Mathf.Clamp(p_Level, 1, MaxLevel);

                return ExpTable[level];
            }
            
            public float GetNeededExpInv(int p_Level)
            {
                var level = Mathf.Clamp(p_Level, 1, MaxLevel);

                return InvExpTable[level];
            }
            
            public float GetLevelUpStatusBonusRateDelta(int p_Delta)
            {
                if (p_Delta < 0)
                {
                    return LevelUpBonusRateInv.Pow(-p_Delta);
                }
                else
                {
                    return LevelUpBonusRate.Pow(p_Delta);
                }
            }
            
            public float GetLevelUpStatusBonusRate(int p_Level)
            {
                if (p_Level < 2)
                {
                    return 1f;
                }
                else
                {
                    return LevelUpBonusRate.Pow(p_Level - 1);
                }
            }

#if UNITY_EDITOR
            public void PrintEnhanceTable()
            {
                Debug.LogError($"Max Level : {MaxLevel}");
                Debug.LogError($"Bonus Rate Per 5 Level : {GetLevelUpStatusBonusRateDelta(5)}");
                Debug.LogError($"Bonus Rate Per 10 Level : {GetLevelUpStatusBonusRateDelta(10)}");
                Debug.LogError($"Bonus Rate Per 50 Level : {GetLevelUpStatusBonusRateDelta(50)}");
                Debug.LogError($"Bonus Rate Per 100 Level : {GetLevelUpStatusBonusRateDelta(100)}");
                for (var i = 0; i < 50; i++)
                {
                    Debug.LogError($"Need Exp {i} : {GetNeededExp(i)}");
                }
            }
#endif

            #endregion
        }

        #region <Methods>

        protected override async UniTask AddDefaultRecords(CancellationToken p_CancellationToken)
        {
            await base.AddDefaultRecords(p_CancellationToken);

            await AddRecord
            (
                0, false, p_CancellationToken, 
                100, 0.01f, new Dictionary<int, float> { {1, 100f} }
            );
        }

        #endregion
    }
}