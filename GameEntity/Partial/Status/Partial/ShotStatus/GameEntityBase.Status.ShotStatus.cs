using System.Collections.Generic;

namespace k514.Mono.Common
{
    public partial class GameEntityBase<Content, CreateParams, ActivateParams>
    {
        #region <Fields>

        /// <summary>
        /// 샷 능력치 테이블
        /// </summary>
        private Dictionary<StatusTool.ShotStatusGroupType, ShotStatusPresetWrapper> _ShotStatusTable;

        #endregion
        
        #region <Indexer>

        public ShotStatusPreset this[StatusTool.ShotStatusGroupType p_Group] => _ShotStatusTable[p_Group];
        public float this[StatusTool.ShotStatusGroupType p_Group, ShotStatusTool.ShotStatusType p_Type] => _ShotStatusTable[p_Group][p_Type];
        public float this[StatusTool.ShotStatusGroupType p_Group, ShotStatusTool.ShotStatusType p_Type, float p_Rate] => _ShotStatusTable[p_Group][p_Type, p_Rate];

        #endregion

        #region <Callbacks>
        
        private void OnCreateShotStatus()
        {
            _ShotStatusTable = new Dictionary<StatusTool.ShotStatusGroupType, ShotStatusPresetWrapper>();

            var enumerator = EnumFlag.GetEnumEnumerator<StatusTool.ShotStatusGroupType>(EnumFlag.GetEnumeratorType.GetAll);
            foreach (var ShotStatusGroupType in enumerator)
            {
                switch (ShotStatusGroupType)
                {
                    case StatusTool.ShotStatusGroupType.SimpleMul:
                    case StatusTool.ShotStatusGroupType.CompoundMul:
                        _ShotStatusTable.Add(ShotStatusGroupType, new ShotStatusPresetWrapper(ShotStatusTool.EinShotStatusPreset));
                        break;
                    case StatusTool.ShotStatusGroupType.ExtraAdd:
                    case StatusTool.ShotStatusGroupType.Main:
                    case StatusTool.ShotStatusGroupType.Total:
                        _ShotStatusTable.Add(ShotStatusGroupType, new ShotStatusPresetWrapper(ShotStatusTool.BasisShotStatusPreset));
                        break;
                }
            }

            OnCreateShotStatusChanged();
        }

        private void OnActivateShotStatus()
        {
            ResetShotStatus();
        }

        private void OnRetrieveShotStatus()
        {
        }
        
        #endregion

        #region <Methods>
        
