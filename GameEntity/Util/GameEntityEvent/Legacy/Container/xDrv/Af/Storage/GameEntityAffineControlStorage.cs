using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace k514.Mono.Common
{
    public partial class GameEntityAffineControlStorage : AsyncSingleton<GameEntityAffineControlStorage>
    {
        #region <Fields>

        private Dictionary<int, GameEntityAffineControlEventBase> _EntityEventTable;

        #endregion

        #region <Callbacks>

        protected override async UniTask OnCreated(CancellationToken p_CancellationToken)
        {
            _EntityEventTable = new Dictionary<int, GameEntityAffineControlEventBase>();

            OnCreateCommon();
            OnCreateRotation();
            OnCreatePhysics();
            
            await UniTask.CompletedTask;
        }

        protected override async UniTask OnInitiate(CancellationToken p_CancellationToken)
        {
            await UniTask.CompletedTask;
        }

        #endregion

        #region <Methods>
        
        public bool TryGetAffineControlEvent(int p_Index, out GameEntityAffineControlEventBase o_Event)
        {
            return _EntityEventTable.TryGetValue(p_Index, out o_Event);
        }

        #endregion
    }
}