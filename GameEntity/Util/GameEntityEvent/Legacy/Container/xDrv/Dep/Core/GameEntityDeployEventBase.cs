using System.Threading;
using Cysharp.Threading.Tasks;
using k514.Mono.Feature;
using UnityEngine;

namespace k514.Mono.Common
{
    public abstract class GameEntityDeployEventBase
    {
        public virtual void PreloadDeployEvent()
        {
        }
        
        public virtual bool IsRunnable(GameEntityDeployEventContainer p_Handler)
        {
            return true;
        }

        public abstract UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken);
   
        
        public string GetDeployEventName(int p_Index, GameEntityEventCommonParams p_Params = default, GameEntityDeployTool.GameEntityDeployParams p_DeployParams = default)
        {
            {
                return string.Empty;
            }
        }
   
        public string GetDeployEventDescription(int p_Index, GameEntityEventCommonParams p_Params = default, GameEntityDeployTool.GameEntityDeployParams p_DeployParams = default)
        {
            {
                return string.Empty;
            }
        }
    }
}