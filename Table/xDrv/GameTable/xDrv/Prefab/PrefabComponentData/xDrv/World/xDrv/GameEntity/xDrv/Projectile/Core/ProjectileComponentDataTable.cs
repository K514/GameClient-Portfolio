using System;
using k514.Mono.Feature;

namespace k514.Mono.Common
{
    /// <summary>
    /// 투사체 컴포넌트 테이블 클래스의 브릿지 인터페이스
    /// </summary>
    public interface IProjectileComponentDataTableBridge<out RecordBridge> : IGameEntityComponentDataTableBridge<RecordBridge>, ITableBridgeLabel<ProjectileComponentDataTableQuery.TableLabel>
    {
    }

    /// <summary>
    /// 투사체 컴포넌트 테이블 레코드 클래스의 브릿지 인터페이스
    /// </summary>
    public interface IProjectileComponentDataTableRecordBridge : IGameEntityComponentDataTableRecordBridge
    {
    }
    
    /// <summary>
    /// 투사체 컴포넌트 테이블 클래스의 기저 클래스
    /// </summary>
    public abstract class ProjectileComponentDataTable<Table, Record, RecordBridge> : GameEntityComponentDataTable<Table, Record, RecordBridge>, IProjectileComponentDataTableBridge<RecordBridge>
        where Table : ProjectileComponentDataTable<Table, Record, RecordBridge>, new() 
        where Record : ProjectileComponentDataTable<Table, Record, RecordBridge>.ProjectileComponentDataTableRecord, RecordBridge, new()
        where RecordBridge : class, IProjectileComponentDataTableRecordBridge
    {
        #region <Fields>

        protected ProjectileComponentDataTableQuery.TableLabel _ProjectileComponentLabel;
        ProjectileComponentDataTableQuery.TableLabel ITableBridgeLabel<ProjectileComponentDataTableQuery.TableLabel>.TableLabel => _ProjectileComponentLabel;

        #endregion

        #region <Record>

        /// <summary>
        /// 투사체 컴포넌트 테이블 레코드 클래스의 기저 클래스
        /// </summary>
        [Serializable]
        public abstract class ProjectileComponentDataTableRecord : GameEntityComponentDataTableRecord, IProjectileComponentDataTableRecordBridge
        {
            #region <Callbacks>
            
            protected override void TryInitiateFallbackComponent(Table p_Self)
            {
                var tryLabelType = p_Self._ProjectileComponentLabel;
                
                switch (tryLabelType)
                {
                    case ProjectileComponentDataTableQuery.TableLabel.Default:
                        MainComponentType = typeof(DefaultProjectile);
                        break;
                }
            }
            
            #endregion
        }
        
        #endregion
                                                        
        #region <Callbacks>
        
        protected override void OnCreateTableBridge()
        {
            base.OnCreateTableBridge();
            
            _GameEntityComponentLabel = GameEntityComponentDataTableQuery.TableLabel.Projectile;
        }
        
        #endregion
    }
}