using System.Threading;
using Cysharp.Threading.Tasks;

namespace k514
{
    public class SystemIntValueTable : SystemValueIndexTable<SystemIntValueTable, SystemIntValueTable.KeyType, int>
    {
        #region <Enums>

        public enum KeyType
        {
            CommandExpireMsec,
            CommandMaxCapacity,
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
                    case KeyType.CommandExpireMsec:
                        await AddRecord(keyType, false, p_CancellationToken, 500);
                        break;
                    case KeyType.CommandMaxCapacity:
                        await AddRecord(keyType, false, p_CancellationToken, 4);
                        break;
                }
            }
        }

        #endregion
    }
}