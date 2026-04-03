using System.Threading;
using Cysharp.Threading.Tasks;
using k514.Mono.Common;
using UnityEngine;

namespace k514
{
    /// <summary>
    /// 텍스트의 토큰화, 어휘데이터화, 파싱 등의 기능을 수행하는 매니저 클래스
    /// </summary>
    public partial class TableLoader : AsyncSingleton<TableLoader>
    {
        #region <Fields>

        private ObjectPoolCluster<TableTool.TableFileType, ObjectCreateParams, ObjectActivateParams> _PoolCluster;

        #endregion
        
        #region <Callbacks>

        protected override async UniTask OnCreated(CancellationToken p_CancellationToken)
        {
            _PoolCluster = new ObjectPoolCluster<TableTool.TableFileType, ObjectCreateParams, ObjectActivateParams>();
            _PoolCluster.TryAddPool(TableTool.TableFileType.Xml, new ObjectPool<XmlTokenizer, ObjectCreateParams, ObjectActivateParams>(), 4);
            _PoolCluster.TryAddPool(TableTool.TableFileType.CSV, new ObjectPool<CsvTokenizer, ObjectCreateParams, ObjectActivateParams>(), 2);
            _PoolCluster.TryAddPool(TableTool.TableFileType.JSON, new ObjectPool<JsonTokenizer, ObjectCreateParams, ObjectActivateParams>(), 2);

            await UniTask.CompletedTask;
        }

        protected override async UniTask OnInitiate(CancellationToken p_CancellationToken)
        {
            await UniTask.CompletedTask;
        }

        #endregion
    }
}