#if UNITY_EDITOR

using System.IO;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace k514
{
    public partial class TableModifier
    {
        /// <summary>
        /// 테이블 파일을 지우는 메서드
        /// </summary>
        public async UniTask DeleteTable(ITable p_TableBase, DataIOTool.WriteType p_WriteType, string p_TargetRootPath, CancellationToken p_CancellationToken)
        {
            p_CancellationToken.ThrowIfCancellationRequested();
            
            switch (p_WriteType)
            {
                // Overlap, BackUp 타입으로 호출시 각각 대응하는 이름의 테이블 파일을 찾아 제거한다.
                case DataIOTool.WriteType.Overlap:
                case DataIOTool.WriteType.BackUp:
                {
                    var tableDirectory = await GetTableWriteFullPath(p_WriteType, p_TargetRootPath, p_TableBase, p_CancellationToken);
                    if (File.Exists(tableDirectory))
                    {
                        File.Delete(tableDirectory);
                    }
                    break;
                }
                // CopyWithNumbering 타입으로 호출시, 넘버링 카피 파일을 전부 제거한다.
                case DataIOTool.WriteType.CopyWithNumbering:
                {
                    var tableFormat = p_TableBase.GetTableFileType();
                    var tableNameWithOutExt = p_TableBase.GetTableFileName(TableTool.TableNameQueryType.WithBranchHeaderMain);
                    var tableExt = tableFormat.GetTableExtension();

                    if (Directory.Exists(p_TargetRootPath))
                    {
                        var backUpFilePattern = SystemTool.GetNumberingFileRegex(tableNameWithOutExt, tableExt);
                        var fileNameSet = Directory.GetFiles(p_TargetRootPath, backUpFilePattern);
                        foreach (var fileName in fileNameSet)
                        {
                            if (File.Exists(fileName))
                            {
                                File.Delete(fileName);
                            }
                        }
                    }
                }
                    break;
            }
        }
    }
}

#endif