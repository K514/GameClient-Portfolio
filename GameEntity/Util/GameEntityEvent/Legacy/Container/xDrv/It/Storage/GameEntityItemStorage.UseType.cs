using UnityEngine;

namespace k514.Mono.Common
{
    public partial class GameEntityItemStorage
    {
        #region <Callbacks>

        private void OnCreateItemUseType()
        {
            _EntityItemTable.Add(GameEntityItemTool.ItemUseType.None, new CannotUseItem());
            _EntityItemTable.Add(GameEntityItemTool.ItemUseType.CannotUse, new CannotUseItem());
            _EntityItemTable.Add(GameEntityItemTool.ItemUseType.Equip, new EquipmentItem());
            _EntityItemTable.Add(GameEntityItemTool.ItemUseType.Consume, new ConsumeItem());
        }

        #endregion

        #region <Classess>
        
        /// <summary>
        /// 장착불가 아이템
        /// </summary>
        public class CannotUseItem : GameEntityItemEventBase
        {
            public override bool IsUsable(GameEntityItemEventContainer p_Handler)
            {
                return false;
            }

            public override GameEntityItemTool.EquipItemResult UseItem(GameEntityItemEventContainer p_Handler)
            {
                return GameEntityItemTool.EquipItemResult.EquipFail;
            }
        }
        
        /// <summary>
        /// 장착 아이템
        /// </summary>
        public class EquipmentItem : GameEntityItemEventBase
        {
            public override GameEntityItemTool.EquipItemResult UseItem(GameEntityItemEventContainer p_Handler)
            {
                var itemRecord = p_Handler.Record;
                var itemOptionSet = itemRecord.ExtraOptionIndexList;
                if (itemOptionSet.CheckCollectionSafe())
                {
                    foreach (var itemOptionIndex in itemOptionSet)
                    {
                        p_Handler.TryAddExtraOptionHandler(itemOptionIndex, default, out var o_Handler);
                    }
                }
                
                return GameEntityItemTool.EquipItemResult.Equipped;
            }
        }
        
        /// <summary>
        /// 소모품 아이템
        /// </summary>
        public class ConsumeItem : GameEntityItemEventBase
        {
            public override GameEntityItemTool.EquipItemResult UseItem(GameEntityItemEventContainer p_Handler)
            {
                var itemRecord = p_Handler.Record;
                var itemOptionSet = itemRecord.ExtraOptionIndexList;
                if (itemOptionSet.CheckCollectionSafe())
                {
                    foreach (var itemOptionIndex in itemOptionSet)
                    {
                        p_Handler.TryAddExtraOptionHandler(itemOptionIndex, default, out var o_Handler);
                    }
                }

                return GameEntityItemTool.EquipItemResult.Consumed;
            }
        }
        
        #endregion
    }
}