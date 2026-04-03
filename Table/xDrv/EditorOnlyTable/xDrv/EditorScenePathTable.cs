#if UNITY_EDITOR

using System.Threading;
using Cysharp.Threading.Tasks;
using k514.Mono.Common;

namespace k514
{
    public class EditorScenePathTable : EditorOnlyTable<EditorScenePathTable, TableMetaData, SceneTool.EditorSceneType, EditorScenePathTable.TableRecord>
    {
        #region <Record>

        public class TableRecord : EditorOnlyTableRecord
        {
            #region <Fields>

            public string SceneFullPath { get; private set; }
            public string SceneName { get; private set; }

            #endregion

            #region <Callbacks>

            public override async UniTask OnRecordAdded(EditorScenePathTable p_Table, CancellationToken p_Cancellation)
            {
                await base.OnRecordAdded(p_Table, p_Cancellation);

                if (!string.IsNullOrWhiteSpace(SceneFullPath))
                {
                    SceneName = SceneFullPath.GetFileNameFromPath(true);
                }
            }

            #endregion
            
            #region <Methods>

            public override async UniTask SetRecord(SceneTool.EditorSceneType p_Key, object[] p_RecordField, CancellationToken p_CancellationToken)
            {
                await base.SetRecord(p_Key, p_RecordField, p_CancellationToken);
                
                SceneFullPath = p_RecordField.As<string>(0);
            }

            #endregion
        }

        #endregion

        #region <Methods>

        protected override async UniTask AddDefaultRecords(CancellationToken p_CancellationToken)
        {
            await base.AddDefaultRecords(p_CancellationToken);

            var enumerator = EnumFlag.GetEnumEnumerator<SceneTool.EditorSceneType>(EnumFlag.GetEnumeratorType.ExceptNone);
            foreach (var sceneType in enumerator)
            {
                switch (sceneType)
                {
                    case SceneTool.EditorSceneType.Dexter:
                        await AddRecord(sceneType, false, p_CancellationToken, "Assets/Scenes/EditorScenes/xDexter.unity");
                        break;
                }
            }
        }
        
        #endregion
    }
}

#endif