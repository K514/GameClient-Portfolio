using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace k514
{
    public interface IAccountDataTable<Record> : IDataStorageTable<Record>
        where Record : IAccountDataTableRecord<Record>
    {
    }

    public interface IAccountDataTableRecord<Record> : IDataStorageTableRecord<int, Record> 
        where Record : IAccountDataTableRecord<Record>
    {
    }

    public abstract class AccountDataTable<Table, Record> : DataStorageTable<Table, Record>, IAccountDataTable<Record>
        where Table : AccountDataTable<Table, Record>, new() 
        where Record : AccountDataTable<Table, Record>.AccountTableRecordBase, new()
    {
        #region <Record>

        [Serializable]
        public abstract class AccountTableRecordBase : DataStorageTableRecordBase, IAccountDataTableRecord<Record> 
        {
            #region <Fields>

            /// <summary>
            /// 닉네임
            /// </summary>
            public string NickName { get; protected set; }
      
            /// <summary>
            /// 마지막 접속 일자
            /// </summary>
            public string LastAccess { get; protected set; }

            /// <summary>
            /// 게임 진행도
            /// </summary>
            public int Progress { get; protected set; }

            #endregion

            #region <Callbacks>

            public override async UniTask OnRecordAdded(Table p_Table, CancellationToken p_CancellationToken)
            {
                await base.OnRecordAdded(p_Table, p_CancellationToken);

                NickName ??= string.Empty;
                LastAccess ??= string.Empty;
            }

            #endregion

            #region <Methods>

            public override async UniTask SetRecord(int p_Key, object[] p_RecordField, CancellationToken p_CancellationToken)
            {
                await base.SetRecord(p_Key, p_RecordField, p_CancellationToken);

                NickName = p_RecordField.As<string>(0);
                LastAccess = p_RecordField.As<string>(1);
                Progress = p_RecordField.As<int>(2);
            }

            #endregion
        }

        #endregion

        #region <Callbacks>

        protected override async UniTask AddDefaultRecords(CancellationToken p_CancellationToken)
        {
            await base.AddDefaultRecords(p_CancellationToken);

            await AddRecord(0, false, p_CancellationToken);
        }

        #endregion
    }
}