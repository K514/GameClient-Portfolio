using k514.Mono.Feature;

namespace k514.Mono.Common
{
    public partial class GameEntityBase<Content, CreateParams, ActivateParams>
    {
        #region <Fields>
        
        /// <summary>
        /// 경험치 테이블 키
        /// </summary>
        private EnhanceTable.TableRecord _EnhanceRecord;

        #endregion
        
        #region <Callbacks>

        private void OnCreateEnhance()
        {
            
            OnCreateLevel();
            OnCreateExp();
        }
        
        private void OnActivateEnhance()
        {
            _EnhanceRecord = ComponentDataRecord.GetEnhanceRecord();

            OnActivateLevel();
            OnActivateExp();
        }

        private void OnRetrieveEnhance()
        {
            OnRetrieveExp();
            OnRetrieveLevel();
        }
        
        #endregion

        #region <Methods>

        public bool TryChangeEnhanceRecord(int p_Index)
        {
            if (EnhanceTable.GetInstanceUnsafe.TryGetRecord(p_Index, out var o_Record))
            {
                var prevLevel = _CurrentLevel;
                SetLevel(1);
                _EnhanceRecord = o_Record;
                SetLevel(prevLevel);

                return true;
            }
            else
            {
                return false;
            }
        }
        
        public bool TryChangeEnhanceRecord(int p_Index, int p_StartLevel)
        {
            if (EnhanceTable.GetInstanceUnsafe.TryGetRecord(p_Index, out var o_Record))
            {
                SetLevel(1);
                _EnhanceRecord = o_Record;
                SetLevel(p_StartLevel);

                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion
    }
}