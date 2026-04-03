using System;
using System.Collections.Generic;
using UnityEngine;

namespace k514.Mono.Common
{
    /// <summary>
    /// 샷 스탯
    /// </summary>
    [Serializable]
    public struct ShotStatusPreset
    {
        #region <Fields>

        /// <summary>
        /// 포함된 능력치 타입 플래그 마스크
        /// </summary>
        public ShotStatusTool.ShotStatusType FlagMask;

        /// <summary>
        /// 샷 위력
        /// </summary>
        public float ShotPower;

        /// <summary>
        /// 발사 수
        /// </summary>
        public float ShotCount;

        /// <summary>
        /// 발사 속도
        /// </summary>
        public float ShotSpeed;

        /// <summary>
        /// 샷 크기
        /// </summary>
        public float ShotScale;

        /// <summary>
        /// 샷 수명
        /// </summary>
        public float ShotDuration;

        /// <summary>
        /// 샷 관통횟수
        /// </summary>
        public float ShotPierce;

        #endregion

        #region <Indexer>

        public float this[ShotStatusTool.ShotStatusType p_Type] => GetProperty(p_Type);
        public float this[ShotStatusTool.ShotStatusType p_Type, float p_Rate] => GetProperty(p_Type, p_Rate);

        #endregion

        #region <Constructors>

        public ShotStatusPreset(ShotStatusTool.ShotStatusType p_Type, float p_Value)
        {
            this = default;

            SetProperty(p_Type, p_Value);
        }

        public ShotStatusPreset(Dictionary<ShotStatusTool.ShotStatusType, float> p_ValueSet)
        {
            this = default;

            if (p_ValueSet.CheckGenericCollectionSafe())
            {
                foreach (var shotStatusType in ShotStatusTool.ShotStatusTypeEnumerator)
                {
                    if (p_ValueSet.TryGetValue(shotStatusType, out var o_Value))
                    {
                        SetProperty(shotStatusType, o_Value);
                    }
                }
            }
        }

        #endregion

        #region <Operator>

        public static implicit operator ShotStatusPreset(float p_Value)
        {
            var result = new ShotStatusPreset();

            result.ShotPower = p_Value;
            result.ShotCount = p_Value;
            result.ShotSpeed = p_Value;
            result.ShotScale = p_Value;
            result.ShotDuration = p_Value;
            result.ShotPierce = p_Value;

            result.InitFlagMask();

            return result;
        }

        public static ShotStatusPreset operator+(ShotStatusPreset p_Left, ShotStatusPreset p_Right)
        {
            var result = new ShotStatusPreset();

            result.ShotPower = p_Left.ShotPower + p_Right.ShotPower;
            result.ShotCount = p_Left.ShotCount + p_Right.ShotCount;
            result.ShotSpeed = p_Left.ShotSpeed + p_Right.ShotSpeed;
            result.ShotScale = p_Left.ShotScale + p_Right.ShotScale;
            result.ShotDuration = p_Left.ShotDuration + p_Right.ShotDuration;
            result.ShotPierce = p_Left.ShotPierce + p_Right.ShotPierce;

            result.InitFlagMask();

            return result;
        }

        public static ShotStatusPreset operator+(ShotStatusPreset p_Left, float p_Right)
        {
            var result = new ShotStatusPreset();

            result.ShotPower = p_Left.ShotPower + p_Right;
            result.ShotCount = p_Left.ShotCount + p_Right;
            result.ShotSpeed = p_Left.ShotSpeed + p_Right;
            result.ShotScale = p_Left.ShotScale + p_Right;
            result.ShotDuration = p_Left.ShotDuration + p_Right;
            result.ShotPierce = p_Left.ShotPierce + p_Right;

            result.InitFlagMask();

            return result;
        }

        public static ShotStatusPreset operator+(float p_Left, ShotStatusPreset p_Right)
        {
            return p_Right + p_Left;
        }

        public static ShotStatusPreset operator-(ShotStatusPreset p_Left, ShotStatusPreset p_Right)
        {
            var result = new ShotStatusPreset();

            result.ShotPower = p_Left.ShotPower - p_Right.ShotPower;
            result.ShotCount = p_Left.ShotCount - p_Right.ShotCount;
            result.ShotSpeed = p_Left.ShotSpeed - p_Right.ShotSpeed;
            result.ShotScale = p_Left.ShotScale - p_Right.ShotScale;
            result.ShotDuration = p_Left.ShotDuration - p_Right.ShotDuration;
            result.ShotPierce = p_Left.ShotPierce - p_Right.ShotPierce;

            result.InitFlagMask();

            return result;
        }

