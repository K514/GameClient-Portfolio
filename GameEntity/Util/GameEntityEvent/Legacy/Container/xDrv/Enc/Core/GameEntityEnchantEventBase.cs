namespace k514.Mono.Common
{
    public abstract class GameEntityEnchantEventBase
    {
        public virtual bool IsCastable(IGameEntityEnchantEventContainer p_Handler)
        {
            return true;
        }
        
        public abstract bool CastEnchant(IGameEntityEnchantEventContainer p_Handler);
        
        public bool IsDisenchantible(IGameEntityEnchantEventContainer p_Handler)
        {
            return true;
        }
        
        public virtual bool TerminateEnchant(IGameEntityEnchantEventContainer p_Handler)
        {
            return true;
        }

        public virtual string GetOptionDescription(IGameEntityEnchantEventContainer p_Container)
        {
            var record = p_Container.Record;
            if (ReferenceEquals(null, record))
            {
                return record.CacheDescription;
            }

            return string.Empty;
        }
    }
}