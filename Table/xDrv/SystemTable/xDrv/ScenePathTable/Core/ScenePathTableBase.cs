using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace k514
{
    public abstract class ScenePathTableBase<Table, Key> : SystemTable<Table, TableMetaData, Key, ScenePathTableBase<Table, Key>.ScenePathTableRecord>
        where Table : ScenePathTableBase<Table, Key>, new()
        where Key : struct, Enum
    {
        #region <Record>
        
        [Serializable]
        public class ScenePathTableRecord : SystemTableRecordBase
        {
            #region <Fields>

            public string SceneFullPath { get; private set; }
            public string SceneName { get; private set; }

            #endregion

            #region <Callbacks>

            public override async UniTask OnRecordAdded(Table p_Table, CancellationToken p_Cancellation)
            {
                await base.OnRecordAdded(p_Table, p_Cancellation);

                if (!string.IsNullOrWhiteSpace(SceneFullPath))
                {
                    SceneName = SceneFullPath.GetFileNameFromPath(true);
                }
            }

            #endregion
            
            #region <Methods>

            public override async UniTask SetRecord(Key p_Key, object[] p_RecordField, CancellationToken p_CancellationToken)
            {
                await base.SetRecord(p_Key, p_RecordField, p_CancellationToken);
                
                SceneFullPath = p_RecordField.As<string>(0);
            }

            #endregion
        }
        
        #endregion
    }
}