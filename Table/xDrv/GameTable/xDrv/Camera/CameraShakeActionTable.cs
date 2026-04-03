#if !SERVER_DRIVE

using System;
using UnityEngine;

namespace k514.Mono.Common
{
    /// <summary>
    /// 카메라를 가속하는 속도로 흔드는 연출에 관한 테이블 데이터 클래스
    /// </summary>
    public class CameraShakeActionTable : GameTable<CameraShakeActionTable, TableMetaData, int, CameraShakeActionTable.TableRecord>
    {
        [Serializable]
        public class TableRecord : GameTableRecord
        {
            /// <summary>
            /// 최대 속도
            /// </summary>
            public TableTool.SerializableVector3 SwingBoundVector { get; private set; }
            
            /// <summary>
            /// 가속도
            /// </summary>
            public TableTool.SerializableVector3 Acceleration { get; private set; }
        }
    }
}

#endif