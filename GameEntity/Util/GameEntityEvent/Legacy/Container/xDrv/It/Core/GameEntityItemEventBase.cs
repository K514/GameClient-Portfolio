using System;
using UnityEngine;

namespace k514.Mono.Common
{
    public abstract class GameEntityItemEventBase
    {
        public virtual bool IsUsable(GameEntityItemEventContainer p_Handler)
        {
            return true;
        }
        
        public abstract GameEntityItemTool.EquipItemResult UseItem(GameEntityItemEventContainer p_Handler);

        public bool IsDiscardible(GameEntityItemEventContainer p_Handler)
        {
            return true;
        }
        
        public bool TerminateItem(GameEntityItemEventContainer p_Handler)
        {
            return true;
        }
    }
}