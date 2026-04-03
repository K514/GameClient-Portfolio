using System.Threading;
using Cysharp.Threading.Tasks;

namespace k514
{
    public class SystemMessageTable : SystemValueIndexTable<SystemMessageTable, SystemMessageTable.KeyType, string>
    {
        #region <Enums>

        public enum KeyType
        {
            Patch_CompareVersion,
            Patch_CompareBundle,
            Patch_DownloadFile,
            Patch_Terminate,
            
            Patch_Fail,
        }

        #endregion

        #region <Methods>

        protected override async UniTask AddDefaultRecords(CancellationToken p_CancellationToken)
        {
            await base.AddDefaultRecords(p_CancellationToken);

            var enumerator = EnumFlag.GetEnumEnumerator<KeyType>(EnumFlag.GetEnumeratorType.ExceptNone);
            foreach (var keyType in enumerator)
            {
                switch (keyType)
                {
                    case KeyType.Patch_CompareVersion:
                        await AddRecord(keyType, false, p_CancellationToken, "버전을 비교중입니다.");
                        break;
                    case KeyType.Patch_CompareBundle:
                        await AddRecord(keyType, false, p_CancellationToken, "번들을 검증중입니다.");
                        break;
                    case KeyType.Patch_DownloadFile:
                        await AddRecord(keyType, false, p_CancellationToken, "패치파일을 다운로드중입니다.");
                        break;
                    case KeyType.Patch_Terminate:
                        await AddRecord(keyType, false, p_CancellationToken, "패치모듈 종료중");
                        break;
                    case KeyType.Patch_Fail:
                        await AddRecord(keyType, false, p_CancellationToken, "패치에 실패했습니다.");
                        break;
                }
            }
        }

        #endregion
    }
}