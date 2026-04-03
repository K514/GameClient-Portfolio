#if !SERVER_DRIVE

namespace k514.Mono.Common
{
    public static partial class CameraTool
    {
        #region <Consts>

        /// <summary>
        /// 근접 컬링 반경 하한
        /// </summary>
        public static float DefaultNearCullingRadiusLowerBound = 0.1f;

        #endregion
        
        #region <Constructor>

        static CameraTool()
        {
            CameraEventTypeEnumerator = EnumFlag.GetEnumEnumerator<CameraEventType>(EnumFlag.GetEnumeratorType.ExceptMaskNone);
            CameraCullingStateEnumerator = EnumFlag.GetEnumEnumerator<CameraCullingState>(EnumFlag.GetEnumeratorType.ExceptMaskNone);
            CameraWrapperTypeEnumerator = EnumFlag.GetEnumEnumerator<CameraWrapperType>(EnumFlag.GetEnumeratorType.GetAll);
            ViewControlEnumerator = EnumFlag.GetEnumEnumerator<ViewControlType>(EnumFlag.GetEnumeratorType.GetAll);
        }

        #endregion

        #region <Methods>

        public static bool IsControllableWrapperType(this CameraWrapperType p_Type)
        {
            switch (p_Type)
            {
                case CameraWrapperType.ViewControl_0:
                case CameraWrapperType.ViewControl_1:
                    return true;
                default:
                case CameraWrapperType.Root:
                case CameraWrapperType.ViewControl_Shake:
                    return false;
            }
        }

        #endregion
    }
}

#endif