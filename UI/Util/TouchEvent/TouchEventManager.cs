#if !SERVER_DRIVE

using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace k514.Mono.Common
{
    /// <summary>
    /// 터치 드래그 이벤트를 수신받고 적합한 터치 이벤트 처리자에게 분배하는 등의 기능을 수행하는 매니저 클래스
    /// </summary>
    public class TouchEventManager : SceneChangeEventReceiveAsyncSingleton<TouchEventManager>
    {
        #region <Consts>

        private const float _CastDistanceUpperBound = 1000f;

        #endregion

        #region <Callbacks>

        protected override async UniTask OnInitiate(CancellationToken p_CancellationToken)
        {
            await UniTask.CompletedTask;
        }
        
        public override async UniTask OnScenePreload(CancellationToken p_CancellationToken)
        {
            await UniTask.CompletedTask;
        }

        public override async UniTask OnSceneStart(CancellationToken p_CancellationToken)
        {
            await UniTask.CompletedTask;
        }

        public override async UniTask OnSceneTerminate(CancellationToken p_CancellationToken)
        {
            await UniTask.CompletedTask;
        }
        
        public override async UniTask OnSceneTransition(CancellationToken p_CancellationToken)
        {
            await UniTask.CompletedTask;
        }

        public void OnTouchStart(UIxTool.UITouchEventParams p_TouchEventParams, TouchInputEventPreset p_InputGesture)
        {
            var eventMask = p_TouchEventParams.TouchEventType;
            var enumerator = TouchEventTool._TouchEventTypeEnumerator;
            foreach (var touchEvent in enumerator)
            {
                if (eventMask.HasAnyFlagExceptNone(touchEvent))
                {
                    switch (touchEvent)
                    {
                        case TouchEventTool.TouchEventType.ControlView:
                        {
                            CastTouchControlView(p_InputGesture);
                            break;
                        }
                        case TouchEventTool.TouchEventType.TouchWorldObject:
                        {
                            break;
                        }
                    }        
                }
            }
        }
        
        public void OnTouchDragging(UIxTool.UITouchEventParams p_TouchEventParams, TouchInputEventPreset p_InputGesture)
        {
            var eventMask = p_TouchEventParams.TouchEventType;
            var enumerator = TouchEventTool._TouchEventTypeEnumerator;
            foreach (var touchEvent in enumerator)
            {
                if (eventMask.HasAnyFlagExceptNone(touchEvent))
                {
                    switch (touchEvent)
                    {
                        case TouchEventTool.TouchEventType.ControlView:
                        {
                            CastTouchControlView(p_InputGesture);
                            break;
                        }
                        case TouchEventTool.TouchEventType.TouchWorldObject:
                        {
                            break;
                        }
                    }        
                }
            }
        }

        public void OnTouchOver(UIxTool.UITouchEventParams p_TouchEventParams, TouchInputEventPreset p_InputGesture)
        {
            var eventMask = p_TouchEventParams.TouchEventType;
            var enumerator = TouchEventTool._TouchEventTypeEnumerator;
            foreach (var touchEvent in enumerator)
            {
                if (eventMask.HasAnyFlagExceptNone(touchEvent))
                {
                    switch (touchEvent)
                    {
                        case TouchEventTool.TouchEventType.ControlView:
                        {
                            CastTouchControlView(p_InputGesture);
                            break;
                        }
                        case TouchEventTool.TouchEventType.TouchWorldObject:
                        {
                            CastTouchWorldObject(p_InputGesture);
                            break;
                        }
                    }        
                }
            }
        }

        #endregion

        #region <Methods>

        private void CastTouchControlView(TouchInputEventPreset p_InputGesture)
        {
            var eventPreset = new TouchEventParams(p_InputGesture, default);
            TouchEventSenderManager.GetInstanceUnsafe.SendEvent(TouchEventTool.TouchEventType.ControlView, eventPreset);
        }

        private void CastTouchWorldObject(TouchInputEventPreset p_InputGesture)
        {
            var screenPos = p_InputGesture.PointerEventDataPreset.ScreenPos;
            var ray = CameraManager.GetInstanceUnsafe.MainCamera.ScreenPointToRay(screenPos);
            var resultTuple = PhysicsTool.GetNearestObject_RayCast(ray, _CastDistanceUpperBound, GameConst.VisibleBlock_Unit_LayerMask, QueryTriggerInteraction.Collide);

            if (resultTuple.Item1)
            {
                var rayCastHit = resultTuple.Item2;
                var hitObject = rayCastHit.transform.gameObject;
                if (InteractManager.GetInstanceUnsafe.TryGetEntity(hitObject, out var o_Entity))
                {
                    var eventPreset = new TouchEventParams(p_InputGesture, new TouchObjectResultPreset(o_Entity, hitObject));
                    TouchEventSenderManager.GetInstanceUnsafe.SendEvent(TouchEventTool.TouchEventType.TouchWorldObject, eventPreset);
                }
                else
                {
                    var eventPreset = new TouchEventParams(p_InputGesture, new TouchObjectResultPreset(rayCastHit.point, hitObject));
                    TouchEventSenderManager.GetInstanceUnsafe.SendEvent(TouchEventTool.TouchEventType.TouchWorldObject, eventPreset);
                }
            }
        }

        #endregion
    }
}

#endif