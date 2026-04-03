#if !SERVER_DRIVE

using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace k514.Mono.Common
{
    /// <summary>
    /// 카메라 상수 데이터를 기술하는 테이블 클래스
    /// </summary>
    public class CameraConstantDataTable : GameTable<CameraConstantDataTable, TableMetaData, int, CameraConstantDataTable.TableRecord>
    {
        [Serializable]
        public class TableRecord : GameTableRecord
        {
            #region <Fields>

            /// <summary>
            /// 카메라 뷰 컨트롤 [세로, 가로]회전 속도배율 마스크 벡터
            /// </summary>
            public TableTool.SerializableVector2 CameraRotationSpeedRateMask { get; private set; }
            
            /// <summary>
            /// 뷰컨트롤 줌인/아웃 속력
            /// </summary>
            public float ZoomSpeed { get; private set; }
            
            /// <summary>
            /// 뷰컨트롤 회전 속력
            /// </summary>
            public float RotationSpeed { get; private set; }
            
            /// <summary>
            /// 뷰컨트롤 드래그 지속시, 회전 속력 최저 배율
            /// </summary>
            public float RotationSpeedMinRate { get; private set; }
            
            /// <summary>
            /// 뷰컨트롤 드래그 지속시, 속력 증가 배율
            /// </summary>
            public float RotationSpeedRate { get; private set; }
            
            /// <summary>
            /// 뷰컨트롤 드래그 지속시, 회전 속력 최대 배율
            /// </summary>
            public float RotationSpeedMaxRate { get; private set; }

            /// <summary>
            /// 카메라 뷰 컨트롤 초기화시 선딜레이
            /// </summary>
            public uint ResetLerpPreMsec { get; private set; }            
            
            /// <summary>
            /// 카메라 뷰 컨트롤 초기화시 지속시간
            /// </summary>
            public uint ResetLerpMsec { get; private set; }

            /// <summary>
            /// 카메라 스무딩 중, 초점 추적 거리가 늘어나는 속력
            /// </summary>
            public float SmoothLerpRadiusIncreaseSpeed { get; private set; }
            
            /// <summary>
            /// 카메라 스무딩 중, 초점 추적 거리가 줄어드는 속력
            /// </summary>
            public float SmoothLerpRadiusDecreaseSpeed { get; private set; }
                        
            /// <summary>
            /// 카메라 초점 추적을 기술하는 오브젝트
            /// </summary>
            public CameraFocusParams TraceRadiusRate { get; private set; }


            #endregion

            #region <Methods>

            public override async UniTask SetRecord(int p_Key, object[] p_RecordField, CancellationToken p_CancellationToken)
            {
                await base.SetRecord(p_Key, p_RecordField, p_CancellationToken);

                CameraRotationSpeedRateMask = (TableTool.SerializableVector2)p_RecordField.GetElementSafe(0);
                ZoomSpeed = (float)p_RecordField.GetElementSafe(1);
                RotationSpeed = (float)p_RecordField.GetElementSafe(2);
                RotationSpeedMinRate = (float)p_RecordField.GetElementSafe(3);
                RotationSpeedRate = (float)p_RecordField.GetElementSafe(4);
                RotationSpeedMaxRate = (float)p_RecordField.GetElementSafe(5);
                ResetLerpPreMsec = (uint)p_RecordField.GetElementSafe(6);
                ResetLerpMsec = (uint)p_RecordField.GetElementSafe(7);
                SmoothLerpRadiusIncreaseSpeed = (float)p_RecordField.GetElementSafe(8);
                SmoothLerpRadiusDecreaseSpeed = (float)p_RecordField.GetElementSafe(9);
                TraceRadiusRate = (CameraFocusParams)p_RecordField.GetElementSafe(10);
            }
            
            public override async UniTask OnRecordAdded(CameraConstantDataTable p_Table, CancellationToken p_CancellationToken)
            {
                await base.OnRecordAdded(p_Table, p_CancellationToken);

                if (!TraceRadiusRate.ValidFlag)
                {
                    TraceRadiusRate = CameraFocusParams.GetDefault();
                }
            }
            
            #endregion
        }

        protected override async UniTask AddDefaultRecords(CancellationToken p_CancellationToken)
        {
            await base.AddDefaultRecords(p_CancellationToken);

            await AddRecord
            (
                0, false, p_CancellationToken, 
                new TableTool.SerializableVector2(1f, 1.5f),
                30f, 120f, 1f, 0.1f, 1.1f, 
                50u, 200u, 2f, 6f, 
                CameraFocusParams.GetDefault()
            );
        }
    }
}

#endif