#if UNITY_EDITOR

using System.IO;
using System.Reflection;
using System.Threading;
using System.Xml;
using Cysharp.Threading.Tasks;

namespace k514
{
    /// <summary>
    /// 테이블 상태를 파일로 저정하는 기능을 수행하는 매니저 클래스
    /// </summary>
    public partial class TableModifier : AsyncSingleton<TableModifier>
    {
        #region <Fields>

        private XmlWriterSettings xmlWriterSettings;
        
        #endregion

        #region <Callbacks>

        protected override async UniTask OnCreated(CancellationToken p_CancellationToken)
        {
            xmlWriterSettings = 
                new XmlWriterSettings
                {
                    Indent = true,           // 들여쓰기 활성화
                    IndentChars = "    ",    // 공백 4칸
                    NewLineChars = "\n",     // (선택) 줄바꿈
                    NewLineOnAttributes = false
                };
            
            await UniTask.CompletedTask;
        }

        protected override async UniTask OnInitiate(CancellationToken p_CancellationToken)
        {
            await UniTask.CompletedTask;
        }

        #endregion
        
        #region <Methods>

        public async UniTask<string> GetTableWriteFullPath(DataIOTool.WriteType p_WriteType, string p_TryRootPath, ITable p_Table, CancellationToken p_Cancellation)
        {
            var tableNameExceptExt = p_Table.GetTableFileName(TableTool.TableNameQueryType.WithMainTableName);
            var tableExt = p_Table.GetTableFileType().GetTableExtension();
                
            return await SystemTool.TryCheckGetExportFilePath
            (
                p_TryRootPath,
                tableNameExceptExt,
                tableExt,
                p_WriteType,
                p_Cancellation
            );
        }
        
        #endregion
    }
}

#endif