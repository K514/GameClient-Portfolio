using System.Collections.Generic;

namespace k514.Mono.Common
{
    public partial class GameEntityBase<Content, CreateParams, ActivateParams>
    {
        #region <Fields>

        /// <summary>
        /// 값이 변한 기저 능력치 프리셋 리스트
        /// </summary>
        private List<StatusTool.BaseStatusChangeResult> _BaseStatusChangeResultList;
        
        /// <summary>
        /// 값이 변한 기저 능력치 프리셋 리스트 버퍼
        /// </summary>
        private List<StatusTool.BaseStatusChangeResult> _BaseStatusChangeResultListMirror;
        
        #endregion
        
        #region <Callbacks>

        private void OnCreateBaseStatusChanged()
        {
            _BaseStatusChangeResultList = new List<StatusTool.BaseStatusChangeResult>(GameConst.__CAPACITY_STATUS_CHANGE);
            _BaseStatusChangeResultListMirror = new List<StatusTool.BaseStatusChangeResult>(GameConst.__CAPACITY_STATUS_CHANGE);
        }
        
        private void OnBaseStatusChanged()
        {
            _BaseStatusChangeResultList.Clear();
        }

        #endregion
    }
}