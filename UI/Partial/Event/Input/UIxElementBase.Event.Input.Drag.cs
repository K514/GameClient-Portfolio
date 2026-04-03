#if !SERVER_DRIVE

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace k514.Mono.Common
{
    /// <summary>
    /// 모든 UI 스크립트가 IDragHandler를 구현하고 있으면, ScrollRect라던가 중간에서 드래그를 처리하는
    ///
    /// 유니티 UI 고유 컴포넌트들이 동작을 하지 않으므로, 필요한 파생 클래스에서 IDragHandler를 구현하도록 하고
    /// 상위 클래스에서는 아래와 같이 구현을 주석처리 했음.
    /// </summary>
    public partial class UIxElementBase // : IDragHandler
    {
        #region <Consts>

        /// <summary>
        /// 드래그 최대 거리 제곱 기본값
        /// </summary>
        protected const float _DefaultDragMaxDistance = 150;

        /// <summary>
        /// 드래그 방향 변환에 필요한 기저 변화량 하한값
        /// </summary>
        private const float _DragLowerBoundPositive = 0.257f;

        /// <summary>
        /// 드래그 방향 변환에 필요한 기저 변화량 하한값 음수
        /// </summary>
        private const float _DragLowerBoundNegative = -_DragLowerBoundPositive;

        #endregion
        
        #region <Fields>

        /// <summary>
        /// 현재 입력중인 포인터 이벤트 그룹
        /// </summary>
        private List<PointerEventData> _CurrentDragInputSet;
        
        /// <summary>
        /// 현재 입력된 뷰포트 좌표계 유닛 벡터
        /// </summary>
        protected ArrowType _CurrentArrowType;

        /// <summary>
        /// 현재 입력된 뷰포트 좌표계 유닛 벡터
        /// </summary>
        protected Vector2 _LatestViewPortUV;

        /// <summary>
        /// 현재 입력된 월드 좌표계 유닛 벡터
        /// </summary>
        protected Vector3 _LatestWorldPortUV;
        
        /// <summary>
        /// 현재 포인터 이동거리
        /// </summary>
        protected float _CurrentDistance;
        
        /// <summary>
        /// 드래그 최대 거리 제곱
        /// </summary>
        protected float _DragMaxDistance;

        /// <summary>
        /// 현재 드래그 위치가 변경되었는지 표시하는 플래그
        /// </summary>
        private bool IsDragUpdated;

        #endregion
        
        #region <Callbacks>

        private void OnCreateInputEventDrag()
        {
            _CurrentDragInputSet = new List<PointerEventData>();
        }
  
        /// <summary>
        /// 드래그가 감지된 경우의 유니티 엔진 입력 콜백
        ///
        /// IDragHandler를 구현한 클래스에서만 동작함
        /// </summary>
        public virtual void OnDrag(PointerEventData p_EventData)
        {
            if (_UIDynamicStateFlagMask.HasAnyFlagExceptNone(UIxTool.UIxDynamicStateType.HasEvent))
            {
                _CurrentDragInputSet.Add(p_EventData);
                _LatestPointer = new PointerEventDataPreset(p_EventData);

                var direction = p_EventData.position - _PointerDownPosition;
                var uv = direction.normalized;
                if (_UIStaticStateFlagMask.HasAnyFlagExceptNone(UIxTool.UIxStaticStateType.FloatRect))
                {
                    _CurrentDistance = direction.magnitude;
                    RectTransform.position = Mathf.Min(_CurrentDistance, _DragMaxDistance) * uv + _PointerDownPosition;
                }
                
                _CurrentArrowType = ArrowType.None;
                _LatestViewPortUV = _UIStaticStateFlagMask.HasAnyFlagExceptNone(UIxTool.UIxStaticStateType.UpdateDragDirectionUsingPointerData) ? p_EventData.delta.normalized : uv;
                _LatestWorldPortUV = new Vector3(_LatestViewPortUV.x, 0f, _LatestViewPortUV.y);

                // U방향에 대해서
                if (_LatestWorldPortUV.x > _DragLowerBoundPositive)
                {
                    _CurrentArrowType.AddFlag(ArrowType.Right);
                }
                else if (_LatestWorldPortUV.x < _DragLowerBoundNegative)
                {
                    _CurrentArrowType.AddFlag(ArrowType.Left);
                }

                // V방향에 대해서
                if (_LatestWorldPortUV.z > _DragLowerBoundPositive)
                {
                    _CurrentArrowType.AddFlag(ArrowType.Up);
                }
                else if (_LatestWorldPortUV.z < _DragLowerBoundNegative)
                {
                    _CurrentArrowType.AddFlag(ArrowType.Down);
                }

                IsDragUpdated = true;
            }
        }

        #endregion
        
        #region <Methods>

        public void SetDragMaxDistance(float p_DistanceSqr)
        {
            _DragMaxDistance = p_DistanceSqr;
        }
        
        private void ResetInputDragState()
        {
            _CurrentDragInputSet.Clear();
            _CurrentDistance = 0f;
            SetDragMaxDistance(_DefaultDragMaxDistance);
            IsDragUpdated = false;
            
            ResetInputDragEventPreset();
        }
        
        protected void ResetInputDragEventPreset()
        {
            _CurrentArrowType = default;
            _LatestViewPortUV = default;
            _LatestWorldPortUV = default;
            _LatestTouchHoldEventPreset = default;
        }

        #endregion
    }
}
#endif