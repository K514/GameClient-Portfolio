using System.Collections.Generic;

namespace k514.Mono.Common
{
    public abstract partial class SceneEnvironment
    {
        #region <Fields>
                
        /// <summary>
        /// 카메라 상수값 레코드 인덱스
        /// </summary>
        public int CameraConstantIndex { get; protected set; }
        
        /// <summary>
        /// 카메라 변수값 레코드 인덱스
        /// </summary>
        public int CameraVariableIndex { get; protected set; }
        
        /// <summary>
        /// 스카이박스 인덱스
        /// </summary>
        public int SkyBoxIndex { get; protected set; }

        /// <summary>
        /// Bgm 인덱스
        /// </summary>
        public int BgmIndex { get; protected set; }
        
#if !SERVER_DRIVE && APPLY_PPS
        /// <summary>
        /// PpsObject 생성 파라미터 리스트
        /// </summary>
        public List<PpsObjectPoolManager.CreateParams> PpsObjectCreateParamsList { get; protected set; }
#endif
        
        #endregion

        #region <Callbacks>

        protected virtual void OnCreateAttribute()
        {
        }

        #endregion

        #region <Methods>

        public CameraConstantDataTable.TableRecord GetCameraConstantDataRecord()
        {
            return CameraConstantDataTable.GetInstanceUnsafe.GetRecordOrFallback(CameraConstantIndex);
        }
        
        public CameraVariableDataTable.TableRecord GetCameraVariableDataRecord()
        {
            return CameraVariableDataTable.GetInstanceUnsafe.GetRecordOrFallback(CameraVariableIndex);
        }

        #endregion
    }
}