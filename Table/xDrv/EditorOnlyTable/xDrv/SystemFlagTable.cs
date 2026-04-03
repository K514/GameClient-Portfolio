#if UNITY_EDITOR

using System.Threading;
using Cysharp.Threading.Tasks;

namespace k514
{
    public class SystemFlagTable : EditorOnlyTable<SystemFlagTable, TableMetaData, SystemFlagTable.SystemFlagType, SystemFlagTable.SystemFlagRecord>
    {
        #region <Consts>
        
        /// <summary>
        /// 시스템 배포모드 플래그 숏컷
        /// </summary>
        public static bool IsSystemReleaseMode()
        {
            return GetInstanceUnsafe?.GetRecord(SystemFlagType.ReleaseMode).Flag ?? false;
        }

        /// <summary>
        /// 시스템 배포모드 플래그 숏컷
        /// </summary>
        public static bool IsSystemDevMode()
        {
            return !IsSystemReleaseMode();
        }

        /// <summary>
        /// 테이블 바이트코드 로드 플래그 숏컷
        /// </summary>
        public static bool IsUsingSerializedTable()
        {
            return GetInstanceUnsafe?.GetRecord(SystemFlagType.UsingSerializedTable).Flag ?? false;
        }

        /// <summary>
        /// 리소스 리스트 자동 갱신 플래그
        /// </summary>
        public static bool GetAutoUpdateResourceListFlag()
        {
            return GetInstanceUnsafe.GetRecord(SystemFlagType.ResourceListAutoUpdate).Flag;
        }

        #endregion
        
        #region <Enums>

        public enum SystemFlagType
        {
            /// <summary>
            /// 해당 프로젝트가 배포 플랫폼에서 동작해야 하는 경우 참.
            /// 실제 플랫폼 상에서는 사용하지 않고, 빌드 전에 실제 플랫폼의 환경처럼 프로젝트가 동작하는지
            /// 체크하고, 빌드에 필요없는 리소스 파일을 제거하는 용도로 활용된다.
            ///
            /// 해당 모드가 꺼진 경우, PatchPackageBuilder를 통해 선택된 버전으로 패치가 진행되며
            /// 해당 모드가 켜진 경우, 최신 버전으로 패치를 진행한다.
            /// </summary>
            ReleaseMode,

            /// <summary>
            /// 배포모드가 아닐 때, 리소스 리스트 테이블이 활성화되면서
            /// 리소스 폴더를 전부 검색하여 리스트를 갱신하도록 하는 플래그
            /// </summary>
            ResourceListAutoUpdate,
            
            /// <summary>
            /// 각 테이블에서 지정하는 방식으로 테이블 데이터를 직렬화하고, 해당 방식으로 테이블을 로드하게 하는 플래그
            /// </summary>
            UsingSerializedTable,
        }
        
        #endregion

        #region <Record>

        public class SystemFlagRecord : EditorOnlyTableRecord
        {
            public bool Flag { get; private set; }
            
            public override async UniTask SetRecord(SystemFlagType p_Key, object[] p_RecordField, CancellationToken p_CancellationToken)
            {
                await base.SetRecord(p_Key, p_RecordField, p_CancellationToken);
                
                Flag = (bool) p_RecordField.GetElementSafe(0);
            }
        }
        
        #endregion

        #region <Methods>

        protected override async UniTask AddDefaultRecords(CancellationToken p_CancellationToken)
        {
            await base.AddDefaultRecords(p_CancellationToken);

            var enumerator = EnumFlag.GetEnumEnumerator<SystemFlagType>(EnumFlag.GetEnumeratorType.GetAll);
            foreach (var systemFlagType in enumerator)
            {
                switch (systemFlagType)
                {
                    case SystemFlagType.ReleaseMode :
                        await AddRecord(systemFlagType, false, p_CancellationToken, false);
                        break;
                    case SystemFlagType.ResourceListAutoUpdate :
                        await AddRecord(systemFlagType, false, p_CancellationToken, false);
                        break;   
                    case SystemFlagType.UsingSerializedTable :
                        await AddRecord(systemFlagType, false, p_CancellationToken, false);
                        break;
                } 
            }
        }

        /// <summary>
        /// 배포모드 시스템 플래그를 지정한 값으로 테이블에 업데이트 시키는 메서드
        /// </summary>
        public async UniTask UpdateSystemFlagReleaseMode(SystemFlagType p_TargetType, bool p_Flag, CancellationToken p_CancellationToken)
        {
            if (!GetTable().ContainsKey(p_TargetType)
                || p_Flag != GetRecord(p_TargetType).Flag)
            {
                await AddRecord(p_TargetType, true, p_CancellationToken, p_Flag);
                await WriteTableTextFileToAutoPath(DataIOTool.WriteType.Overlap, p_CancellationToken);
            }
        }

        #endregion
    }
}

#endif