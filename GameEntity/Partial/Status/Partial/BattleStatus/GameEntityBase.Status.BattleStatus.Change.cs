using System.Collections.Generic;
using k514.Mono.Feature;
using UnityEngine;

namespace k514.Mono.Common
{
    public partial class GameEntityBase<Content, CreateParams, ActivateParams>
    {
        #region <Fields>

        /// <summary>
        /// 값이 변한 전투 능력치 프리셋 리스트
        /// </summary>
        private List<StatusTool.BattleStatusChangeResult> _BattleStatusChangeResultList;
        
        /// <summary>
        /// 값이 변한 전투 능력치 프리셋 리스트 버퍼
        /// </summary>
        private List<StatusTool.BattleStatusChangeResult> _BattleStatusChangeResultListMirror;
        
        #endregion

        #region <Callbacks>

        private void OnCreateBattleStatusChanged()
        {
            _BattleStatusChangeResultList = new List<StatusTool.BattleStatusChangeResult>(GameConst.__CAPACITY_STATUS_CHANGE);
            _BattleStatusChangeResultListMirror = new List<StatusTool.BattleStatusChangeResult>(GameConst.__CAPACITY_STATUS_CHANGE);
        }
        
        private void OnBattleStatusChanged()
        {
            var battleStatusChangeFlagMask = BattleStatusTool.BattleStatusType.None;
            
            _BattleStatusChangeResultListMirror.AddRange(_BattleStatusChangeResultList);
            _BattleStatusChangeResultList.Clear();
            
            foreach (var battleStatusChangeResult in _BattleStatusChangeResultListMirror)
            {
                var param = battleStatusChangeResult.Params;
                var groupType = battleStatusChangeResult.GroupType;
                var statusType = battleStatusChangeResult.StatusType;

                if (battleStatusChangeResult.ValidFlag)
                {
                    battleStatusChangeFlagMask.AddFlag(statusType);
                }
                
                switch (groupType)
                {
                    case StatusTool.BattleStatusGroupType.Current:
                    {
                        switch (statusType)
                        {
                            case BattleStatusTool.BattleStatusType.HP_Base:
                            {
                                OnBattleStatusCurrentChanged(battleStatusChangeResult.DeltaValue, param);

                                var currentHP = _BattleStatusTable[StatusTool.BattleStatusGroupType.Current].GetProperty(BattleStatusTool.BattleStatusType.HP_Base);
                                if (currentHP < 0f)
                                {
                                    _BattleStatusTable[StatusTool.BattleStatusGroupType.Current].SetProperty(BattleStatusTool.BattleStatusType.HP_Base, 0f);
                                    OnMurdered(param);
                                }
                         
                                break;
                            }
                            case BattleStatusTool.BattleStatusType.MP_Base:
                            {
                                OnBattleStatusCurrentChanged(battleStatusChangeResult.DeltaValue, param);
                                
                                var currentMP = _BattleStatusTable[StatusTool.BattleStatusGroupType.Current].GetProperty(BattleStatusTool.BattleStatusType.MP_Base);
                                if (currentMP < 0f)
                                {
                                    _BattleStatusTable[StatusTool.BattleStatusGroupType.Current].SetProperty(BattleStatusTool.BattleStatusType.MP_Base, 0f);
                                }
                                
                                break;
                            }
                        }
                        break;
                    }
                }
            }
            
            GameEntityBaseEventSender.SendEvent(GameEntityTool.GameEntityBaseEventType.BattleStatus_Change, new GameEntityBaseEventParams(this, battleStatusChangeFlagMask, _BattleStatusChangeResultListMirror));
            _BattleStatusChangeResultListMirror.Clear();
        }

