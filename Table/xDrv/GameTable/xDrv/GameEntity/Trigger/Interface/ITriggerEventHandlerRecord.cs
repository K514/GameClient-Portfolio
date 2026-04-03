using System;

namespace k514.Mono.Common
{
    public interface ITriggerEventHandlerRecord : ITableRecord<int>
    {
        /// <summary>
        /// 이벤트 핸들러 타입
        /// </summary>
        Type EventHandlerType { get; }
        
        /// <summary>
        /// 자원소모
        /// </summary>
        float Cost { get; }
        
        /// <summary>
        /// 쿨타임
        /// </summary>
        float Cooldown { get; }
        
        /// <summary>
        /// 최대 레벨
        /// </summary>
        int LevelUpperBound { get; }
    }
}