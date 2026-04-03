using System.Threading;
using Cysharp.Threading.Tasks;
using k514.Mono.Common;
using UnityEngine;

namespace k514
{
    public abstract class GameConfigureDataStorageBase<This, Table, Record> : DataStorageBase<This, Table, Record> 
        where This : GameConfigureDataStorageBase<This, Table, Record>, new()
        where Table : class, IGameConfigureDataTable<Record>
        where Record : class, IGameConfigureDataTableRecord<Record>, new()
    {
        #region <Properties>

        public Record DefaultDataRecord => _DataTable.GetRecord(0);
        public override Record DataRecord => _DataTable.GetRecord(1);
        public Record TempRecord => _DataTable.GetRecord(2);
        
        #endregion
        
        #region <Callbacks>

        protected override void TryInitializeDependency()
        {
            base.TryInitializeDependency();

            _Dependencies.Add(typeof(AudioManager));
        }

        protected override async UniTask OnCreated(CancellationToken p_CancellationToken)
        {
            _DataKey = "CFFF";

            await base.OnCreated(p_CancellationToken);
            
            await ApplyConfig(false, GetCancellationToken());
        }

        #endregion
        
        #region <Methods>

        public async UniTask ApplyConfig(bool p_SaveFlag, CancellationToken p_Token)
        {
            await SystemBoot.SwitchLanguage(DataRecord.LanguageType, p_Token);
            
            AudioManager.GetInstanceUnsafe.SetVolume(AudioClipNameTableQuery.TableLabel.None, DataRecord.MasterVolumeMuteFlag ? 0f : DataRecord.MasterVolume);
            AudioManager.GetInstanceUnsafe.SetVolume(AudioClipNameTableQuery.TableLabel.BGM, DataRecord.BgmVolumeMuteFlag ? 0f : DataRecord.BgmVolume);
            AudioManager.GetInstanceUnsafe.SetVolume(AudioClipNameTableQuery.TableLabel.Effect, DataRecord.EffectVolumeMuteFlag ? 0f : DataRecord.EffectVolume);
            
            if (p_SaveFlag)
            {
                await SaveData(p_Token);
            }
        }
        
        public async UniTask TurnToDefault(bool p_SaveFlag, CancellationToken p_Token)
        {
            DataRecord.OverlapRecord(DefaultDataRecord);
            
            await ApplyConfig(p_SaveFlag, p_Token);
        }
        
        public async UniTask TurnToBackUp(bool p_SaveFlag, CancellationToken p_Token)
        {
            DataRecord.OverlapRecord(TempRecord);
            
            await ApplyConfig(p_SaveFlag, p_Token);
        }
        
        public void BackUpData()
        {
            TempRecord.OverlapRecord(DataRecord);
        }

        #endregion
    }
}