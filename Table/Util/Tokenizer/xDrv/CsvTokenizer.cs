using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace k514
{
    public class CsvTokenizer : Tokenizer<CsvTokenizer>
    {
        public override async UniTask<TableTool.TableLexicalData<KeyType>> Lexicalize<KeyType>(string p_RawText, CancellationToken p_Cancellation)
        {
            p_Cancellation.ThrowIfCancellationRequested();

            var keyType = typeof(KeyType);
            var recordSet = new Dictionary<KeyType, Dictionary<string, string>>();
            var lines = p_RawText.Split('\n');
            if (lines.CheckCollectionSafe(1))
            {
                var lineCount = lines.Length;
                var fieldNameLine = lines[0].TrimStart().TrimEnd();
                var fieldNameTokens = fieldNameLine.Split(',');
                
                for (var i = 1; i < lineCount; i++)
                {
                    var tryLine = lines[i];
                    if (!string.IsNullOrEmpty(tryLine))
                    {
                        var tokens = tryLine.SplitWithSymbol(',', StringTool.StringSplitFlag.SplitBracket | StringTool.StringSplitFlag.RemoveEmpty);
                        if (tokens.CheckCollectionSafe())
                        {
                            var tokenCount = tokens.Count;
                            var fieldLex = new Dictionary<string, string>();
                            var tryKeyValue = (KeyType) tokens[0].DecodeValue(keyType);
                            
                            recordSet.Add(tryKeyValue, fieldLex);
                            if (lines.CheckCollectionSafe(1))
                            {
                                for (var j = 1; j < tokenCount; j++)
                                {
                                    fieldLex.Add(fieldNameTokens[j], tokens[j]);
                                }
                            }
                        }
                    }
                }
            }
            
            return new TableTool.TableLexicalData<KeyType>(default, recordSet);
        }
    }
}