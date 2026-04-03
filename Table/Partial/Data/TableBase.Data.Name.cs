namespace k514
{
    public partial class TableBase<Table, Meta, Key, Record>
    {
        #region <Fields>

        /// <summary>
        /// 클래스가 참조할 기본 테이블 이름
        /// </summary>
        private string _DefaultTableName;
        
        /// <summary>
        /// 테이블 생성등에 사용할 기본 브랜치
        /// </summary>
        private string _BranchHeader;
        
        /// <summary>
        /// 테이블 별명
        /// </summary>
        private string _MainTableName;
        
        #endregion
        
        #region <Callbacks>

        private void OnCreateTableName()
        {
            _DefaultTableName = GetType().Name;
            _MainTableName = _DefaultTableName;
        }

        #endregion
        
        #region <Methods>
                
        /// <summary>
        /// 해당 테이블의 기본 파일명을 리턴하는 메서드
        /// </summary>
        private string GetDefaultTableFileName()
        {
            return _DefaultTableName;
        }
        
        protected virtual string GetMainTableFileName()
        {
            return _MainTableName;
        }

        /// <summary>
        /// 테이블 별명을 지정하는 메서드
        /// 확장자를 제외한 이름을 넘겨야 한다.
        /// null이나 공백을 넘겨받는 경우 리셋된다.
        /// </summary>
        private void SetTableName(string p_TableName)
        {
            if (string.IsNullOrEmpty(p_TableName))
            {
                _TableStateFlag.RemoveFlag(TableTool.TableStateFlag.HasAlterTableName);
                _MainTableName = _DefaultTableName;
            }
            else
            {
                _TableStateFlag.AddFlag(TableTool.TableStateFlag.HasAlterTableName);
                _MainTableName = p_TableName;
            }
        }

        /// <summary>
        /// 테이블 상위 경로 헤더를 지정하는 메서드
        /// 헤더 뒤에는 / 가 붙어야 한다.
        /// null이나 공백을 넘겨받는 경우 리셋된다.
        /// </summary>
        protected void SetBranchHeader(string p_BranchHeader)
        {
            if (string.IsNullOrEmpty(p_BranchHeader))
            {
                _TableStateFlag.RemoveFlag(TableTool.TableStateFlag.HasBranchHeader);
                _BranchHeader = null;
            }
            else
            {
                _TableStateFlag.AddFlag(TableTool.TableStateFlag.HasBranchHeader);
                _BranchHeader = p_BranchHeader;
            }
        }

        /// <summary>
        /// 해당 테이블 파일 타입
        /// </summary>
        public virtual TableTool.TableFileType GetTableFileType()
        {
            return TableTool.TableFileType.Xml;
        }

        /// <summary>
        /// 테이블 상위 경로 헤더를 리턴하는 메서드
        /// </summary>
        public string GetBranchHeader()
        {
            return _TableStateFlag.HasAnyFlagExceptNone(TableTool.TableStateFlag.HasBranchHeader)
                ? _BranchHeader
                : string.Empty;
        }

        /// <summary>
        /// 해당 테이블의 파일명을 리턴하는 메서드
        /// </summary>
        public string GetTableFileName(TableTool.TableNameQueryType p_TableNameQueryFlag)
        {
            var branchHeader = p_TableNameQueryFlag.HasAnyFlagExceptNone(TableTool.TableNameQueryType.WithBranchHeader)
                                ? GetBranchHeader()
                                : string.Empty;
            var tableName = p_TableNameQueryFlag.HasAnyFlagExceptNone(TableTool.TableNameQueryType.WithMainTableName)
                                ? GetMainTableFileName()
                                : GetDefaultTableFileName();

            var extension = string.Empty;
            switch(p_TableNameQueryFlag)
            {
                case var _ when p_TableNameQueryFlag.HasAnyFlagExceptNone(TableTool.TableNameQueryType.WithByteExtension):
                    {
                        extension =  TableTool.BYTES_EXT;
                        break;
                    }
                case var _ when p_TableNameQueryFlag.HasAnyFlagExceptNone(TableTool.TableNameQueryType.WithTableExtension):
                    {
                        extension = GetTableFileType().GetTableExtension();
                        break;
                    }
            }

            return $"{branchHeader}{tableName}{extension}";
        }

        #endregion
    }
}