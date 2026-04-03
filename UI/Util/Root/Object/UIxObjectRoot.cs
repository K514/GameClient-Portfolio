#if !SERVER_DRIVE

using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace k514.Mono.Common
{
    public partial class UIxObjectRoot : SceneChangeEventReceiveUnityAsyncSingleton<UIxObjectRoot>, ILateUpdateEvent
    {
        #region <Fields>

        private Dictionary<RenderMode, UIxTool.UIxRootPreset> _RootPresetGroup;
        private RenderMode[] _RenderMode_Enumerator;
        private RectTransform _Rect;
        
        #endregion

        #region <Callbacks>

        protected override void TryInitializeDependency()
        {
            base.TryInitializeDependency();

            _Dependencies.Add(typeof(UIPoolManager));
            _Dependencies.Add(typeof(UIxLayerDeployDataTableQuery));
            _Dependencies.Add(typeof(TouchEventManager));
        }

        protected override async UniTask OnCreated(CancellationToken p_CancellationToken)
        {
            await base.OnCreated(p_CancellationToken);

            gameObject.SetActiveSafe(false);
            
            _RootPresetGroup = new Dictionary<RenderMode, UIxTool.UIxRootPreset>();
            _RenderMode_Enumerator = EnumFlag.GetEnumEnumerator<RenderMode>(EnumFlag.GetEnumeratorType.GetAll);
            _Rect = GetComponent<RectTransform>();
            _SceneEventAsyncTaskSet = new List<UniTask>();
            
            foreach (var renderMode in _RenderMode_Enumerator)
            {
                if (UIxLayerDeployDataTableQuery.GetInstanceUnsafe.TryGetLabelTableList(renderMode, out var o_LabelTableList))
                {
                    foreach (var targetTable in o_LabelTableList)
                    {
                        var keySet = targetTable.GetCurrentKeyEnumerator();
                        foreach (var key in keySet)
                        {
                            if (UIxLayerDeployDataTableQuery.GetInstanceUnsafe.TryGetRecordBridge(key, out var o_Record))
                            {
                                var elementTypeList = o_Record.UIxElementTypeList;
                                if (elementTypeList.CheckCollectionSafe())
                                {
                                    foreach (var elementType in elementTypeList)
                                    {
                                        await AddElement(renderMode, key, elementType, p_CancellationToken);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            gameObject.SetActiveSafe(true);
            Broadcast_HideUI(true);
        }

        protected override async UniTask OnInitiate(CancellationToken p_CancellationToken)
        {
            await UniTask.CompletedTask;
        }

        /// <summary>
        /// 인스턴스가 파기될 때 수행할 작업을 기술한다.
        /// </summary>
        protected override void OnDisposeSingleton()
        {
            if (_RootPresetGroup != null)
            {
                foreach (var renderMode in _RenderMode_Enumerator)
                {
                    if (_RootPresetGroup.TryGetValue(renderMode, out var o_RenderPreset))
                    {
                        o_RenderPreset.Dispose();
                    }
                }
                _RootPresetGroup.Clear();
                _RootPresetGroup = null;
            }
            
            base.OnDisposeSingleton();
        }

        #endregion
    }
}

#endif