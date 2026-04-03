using System;
using k514.Mono.Feature;

namespace k514.Mono.Common
{
    /// <summary>
    /// 프로젝터 컴포넌트 테이블 클래스의 브릿지 인터페이스
    /// </summary>
    public interface IProjectorComponentDataTableBridge<out RecordBridge> : IWorldObjectComponentDataTableBridge<RecordBridge>, ITableBridgeLabel<ProjectorComponentDataTableQuery.TableLabel>
    {
    }

    /// <summary>
    /// 프로젝터 컴포넌트 테이블 레코드 클래스의 브릿지 인터페이스
    /// </summary>
    public interface IProjectorComponentDataTableRecordBridge : IWorldObjectComponentDataTableRecordBridge
    {
        /// <summary>
        /// 페이드 인 시간
        /// </summary>
        float FadeInTime { get; }

        /// <summary>
        /// 페이드 연출 이후, 투사 지속 시간
        /// </summary>
        float ProjectTime { get; }

        /// <summary>
        /// 페이드 아웃 시간
        /// </summary>
        float FadeOutTime { get; }

        /// <summary>
        /// 스케일 애니메이션 타입
        /// </summary>
        ProjectorTool.ProjectorScaleProgressType ProjectorScaleProgressType { get; }
    }
    
    /// <summary>
    /// 프로젝터 컴포넌트 테이블 클래스의 기저 클래스
    /// </summary>
    public abstract class ProjectorComponentDataTable<Table, Record> : WorldObjectComponentDataTable<Table, Record, IProjectorComponentDataTableRecordBridge>, IProjectorComponentDataTableBridge<IProjectorComponentDataTableRecordBridge>
        where Table : ProjectorComponentDataTable<Table, Record>, new() 
        where Record : ProjectorComponentDataTable<Table, Record>.ProjectorComponentDataTableRecord, new()
    {
        #region <Fields>

        protected ProjectorComponentDataTableQuery.TableLabel _ProjectorComponentLabel;
        ProjectorComponentDataTableQuery.TableLabel ITableBridgeLabel<ProjectorComponentDataTableQuery.TableLabel>.TableLabel => _ProjectorComponentLabel;

        #endregion

        #region <Record>
        
        /// <summary>
        /// 프로젝터 컴포넌트 테이블 레코드 클래스의 기저 클래스
        /// </summary>
        [Serializable]
        public abstract class ProjectorComponentDataTableRecord : WorldObjectComponentDataTableRecord, IProjectorComponentDataTableRecordBridge
        {
            #region <Fields>

            public float FadeInTime { get; protected set; }
            public float ProjectTime { get; protected set; }
            public float FadeOutTime { get; protected set; }
            public ProjectorTool.ProjectorScaleProgressType ProjectorScaleProgressType { get; protected set; }

            #endregion

            #region <Callbacks>

            protected override void TryInitiateFallbackComponent(Table p_Self)
            {
                var tryLabelType = p_Self._ProjectorComponentLabel;
                
                switch (tryLabelType)
                {
                    case ProjectorComponentDataTableQuery.TableLabel.SingleSprite:
                        MainComponentType = typeof(SingleSpriteProjector);
                        break;
                    case ProjectorComponentDataTableQuery.TableLabel.MultiSprite:
                        MainComponentType = typeof(MultiSpriteProjector);
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
            
            _WorldObjectComponentLabel = WorldObjectComponentDataTableQuery.TableLabel.Projector;
        }
        
        #endregion
    }
}