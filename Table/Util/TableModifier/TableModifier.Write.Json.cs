#if UNITY_EDITOR

using System.Threading;
using Cysharp.Threading.Tasks;
using System.IO;

namespace k514
{
    public partial class TableModifier
    {
        /// <summary>
        /// 지정한 절대경로에 테이블을 Json파일로 변환하여 저장하는 메서드
        /// </summary>
        private async UniTask WriteJsonTable<Meta, Key, Record>(string p_WriteFullPath, TableTool.TableDataImage<Meta, Key, Record> p_TableData, CancellationToken p_Cancellation) 
            where Record : ITableRecord
            where Meta : TableMetaData
        {
            var value = await WriteXmlTable(p_TableData, p_Cancellation);
            p_Cancellation.ThrowIfCancellationRequested();
            await File.WriteAllTextAsync(p_WriteFullPath, value, p_Cancellation);
        }
    }
}

#endif