namespace k514.Mono.Common
{
    public class ParticleModelDataTableQuery : MultiTableIndexBase<ParticleModelDataTableQuery, TableMetaData, ParticleModelDataTableQuery.TableLabel, IParticleModelDataTableBridge<IParticleModelDataTableRecordBridge>, IParticleModelDataTableRecordBridge>
    {
        #region <Enums>

        public enum TableLabel
        {
            Weapon,         // 100_000_000 ~ 110_000_000
            Enchant,        // 110_000_000 ~ 120_000_000
            Field,          // 120_000_000 ~ 130_000_000
            Effect,         // 130_000_000 ~ 140_000_000
            Projectile,     // 140_000_000 ~ 150_000_000
        }

        #endregion
    }
}