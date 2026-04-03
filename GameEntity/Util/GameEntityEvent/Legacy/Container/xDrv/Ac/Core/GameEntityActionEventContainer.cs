using UnityEngine;

namespace k514.Mono.Common
{
    public struct GameEntityActionEventContainerActivateParams : IGameEntityEventContainerActivateParams
    {
        #region <Fields>

        public IGameEntityBridge Caster { get; }
        public int EventId { get; }
        public GameEntityEventCommonParams CommonParams { get; }

        #endregion
        
        #region <Constructors>

        public GameEntityActionEventContainerActivateParams(IGameEntityBridge p_Caster, int p_Id, GameEntityEventCommonParams p_CommonParams = default)
        {
            Caster = p_Caster;
            EventId = p_Id;
            CommonParams = p_CommonParams;
        }

        #endregion
    }
    
    public class GameEntityActionEventContainer : GameEntityEventContainer<GameEntityActionEventContainer, ObjectCreateParams, GameEntityActionEventContainerActivateParams, ITableRecord>
    {
        #region <Callbacks>

        protected override void OnContainerTerminate()
        {
        }

        #endregion
        
        #region <Methods>

        protected override void InitCancellationToken()
        {
            Caster.GetLinkedCancellationTokenSource(ref _CancellationTokenSource);
            _CancellationToken = _CancellationTokenSource.Token;
        }

        #endregion
    }

}