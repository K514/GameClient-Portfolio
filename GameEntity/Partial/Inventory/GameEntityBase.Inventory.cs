using System.Collections.Generic;
using k514.Mono.Feature;
using xk514;

namespace k514.Mono.Common
{
    public partial class GameEntityBase<Content, CreateParams, ActivateParams>
    {
        #region <Fields>

        /// <summary>
        /// [아이템 인덱스, 아이템 슬롯] 테이블
        /// </summary>
        private Dictionary<int, GameEntityItemTool.GameItemSlot> _ItemInventory;
        
        /// <summary>
        /// [아이템 그룹 쿨타임 인덱스, 타이머] 테이블
        /// </summary>
        private Dictionary<int, ProgressTimerWrap> _ItemGroupCooldownTable;
        
        #endregion
        
        #region <Callbacks>

        private void OnCreateEntityInventory()
        {
            _ItemInventory = new Dictionary<int, GameEntityItemTool.GameItemSlot>();
            _ItemGroupCooldownTable = new Dictionary<int, ProgressTimerWrap>();
        }

        private void OnActivateEntityInventory(ActivateParams p_ActivateParams)
        {
            foreach (var timerKV in _ItemGroupCooldownTable)
            {
                var timer = timerKV.Value;
                timer.Terminate();
            }
        }
        
        private void OnRetrieveEntityInventory()
        {
            ClearInventory();
        }
        
        private void OnUpdateInventory(float p_DeltaTime)
        {
            foreach (var timerKV in _ItemGroupCooldownTable)
            {
                var timer = timerKV.Value;
                if (timer.IsOver())
                {
                    
                }
                else
                {
                    timer.Progress(p_DeltaTime);
                }
            }
        }
        
        #endregion

        #region <Methods>

        public bool HasItem(int p_ItemIndex)
        {
            return _ItemInventory.ContainsKey(p_ItemIndex);
        }
        
        public void SetItem(int p_ItemIndex, int p_Number = 1)
        {
            if (_ItemInventory.TryGetValue(p_ItemIndex, out var o_Slot))
            {
                o_Slot.AddNumber(p_Number - o_Slot.ItemNumber);
            }
            else
            {
                SetSlot(p_ItemIndex, p_Number);
            }
        }

        public void AddItem(int p_ItemIndex, int p_Number = 1)
        {
            if (_ItemInventory.TryGetValue(p_ItemIndex, out var o_Slot))
            {
                o_Slot.AddNumber(p_Number);
            }
            else
            {
                SetSlot(p_ItemIndex, p_Number);
            }
        }

        private void SetSlot(int p_ItemIndex, int p_Number = 1)
        {
            if (p_Number > 0)
            {
                var itemSlot = new GameEntityItemTool.GameItemSlot(this, p_ItemIndex, p_Number);
                _ItemInventory.Add(p_ItemIndex, itemSlot);
                ReserveInventoryUpdate();

                var itemType = itemSlot.ItemUseType;
                switch (itemType)
                {
                    case GameEntityItemTool.ItemUseType.None:
                    case GameEntityItemTool.ItemUseType.CannotUse:
                    case GameEntityItemTool.ItemUseType.Equip:
                    {
                        itemSlot.UseItem();
                        break;
                    }
                    case GameEntityItemTool.ItemUseType.Consume:
                    {
                        var cooldownGroupIndex = itemSlot.ItemCooldownGroupIndex;
                        if (_ItemGroupCooldownTable.TryGetValue(cooldownGroupIndex, out var o_Timer))
                        {
                            itemSlot.SetTimer(o_Timer);
                        }
                        else
                        {
                            if (ItemGroupCooldownDataTable.GetInstanceUnsafe.TryGetRecord(cooldownGroupIndex, out var o_Record))
                            {
                                o_Timer = new ProgressTimerWrap(o_Record.Duration);
                                o_Timer.Terminate();
                                _ItemGroupCooldownTable.Add(cooldownGroupIndex, o_Timer);
                                itemSlot.SetTimer(o_Timer);
                            }
                        }
                        break;
                    }
                }
            }
        }
       
        public void RemoveSlot(int p_Index)
        {
            if (_ItemInventory.TryGetValue(p_Index, out var o_Slot))
            {
                o_Slot.Dispose();
                _ItemInventory.Remove(p_Index);
                ReserveInventoryUpdate();
            }
        }
        
        public void ClearInventory()
        {
            foreach (var slotKV in _ItemInventory)
            {
                var slot = slotKV.Value;
                slot.Dispose();
            }
            _ItemInventory.Clear();
            ReserveInventoryUpdate();
        }

        public Dictionary<int, GameEntityItemTool.GameItemSlot> GetInventory()
        {
            return _ItemInventory;
        }
        
        public int GetInventoryCount()
        {
            return _ItemInventory.Count;
        }
        
        public bool TryGetItemSlot(int p_Index, out GameEntityItemTool.GameItemSlot o_Slot)
        {
            return _ItemInventory.TryGetValue(p_Index, out o_Slot);
        }

#if UNITY_EDITOR
        public void PrintInventoryInfo()
        {
            CustomDebug.LogError($" Inventory List (Count : {_ItemInventory.Count})");
            foreach (var slotKV in _ItemInventory)
            {
                var slot = slotKV.Value;
                CustomDebug.LogError($"[{slot.ItemRecord.GetLanguage().Text}, {slot.ItemNumber}]");
            }
        }
#endif
        
        #endregion
    }
}