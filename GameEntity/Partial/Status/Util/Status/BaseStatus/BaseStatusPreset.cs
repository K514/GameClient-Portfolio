using System;
using System.Collections.Generic;
using k514.Mono.Feature;
using UnityEngine;

namespace k514.Mono.Common
{
    /// <summary>
    /// 기본 유닛 스탯
    /// </summary>
    [Serializable]
    public struct BaseStatusPreset
    {
        #region <Fields>

        /// <summary>
        /// 포함된 능력치 타입 플래그 마스크
        /// </summary>
        public BaseStatusTool.BaseStatusType FlagMask;

        /// <summary>
        /// 힘
        /// </summary>
        public float STR;

        /// <summary>
        /// 지능
        /// </summary>
        public float INT;

        /// <summary>
        /// 체력
        /// </summary>
        public float VIT;

        /// <summary>
        /// 정신력
        /// </summary>
        public float WIL;

        /// <summary>
        /// 민첩
        /// </summary>
        public float AGI;

        /// <summary>
        /// 기량
        /// </summary>
        public float DEX;

        #endregion

        #region <Indexer>

        public float this[BaseStatusTool.BaseStatusType p_Type] => GetProperty(p_Type);
        public float this[BaseStatusTool.BaseStatusType p_Type, float p_Rate] => GetProperty(p_Type, p_Rate);
 
        #endregion

        #region <Constructors>

        public BaseStatusPreset(BaseStatusTool.BaseStatusType p_Type, float p_Value)
        {
            this = default;

            SetProperty(p_Type, p_Value);
        }

        public BaseStatusPreset(Dictionary<BaseStatusTool.BaseStatusType, float> p_ValueSet)
        {
            this = default;
            
            if (p_ValueSet.CheckGenericCollectionSafe())
            {
                foreach (var baseStatusType in BaseStatusTool.BaseStatusTypeEnumerator)
                {
                    if (p_ValueSet.TryGetValue(baseStatusType, out var o_Value))
                    {
                        SetProperty(baseStatusType, o_Value);
                    }
                }
            }
        }

        public BaseStatusPreset(int p_Key)
        {
            this = BaseStatusTable.GetInstanceUnsafe.GetRecord(p_Key).BaseStatusPreset;
        }

        #endregion
        
        #region <Operator>

        public static implicit operator BaseStatusPreset(float p_Value)
        {
            var result = new BaseStatusPreset();
            
            result.STR = p_Value;
            result.INT = p_Value;
            result.VIT = p_Value;
            result.WIL = p_Value;
            result.AGI = p_Value;
            result.DEX = p_Value;
            
            result.InitFlagMask();

            return result;
        }

        public static BaseStatusPreset operator+(BaseStatusPreset p_Left, BaseStatusPreset p_Right) 
        {
            var result = new BaseStatusPreset();
            
            result.STR = p_Left.STR + p_Right.STR;
            result.INT = p_Left.INT + p_Right.INT;
            result.VIT = p_Left.VIT + p_Right.VIT;
            result.WIL = p_Left.WIL + p_Right.WIL;
            result.AGI = p_Left.AGI + p_Right.AGI;
            result.DEX = p_Left.DEX + p_Right.DEX;
            
            result.InitFlagMask();
            
            return result;
        }
        
        public static BaseStatusPreset operator+(BaseStatusPreset p_Left, float p_Right) 
        {
            var result = new BaseStatusPreset();
            
            result.STR = p_Left.STR + p_Right;
            result.INT = p_Left.INT + p_Right;
            result.VIT = p_Left.VIT + p_Right;
            result.WIL = p_Left.WIL + p_Right;
            result.AGI = p_Left.AGI + p_Right;
            result.DEX = p_Left.DEX + p_Right;
            
            result.InitFlagMask();
            
            return result;
        }
        
        public static BaseStatusPreset operator+(float p_Left, BaseStatusPreset p_Right) 
        {
            var result = new BaseStatusPreset();
            
            result.STR = p_Left + p_Right.STR;
            result.INT = p_Left + p_Right.INT;
            result.VIT = p_Left + p_Right.VIT;
            result.WIL = p_Left + p_Right.WIL;
            result.AGI = p_Left + p_Right.AGI;
            result.DEX = p_Left + p_Right.DEX;
            
            result.InitFlagMask();
            
            return result;
        }
        
