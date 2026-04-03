using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

namespace k514.Mono.Common
{
    public partial class GameEntityBase<Content, CreateParams, ActivateParams>
    {
        #region <Fields>

        /// <summary>
        /// 전투능력치 테이블
        /// </summary>
        private Dictionary<StatusTool.BattleStatusGroupType, BattleStatusPresetWrapper> _BattleStatusTable;

        /// <summary>
        /// 시야
        /// </summary>
        public float SightRange => _BattleStatusTable[StatusTool.BattleStatusGroupType.Total][BattleStatusTool.BattleStatusType3.SightRange];
    
        /// <summary>
        /// 시야 제곱
        /// </summary>
        public float SqrSightRange => _BattleStatusTable[StatusTool.BattleStatusGroupType.TotalSqr][BattleStatusTool.BattleStatusType3.SightRange];
   
        #endregion

        #region <Indexer>

        public BattleStatusPreset this[StatusTool.BattleStatusGroupType p_Group] => _BattleStatusTable[p_Group];
        public float this[StatusTool.BattleStatusGroupType p_Group, BattleStatusTool.BattleStatusType p_Type] => _BattleStatusTable[p_Group][p_Type];
        public float this[StatusTool.BattleStatusGroupType p_Group, BattleStatusTool.BattleStatusType1 p_Type] => _BattleStatusTable[p_Group][p_Type];
        public float this[StatusTool.BattleStatusGroupType p_Group, BattleStatusTool.BattleStatusType2 p_Type] => _BattleStatusTable[p_Group][p_Type];
        public float this[StatusTool.BattleStatusGroupType p_Group, BattleStatusTool.BattleStatusType3 p_Type] => _BattleStatusTable[p_Group][p_Type];
        public float this[StatusTool.BattleStatusGroupType p_Group, BattleStatusTool.BattleStatusType p_Type, float p_Rate] => _BattleStatusTable[p_Group][p_Type, p_Rate];
        public float this[StatusTool.BattleStatusGroupType p_Group, BattleStatusTool.BattleStatusType1 p_Type, float p_Rate] => _BattleStatusTable[p_Group][p_Type, p_Rate];
        public float this[StatusTool.BattleStatusGroupType p_Group, BattleStatusTool.BattleStatusType2 p_Type, float p_Rate] => _BattleStatusTable[p_Group][p_Type, p_Rate];
        public float this[StatusTool.BattleStatusGroupType p_Group, BattleStatusTool.BattleStatusType3 p_Type, float p_Rate] => _BattleStatusTable[p_Group][p_Type, p_Rate];

        #endregion

        #region <Callbacks>

        private void OnCreateBattleStatus()
        {
            _BattleStatusTable = new Dictionary<StatusTool.BattleStatusGroupType, BattleStatusPresetWrapper>();
            
            var enumerator = EnumFlag.GetEnumEnumerator<StatusTool.BattleStatusGroupType>(EnumFlag.GetEnumeratorType.GetAll);
            foreach (var battleStatusGroup in enumerator)
            {
                switch (battleStatusGroup)
                {
                    case StatusTool.BattleStatusGroupType.SimpleMul:
                    case StatusTool.BattleStatusGroupType.CompoundMul:
                        _BattleStatusTable.Add(battleStatusGroup, new BattleStatusPresetWrapper(BattleStatusTool.EinBattleStatusPreset));
                        break;
                    case StatusTool.BattleStatusGroupType.ExtraAdd:
                    case StatusTool.BattleStatusGroupType.Main:
                    case StatusTool.BattleStatusGroupType.Total:
                    case StatusTool.BattleStatusGroupType.Current:
                        _BattleStatusTable.Add(battleStatusGroup, new BattleStatusPresetWrapper(BattleStatusTool.BasisBattleStatusPreset));
                        break;
                    case StatusTool.BattleStatusGroupType.TotalInverse:
                        _BattleStatusTable.Add(battleStatusGroup, new BattleStatusPresetWrapper(BattleStatusTool.BasisInvBattleStatusPreset));
                        break;
                    case StatusTool.BattleStatusGroupType.TotalSqr:
                        _BattleStatusTable.Add(battleStatusGroup, new BattleStatusPresetWrapper(BattleStatusTool.BasisSqrBattleStatusPreset));
                        break;
                }
            }

            OnCreateBattleStatusChanged();
            OnCreateBattleStatusCurrent();
            OnCreateBattleStatusRegen();
        }

