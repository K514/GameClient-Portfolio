using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using k514.Mono.Feature;

namespace k514.Mono.Common
{
    public partial class GameEntityExtraOptionStorage : AsyncSingleton<GameEntityExtraOptionStorage>
    {
        #region <Fields>

        private Dictionary<GameEntityExtraOptionTool.ExtraOptionType, GameEntityExtraOptionBase> _ExtraOptionTable;

        #endregion
        
        #region <Callbacks>

        protected override void TryInitializeDependency()
        {
            base.TryInitializeDependency();

            _Dependencies.Add(typeof(ExtraOptionDataTable));
        }
        
        protected override async UniTask OnCreated(CancellationToken p_CancellationToken)
        {
            _ExtraOptionTable = new Dictionary<GameEntityExtraOptionTool.ExtraOptionType, GameEntityExtraOptionBase>();

            OnCreateOptionA();
            OnCreateOptionB();
            OnCreateOptionC();
            OnCreateOptionD();
            
            await UniTask.CompletedTask;
        }

        protected override async UniTask OnInitiate(CancellationToken p_CancellationToken)
        {
            await UniTask.CompletedTask;
        }
        
        #endregion

        #region <Methods>
         
        public bool TryGetExtraOption(GameEntityExtraOptionTool.ExtraOptionType p_Type, out GameEntityExtraOptionBase o_ItemOption)
        {
            return _ExtraOptionTable.TryGetValue(p_Type, out o_ItemOption);
        }

        #endregion
    }
}