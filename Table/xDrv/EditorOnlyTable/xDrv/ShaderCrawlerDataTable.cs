

using System.Threading;
#if UNITY_EDITOR
using Cysharp.Threading.Tasks;

namespace k514
{
    /// <summary>
    /// 셰이더의 경우에는 기본적으로 GraphicSetting.asset에 빌드에 포함될 셰이더가 자동으로 추가되어야 하는데, Resource폴더의
    /// 랜더러들이 사용하는 셰이더는 빌드 시에, 자동 등록되지만 번들의 렌더러들이 사용하는 셰이더 중에서 유니티 기본 셰이더 들은
    /// 빌드 시에 포함되지 않기 때문에 수동으로 등록해주어야 한다. 기본 셰이더가 아닌 셰이더들은 의존성에 의해 에셋번들에 포함시키면
    /// 같이 로드되므로 문제가 없다.
    ///
    /// 셰이더 의존성을 만드려면, 우선 ShaderCrawler로 빌드에 포함될 셰이더를 등록시킨 다음에
    /// 에셋 번들을 만들면 된다.
    /// 
    /// </summary>
    public class ShaderCrawlerDataTable : EditorOnlyTable<ShaderCrawlerDataTable, TableMetaData, string, ShaderCrawlerDataTable.TableRecord>
    {
        #region <Record>

        public class TableRecord : EditorOnlyTableRecord
        {
            public bool IsDefaultTable { get; private set; }

            public override async UniTask SetRecord(string p_Key, object[] p_RecordField, CancellationToken p_CancellationToken)
            {
                await base.SetRecord(p_Key, p_RecordField, p_CancellationToken);
                
                IsDefaultTable = (bool)p_RecordField.GetElementSafe(0);
            }
        }

        #endregion

        #region <Methods>

        protected override async UniTask AddDefaultRecords(CancellationToken p_CancellationToken)
        {
            await base.AddDefaultRecords(p_CancellationToken);

            var enumerator = SystemMaintenance.DefaultShaderNameList;
            foreach (var shaderName in enumerator)
            {
                await AddRecord(shaderName, false, p_CancellationToken, true);
            }
        }
        
        #endregion
    }
}

#endif