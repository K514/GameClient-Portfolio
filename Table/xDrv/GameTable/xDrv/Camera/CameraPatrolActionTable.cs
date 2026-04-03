#if !SERVER_DRIVE

using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace k514.Mono.Common
{
    /// <summary>
    /// 카메라를 일정 속도로 흔드는 연출에 관한 테이블 데이터 클래스
    /// </summary>
    public class CameraPatrolActionTable : GameTable<CameraPatrolActionTable, TableMetaData, int, CameraPatrolActionTable.TableRecord>
    {
        [Serializable]
        public class TableRecord : GameTableRecord
        {
            /// <summary>
            /// 흔들림 지속 시간
            /// </summary>
            public uint DurationMsec { get; private set; }
            
            /// <summary>
            /// 최대 흔들림 거리
            /// </summary>
            public float Distance { get; private set; }
            
            /// <summary>
            /// 흔들림 왕복 횟수
            /// </summary>
            public int CycleCount { get; private set; }
            
            /// <summary>
            /// 흔들림 왕복 횟수 역수값
            /// </summary>
            public float InvCycleCount { get; private set; }

            /// <summary>
            /// 흔들릴 방향
            /// </summary>
            public TableTool.SerializableVector3 Direction { get; private set; }

            public override async UniTask OnRecordAdded(CameraPatrolActionTable p_Table, CancellationToken p_CancellationToken)
            {
                await base.OnRecordAdded(p_Table, p_CancellationToken);

                if (CycleCount != 0)
                {
                    InvCycleCount = 1f / CycleCount;
                }
            }
        }
    }
}

#endif