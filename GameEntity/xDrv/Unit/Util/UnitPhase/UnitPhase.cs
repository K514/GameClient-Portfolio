using System;
using System.Collections.Generic;
using k514.Mono.Common;

namespace k514.Mono.Feature
{
    [Serializable]
    public struct UnitPhaseTerminateCondition
    {
        #region <Fields>

        public readonly UnitTool.UnitPhaseTerminateConditionType Type;
        public readonly float Value;

        #endregion

        #region <Constructor>

        public UnitPhaseTerminateCondition(UnitTool.UnitPhaseTerminateConditionType p_Type, float p_Value)
        {
            Type = p_Type;
            Value = p_Value;
        }

        #endregion
    }
    
    [Serializable]
    public class UnitPhase
    {
        #region <Fields>

        private Dictionary<UnitTool.UnitPhaseTerminateConditionType, UnitPhaseTerminateCondition> PhaseOverConditionTable;
        public readonly int PhaseNormalActionModuleIndex;
        public readonly int PhaseSuccessEventIndex;
        public readonly int PhaseFailEventIndex;
        public readonly UnitTool.UnitPhaseAttribute AttributeMask;

        #endregion

        #region <Indexor>

        public float this[UnitTool.UnitPhaseTerminateConditionType p_Type] => PhaseOverConditionTable[p_Type].Value;

        #endregion
        
        #region <Constructor>

        public UnitPhase(List<UnitPhaseTerminateCondition> p_List, int p_PhaseNormalActionModuleIndex)
        {
            PhaseOverConditionTable = new Dictionary<UnitTool.UnitPhaseTerminateConditionType, UnitPhaseTerminateCondition>();
            PhaseNormalActionModuleIndex = p_PhaseNormalActionModuleIndex;
            AttributeMask = UnitTool.UnitPhaseAttribute.None;

            if (p_List.CheckCollectionSafe())
            {
                foreach (var PhaseOverCondition in p_List)
                {
                    var type = PhaseOverCondition.Type;
                    switch (type)
                    {
                        case UnitTool.UnitPhaseTerminateConditionType.Duration:
                        {
                            PhaseOverConditionTable.Add(UnitTool.UnitPhaseTerminateConditionType.Duration, PhaseOverCondition);
                            AttributeMask.AddFlag(UnitTool.UnitPhaseAttribute.HasDuration);
                            break;
                        }
                        case UnitTool.UnitPhaseTerminateConditionType.HitCount:
                        {
                            PhaseOverConditionTable.Add(UnitTool.UnitPhaseTerminateConditionType.HitCount, PhaseOverCondition);
                            AttributeMask.AddFlag(UnitTool.UnitPhaseAttribute.HasHitCount);
                            break;
                        }
                        case UnitTool.UnitPhaseTerminateConditionType.HpRate:
                        {
                            PhaseOverConditionTable.Add(UnitTool.UnitPhaseTerminateConditionType.HpRate, PhaseOverCondition);
                            AttributeMask.AddFlag(UnitTool.UnitPhaseAttribute.HasHpRate);
                            break;
                        }
                    }
                }
            }
        }
        
        public UnitPhase(List<UnitPhaseTerminateCondition> p_List, int p_ActionModuleIndex, int p_PhaseSuccessEventIndex) : this(p_List, p_ActionModuleIndex)
        {
            PhaseSuccessEventIndex = p_PhaseSuccessEventIndex;
            AttributeMask.AddFlag(UnitTool.UnitPhaseAttribute.HasPhaseSuccessEvent);
        }
        
        public UnitPhase(List<UnitPhaseTerminateCondition> p_List, int p_ActionModuleIndex, int p_PhaseSuccessActionModuleIndex, int p_PhaseFailEventIndex) : this(p_List, p_ActionModuleIndex, p_PhaseSuccessActionModuleIndex)
        {
            PhaseFailEventIndex = p_PhaseFailEventIndex;
            AttributeMask.AddFlag(UnitTool.UnitPhaseAttribute.HasPhaseFailEvent);
        }

        #endregion
    }
}