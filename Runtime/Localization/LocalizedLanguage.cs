namespace Amilious.Core.Localization {
    public class LocalizedLanguage {
        
        public LocalizedLanguage() { }

        public LocalizedLanguage(string languageName) { this.languageName = languageName; }
        
        public string languageName = "English";

        public static implicit operator string(LocalizedLanguage language) => language.languageName;
        public static explicit operator LocalizedLanguage(string languageName) => new LocalizedLanguage(languageName);
        
    }
}