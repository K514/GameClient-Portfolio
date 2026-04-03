using System.Collections.Generic;

namespace k514.Mono.Common
{
    public partial class GameEntityBase<Content, CreateParams, ActivateParams>
    {
        #region <Fields>

        /// <summary>
        /// 기저능력치 테이블
        /// </summary>
        private Dictionary<StatusTool.BaseStatusGroupType, BaseStatusPresetWrapper> _BaseStatusTable;

        #endregion
        
        #region <Indexer>

        public BaseStatusPreset this[StatusTool.BaseStatusGroupType p_Group] => _BaseStatusTable[p_Group];
        public float this[StatusTool.BaseStatusGroupType p_Group, BaseStatusTool.BaseStatusType p_Type] => _BaseStatusTable[p_Group][p_Type];
        public float this[StatusTool.BaseStatusGroupType p_Group, BaseStatusTool.BaseStatusType p_Type, float p_Rate] => _BaseStatusTable[p_Group][p_Type, p_Rate];

        #endregion

        #region <Callbacks>
        
        private void OnCreateBaseStatus()
        {
            _BaseStatusTable = new Dictionary<StatusTool.BaseStatusGroupType, BaseStatusPresetWrapper>();

            var enumerator = EnumFlag.GetEnumEnumerator<StatusTool.BaseStatusGroupType>(EnumFlag.GetEnumeratorType.GetAll);
            foreach (var baseStatusGroupType in enumerator)
            {
                switch (baseStatusGroupType)
                {
                    case StatusTool.BaseStatusGroupType.SimpleMul:
                    case StatusTool.BaseStatusGroupType.CompoundMul:
                        _BaseStatusTable.Add(baseStatusGroupType, new BaseStatusPresetWrapper(BaseStatusTool.EinBaseStatusPreset));
                        break;
                    case StatusTool.BaseStatusGroupType.ExtraAdd:
                    case StatusTool.BaseStatusGroupType.Main:
                    case StatusTool.BaseStatusGroupType.Total:
                        _BaseStatusTable.Add(baseStatusGroupType, new BaseStatusPresetWrapper(BaseStatusTool.BasisBaseStatusPreset));
                        break;
                }
            }

            OnCreateBaseStatusChanged();
        }

        private void OnActivateBaseStatus()
        {
            ResetBaseStatus();
        }