        public static BaseStatusPreset operator-(BaseStatusPreset p_Left, BaseStatusPreset p_Right) 
        {
            var result = new BaseStatusPreset();
            
            result.STR = p_Left.STR - p_Right.STR;
            result.INT = p_Left.INT - p_Right.INT;
            result.VIT = p_Left.VIT - p_Right.VIT;
            result.WIL = p_Left.WIL - p_Right.WIL;
            result.AGI = p_Left.AGI - p_Right.AGI;
            result.DEX = p_Left.DEX - p_Right.DEX;
            
            result.InitFlagMask();
            
            return result;
        }
        
        public static BaseStatusPreset operator-(BaseStatusPreset p_Left, float p_Right) 
        {
            var result = new BaseStatusPreset();
            
            result.STR = p_Left.STR - p_Right;
            result.INT = p_Left.INT - p_Right;
            result.VIT = p_Left.VIT - p_Right;
            result.WIL = p_Left.WIL - p_Right;
            result.AGI = p_Left.AGI - p_Right;
            result.DEX = p_Left.DEX - p_Right;
            
            result.InitFlagMask();
            
            return result;
        }
        
        public static BaseStatusPreset operator-(float p_Left, BaseStatusPreset p_Right) 
        {
            var result = new BaseStatusPreset();
            
            result.STR = p_Left - p_Right.STR;
            result.INT = p_Left - p_Right.INT;
            result.VIT = p_Left - p_Right.VIT;
            result.WIL = p_Left - p_Right.WIL;
            result.AGI = p_Left - p_Right.AGI;
            result.DEX = p_Left - p_Right.DEX;
            
            result.InitFlagMask();
            
            return result;
        }
        
        public static BaseStatusPreset operator-(BaseStatusPreset p_Left)
        {
            return -1f * p_Left;
        }
        
        public static BaseStatusPreset operator*(BaseStatusPreset p_Left, BaseStatusPreset p_Right) 
        {
            var result = new BaseStatusPreset();
            
            result.STR = p_Left.STR * p_Right.STR;
            result.INT = p_Left.INT * p_Right.INT;
            result.VIT = p_Left.VIT * p_Right.VIT;
            result.WIL = p_Left.WIL * p_Right.WIL;
            result.AGI = p_Left.AGI * p_Right.AGI;
            result.DEX = p_Left.DEX * p_Right.DEX;
            
            result.InitFlagMask();
            
            return result;
        }
        
        public static BaseStatusPreset operator*(BaseStatusPreset p_Left, float p_Right) 
        {
            var result = new BaseStatusPreset();
            
            result.STR = p_Left.STR * p_Right;
            result.INT = p_Left.INT * p_Right;
            result.VIT = p_Left.VIT * p_Right;
            result.WIL = p_Left.WIL * p_Right;
            result.AGI = p_Left.AGI * p_Right;
            result.DEX = p_Left.DEX * p_Right;
            
            result.InitFlagMask();
            
            return result;
        }
        
        public static BaseStatusPreset operator*(float p_Left, BaseStatusPreset p_Right)
        {
            return p_Right * p_Left;
        }
        
        public static BaseStatusPreset operator/(BaseStatusPreset p_Left, BaseStatusPreset p_Right)
        {
            return p_Left * (1f / p_Right);
        }

        public static BaseStatusPreset operator/(BaseStatusPreset p_Left, float p_Right)
        {
            var result = new BaseStatusPreset();
            p_Right = p_Right.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            
            result.STR = p_Left.STR / p_Right;
            result.INT = p_Left.INT / p_Right;
            result.VIT = p_Left.VIT / p_Right;
            result.WIL = p_Left.WIL / p_Right;
            result.AGI = p_Left.AGI / p_Right;
            result.DEX = p_Left.DEX / p_Right;
            
            result.InitFlagMask();
            
            return result;
        }

