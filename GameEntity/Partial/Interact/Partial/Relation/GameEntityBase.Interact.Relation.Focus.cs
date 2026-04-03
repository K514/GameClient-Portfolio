using System.Collections.Generic;
using UnityEngine;

namespace k514.Mono.Common
{
    public partial class GameEntityBase<Content, CreateParams, ActivateParams>
    {
        #region <Fields>

        private GameEntityRelation _FocusEntityRelation;

        #endregion
        
        #region <Callbacks>

        private void OnCreateFocusInteraction()
        {
            _FocusEntityRelation = _GameEntityRelationTable[GameEntityRelationTool.GameEntityRelationType.Focus];
        }
        
        private void OnActivateFocusInteraction()
        {
        }
        
        private void OnRetrieveFocusInteraction()
        {
            _FocusEntityRelation.ResetNode();
        }
        
        private void OnHandleFocusEvent(GameEntityRelationTool.GameEntityRelationEventType p_EventType, GameEntityRelationTool.GameEntityRelationEventParams p_EventParams)
        {
            switch (p_EventType)
            {
                case GameEntityRelationTool.GameEntityRelationEventType.SubNodeAdded:
                    break;
                case GameEntityRelationTool.GameEntityRelationEventType.SubNodeSelected:
                    break;
                case GameEntityRelationTool.GameEntityRelationEventType.SubNodeRemoved:
                    break;
            }
        }

        public void OnFocusRelationChanged(ValidStateType p_Type, IGameEntityBridge p_Target, float p_SqrDistance)
        {
            switch (p_Type)
            {
                case ValidStateType.Added:
                    AddFocus(p_Target);
                    break;
                case ValidStateType.Removed:
                    RemoveFocus(p_Target);
                    break;
            }
        }

        #endregion

        #region <Methods>

        public void AddFocus(IGameEntityBridge p_TargetEntity)
        {
            _FocusEntityRelation.AddNode(p_TargetEntity);
        }
        
        public void AddFocus(List<IGameEntityBridge> p_TargetEntityList)
        {
            foreach (var entity in p_TargetEntityList)
            {
                AddFocus(entity);
            }
        }
        
        public void RemoveFocus(IGameEntityBridge p_TargetEntity)
        {
            _FocusEntityRelation.RemoveNode(p_TargetEntity);
        }

        public IGameEntityBridge GetCurrentFocus()
        {
            return _FocusEntityRelation.CurrentSubNode;
        }
        
        public (bool, IGameEntityBridge) TryGetCurrentFocus()
        {
            var focusEntity = _FocusEntityRelation.CurrentSubNode;
            return (focusEntity.IsEntityValid(), focusEntity);
        }
        
        public HashSet<IGameEntityBridge> GetFocusGroup()
        {
            return _FocusEntityRelation.GetSubNodes();
        }

        public void SetLookToFocus(bool p_UpdateMotionCachedRotation)
        {
            var focusEntity = _FocusEntityRelation.CurrentSubNode;
            if (focusEntity.IsEntityValid())
            {
                SetLook(focusEntity.GetBottomPosition());
                
                if (p_UpdateMotionCachedRotation)
                {
                    AnimationModule.SaveMotionAffine();
                }
            }
        }

        /// <summary>
        /// [포커스 타겟의 반경 + 해당 개체의 반경] 을 리턴하는 메서드
        /// </summary>
        public float GetDistanceLowerBoundFromFocusNode()
        {
            var focusEntity = _FocusEntityRelation.CurrentSubNode;
            return focusEntity.IsEntityValid() ? GetRadius() + focusEntity.GetRadius() : GetRadius();
        }

        /// <summary>
        /// [해당 개체에서 포커스 타겟으로의 단위벡터] 를 리턴하는 메서드
        /// </summary>
        public Vector3 GetFocusUV()
        {
            var focusEntity = _FocusEntityRelation.CurrentSubNode;
            return focusEntity.IsEntityValid()
                ? Affine.GetDirectionUnitVectorTo(focusEntity.GetBottomPosition())
                : Affine.forward;
        }
        
        /// <summary>
        /// [포커스 타겟의 위치] 를 리턴하는 메서드
        /// </summary>
        public Vector3 GetFocusPosition()
        {
            var focusEntity = _FocusEntityRelation.CurrentSubNode;
            return focusEntity.IsEntityValid() ? focusEntity.GetBottomPosition() : Affine.position;
        }

        #endregion
    }
}