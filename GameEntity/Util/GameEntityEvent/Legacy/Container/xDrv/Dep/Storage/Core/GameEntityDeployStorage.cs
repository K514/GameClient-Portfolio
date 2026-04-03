using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using k514.Mono.Feature;
using UnityEngine;

namespace k514.Mono.Common
{
    public partial class GameEntityDeployStorage : AsyncSingleton<GameEntityDeployStorage>
    {
        #region <Fields>

        private Dictionary<int, GameEntityDeployEventBase> _EntityEventTable;

        #endregion

        #region <Callbacks>

        protected override void TryInitializeDependency()
        {
            base.TryInitializeDependency();
        }
        
        protected override async UniTask OnCreated(CancellationToken p_CancellationToken)
        {
            _EntityEventTable = new Dictionary<int, GameEntityDeployEventBase>();

            OnCreateCommon();
            OnCreateHero();
            OnCreateBoss();
            
            await UniTask.CompletedTask;
        }

        protected override async UniTask OnInitiate(CancellationToken p_CancellationToken)
        {
            await UniTask.CompletedTask;
        }

        #endregion

        #region <Methods>

        public bool TryGetDeployEvent(int p_Index, out GameEntityDeployEventBase o_DeployEvent)
        {
            return _EntityEventTable.TryGetValue(p_Index, out o_DeployEvent);
        }
        
        public void PreloadDeployEvent(int p_Index)
        {
            if (_EntityEventTable.TryGetValue(p_Index, out var o_EntityEvent))
            {
                o_EntityEvent.PreloadDeployEvent();
            }
        }
        
        public bool IsRunnable(GameEntityDeployEventContainer p_Container)
        {
            if (_EntityEventTable.TryGetValue(p_Container.EventId, out var o_EntityEvent))
            {
                if (o_EntityEvent.IsRunnable(p_Container))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        
        public async UniTask<bool> RunAction(GameEntityDeployEventContainer p_Container, CancellationToken p_CancellationToken)
        {
            if (_EntityEventTable.TryGetValue(p_Container.EventId, out var o_EntityEvent))
            {
                if (o_EntityEvent.IsRunnable(p_Container))
                {
                    await o_EntityEvent.RunEvent(p_Container, p_CancellationToken);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        #endregion
    }
}