namespace k514.Mono.Common
{
    public interface IGameEntityIdentify
    {
        EntityLocalId LocalId { get; }
        void SetLocalId(EntityLocalId p_LocalId);
        void ResetLocalId();
    }
}