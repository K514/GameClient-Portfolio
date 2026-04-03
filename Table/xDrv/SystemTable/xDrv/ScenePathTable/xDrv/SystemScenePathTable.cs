using System.Threading;
using Cysharp.Threading.Tasks;
using k514.Mono.Common;

namespace k514
{
    public class SystemScenePathTable : ScenePathTableBase<SystemScenePathTable, SceneTool.SystemSceneType>
    {
        #region <Methods>

        protected override async UniTask AddDefaultRecords(CancellationToken p_CancellationToken)
        {
            await base.AddDefaultRecords(p_CancellationToken);

            var enumerator = EnumFlag.GetEnumEnumerator<SceneTool.SystemSceneType>(EnumFlag.GetEnumeratorType.ExceptNone);
            foreach (var systemSceneType in enumerator)
            {
                switch (systemSceneType)
                {
                    case SceneTool.SystemSceneType.BootScene:
                        await AddRecord(systemSceneType, false, p_CancellationToken, "Assets/Scenes/SystemScenes/BootScene.unity");
                        break;
                    case SceneTool.SystemSceneType.PatchScene:
                        await AddRecord(systemSceneType, false, p_CancellationToken, "Assets/Scenes/SystemScenes/PatchScene.unity");
                        break;
                    case SceneTool.SystemSceneType.InitScene:
                        await AddRecord(systemSceneType, false, p_CancellationToken, "Assets/Scenes/SystemScenes/InitScene.unity");
                        break;
                    case SceneTool.SystemSceneType.TitleScene:
                        await AddRecord(systemSceneType, false, p_CancellationToken, "Assets/Scenes/SystemScenes/TitleScene.unity");
                        break;
                }
            }
        }

        #endregion
    }
}