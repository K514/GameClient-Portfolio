using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace k514
{
    public partial class ResourceListTable
    {
        #region <Method/Query/Key>

        public override bool IsAddibleKey(string p_Key)
        {
            return !p_Key.IsBlockedAssetName();
        }

        #endregion

        #region <Method/Query/Record>

        public override bool TryGetRecord(string p_Key, out TableRecord o_Record)
        {
            if (string.IsNullOrWhiteSpace(p_Key))
            {
                o_Record = GetFallbackRecord();
                return false;
            }
            else
            {
                return base.TryGetRecord(p_Key, out o_Record);
            }
        }

        #endregion
        
        #region <Method/Query/Asset>

        /// <summary>
        /// 에셋 이름으로부터 해당 에셋을 유니티 함수를 통해 로드할 패스 포맷을 찾아 리턴하는 메서드
        /// </summary>
        public string GetUnityResourceLoadPath(string p_AssetName)
        {
            return GetTable()[p_AssetName].UnityResourceLoadPath;
        }
        
        /// <summary>
        /// 에셋 이름으로부터 해당 에셋을 포함하는 번들 이름을 찾아 리턴하는 메서드
        /// </summary>
        public string GetAssetBundleName(string p_AssetName)
        {
            return GetTable()[p_AssetName].AssetBundleName;
        }

        /// <summary>
        /// 리소스 리스트로부터 특정 확장자로 끝나는 에셋의 레코드를 찾아 리스트로 리턴하는 메서드
        /// </summary>
        /// <returns></returns>
        public List<TableRecord> SearchResourcesByExtension(List<string> p_ExtList)
        {
            if (p_ExtList.CheckCollectionSafe())
            {
                var result = new List<TableRecord>();
                var thisTable = GetTable();
                foreach (var recordKV in thisTable)
                {
                    var tryKey = recordKV.Key;
                    if (tryKey.IsEndWithAny(p_ExtList))
                    {
                        result.Add(recordKV.Value);
                    }
                }

                return result;
            }
            else
            {
                return null;
            }
        }     

        #endregion
    }
}