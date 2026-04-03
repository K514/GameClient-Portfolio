#if !SERVER_DRIVE

using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace k514.Mono.Common
{
    /// <summary>
    /// 카메라 변수 데이터를 기술하는 테이블
    /// </summary>
    public class CameraVariableDataTable : GameTable<CameraVariableDataTable, TableMetaData, int, CameraVariableDataTable.TableRecord>
    {
        [Serializable]
        public class TableRecord : GameTableRecord
        {
            #region <Fields>

            /// <summary>
            /// [레이어 타입, 원거리 컬링 거리] 컬렉션
            /// </summary>
            public Dictionary<GameConst.GameLayerType, float> FarCullingDistances { get; private set; }

            /// <summary>
            /// 0보다 크면 직교 카메라로 동작한다.
            /// </summary>
            public float OrthographicSize { get; private set; }
            
            /* 카메라 관련 */
            /// <summary>
            /// 카메라 초기 거리
            /// </summary>
            public float CameraDistance { get; private set; }

            /// <summary>
            /// 카메라 추적 오프셋
            /// </summary>
            public TableTool.SerializableVector3 CameraTraceOffset { get; private set; }

            /// <summary>
            /// 카메라 초기 수직 회전
            /// </summary>
            public float CameraWrapperTiltDegree { get; private set; }

            /// <summary>
            /// 카메라 초기 수평 회전
            /// </summary>
            public float CameraWrapperSightDegree { get; private set; }
         
            #endregion

            #region <Methods>

            public override async UniTask SetRecord(int p_Key, object[] p_RecordField, CancellationToken p_CancellationToken)
            {
                await base.SetRecord(p_Key, p_RecordField, p_CancellationToken);

                FarCullingDistances = (Dictionary<GameConst.GameLayerType, float>)p_RecordField.GetElementSafe(0);
                CameraDistance = (float)p_RecordField.GetElementSafe(1);
                CameraTraceOffset = (TableTool.SerializableVector3)p_RecordField.GetElementSafe(2);
                CameraWrapperTiltDegree = (float)p_RecordField.GetElementSafe(3);
                CameraWrapperSightDegree = (float)p_RecordField.GetElementSafe(4);
            }
            
            public Vector3 ApplyCaemraWrapperDegree(Transform p_Pivot)
            {
                p_Pivot.rotation = Quaternion.identity;
                p_Pivot.Rotate(new Vector3(CameraWrapperTiltDegree, CameraWrapperSightDegree, 0f), Space.World);
                return p_Pivot.forward;
            }
            
            #endregion
        }

        protected override async UniTask AddDefaultRecords(CancellationToken p_CancellationToken)
        {
            await base.AddDefaultRecords(p_CancellationToken);

            await AddRecord
            (
                0, false, p_CancellationToken, 
                new Dictionary<GameConst.GameLayerType, float>
                {
                    {GameConst.GameLayerType.UnitA, 50f},
                    {GameConst.GameLayerType.UnitB, 50f},
                    {GameConst.GameLayerType.UnitC, 50f},
                }, 
                18f, 
                new TableTool.SerializableVector3(0f, 1.5f, 0f),
                60f, 0f
            );
        }
    }
}

#endif