using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace k514.Mono.Common
{
    public interface IWorldObjectModelDataTableBridge<out RecordBridge> : IPrefabModelDataTableBridge<RecordBridge>, ITableBridgeLabel<WorldObjectModelDataTableQuery.TableLabel>
    {
    }

    public interface IWorldObjectModelDataTableRecordBridge : IPrefabModelDataTableRecordBridge
    {
    }
    
    /// <summary>
    /// 월드 오브젝트 모델 데이터 테이블 클래스의 추상 클래스
    /// </summary>
    public abstract class WorldObjectModelDataTable<Table, Record, RecordBridge> : PrefabModelDataTable<Table, Record, RecordBridge>, IWorldObjectModelDataTableBridge<RecordBridge>
        where Table : WorldObjectModelDataTable<Table, Record, RecordBridge>, new() 
        where Record : WorldObjectModelDataTable<Table, Record, RecordBridge>.WorldObjectModelDataTableRecord, RecordBridge, new()
        where RecordBridge : class, IWorldObjectModelDataTableRecordBridge
    {
        #region <Fields>

        protected WorldObjectModelDataTableQuery.TableLabel _WorldObjectModelLabel;
        WorldObjectModelDataTableQuery.TableLabel ITableBridgeLabel<WorldObjectModelDataTableQuery.TableLabel>.TableLabel => _WorldObjectModelLabel;

        #endregion

        #region <Record>

        /// <summary>
        /// 월드 오브젝트 모델 데이터 테이블 레코드 클래스의 추상 클래스
        /// </summary>
        [Serializable]
        public abstract class WorldObjectModelDataTableRecord : PrefabModelDataTableRecord, IWorldObjectModelDataTableRecordBridge
        {
        }
        
        #endregion
        
        #region <Callbacks>
        
        protected override void OnCreateTableBridge()
        {
            _PrefabModelLabel = PrefabModelDataTableQuery.TableLabel.WorldObject;
        }
        
        #endregion
    }
}