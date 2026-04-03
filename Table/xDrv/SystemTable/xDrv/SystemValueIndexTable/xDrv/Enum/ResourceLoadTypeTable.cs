using System.Threading;
using Cysharp.Threading.Tasks;

namespace k514
{
    public class ResourceLoadTypeTable : SystemValueIndexTable<ResourceLoadTypeTable, ResourceType, ResourceLoadType>
    {
        #region <Fields>
        
        /// <summary>
        /// 해당 프로젝트에서 하나라도 에셋번들 로드 타입의 리소스 타입을 가지는지 검증하는 메서드.
        /// 대부분의 프로젝트의 경우 True 이다.
        /// </summary>
        public bool HasBundleResource;

        #endregion

        #region <Callbacks>

        protected override async UniTask OnInitiate(CancellationToken p_CancellationToken)
        {
            // 테이블을 읽고 컬렉션을 초기화 시킨다.
            await base.OnInitiate(p_CancellationToken);

            var table = GetTable();
            foreach (var recordKV in table)
            {
                if (recordKV.Value.Value == ResourceLoadType.FromAssetBundle)
                {
                    HasBundleResource = true;
                    break;
                }
            }
        }

        #endregion

        #region <Methods>

        protected override async UniTask AddDefaultRecords(CancellationToken p_CancellationToken)
        {
            await base.AddDefaultRecords(p_CancellationToken);

            var enumerator = EnumFlag.GetEnumEnumerator<ResourceType>(EnumFlag.GetEnumeratorType.ExceptNone);
            foreach (var resourceType in enumerator)
            {
                await AddRecord(resourceType, false, p_CancellationToken, ResourceLoadType.FromUnityResource);
            }
        }

        #endregion
    }
}