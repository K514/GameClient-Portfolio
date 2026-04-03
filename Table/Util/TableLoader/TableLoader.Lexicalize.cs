using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace k514
{
    public partial class TableLoader
    {
        /// <summary>
        /// 텍스트 에셋을 테이블 어휘 데이터로 변환하는 메서드
        /// </summary>
        public async UniTask<TableTool.TableLexicalData<KeyType>> LexicalizeText<KeyType>(TableTool.TableFileType p_Type, TextAsset p_Text, CancellationToken p_Cancellation)
        {
            return await LexicalizeText<KeyType>(p_Type, p_Text.text, p_Cancellation);
        }

        /// <summary>
        /// 문자열을 테이블 어휘 데이터로 변환하는 메서드
        /// </summary>
        public async UniTask<TableTool.TableLexicalData<KeyType>> LexicalizeText<KeyType>(TableTool.TableFileType p_Type, string p_Text, CancellationToken p_Cancellation)
        {
            switch (p_Type)
            {
                case var _ when string.IsNullOrWhiteSpace(p_Text):
                    return default;
                case TableTool.TableFileType.Xml:
                {
                    var tokenizer = _PoolCluster.Pop<XmlTokenizer>(TableTool.TableFileType.Xml, default);
                    var lexicalizeData = await tokenizer.Lexicalize<KeyType>(p_Text, p_Cancellation);
                    tokenizer.Pooling();
                    
                    return lexicalizeData;
                }
                case TableTool.TableFileType.JSON:
                    {
                        var tokenizer = _PoolCluster.Pop<JsonTokenizer>(TableTool.TableFileType.JSON, default);
                        var lexicalizeData = await tokenizer.Lexicalize<KeyType>(p_Text, p_Cancellation);
                        tokenizer.Pooling();

                        return lexicalizeData;
                    }
                case TableTool.TableFileType.CSV:
                {
                    var tokenizer = _PoolCluster.Pop<CsvTokenizer>(TableTool.TableFileType.CSV, default);
                    var lexicalizeData = await tokenizer.Lexicalize<KeyType>(p_Text, p_Cancellation);
                    tokenizer.Pooling();
                    
                    return lexicalizeData;
                }
                default:
                    return default;
            }
        }
    }
}