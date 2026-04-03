using k514.Mono.Feature;

namespace k514.Mono.Common
{
    public abstract class GameEntityExtraOptionBase
    {
        #region <Methods>

        public virtual void PreloadOption(GameEntityExtraOptionHandler p_Handler)
        {
        }
        
        public abstract bool ActivateOption(GameEntityExtraOptionHandler p_Handler);

        public virtual bool DeactivateOption(GameEntityExtraOptionHandler p_Handler)
        {
            return true;
        }

        public string GetOptionName(GameEntityExtraOptionHandler p_Handler)
        {
            return GetOptionName(p_Handler.Record, p_Handler.Container.CommonParams, p_Handler.ExtraOptionParams);
        }

        public string GetOptionName(int p_Index, GameEntityEventCommonParams p_Params = default, GameEntityExtraOptionTool.GameEntityExtraOptionParams p_OptionParams = default)
        {
            if (ExtraOptionDataTable.GetInstanceUnsafe.TryGetRecord(p_Index, out var o_Record))
            {
                return GetOptionName(o_Record, p_Params, p_OptionParams);
            }
            else
            {
                return string.Empty;
            }
        }
        
        public virtual string GetOptionName(ExtraOptionDataTable.TableRecord p_Record, GameEntityEventCommonParams p_Params = default, GameEntityExtraOptionTool.GameEntityExtraOptionParams p_OptionParams = default)
        {
            if (p_Record.TryGetLanguageRecord(out var o_Record))
            {
                return o_Record.Text;
            }
            else
            {
                return string.Empty;
            }
        }
        
        public string GetOptionDescription(GameEntityExtraOptionHandler p_Handler)
        {
            return GetOptionDescription(p_Handler.Record, p_Handler.Container.CommonParams, p_Handler.ExtraOptionParams);
        }
        
        public string GetOptionDescription(int p_Index, GameEntityEventCommonParams p_Params = default, GameEntityExtraOptionTool.GameEntityExtraOptionParams p_OptionParams = default)
        {
            if (ExtraOptionDataTable.GetInstanceUnsafe.TryGetRecord(p_Index, out var o_Record))
            {
                return GetOptionDescription(o_Record, p_Params, p_OptionParams);
            }
            else
            {
                return string.Empty;
            }
        }
        
        public abstract string GetOptionDescription(ExtraOptionDataTable.TableRecord p_Record, GameEntityEventCommonParams p_Params = default, GameEntityExtraOptionTool.GameEntityExtraOptionParams p_OptionParams = default);

        #endregion
    }
}