#if UNITY_EDITOR

using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using xk514;

namespace k514
{
    public partial class TableBase<Table, Meta, Key, Record>
    {
        /// <summary>
        /// 직렬화 타입에 따라 테이블 데이터를 직렬화하여 파일로 쓰는 메서드
        /// </summary>
        public async UniTask SerializeTable(CancellationToken p_Token)
        {
            try
            {
                switch (TableSerializeType)
                {
                    case TableTool.TableSerializeType.NoneSerialize:
                    {
#if APPLY_PRINT_LOG
                        CustomDebug.LogError(($"{typeof(Table)} is NoneSerialize Type Table", Color.yellow));
#endif
                        break;
                    }
                    case TableTool.TableSerializeType.SerializeBinaryTableImage:
                    {
                        await WriteBinaryTableImage(p_Token);
                        break;
                    }
                }
            }
            catch(Exception e)
            {
#if APPLY_PRINT_LOG
                CustomDebug.LogError(($"{typeof(Table)} 직렬화에 실패했습니다. : {e.Message}\n\n{e.StackTrace}", Color.yellow));
#endif
            }
        }
    }
}

#endif