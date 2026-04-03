namespace k514.Mono.Common
{
    public class AnimationModuleDataTableQuery : MultiTableIndexBase<AnimationModuleDataTableQuery, TableMetaData, AnimationModuleDataTableQuery.TableLabel, IAnimationModuleDataTableBridge, IAnimationModuleDataTableRecordBridge>
    {
        #region <Enums>

        public enum TableLabel
        {
            None,
            AnimatorAnimation,
        }

        #endregion
    }
}