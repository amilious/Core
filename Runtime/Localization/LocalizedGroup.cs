using Cysharp.Text;

namespace Amilious.Core.Localization {
    
    public class LocalizedGroup {
        
        public string GroupName { get; }

        public string this[string key] => AmiliousLocalization.GetTranslation(GetKey(key));
        
        public string this[string key, params object[] args] => 
            string.Format(AmiliousLocalization.GetTranslation(GetKey(key)), args);

        public bool IsInGroup(string key) => key.Contains(GroupName);
        
        public LocalizedGroup(string groupName) {
            GroupName = !groupName.EndsWith("/") ? $"{groupName}/" : groupName;
        }

        public string GetKey(string key) => ZString.Concat(GroupName, key);

    }
    
}