#if UNITY_EDITOR

using System.Threading;
using Cysharp.Threading.Tasks;

namespace k514
{
    public partial class TableBase<Table, Meta, Key, Record>
    {
        /// <summary>
        /// 현재 로드된 테이블의 상태를 바이너리 파일로 저장하는 메서드
        /// </summary>
        private async UniTask WriteBinaryTableImage(CancellationToken p_Token)
        {
            var tableImage = GetTableImage();
            tableImage.SerializeObject(GetByteTableFullPath());

            await UniTask.CompletedTask;
        }
    }
}

#endif