        /// <summary>
        /// 지정한 능력치 그룹에 기저 능력치 값을 더하는 메서드
        ///
        /// 그룹 중에서는 Main, ExtraAdd/Mul1/Mul2/Current 값만 제어가 가능하며
        /// Total 계열 그룹은 나머지 그룹에 의해 갱신된다.
        /// </summary>
        public void AddStatus(StatusTool.ShotStatusGroupType p_GroupType, ShotStatusPreset p_Preset, StatusTool.StatusChangeParams p_Params = default)
        {
            switch (p_GroupType)
            {
                case StatusTool.ShotStatusGroupType.ExtraAdd:
                {
                    if (p_Preset.HasProperValue())
                    {
                        var extraAddStatus = _ShotStatusTable[StatusTool.ShotStatusGroupType.ExtraAdd];
                        var simpleMulStatus = _ShotStatusTable[StatusTool.ShotStatusGroupType.SimpleMul];
                        var compoundMulStatus = _ShotStatusTable[StatusTool.ShotStatusGroupType.CompoundMul];
                        var totalStatus = _ShotStatusTable[StatusTool.ShotStatusGroupType.Total];
                 
                        var enumerator = ShotStatusTool.ShotStatusTypeEnumerator;
                        foreach (var statusType in enumerator)
                        {
                            if (p_Preset.HasProperValue(statusType))
                            {
                                var addVal = p_Preset[statusType];
                                var deltaVal = addVal * simpleMulStatus[statusType] * compoundMulStatus[statusType];
                                extraAddStatus.AddProperty(statusType, addVal);
                                totalStatus.AddProperty(statusType, deltaVal);
                                
                                _ShotStatusChangeResultList.Add(new StatusTool.ShotStatusChangeResult(p_GroupType, statusType, p_Params, addVal, deltaVal));
                            }
                        }
                    }
                    break;
                }
                case StatusTool.ShotStatusGroupType.SimpleMul:
                {
                    if (p_Preset.HasProperValue())
                    {
                        var mainWithAddStatus = _ShotStatusTable[StatusTool.ShotStatusGroupType.Main] + _ShotStatusTable[StatusTool.ShotStatusGroupType.ExtraAdd];
                        var simpleMulStatus = _ShotStatusTable[StatusTool.ShotStatusGroupType.SimpleMul];
                        var compoundMulStatus = _ShotStatusTable[StatusTool.ShotStatusGroupType.CompoundMul];
                        var totalStatus = _ShotStatusTable[StatusTool.ShotStatusGroupType.Total];
                        
                        var enumerator = ShotStatusTool.ShotStatusTypeEnumerator;
                        foreach (var statusType in enumerator)
                        {
                            if (p_Preset.HasProperValue(statusType))
                            {
                                var addVal = p_Preset[statusType];
                                var deltaVal = addVal * mainWithAddStatus[statusType] * compoundMulStatus[statusType];
                                simpleMulStatus.AddProperty(statusType, addVal);
                                totalStatus.AddProperty(statusType, deltaVal);
                                
                                _ShotStatusChangeResultList.Add(new StatusTool.ShotStatusChangeResult(p_GroupType, statusType, p_Params, addVal, deltaVal));
                            }
                        }
                    }
                    break;
                }
                case StatusTool.ShotStatusGroupType.CompoundMul:
                {
                    if (p_Preset.HasProperValue())
                    {
                        var mainWithAddStatus = _ShotStatusTable[StatusTool.ShotStatusGroupType.Main] + _ShotStatusTable[StatusTool.ShotStatusGroupType.ExtraAdd];
                        var simpleMulStatus = _ShotStatusTable[StatusTool.ShotStatusGroupType.SimpleMul];
                        var compoundMulStatus = _ShotStatusTable[StatusTool.ShotStatusGroupType.CompoundMul];
                        var totalStatus = _ShotStatusTable[StatusTool.ShotStatusGroupType.Total];

                        var enumerator = ShotStatusTool.ShotStatusTypeEnumerator;
                        foreach (var statusType in enumerator)
                        {
                            if (p_Preset.HasProperValue(statusType))
                            {
                                var rateVal = p_Preset[statusType];
                                var addVal = (rateVal - 1f) * compoundMulStatus[statusType];
                                var deltaVal = addVal * mainWithAddStatus[statusType] * simpleMulStatus[statusType];
                                compoundMulStatus.AddProperty(statusType, addVal);
                                totalStatus.AddProperty(statusType, deltaVal);
                                
                                _ShotStatusChangeResultList.Add(new StatusTool.ShotStatusChangeResult(p_GroupType, statusType, p_Params, addVal, deltaVal));
                            }
                        }
                    }
                    break;
                }
            }

            ReserveShotStatusChange();
        }

        public void AddStatus(StatusTool.ShotStatusGroupType p_GroupType, ShotStatusTool.ShotStatusType p_StatusType, float p_Value, StatusTool.StatusChangeParams p_Params = default)
        {
            AddStatus(p_GroupType, new ShotStatusPreset(p_StatusType, p_Value), p_Params);
        }
        
        public void SetStatus(StatusTool.ShotStatusGroupType p_GroupType, ShotStatusTool.ShotStatusType p_StatusType, float p_Value, StatusTool.StatusChangeParams p_Params = default)
        {
            AddStatus(p_GroupType, p_StatusType, p_Value -  this[p_GroupType][p_StatusType], p_Params);
        }

        public void AddStatusRate(StatusTool.ShotStatusGroupType p_GroupType, ShotStatusTool.ShotStatusType p_StatusType, float p_Rate, StatusTool.StatusChangeParams p_Params = default)
        {
            AddStatus(p_GroupType, p_StatusType, this[StatusTool.ShotStatusGroupType.Total, p_StatusType, p_Rate], p_Params);
        }

