using k514.Mono.Feature;

namespace k514.Mono.Common
{
    public partial class GameEntityEnchantStorage
    {
        #region <Callbacks>

        private void OnCreateBuff()
        {
            _EntityBuffTable.Add(BuffDataTableQuery.TableLabel.Bless, new BlessBuff());
            _EntityBuffTable.Add(BuffDataTableQuery.TableLabel.Immune, new ImmuneBuff());
            _EntityBuffTable.Add(BuffDataTableQuery.TableLabel.Heal, new HealBuff());
        }

        #endregion

        #region <Classess>

        /// <summary>
        /// 축복 버프
        /// </summary>
        public class BlessBuff : GameEntityEnchantEventBase
        {
            public override bool CastEnchant(IGameEntityEnchantEventContainer p_Container)
            {
                var record = p_Container.Record as BlessBuffDataTable.TableRecord;
                if (ReferenceEquals(null, record))
                {
                    return false;
                }
                else
                {
                    var handler = p_Container.AddDurationHandler(record.Duration);
                    handler.AddState(GameEntityTool.EntityStateType.BLESSED);
                    handler.AddAdditiveStatus(record.AdditiveBattleStatus);
                    handler.AddSimpleMultiplyStatus(record.SimpleMultiplyBattleStatus);
                    handler.AddCompoundMultiplyStatus(record.CompoundMultiplyBattleStatus);

                    return true;
                }
            }
        }
        
        /// <summary>
        /// 면역 버프
        /// </summary>
        public class ImmuneBuff : GameEntityEnchantEventBase
        {
            public override bool CastEnchant(IGameEntityEnchantEventContainer p_Container)
            {
                var record = p_Container.Record as ImmuneBuffDataTable.TableRecord;
                if (ReferenceEquals(null, record))
                {
                    return false;
                }
                else
                {
                    var handler = p_Container.AddDurationHandler(record.Duration);
                    handler.AddAdditiveStatus(record.AdditiveBattleStatus);

                    return true;
                }
            }
        }
        
        /// <summary>
        /// 힐 버프
        /// </summary>
        public class HealBuff : GameEntityEnchantEventBase
        {
            public override bool CastEnchant(IGameEntityEnchantEventContainer p_Container)
            {
                var record = p_Container.Record as HealBuffDataTable.TableRecord;
                if (ReferenceEquals(null, record))
                {
                    return false;
                }
                else
                {
                    var caster = p_Container.Caster;
                    caster.AttachParticle(VfxTool.__HealVfxIndex, caster.GetBottomPosition());
                    
                    var handler = p_Container.AddDurationHandler(record.Duration);
                    handler.AddTickEvent
                    (
                        1f,
                        (_handler, extraOptionParam) =>
                        {
                            if (_handler.Container.TryGetRecord(out HealBuffDataTable.TableRecord o_Record))
                            {
                                var caster = _handler.Caster;
                                caster.HealRateHP(o_Record.Value);
                            }
                        },
                        true
                    );

                    return true;
                }
            }
            
            public override bool TerminateEnchant(IGameEntityEnchantEventContainer p_Container)
            {
                var caster = p_Container.Caster;
                caster.RemoveAttachedParticle(VfxTool.__HealVfxIndex);
                
                return true;
            }
        }
        
        /// <summary>
        /// 승리의 전율 버프
        /// </summary>
        /*public class VictoriaBuff : GameEntityEnchantEventBase
        {
            public override bool CastEnchant(IGameEntityEnchantEventContainer p_Container)
            {
                var record = p_Container.Record as VictoriaBuffDataTable.TableRecord;
                if (ReferenceEquals(null, record))
                {
                    return false;
                }
                else
                {
                    var caster = p_Container.Caster;
                    caster.AttachParticle(VfxTool.__VictoriaVfxIndex, caster.GetBottomPosition());
                    caster.AttachParticle(VfxTool.__VictoriaReleaseVfxIndex, caster.GetBottomPosition());

                    var handler = p_Container.AddDurationHandler(record.Duration);
                    handler.AddEvent
                    (
                        GameEntityTool.GameEntityBaseEventType.Kill,
                        (_handler, baseEventParam, extraOptionParam) =>
                        {
                            if (_handler.Container.TryGetRecord(out VictoriaBuffDataTable.TableRecord o_Record))
                            {
                                var _caster = _handler.Caster;
                                _caster.AttachParticle(VfxTool.__VictoriaVfxIndex, _caster.GetBottomPosition());
                                _handler.AddAdditiveStatus(o_Record.AdditiveBattleStatus);
                                _handler.AddSimpleMultiplyStatus(o_Record.SimpleMultiplyBattleStatus);
                                _handler.AddCompoundMultiplyStatus(o_Record.CompoundMultiplyBattleStatus);
                            }
                        }
                    );
                    handler.AddEvent
                    (
                        GameEntityTool.GameEntityBaseEventType.Hit,
                        (_handler, baseEventParam, extraOptionParam) =>
                        {
                            var _caster = _handler.Caster;
                            _caster.AttachParticle(VfxTool.__VictoriaReleaseVfxIndex, _caster.GetBottomPosition());
                            _handler.ResetStatus();
                        }
                    );

                    return true;
                }
            }
        }
        
        /// <summary>
        /// 불멸 버프
        /// </summary>
        public class ImmortalBuff : GameEntityEnchantEventBase
        {
            public override bool CastEnchant(IGameEntityEnchantEventContainer p_Container)
            {
                var record = p_Container.Record as ImmortalBuffDataTable.TableRecord;
                if (ReferenceEquals(null, record))
                {
                    return false;
                }
                else
                {
                    var handler = p_Container.AddDurationHandler(record.Duration);
                    handler.AddState(GameEntityTool.EntityStateType.IMMORTAL);

                    return true;
                }
            }
        }
        
        /// <summary>
        /// 영체화 버프
        /// </summary>
        public class CloakingBuff : GameEntityEnchantEventBase
        {
            public override bool CastEnchant(IGameEntityEnchantEventContainer p_Container)
            {
                var record = p_Container.Record as CloakingBuffDataTable.TableRecord;
                if (ReferenceEquals(null, record))
                {
                    return false;
                }
                else
                {
                    var handler = p_Container.AddDurationHandler(record.Duration);
                    handler.AddState(GameEntityTool.EntityStateType.CLOAK);

                    return true;
                }
            }
        }*/
        
        #endregion
    }
}