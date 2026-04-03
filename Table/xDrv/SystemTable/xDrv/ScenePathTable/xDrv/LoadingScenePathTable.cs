using System.Threading;
using Cysharp.Threading.Tasks;
using k514.Mono.Common;

namespace k514
{
    public class LoadingScenePathTable : ScenePathTableBase<LoadingScenePathTable, SceneTool.LoadingSceneType>
    {
        #region <Methods>

        protected override async UniTask AddDefaultRecords(CancellationToken p_CancellationToken)
        {
            await base.AddDefaultRecords(p_CancellationToken);
            
            var enumerator = EnumFlag.GetEnumEnumerator<SceneTool.LoadingSceneType>(EnumFlag.GetEnumeratorType.ExceptNone);
            foreach (var loadingSceneType in enumerator)
            {
                switch (loadingSceneType)
                {
                    case SceneTool.LoadingSceneType.Black:
                        await AddRecord(loadingSceneType, false, p_CancellationToken, "Assets/Scenes/LoadingScenes/LoadingScene_B.unity");
                        break;
                    case SceneTool.LoadingSceneType.SolidImage:
                        await AddRecord(loadingSceneType, false, p_CancellationToken, "Assets/Scenes/LoadingScenes/LoadingScene_SL.unity");
                        break;
                }
            }
        }

        #endregion
    }
}