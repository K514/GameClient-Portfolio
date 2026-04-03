using UnityEngine;
 
namespace k514.Mono.Common
{
    public static partial class GameEntityTool
    {
        #region <Fields>

        /// <summary>
        /// 전투 발생후 전투 정보 유효시간
        /// </summary>
        public const float __CombatInfo_ValidTime_UpperBound = 15f;
        
        #endregion

        #region <Constructor>
 
        static GameEntityTool()
        {
            AttributeTypeEnumerator = EnumFlag.GetEnumEnumerator<GameEntityAttributeType>(EnumFlag.GetEnumeratorType.ExceptMaskNone);
            ElementTypeEnumerator = EnumFlag.GetEnumEnumerator<ElementType>(EnumFlag.GetEnumeratorType.ExceptNone);
            ObjectAddForceProcessTypeEnumerator = EnumFlag.GetEnumEnumerator<ForceControlType>(EnumFlag.GetEnumeratorType.ExceptNone);
            ObjectEnvironmentSoundTypeEnumerator = EnumFlag.GetEnumEnumerator<EnvironmentSoundType>(EnumFlag.GetEnumeratorType.ExceptNone);
            StateEnumerator = EnumFlag.GetEnumEnumerator<EntityStateType>(EnumFlag.GetEnumeratorType.ExceptMaskNone);
            RenderTypeEnumerator = EnumFlag.GetEnumEnumerator<GameEntityRenderType>(EnumFlag.GetEnumeratorType.ExceptMaskNone);
            GameEntityBaseEventTypeEnumerator = EnumFlag.GetEnumEnumerator<GameEntityBaseEventType>(EnumFlag.GetEnumeratorType.ExceptMaskNone);
            GameEntityUIEventTypeEnumerator = EnumFlag.GetEnumEnumerator<GameEntityUIEventType>(EnumFlag.GetEnumeratorType.ExceptMaskNone);
            GameEntityInnerModuleEventTypeEnumerator = EnumFlag.GetEnumEnumerator<GameEntityModuleEventType>(EnumFlag.GetEnumeratorType.ExceptMaskNone);
            LateEventTypeEnumeraotr = EnumFlag.GetEnumEnumerator<LateEventType>(EnumFlag.GetEnumeratorType.ExceptNone);
            ActivateParamsAttributeTypeEnumerator = EnumFlag.GetEnumEnumerator<ActivateParamsAttributeType>(EnumFlag.GetEnumeratorType.ExceptNone);
        }
 
        #endregion
 
        #region <Methods>

        public static bool HasUnitLayer(this GameObject p_Entity)
        {
            var tryLayer = p_Entity.layer;

            return tryLayer == GameConst.Unit_A_Layer
                   || tryLayer == GameConst.Unit_B_Layer
                   || tryLayer == GameConst.Unit_C_Layer;
        }
         
        public static bool IsUnitLayer(this GameConst.GameLayerType p_Entity)
        {
            return p_Entity == GameConst.GameLayerType.UnitA
                   || p_Entity == GameConst.GameLayerType.UnitB
                   || p_Entity == GameConst.GameLayerType.UnitC;
        }
        
        #endregion
    }
}