        public static ShotStatusPreset operator-(ShotStatusPreset p_Left, float p_Right)
        {
            var result = new ShotStatusPreset();

            result.ShotPower = p_Left.ShotPower - p_Right;
            result.ShotCount = p_Left.ShotCount - p_Right;
            result.ShotSpeed = p_Left.ShotSpeed - p_Right;
            result.ShotScale = p_Left.ShotScale - p_Right;
            result.ShotDuration = p_Left.ShotDuration - p_Right;
            result.ShotPierce = p_Left.ShotPierce - p_Right;

            result.InitFlagMask();

            return result;
        }

        public static ShotStatusPreset operator-(float p_Left, ShotStatusPreset p_Right)
        {
            var result = new ShotStatusPreset();

            result.ShotPower = p_Left - p_Right.ShotPower;
            result.ShotCount = p_Left - p_Right.ShotCount;
            result.ShotSpeed = p_Left - p_Right.ShotSpeed;
            result.ShotScale = p_Left - p_Right.ShotScale;
            result.ShotDuration = p_Left - p_Right.ShotDuration;
            result.ShotPierce = p_Left - p_Right.ShotPierce;

            result.InitFlagMask();

            return result;
        }

        public static ShotStatusPreset operator-(ShotStatusPreset p_Left)
        {
            return -1f * p_Left;
        }

        public static ShotStatusPreset operator*(ShotStatusPreset p_Left, ShotStatusPreset p_Right)
        {
            var result = new ShotStatusPreset();

            result.ShotPower = p_Left.ShotPower * p_Right.ShotPower;
            result.ShotCount = p_Left.ShotCount * p_Right.ShotCount;
            result.ShotSpeed = p_Left.ShotSpeed * p_Right.ShotSpeed;
            result.ShotScale = p_Left.ShotScale * p_Right.ShotScale;
            result.ShotDuration = p_Left.ShotDuration * p_Right.ShotDuration;
            result.ShotPierce = p_Left.ShotPierce * p_Right.ShotPierce;

            result.InitFlagMask();

            return result;
        }

        public static ShotStatusPreset operator*(ShotStatusPreset p_Left, float p_Right)
        {
            var result = new ShotStatusPreset();

            result.ShotPower = p_Left.ShotPower * p_Right;
            result.ShotCount = p_Left.ShotCount * p_Right;
            result.ShotSpeed = p_Left.ShotSpeed * p_Right;
            result.ShotScale = p_Left.ShotScale * p_Right;
            result.ShotDuration = p_Left.ShotDuration * p_Right;
            result.ShotPierce = p_Left.ShotPierce * p_Right;

            result.InitFlagMask();

            return result;
        }

        public static ShotStatusPreset operator*(float p_Left, ShotStatusPreset p_Right)
        {
            return p_Right * p_Left;
        }

        public static ShotStatusPreset operator/(ShotStatusPreset p_Left, ShotStatusPreset p_Right)
        {
            var result = new ShotStatusPreset();

            result.ShotPower = p_Left.ShotPower / p_Right.ShotPower.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.ShotCount = p_Left.ShotCount / p_Right.ShotCount.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.ShotSpeed = p_Left.ShotSpeed / p_Right.ShotSpeed.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.ShotScale = p_Left.ShotScale / p_Right.ShotScale.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.ShotDuration = p_Left.ShotDuration /
                                  p_Right.ShotDuration.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.ShotPierce =
                p_Left.ShotPierce / p_Right.ShotPierce.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);

            result.InitFlagMask();

            return result;
        }

        public static ShotStatusPreset operator/(ShotStatusPreset p_Left, float p_Right)
        {
            var result = new ShotStatusPreset();
            p_Right = p_Right.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);

            result.ShotPower = p_Left.ShotPower / p_Right;
            result.ShotCount = p_Left.ShotCount / p_Right;
            result.ShotSpeed = p_Left.ShotSpeed / p_Right;
            result.ShotScale = p_Left.ShotScale / p_Right;
            result.ShotDuration = p_Left.ShotDuration / p_Right;
            result.ShotPierce = p_Left.ShotPierce / p_Right;

            result.InitFlagMask();

