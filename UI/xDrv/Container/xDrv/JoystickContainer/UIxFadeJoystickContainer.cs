#if !SERVER_DRIVE

using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.EventSystems;

namespace k514.Mono.Common
{
    /// <summary>
    /// 입력 이벤트 발생 시, 포함되어 있는 UIxElement들을 페이드 인/아웃 연출로 제어하는 컨테이너
    /// </summary>
    public class UIxFadeJoystickContainer : UIxContainerBase, IDragHandler
    {
        #region <Callbacks>

        protected override void OnCreate(UIPoolManager.CreateParams p_CreateParams)
        {
            base.OnCreate(p_CreateParams);

            SetStateFlag(UIxTool.UIxStaticStateType.ReleaseInputEventIndependentHover);
        }
        
        protected override bool OnActivate(UIPoolManager.CreateParams p_CreateParams, UIPoolManager.ActivateParams p_ActivateParams, bool p_IsPooled)
        {
            if (base.OnActivate(p_CreateParams, p_ActivateParams, p_IsPooled))
            {
                SetHide(true);

                return true;
            }
            else
            {
                return false;
            }
        }
        
        protected override void OnElementAdded(UIxElementBase p_Element)
        {
            base.OnElementAdded(p_Element);
            
            p_Element.SetHide(true);
        }

        #endregion

        #region <Methods>
        
        protected override void CastPointerDownEvent(PointerEventData p_EventData)
        {
            base.CastPointerDownEvent(p_EventData);
            
            foreach (var slaveNode in _Elements)
            {
                slaveNode.RectTransform.position = p_EventData.position;
                slaveNode.SetFadeIn(UIxTool.UIAfterFadeType.None, false);
                slaveNode.OnPointerDown(p_EventData);
            }
        }

        protected override void CastPointerUpEvent(PointerEventData p_EventData)
        {
            base.CastPointerUpEvent(p_EventData);
            
            foreach (var slaveNode in _Elements)
            {
                slaveNode.SetFadeOut(UIxTool.UIAfterFadeType.Hide, false);
                slaveNode.OnPointerUp(p_EventData);
            }
        }
        
        /// <summary>
        /// 해당 컨테이너의 하위 엘리먼트들은 OnPointer 콜백을 통해 제어받아야 하므로
        /// 오버라이드로 하위 엘리먼트 간섭을 지워준다.
        /// </summary>
        protected override void SetElementHide(bool p_HideFlag)
        {
        }

        #endregion
    }
}
#endif