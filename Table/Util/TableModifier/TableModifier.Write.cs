#if UNITY_EDITOR

using System.Threading;
using Cysharp.Threading.Tasks;

namespace k514
{
    public partial class TableModifier
    {
        /// <summary>
        /// 지정한 브랜치에 테이블을 xml파일로 변환하여 저장하는 메서드
        /// </summary>
        public async UniTask<string> WriteTable<Meta, Key, Record>(ITable<Meta, Key, Record> p_TableBase, DataIOTool.WriteType p_WriteType, string p_RootPath, CancellationToken p_Cancellation) where Record : ITableRecord
            where Meta : TableMetaData
        {
            var writeFullPath = await GetTableWriteFullPath(p_WriteType, p_RootPath, p_TableBase, p_Cancellation);
            var tableType = p_TableBase.GetTableFileType();
            switch (tableType)
            {
                case TableTool.TableFileType.Xml:
                    await WriteXmlTable(writeFullPath, p_TableBase.GetTableImage(), p_Cancellation);
                    break;
                case TableTool.TableFileType.JSON:
                    await WriteJsonTable(writeFullPath, p_TableBase.GetTableImage(), p_Cancellation);
                    break;
                case TableTool.TableFileType.CSV:
                    break;
            }

            return writeFullPath;
        }
    }
}

#endif
