using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace k514
{
    public interface IGameConfigureDataTable<Record> : IDataStorageTable<Record>
        where Record : IGameConfigureDataTableRecord<Record>
    {
    }

    public interface IGameConfigureDataTableRecord<Record> : IDataStorageTableRecord<int, Record> 
        where Record : IGameConfigureDataTableRecord<Record>
    {
        public LanguageType LanguageType { get; }
        public float MasterVolume { get; }
        public bool MasterVolumeMuteFlag { get; }
        public float BgmVolume { get; }
        public bool BgmVolumeMuteFlag { get; }
        public float EffectVolume { get; }
        public bool EffectVolumeMuteFlag { get; }
        public float VoiceVolume { get; }
        public bool VoiceVolumeMuteFlag { get; set; }
    }

    public abstract class GameConfigureDataTable<Table, Record> : DataStorageTable<Table, Record>, IGameConfigureDataTable<Record>
        where Table : GameConfigureDataTable<Table, Record>, new() 
        where Record : GameConfigureDataTable<Table, Record>.GameConfigureTableRecordBase, new()
    {
        #region <Record>

        [Serializable]
        public abstract class GameConfigureTableRecordBase : DataStorageTableRecordBase, IGameConfigureDataTableRecord<Record>
        {
            #region <Fields>

            /// <summary>
            /// 현재 언어
            /// </summary>
            public LanguageType LanguageType { get; set; }
      
            /// <summary>
            /// 마스터 볼륨
            /// </summary>
            public float MasterVolume { get; set; }
            
            /// <summary>
            /// 마스터 볼륨 뮤트
            /// </summary>
            public bool MasterVolumeMuteFlag { get; set; }

            /// <summary>
            /// Bgm 볼륨
            /// </summary>
            public float BgmVolume { get; set; }
            
            /// <summary>
            /// Bgm 볼륨 뮤트
            /// </summary>
            public bool BgmVolumeMuteFlag { get; set; }
            
            /// <summary>
            /// 이펙트 볼륨
            /// </summary>
            public float EffectVolume { get; set; }
                        
            /// <summary>
            /// 이펙트 볼륨 뮤트
            /// </summary>
            public bool EffectVolumeMuteFlag { get; set; }
            
            /// <summary>
            /// 음성 볼륨
            /// </summary>
            public float VoiceVolume { get; set; }
                        
            /// <summary>
            /// 음성 볼륨 뮤트
            /// </summary>
            public bool VoiceVolumeMuteFlag { get; set; }

            #endregion

            #region <Methods>

            public override async UniTask SetRecord(int p_Key, object[] p_RecordField, CancellationToken p_CancellationToken)
            {
                await base.SetRecord(p_Key, p_RecordField, p_CancellationToken);

                LanguageType = p_RecordField.As<LanguageType>(0);
                MasterVolume = p_RecordField.As<float>(1);
                MasterVolumeMuteFlag = p_RecordField.As<bool>(2);
                BgmVolume = p_RecordField.As<float>(3);
                BgmVolumeMuteFlag = p_RecordField.As<bool>(4);
                EffectVolume = p_RecordField.As<float>(5);
                EffectVolumeMuteFlag = p_RecordField.As<bool>(6);
                VoiceVolume = p_RecordField.As<float>(7);
                VoiceVolumeMuteFlag = p_RecordField.As<bool>(8);
            }

            public override void OverlapRecord(Record p_Record)
            {
                base.OverlapRecord(p_Record);

                LanguageType = p_Record.LanguageType;
                MasterVolume = p_Record.MasterVolume;
                MasterVolumeMuteFlag = p_Record.MasterVolumeMuteFlag;
                BgmVolume = p_Record.BgmVolume;
                BgmVolumeMuteFlag = p_Record.BgmVolumeMuteFlag;
                EffectVolume = p_Record.EffectVolume;
                EffectVolumeMuteFlag = p_Record.EffectVolumeMuteFlag;
                VoiceVolume = p_Record.VoiceVolume;
                VoiceVolumeMuteFlag = p_Record.VoiceVolumeMuteFlag;
            }

            #endregion
        }

        #endregion

        #region <Callbacks>

        protected override async UniTask AddDefaultRecords(CancellationToken p_CancellationToken)
        {
            await base.AddDefaultRecords(p_CancellationToken);
            
            await AddRecord
            (
                0, true, p_CancellationToken,
                LanguageType.KOR, 
                1f, false,
                1f, false,
                1f, false,
                1f, false
            );
            
            await AddRecord
            (
                0, false, p_CancellationToken,
                LanguageType.KOR, 
                1f, false,
                1f, false,
                1f, false,
                1f, false
            );
            
            await AddRecord
            (
                0, true, p_CancellationToken,
                LanguageType.KOR, 
                1f, false,
                1f, false,
                1f, false,
                1f, false
            );
        }

        #endregion
    }
}