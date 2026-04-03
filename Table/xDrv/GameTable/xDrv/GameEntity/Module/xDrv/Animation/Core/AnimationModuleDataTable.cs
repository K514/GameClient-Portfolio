using System;

namespace k514.Mono.Common
{
    public interface IAnimationModuleDataTableBridge : IGameEntityModuleDataTableBridge<IAnimationModuleDataTableRecordBridge>, ITableBridgeLabel<AnimationModuleDataTableQuery.TableLabel>
    {
    }    
    
    public interface IAnimationModuleDataTableRecordBridge : IGameEntityModuleDataTableRecordBridge
    {
    }
    
    public abstract class AnimationModuleDataTable<Table, Record> : GameEntryModuleDataTable<Table, TableMetaData, Record, IAnimationModuleDataTableRecordBridge>, IAnimationModuleDataTableBridge
        where Table : AnimationModuleDataTable<Table, Record>, new()
        where Record : AnimationModuleDataTable<Table, Record>.AnimationModuleTableRecord, new()
    {
        #region <Fields>

        protected AnimationModuleDataTableQuery.TableLabel _AnimationModuleTableLabel;
        AnimationModuleDataTableQuery.TableLabel ITableBridgeLabel<AnimationModuleDataTableQuery.TableLabel>.TableLabel => _AnimationModuleTableLabel;
        
        #endregion
        
        #region <Record>

        [Serializable]
        public abstract class AnimationModuleTableRecord : GameEntityModuleTableRecord, IAnimationModuleDataTableRecordBridge
        {
        }

        #endregion
        
        #region <Methods>

        protected override void OnCreateTableBridge()
        {
            _ModuleLabel = GameEntityModuleDataTableQuery.TableLabel.Animation;
        }

        #endregion
    }
}