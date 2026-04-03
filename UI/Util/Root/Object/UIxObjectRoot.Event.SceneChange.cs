#if !SERVER_DRIVE

using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace k514.Mono.Common
{
    public partial class UIxObjectRoot
    {
        #region <Fields>

        /// <summary>
        /// 씬 이벤트 콜백을 병렬로 수행하기 위한 리스트
        /// </summary>
        private List<UniTask> _SceneEventAsyncTaskSet;

        #endregion
        
        #region <Callbacks>
        
        public override async UniTask OnScenePreload(CancellationToken p_CancellationToken)
        {
            foreach (var renderMode in _RenderMode_Enumerator)
            {
                if (_RootPresetGroup.TryGetValue(renderMode, out var o_RootPreset))
                {
                    var uiCanvasCollection = o_RootPreset.UIxCanvasCollection;
                    foreach (var uiCanvasPresetKV in uiCanvasCollection)
                    {
                        var uiCanvasPreset = uiCanvasPresetKV.Value;
                        var uiElementCollection = uiCanvasPreset.UIxElementCollection;
                        foreach (var uiElementKV in uiElementCollection)
                        {
                            var uiElement = uiElementKV.Value;
                            var asyncTask = uiElement.OnScenePreload(p_CancellationToken);
                            _SceneEventAsyncTaskSet.Add(asyncTask);
                        }
                    }
                }
            }

            await _SceneEventAsyncTaskSet;
            _SceneEventAsyncTaskSet.Clear();
        }

        public override async UniTask OnSceneStart(CancellationToken p_CancellationToken)
        {
            foreach (var renderMode in _RenderMode_Enumerator)
            {
                if (_RootPresetGroup.TryGetValue(renderMode, out var o_RootPreset))
                {
                    var uiCanvasCollection = o_RootPreset.UIxCanvasCollection;
                    foreach (var uiCanvasPresetKV in uiCanvasCollection)
                    {
                        var uiCanvasPreset = uiCanvasPresetKV.Value;
                        var uiElementCollection = uiCanvasPreset.UIxElementCollection;
                        foreach (var uiElementKV in uiElementCollection)
                        {
                            var uiElement = uiElementKV.Value;
                            var asyncTask = uiElement.OnSceneStart(p_CancellationToken);
                            _SceneEventAsyncTaskSet.Add(asyncTask);
                        }
                    }
                }
            }

            await _SceneEventAsyncTaskSet;
            _SceneEventAsyncTaskSet.Clear();
        }

        public override async UniTask OnSceneTerminate(CancellationToken p_CancellationToken)
        {
            foreach (var renderMode in _RenderMode_Enumerator)
            {
                if (_RootPresetGroup.TryGetValue(renderMode, out var o_RootPreset))
                {
                    var uiCanvasCollection = o_RootPreset.UIxCanvasCollection;
                    foreach (var uiCanvasPresetKV in uiCanvasCollection)
                    {
                        var uiCanvasPreset = uiCanvasPresetKV.Value;
                        var uiElementCollection = uiCanvasPreset.UIxElementCollection;
                        foreach (var uiElementKV in uiElementCollection)
                        {
                            var uiElement = uiElementKV.Value;
                            var asyncTask = uiElement.OnSceneTerminate(p_CancellationToken);
                            _SceneEventAsyncTaskSet.Add(asyncTask);
                        }
                    }
                }
            }

            await _SceneEventAsyncTaskSet;
            _SceneEventAsyncTaskSet.Clear();
        }

        public override async UniTask OnSceneTransition(CancellationToken p_CancellationToken)
        {
            foreach (var renderMode in _RenderMode_Enumerator)
            {
                if (_RootPresetGroup.TryGetValue(renderMode, out var o_RootPreset))
                {
                    var uiCanvasCollection = o_RootPreset.UIxCanvasCollection;
                    foreach (var uiCanvasPresetKV in uiCanvasCollection)
                    {
                        var uiCanvasPreset = uiCanvasPresetKV.Value;
                        var uiElementCollection = uiCanvasPreset.UIxElementCollection;
                        foreach (var uiElementKV in uiElementCollection)
                        {
                            var uiElement = uiElementKV.Value;
                            var asyncTask = uiElement.OnSceneTransition(p_CancellationToken);
                            _SceneEventAsyncTaskSet.Add(asyncTask);
                        }
                    }
                }
            }

            await _SceneEventAsyncTaskSet;
            _SceneEventAsyncTaskSet.Clear();
        }
        
        #endregion
    }
}

#endif