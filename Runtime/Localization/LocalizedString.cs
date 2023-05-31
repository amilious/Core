using UnityEngine;

namespace Amilious.Core.Localization {
    
    [System.Serializable]
    public class LocalizedString {

        [SerializeField]private string key;

        public string Key => key;

        public string Value => AmiliousLocalization.GetTranslation(key);

        public string this[string language] {
            get {
                AmiliousLocalization.TryGetTranslation(language, key, out var translation);
                return translation;
            }
        }

        public LocalizedString(string key) => this.key = key;

        public static implicit operator LocalizedString(string key) => new LocalizedString(key);

    }
    
}