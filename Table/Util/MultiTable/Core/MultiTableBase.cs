using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using xk514;

namespace k514
{
    /// <summary>
    /// 특정한 멀티테이블에 대한 쿼리 기능을 제공하는 싱글톤
    /// 멀티테이블은 클래스 이름의 placeHolder 때문에 외부에 기능을 제공하기 번거롭고
    /// 자기 자신을 대상으로 한 쿼리함수는 직관적이지 않기 때문에 해당 싱글톤이 기능 제공을 대신하고 있다.
    /// </summary>
    public abstract partial class MultiTableBase<This, Key, Meta, Label, TableBridge, RecordBridge> : AsyncSingleton<This>, IMultiTable<Key, Meta, Label, TableBridge, RecordBridge>
        where This : MultiTableBase<This, Key, Meta, Label, TableBridge, RecordBridge>, new()
        where Meta : TableMetaData
        where Label : struct, Enum
        where TableBridge : class, ITableBridge<Key, Meta, RecordBridge>, ITableBridgeLabel<Label>
        where RecordBridge : class, ITableRecord
    {
        #region <Fields>
        
        /// <summary>
        /// 라벨 별 테이블 컬렉션
        /// </summary>
        protected Dictionary<Label, List<TableBridge>> _LabelTableListTable;

        #endregion

        #region <Callbacks>

        protected override async UniTask OnCreated(CancellationToken p_CancellationToken)
        {
            _LabelTableListTable = new Dictionary<Label, List<TableBridge>>();
            var enumerator = EnumFlag.GetEnumEnumerator<Label>(EnumFlag.GetEnumeratorType.ExceptNone);
            foreach (var label in enumerator)
            {
                _LabelTableListTable.Add(label, new List<TableBridge>());
            }
            
            await UniTask.CompletedTask;
        }

        protected override async UniTask OnInitiate(CancellationToken p_CancellationToken)
        {
            var tableBridgeType = typeof(TableBridge);
            var subTableTypeSet = tableBridgeType.IsGenericType ? 
                tableBridgeType.GetGenericTypeDefinition().GetSubTypeSet() 
                : tableBridgeType.GetSubTypeSet();
            
            var (valid, result) = await SingletonTool.CreateSingletonAsync(subTableTypeSet, MultiTaskMode.Sequence, p_CancellationToken);
            if (valid)
            {
                foreach (var subTable in result)
                {
                    if (subTable is TableBridge c_SubTable)
                    {
                        SystemBoot.OnSingletonControlInterrupted(c_SubTable);
                        JoinTable(c_SubTable);
                    }
                }

                SortTable();
            }
            else
            {
                Dispose();
            }
        }

        protected override void OnDisposeSingleton()
        {
            if (!ReferenceEquals(null, _LabelTableListTable))
            {
                foreach (var subTableListKV in _LabelTableListTable)
                {
                    var subTableList = subTableListKV.Value;
                    foreach (var subTable in subTableList)
                    {
                        subTable.Dispose();
                    }
                    subTableList.Clear();
                }
                _LabelTableListTable.Clear();
            }
            
            base.OnDisposeSingleton();
        }

        #endregion

        #region <Methods>

        private void JoinTable(TableBridge p_Table)
        {
            var labelType = p_Table.TableLabel;
#if UNITY_EDITOR
            try
            {
                var targetList = _LabelTableListTable[labelType];
                if (targetList.Contains(p_Table))
                {
    #if APPLY_PRINT_LOG
                    if (CustomDebug.CustomDebugLogFlag.PrintTableLog.HasOpen())
                    {
                        CustomDebug.LogError($"[{p_Table.GetType().Name}] 테이블의 라벨 [{labelType}] 멀티테이블로 삽입되는 과정에서 중복키 오류가 발생했습니다.");
                    }
    #endif
                }
                else
                {
                    targetList.Add(p_Table);
                }
            }
    #if APPLY_PRINT_LOG
            catch (Exception e)
            {
                if (CustomDebug.CustomDebugLogFlag.PrintTableLog.HasOpen())
                {
                    CustomDebug.LogError(($"[{p_Table.GetType().Name}] 테이블의 라벨 [{labelType}] 멀티테이블로 삽입되는 과정에서 오류가 발생했습니다.", e));
                }
                throw;
            }
    #else
            catch
            {
                throw;
            }
    #endif
#else
            var targetList = _LabelTableSet[labelType];
            if (targetList.Contains(p_Table))
            {
            }
            else
            {
                targetList.Add(p_Table);
            }
#endif
        }

        protected virtual void SortTable()
        {
        }

#if UNITY_EDITOR
        public virtual void PrintTable()
        {
            Debug.LogError($"*** {GetType().Name} List ***");
            foreach (var subTableSet in _LabelTableListTable)
            {
                var tableLabel = subTableSet.Key;
                var subTable = subTableSet.Value;
                if (subTable.Count > 0)
                {
                    Debug.LogError($"[Label : {tableLabel}]");
                    foreach (var table in subTable)
                    {
                        Debug.LogError($"{table.GetTableFileName(TableTool.TableNameQueryType.None)} [Record Count : {table.GetCurrentKeyEnumerator().Count}]");
                    }
                }
            }
            Debug.LogError("*************************");
        }
#endif
        
        #endregion
    }
    
    public abstract partial class MultiTableIndexBase<This, Meta, Label, TableBridge, RecordBridge> : MultiTableBase<This, int, Meta, Label, TableBridge, RecordBridge>
        where This : MultiTableIndexBase<This, Meta, Label, TableBridge, RecordBridge>, new()
        where Meta : TableMetaData
        where Label : struct, Enum
        where TableBridge : class, ITableIndexBridge<Meta, RecordBridge>, ITableBridgeLabel<Label>
        where RecordBridge : class, ITableRecord
    {
        protected override void SortTable()
        {
            var enumerator = EnumFlag.GetEnumEnumerator<Label>(EnumFlag.GetEnumeratorType.ExceptNone);
            foreach (var label in enumerator)
            {
                var labelTableSet = _LabelTableListTable[label];
                labelTableSet.Sort((left, right) => left.StartIndex.CompareTo(right.StartIndex));
            }
        }
    }
}