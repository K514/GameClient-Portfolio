namespace k514.Mono.Common
{
    public class TestMotionClipDataTable : MotionClipDataTableBase<TestMotionClipDataTable>
    {
        protected override void OnCreateTableBridge()
        {
            _MotionClipLabel = MotionClipDataTableQuery.TableLabel.Test;
        }
    }
}