        /// <summary>
        /// 기저 능력치를 초기화 시키는 메서드
        /// </summary>
        private void ResetShotStatus()
        {
            var enumerator = EnumFlag.GetEnumEnumerator<StatusTool.ShotStatusGroupType>(EnumFlag.GetEnumeratorType.GetAll);
            foreach (var statusGroupType in enumerator)
            {
                switch (statusGroupType)
                {
                    case StatusTool.ShotStatusGroupType.Main:
                    {
                        _ShotStatusTable[StatusTool.ShotStatusGroupType.Main].SetPreset(ShotStatusTool.BasisShotStatusPreset);
                        break;
                    }
                    case StatusTool.ShotStatusGroupType.ExtraAdd:
                    {
                        _ShotStatusTable[StatusTool.ShotStatusGroupType.ExtraAdd].SetPreset(0f);
                        break;
                    }
                    case StatusTool.ShotStatusGroupType.SimpleMul:
                    {
                        _ShotStatusTable[StatusTool.ShotStatusGroupType.SimpleMul].SetPreset(1f);
                        break;
                    }
                    case StatusTool.ShotStatusGroupType.CompoundMul:
                    {
                        _ShotStatusTable[StatusTool.ShotStatusGroupType.CompoundMul].SetPreset(1f);
                        break;
                    }
                    case StatusTool.ShotStatusGroupType.Total:
                    {
                        _ShotStatusTable[StatusTool.ShotStatusGroupType.Total] 
                            .SetPreset
                            (
                                (_ShotStatusTable[StatusTool.ShotStatusGroupType.Main] + _ShotStatusTable[StatusTool.ShotStatusGroupType.ExtraAdd]) 
                                * _ShotStatusTable[StatusTool.ShotStatusGroupType.SimpleMul] 
                                * _ShotStatusTable[StatusTool.ShotStatusGroupType.CompoundMul]
                            );
                        break;
                    }
                }
            }
        }
        
        /// <summary>
        /// 기저 능력치를 초기화 시키는 메서드
        /// </summary>
        private void ResetShotStatus(StatusTool.ShotStatusGroupType p_GroupType)
        {
            switch (p_GroupType)
            {
                case StatusTool.ShotStatusGroupType.Main:
                {
                    _ShotStatusTable[StatusTool.ShotStatusGroupType.Main].SetPreset(ShotStatusTool.BasisShotStatusPreset);
                    goto case StatusTool.ShotStatusGroupType.Total;
                }
                case StatusTool.ShotStatusGroupType.ExtraAdd:
                {
                    _ShotStatusTable[StatusTool.ShotStatusGroupType.ExtraAdd].SetPreset(0f);
                    goto case StatusTool.ShotStatusGroupType.Total;
                }
                case StatusTool.ShotStatusGroupType.SimpleMul:
                {
                    _ShotStatusTable[StatusTool.ShotStatusGroupType.SimpleMul].SetPreset(1f);
                    goto case StatusTool.ShotStatusGroupType.Total;
                }
                case StatusTool.ShotStatusGroupType.CompoundMul:
                {
                    _ShotStatusTable[StatusTool.ShotStatusGroupType.CompoundMul].SetPreset(1f);
                    goto case StatusTool.ShotStatusGroupType.Total;
                }
                case StatusTool.ShotStatusGroupType.Total:
                {
                    _ShotStatusTable[StatusTool.ShotStatusGroupType.Total] 
                        .SetPreset
                        (
                            (_ShotStatusTable[StatusTool.ShotStatusGroupType.Main] + _ShotStatusTable[StatusTool.ShotStatusGroupType.ExtraAdd]) 
                            * _ShotStatusTable[StatusTool.ShotStatusGroupType.SimpleMul] 
                            * _ShotStatusTable[StatusTool.ShotStatusGroupType.CompoundMul]
                        );
                    break;
                }
            }
        }

        #endregion
    }
}