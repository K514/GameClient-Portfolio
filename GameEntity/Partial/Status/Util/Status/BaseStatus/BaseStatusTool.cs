using System;
using System.Collections.Generic;

namespace k514.Mono.Common
{
    public static class BaseStatusTool
    {
        #region <Consts>
        
        public const float PropertyLowerBound = 0.01f;
        public static readonly BaseStatusPreset BasisBaseStatusPreset;
        public static readonly BaseStatusPreset EinBaseStatusPreset;

        #endregion
        
        #region <Enums>

        [Flags]
        public enum BaseStatusType
        {
            None = 0,
            
            /// <summary>
            /// 힘
            /// </summary>
            STR = 1 << 0,
            
            /// <summary>
            /// 지능
            /// </summary>
            INT = 1 << 1,
            
            /// <summary>
            /// 체력
            /// </summary>
            VIT = 1 << 2,
            
            /// <summary>
            /// 정신력
            /// </summary>
            WIL = 1 << 3,
            
            /// <summary>
            /// 민첩
            /// </summary>
            AGI = 1 << 5,
            
            /// <summary>
            /// 기량
            /// </summary>
            DEX = 1 << 4,
        }
        
        public static BaseStatusType[] BaseStatusTypeEnumerator;

        #endregion

        #region <Constructor>

        static BaseStatusTool()
        {
            BaseStatusTypeEnumerator = EnumFlag.GetEnumEnumerator<BaseStatusType>(EnumFlag.GetEnumeratorType.ExceptNone);
            
            var defaultMetaValueSet = new Dictionary<BaseStatusType, float>();
            foreach (var baseStatusType in BaseStatusTypeEnumerator)
            {
                switch (baseStatusType)
                {
                    default:
                    case BaseStatusType.None:
                        break;
                    case BaseStatusType.STR:
                    case BaseStatusType.INT:
                    case BaseStatusType.VIT:
                    case BaseStatusType.WIL:
                    case BaseStatusType.AGI:
                    case BaseStatusType.DEX:
                        defaultMetaValueSet.Add(baseStatusType, 0f);
                        break;
                }
            }

            BasisBaseStatusPreset = new BaseStatusPreset(defaultMetaValueSet);
            EinBaseStatusPreset = 1f;
        }

        #endregion

        #region <Methods>

#if !SERVER_DRIVE
        public static string GetPropertyName(BaseStatusType p_Type)
        {
            switch (p_Type)
            {
                default:
                case BaseStatusType.None:
                    return LanguageDataTableQuery.GetContent(0);;
                    
                case BaseStatusType.STR:
                    return LanguageDataTableQuery.GetContent(145000);
                case BaseStatusType.INT:
                    return LanguageDataTableQuery.GetContent(145001);
                case BaseStatusType.VIT:
                    return LanguageDataTableQuery.GetContent(145002);
                case BaseStatusType.WIL:
                    return LanguageDataTableQuery.GetContent(145003);
                case BaseStatusType.AGI:
                    return LanguageDataTableQuery.GetContent(145004);
                case BaseStatusType.DEX:
                    return LanguageDataTableQuery.GetContent(145005);
            }
        }
#endif

        #endregion
    }
}