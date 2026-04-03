#if UNITY_EDITOR

using System.Threading;
using Cysharp.Threading.Tasks;

namespace k514
{
    public class GlobalDefineDataTable : EditorOnlyTable<GlobalDefineDataTable, TableMetaData, GlobalDefineDataTable.GlobalDefineType, GlobalDefineDataTable.TableRecord>
    {
        #region <Enums>

        public enum GlobalDefineType
        {
            /// <summary>
            /// 타입 기본값
            /// </summary>
            None,
            
            /// <summary>
            /// 해당 전처리자가 선언된 경우, 방향키 입력이 교착상태가 되었을 때 늦게 입력된쪽을 우선 처리한다.
            /// 예를 들어, 왼쪽 방향키를 누르다가 오른쪽도 동시에 눌렀을 때 오른쪽 입력이 동작한다.
            /// 선언되지 않은 경우, 그대로 왼쪽 입력이 동작한다.
            /// </summary>
            OVERRAP_DEADLOCK_ARROWTYPE,
            
            /// <summary>
            /// 해당 전처리자가 선언된 경우, 카메라 드래그 입력이 지속되는 동안 카메라 회전을 유지시킨다.
            /// 선언되지 않은 경우, 드래그 위치가 변했을 때만 카메라 회전 이벤트가 발생한다.
            /// </summary>
            KEEP_VIEW_DRAG_DIRECTION,

            /// <summary>
            /// 해당 전처리자가 선언된 경우, 해당 클라이언트를 서버 노드로 사용한다.
            /// 선언되지 않은 경우, 해당 클러이언트를 클라이언트 노드로 사용한다.
            /// </summary>
            SERVER_DRIVE,
            
            /// <summary>
            /// 해당 전처리자가 선언된 경우, 메인 카메라와 포커스 사이에
            /// 장해물 오브젝트가 있다면 카메라를 줌인하며 포커스를 비춘다.
            /// </summary>
            CAMERA_AUTO_ZOOM,
            
            /// <summary>
            /// 해당 전처리자가 선언된 경우, 테스트 매니저를 활성화시킨다.
            /// </summary>
            APPLY_TEST_MANAGER,
            
            /// <summary>
            /// 해당 전처리자가 선언된 경우, 디버그 그래픽 기능을 활성화한다.
            /// </summary>
            APPLY_DRAW_GISMOS,
            
            /// <summary>
            /// 해당 전처리자가 선언된 경우, 디버그 로그 기능을 활성화한다.
            /// </summary>
            APPLY_PRINT_LOG,
            
            /// <summary>
            /// 해당 전처리자가 선언된 경우, 게임 개체 및 상호작용 매니저에 고정업데이트 콜백을 생성하고 호출해준다.
            /// </summary>
            ADD_FIXED_UPDATE_GAME_ENTITY,

#if SERVER_DRIVE
#else
            /// <summary>
            /// 해당 전처리자가 선언된 경우, PPS 기능을 활성화시킨다.
            /// </summary>
            APPLY_PPS,
            
            /// <summary>
            /// 해당 전처리자가 선언된 경우, URP 기능을 활성화시킨다.
            /// </summary>
            APPLY_URP
#endif
        }

        #endregion

        #region <Record>

        public class TableRecord : EditorOnlyTableRecord
        {
            public string Description { get; private set; }

            public override async UniTask SetRecord(GlobalDefineType p_Key, object[] p_RecordField, CancellationToken p_CancellationToken)
            {
                await base.SetRecord(p_Key, p_RecordField, p_CancellationToken);
                
                Description = (string)p_RecordField.GetElementSafe(0);
            }
        }

        #endregion

        #region <Methods>

        protected override async UniTask AddDefaultRecords(CancellationToken p_CancellationToken)
        {
            await base.AddDefaultRecords(p_CancellationToken);

            var enumerator = EnumFlag.GetEnumEnumerator<GlobalDefineType>(EnumFlag.GetEnumeratorType.ExceptNone);
            foreach (var globalDefineType in enumerator)
            {
                await AddRecord(globalDefineType, false, p_CancellationToken, string.Empty);
            }
        }

        #endregion
    }
}

#endif