using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace k514.Mono.Common
{
    /// <summary>
    /// 오디오 컴포넌트 테이블 클래스의 브릿지 인터페이스
    /// </summary>
    public interface IAudioClipNameTableBridge<out RecordBridge> : IResourceNameTableBridge<RecordBridge, AudioClip>, ITableBridgeLabel<AudioClipNameTableQuery.TableLabel> 
        where RecordBridge : class, IAudioClipNameTableRecordBridge
    {
    }

    /// <summary>
    /// 오디오 컴포넌트 테이블 레코드 클래스의 브릿지 인터페이스
    /// </summary>
    public interface IAudioClipNameTableRecordBridge : IResourceNameTableRecord<AudioClip>
    {
        public List<AssetLoadResult<AudioClip>> LoadAudioClipList(ResourceLifeCycleType p_Type);
        public UniTask<List<AssetLoadResult<AudioClip>>> LoadAudioClipList(ResourceLifeCycleType p_Type, CancellationToken p_Token);
    }
    
    /// <summary>
    /// 오디오 컴포넌트 테이블 클래스의 기저 클래스
    /// </summary>
    public abstract class AudioClipNameTable<Table, Record> : ResourceNameMapTableBridge<Table, Record, IAudioClipNameTableRecordBridge, AudioClip>, IAudioClipNameTableBridge<IAudioClipNameTableRecordBridge>
        where Table : AudioClipNameTable<Table, Record>, new()
        where Record : AudioClipNameTable<Table, Record>.AudioClipNameTableRecord, new()
    {
        #region <Fields>

        protected AudioClipNameTableQuery.TableLabel _AudioClipTableLabel;

        AudioClipNameTableQuery.TableLabel ITableBridgeLabel<AudioClipNameTableQuery.TableLabel>.TableLabel => _AudioClipTableLabel;

        #endregion

        #region <Record>

        /// <summary>
        /// 오디오 컴포넌트 테이블 클래스의 기저 클래스
        /// </summary>
        [Serializable]
        public abstract class AudioClipNameTableRecord : ResourceNameTableRecord, IAudioClipNameTableRecordBridge
        {
            public virtual List<AssetLoadResult<AudioClip>> LoadAudioClipList(ResourceLifeCycleType p_Type)
            {
                var result = new List<AssetLoadResult<AudioClip>>();
                result.Add(GetResource(p_Type));
                
                return result;
            }

            public virtual async UniTask<List<AssetLoadResult<AudioClip>>> LoadAudioClipList(ResourceLifeCycleType p_Type, CancellationToken p_Token)
            {
                var result = new List<AssetLoadResult<AudioClip>>();
                result.Add(await GetResourceAsync(p_Type, p_Token));
                
                return result;
            }
        }

        #endregion
    }
}