        public static BaseStatusPreset operator/(float p_Left, BaseStatusPreset p_Right)
        {
            var result = new BaseStatusPreset();
            
            result.STR = p_Left / p_Right.STR.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.INT = p_Left / p_Right.INT.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.VIT = p_Left / p_Right.VIT.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.WIL = p_Left / p_Right.WIL.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.AGI = p_Left / p_Right.AGI.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.DEX = p_Left / p_Right.DEX.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            
            result.InitFlagMask();
            
            return result;
        }
        
        public static BaseStatusPreset operator!(BaseStatusPreset p_Left)
        {
            return -1f * p_Left;
        }
        
#if UNITY_EDITOR
        public override string ToString()
        {
            var result = "";
            foreach (var baseStatusType in BaseStatusTool.BaseStatusTypeEnumerator)
            {
                if (FlagMask.HasAnyFlagExceptNone(baseStatusType))
                {
                    var property = GetProperty(baseStatusType);
                    result += $"[{baseStatusType} : {property}]\n";
                }
            }

            return string.IsNullOrEmpty(result) ? "유효한 스탯 없음!" : result;
        }
#endif

        #endregion

        #region <Methods>
        
        public bool HasProperValue()
        {
            return FlagMask != BaseStatusTool.BaseStatusType.None;
        }
        
        public bool HasProperValue(BaseStatusTool.BaseStatusType p_Type)
        {
            return FlagMask.HasAnyFlagExceptNone(p_Type);
        }
        
        private void InitFlagMask()
        {
            FlagMask = default;
            
            foreach (var baseStatusType in BaseStatusTool.BaseStatusTypeEnumerator)
            {
                var property = GetProperty(baseStatusType);
                if (property.IsReachedZero(BaseStatusTool.PropertyLowerBound))
                {
                    FlagMask.RemoveFlag(baseStatusType);
                }
                else
                {
                    FlagMask.AddFlag(baseStatusType);
                }
            }
        }
        
        public void AddProperty(BaseStatusTool.BaseStatusType p_Type, float p_Value)
        {
            var appliedProperty = GetProperty(p_Type) + p_Value;
            SetProperty(p_Type, appliedProperty);
        }
        
        public void SetProperty(BaseStatusTool.BaseStatusType p_Type, float p_Value)
        {
            if (p_Type != BaseStatusTool.BaseStatusType.None)
            {
                switch (p_Type)
                {
                    case BaseStatusTool.BaseStatusType.STR:
                        STR = p_Value;
                        break;
                    case BaseStatusTool.BaseStatusType.INT:
                        INT = p_Value;
                        break;
                    case BaseStatusTool.BaseStatusType.VIT:
                        VIT = p_Value;
                        break;
                    case BaseStatusTool.BaseStatusType.WIL:
                        WIL = p_Value;
                        break;
                    case BaseStatusTool.BaseStatusType.AGI:
                        AGI = p_Value;
                        break;
                    case BaseStatusTool.BaseStatusType.DEX:
                        DEX = p_Value;
                        break;
                }
            }
 
            if (p_Value.IsReachedZero(BaseStatusTool.PropertyLowerBound))
            {
                FlagMask.RemoveFlag(p_Type);
            }
            else
            {
                FlagMask.AddFlag(p_Type);
            }
        }
        
        public float GetProperty(BaseStatusTool.BaseStatusType p_Type)
        {
            switch (p_Type)
            {
                default:
                case BaseStatusTool.BaseStatusType.None:
                    return 0;
                case BaseStatusTool.BaseStatusType.STR:
                    return STR;
                case BaseStatusTool.BaseStatusType.INT:
                    return INT;
                case BaseStatusTool.BaseStatusType.VIT:
                    return VIT;
                case BaseStatusTool.BaseStatusType.WIL:
                    return WIL;
                case BaseStatusTool.BaseStatusType.AGI:
                    return AGI;
                case BaseStatusTool.BaseStatusType.DEX:
                    return DEX;
            }
        }
    
        public float GetProperty(BaseStatusTool.BaseStatusType p_Type, float p_Rate)
        {
            return GetProperty(p_Type) * p_Rate;
        }

        #endregion
    }

