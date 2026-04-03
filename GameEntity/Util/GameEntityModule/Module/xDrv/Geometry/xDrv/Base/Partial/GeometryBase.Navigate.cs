using System;
using UnityEngine;

namespace k514.Mono.Common
{
    public partial class GeometryBase
    {
        #region <Fields>

        /// <summary>
        /// 현재 목적지
        /// </summary>
        protected GeometryTool.NavigateDestinationPreset _CurrentDestination;

        /// <summary>
        /// 현재 목적지 타입
        /// </summary>
        protected GeometryTool.NavigateDestinationType _CurrentDestinationType => _CurrentDestination.DestinationType;
        
        /// <summary>
        /// 길찾기 페이즈
        /// </summary>
        private GeometryTool.NavigatePhase _Phase;
        
        /// <summary>
        /// 길찾기 시작스탬프
        /// </summary>
        private float _NavigateStartTimeStamp;

        #endregion
        
        #region <Callbacks>

        private void OnCreateNavigate()
        {
        }
        
        private void OnAwakeNavigate()
        {
        }

        private void OnSleepNavigate()
        {
            _CurrentDestination = default;
            _Phase = GeometryTool.NavigatePhase.None;
        }

        protected virtual void OnDestinationUpdate()
        {
            if (_CurrentDestination.OnSelected(Entity))
            {
                _Phase = GeometryTool.NavigatePhase.WaitSqrDistanceUpdate;
            }
            else
            {
                _Phase = GeometryTool.NavigatePhase.Ready;
            }
        }

        private void OnUpdateNavigate(float p_DeltaTime)
        {
            switch (_Phase)
            {
                default:
                case GeometryTool.NavigatePhase.None:
                    break;
                case GeometryTool.NavigatePhase.WaitSqrDistanceUpdate:
                {
                    _Phase = GeometryTool.NavigatePhase.Ready;
                    break;
                }
                case GeometryTool.NavigatePhase.Ready:
                {
                    _NavigateStartTimeStamp = Time.time;
                    var navigationState = GetNavigationState();
                    var navigationResult = _CurrentDestination.CalcNavigate(Entity, navigationState, _NavigateStartTimeStamp, p_DeltaTime);
                    
                    if (navigationResult.IsOver)
                    {
                        _OnNavigateEnd(navigationResult);
                    }
                    else
                    {
                        if (navigationResult.IsMoving)
                        {
                            _OnNavigateStart(navigationResult);
                            _Phase = GeometryTool.NavigatePhase.Processing;
                        }
                        else
                        {
                            _OnNavigateEnd(navigationResult);
                        }
                    }
                    break;
                }
                case GeometryTool.NavigatePhase.Processing:
                {
                    var navigationState = GetNavigationState();
                    var navigationResult = _CurrentDestination.CalcNavigate(Entity, navigationState, _NavigateStartTimeStamp, p_DeltaTime);
                    
                    if (navigationResult.IsOver)
                    {
                        _OnNavigateEnd(navigationResult);
                    }
                    else
                    {
                        if (navigationResult.IsMoving)
                        {
                            _OnNavigateProgress(navigationResult);
                        }
                        else
                        {
                            _OnNavigateEnd(navigationResult);
                        }
                    }
                    break;
                }
            }
        }
        
        protected virtual void _OnNavigateStart(GeometryTool.NavigationResultPreset p_Preset)
        {
            Entity.OnNavigateStart(p_Preset);
        }
        
        protected virtual void _OnNavigateProgress(GeometryTool.NavigationResultPreset p_Preset)
        {
            Entity.OnNavigateProgress(p_Preset);
        }
        
        protected virtual void _OnNavigateEnd(GeometryTool.NavigationResultPreset p_Preset)
        {
            _Phase = GeometryTool.NavigatePhase.None;
            _CurrentDestination = default;
            Entity.OnNavigateEnd(p_Preset);
        }

        #endregion
        
        #region <Methods>

        public bool IsOnNavigate()
        {
            return _CurrentDestination.ValidFlag;
        }
              
        public (bool, GeometryTool.NavigateDestinationPreset) TryGetDestination()
        {
            return (_CurrentDestination.ValidFlag, _CurrentDestination);
        }
        
        protected virtual bool IsEnterable(GeometryTool.NavigateDestinationPreset p_Preset)
        {
            if (p_Preset.ValidFlag)
            {
                if (_CurrentDestination.ValidFlag)
                {
                    if (_CurrentDestination != p_Preset)
                    {
                        return true;
                    }
                }
                else
                {
                    return true;
                }
            }
            
            return false;
        }

        public bool NavigateTo(GeometryTool.NavigateDestinationPreset p_Preset)
        {
            if (IsEnterable(p_Preset))
            {
                _CurrentDestination = p_Preset;
                
                OnDestinationUpdate();
                
                return true;
            }
            else
            {
                return false;
            }
        }

        protected abstract GeometryTool.NavigationStatePreset GetNavigationState();

        public void StopNavigate()
        {
            switch (_Phase)
            {
                case GeometryTool.NavigatePhase.Ready:
                case GeometryTool.NavigatePhase.Processing:
                {
                    var cancelResult = new GeometryTool.NavigationResultPreset(GeometryTool.NavigateResultType.Canceled, _NavigateStartTimeStamp, 0f, _CurrentDestination.LatestProgress);
                    _OnNavigateEnd(cancelResult);
                    break;
                }
            }
        }

        #endregion
    }
}