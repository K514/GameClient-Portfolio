using System;

namespace k514.Mono.Common
{
    /// <summary>
    /// 단일 스프라이트 프로젝터 컴포넌트 테이블 클래스
    /// </summary>
    public class SingleSpriteProjectorComponentDataTable : ProjectorComponentDataTable<SingleSpriteProjectorComponentDataTable, SingleSpriteProjectorComponentDataTable.TableRecord>
    {
        #region <Record>

        /// <summary>
        /// 단일 스프라이트 프로젝터 컴포넌트 테이블 레코드 클래스
        /// </summary>
        [Serializable]
        public class TableRecord : ProjectorComponentDataTableRecord
        {
        }
        
        #endregion

        #region <Callbacks>
        
        protected override void OnCreateTableBridge()
        {
            base.OnCreateTableBridge();

            _ProjectorComponentLabel = ProjectorComponentDataTableQuery.TableLabel.SingleSprite;
            StartIndex = 40_000_000;
            EndIndex = 41_000_000;
        }

        #endregion
    }
}