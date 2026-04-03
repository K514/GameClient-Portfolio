using k514.Mono.Feature;

namespace k514.Mono.Common
{
    public struct GameEntityItemEventContainerActivateParams : IGameEntityEventContainerActivateParams
    {
        #region <Fields>

        public IGameEntityBridge Caster { get; }
        public int EventId { get; }
        public GameEntityEventCommonParams CommonParams { get; }
        
        #endregion

        #region <Constructors>

        public GameEntityItemEventContainerActivateParams(IGameEntityBridge p_Caster, GameEntityItemTool.GameEntityItemUsePreset p_Params)
        {
            Caster = p_Caster;
            EventId = p_Params.ItemIndex;
            CommonParams = new GameEntityEventCommonParams(p_Params.Cost);
        } 

        #endregion
    }
    
    public class GameEntityItemEventContainer : GameEntityEventContainer<GameEntityItemEventContainer, ObjectCreateParams, GameEntityItemEventContainerActivateParams, IItemDataTableRecordBridge>
    {
        #region <Fields>

        public ItemDataTableQuery.TableLabel ItemType { get; private set; }
        public GameEntityItemTool.ItemUseType ItemUseType { get; private set; }
        
        #endregion
        
        #region <Callbacks>

        protected override bool OnActivate(ObjectCreateParams p_CreateParams, GameEntityItemEventContainerActivateParams p_ActivateParams, bool p_IsPooled)
        {
            if (base.OnActivate(p_CreateParams, p_ActivateParams, p_IsPooled))
            {
                Record = ItemDataTableQuery.GetInstanceUnsafe.GetRecord(EventId);
                // ItemType = ItemDataTableQuery.GetInstanceUnsafe.GetLabel(EventId);
                ItemUseType = default;
                
                return true;
            }
            else
            {
                return false;
            }
        }

        protected override void OnRetrieve(ObjectCreateParams p_CreateParams)
        {
            base.OnRetrieve(p_CreateParams);

            ItemType = default;
            ItemUseType = default;
        }

        protected override void OnContainerTerminate()
        {
            GameEntityItemStorage.GetInstanceUnsafe.TerminateItem(Record, this);
        }

        #endregion

        #region <Methods>

        protected override void InitCancellationToken()
        {
            GameEntityItemStorage.GetInstanceUnsafe.GetLinkedCancellationTokenSource(ref _CancellationTokenSource);
            _CancellationToken = _CancellationTokenSource.Token;
        }
        
        public bool IsUsable()
        {
            return GameEntityItemStorage.GetInstanceUnsafe.IsUsable(Record, this);
        }
        
        public GameEntityItemTool.EquipItemResult UseItem()
        {
            if (IsUsable())
            {
                var result = GameEntityItemStorage.GetInstanceUnsafe.UseItem(Record, this);
                switch (result)
                {
                    case GameEntityItemTool.EquipItemResult.Equipped:
                    case GameEntityItemTool.EquipItemResult.Consumed:
                    {
                        RunContainer();

                        return result;
                    }
                    default:
                    case GameEntityItemTool.EquipItemResult.EquipFail:
                    {
                        return result;
                    }
                }
            }
            else
            {
                Pooling();

                return GameEntityItemTool.EquipItemResult.EquipFail;
            }
        }
   
        public bool DiscardItem()
        {
            if (GameEntityItemStorage.GetInstanceUnsafe.IsDiscardible(Record, this))
            {
                CancelContainer();

                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion
    }
}