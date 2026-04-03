#if !SERVER_DRIVE

using System;
using UnityEngine;

namespace k514.Mono.Common
{
    public class SpriteAnimationTable : GameTable<SpriteAnimationTable, TableMetaData, UIxTool.AnimationSpriteType, SpriteAnimationTable.TableRecord>
    {
        #region <Record>

        [Serializable]
        public class TableRecord : GameTableRecord
        {
            #region <Fields>

            /// <summary>
            /// 이미지 스케일
            /// </summary>
            public TableTool.SerializableVector3 Scale { private set; get; }
            
            /// <summary>
            /// 이미지 오프셋
            /// </summary>
            public TableTool.SerializableVector3 Offset { private set; get; }

            /// <summary>
            /// 스프라이트 파일 이름 포맷, 넘버링은 0부터 시작하도록 지을 것.
            /// </summary>
            public string SpriteNameFormat { private set; get; }
            
            /// <summary>
            /// 스프라이트 파일 이름 인덱스 번호 길이
            /// </summary>
            public int IndexNumberLength { private set; get; }
            
            /// <summary>
            /// 스프라이트 갯수
            /// </summary>
            public int SpriteNumber { private set; get; }
            
            /// <summary>
            /// 애니메이션 재생시간
            /// </summary>
            public float AnimationDuration { private set; get; }
            
            /// <summary>
            /// 애니메이션 반복 수, 0이면 무한
            /// </summary>
            public int LoopCount { private set; get; }

            #endregion
        }
        
        #endregion
    }
}

#endif