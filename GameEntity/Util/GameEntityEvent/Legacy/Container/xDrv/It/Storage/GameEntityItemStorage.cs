using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using k514.Mono.Feature;

namespace k514.Mono.Common
{
    public partial class GameEntityItemStorage : AsyncSingleton<GameEntityItemStorage>
    {
        #region <Fields>

        private Dictionary<GameEntityItemTool.ItemUseType, GameEntityItemEventBase> _EntityItemTable;

        #endregion

        #region <Callbacks>

        protected override void TryInitializeDependency()
        {
            base.TryInitializeDependency();

            _Dependencies.Add(typeof(ItemDataTableQuery));
            _Dependencies.Add(typeof(MaterialItemDataTableQuery));
        }

        protected override async UniTask OnCreated(CancellationToken p_CancellationToken)
        {
            _EntityItemTable = new Dictionary<GameEntityItemTool.ItemUseType, GameEntityItemEventBase>();

            OnCreateItemUseType();
            
            await UniTask.CompletedTask;
        }

        protected override async UniTask OnInitiate(CancellationToken p_CancellationToken)
        {
            await UniTask.CompletedTask;
        }

        #endregion

        #region <Methods>

        public bool IsUsable(IItemDataTableRecordBridge p_Record, GameEntityItemEventContainer p_Handler)
        {
            if (_EntityItemTable.TryGetValue(default, out var o_Item))
            {
                return o_Item.IsUsable(p_Handler);
            }
            else
            {
                return false;
            }
        }
        
        public GameEntityItemTool.EquipItemResult UseItem(IItemDataTableRecordBridge p_Record, GameEntityItemEventContainer p_Handler)
        {
            if (_EntityItemTable.TryGetValue(default, out var o_Item))
            {
                return o_Item.UseItem(p_Handler);
            }
            else
            {
                return GameEntityItemTool.EquipItemResult.EquipFail;
            }
        }
        
        public bool IsDiscardible(IItemDataTableRecordBridge p_Record, GameEntityItemEventContainer p_Handler)
        {
            if (_EntityItemTable.TryGetValue(default, out var o_Item))
            {
                return o_Item.IsDiscardible(p_Handler);
            }
            else
            {
                return false;
            }
        }
        
        public bool TerminateItem(IItemDataTableRecordBridge p_Record, GameEntityItemEventContainer p_Handler)
        {
            if (_EntityItemTable.TryGetValue(default, out var o_Item))
            {
                return o_Item.TerminateItem(p_Handler);
            }
            else
            {
                return false;
            }
        }

#endregion
    }
}