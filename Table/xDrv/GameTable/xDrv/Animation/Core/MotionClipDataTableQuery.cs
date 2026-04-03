namespace k514.Mono.Common
{
    public class MotionClipDataTableQuery : MultiTableBase<MotionClipDataTableQuery, string, TableMetaData, MotionClipDataTableQuery.TableLabel, IMotionClipDataTable, IMotionClipDataTableRecord>
    {
        public enum TableLabel
        {
            Test,
        }
    }
}