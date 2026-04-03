using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace k514
{
    public abstract class SystemValueIndexTable<Table, Key, RecordValue> : SystemTable<Table, TableMetaData, Key, SystemValueIndexTable<Table, Key, RecordValue>.SystemValueIndexTableRecord>
        where Table : SystemValueIndexTable<Table, Key, RecordValue>, new()
        where Key : struct, Enum
    {
        #region <Consts>

        public static RecordValue GetValue(Key p_Key)
        {
            // 시스템 변수는 특성상 시스템 초기화 시에 재귀호출되는 경우가 많으므로 UnSafe 함수를 통해 싱글톤 인스턴스에 접근한다.
            if (GetInstanceUnsafe.TryGetRecord(p_Key, out var o_Record))
            {
                return o_Record.Value;
            }
            else
            {
                return default;
            }
        }

        #endregion
        
        #region <Record>
        
        [Serializable]
        public class SystemValueIndexTableRecord : SystemTableRecordBase
        {
            #region <Fields>

            public RecordValue Value { get; private set; }

            #endregion

            #region <Methods>

            public override async UniTask SetRecord(Key p_Key, object[] p_RecordField, CancellationToken p_CancellationToken)
            {
                await base.SetRecord(p_Key, p_RecordField, p_CancellationToken);
                
                Value = (RecordValue) p_RecordField.GetElementSafe(0);
            }

            #endregion
        }
        
        #endregion
    }
}