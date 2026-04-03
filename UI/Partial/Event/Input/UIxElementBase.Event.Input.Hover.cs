#if !SERVER_DRIVE
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace k514.Mono.Common
{
    public partial class UIxElementBase : IPointerEnterHandler, IPointerExitHandler
    {
        #region <Fields>

        /// <summary>
        /// 현재 커서가 해당 UI 내부에 있는지 표시하는 플래그
        /// </summary>
        private bool _IsHovering;

        #endregion

        #region <Callbacks>

        public void OnPointerEnter(PointerEventData eventData)
        {
            _IsHovering = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _IsHovering = false;
        }
        
        #endregion

        #region <Methods>

        private void ResetInputHoverState()
        {
            _IsHovering = false;
        }
   
        public bool IsHovering()
        {
            return _IsHovering;
        }

        #endregion
    }
}
#endif