using System.Collections.Generic;
using k514.Mono.Common;
using UnityEngine;

namespace k514.Mono.Feature
{
    public interface IUnitBridge : IGameEntityBridge
    {
        /* Attach Point */
        bool TryGetAttachPoint(UnitTool.AttachPoint p_TargetPoint, out Transform o_Transform);
        Vector3 GetAttachPosition(UnitTool.AttachPoint p_TargetPoint);
        
        /* Exp */
        float GetNeededExp();
        float GetNeededExpInv();
        float GetCurrentExp();
        float GetCurrentExpRate();
        void SetExp(float p_Exp);
        void AddExp(float p_Exp);
        void SetExpRate(float p_Rate);
        void AddExpRate(float p_Rate);
    }
}