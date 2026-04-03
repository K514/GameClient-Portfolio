using System.Collections.Generic;
using System.Text;
using System.Threading;
using Cysharp.Threading.Tasks;
using SimpleJSON;

namespace k514
{
    public class JsonTokenizer : Tokenizer<JsonTokenizer>
    {
        #region <Fields>

        private StringBuilder _stringBuilder;

        #endregion

        #region <Callbacks>

        protected override void OnCreate(ObjectCreateParams p_CreateParams)
        {
            base.OnCreate(p_CreateParams);

            _stringBuilder = new StringBuilder();
        }

        protected override void OnRetrieve(ObjectCreateParams p_CreateParams)
        {
            base.OnRetrieve(p_CreateParams);

            _stringBuilder.Clear();
        }

        #endregion
        
        #region <Methods>

        /// <summary>
        /// 문자열을 어휘데이터로 변환하는 메서드
        /// </summary>
        public override async UniTask<TableTool.TableLexicalData<KeyType>> Lexicalize<KeyType>(string p_RawText, CancellationToken p_Cancellation)
        {
            var tokens = await Tokenize(p_RawText, p_Cancellation);
            return await Lexicalize<KeyType>(tokens, p_Cancellation);
        }

        /// <summary>
        /// 문자열을 토큰단위로 나누는 메서드
        /// </summary>
        private async UniTask<JSONNode> Tokenize(string p_RawText, CancellationToken p_CancellationToken)
        {
            p_CancellationToken.ThrowIfCancellationRequested();
            return JSON.Parse(p_RawText);
        }

        /// <summary>
        /// 토큰을 어휘데이터로 변환하는 메서드
        /// </summary>
        private async UniTask<TableTool.TableLexicalData<KeyType>> Lexicalize<KeyType>(JSONNode p_JsonToken, CancellationToken p_Cancellation)
        {
            p_Cancellation.ThrowIfCancellationRequested();

            var metaSet = new Dictionary<string, string>();
            var recordSet = new Dictionary<KeyType, Dictionary<string, string>>();

            if (!ReferenceEquals(null, p_JsonToken))
            {
                // Root 태그가 테이블이었던 경우
                if (p_JsonToken.IsObject && p_JsonToken.HasKey(TableTool.XmlTableTagName))
                {
                    // Root 태그로부터 하위 엘리먼트를 가져온다.
                    var elementSet = p_JsonToken[TableTool.XmlTableTagName];
                    if (!ReferenceEquals(null, elementSet))
                    {
                        foreach (KeyValuePair<string, JSONNode> elementSetValue in elementSet)
                        {
                            switch (elementSetValue.Key) 
                            {
                                case var _ when string.IsNullOrEmpty(elementSetValue.Key) || string.IsNullOrEmpty(elementSetValue.Value.ToString()):
                                    continue;
                                case var _ when elementSetValue.Key == TableTool.XmlMetaTagName:
                                    {
                                        foreach (KeyValuePair<string, JSONNode> subElement in elementSetValue.Value)
                                        {
                                            p_Cancellation.ThrowIfCancellationRequested();

                                            var subElementTag = subElement.Key;
                                            var subElementText = ConvertJsonValueToString(subElement.Value);

                                            switch (subElementTag)
                                            {
                                                case var _ when string.IsNullOrEmpty(subElementTag) || string.IsNullOrEmpty(subElementText):
                                                    continue;
                                                default:
                                                    {
                                                        if (!metaSet.ContainsKey(subElementTag))
                                                        {
                                                            metaSet.Add(subElementTag, subElementText);
                                                        }
                                                        break;
                                                    }
                                            }
                                        }
                                        break;
                                    }
                                case var _ when elementSetValue.Key == TableTool.XmlRecordTagName:
                                    {
                                        var dataSet = new Dictionary<string, string>();

                                        foreach (KeyValuePair<string, JSONNode> subElement in elementSetValue.Value)
                                        {
                                            p_Cancellation.ThrowIfCancellationRequested();

                                            if (subElement.Value.Count > 0)
                                            {
                                                var dataSetSecond = new Dictionary<string, string>();
                                                foreach (KeyValuePair<string, JSONNode> subElementSecond in subElement.Value)
                                                {
                                                    AddRecord<KeyType>(subElementSecond.Key, subElementSecond.Value, recordSet, dataSetSecond, p_Cancellation);
                                                }
                                            }
                                            else
                                            {
                                                AddRecord<KeyType>(subElement.Key, subElement.Value, recordSet, dataSet, p_Cancellation);
                                            }
                                        }
                                        break;
                                    }
                            }
                        }
                    }
                }
            }
            return new TableTool.TableLexicalData<KeyType>(metaSet, recordSet);
        }
        
        private void AddRecord<KeyType>(string p_Key, JSONNode p_Value, Dictionary<KeyType, Dictionary<string, string>> p_RecordSet, Dictionary<string, string> p_DataSet, CancellationToken p_Cancellation)
        {
            var keyType = typeof(KeyType);
            var subElementTag = p_Key;
            var subElementText = ConvertJsonValueToString(p_Value);
            p_Cancellation.ThrowIfCancellationRequested();

            if (string.IsNullOrEmpty(subElementText)) return;
            switch (subElementTag)
            {
                case TableTool.TableKeyTagName:
                    {
                        var key = (KeyType)subElementText.DecodeValue(keyType);
                        if (ReferenceEquals(null, key) || p_RecordSet.ContainsKey(key))
                        {
                            return;
                        }
                        else
                        {
                            p_RecordSet.Add(key, p_DataSet);
                        }
                        break;
                    }
                default:
                    {
                        
                        if (!p_DataSet.ContainsKey(subElementTag))
                        {
                            p_DataSet.Add(subElementTag, subElementText);
                        }

                        break;
                    }
            }
        }
        
        public string ConvertJsonValueToString(JSONNode p_JsonNode)
        {
            _stringBuilder.Clear();
            _stringBuilder.Append(p_JsonNode.ToString());
            var strLength = _stringBuilder.Length - 1;
            _stringBuilder.Remove(strLength, 1).Remove(0, 1);//.Replace("\\\\", "\\");
            return _stringBuilder.ToString();
        }
    }
        #endregion
}