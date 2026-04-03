using System;

namespace k514.Mono.Common
{
    public static class GameEntityRelationTool
    {
        #region <Enums>

        /// <summary>
        /// 개체 오브젝트의 관계를 기술하는 열거형 상수
        /// </summary>
        [Flags]
        public enum GameEntityRelationType
        {
            /// <summary>
            /// 관계 없음
            /// </summary>
            None = 0,
            
            /// <summary>
            /// 파티원과 파티장
            /// </summary>
            Party = 1 << 0,
            
            /// <summary>
            /// 마스터와 슬레이브
            /// </summary>
            Possession = 1 << 1,
            
            /// <summary>
            /// 관측자와 시야에 들어온 개체
            /// </summary>
            Focus = 1 << 2,
            
            /// <summary>
            /// 공격 개체와 공격대상 후보 개체
            /// </summary>
            Enemy = 1 << 3,
        }

        public enum GameEntityRelationEventType
        {
            SubNodeAdded,
            SubNodeSelected,
            SubNodeDead,
            SubNodeRemoved,
        }
        
        #endregion

        #region <Structs>

        public struct GameEntityRelationEventParams
        {
            #region <Fields>

            public readonly GameEntityRelation GameEntityRelation;
            public readonly IGameEntityBridge TargetEntity, PrevTargetEntity;

            #endregion

            #region <Constructors>

            public GameEntityRelationEventParams(GameEntityRelation p_GameEntityRelation, IGameEntityBridge p_TargetEntity)
            {
                GameEntityRelation = p_GameEntityRelation;
                TargetEntity = p_TargetEntity;
                PrevTargetEntity = null;
            }
        
            public GameEntityRelationEventParams(GameEntityRelation p_GameEntityRelation, IGameEntityBridge p_PrevTargetEntity, IGameEntityBridge p_TargetEntity)
            {
                GameEntityRelation = p_GameEntityRelation;
                TargetEntity = p_TargetEntity;
                PrevTargetEntity = p_PrevTargetEntity;
            }

            #endregion
        }

        #endregion
    }
}