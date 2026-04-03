#if !SERVER_DRIVE
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine.UI;

namespace k514.Mono.Common
{
    public partial class SceneController<Me, Phase, Sequence, Handler, Result>
    {
        #region <Callbacks>

        protected virtual void OnCreateUI()
        {
            var canvasScaler = GetComponent<CanvasScaler>();
            if (!ReferenceEquals(null, canvasScaler))
            {
                canvasScaler.InitCanvasScale();
            }

            OnCreateFader();
            OnCreateProgress();
            OnCreateVideo();
        }
        
        #endregion
    }
}
#endif