        private void OnActivateBattleStatus()
        {
#if LOADING_OPTIMIZATION
             /* if(UnityMainThreadDispatcher.Exists())
              {
                  UniTask.RunOnThreadPool(() =>
                  {
                      ResetBattleStatus();

                      UnityMainThreadDispatcher.Instance().Enqueue(() => { OnActivateBattleStatusRegen(); });

                  });
              }
              else
              {
                  ResetBattleStatus();
                  OnActivateBattleStatusRegen();
              }*/
            ResetBattleStatus();
            OnActivateBattleStatusRegen();


#else
            ResetBattleStatus();
            OnActivateBattleStatusRegen();
#endif
        }

        private void OnRetrieveBattleStatus()
        {
        }

#endregion

        #region <Methods>

        /// <summary>
        /// 지정한 능력치 그룹에 전투 능력치 값을 더하는 메서드
        ///
        /// 그룹 중에서는 Main, ExtraAdd/Mul1/Mul2/Current 값만 제어가 가능하며
        /// Total 계열 그룹은 나머지 그룹에 의해 갱신된다.
        /// </summary>
        public void AddStatus(StatusTool.BattleStatusGroupType p_GroupType, BattleStatusPreset p_Preset, StatusTool.StatusChangeParams p_Params = default)
        {
            switch (p_GroupType)
            {
                case StatusTool.BattleStatusGroupType.ExtraAdd:
                {
                    if (p_Preset.HasProperValue())
                    {
                        var extraAddStatus = _BattleStatusTable[StatusTool.BattleStatusGroupType.ExtraAdd];
                        var simpleMulStatus = _BattleStatusTable[StatusTool.BattleStatusGroupType.SimpleMul];
                        var compoundMulStatus = _BattleStatusTable[StatusTool.BattleStatusGroupType.CompoundMul];
                        var totalStatus = _BattleStatusTable[StatusTool.BattleStatusGroupType.Total];
                        var totalInvStatus = _BattleStatusTable[StatusTool.BattleStatusGroupType.TotalInverse];
                        var totalSqrStatus = _BattleStatusTable[StatusTool.BattleStatusGroupType.TotalSqr];
                        var currentStatus = _BattleStatusTable[StatusTool.BattleStatusGroupType.Current];

                        var enumerator = BattleStatusTool.BattleStatusTypeEnumerator;
                        foreach (var statusType in enumerator)
                        {
                            if (p_Preset.HasProperValue(statusType))
                            {
                                var addVal = p_Preset[statusType];
                                var deltaVal = addVal * simpleMulStatus[statusType] * compoundMulStatus[statusType];
                                extraAddStatus.AddProperty(statusType, addVal);
                                totalStatus.AddProperty(statusType, deltaVal);
                              
                                var totalVal = totalStatus[statusType];
                                totalInvStatus.SetProperty(statusType, 1f / totalVal);
                                totalSqrStatus.SetProperty(statusType, totalVal * totalVal);
                                
                                _BattleStatusChangeResultList.Add(new StatusTool.BattleStatusChangeResult(p_GroupType, statusType, p_Params, addVal, deltaVal));

                                if (_CurrentStatusTypeFlagMask.HasFlag(statusType))
                                {
                                    if (p_Params.HasAttribute(StatusTool.StatusChangeAttribute.ApplyCurrent))
                                    {
                                        var tryCurrentVal = currentStatus[statusType] + deltaVal;
                                        var correctedCurrentVal = Mathf.Min(tryCurrentVal, totalVal);
                                        var currentDeltaVal = correctedCurrentVal - currentStatus[statusType];
                                        
                                        currentStatus.SetProperty(statusType, correctedCurrentVal);
                                        
                                        _BattleStatusChangeResultList.Add(new StatusTool.BattleStatusChangeResult(StatusTool.BattleStatusGroupType.Current, statusType, p_Params, deltaVal, currentDeltaVal));
                                    }
                                    else
                                    {
                                        var currentVal = currentStatus[statusType];
                                        var currentDeltaVal = totalVal - currentVal;

                                        if (currentDeltaVal < 0f)
                                        {
                                            currentStatus.SetProperty(statusType, totalVal);
                                            
                                            _BattleStatusChangeResultList.Add(new StatusTool.BattleStatusChangeResult(StatusTool.BattleStatusGroupType.Current, statusType, p_Params, deltaVal, currentDeltaVal));
                                        }
                                    }
                                }
                            }
                        }
                    }
                    break;
                }
                case StatusTool.BattleStatusGroupType.SimpleMul:
                {
                    if (p_Preset.HasProperValue())
                    {
                        var mainWithAddStatus = _BattleStatusTable[StatusTool.BattleStatusGroupType.Main] + _BattleStatusTable[StatusTool.BattleStatusGroupType.ExtraAdd];
                        var simpleMulStatus = _BattleStatusTable[StatusTool.BattleStatusGroupType.SimpleMul];
                        var compoundMulStatus = _BattleStatusTable[StatusTool.BattleStatusGroupType.CompoundMul];
                        var totalStatus = _BattleStatusTable[StatusTool.BattleStatusGroupType.Total];
                        var totalInvStatus = _BattleStatusTable[StatusTool.BattleStatusGroupType.TotalInverse];
                        var totalSqrStatus = _BattleStatusTable[StatusTool.BattleStatusGroupType.TotalSqr];
                        var currentStatus = _BattleStatusTable[StatusTool.BattleStatusGroupType.Current];
                        
                        var enumerator = BattleStatusTool.BattleStatusTypeEnumerator;
                        foreach (var statusType in enumerator)
                        {
                            if (p_Preset.HasProperValue(statusType))
                            {
                                var addVal = p_Preset[statusType];
                                var deltaVal = addVal * mainWithAddStatus[statusType] * compoundMulStatus[statusType];
                                simpleMulStatus.AddProperty(statusType, addVal);
                                totalStatus.AddProperty(statusType, deltaVal);
                            
                                var totalVal = totalStatus[statusType];
                                totalInvStatus.SetProperty(statusType, 1f / totalVal);
                                totalSqrStatus.SetProperty(statusType, totalVal * totalVal);
                                
                                _BattleStatusChangeResultList.Add(new StatusTool.BattleStatusChangeResult(p_GroupType, statusType, p_Params, addVal, deltaVal));

                                if (_CurrentStatusTypeFlagMask.HasFlag(statusType))
                                {
                                    if (p_Params.HasAttribute(StatusTool.StatusChangeAttribute.ApplyCurrent))
                                    {
                                        var tryCurrentVal = currentStatus[statusType] + deltaVal;
                                        var correctedVal = Mathf.Min(tryCurrentVal, totalVal);
                                        var curDeltaVal = correctedVal - currentStatus[statusType];
                                        
                                        currentStatus.SetProperty(statusType, correctedVal);
                                     
                                        _BattleStatusChangeResultList.Add(new StatusTool.BattleStatusChangeResult(StatusTool.BattleStatusGroupType.Current, statusType, p_Params, deltaVal, curDeltaVal));
                                    }
                                    else
                                    {
                                        var currentVal = currentStatus[statusType];
                                        var currentDeltaVal = totalVal - currentVal;

                                        if (currentDeltaVal < 0f)
                                        {
                                            currentStatus.SetProperty(statusType, totalVal);
                                            
                                            _BattleStatusChangeResultList.Add(new StatusTool.BattleStatusChangeResult(StatusTool.BattleStatusGroupType.Current, statusType, p_Params, deltaVal, currentDeltaVal));
                                        }
                                    }
                                }
                            }
                        }
                    }
                    break;
                }
                case StatusTool.BattleStatusGroupType.CompoundMul:
                {
                    if (p_Preset.HasProperValue())
                    {
                        var mainWithAddStatus = _BattleStatusTable[StatusTool.BattleStatusGroupType.Main] + _BattleStatusTable[StatusTool.BattleStatusGroupType.ExtraAdd];
                        var simpleMulStatus = _BattleStatusTable[StatusTool.BattleStatusGroupType.SimpleMul];
                        var compoundMulStatus = _BattleStatusTable[StatusTool.BattleStatusGroupType.CompoundMul];
                        var totalStatus = _BattleStatusTable[StatusTool.BattleStatusGroupType.Total];
                        var totalInvStatus = _BattleStatusTable[StatusTool.BattleStatusGroupType.TotalInverse];
                        var totalSqrStatus = _BattleStatusTable[StatusTool.BattleStatusGroupType.TotalSqr];
                        var currentStatus = _BattleStatusTable[StatusTool.BattleStatusGroupType.Current];

                        var enumerator = BattleStatusTool.BattleStatusTypeEnumerator;
                        foreach (var statusType in enumerator)
                        {
                            if (p_Preset.HasProperValue(statusType))
                            {
                                var rateVal = p_Preset[statusType];
                                var addVal = (rateVal - 1f) * compoundMulStatus[statusType];
                                var deltaVal = addVal * mainWithAddStatus[statusType] * simpleMulStatus[statusType];
                                compoundMulStatus.AddProperty(statusType, addVal);
                                totalStatus.AddProperty(statusType, deltaVal);
                            
                                var totalVal = totalStatus[statusType];
                                totalInvStatus.SetProperty(statusType, 1f / totalVal);
                                totalSqrStatus.SetProperty(statusType, totalVal * totalVal);
                                
                                _BattleStatusChangeResultList.Add(new StatusTool.BattleStatusChangeResult(p_GroupType, statusType, p_Params, addVal, deltaVal));
                                    
                                if (_CurrentStatusTypeFlagMask.HasFlag(statusType))
                                {
                                    if (p_Params.HasAttribute(StatusTool.StatusChangeAttribute.ApplyCurrent))
                                    {
                                        var tryCurrentVal = currentStatus[statusType] + deltaVal;
                                        var correctedVal = Mathf.Min(tryCurrentVal, totalVal);
                                        var curDeltaVal = correctedVal - currentStatus[statusType];
                                        
                                        currentStatus.SetProperty(statusType, correctedVal);
                                        _BattleStatusChangeResultList.Add(new StatusTool.BattleStatusChangeResult(StatusTool.BattleStatusGroupType.Current, statusType, p_Params, deltaVal, curDeltaVal));
                                    }
                                    else
                                    {
                                        var currentVal = currentStatus[statusType];
                                        var currentDeltaVal = totalVal - currentVal;

                                        if (currentDeltaVal < 0f)
                                        {
                                            currentStatus.SetProperty(statusType, totalVal);
                                            
                                            _BattleStatusChangeResultList.Add(new StatusTool.BattleStatusChangeResult(StatusTool.BattleStatusGroupType.Current, statusType, p_Params, deltaVal, currentDeltaVal));
                                        }
                                    }
                                }
                            }
                        }
                    }
                    break;
                }
                case StatusTool.BattleStatusGroupType.Current:
                {
                    if (p_Preset.HasProperValue())
                    {
                        var flagMask = p_Preset.FlagMask;
                        var currentStatus = _BattleStatusTable[StatusTool.BattleStatusGroupType.Current];
                        var totalStatus = _BattleStatusTable[StatusTool.BattleStatusGroupType.Total];

                        var enumerator = BattleStatusTool.BattleStatusTypeEnumerator;
                        foreach (var statusType in enumerator)
                        {
                            if (_CurrentStatusTypeFlagMask.HasFlag(statusType))
                            {
                                if (flagMask.HasFlag(statusType))
                                {
                                    var addVal = p_Preset[statusType];
                                    var tryCurrentVal = currentStatus[statusType] + addVal;
                                    var correctedVal = Mathf.Min(tryCurrentVal, totalStatus[statusType]);
                                    var deltaVal = correctedVal - currentStatus[statusType];
                                    currentStatus.SetProperty(statusType, correctedVal);
                                    
                                    _BattleStatusChangeResultList.Add(new StatusTool.BattleStatusChangeResult(p_GroupType, statusType, p_Params, addVal, deltaVal));
                                }
                            }
                        }
                    }
                    break;
                }
            }

            ReserveBattleStatusChange();
        }

