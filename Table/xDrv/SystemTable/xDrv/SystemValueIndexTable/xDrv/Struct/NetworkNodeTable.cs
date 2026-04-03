using System.Threading;
using Cysharp.Threading.Tasks;

namespace k514
{
    public class NetworkNodeTable : SystemValueIndexTable<NetworkNodeTable, NetworkTool.NetworkNodeType, NetworkTool.UniformResourceIdentifier>
    {
        protected override async UniTask AddDefaultRecords(CancellationToken p_CancellationToken)
        {
            await base.AddDefaultRecords(p_CancellationToken);

            var enumerator = EnumFlag.GetEnumEnumerator<NetworkTool.NetworkNodeType>(EnumFlag.GetEnumeratorType.ExceptNone);
            foreach (var nodeType in enumerator)
            {
                switch (nodeType)
                {
                    case NetworkTool.NetworkNodeType.TestLocal :
                        await AddRecord(nodeType, false, p_CancellationToken, new NetworkTool.UniformResourceIdentifier("localhost", 8000));
                        break;
                    default:
                        await AddRecord(nodeType,false, p_CancellationToken, default(NetworkTool.UniformResourceIdentifier));
                        break;
                }
            }
        }
    }
}