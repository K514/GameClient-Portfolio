using System.Collections.Generic;

namespace k514.Mono.Common
{
    public partial class GameEntityBase<Content, CreateParams, ActivateParams>
    {
        #region <Fields>

        /// <summary>
        /// 값이 변한 샷 능력치 프리셋 리스트
        /// </summary>
        private List<StatusTool.ShotStatusChangeResult> _ShotStatusChangeResultList;
        
        /// <summary>
        /// 값이 변한 샷 능력치 프리셋 리스트 버퍼
        /// </summary>
        private List<StatusTool.ShotStatusChangeResult> _ShotStatusChangeResultListMirror;
        
        #endregion
        
        #region <Callbacks>

        private void OnCreateShotStatusChanged()
        {
            _ShotStatusChangeResultList = new List<StatusTool.ShotStatusChangeResult>(GameConst.__CAPACITY_STATUS_CHANGE);
            _ShotStatusChangeResultListMirror = new List<StatusTool.ShotStatusChangeResult>(GameConst.__CAPACITY_STATUS_CHANGE);
        }
        
        private void OnShotStatusChanged()
        {
            _ShotStatusChangeResultList.Clear();
        }

        #endregion
    }
}