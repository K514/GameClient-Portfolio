using UnityEngine.EventSystems;

#if !SERVER_DRIVE

namespace k514.Mono.Common
{
    /// <summary>
    /// UIxContainerBase 기본 구현체
    /// </summary>
    public class UIxDefaultJoystickContainer : UIxContainerBase, IDragHandler
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
            
            p_Element.SetHide(false);
        }

        #endregion

        #region <Methods>

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