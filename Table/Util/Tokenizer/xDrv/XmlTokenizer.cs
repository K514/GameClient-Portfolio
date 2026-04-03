using System;
using System.Collections.Generic;
using System.Security;
using System.Threading;
using Cysharp.Threading.Tasks;
using k514.Mono.Common;
using xk514;

namespace k514
{
    public class XmlTokenizer : Tokenizer<XmlTokenizer>
    {
        #region <Fields>

        private SecurityParser _securityParser;
    
        #endregion

        #region <Callbacks>

        protected override bool OnActivate(ObjectCreateParams p_CreateParams, ObjectActivateParams p_ActivateParams, bool p_IsPooled)
        {
            if (base.OnActivate(p_CreateParams, p_ActivateParams, p_IsPooled))
            {
                _securityParser = new SecurityParser();
                
                return true;
            }
            else
            {
                return false;
            }
            
        }

        protected override void OnRetrieve(ObjectCreateParams p_CreateParams)
        {
            base.OnRetrieve(p_CreateParams);

            _securityParser = null;
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
        private async UniTask<SecurityElement> Tokenize(string p_RawText, CancellationToken p_CancellationToken)
        {
            p_CancellationToken.ThrowIfCancellationRequested();
            _securityParser.LoadXml(p_RawText);

            return _securityParser.ToXml();
        }

        /// <summary>
        /// 토큰을 어휘데이터로 변환하는 메서드
        /// </summary>
        private async UniTask<TableTool.TableLexicalData<KeyType>> Lexicalize<KeyType>(SecurityElement p_XmlToken, CancellationToken p_Cancellation)
        {
            p_Cancellation.ThrowIfCancellationRequested();

            var metaSet = new Dictionary<string, string>();
            var recordSet = new Dictionary<KeyType, Dictionary<string, string>>();

            if (!ReferenceEquals(null, p_XmlToken))
            {
                // Root 태그가 테이블이었던 경우
                if (p_XmlToken.Tag == TableTool.XmlTableTagName)
                {
                    // Root 태그로부터 하위 엘리먼트를 가져온다.
                    var elementSet = p_XmlToken.Children;
                    if (elementSet.CheckCollectionSafe())
                    {
                        var elementCount = elementSet.Count;
                        for (var i = 0; i < elementCount; i++)
                        {
                            p_Cancellation.ThrowIfCancellationRequested();

                            // 하위 엘리먼트를 가져온다.
                            var element = elementSet[i] as SecurityElement;
                            var subElementSet = element.Children;

                            switch (subElementSet)
                            {
                                case var _ when !subElementSet.CheckCollectionSafe():
                                {
                                    continue;
                                }
                                case var _ when element.Tag == TableTool.XmlMetaTagName:
                                {
                                    var subElementCount = subElementSet.Count;
                             
                                    for (var j = 0; j < subElementCount; j++)
                                    {
                                        p_Cancellation.ThrowIfCancellationRequested();

                                        var subElement = subElementSet[j] as SecurityElement;
                                        var subElementTag = subElement.Tag;
                                        var subElementText = subElement.Text;

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
                                case var _ when element.Tag == TableTool.XmlRecordTagName:
                                {
                                    var keyType = typeof(KeyType);
                                    var dataSet = new Dictionary<string, string>();
                                    var subElementCount = subElementSet.Count;
                                
                                    for (var j = 0; j < subElementCount; j++)
                                    {
                                        p_Cancellation.ThrowIfCancellationRequested();

                                        var subElement = subElementSet[j] as SecurityElement;
                                        var subElementTag = subElement.Tag;
                                        var subElementText = subElement.Text;
                                  
                                        switch (subElementTag)
                                        {
                                            case var _ when string.IsNullOrEmpty(subElementTag) || string.IsNullOrEmpty(subElementText):
                                                continue;
                                            case TableTool.TableKeyTagName:
                                            {
                                                try
                                                {
                                                    var key = (KeyType) subElementText.DecodeValue(keyType);
                                                    if (ReferenceEquals(null, key) || recordSet.ContainsKey(key))
                                                    {
                                                        continue;
                                                    }
                                                    else
                                                    {
                                                        recordSet.Add(key, dataSet);
                                                    }
                                                }
#if UNITY_EDITOR
                                                catch (Exception e)
                                                {
                                                    CustomDebug.LogError(($"[Lexicalize Error] {subElementText} -> Type[{typeof(KeyType)}]", UnityEngine.Color.red));
                                                    throw;
                                                }
#else
                                                catch
                                                {
                                                    throw;
                                                }
#endif
                                                break;
                                            }
                                            default:
                                            {
                                                if (!dataSet.ContainsKey(subElementTag))
                                                {
                                                    dataSet.Add(subElementTag, subElementText);
                                                }
                                                break;
                                            }
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
        
        #endregion
    }
}