#if !SERVER_DRIVE

namespace k514.Mono.Common
{
    /// <summary>
    /// 오디오 멀티테이블에 접근할 수 있는 쿼리 클래스
    /// </summary>
    public class AudioClipNameTableQuery : MultiTableIndexBase<AudioClipNameTableQuery, TableMetaData, AudioClipNameTableQuery.TableLabel, IAudioClipNameTableBridge<IAudioClipNameTableRecordBridge>, IAudioClipNameTableRecordBridge>
    {
        #region <Enum>

        public enum TableLabel
        {
            None, // 마스터 사운드
            System,
            BGM,
            Effect,
            Voice,
        }

        #endregion
    }
}

#endif