        private void OnRetrieveBaseStatus()
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
        public void AddStatus(StatusTool.BaseStatusGroupType p_GroupType, BaseStatusPreset p_Preset, StatusTool.StatusChangeParams p_Params = default)
        {
            switch (p_GroupType)
            {
                case StatusTool.BaseStatusGroupType.ExtraAdd:
                {
                    if (p_Preset.HasProperValue())
                    {
                        var extraAddStatus = _BaseStatusTable[StatusTool.BaseStatusGroupType.ExtraAdd];
                        var simpleMulStatus = _BaseStatusTable[StatusTool.BaseStatusGroupType.SimpleMul];
                        var compoundMulStatus = _BaseStatusTable[StatusTool.BaseStatusGroupType.CompoundMul];
                        var totalStatus = _BaseStatusTable[StatusTool.BaseStatusGroupType.Total];
                        
                        var enumerator = BaseStatusTool.BaseStatusTypeEnumerator;
                        foreach (var statusType in enumerator)
                        {
                            if (p_Preset.HasProperValue(statusType))
                            {
                                var addVal = p_Preset[statusType];
                                var deltaVal = addVal * simpleMulStatus[statusType] * compoundMulStatus[statusType];
                                extraAddStatus.AddProperty(statusType, addVal);
                                totalStatus.AddProperty(statusType, deltaVal);
                                
                                _BaseStatusChangeResultList.Add(new StatusTool.BaseStatusChangeResult(p_GroupType, statusType, p_Params, addVal, deltaVal));
                            }
                        }
                    }
                    break;
                }
                case StatusTool.BaseStatusGroupType.SimpleMul:
                {
                    if (p_Preset.HasProperValue())
                    {
                        var mainWithAddStatus = _BaseStatusTable[StatusTool.BaseStatusGroupType.Main] + _BaseStatusTable[StatusTool.BaseStatusGroupType.ExtraAdd];
                        var simpleMulStatus = _BaseStatusTable[StatusTool.BaseStatusGroupType.SimpleMul];
                        var compoundMulStatus = _BaseStatusTable[StatusTool.BaseStatusGroupType.CompoundMul];
                        var totalStatus = _BaseStatusTable[StatusTool.BaseStatusGroupType.Total];
                        
                        var enumerator = BaseStatusTool.BaseStatusTypeEnumerator;
                        foreach (var statusType in enumerator)
                        {
                            if (p_Preset.HasProperValue(statusType))
                            {
                                var addVal = p_Preset[statusType];
                                var deltaVal = addVal * mainWithAddStatus[statusType] * compoundMulStatus[statusType];
                                simpleMulStatus.AddProperty(statusType, addVal);
                                totalStatus.AddProperty(statusType, deltaVal);
                                
                                _BaseStatusChangeResultList.Add(new StatusTool.BaseStatusChangeResult(p_GroupType, statusType, p_Params, addVal, deltaVal));
                            }
                        }
                    }
                    break;
                }
                case StatusTool.BaseStatusGroupType.CompoundMul:
                {
                    if (p_Preset.HasProperValue())
                    {
                        var mainWithAddStatus = _BaseStatusTable[StatusTool.BaseStatusGroupType.Main] + _BaseStatusTable[StatusTool.BaseStatusGroupType.ExtraAdd];
                        var simpleMulStatus = _BaseStatusTable[StatusTool.BaseStatusGroupType.SimpleMul];
                        var compoundMulStatus = _BaseStatusTable[StatusTool.BaseStatusGroupType.CompoundMul];
                        var totalStatus = _BaseStatusTable[StatusTool.BaseStatusGroupType.Total];

                        var enumerator = BaseStatusTool.BaseStatusTypeEnumerator;
                        foreach (var statusType in enumerator)
                        {
                            if (p_Preset.HasProperValue(statusType))
                            {
                                var rateVal = p_Preset[statusType];
                                var addVal = (rateVal - 1f) * compoundMulStatus[statusType];
                                var deltaVal = addVal * mainWithAddStatus[statusType] * simpleMulStatus[statusType];
                                compoundMulStatus.AddProperty(statusType, addVal);
                                totalStatus.AddProperty(statusType, deltaVal);
                                
                                _BaseStatusChangeResultList.Add(new StatusTool.BaseStatusChangeResult(p_GroupType, statusType, p_Params, addVal, deltaVal));
                            }
                        }
                    }
                    break;
                }
            }

            ReserveBaseStatusChange();
        }

        public void AddStatus(StatusTool.BaseStatusGroupType p_GroupType, BaseStatusTool.BaseStatusType p_StatusType, float p_Value, StatusTool.StatusChangeParams p_Params = default)
        {
            AddStatus(p_GroupType, new BaseStatusPreset(p_StatusType, p_Value), p_Params);
        }
        
        public void SetStatus(StatusTool.BaseStatusGroupType p_GroupType, BaseStatusTool.BaseStatusType p_StatusType, float p_Value, StatusTool.StatusChangeParams p_Params = default)
        {
            AddStatus(p_GroupType, p_StatusType, p_Value -  this[p_GroupType, p_StatusType], p_Params);
        }

        public void AddStatusRate(StatusTool.BaseStatusGroupType p_GroupType, BaseStatusTool.BaseStatusType p_StatusType, float p_Rate, StatusTool.StatusChangeParams p_Params = default)
        {
            AddStatus(p_GroupType, p_StatusType, this[StatusTool.BaseStatusGroupType.Total, p_StatusType, p_Rate], p_Params);
        }
        
