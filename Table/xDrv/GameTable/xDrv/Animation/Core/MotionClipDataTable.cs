using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace k514.Mono.Common
{
    public interface IMotionClipDataTable : ITableBridge<string, TableMetaData, IMotionClipDataTableRecord>, ITableBridgeLabel<MotionClipDataTableQuery.TableLabel>
    {
    }

    public interface IMotionClipDataTableRecord : ITableRecord
    {
        List<ClipEventPreset> ClipEventPresetList { get; }
        AnimationTool.MotionPlaceType MotionPlaceType { get; }
    }
    
    /// <summary>
    /// 모션 클립의 지정한 타임 레이트(TimeRate01)에 특정 타입의 콜백 타임스탬프를 추가하는 테이블
    /// </summary>
    public abstract class MotionClipDataTableBase<This> : GameTableBridge<This, TableMetaData, string, MotionClipDataTableBase<This>.TableRecord, IMotionClipDataTableRecord>, IMotionClipDataTable
        where This : MotionClipDataTableBase<This>, new()
    {
        #region <Fields>

        protected MotionClipDataTableQuery.TableLabel _MotionClipLabel;

        MotionClipDataTableQuery.TableLabel ITableBridgeLabel<MotionClipDataTableQuery.TableLabel>.TableLabel => _MotionClipLabel;

        #endregion

        #region <Record>
        
        [Serializable]
        public class TableRecord : GameTableRecord, IMotionClipDataTableRecord
        {
            #region <Fields>

            public List<ClipEventPreset> ClipEventPresetList { get; protected set; }
            public AnimationTool.MotionPlaceType MotionPlaceType { get; protected set; }

            #endregion

            #region <Callbacks>

            public override async UniTask SetRecord(string p_Key, object[] p_RecordField, CancellationToken p_CancellationToken)
            {
                await base.SetRecord(p_Key, p_RecordField, p_CancellationToken);
                
                ClipEventPresetList = (List<ClipEventPreset>)p_RecordField.GetElementSafe(0);
                MotionPlaceType = (AnimationTool.MotionPlaceType)p_RecordField.GetElementSafe(1);
            }

            #endregion
        }
        
        #endregion
    }
}