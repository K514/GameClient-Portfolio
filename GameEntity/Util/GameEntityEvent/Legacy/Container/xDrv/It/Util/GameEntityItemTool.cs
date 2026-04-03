using System;
using k514.Mono.Feature;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace k514.Mono.Common
{
    public static class GameEntityItemTool
    {
        #region <Consts>

        public const int _MagicStoneIndex = 90000;
        public const int _GoldIndex = 90001;
        public const int _CashIndex = 90002;
        public const int _NormalTicketIndex = 90010;

        #endregion
        
        #region <Enums>

        [Flags]
        public enum StageItemAppearanceConditionFlag
        {
            None = 0,
            
            StageWaveLower = 1 << 0,
            StageWaveUpper = 1 << 1,
            BossClear = 1 << 2,
            NoHitRun = 1 << 3,
            NotOverlapOtherSlot = 1 << 4,
            NotHasItem = 1 << 5,
            PassiveLevel = 1 << 6,
            HasAnyItem = 1 << 7,
            HasAnyPassive = 1 << 8,
        }
        
        public enum ItemRankType
        {
            None,
            Normal,
            Magic,
            Rare,
            Unique,
            Legendary,
            Epic,
            Fantasm,
        }
        
        public enum ItemUseType
        {
            None,
            
            /// <summary>
            /// 사용 불가능. 효과 없는 아이템.
            /// </summary>
            CannotUse,
            
            /// <summary>
            /// 장비 아이템. 패시브 스킬처럼 소지하는 것만으로 효과가 적용됨.
            /// </summary>
            Equip,
            
            /// <summary>
            /// 소모품 아이템. 슬롯에서 사용을 하면 효과가 적용됨. 효과 적용 후 삭제.
            /// </summary>
            Consume,
        }
        
        public enum EquipItemResult
        {
            Equipped,
            Consumed,
            EquipFail,
        }

        #endregion
        
        #region <Structs>

        [Serializable]
        public struct GameEntityItemAppearanceConditionPreset
        {
            #region <Fields>

            public StageItemAppearanceConditionFlag Flag;
            public int Count;

            #endregion
            
            #region <Constructor>

            public GameEntityItemAppearanceConditionPreset(StageItemAppearanceConditionFlag p_Flag)
            {
                this = default;
                
                Flag = p_Flag;
            }
            
            public GameEntityItemAppearanceConditionPreset(StageItemAppearanceConditionFlag p_Flag, int p_Count)
            {
                this = default;
                
                Flag = p_Flag;
                Count = p_Count;
            }
            
            #endregion
        }
        
        [Serializable]
        public struct GameEntityDropItemPreset
        {
            #region <Fields>

            public int ItemIndex;
            public int StartQuantity;
            public int EndQuantity;
            [TableTool.TableRecordAttribute(TableTool.TableRecordAttributeType.Runtime)]
            private readonly bool IsRandomQuantity;
            
            #endregion

            #region <Constructor>

            public GameEntityDropItemPreset(int p_Index, int p_Quantity = 1)
            {
                ItemIndex = p_Index;
                StartQuantity = p_Quantity;
                EndQuantity = default;
                IsRandomQuantity = false;
            }
            
            public GameEntityDropItemPreset(int p_Index, int p_StartQuantity, int p_EndQuantity)
            {
                ItemIndex = p_Index;
                StartQuantity = p_StartQuantity;
                EndQuantity = p_EndQuantity;
                IsRandomQuantity = true;
            }

            #endregion

            #region <Methods>

            public GameEntityItemPreset GetItemPreset()
            {
                if (IsRandomQuantity)
                {
                    return new GameEntityItemPreset(ItemIndex, Random.Range(StartQuantity, EndQuantity + 1));
                }
                else
                {
                    return new GameEntityItemPreset(ItemIndex, StartQuantity);
                }
            }

            #endregion
        }
        
        [Serializable]
        public struct GameEntityItemPreset
        {
            #region <Fields>

            public int ItemIndex;
            public int Number;

            #endregion

            #region <Constructor>

            public GameEntityItemPreset(int p_Index, int p_Number = 1)
            {
                ItemIndex = p_Index;
                Number = p_Number;
            }

            #endregion

            #region <Operator>

#if UNITY_EDITOR
            public override string ToString()
            {
                return $"{ItemDataTableQuery.GetInstanceUnsafe.GetRecord(ItemIndex).GetLanguage().Text} x {Number}";
            }
#endif

            #endregion
        }
        
        [Serializable]
        public struct GameEntityItemUsePreset
        {
            #region <Fields>

            public int ItemIndex;
            public int Cost;

            #endregion
            
            #region <Constructor>

            public GameEntityItemUsePreset(int p_Index, int p_Value = 0)
            {
                ItemIndex = p_Index;
                Cost = p_Value;
            }

            #endregion
        }
        
        #endregion
        
        #region <Classess>

        public class GameItemSlot : _IDisposable
        {
            #region <Finalizer>

            ~GameItemSlot()
            {
                Dispose();
            }

            #endregion
            
            #region <Fields>

            /// <summary>
            /// 아이템 소유개체
            /// </summary>
            public IGameEntityBridge Entity;
            
            /// <summary>
            /// 아이템 인덱스
            /// </summary>
            public int ItemRecordIndex;

            /// <summary>
            /// 아이템 개수
            /// </summary>
            public int ItemNumber;

            /// <summary>
            /// 아이템 레코드
            /// </summary>
            public IItemDataTableRecordBridge ItemRecord;

            /// <summary>
            /// 아이템 타입
            /// </summary>
            public ItemUseType ItemUseType;

            /// <summary>
            /// 아이템 쿨타임 그룹 인덱스
            /// </summary>
            public int ItemCooldownGroupIndex;

            /// <summary>
            /// 쿨타임 타이머
            /// </summary>
            public ProgressTimerWrap CooldownTimer;

            #endregion

            #region <Constructor>

            public GameItemSlot(IGameEntityBridge p_Entity, int p_Index, int p_Number = 1)
            {
                Entity = p_Entity;
                ItemRecordIndex = p_Index;
                ItemNumber = p_Number;
                
                if (ItemDataTableQuery.GetInstanceUnsafe.TryGetRecordBridge(ItemRecordIndex, out ItemRecord))
                {
                    ItemUseType = default;
                    ItemCooldownGroupIndex = ItemRecord.ItemCooldownGroupIndex;
                }
            }

            #endregion
            
            #region <Callbacks>

            /// <summary>
            /// 인스턴스가 파기될 때 수행할 작업을 기술한다.
            /// </summary>
            private void OnDisposeUnmanaged()
            {
                ItemRecordIndex = default;
                ItemNumber = default;
                ItemRecord = default;
                ItemUseType = default;
                ItemCooldownGroupIndex = default;
                CooldownTimer = default;
            }

            #endregion
            
            #region <Methods>

            public void AddNumber(int p_Number)
            {
                ItemNumber += p_Number;

                if (ItemNumber < 1)
                {
                    Entity.RemoveSlot(ItemRecordIndex);
                }
                else
                {
                    Entity.ReserveInventoryUpdate();
                }
            }

            public void SetTimer(ProgressTimerWrap p_Timer)
            {
                CooldownTimer = p_Timer;
            }

            public bool UseItem()
            {
                switch (ItemUseType)
                {
                    case ItemUseType.Consume:
                    {
                        if (CooldownTimer.IsOver() && ItemNumber > 0)
                        {
                            /*var result = Entity.UseItem(new GameEntityItemUsePreset(ItemRecordIndex));
                            switch (result)
                            {
                                case EquipItemResult.Consumed:
                                    CooldownTimer.Reset();
                                    Entity.SendEvent(GameEntityTool.GameEntityBaseEventType.Activate_Item, new GameEntityBaseEventParams(Entity));
                                    AddNumber(-1);
                                    return true;
                            }*/
                        }
                        break;
                    }
                }
                
                return false;
            }
            
            #endregion

            #region <Disposable>
        
            /// <summary>
            /// dispose 패턴 onceFlag
            /// </summary>
            public bool IsDisposed { get; private set; }
        
            /// <summary>
            /// dispose 플래그를 초기화 시키는 메서드
            /// </summary>
            public void Rejuvenate()
            {
                IsDisposed = false;
            }

            /// <summary>
            /// 인스턴스 파기 메서드
            /// </summary>
            public void Dispose()
            {
                if (IsDisposed)
                {
                    return;
                }
                else
                {
                    IsDisposed = true;
                    OnDisposeUnmanaged();
                }
            }

            #endregion
        }
            
        #endregion  
    }
}