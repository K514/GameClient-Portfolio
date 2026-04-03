#if !SERVER_DRIVE

using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace k514.Mono.Common
{
    public partial class CameraManager
    {
        #region <Fields>
        
        /// <summary>
        /// [카메라 래퍼 타입, 카메라 래퍼Transform] 컬렉션
        /// </summary>
        private Dictionary<CameraTool.CameraWrapperType, Transform> _CameraAffineSet;

        /// <summary>
        /// [카메라 래퍼 타입, 카메라 뷰 컨트롤 제어 오브젝트] 컬렉션
        /// </summary>
        private Dictionary<CameraTool.CameraWrapperType, CameraViewControl> _CameraAffineWrapperControllerSet;

        #endregion

        #region <Callbacks>

        private void OnCreateWrapper()
        {
            // 래퍼 컬렉션 초기화
            _CameraAffineSet = new Dictionary<CameraTool.CameraWrapperType, Transform>();
            _CameraAffineWrapperControllerSet = new Dictionary<CameraTool.CameraWrapperType, CameraViewControl>();

            // 래퍼 타입 순서에 맞게 각 래퍼를 오브젝트 계층 관계로 구성시킨다.
            var enumerator = CameraTool.CameraWrapperTypeEnumerator;
            var prevRearWrapper = default(Transform);
            
            foreach (var wrapperType in enumerator)
            {
                var tryRearWrapper = default(Transform);
                if (wrapperType.IsControllableWrapperType())
                {
                    var tryViewController = new CameraViewControl(wrapperType);
                    _CameraAffineWrapperControllerSet.Add(wrapperType, tryViewController);
                    _CameraAffineSet.Add(wrapperType,tryViewController.Head);
                    tryRearWrapper = tryViewController.Rear;
                }
                else
                {
                    var wrapperObjectName = 
                        wrapperType == CameraTool.CameraWrapperType.Root 
                            ? "CameraManager" : 
                            wrapperType.ToString();
                    var tryWrapper = new GameObject(wrapperObjectName).transform;
                    _CameraAffineSet.Add(wrapperType, tryWrapper);
                    tryRearWrapper = tryWrapper;
                }
                
                if (!ReferenceEquals(null, prevRearWrapper))
                {
                    _CameraAffineSet[wrapperType].SetParent(prevRearWrapper, false);
                }

                prevRearWrapper = tryRearWrapper;
            }
            
            OnCreateRootWrapper();
            OnCreateViewControlWrapper();
            OnCreateShakeWrapper();
        }

        #endregion
    }
}

#endif