#if !SERVER_DRIVE

using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace k514.Mono.Common
{
    /// <summary>
    /// 기본 UI 컴포넌트
    /// </summary>
    public abstract partial class UIxElementBase : UnityObjectBase<UIxElementBase, UIPoolManager.CreateParams, UIPoolManager.ActivateParams>, ISceneChangeEvent
    {
        #region <Fields>
        
        /// <summary>
        /// 해당 UI 상태 플래그 마스크
        /// </summary>
        protected UIxTool.UIxDynamicStateType _UIDynamicStateFlagMask;

        /// <summary>
        /// 해당 UI 상태 플래그 마스크
        /// </summary>
        protected UIxTool.UIxStaticStateType _UIStaticStateFlagMask;
        
        /// <summary>
        /// RectTransform 캐시
        /// </summary>
        [NonSerialized] public RectTransform RectTransform;

        
        #endregion

        #region <Callbacks>

        protected override void OnInitializeModel(float p_Scale)
        {
            base.OnInitializeModel(p_Scale);

            RectTransform = transform as RectTransform;
        }

        protected override void OnCreate(UIPoolManager.CreateParams p_CreateParams)
        {
            base.OnCreate(p_CreateParams);
            
            OnCreateAnimation();
            OnCreateGraphics();
            OnCreateInputEvent();
        }
        
        protected override bool OnActivate(UIPoolManager.CreateParams p_CreateParams, UIPoolManager.ActivateParams p_ActivateParams, bool p_IsPooled)
        {
            if (base.OnActivate(p_CreateParams, p_ActivateParams, p_IsPooled))
            {
                OnActivateCanvas(p_ActivateParams);
                OnActivateInputEvent();
                
                return true;
            }
            else
            {
                return false;
            }
        }

        protected override void OnRetrieve(UIPoolManager.CreateParams p_CreateParams, bool p_IsPooled, bool p_IsDisposed)
        {
            OnRetrieveInputEvent();
            OnRetrieveGraphics();
            OnRetrieveAnimation();
            OnRetrieveCanvas();
            OnRetrieveControl();
            
            base.OnRetrieve(p_CreateParams, p_IsPooled, p_IsDisposed);
        }
        
        protected override void OnDispose()
        {
            OnDisposeGraphics();
            
            base.OnDispose();
        }

        public virtual async UniTask OnScenePreload(CancellationToken p_CancellationToken)
        {
            await UniTask.CompletedTask;
        }

        public virtual async UniTask OnSceneStart(CancellationToken p_CancellationToken)
        {
            await UniTask.CompletedTask;
        }

        public virtual async UniTask OnSceneTerminate(CancellationToken p_CancellationToken)
        {
            await UniTask.CompletedTask;
        }

        public virtual async UniTask OnSceneTransition(CancellationToken p_CancellationToken)
        {
            SetHide(true);
            
            await UniTask.CompletedTask;
        }

        protected override void OnScaleChanged(float p_DeltaRatio)
        {
            RectTransform.localScale *= p_DeltaRatio;
        }

        #endregion

        #region <Methods>

        public void SetStateFlag(UIxTool.UIxStaticStateType p_Type)
        {
            _UIStaticStateFlagMask.AddFlag(p_Type);
        }

        #endregion
    }
}

#endif