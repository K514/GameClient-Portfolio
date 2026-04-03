using System;
using k514.Mono.Feature;
using UnityEngine;

namespace k514.Mono.Common
{
    public interface IGameEntityEnchantEventContainer : IGameEntityEventContainer, IContent
    {
        IEnchantDataTableRecordBridge Record { get; }
        EnchantDataTableQuery.TableLabel EnchantType { get; }
        bool IsEnchantible();
        bool Enchant();
        bool IsDisenchantible();
        bool EnchantImmune();
    }

    public struct GameEntityEnchantEventContainerActivateParams : IGameEntityEventContainerActivateParams
    {
        #region <Fields>

        public IGameEntityBridge Caster { get; }
        public int EventId { get; }
        public GameEntityEventCommonParams CommonParams { get; }

        #endregion

        #region <Constructors>

        public GameEntityEnchantEventContainerActivateParams(IGameEntityBridge p_Caster, int p_Id, GameEntityEventCommonParams p_Params = default)
        {
            Caster = p_Caster;
            EventId = p_Id;
            CommonParams = p_Params;
        }

        #endregion
    }

    public abstract class GameEntityEnchantEventContainer<This, SubType> : GameEntityEventContainer<This, ObjectCreateParams, GameEntityEnchantEventContainerActivateParams, IEnchantDataTableRecordBridge>, IGameEntityEnchantEventContainer
        where This : GameEntityEventContainer<This, ObjectCreateParams, GameEntityEnchantEventContainerActivateParams, IEnchantDataTableRecordBridge>, new()
        where SubType : struct, Enum
    {
        #region <Fields>

        public EnchantDataTableQuery.TableLabel EnchantType { get; private set; }
        public SubType EnchantSubType { get; protected set; }

        #endregion
        
        #region <Callbacks>

        protected override bool OnActivate(ObjectCreateParams p_CreateParams, GameEntityEnchantEventContainerActivateParams p_ActivateParams, bool p_IsPooled)
        {
            if (base.OnActivate(p_CreateParams, p_ActivateParams, p_IsPooled))
            {
                Record = EnchantDataTableQuery.GetInstanceUnsafe.GetRecord(EventId);
                // EnchantType = EnchantDataTableQuery.GetInstanceUnsafe.GetLabel(EventId);
                
                return true;
            }
            else
            {
                return false;
            }
        }

        protected override void OnRetrieve(ObjectCreateParams p_CreateParams)
        {
            base.OnRetrieve(p_CreateParams);

            EnchantType = default;
            EnchantSubType = default;
        }

        #endregion

        #region <Methods>
        
        protected override void InitCancellationToken()
        {
            GameEntityItemStorage.GetInstanceUnsafe.GetLinkedCancellationTokenSource(ref _CancellationTokenSource);
            _CancellationToken = _CancellationTokenSource.Token;
        }

        public abstract bool IsEnchantible();
        public abstract bool Enchant();
        public abstract bool IsDisenchantible();
        public abstract bool EnchantImmune();

        #endregion
    }

    public class GameEntityBuffEventContainer : GameEntityEnchantEventContainer<GameEntityBuffEventContainer, BuffDataTableQuery.TableLabel>
    {
        #region <Callbacks>

        protected override bool OnActivate(ObjectCreateParams p_CreateParams, GameEntityEnchantEventContainerActivateParams p_ActivateParams, bool p_IsPooled)
        {
            if (base.OnActivate(p_CreateParams, p_ActivateParams, p_IsPooled))
            {
                // EnchantSubType = BuffDataTableQuery.GetInstanceUnsafe.GetLabel(EventId);
                
                return true;
            }
            else
            {
                return false;
            }
        }

        protected override void OnContainerTerminate()
        {
            GameEntityEnchantStorage.GetInstanceUnsafe.TerminateEnchant(EnchantSubType, this);
        }

        #endregion

        #region <Methods>

        public override bool IsEnchantible()
        {
            return GameEntityEnchantStorage.GetInstanceUnsafe.IsEnchantible(EnchantSubType, this);
        }

        public override bool Enchant()
        {
            if (IsEnchantible())
            {
                if (GameEntityEnchantStorage.GetInstanceUnsafe.Enchant(EnchantSubType, this))
                {
                    RunContainer();
                    
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
   
        public override bool IsDisenchantible()
        {
            if (GameEntityEnchantStorage.GetInstanceUnsafe.IsDisenchantible(EnchantSubType, this))
            {
                CancelContainer();
                
                return true;
            }
            else
            {
                return false;
            }
        }

        public override bool EnchantImmune()
        {
            return false;
        }

        #endregion
    }
    
    public class GameEntityDebuffEventContainer : GameEntityEnchantEventContainer<GameEntityDebuffEventContainer, DebuffDataTableQuery.TableLabel>
    {
        #region <Callbacks>

        protected override bool OnActivate(ObjectCreateParams p_CreateParams, GameEntityEnchantEventContainerActivateParams p_ActivateParams, bool p_IsPooled)
        {
            if (base.OnActivate(p_CreateParams, p_ActivateParams, p_IsPooled))
            {
                // EnchantSubType = DebuffDataTableQuery.GetInstanceUnsafe.GetLabel(EventId);
                
                return true;
            }
            else
            {
                return false;
            }
        }

        protected override void OnContainerTerminate()
        {
            GameEntityEnchantStorage.GetInstanceUnsafe.TerminateEnchant(EnchantSubType, this);
        }

        #endregion
        
        #region <Methods>

        public override bool IsEnchantible()
        {
            return GameEntityEnchantStorage.GetInstanceUnsafe.IsEnchantible(EnchantSubType, this);
        }

        public override bool Enchant()
        {
            if (IsEnchantible() && GameEntityEnchantStorage.GetInstanceUnsafe.Enchant(EnchantSubType, this))
            {
                RunContainer();
                
                return true;
            }
            else
            {
                return false;
            }
        }
   
        public override bool IsDisenchantible()
        {
            if (GameEntityEnchantStorage.GetInstanceUnsafe.IsDisenchantible(EnchantSubType, this))
            {
                CancelContainer();
                
                return true;
            }
            else
            {
                return false;
            }
        }
        
        public override bool EnchantImmune()
        {
            switch (EnchantSubType)
            {
                /*
                case DebuffDataTableQuery.TableLabel.Stun:
                    return Caster.Enchant(2000200, new GameEntityEventCommonParams(Caster));
                case DebuffDataTableQuery.TableLabel.Freeze:
                    return Caster.Enchant(2000700, new GameEntityEventCommonParams(Caster));
                case DebuffDataTableQuery.TableLabel.Confuse:
                    return Caster.Enchant(2000800, new GameEntityEventCommonParams(Caster));
                case DebuffDataTableQuery.TableLabel.Blind:
                    return Caster.Enchant(2000900, new GameEntityEventCommonParams(Caster));
                case DebuffDataTableQuery.TableLabel.Silence:
                    return Caster.Enchant(2001000, new GameEntityEventCommonParams(Caster));
                case DebuffDataTableQuery.TableLabel.Bind:
                    return Caster.Enchant(2001100, new GameEntityEventCommonParams(Caster));
                case DebuffDataTableQuery.TableLabel.Groggy:
                    return Caster.Enchant(2001200, new GameEntityEventCommonParams(Caster));
                */
                default:
                    return false;
            }
        }

        #endregion
    }
}