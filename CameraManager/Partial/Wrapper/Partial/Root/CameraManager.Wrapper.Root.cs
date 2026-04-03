using UnityEngine;

namespace k514.Mono.Common
{
    public partial class CameraManager
    {
        #region <Fields>
        
        /// <summary>
        /// 최상위 Wrapper : 초점이 초기화 지점이라면, 카메라가 바라보는 지점이며 카메라의 회전중심
        /// </summary>
        public Transform RootWrapper { get; private set; }

        #endregion

        #region <Callbacks>

        private void OnCreateRootWrapper()
        {
            RootWrapper = _CameraAffineSet[CameraTool.CameraWrapperType.Root];
        }

        #endregion
        
        #region <Methods>
                
        public void SetRootPosition(Vector3 p_TargetPosition)
        {
            RootWrapper.position = p_TargetPosition;
        }
        
        public Vector3 GetRootPosition()
        {
            return RootWrapper.position;
        }
        
        public void SetRootPositionZero()
        {
            SetRootPosition(Vector3.zero);
        }
        
        /// <summary>
        /// Root Transform으로부터 지정한 위치까지의 단위 벡터를 리턴하는 메서드
        /// </summary>
        public Vector3 GetUnitVectorFromRoot(Vector3 p_TargetPos)
        {
            return RootWrapper.position.GetDirectionUnitVectorTo(p_TargetPos);
        }

        #endregion
    }
}