            return result;
        }

        public static ShotStatusPreset operator/(float p_Left, ShotStatusPreset p_Right)
        {
            var result = new ShotStatusPreset();

            result.ShotPower = p_Left / p_Right.ShotPower.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.ShotCount = p_Left / p_Right.ShotCount.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.ShotSpeed = p_Left / p_Right.ShotSpeed.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.ShotScale = p_Left / p_Right.ShotScale.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.ShotDuration = p_Left / p_Right.ShotDuration.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.ShotPierce = p_Left / p_Right.ShotPierce.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);

            result.InitFlagMask();

            return result;
        }

        public static ShotStatusPreset operator!(ShotStatusPreset p_Left)
        {
            return -1f * p_Left;
        }

#if UNITY_EDITOR
        public override string ToString()
        {
            var result = "";
            foreach (var shotStatusType in ShotStatusTool.ShotStatusTypeEnumerator)
            {
                if (FlagMask.HasAnyFlagExceptNone(shotStatusType))
                {
                    var property = GetProperty(shotStatusType);
                    result += $"[{shotStatusType} : {property}]\n";
                }
            }

            return string.IsNullOrEmpty(result) ? "유효한 스탯 없음!" : result;
        }
#endif

        #endregion

        #region <Methods>

        private void InitFlagMask()
        {
            FlagMask = default;

            foreach (var shotStatusType in ShotStatusTool.ShotStatusTypeEnumerator)
            {
                var property = GetProperty(shotStatusType);
                if (property.IsReachedZero(ShotStatusTool.PropertyLowerBound))
                {
                    FlagMask.RemoveFlag(shotStatusType);
                }
                else
                {
                    FlagMask.AddFlag(shotStatusType);
                }
            }
        }

        public bool HasProperValue()
        {
            return FlagMask != ShotStatusTool.ShotStatusType.None;
        }


        public bool HasProperValue(ShotStatusTool.ShotStatusType p_Type)
        {
            return FlagMask.HasAnyFlagExceptNone(p_Type);
        }

        public void AddProperty(ShotStatusTool.ShotStatusType p_Type, float p_Value)
        {
            var appliedProperty = GetProperty(p_Type) + p_Value;
            SetProperty(p_Type, appliedProperty);
        }

        public void SetProperty(ShotStatusTool.ShotStatusType p_Type, float p_Value)
        {
            if (p_Type != ShotStatusTool.ShotStatusType.None)
            {
                switch (p_Type)
                {
                    case ShotStatusTool.ShotStatusType.ShotPower:
                        ShotPower = p_Value;
                        break;
                    case ShotStatusTool.ShotStatusType.ShotCount:
                        ShotCount = p_Value;
                        break;
                    case ShotStatusTool.ShotStatusType.ShotSpeed:
                        ShotSpeed = p_Value;
                        break;
                    case ShotStatusTool.ShotStatusType.ShotScale:
                        ShotScale = p_Value;
                        break;
                    case ShotStatusTool.ShotStatusType.ShotDuration:
                        ShotDuration = p_Value;
                        break;
                    case ShotStatusTool.ShotStatusType.ShotPierce:
                        ShotPierce = p_Value;
                        break;
                }
            }

            if (p_Value.IsReachedZero(ShotStatusTool.PropertyLowerBound))
            {
                FlagMask.RemoveFlag(p_Type);
            }
            else
            {
                FlagMask.AddFlag(p_Type);
            }
        }

        public readonly float GetProperty(ShotStatusTool.ShotStatusType p_Type)
        {
            switch (p_Type)
            {
                default:
                case ShotStatusTool.ShotStatusType.None:
                    return 0;
                case ShotStatusTool.ShotStatusType.ShotPower:
                    return ShotPower;
                case ShotStatusTool.ShotStatusType.ShotCount:
                    return ShotCount;
                case ShotStatusTool.ShotStatusType.ShotSpeed:
                    return ShotSpeed;
                case ShotStatusTool.ShotStatusType.ShotScale:
                    return ShotScale;
                case ShotStatusTool.ShotStatusType.ShotDuration:
                    return ShotDuration;
                case ShotStatusTool.ShotStatusType.ShotPierce:
                    return ShotPierce;
            }
        }

        public readonly float GetProperty(ShotStatusTool.ShotStatusType p_Type, float p_MultiplyRate)
        {
            return GetProperty(p_Type) * p_MultiplyRate;
        }

        public readonly string GetPropertyText(ShotStatusTool.ShotStatusType p_Type, float p_MultiplyRate = 1f)
        {
            var valueType = p_Type.GetPropertyValueType();
            var value = GetProperty(p_Type, p_MultiplyRate);

            switch (valueType)
            {
                default:
                case StatusTool.PropertyValueType.None:
                    return "NULL";
                case StatusTool.PropertyValueType.FixedValue:
                    return $"{Mathf.CeilToInt(value)}";
                case StatusTool.PropertyValueType.SimpleRateValue:
                case StatusTool.PropertyValueType.CompoundRateValue:
                    return $"{Mathf.CeilToInt(100f * value)} %";
            }
        }

        #endregion
    }

    public class ShotStatusPresetWrapper
    {
        #region <Fields>

        public ShotStatusPreset ShotStatusPreset;

        #endregion

        #region <Indexer>

        public float this[ShotStatusTool.ShotStatusType p_Type] => ShotStatusPreset[p_Type];
        public float this[ShotStatusTool.ShotStatusType p_Type, float p_Rate] => ShotStatusPreset[p_Type, p_Rate];

        #endregion

        #region <Constructors>

        public ShotStatusPresetWrapper(ShotStatusPreset p_Preset)
        {
            ShotStatusPreset = p_Preset;
        }

        #endregion

        #region <Operator>

        public static implicit operator ShotStatusPreset(ShotStatusPresetWrapper p_Wrapper)
        {
            return p_Wrapper.ShotStatusPreset;
        }

        public static ShotStatusPreset operator +(ShotStatusPresetWrapper p_Left, ShotStatusPresetWrapper p_Right)
        {
            return p_Left.ShotStatusPreset + p_Right.ShotStatusPreset;
        }

        public static ShotStatusPreset operator +(ShotStatusPresetWrapper p_Left, float p_Right)
        {
            return p_Left.ShotStatusPreset + p_Right;
        }

        public static ShotStatusPreset operator +(float p_Left, ShotStatusPresetWrapper p_Right)
        {
            return p_Left + p_Right.ShotStatusPreset;
        }

        public static ShotStatusPreset operator -(ShotStatusPresetWrapper p_Left, ShotStatusPresetWrapper p_Right)
        {
            return p_Left.ShotStatusPreset - p_Right.ShotStatusPreset;
        }

        public static ShotStatusPreset operator -(ShotStatusPresetWrapper p_Left, float p_Right)
        {
            return p_Left.ShotStatusPreset - p_Right;
        }

        public static ShotStatusPreset operator -(float p_Left, ShotStatusPresetWrapper p_Right)
        {
            return p_Left - p_Right.ShotStatusPreset;
        }

        public static ShotStatusPreset operator *(ShotStatusPresetWrapper p_Left, ShotStatusPresetWrapper p_Right)
        {
            return p_Left.ShotStatusPreset * p_Right.ShotStatusPreset;
        }

        public static ShotStatusPreset operator *(ShotStatusPresetWrapper p_Left, float p_Right)
        {
            return p_Left.ShotStatusPreset * p_Right;
        }

        public static ShotStatusPreset operator *(float p_Left, ShotStatusPresetWrapper p_Right)
        {
            return p_Left * p_Right.ShotStatusPreset;
        }

        public static ShotStatusPreset operator /(ShotStatusPresetWrapper p_Left, ShotStatusPresetWrapper p_Right)
        {
            return p_Left.ShotStatusPreset / p_Right.ShotStatusPreset;
        }

        public static ShotStatusPreset operator /(ShotStatusPresetWrapper p_Left, float p_Right)
        {
            return p_Left.ShotStatusPreset / p_Right;
        }

        public static ShotStatusPreset operator /(float p_Left, ShotStatusPresetWrapper p_Right)
        {
            return p_Left / p_Right.ShotStatusPreset;
        }

        #endregion

        #region <Methods>

        public void SetPreset(ShotStatusPreset p_Preset)
        {
            ShotStatusPreset = p_Preset;
        }

        public void AddProperty(ShotStatusTool.ShotStatusType p_Type, float p_Value)
        {
            ShotStatusPreset.AddProperty(p_Type, p_Value);
        }

        public void SetProperty(ShotStatusTool.ShotStatusType p_Type, float p_Value)
        {
            ShotStatusPreset.SetProperty(p_Type, p_Value);
        }

        public float GetProperty(ShotStatusTool.ShotStatusType p_Type)
        {
            return ShotStatusPreset.GetProperty(p_Type);
        }

        public float GetProperty(ShotStatusTool.ShotStatusType p_Type, float p_Rate)
        {
            return ShotStatusPreset.GetProperty(p_Type, p_Rate);
        }

        #endregion
    }
}