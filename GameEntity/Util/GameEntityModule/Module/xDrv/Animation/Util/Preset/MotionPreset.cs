using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace k514.Mono.Common
{         
    public struct AnimatorPreset
    {
        #region <Fields>

        public readonly int Index;
        public readonly AssetLoadResult<RuntimeAnimatorController> Animator;
        public readonly Dictionary<AnimationTool.MotionType, List<ClipPreset>> MotionTable;
        public readonly bool ValidFlag;
        
        #endregion

        #region <Constructors>

        public AnimatorPreset
        (
            int p_Index, AssetLoadResult<RuntimeAnimatorController> p_Animator,
            Dictionary<AnimationTool.MotionType, List<ClipPreset>> p_MotionTable
        )
        {
            Index = p_Index;
            Animator = p_Animator;
            MotionTable = p_MotionTable;
            ValidFlag = true;
        }

        #endregion

        #region <Methods>

        public bool HasMotion(AnimationTool.MotionType p_Type)
        {
            return HasMotion(p_Type, 0);
        }

        public bool HasMotion(AnimationTool.MotionType p_Type, int p_Index)
        {
            if (MotionTable.TryGetValue(p_Type, out var o_List))
            {
                return o_List.CheckCollectionSafe(p_Index);
            }
            else
            {
                return false;
            }
        }

        public bool TryGetRandomMotionIndex(AnimationTool.MotionType p_Type, out int o_Index)
        {
            if (MotionTable.TryGetValue(p_Type, out var o_List))
            {
                o_Index = o_List.GetRandomIndex();
                return true;
            }
            else
            {
                o_Index = -1;
                return false;
            }
        }

        #endregion
    }

    public struct ClipPreset : IEquatable<ClipPreset>
    {
        #region <Fields>

        /// <summary>
        /// 모션 타입
        /// </summary>
        public readonly AnimationTool.MotionType MotionType;
              
        /// <summary>
        /// 모션 인덱스
        /// </summary>
        public readonly int MotionIndex;

        /// <summary>
        /// 모션 플레이스 타입
        /// </summary>
        public readonly AnimationTool.MotionPlaceType MotionPlaceType;
        
        /// <summary>
        /// 애니메이션 클립 에셋
        /// </summary>
        public readonly AnimationClip Clip;

        /// <summary>
        /// 애니메이션 클립 에셋
        /// </summary>
        public readonly string ClipName;
                
        /// <summary>
        /// 유효성 검증 플래그
        /// </summary>
        public readonly bool ValidFlag;
        
        /// <summary>
        /// 해시코드 캐시
        /// </summary>
        private readonly int _Hash;
        
        #endregion

        #region <Constructors>

        public ClipPreset(AnimationTool.MotionType p_MotionType, int p_MotionIndex, AnimationTool.MotionPlaceType p_MotionPlaceType, AnimationClip p_Clip)
        {
            MotionType = p_MotionType;
            MotionIndex = p_MotionIndex;
            MotionPlaceType = p_MotionPlaceType;
            Clip = p_Clip;
            ClipName = Clip.name;
            ValidFlag = true;

            _Hash = HashCode.Combine(MotionType, MotionIndex, MotionPlaceType, Clip);
        }

        #endregion

        #region <Operator>

        /// <summary>
        /// IEquatable 동등성 검증 메서드
        /// </summary>
        public override bool Equals(object p_Right)
        {
            return p_Right is ClipPreset c_Right && Equals(c_Right);
        }
        
        /// <summary>
        /// IEquatable<T>동등성 검증 메서드
        /// </summary>
        public bool Equals(ClipPreset p_RightValue)
        {
            return MotionType == p_RightValue.MotionType
                   && MotionIndex == p_RightValue.MotionIndex
                   && MotionPlaceType == p_RightValue.MotionPlaceType
                   && Clip == p_RightValue.Clip;
        }

        /// <summary>
        /// 해쉬코드는 불변값이므로 미리 계산해서 캐싱한 값을 리턴한다.
        /// </summary>
        public override int GetHashCode()
        {
            return _Hash;
        }

        /// <summary>
        /// 동등연산자 == 재정의
        /// </summary>
        public static bool operator ==(ClipPreset p_Left, ClipPreset p_Right)
        {
            return p_Left.Equals(p_Right);
        }

        /// <summary>
        /// 동등연산자 != 재정의
        /// </summary>
        public static bool operator !=(ClipPreset p_Left, ClipPreset p_Right)
        {
            return !p_Left.Equals(p_Right);
        }

#if UNITY_EDITOR
        public override string ToString()
        {
            return $"{MotionType}({MotionIndex})";
        }
#endif

        #endregion
    }
    
    [Serializable]
    public struct ClipEventPreset
    {
        #region <Fields>

        public readonly AnimationTool.ClipEventType Type;
        public readonly float TimeRate;
          
        #endregion

        #region <Constructors>

        public ClipEventPreset(AnimationTool.ClipEventType p_Type, float p_Rate)
        {
            Type = p_Type;
            TimeRate = p_Rate;
        }

        #endregion
    }
    
    [Serializable]
    public struct MotionFallBackPreset
    {
        public AnimationTool.MotionType FallBackMotionType;
        public AnimationTool.FallBackFailHandleType FallBackFailHandleType;

        public MotionFallBackPreset(AnimationTool.MotionType p_FallBackMotionType) : this(p_FallBackMotionType, AnimationTool.FallBackFailHandleType.JustPlay)
        {
        }
            
        public MotionFallBackPreset(AnimationTool.MotionType p_FallBackMotionType, AnimationTool.FallBackFailHandleType p_FallBackFailHandleType)
        {
            FallBackMotionType = p_FallBackMotionType;
            FallBackFailHandleType = p_FallBackFailHandleType;
        }
    }
}