using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using k514.Mono.Common;

namespace k514
{
    /// <summary>
    /// SceneConstantDataTable의 [Key, VariableIndex] 슈퍼키를 제공하여 레코드에 접근하도록 돕는 테이블 
    /// </summary>
    public class SceneEntryDataTable : GameTable<SceneEntryDataTable, TableMetaData, int, SceneEntryDataTable.TableRecord>
    {
        #region <Record>

        [Serializable]
        public class TableRecord : GameTableRecord
        {
            #region <Fields>
            
            /// <summary>
            /// SceneListTable 키 값
            /// </summary>
            public string SceneName { get;set; }
            
            /// <summary>
            /// 지정한 SceneConstantDataTable 레코드의 SceneVariableDataRecordList에서 참조할 인덱스
            /// </summary>
            public int SceneVariableIndex { get; set; }

            #endregion
            
            #region <Methods>

            public override async UniTask SetRecord(int p_Key, object[] p_RecordField, CancellationToken p_CancellationToken)
            {
                await base.SetRecord(p_Key, p_RecordField, p_CancellationToken);

                SceneName = p_RecordField.As<string>(0);
                SceneVariableIndex = p_RecordField.As<int>(1);
            }
            
            #endregion
        }

        #endregion

        #region <Callbacks>

        protected override void TryInitializeDependency()
        {
            base.TryInitializeDependency();
            
            _Dependencies.Add(typeof(SceneInfoQueryTable));
        }

        protected override async UniTask OnCreated(CancellationToken p_CancellationToken)
        {
            await base.OnCreated(p_CancellationToken);

            SetBranchHeader("Scene/SceneEntry/");
        }
        
        protected override async UniTask AddDefaultRecords(CancellationToken p_CancellationToken)
        {
            await base.AddDefaultRecords(p_CancellationToken);

            await AddRecord(SceneTool.LOBBY_SCENE_ENTRY_INDEX, false, p_CancellationToken, SceneTool.LOBBY_SCENE_NAME);
        }
        
        #endregion
    }
}