using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using k514.Mono.Common;

namespace k514
{
    /// <summary>
    /// 각 씬의 고유 데이터를 기술하는 테이블 클래스, 키 값은 확장자를 포함한 씬 에셋 파일 이름이다.
    ///
    /// SceneListTable도 정수 key와 씬 이름이 1:1로 대응되기 때문에 해당 테이블과 구성은 같으나 씬 리스트 테이블은
    /// SceneListType단위로 씬을 분류하는 기능을 가지므로, Built-In 씬의 정보는 가지지 못하기 때문에
    /// 해당 씬에 대한 정보도 기술할 수 없기 때문에 이름 단위로 모든 씬을 다룰 수 있는 해당 씬과 차이를 가진다.
    /// </summary>
    public class SceneConstantDataTable : GameTable<SceneConstantDataTable, TableMetaData, string, SceneConstantDataTable.TableRecord>
    {
        #region <Record>
        
        [Serializable]
        public class TableRecord : GameTableRecord
        {
            #region <Fields>

            /// <summary>
            /// 해당 씬에 적용할 변수 레코드 인덱스 리스트
            /// </summary>
            public List<int> SceneVariableDataRecordIndexList { get; private set; }

            #endregion

            #region <Callbacks>

            public override async UniTask OnRecordAdded(SceneConstantDataTable p_Table, CancellationToken p_Cancellation)
            {
                await base.OnRecordAdded(p_Table, p_Cancellation);

                if (ReferenceEquals(null, SceneVariableDataRecordIndexList))
                {
                    SceneVariableDataRecordIndexList = new List<int>(); 
                }
            }

            #endregion
            
            #region <Methods>

            public override async UniTask SetRecord(string p_Key, object[] p_RecordField, CancellationToken p_CancellationToken)
            {
                await base.SetRecord(p_Key, p_RecordField, p_CancellationToken);

                SceneVariableDataRecordIndexList = p_RecordField.As<List<int>>(0);
            }

            public SceneVariableDataTable.TableRecord GetSceneVariableDataRecord(int p_Index)
            {
                var tryIndex = SceneVariableDataRecordIndexList.GetElementSafe(p_Index);
                
                return SceneVariableDataTable.GetInstanceUnsafe.GetRecordOrFallback(tryIndex);
            }

            #endregion
        }
        
        #endregion

        #region <Callbacks>

        protected override async UniTask OnCreated(CancellationToken p_CancellationToken)
        {
            await base.OnCreated(p_CancellationToken);

            SetBranchHeader("Scene/SceneConstant/");
        }

        #endregion
    }
}