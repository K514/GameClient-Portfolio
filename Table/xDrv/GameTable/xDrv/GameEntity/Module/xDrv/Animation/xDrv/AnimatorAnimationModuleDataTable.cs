using System;

namespace k514.Mono.Common
{
    public class AnimatorAnimationModuleDataTable : AnimationModuleDataTable<AnimatorAnimationModuleDataTable, AnimatorAnimationModuleDataTable.TableRecord>
    {
        #region <Record>

        [Serializable]
        public class TableRecord : AnimationModuleTableRecord
        {
        }

        #endregion

        #region <Callbacks>
        
        protected override void OnCreateTableBridge()
        {
            base.OnCreateTableBridge();
            
            _AnimationModuleTableLabel = AnimationModuleDataTableQuery.TableLabel.AnimatorAnimation;
            StartIndex = 0;
            EndIndex = 1000;
        }

        #endregion
    }
}