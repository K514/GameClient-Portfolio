#if !SERVER_DRIVE
using System;
using UnityEngine;
using UnityEngine.UI;
#endif

namespace k514.Mono.Common
{
    public static partial class UIxTool
    {
        #region <Enums>

#if !SERVER_DRIVE
        public enum UIxElementType
        {
            None = 0,
            
            Fader,
            
            HpBarTheater, HpBarPanel,
            NameTheater, NamePanel,
            NumberTheater, NumberPanel,
            GaugeTheater, GaugePanel,

            MainHUD,
            TouchPanel,
            PlayerState,
            Controller,
            SystemMenu,
            GameConfig,
            MainHUDPauseButton,
        }

        public enum UIAfterFadeType
        {
            None,
            Hide,
            Retrieve
        }

        public enum XAnchorType
        {
            Left,
            Center,
            Right,
            Stretch
        }

        public enum YAnchorType
        {
            Top,
            Middle,
            Bottom,
            Stretch
        }

        [Flags]
        public enum UIxStaticStateType
        {
            None = 0,
            
            HasAnimation = 1 << 0,
            HasMainImage = 1 << 1,
            HasMainText = 1 << 2,
            
            /// <summary>
            /// 입력시 해당 위치로 기준점이 이동한다.
            /// </summary>
            FloatPivotWhenPress = 1 << 3,
            
            /// <summary>
            /// 입력 및 입력 지속시 해당 위치로 RectAffine이 이동한다.
            /// </summary>
            FloatRect = 1 << 4,
            
            /// <summary>
            /// 입력이 해제되었을 때 기준점을 처음 위치로 되돌린다.
            /// </summary>
            ResetBaseWhenRelease = 1 << 5,
            
            /// <summary>
            /// 드래그 제스쳐의 경우에 이동방향은 해당 UI 원점으로부터 입력지점으로 제스처 방향을 잡는데,
            /// 해당 플래그를 활성화 하는 경우 바로 이전 입력과 현재 입력을 비교한 값을 제스처 방향으로 잡는다.
            /// </summary>
            UpdateDragDirectionUsingPointerData = 1 << 6,
            
            /// <summary>
            /// 드래그 입력 이벤트 송신 후에 해당 프레임의 제스쳐 정보를 초기화 시킨다.
            /// 해당 플래그가 활성화 되어 있는 경우 드래그 위치가 바뀐 경우에만 해당 벡터 정보가 반영된다.
            /// </summary>
            ResetInputDragEveryFrame = 1 << 7,
            
            /// <summary>
            /// 릴리스 이벤트가 호버링과 관계없이 발동하게 됨
            /// 기본적으로 호버링 상태일 때만 릴리스 이벤트 발동함
            /// </summary>
            ReleaseInputEventIndependentHover = 1 << 8,
        }
        
        [Flags]
        public enum UIxDynamicStateType
        {
            None = 0,
            
            PivotPosition = 1 << 3,
            Contained = 1 << 4,
            Hide = 1 << 5,
            
            DispatchInputEvent = 1 << 6,
            InteractEvent = 1 << 7,
            
            HasEventEntity = 1 << 10,
            HasEntityBaseEvent = 1 << 11,
            HasEntityUIEvent = 1 << 12,
            HasEntityRenderEvent = 1 << 13,
            
            DeferredHide = 1 << 20,
            DeferredRetrieve = 1 << 21,
            
            HasEvent = DispatchInputEvent | InteractEvent,
        }
        
        public static UIxDynamicStateType[] _UIDynamicStateTypeEnumerator;
        public static UIxDynamicStateType[] _UIDynamicStateTypeEnumerator_EntityEvent;

        [Flags]
        public enum UIxLateEventType
        {
            None = 0,
            
            Retrieve = 1 << 0,
            
            TurnVisible = 1 << 1,
            TurnHide = 1 << 2,
            TurnFadeIn = 1 << 3,
            TurnFadeOut = 1 << 4,
            
            LateUpdatePosition = 1 << 5,
            LateUpdateText = 1 << 6,
            LateUpdateColor = 1 << 7,
            LateUpdateImage = 1 << 8,
            LateUpdateRate = 1 << 9,
        }
        
        public static UIxLateEventType[] _UILateEventTypeEnumerator;
#endif

        #endregion

        #region <Constructor>

        static UIxTool()
        {
#if !SERVER_DRIVE
            _UILateEventTypeEnumerator =
                EnumFlag.GetEnumEnumerator<UIxLateEventType>(EnumFlag.GetEnumeratorType.ExceptMaskNone);
            
            _UIDynamicStateTypeEnumerator =
                EnumFlag.GetEnumEnumerator<UIxDynamicStateType>(EnumFlag.GetEnumeratorType.ExceptMaskNone);
            _UIDynamicStateTypeEnumerator_EntityEvent = new[]
                {UIxDynamicStateType.HasEntityBaseEvent, UIxDynamicStateType.HasEntityUIEvent, UIxDynamicStateType.HasEntityRenderEvent};
            _UIEventTypeEnumerator =
                EnumFlag.GetEnumEnumerator<UIEventType>(EnumFlag.GetEnumeratorType.ExceptMaskNone);
#endif
        }

        #endregion
    }
}