        private void OnBattleStatusCurrentChanged(float p_DeltaValue, StatusTool.StatusChangeParams p_Params)
        {
            var eventType = p_Params.EventType;
            switch (eventType)
            {
                case StatusTool.StatusChangeEventType.HealHP when p_DeltaValue >= 0f :
                {
                    var targetPosition = p_Params.HasAttribute(StatusTool.StatusChangeAttribute.HasTargetPosition)
                        ? p_Params.TargetPosition
                        : GetCenterPosition();
                    var randomizeRadius = p_Params.RandomizeRadius;
                    
                    UIxControlRoot.GetInstanceUnsafe?.NumberTheater?.PopTheaterElement
                    (
                        targetPosition, randomizeRadius, 
                        Color.green, p_DeltaValue, UIxNumberTheater.NumberEventType.None
                    );   
                    
                    var createParams = VfxPoolManager.GetInstanceUnsafe.GetCreateParams(VfxTool.__InstantHealHPVfxIndex);
                    var activateParams =
                        new VfxPoolManager.ActivateParams
                        (
                            null,
                            new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, new AffinePreset(GetCenterPosition(), Scale))
                        );
                    VfxPoolManager.GetInstanceUnsafe.Pop(createParams, activateParams);
               
                    SetMaterialColor(_HealHPFlashColor);
                    OnHeal(p_Params);
                    break;
                }
                case StatusTool.StatusChangeEventType.HealMP when p_DeltaValue >= 0f :
                {
                    var targetPosition = p_Params.HasAttribute(StatusTool.StatusChangeAttribute.HasTargetPosition)
                        ? p_Params.TargetPosition
                        : GetCenterPosition();
                    var randomizeRadius = p_Params.RandomizeRadius;
                    
                    UIxControlRoot.GetInstanceUnsafe?.NumberTheater?.PopTheaterElement
                    (
                        targetPosition, randomizeRadius, 
                        Color.cyan, p_DeltaValue, UIxNumberTheater.NumberEventType.None
                    );   
                    
                    var createParams = VfxPoolManager.GetInstanceUnsafe.GetCreateParams(VfxTool.__InstantHealMPVfxIndex);
                    var activateParams =
                        new VfxPoolManager.ActivateParams
                        (
                            null,
                            new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, new AffinePreset(GetCenterPosition(), Scale))
                        );
                    VfxPoolManager.GetInstanceUnsafe.Pop(createParams, activateParams);
              
                    SetMaterialColor(_HealMPFlashColor);
                    OnHeal(p_Params);
                    break;
                }
                case StatusTool.StatusChangeEventType.Combat when p_DeltaValue <= 0f :
                {
                    var targetPosition = p_Params.HasAttribute(StatusTool.StatusChangeAttribute.HasTargetPosition)
                        ? p_Params.TargetPosition
                        : GetCenterPosition();
                    var randomizeRadius = p_Params.RandomizeRadius;
                    var critical = p_Params.HasAttribute(StatusTool.StatusChangeAttribute.Critical);
                    
                    UIxControlRoot.GetInstanceUnsafe?.NumberTheater?.PopTheaterElement
                    (
                        targetPosition, randomizeRadius, 
                        critical? new Color(255,0,255) : Color.red, p_DeltaValue, critical ? UIxNumberTheater.NumberEventType.Critical : UIxNumberTheater.NumberEventType.None
                        
                    );   

                    SetMaterialColor(_CombatFlashColor);
                    OnHit(p_Params);
                    break;
                }
                case StatusTool.StatusChangeEventType.Shocking when p_DeltaValue <= 0f :
                {
                    var targetPosition = p_Params.HasAttribute(StatusTool.StatusChangeAttribute.HasTargetPosition)
                        ? p_Params.TargetPosition
                        : GetCenterPosition();
                    var randomizeRadius = p_Params.RandomizeRadius;
                    
                    UIxControlRoot.GetInstanceUnsafe?.NumberTheater?.PopTheaterElement
                    (
                        targetPosition, randomizeRadius, 
                        new Color(0.6f, 0.6f, 0.1f), p_DeltaValue, UIxNumberTheater.NumberEventType.None
                    );    

                    var createParams = VfxPoolManager.GetInstanceUnsafe.GetCreateParams(VfxTool.__ShockVfxIndex);
                    var activateParams =
                        new VfxPoolManager.ActivateParams(
                            null,
                            new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, new AffinePreset(GetBottomPosition(), Scale))
                        );
                    VfxPoolManager.GetInstanceUnsafe.Pop(createParams, activateParams);
                 
