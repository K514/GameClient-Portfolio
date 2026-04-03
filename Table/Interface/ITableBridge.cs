using System;

namespace k514
{
    public interface ITableBridgeLabel<Label> where Label : struct, Enum
    {
        Label TableLabel { get; }
    }

    public interface ITableRecordBridgeQuery<Key, out RecordBridge>
    {
        RecordBridge GetRecordBridge(Key p_Key);
        RecordBridge GetRecordBridgeOrFallback(Key p_Key);
        RecordBridge GetFirstRecordBridge();
        RecordBridge GetFallbackRecordBridge();
        bool TryGetRecordBridge<Bridge>(Key p_Key, out Bridge o_Record) where Bridge : class;
        bool TryGetFirstRecordBridge<Bridge>(out Bridge o_Record) where Bridge : class;
        bool TryGetFallbackRecordBridge<Bridge>(out Bridge o_Record) where Bridge : class;
    }
    
    /// <summary>
    /// 멀티 테이블은 서로 다른 테이블과 레코드를, 하나의 타입으로 다루기 위해 브릿지 인터페이스로 추상화를 하는데
    /// 해당 테이블의 경우 내부의 레코드의 서브 타입을 알고 있어야 쿼리 기능을 제공할 수 있으므로 TableInterface<RecordInterface> 꼴로 정의된다.
    /// 따라서, TableInterface<RecordInterface1>, TableInterface<RecordInterface2>, ... 와 같은 variant 타입을 하나로 추상화 시켜야 해서
    /// 공변성 키워드를 사용하여 TableInterface<out RecordInterface> 꼴로 정의하고 있다.
    /// </summary>
    public interface ITableBridge<Key, Meta, out RecordBridge> : ITable<Key>, ITableRecordBridgeQuery<Key, RecordBridge>, ITableMetaQuery<Meta> 
        where Meta : TableMetaData
    {
        bool IsBridged(Key p_Key);
    }
}