        public void AddStatus(StatusTool.BattleStatusGroupType p_GroupType, BattleStatusTool.BattleStatusType p_StatusType, float p_Value, StatusTool.StatusChangeParams p_Params = default)
        {
            AddStatus(p_GroupType, new BattleStatusPreset(p_StatusType, p_Value), p_Params);
        }

        public void SetStatus(StatusTool.BattleStatusGroupType p_GroupType, BattleStatusTool.BattleStatusType p_StatusType, float p_Value, StatusTool.StatusChangeParams p_Params = default)
        {
            AddStatus(p_GroupType, p_StatusType, p_Value -  this[p_GroupType, p_StatusType], p_Params);
        }
        
        public void AddStatusRate(StatusTool.BattleStatusGroupType p_GroupType, BattleStatusTool.BattleStatusType p_StatusType, float p_Rate, StatusTool.StatusChangeParams p_Params = default)
        {
            AddStatus(p_GroupType, p_StatusType, this[StatusTool.BattleStatusGroupType.Total, p_StatusType, p_Rate], p_Params);
        }
        
        /// <summary>
        /// 전투 능력치를 초기화 시키는 메서드
        /// </summary>
        private void ResetBattleStatus()
        {
            var enumerator = EnumFlag.GetEnumEnumerator<StatusTool.BattleStatusGroupType>(EnumFlag.GetEnumeratorType.GetAll);
            foreach (var statusGroupType in enumerator)
            {
                switch (statusGroupType)
                {
                    case StatusTool.BattleStatusGroupType.Main:
                    {
                        _BattleStatusTable[StatusTool.BattleStatusGroupType.Main].SetPreset(BattleStatusTool.BasisBattleStatusPreset + ComponentDataRecord.BattleStatusPreset);
                        break;
                    }
                    case StatusTool.BattleStatusGroupType.ExtraAdd:
                    {
                        _BattleStatusTable[StatusTool.BattleStatusGroupType.ExtraAdd].SetPreset(0f);
                        break;
                    }
                    case StatusTool.BattleStatusGroupType.SimpleMul:
                    {
                        _BattleStatusTable[StatusTool.BattleStatusGroupType.SimpleMul].SetPreset(1f);
                        break;
                    }
                    case StatusTool.BattleStatusGroupType.CompoundMul:
                    {
                        _BattleStatusTable[StatusTool.BattleStatusGroupType.CompoundMul].SetPreset(1f);
                        break;
                    }
                    case StatusTool.BattleStatusGroupType.Total:
                    {
                        _BattleStatusTable[StatusTool.BattleStatusGroupType.Total]
                            .SetPreset
                            (
                                (_BattleStatusTable[StatusTool.BattleStatusGroupType.Main] + _BattleStatusTable[StatusTool.BattleStatusGroupType.ExtraAdd])
                                * _BattleStatusTable[StatusTool.BattleStatusGroupType.SimpleMul]
                                * _BattleStatusTable[StatusTool.BattleStatusGroupType.CompoundMul]    
                            );
                        break;
                    }
                    case StatusTool.BattleStatusGroupType.TotalInverse:
                    {
                        _BattleStatusTable[StatusTool.BattleStatusGroupType.TotalInverse].SetPreset(1f / _BattleStatusTable[StatusTool.BattleStatusGroupType.Total]);
                        break;
                    }
                    case StatusTool.BattleStatusGroupType.TotalSqr:
                    {
                        _BattleStatusTable[StatusTool.BattleStatusGroupType.TotalSqr].SetPreset(_BattleStatusTable[StatusTool.BattleStatusGroupType.Total] * _BattleStatusTable[StatusTool.BattleStatusGroupType.Total]);
                        break;
                    }
                    case StatusTool.BattleStatusGroupType.Current:
                    {
                        _BattleStatusTable[StatusTool.BattleStatusGroupType.Current].SetPreset(BattleStatusTool.BasisBattleStatusPreset);
                        foreach (var battleStatusType in _CurrentStatusTypeEnumerator)
                        {
                            _BattleStatusTable[StatusTool.BattleStatusGroupType.Current].SetProperty(battleStatusType, _BattleStatusTable[StatusTool.BattleStatusGroupType.Total][battleStatusType]);
                        }
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 전투 능력치를 초기화 시키는 메서드
        /// </summary>
        private void ResetBattleStatus(StatusTool.BattleStatusGroupType p_GroupType)
        {
            switch (p_GroupType)
            {
                case StatusTool.BattleStatusGroupType.Main:
                {
                    _BattleStatusTable[StatusTool.BattleStatusGroupType.Main].SetPreset(BattleStatusTool.BasisBattleStatusPreset + ComponentDataRecord.BattleStatusPreset);
                    goto case StatusTool.BattleStatusGroupType.Total;
                }
                case StatusTool.BattleStatusGroupType.ExtraAdd:
                {
                    _BattleStatusTable[StatusTool.BattleStatusGroupType.ExtraAdd].SetPreset(0f);
                    goto case StatusTool.BattleStatusGroupType.Total;
                }
                case StatusTool.BattleStatusGroupType.SimpleMul:
                {
                    _BattleStatusTable[StatusTool.BattleStatusGroupType.SimpleMul].SetPreset(1f);
                    goto case StatusTool.BattleStatusGroupType.Total;
                }
                case StatusTool.BattleStatusGroupType.CompoundMul:
                {
                    _BattleStatusTable[StatusTool.BattleStatusGroupType.CompoundMul].SetPreset(1f);
                    goto case StatusTool.BattleStatusGroupType.Total;
                }
                case StatusTool.BattleStatusGroupType.Total:
                {
                    _BattleStatusTable[StatusTool.BattleStatusGroupType.Total]
                        .SetPreset
                        (
                            (_BattleStatusTable[StatusTool.BattleStatusGroupType.Main] + _BattleStatusTable[StatusTool.BattleStatusGroupType.ExtraAdd])
                            * _BattleStatusTable[StatusTool.BattleStatusGroupType.SimpleMul]
                            * _BattleStatusTable[StatusTool.BattleStatusGroupType.CompoundMul]
                        );
                    goto case StatusTool.BattleStatusGroupType.TotalInverse;
                }
                case StatusTool.BattleStatusGroupType.TotalInverse:
                {
                    _BattleStatusTable[StatusTool.BattleStatusGroupType.TotalInverse].SetPreset(1f / _BattleStatusTable[StatusTool.BattleStatusGroupType.Total]);
                    goto case StatusTool.BattleStatusGroupType.TotalSqr;
                }
                case StatusTool.BattleStatusGroupType.TotalSqr:
                {
                    _BattleStatusTable[StatusTool.BattleStatusGroupType.TotalSqr].SetPreset(_BattleStatusTable[StatusTool.BattleStatusGroupType.Total] * _BattleStatusTable[StatusTool.BattleStatusGroupType.Total]);
                    goto case StatusTool.BattleStatusGroupType.Current;
                }
                case StatusTool.BattleStatusGroupType.Current:
                {
                    _BattleStatusTable[StatusTool.BattleStatusGroupType.Current].SetPreset(BattleStatusTool.BasisBattleStatusPreset);
                    foreach (var battleStatusType in _CurrentStatusTypeEnumerator)
                    {
                        _BattleStatusTable[StatusTool.BattleStatusGroupType.Current].SetProperty(battleStatusType, _BattleStatusTable[StatusTool.BattleStatusGroupType.Total][battleStatusType]);
                    }
                    break;
                }
            }
        }

        public float GetCurrentStatusRate(BattleStatusTool.BattleStatusType p_Type)
        {
            if (_CurrentStatusTypeFlagMask.HasFlag(p_Type))
            {
                return this[StatusTool.BattleStatusGroupType.Current, p_Type] * this[StatusTool.BattleStatusGroupType.TotalInverse, p_Type];
            }
            else
            {
                return 1f;
            }
        }

        #endregion
    }
}