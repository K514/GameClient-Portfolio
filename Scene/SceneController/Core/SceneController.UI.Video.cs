using UnityEngine;
using UnityEngine.Video;

#if !SERVER_DRIVE

namespace k514.Mono.Common
{
    public partial class SceneController<Me, Phase, Sequence, Handler, Result>
    {
        #region <Fields>

        [SerializeField] private VideoPlayer _VideoPlayer;

        #endregion

        #region <Callbacks>

        private void OnCreateVideo()
        {
        }

        #endregion

        #region <Methods>
        
        public void SetPlayVideo()
        {
            try
            {
                _VideoPlayer.Play();
            }
            catch
            {
                //
            }
        }

        #endregion
    }
}
#endif
