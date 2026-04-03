using System;

namespace k514.Mono.Common
{
    /// <summary>
    /// 월드 오브젝트 컴포넌트 테이블 클래스의 브릿지 인터페이스
    /// </summary>
    public interface IWorldObjectComponentDataTableBridge<out RecordBridge> : IPrefabComponentDataTableBridge<RecordBridge>, ITableBridgeLabel<WorldObjectComponentDataTableQuery.TableLabel>
    {
    }

    /// <summary>
    /// 월드 오브젝트 컴포넌트 테이블 레코드 클래스의 브릿지 인터페이스
    /// </summary>
    public interface IWorldObjectComponentDataTableRecordBridge : IPrefabComponentDataTableRecordBridge
    {
    }
    
    /// <summary>
    /// 유니티 오브젝트 컴포넌트 테이블 클래스의 기저 클래스
    /// </summary>
    public abstract class WorldObjectComponentDataTable<Table, Record, RecordBridge> : PrefabComponentDataTable<Table, Record, RecordBridge>, IWorldObjectComponentDataTableBridge<RecordBridge>
        where Table : WorldObjectComponentDataTable<Table, Record, RecordBridge>, new() 
        where Record : WorldObjectComponentDataTable<Table, Record, RecordBridge>.WorldObjectComponentDataTableRecord, RecordBridge, new()
        where RecordBridge : class, IWorldObjectComponentDataTableRecordBridge
    {
        #region <Fields>

        protected WorldObjectComponentDataTableQuery.TableLabel _WorldObjectComponentLabel;
        WorldObjectComponentDataTableQuery.TableLabel ITableBridgeLabel<WorldObjectComponentDataTableQuery.TableLabel>.TableLabel => _WorldObjectComponentLabel;

        #endregion
        
        #region <Record>

        /// <summary>
        /// 월드 오브젝트 컴포넌트 테이블 레코드 클래스의 기저 클래스
        /// </summary>
        [Serializable]
        public abstract class WorldObjectComponentDataTableRecord : PrefabComponentDataTableRecord, IWorldObjectComponentDataTableRecordBridge
        {
        }

        #endregion
                        
        #region <Callbacks>
        
        protected override void OnCreateTableBridge()
        {
            _PrefabComponentLabel = PrefabComponentDataTableQuery.TableLabel.World;
        }
        
        #endregion
    }
}