#if !SERVER_DRIVE

using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace k514.Mono.Common
{
    /// <summary>
    /// UIxElement를 담는 컨테이너 클레스
    /// </summary>
    public abstract partial class UIxContainerBase : UIxElementBase
    {
        #region <Fields>

        /// <summary>
        /// 해당 클러스터가 제어하는 하위 UI 컴포넌트 리스트
        /// </summary>
        protected List<UIxElementBase> _Elements;
        protected List<UniTask> _AsyncTaskSet;
        
        #endregion

        #region <Callbacks>

        protected override void OnCreate(UIPoolManager.CreateParams p_CreateParams)
        {
            base.OnCreate(p_CreateParams);
            
            _Elements = new List<UIxElementBase>();
            _AsyncTaskSet = new List<UniTask>();
        }

        protected override void OnRetrieve(UIPoolManager.CreateParams p_CreateParams, bool p_IsPooled, bool p_IsDisposed)
        {
            base.OnRetrieve(p_CreateParams, p_IsPooled, p_IsDisposed);
            
            RemoveAllElements(true);
        }

        protected override void OnUpdateUIDeferredEvent(float p_DeltaTime)
        {
            base.OnUpdateUIDeferredEvent(p_DeltaTime);
            
            var elementCount = _Elements.Count;
            for (var i = elementCount - 1; i > -1; i--)
            {
                var tryElement = _Elements[i];
                if (tryElement.IsVisible() || tryElement.HasAnyLateEvent())
                {
                    tryElement.OnLateUpdate(p_DeltaTime);
                }
            }
        }

        public override void OnDrag(PointerEventData p_EventData)
        {
            base.OnDrag(p_EventData); 
            
            foreach (var slaveNode in _Elements)
            {
                slaveNode.OnDrag(p_EventData);
            }
        }
        
        public override async UniTask OnScenePreload(CancellationToken p_CancellationToken)
        {
            await base.OnScenePreload(p_CancellationToken);

            foreach (var element in _Elements)
            {
                var asyncTask = element.OnScenePreload(p_CancellationToken);
                _AsyncTaskSet.Add(asyncTask);
            }

            await _AsyncTaskSet;
            _AsyncTaskSet.Clear();
        }
        
        public override async UniTask OnSceneStart(CancellationToken p_CancellationToken)
        {
            await base.OnSceneStart(p_CancellationToken);
            
            foreach (var element in _Elements)
            {
                var asyncTask = element.OnSceneStart(p_CancellationToken);
                _AsyncTaskSet.Add(asyncTask);
            }
            
            await _AsyncTaskSet;
            _AsyncTaskSet.Clear();
        }
        
        public override async UniTask OnSceneTerminate(CancellationToken p_CancellationToken)
        {
            await base.OnSceneTerminate(p_CancellationToken);
            
            foreach (var element in _Elements)
            {
                var asyncTask = element.OnSceneTerminate(p_CancellationToken);
                _AsyncTaskSet.Add(asyncTask);
            }

            await _AsyncTaskSet;
            _AsyncTaskSet.Clear();
        }
        
        public override async UniTask OnSceneTransition(CancellationToken p_CancellationToken)
        {
            await base.OnSceneTransition(p_CancellationToken);
            
            foreach (var element in _Elements)
            {
                var asyncTask = element.OnSceneTransition(p_CancellationToken);
                _AsyncTaskSet.Add(asyncTask);
            }
            
            await _AsyncTaskSet;
            _AsyncTaskSet.Clear();
        }
        
        #endregion

        #region <Methods>

        public override void SetHide(bool p_HideFlag)
        {
            base.SetHide(p_HideFlag);

            SetElementHide(p_HideFlag);
        }

        protected virtual void SetElementHide(bool p_HideFlag)
        {
            foreach (var element in _Elements)
            {
                element.SetHide(p_HideFlag);
            }
        }
        
        public override void SetControlBlock(bool p_Flag)
        {
            base.SetControlBlock(p_Flag);

            SetElementControlBlock(p_Flag);
        }
        
        protected void SetElementControlBlock(bool p_Flag)
        {
            foreach (var element in _Elements)
            {
                element.SetControlBlock(p_Flag);
            }
        }

        #endregion
    }
}
#endif