using System.Text;
using UnityEngine;

namespace k514.Mono.Common
{
    public partial class GameEntityBase<Content, CreateParams, ActivateParams>
    {
        #region <Fields>

        private string _prefix;
        private string _alias;
        private string _name;
        private string _postfix;
        private StringBuilder _fullName;

        #endregion
        
        #region <Callbacks>

        private void OnCreateName()
        {
            _fullName = new StringBuilder();
            SetDefaultName();
        }
        
        private void OnActivateName(ActivateParams p_ActivateParams)
        {
            _alias = p_ActivateParams.Alias;
            
            SetDefaultName();
        }

        private void OnRetrieveName()
        {
        }

        private void OnUpdateName()
        {
            _fullName.Clear();
            _fullName.Append(_prefix);
            _fullName.Append(_name);
            _fullName.Append(_postfix);


#if UNITY_EDITOR
            var tryFullName = _fullName.ToString();
            name = tryFullName;
#endif
        }

        #endregion

        #region <Methods>

        private void SetDefaultName()
        {
            _prefix = string.Empty;
#if UNITY_EDITOR
            _name = string.IsNullOrEmpty(_alias) ? ComponentDataRecord.GetEntityLanguageRecord()?.Text ?? $"Noname [{GetModelName()}]" : _alias;
#else
            _name = string.IsNullOrEmpty(_alias) ? ComponentDataRecord.GetEntityLanguageRecord()?.Text ?? string.Empty : _alias;
#endif
            _postfix = string.Empty;
            
            OnUpdateName();
        }

        public string GetDefaultName()
        {
            return _name;
        }

        public string GetName()
        {
            return _name;
        }
        
        public void SetPreFix(string p_Content)
        {
            _prefix = p_Content;
            OnUpdateName();
        }
        
        public void SetName(string p_Content)
        {
            _name = p_Content;
            OnUpdateName();
        }
        
        public void SetPostFix(string p_Content)
        {
            _postfix = p_Content;
            OnUpdateName();
        }
        
        #endregion
    }
}