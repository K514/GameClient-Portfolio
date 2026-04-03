using System;
using k514.Mono.Common;

namespace k514.Mono.Common
{
    /// <summary>
    /// 다중 스프라이트 프로젝터 컴포넌트 테이블 클래스
    /// </summary>
    public class MultiSpriteProjectorComponentDataTable : ProjectorComponentDataTable<MultiSpriteProjectorComponentDataTable, MultiSpriteProjectorComponentDataTable.TableRecord>
    {
        #region <Record>

        /// <summary>
        /// 다중 스프라이트 프로젝터 컴포넌트 테이블 레코드 클래스
        /// </summary>
        [Serializable]
        public class TableRecord : ProjectorComponentDataTableRecord
        {
            public UIxTool.AnimationSpriteType AnimationSpriteType { get; private set; }
        }
        #endregion

        #region <Callbacks>
        
        protected override void OnCreateTableBridge()
        {
            base.OnCreateTableBridge();

            _ProjectorComponentLabel = ProjectorComponentDataTableQuery.TableLabel.MultiSprite;
            StartIndex = 41_000_000;
            EndIndex = 42_000_000;
        }

        #endregion
    }
}