using System;
using System.Collections.Generic;
using k514.Mono.Feature;

namespace k514.Mono.Common
{
    public static class ShotStatusTool
    {
        #region <Consts>
        
        public const float PropertyLowerBound = 0.01f;
        public static readonly ShotStatusPreset BasisShotStatusPreset;
        public static readonly ShotStatusPreset EinShotStatusPreset;

        #endregion
        
        #region <Enums>

        [Flags]
        public enum ShotStatusType
        {
            None = 0,
            
            /// <summary>
            /// 샷 위력
            /// </summary>
            ShotPower = 1 << 0,
            
            /// <summary>
            /// 발사 수
            /// </summary>
            ShotCount = 1 << 1,
            
            /// <summary>
            /// 발사 속도
            /// </summary>
            ShotSpeed = 1 << 2,

            /// <summary>
            /// 샷 크기
            /// </summary>
            ShotScale = 1 << 3,
            
            /// <summary>
            /// 샷 수명
            /// </summary>
            ShotDuration = 1 << 4,
            
            /// <summary>
            /// 샷 관통횟수
            /// </summary>
            ShotPierce = 1 << 5,
        }
        
        public static ShotStatusType[] ShotStatusTypeEnumerator;

        #endregion

        #region <Constructor>

        static ShotStatusTool()
        {
            ShotStatusTypeEnumerator = EnumFlag.GetEnumEnumerator<ShotStatusType>(EnumFlag.GetEnumeratorType.ExceptNone);
            
            var defaultMetaValueSet = new Dictionary<ShotStatusType, float>();
            foreach (var baseStatusType in ShotStatusTypeEnumerator)
            {
                switch (baseStatusType)
                {
                    default:
                    case ShotStatusType.None:
                        break;
                    case ShotStatusType.ShotPierce:
                        defaultMetaValueSet.Add(baseStatusType, 0f);
                        break;
                    case ShotStatusType.ShotPower:
                    case ShotStatusType.ShotCount:
                    case ShotStatusType.ShotSpeed:
                    case ShotStatusType.ShotScale:
                    case ShotStatusType.ShotDuration:
                        defaultMetaValueSet.Add(baseStatusType, 1f);
                        break;
                }
            }

            BasisShotStatusPreset = new ShotStatusPreset(defaultMetaValueSet);
            EinShotStatusPreset = 1f;
        }

        #endregion

        #region <Methods>

        public static StatusTool.PropertyValueType GetPropertyValueType(this ShotStatusType p_Type)
        {
            switch (p_Type)
            {
                default:
                case ShotStatusType.None:
                    return StatusTool.PropertyValueType.None;
                case ShotStatusType.ShotPierce:
                case ShotStatusType.ShotCount:
                    return StatusTool.PropertyValueType.FixedValue;
                case ShotStatusType.ShotScale:
                case ShotStatusType.ShotDuration:
                case ShotStatusType.ShotSpeed:
                case ShotStatusType.ShotPower:
                    return StatusTool.PropertyValueType.SimpleRateValue;
            }
        }
        
        public static GameContentsLanguageDataTable.TableRecord GetLanguage(this ShotStatusType p_Type)
        {
            switch (p_Type)
            {
                default:
                case ShotStatusType.None:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(0);
                case ShotStatusType.ShotPower:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(147000);
                case ShotStatusType.ShotCount:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(147001);
                case ShotStatusType.ShotSpeed:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(147002);
                case ShotStatusType.ShotScale:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(147003);
                case ShotStatusType.ShotDuration:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(147004);
                case ShotStatusType.ShotPierce:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(147005);
            }
        }
        
        public static string GetPropertyName(this ShotStatusType p_Type)
        {
            return GetLanguage(p_Type).Text;
        }

        #endregion
    }
}