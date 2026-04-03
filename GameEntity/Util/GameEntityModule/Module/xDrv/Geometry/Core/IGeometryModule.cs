using k514.Mono.Feature;
using UnityEngine;
using UnityEngine.AI;

namespace k514.Mono.Common
{
    public interface IGeometryModule : IGameEntityModule
    {
        /* Default */
        GeometryModuleDataTableQuery.TableLabel GetGeometryModuleType();
        
        /* PathFind */
        /// <summary>
        /// 현재 길찾기 이동 중인지 검증히는 메서드
        /// </summary>
        bool IsOnNavigate();

        /// <summary>
        /// 지정한 목적지로 길찾기 이동하는 메서드
        /// </summary>
        bool NavigateTo(GeometryTool.NavigateDestinationPreset p_Preset);
  
        /// <summary>
        /// 길찾기 이동을 취소하는 메서드
        /// </summary>
        void StopNavigate();
        
        /// <summary>
        /// 현재 길찾기 이동중인 경우 목적지 좌표를 리턴하는 메서드
        /// </summary>
        (bool, GeometryTool.NavigateDestinationPreset) TryGetDestination();
    }
}