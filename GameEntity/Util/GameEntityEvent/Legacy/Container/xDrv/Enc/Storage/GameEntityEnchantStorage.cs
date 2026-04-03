using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using k514.Mono.Feature;

namespace k514.Mono.Common
{
    public partial class GameEntityEnchantStorage : AsyncSingleton<GameEntityEnchantStorage>
    {
        #region <Fields>

        private Dictionary<BuffDataTableQuery.TableLabel, GameEntityEnchantEventBase> _EntityBuffTable;
        private Dictionary<DebuffDataTableQuery.TableLabel, GameEntityEnchantEventBase> _EntityDebuffTable;

        #endregion

        #region <Callbacks>

        protected override void TryInitializeDependency()
        {
            base.TryInitializeDependency();

            _Dependencies.Add(typeof(EnchantDataTableQuery));
            _Dependencies.Add(typeof(BuffDataTableQuery));
            _Dependencies.Add(typeof(DebuffDataTableQuery));
        }

        protected override async UniTask OnCreated(CancellationToken p_CancellationToken)
        {
            _EntityBuffTable = new Dictionary<BuffDataTableQuery.TableLabel, GameEntityEnchantEventBase>();
            _EntityDebuffTable = new Dictionary<DebuffDataTableQuery.TableLabel, GameEntityEnchantEventBase>();

            OnCreateBuff();
            OnCreateDebuff();
            
            await UniTask.CompletedTask;
        }

        protected override async UniTask OnInitiate(CancellationToken p_CancellationToken)
        {
            await UniTask.CompletedTask;
        }

        #endregion

        #region <Methods>

        public bool IsEnchantible(BuffDataTableQuery.TableLabel p_Type, IGameEntityEnchantEventContainer p_Container)
        {
            if (_EntityBuffTable.TryGetValue(p_Type, out var o_Buff))
            {
                return o_Buff.IsCastable(p_Container);
            }
            else
            {
                return false;
            }
        }
        
        public bool IsEnchantible(DebuffDataTableQuery.TableLabel p_Type, IGameEntityEnchantEventContainer p_Container)
        {
            if (_EntityDebuffTable.TryGetValue(p_Type, out var o_Debuff))
            {
                return o_Debuff.IsCastable(p_Container);
            }
            else
            {
                return false;
            }
        }
        
        public bool Enchant(BuffDataTableQuery.TableLabel p_Type, IGameEntityEnchantEventContainer p_Container)
        {
            if (_EntityBuffTable.TryGetValue(p_Type, out var o_Buff))
            {
                return o_Buff.CastEnchant(p_Container);
            }
            else
            {
                return false;
            }
        }
        
        public bool Enchant(DebuffDataTableQuery.TableLabel p_Type, IGameEntityEnchantEventContainer p_Container)
        {
            if (_EntityDebuffTable.TryGetValue(p_Type, out var o_Debuff))
            {
                return o_Debuff.CastEnchant(p_Container);
            }
            else
            {
                return false;
            }
        }
        
        public bool IsDisenchantible(BuffDataTableQuery.TableLabel p_Type, IGameEntityEnchantEventContainer p_Container)
        {
            if (_EntityBuffTable.TryGetValue(p_Type, out var o_Buff))
            {
                return o_Buff.IsDisenchantible(p_Container);
            }
            else
            {
                return false;
            }
        }
        
        public bool IsDisenchantible(DebuffDataTableQuery.TableLabel p_Type, IGameEntityEnchantEventContainer p_Container)
        {
            if (_EntityDebuffTable.TryGetValue(p_Type, out var o_Debuff))
            {
                return o_Debuff.IsDisenchantible(p_Container);
            }
            else
            {
                return false;
            }
        }
        
        public bool TerminateEnchant(BuffDataTableQuery.TableLabel p_Type, IGameEntityEnchantEventContainer p_Container)
        {
            if (_EntityBuffTable.TryGetValue(p_Type, out var o_Buff))
            {
                return o_Buff.TerminateEnchant(p_Container);
            }
            else
            {
                return false;
            }
        }
        
        public bool TerminateEnchant(DebuffDataTableQuery.TableLabel p_Type, IGameEntityEnchantEventContainer p_Container)
        {
            if (_EntityDebuffTable.TryGetValue(p_Type, out var o_Debuff))
            {
                return o_Debuff.TerminateEnchant(p_Container);
            }
            else
            {
                return false;
            }
        }

        #endregion
    }
}