    public class BaseStatusPresetWrapper
    {
        #region <Fields>

        public BaseStatusPreset BaseStatusPreset;

        #endregion
        
        #region <Indexer>

        public float this[BaseStatusTool.BaseStatusType p_Type] => BaseStatusPreset[p_Type];
        public float this[BaseStatusTool.BaseStatusType p_Type, float p_Rate] => BaseStatusPreset[p_Type, p_Rate];
 
        #endregion
        
        #region <Constructors>

        public BaseStatusPresetWrapper(BaseStatusPreset p_Preset)
        {
            BaseStatusPreset = p_Preset;
        }

        #endregion

        #region <Operator>

        public static implicit operator BaseStatusPreset(BaseStatusPresetWrapper p_Wrapper)
        {
            return p_Wrapper.BaseStatusPreset;
        }
        
        public static BaseStatusPreset operator+(BaseStatusPresetWrapper p_Left, BaseStatusPresetWrapper p_Right) 
        {
            return p_Left.BaseStatusPreset + p_Right.BaseStatusPreset;
        }
        
        public static BaseStatusPreset operator+(BaseStatusPresetWrapper p_Left, float p_Right) 
        {
            return p_Left.BaseStatusPreset + p_Right;
        }
        
        public static BaseStatusPreset operator+(float p_Left, BaseStatusPresetWrapper p_Right) 
        {
            return p_Left + p_Right.BaseStatusPreset;
        }

        public static BaseStatusPreset operator-(BaseStatusPresetWrapper p_Left, BaseStatusPresetWrapper p_Right) 
        {
            return p_Left.BaseStatusPreset - p_Right.BaseStatusPreset;
        }
        
        public static BaseStatusPreset operator-(BaseStatusPresetWrapper p_Left, float p_Right) 
        {
            return p_Left.BaseStatusPreset - p_Right;
        }
        
        public static BaseStatusPreset operator-(float p_Left, BaseStatusPresetWrapper p_Right) 
        {
            return p_Left - p_Right.BaseStatusPreset;
        }
        
        public static BaseStatusPreset operator*(BaseStatusPresetWrapper p_Left, BaseStatusPresetWrapper p_Right) 
        {
            return p_Left.BaseStatusPreset * p_Right.BaseStatusPreset;
        }
        
        public static BaseStatusPreset operator*(BaseStatusPresetWrapper p_Left, float p_Right) 
        {
            return p_Left.BaseStatusPreset * p_Right;
        }
        
        public static BaseStatusPreset operator*(float p_Left, BaseStatusPresetWrapper p_Right) 
        {
            return p_Left * p_Right.BaseStatusPreset;
        }
        
        public static BaseStatusPreset operator/(BaseStatusPresetWrapper p_Left, BaseStatusPresetWrapper p_Right) 
        {
            return p_Left.BaseStatusPreset / p_Right.BaseStatusPreset;
        }
        
        public static BaseStatusPreset operator/(BaseStatusPresetWrapper p_Left, float p_Right) 
        {
            return p_Left.BaseStatusPreset / p_Right;
        }
        
        public static BaseStatusPreset operator/(float p_Left, BaseStatusPresetWrapper p_Right) 
        {
            return p_Left / p_Right.BaseStatusPreset;
        }
        
        #endregion
        
        #region <Methods>

        public void SetPreset(BaseStatusPreset p_Preset)
        {
            BaseStatusPreset = p_Preset;
        }

        public void AddProperty(BaseStatusTool.BaseStatusType p_Type, float p_Value)
        {
            BaseStatusPreset.AddProperty(p_Type, p_Value);
        }
        
        public void SetProperty(BaseStatusTool.BaseStatusType p_Type, float p_Value)
        {
            BaseStatusPreset.SetProperty(p_Type, p_Value);
        }

        public float GetProperty(BaseStatusTool.BaseStatusType p_Type)
        {
            return BaseStatusPreset.GetProperty(p_Type);
        }

        public float GetProperty(BaseStatusTool.BaseStatusType p_Type, float p_Rate)
        {
            return BaseStatusPreset.GetProperty(p_Type, p_Rate);
        }

        #endregion
    }
}