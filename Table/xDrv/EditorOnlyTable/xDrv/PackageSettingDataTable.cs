using System.Threading;
using Cysharp.Threading.Tasks;

#if UNITY_EDITOR

namespace k514
{
    public class PackageSettingDataTable : EditorOnlyTable<PackageSettingDataTable, TableMetaData, int, PackageSettingDataTable.TableRecord>
    {
        #region <Consts>

        private const string __DefaultCompanyName = "c415kStudio";
        private const string __DefaultTestBuildName = "TestBuild";
        private const string __DefaultReleaseBuildName = "ReleaseBuild";
        
        #endregion
        
        #region <Records>

        public class TableRecord : EditorOnlyTableRecord
        {
            #region <Fields>

            public string CompanyName;
            public string TestBuildName;
            public string ReleaseBuildName;
            
            #endregion

            #region <Callbacks>

            public override async UniTask SetRecord(int p_Key, object[] p_RecordField, CancellationToken p_Cancellation)
            {
                await base.SetRecord(p_Key, p_RecordField, p_Cancellation);
                
                CompanyName = (string) p_RecordField.GetElementSafe(0);
                TestBuildName = (string) p_RecordField.GetElementSafe(1);
                ReleaseBuildName = (string) p_RecordField.GetElementSafe(2);
            }

            #endregion
        }

        #endregion
        
        #region <Callbacks>

        protected override async UniTask AddDefaultRecords(CancellationToken p_CancellationToken)
        {
            await base.AddDefaultRecords(p_CancellationToken);

            await AddRecord(0, false, p_CancellationToken, __DefaultCompanyName, __DefaultTestBuildName, __DefaultReleaseBuildName);
        }
        
        #endregion
    }
}

#endif