using UnityEngine;

namespace k514.Mono.Common
{
    public partial class GameEntityBase<Content, CreateParams, ActivateParams>
    {
        public void OnUnitLayerChanged()
        {
        }

        /// <summary>
        /// 길찾기 에이전트(NavMeshAgent)가 이동할 목적지를 선정한 경우 호출되는 콜백
        /// </summary>
        public void OnSelectDestination()
        {
        }
        
        /// <summary>
        /// 어떤 방식이든 길찾기 에이전트(NavMeshAgent)가 목적지를 잃은 경우 호출되는 콜백
        /// </summary>
        public void OnReachedDestination()
        {
        }
        
        /// <summary>
        /// 스케일이 적용된 이동속도를 리턴하는 메서드
        /// </summary>
        public float GetScaledMovementSpeed()
        {
            return SqrtScale * this[StatusTool.BattleStatusGroupType.Total].GetScaledMoveSpeed();
        }
    }
}