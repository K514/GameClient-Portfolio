using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace k514.Mono.Common
{
    public partial class ActionBase
    {
        #region <Fields>

        /// <summary>
        /// 액션 타입 별로 분류된 액션 핸들러 리스트 테이블
        /// </summary>
        private Dictionary<ActionDataTableQuery.TableLabel, List<IActionEventHandler>> _ActionEventHandlerListTableByLabel;
             
        /// <summary>
        /// 액티브 스킬 핸들러 리스트
        /// </summary>
        private List<IActiveSkillEventHandler> _ActiveSkillHandlerList;

        /// <summary>
        /// 패시브 스킬 핸들러 리스트
        /// </summary>
        private List<IPassiveSkillEventHandler> _PassiveSkillHandlerList;
        
        /// <summary>
        /// 입력 코드 별로 분류된 액티브 스킬 핸들러 테이블
        /// </summary>
        private Dictionary<InputEventTool.TriggerKeyType, IActionEventHandler> _QuickCommandHandlerTable;

        /// <summary>
        /// 연속 커맨드 별로 분류된 액티브 스킬 핸들러 리스트 테이블
        /// </summary>
        private Dictionary<ActionTool.SequenceCommand, List<IActiveSkillEventHandler>> _SequenceCommandHandlerListTable;

        #endregion
        
        #region <Callbacks>

        private void OnCreateTable()
        {
            _ActionEventHandlerListTableByLabel = new Dictionary<ActionDataTableQuery.TableLabel, List<IActionEventHandler>>();
            _ActiveSkillHandlerList = new List<IActiveSkillEventHandler>();
            _PassiveSkillHandlerList = new List<IPassiveSkillEventHandler>();
            _QuickCommandHandlerTable = new Dictionary<InputEventTool.TriggerKeyType, IActionEventHandler>();
            _SequenceCommandHandlerListTable = new Dictionary<ActionTool.SequenceCommand, List<IActiveSkillEventHandler>>();
            {
                var enumerator = EnumFlag.GetEnumEnumerator<ActionDataTableQuery.TableLabel>(EnumFlag.GetEnumeratorType.ExceptNone);
                foreach (var actionLabelType in enumerator)
                {
                    _ActionEventHandlerListTableByLabel.Add(actionLabelType, new List<IActionEventHandler>());
                }
            }
            {
                var enumerator = ActionTool.__DEFAULT_TRIGGER_KEYSET;
                foreach (var actionTriggerType in enumerator)
                {
                    _QuickCommandHandlerTable.Add(actionTriggerType, null);
                }
            }
        }
        
        #endregion

        #region <Methods>
        
#if APPLY_PRINT_LOG
        public void PrintActionTable()
        {
            {
                Debug.LogError("*** Action ***");
                {
                    if (_ActionEventHandlerListTableByLabel.CheckCollectionSafe())
                    {
                        var hasNone = true;
                        foreach (var handlerListKV in _ActionEventHandlerListTableByLabel)
                        {
                            var handlerType = handlerListKV.Key;
                            var handlerList = handlerListKV.Value;
                            foreach (var handler in handlerList)
                            {
                                hasNone = false;
                                Debug.LogError($"ActionType : {handlerType} & Action Id : {handler.EventId}");
                            }
                        }

                        if (hasNone)
                        {
                            Debug.LogError("None");
                        }
                    }
                    else
                    {
                        Debug.LogError("None");
                    }
                }
                Debug.LogError("*******************");
            }
            {
                Debug.LogError("*** Active Skill ***");
                {
                    if (_ActiveSkillHandlerList.CheckCollectionSafe())
                    {
                        foreach (var handler in _ActiveSkillHandlerList)
                        {
                            Debug.LogError($"Skill Id : {handler.EventId}");
                        }
                    }
                    else
                    {
                        Debug.LogError("None");
                    }
                }
                Debug.LogError("*******************");
            }
            {
                Debug.LogError("*** Passive Skill ***");
                {
                    if (_PassiveSkillHandlerList.CheckCollectionSafe())
                    {
                        foreach (var handler in _PassiveSkillHandlerList)
                        {
                            Debug.LogError($"Skill Id : {handler.EventId}");
                        }
                    }
                    else
                    {
                        Debug.LogError("None");
                    }
                }
                Debug.LogError("*******************");
            }
            {
                Debug.LogError("*** Active Quick Command ***");
                if (_QuickCommandHandlerTable.CheckCollectionSafe())
                {
                    foreach (var handlerKV in _QuickCommandHandlerTable)
                    {
                        Debug.LogError($"QuickCommand : {handlerKV.Key}, Action Id : {handlerKV.Value.EventId}");
                    }
                }
                else
                {
                    Debug.LogError("None");
                }
                Debug.LogError("*******************");
            }
            {
                Debug.LogError("*** Active Seq Command ***");
                if (_SequenceCommandHandlerListTable.CheckCollectionSafe())
                {
                    var hasNone = true;
                    foreach (var handlerListKV in _SequenceCommandHandlerListTable)
                    {
                        var handlerList = handlerListKV.Value;
                        foreach (var handler in handlerList)
                        {
                            hasNone = false;
                            Debug.LogError($"Sequence : {handlerListKV.Key}, Action Id : {handler.EventId}");
                        }
                    }

                    if (hasNone)
                    {
                        Debug.LogError("None");
                    }
                }
                else
                {
                    Debug.LogError("None");
                }
                Debug.LogError("*******************");
            }
        }
#endif
        
        #endregion
    }
}