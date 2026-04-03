#if !SERVER_DRIVE
using System;
using Unity.VisualScripting;
using UnityEngine.Events;
using UnityEngine.UI;
using UniRx;

namespace k514.Mono.Common
{
    public partial class UIxTool
    {
        /// <summary>
        /// 지정한 버튼의 클릭이벤트에 이벤트 대리자를 등록시켜주는 메서드
        /// </summary>
        public static void SetButtonHandler(this Button p_TargetButton, UnityAction p_HandleEvent)
        {
            var tryHandler = new Button.ButtonClickedEvent();
            tryHandler.AddListener(p_HandleEvent);
            p_TargetButton.onClick = tryHandler;
        }
        
        public static void ClearButtonHandler(this Button p_TargetButton)
        {
            p_TargetButton?.onClick?.RemoveAllListeners();
        }

        public static void SetDoubleClickHandler(this Button p_TargetButton, UnityAction p_HandleEvent)
        {
            var clickStream = p_TargetButton.OnClickAsObservable();
            clickStream.Buffer(clickStream.Throttle(TimeSpan.FromMilliseconds(500))).Where(x => x.Count >= 2).Subscribe(_ => p_HandleEvent?.Invoke());
        }
    }
}
#endif