        /// <summary>
        /// 기저 능력치를 초기화 시키는 메서드
        /// </summary>
        private void ResetBaseStatus()
        {
            var enumerator = EnumFlag.GetEnumEnumerator<StatusTool.BaseStatusGroupType>(EnumFlag.GetEnumeratorType.GetAll);
            foreach (var statusGroupType in enumerator)
            {
                switch (statusGroupType)
                {
                    case StatusTool.BaseStatusGroupType.Main:
                    {
                        _BaseStatusTable[StatusTool.BaseStatusGroupType.Main].SetPreset(BaseStatusTool.BasisBaseStatusPreset + ComponentDataRecord.BaseStatusPreset);
                        break;
                    }
                    case StatusTool.BaseStatusGroupType.ExtraAdd:
                    {
                        _BaseStatusTable[StatusTool.BaseStatusGroupType.ExtraAdd].SetPreset(0f);
                        break;
                    }
                    case StatusTool.BaseStatusGroupType.SimpleMul:
                    {
                        _BaseStatusTable[StatusTool.BaseStatusGroupType.SimpleMul].SetPreset(1f);
                        break;
                    }
                    case StatusTool.BaseStatusGroupType.CompoundMul:
                    {
                        _BaseStatusTable[StatusTool.BaseStatusGroupType.CompoundMul].SetPreset(1f);
                        break;
                    }
                    case StatusTool.BaseStatusGroupType.Total:
                    {
                        _BaseStatusTable[StatusTool.BaseStatusGroupType.Total]
                            .SetPreset
                            (
                                (_BaseStatusTable[StatusTool.BaseStatusGroupType.Main] + _BaseStatusTable[StatusTool.BaseStatusGroupType.ExtraAdd]) 
                                * _BaseStatusTable[StatusTool.BaseStatusGroupType.SimpleMul] 
                                * _BaseStatusTable[StatusTool.BaseStatusGroupType.CompoundMul]
                            );
                        break;
                    }
                }
            }
        }
        
        /// <summary>
        /// 기저 능력치를 초기화 시키는 메서드
        /// </summary>
        private void ResetBaseStatus(StatusTool.BaseStatusGroupType p_GroupType)
        {
            switch (p_GroupType)
            {
                case StatusTool.BaseStatusGroupType.Main:
                {
                    _BaseStatusTable[StatusTool.BaseStatusGroupType.Main].SetPreset(BaseStatusTool.BasisBaseStatusPreset + ComponentDataRecord.BaseStatusPreset);
                    goto case StatusTool.BaseStatusGroupType.Total;
                }
                case StatusTool.BaseStatusGroupType.ExtraAdd:
                {
                    _BaseStatusTable[StatusTool.BaseStatusGroupType.ExtraAdd].SetPreset(0f);
                    goto case StatusTool.BaseStatusGroupType.Total;
                }
                case StatusTool.BaseStatusGroupType.SimpleMul:
                {
                    _BaseStatusTable[StatusTool.BaseStatusGroupType.SimpleMul].SetPreset(1f);
                    goto case StatusTool.BaseStatusGroupType.Total;
                }
                case StatusTool.BaseStatusGroupType.CompoundMul:
                {
                    _BaseStatusTable[StatusTool.BaseStatusGroupType.CompoundMul].SetPreset(1f);
                    goto case StatusTool.BaseStatusGroupType.Total;
                }
                case StatusTool.BaseStatusGroupType.Total:
                {
                    _BaseStatusTable[StatusTool.BaseStatusGroupType.Total]
                        .SetPreset
                        (
                            (_BaseStatusTable[StatusTool.BaseStatusGroupType.Main] + _BaseStatusTable[StatusTool.BaseStatusGroupType.ExtraAdd]) 
                            * _BaseStatusTable[StatusTool.BaseStatusGroupType.SimpleMul] 
                            * _BaseStatusTable[StatusTool.BaseStatusGroupType.CompoundMul]
                        );
                    break;
                }
            }
        }

        #endregion
    }
}