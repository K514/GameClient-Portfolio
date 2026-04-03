using System.Threading;
using Cysharp.Threading.Tasks;
using k514.Mono.Feature;
using UnityEngine;

namespace k514.Mono.Common
{
    public struct GameEntityDeployEventContainerActivateParams : IGameEntityEventContainerActivateParams
    {
        #region <Fields>

        public IGameEntityBridge Caster => DeployParams.Caster;
        public int EventId { get; }
        public GameEntityEventCommonParams CommonParams { get; }
        public readonly GameEntityDeployTool.GameEntityDeployParams DeployParams;
        public readonly bool ValidFlag;
        
        #endregion

        #region <Constructors>

        public GameEntityDeployEventContainerActivateParams(int p_Id, IGameEntityBridge p_Caster, GameEntityEventCommonParams p_Params = default)
        {
            EventId = p_Id;
            DeployParams = new GameEntityDeployTool.GameEntityDeployParams(p_Caster, Vector3.zero);
            CommonParams = p_Params;
            ValidFlag = true;
        }

        #endregion
    }
    
    public class GameEntityDeployEventContainer : GameEntityEventContainer<GameEntityDeployEventContainer, ObjectCreateParams, GameEntityDeployEventContainerActivateParams, DashActionDataTable.TableRecord>
    {
        #region <Fields>

        public GameEntityDeployTool.GameEntityDeployParams DeployParams => _ActivateParams.DeployParams;
        
        #endregion
        
        #region <Methods>

        protected override void OnContainerTerminate()
        {
        }

        protected override bool CheckContainerTerminate()
        {
            return false;
        }
        
        protected override void InitCancellationToken()
        {
            GameEntityDeployStorage.GetInstanceUnsafe.GetLinkedCancellationTokenSource(ref _CancellationTokenSource);
            _CancellationToken = _CancellationTokenSource.Token;
        }

        public async UniTask RunAction()
        {
            if (RunContainer())
            {
                await GameEntityDeployStorage.GetInstanceUnsafe.RunAction(this, _CancellationToken);
            }
            CancelContainer();
        }
        
        #endregion
    }
}