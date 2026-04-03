using System.Threading;
using Cysharp.Threading.Tasks;
using k514.Mono.Common;

namespace k514
{
    public abstract class Tokenizer<TableType> : ObjectPoolContent<TableType, ObjectCreateParams, ObjectActivateParams> where TableType : ObjectPoolContent<TableType, ObjectCreateParams, ObjectActivateParams>, new()
    {
        #region <Callbacks>
        
        protected override void OnCreate(ObjectCreateParams p_CreateParams)
        {
        }

        protected override bool OnActivate(ObjectCreateParams p_CreateParams, ObjectActivateParams p_ActivateParams, bool p_IsPooled)
        {
            return true;
        }

        protected override void OnRetrieve(ObjectCreateParams p_CreateParams)
        {
        }
        
        protected override void OnDispose()
        {
        }
        
        #endregion

        #region <Methods>

        public abstract UniTask<TableTool.TableLexicalData<KeyType>> Lexicalize<KeyType>(string p_RawText, CancellationToken p_Cancellation);

        #endregion
    }
}