                    SetMaterialColor(_ShockFlashColor);
                    OnHit(p_Params);
                    break;
                }
                case StatusTool.StatusChangeEventType.Bleeding when p_DeltaValue <= 0f :
                {
                    var targetPosition = p_Params.HasAttribute(StatusTool.StatusChangeAttribute.HasTargetPosition)
                        ? p_Params.TargetPosition
                        : GetCenterPosition();
                    var randomizeRadius = p_Params.RandomizeRadius;
                    
                    UIxControlRoot.GetInstanceUnsafe?.NumberTheater?.PopTheaterElement
                    (
                        targetPosition, randomizeRadius, 
                        new Color(0.6f, 0f, 0.1f), p_DeltaValue, UIxNumberTheater.NumberEventType.None
                    );    

                    var createParams = VfxPoolManager.GetInstanceUnsafe.GetCreateParams(VfxTool.__BleedingVfxIndex);
                    var activateParams =
                        new VfxPoolManager.ActivateParams(
                            null,
                            new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, new AffinePreset(GetBottomPosition(), Scale))
                        );
                    VfxPoolManager.GetInstanceUnsafe.Pop(createParams, activateParams);
                 
                    SetMaterialColor(_BleedFlashColor);
                    OnHit(p_Params);
                    break;
                }
                case StatusTool.StatusChangeEventType.Poisoning when p_DeltaValue <= 0f :
                {
                    var targetPosition = p_Params.HasAttribute(StatusTool.StatusChangeAttribute.HasTargetPosition)
                        ? p_Params.TargetPosition
                        : GetCenterPosition();
                    var randomizeRadius = p_Params.RandomizeRadius;
                    
                    UIxControlRoot.GetInstanceUnsafe?.NumberTheater?.PopTheaterElement
                    (
                        targetPosition, randomizeRadius, 
                        new Color(0.6f, 0f, 1f), p_DeltaValue, UIxNumberTheater.NumberEventType.None
                    );    

                    var createParams = VfxPoolManager.GetInstanceUnsafe.GetCreateParams(VfxTool.__PoisoningVfxIndex);
                    var activateParams =
                        new VfxPoolManager.ActivateParams
                        (
                            null,
                            new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, new AffinePreset(GetTopUpPosition(0.5f), Scale))
                        );
                    VfxPoolManager.GetInstanceUnsafe.Pop(createParams, activateParams);
               
                    SetMaterialColor(_PoisonFlashColor);
                    OnHit(p_Params);
                    break;
                }
                case StatusTool.StatusChangeEventType.Burning when p_DeltaValue <= 0f :
                {
                    var targetPosition = p_Params.HasAttribute(StatusTool.StatusChangeAttribute.HasTargetPosition)
                        ? p_Params.TargetPosition
                        : GetCenterPosition();
                    var randomizeRadius = p_Params.RandomizeRadius;
                    
                    UIxControlRoot.GetInstanceUnsafe?.NumberTheater?.PopTheaterElement
                    (
                        targetPosition, randomizeRadius, 
                        new Color(0.8f, 0.3f, 0.1f), p_DeltaValue, UIxNumberTheater.NumberEventType.None
                    );    

                    var createParams = VfxPoolManager.GetInstanceUnsafe.GetCreateParams(VfxTool.__BurnVfxIndex);
                    var activateParams =
                        new VfxPoolManager.ActivateParams(
                            null,
                            new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, new AffinePreset(GetCenterPosition(), Scale))
                        );
                    VfxPoolManager.GetInstanceUnsafe.Pop(createParams, activateParams);
                 
                    SetMaterialColor(_BurnFlashColor);
                    OnHit(p_Params);
                    break;
                }
                case StatusTool.StatusChangeEventType.Chilling when p_DeltaValue <= 0f :
                {
                    var targetPosition = p_Params.HasAttribute(StatusTool.StatusChangeAttribute.HasTargetPosition)
                        ? p_Params.TargetPosition
                        : GetCenterPosition();
                    var randomizeRadius = p_Params.RandomizeRadius;
                    
                    UIxControlRoot.GetInstanceUnsafe?.NumberTheater?.PopTheaterElement
                    (
                        targetPosition, randomizeRadius, 
                        new Color(0f, 0f, 0.5f), p_DeltaValue, UIxNumberTheater.NumberEventType.None
                    );    

                    var createParams = VfxPoolManager.GetInstanceUnsafe.GetCreateParams(VfxTool.__ChilVfxIndex);
                    var activateParams =
                        new VfxPoolManager.ActivateParams
                        (
                            null,
                            new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, new AffinePreset(GetTopPosition(), Scale))
                        );
                    VfxPoolManager.GetInstanceUnsafe.Pop(createParams, activateParams);
              
                    SetMaterialColor(_ChillFlashColor);
                    OnHit(p_Params);
                    break;
                }
            }
        }
        
        #endregion
    }
}