using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using k514.Mono.Common;
using k514.Mono.Feature;
using UnityEngine;

namespace k514
{
    /// <summary>
    /// 하나의 씬 내부에서 설정값이 변해야 하는 경우에 관련 변수 값을 기술하는 테이블
    /// </summary>
    public class SceneVariableDataTable : GameTable<SceneVariableDataTable, TableMetaData, int, SceneVariableDataTable.TableRecord>
    {
        #region <Consts>

        public static readonly Type FallbackSceneEnvironmentType = typeof(FallbackSceneEnvironmentBase);

        #endregion
        
        #region <Record>

        [Serializable]
        public class TableRecord : GameTableRecord
        {
            #region <Fields>

            /// <summary>
            /// 씬 진입 좌표
            /// </summary>
            public SceneTool.SceneStartPreset SceneStartPreset { get; private set; }
  
            /// <summary>
            /// 씬 로케이션 메타
            /// </summary>
            public List<SceneTool.SceneLocationMeta> SceneLocationMetaSet { get; private set; }
     
            /// <summary>
            /// 해당 씬 환경 컴포넌트 타입
            /// </summary>
            public Type SceneEnvironmentType { get; private set; }


            #endregion

            #region <Callbacks>

            public override async UniTask OnRecordAdded(SceneVariableDataTable p_Table, CancellationToken p_Cancellation)
            {
                await base.OnRecordAdded(p_Table, p_Cancellation);

                if (ReferenceEquals(null, SceneLocationMetaSet))
                {
                    SceneLocationMetaSet = new List<SceneTool.SceneLocationMeta>(); 
                }
                
                if (ReferenceEquals(null, SceneEnvironmentType))
                {
                    SceneEnvironmentType = FallbackSceneEnvironmentType; 
                }
            }

            #endregion
            
            #region <Methods>

            public override async UniTask SetRecord(int p_Key, object[] p_RecordField, CancellationToken p_CancellationToken)
            {
                await base.SetRecord(p_Key, p_RecordField, p_CancellationToken);

                SceneStartPreset = p_RecordField.As<SceneTool.SceneStartPreset>(0);
                SceneLocationMetaSet = p_RecordField.As<List<SceneTool.SceneLocationMeta>>(1);
                SceneEnvironmentType = p_RecordField.As<Type>(2);
            }

            #endregion
        }
        
        #endregion
        
        #region <Callbacks>

        protected override async UniTask OnCreated(CancellationToken p_CancellationToken)
        {
            await base.OnCreated(p_CancellationToken);

            SetBranchHeader("Scene/SceneVariable/");
        }
        
        #endregion
    }
}