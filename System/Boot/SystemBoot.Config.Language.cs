using System.Threading;
using Cysharp.Threading.Tasks;
using k514.Mono.Feature;

namespace k514
{
    public partial class SystemBoot
    {
        #region <Consts>

        public static LanguageType SystemLanguageType { get; private set; } = LanguageType.None;

        #endregion

        #region <Methods>

        public static async UniTask SwitchLanguage(LanguageType p_Type, CancellationToken p_Token)
        {
            if(SystemLanguageType == p_Type) return;
            
            var prevType = SystemLanguageType;
            SystemLanguageType = p_Type;
            await (await LanguageDataTableQuery.GetInstanceSafe(GetSystemCancellationToken())).LoadTable(p_Token);

            LanguageEventSenderManager.GetInstanceUnsafe.SendEvent(LanguageDataTableQuery.LanguageEventType.LanguageChanged, new LanguageEventParams(prevType, SystemLanguageType));
        }

        #endregion
    }
}