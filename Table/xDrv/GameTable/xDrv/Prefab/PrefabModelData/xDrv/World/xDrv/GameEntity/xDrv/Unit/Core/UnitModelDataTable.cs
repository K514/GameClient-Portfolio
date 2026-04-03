using System;
using k514.Mono.Common;

namespace k514.Mono.Common
{
    public interface IUnitModelDataTableBridge<out RecordBridge> : IGameEntityModelDataTableBridge<RecordBridge>, ITableBridgeLabel<UnitModelDataTableQuery.TableLabel>
    {
    }

    public interface IUnitModelDataTableRecordBridge : IGameEntityModelDataTableRecordBridge
    {
        int AttachPointQueryIndex { get; }
        GameEntityTool.MaterialType UnitSkinType { get; }
    }
    
    public abstract class UnitModelDataTable<Table, Record, RecordBridge> : GameEntityModelDataTable<Table, Record, RecordBridge>, IUnitModelDataTableBridge<RecordBridge>
        where Table : UnitModelDataTable<Table, Record, RecordBridge>, new() 
        where Record : UnitModelDataTable<Table, Record, RecordBridge>.UnitModelDataTableRecord, RecordBridge, new()
        where RecordBridge : class, IUnitModelDataTableRecordBridge
    {
        #region <Fields>

        protected UnitModelDataTableQuery.TableLabel _UnitModelLabel;
        UnitModelDataTableQuery.TableLabel ITableBridgeLabel<UnitModelDataTableQuery.TableLabel>.TableLabel => _UnitModelLabel;

        #endregion
        
        #region <Record>

        [Serializable]
        public class UnitModelDataTableRecord : GameEntityModelDataTableRecord, IUnitModelDataTableRecordBridge
        {
            #region <Fields>

            /// <summary>
            /// 유닛 rig 오브젝트 탐색 테이블 인덱스
            /// </summary>
            public int AttachPointQueryIndex { get; protected set; }

            /// <summary>
            /// 스킨 타입
            /// </summary>
            public GameEntityTool.MaterialType UnitSkinType { get; protected set; }
            
            #endregion
        }
        
        #endregion

        #region <Callbacks>

        protected override void OnCreateTableBridge()
        {
            base.OnCreateTableBridge();
            
            _GameEntityModelLabel = GameEntityModelDataTableQuery.TableLabel.Unit;
        }

        #endregion
    }
}