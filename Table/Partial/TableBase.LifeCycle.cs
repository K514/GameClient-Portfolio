namespace k514
{
    public partial class TableBase<Table, Meta, Key, Record>
    {
        #region <Callbacks>

        private void OnDisposeLifeCycle()
        {
            if (_TableStateFlag.HasAnyFlagExceptNone(TableTool.TableStateFlag.SceneTerminate))
            {
                TableManager.GetInstanceUnsafe.RemoveSceneLifeCycleTable(this);
            }
        }

        #endregion
        
        #region <Methods>

        public void SetSceneLifeCycle(bool p_Flag)
        {
            if (p_Flag)
            {
                _TableStateFlag.AddFlag(TableTool.TableStateFlag.SceneTerminate);
                TableManager.GetInstanceUnsafe.AddSceneLifeCycleTable(this);
            }
            else
            {
                _TableStateFlag.RemoveFlag(TableTool.TableStateFlag.SceneTerminate);
                TableManager.GetInstanceUnsafe.RemoveSceneLifeCycleTable(this);
            }
        }

        #endregion
    }
}