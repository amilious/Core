using System;
using System.Collections.Generic;
using System.IO;
using Amilious.Core.Extensions;
using Amilious.Core.IO;
using UnityEngine;

namespace Amilious.Core.Localization {
    public static partial class AmiliousLocalization {

        #if UNITY_EDITOR
        
        private const string SHOW_USAGE_NAME = "AmiliousLocalization/Show Usage";
        private const string SHOW_PATH_NAME = "AmiliousLocalization/Show Path";
        private const string SHOW_DESCRIPTION_NAME = "AmiliousLocalization/Show Description";
        private const string SHOW_TRANSLATION_NAME = "AmiliousLocalization/Show Translation";
        
        private static bool? _showUsage;
        private static bool? _showPath;
        private static bool? _showDesription;
        private static bool? _showTranslation;
        
        public static bool ShowUsage {
            get {
                if(_showUsage.HasValue) return _showUsage.Value;
                _showUsage = BasicSave.ReadData(SHOW_USAGE_NAME,false);
                return _showUsage.Value;
            }
            set {
                if(_showUsage == value) return;
                _showUsage = value;
                BasicSave.StoreData(SHOW_USAGE_NAME,value);
                OnShowUsageToggled?.Invoke(value);
            }
        }

        public static bool ShowPath {
            get {
                if(_showPath.HasValue) return _showPath.Value;
                _showPath = BasicSave.ReadData(SHOW_PATH_NAME,true);
                return _showPath.Value;
            }
            set {
                if(_showPath == value) return;
                _showPath = value;
                BasicSave.StoreData(SHOW_PATH_NAME,value);
                OnShowPathToggled?.Invoke(value);
            } 
        }
        
        public static bool ShowDescription {
            get {
                if(_showDesription.HasValue) return _showDesription.Value;
                _showDesription = BasicSave.ReadData(SHOW_DESCRIPTION_NAME,true);
                return _showDesription.Value;
            }
            set {
                if(_showDesription == value) return;
                _showDesription = value;
                BasicSave.StoreData(SHOW_DESCRIPTION_NAME,value);
                OnShowDescriptionToggled?.Invoke(value);
            } 
        }
        
        public static bool ShowTranslation {
            get {
                if(_showTranslation.HasValue) return _showTranslation.Value;
                _showTranslation = BasicSave.ReadData(SHOW_TRANSLATION_NAME,true);
                return _showTranslation.Value;
            }
            set {
                if(_showTranslation == value) return;
                _showTranslation = value;
                #if UNITY_EDITOR
                BasicSave.StoreData(SHOW_TRANSLATION_NAME,value);
                OnShowTranslationToggled?.Invoke(value);
                #endif
            } 
        }
        
        #region Editor Methods /////////////////////////////////////////////////////////////////////////////////////////
        
        [UnityEditor.MenuItem(LOG_LOAD_NAME,priority = 4)] 
        private static void ToggleLogLoad() => LogLoad = !LogLoad;

        [UnityEditor.MenuItem(LOG_UNLOAD_NAME,priority = 6)] 
        private static void ToggleLogUnload() => LogUnload = !LogUnload;
        
        /// <summary>
        /// This method is used to add a key.
        /// </summary>
        /// <param name="key">The key that you want to add.</param>
        /// <param name="description">A description for the key.</param>
        /// <param name="path">The path of the key file that you want to add the key to.</param>
        /// <returns>True if the key was written, otherwise false.</returns>
        /// <remarks>This method is only available in the editor.</remarks>
        public static bool AddKey(string key, string description = "", string path = null) {
            if(KeyData.ContainsKey(key)) return false;
            path ??= FileHelper.CreatePath(Application.dataPath, "Amilious", "Core", 
                "Resources", "Languages", "Keys.csv");
            if(!Path.GetFileName(path).EndsWith("Keys.csv")) return false;
            AddIgnoreFileChange(path,IgnoreType.Add);
            if(!CsvHelper.TryAppendKeyValuePair(path, key, description)) {
                return false;
            }
            KeyData.Add(key,new KeyInfo(path,key,description));
            OnKeysUpdated?.Invoke();
            OnKeyAdded?.Invoke(key);
            return true;
        }
        
        /// <summary>
        /// This method is used to remove a key.
        /// </summary>
        /// <param name="key">The key that you want to remove.</param>
        /// <param name="removeValues">If true the values will also be removed.</param>
        /// <returns>True if the key was removed, otherwise false.</returns>
        /// <remarks>This method is only available in the editor.</remarks>
        public static bool RemoveKey(string key, bool removeValues = true) {
            if(!KeyData.TryGetValueFix(key, out var keyInfo)) {
                OnUnknownKey?.Invoke(key);
                return false;
            }
            if(!CsvHelper.TryRemoveKeys(keyInfo.Path, key)) return false;
            KeyData.Remove(key);
            foreach(var lang in Data.Keys) {
                if(removeValues && TryRemoveTranslation(lang, keyInfo)) continue;
                Data[lang].Remove(key);
            }
            foreach(var lang in OverrideData.Values) lang.Remove(key);
            OnKeysUpdated?.Invoke();
            OnKeyRemoved?.Invoke(key);
            return true;
        }

        public static bool TryMoveKey(string key, string path) {
            path = GetLocationPath(path);
            if(!KeyPaths.ContainsValue(path)) return false;
            if(!TryGetKeyInfo(key, out var keyInfo)) return false;
            //first try to move the key
            if(!CsvHelper.TryEditAddValue(path, key, keyInfo.Description)) return false;
            if(!CsvHelper.TryRemoveKeys(keyInfo.Path, key)) return false;
            KeyData[key] = new KeyInfo(path, key, keyInfo.Description);
            //try move the values
            foreach(var (lang, data) in Data) {
                if(!data.TryGetValueFix(key, out var val))continue;
                AddIgnoreFileChange(path,IgnoreType.Add);
                CsvHelper.TryEditAddValue(FileHelper.GetSiblingFile(path, lang+".csv"), key, val);
                CsvHelper.TryRemoveKeys(keyInfo.GetLanguagePath(lang), key);
            }
            OnKeyMoved?.Invoke(key,KeyData[key]);
            return false;
        }

        public static IEnumerable<string> GetValidKeys(params string[] validPaths) {
            var paths = new List<string>();
            foreach(var valid in validPaths) {
                if(KeyPaths.TryGetValueFix(valid,out var p)&&!paths.Contains(p)) paths.Add(p);
                else if(KeyPaths.ContainsValue(valid)&&!paths.Contains(valid)) paths.Add(valid);
                else Log("<color=red>Invalid Key Path {0}</color>",valid);
            }
            foreach(var keyInfo in KeyData.Values) {
                if(paths.Contains(keyInfo.Path)) yield return keyInfo.Key;
            }
        }
        
        /// <summary>
        /// This method is used to get the path of the location text.
        /// </summary>
        /// <param name="locationName">The name of the location that you want to get the path for.</param>
        /// <returns>The path of the location or the default location.</returns>
        public static string GetLocationPath(string locationName) {
            if(string.IsNullOrEmpty(locationName)) return locationName;
            if(KeyPaths.ContainsValue(locationName)) return locationName;
            if(!KeyPaths.TryGetValueFix(locationName, out var path))
                return DefaultKeyPath;
            return path;
        }
        
        /// <summary>
        /// This method is used to get the location name from the given path.
        /// </summary>
        /// <param name="keyPath">The path that you want to get the name of.</param>
        /// <returns>The name of the given path or the default name.</returns>
        public static string GetLocationName(string keyPath) {
            if(string.IsNullOrEmpty(keyPath)) return keyPath;
            if(KeyPaths.ContainsKey(keyPath)) return keyPath;
            foreach(var keyVal in KeyPaths) {
                if(keyVal.Value == keyPath) return keyVal.Key;
            }
            return DefaultKeyPathName;
        }

        /// <summary>
        /// This method is used to try edit a key's description.
        /// </summary>
        /// <param name="key">The key that you want to edit the description of.</param>
        /// <param name="description">The new key description.</param>
        /// <returns>True if the description was edited, otherwise false.</returns>
        /// <remarks>This method is only available in the editor.</remarks>
        public static bool TryEditDescription(string key, string description) {
            if(!KeyData.TryGetValueFix(key, out var keyInfo)) {
                OnUnknownKey?.Invoke(key);
                return false;
            }
            if(keyInfo.Description == description) return false;
            var path = keyInfo.Path;
            if(!CsvHelper.TryEditValue(path, key, description)) return false;
            keyInfo.SetDescription(description);
            OnKeyDescriptionUpdated?.Invoke(key);
            OnKeysUpdated?.Invoke();
            return true;
        }

        /// <summary>
        /// This method is used to try add a translation of a key for a language.
        /// </summary>
        /// <param name="language">The language that you want to add the translation to.</param>
        /// <param name="key">The key the translation is for.</param>
        /// <param name="value">The value of the translation.</param>
        /// <returns>True if the translation was added, otherwise false.</returns>
        /// <remarks>This method is only available in the editor.</remarks>
        public static bool TryAddTranslation(string language, string key, string value) {
            if(!KeyData.TryGetValueFix(key, out var keyInfo)) {
                OnUnknownKey?.Invoke(key);
                return false;
            }
            if(!Data.TryGetValueFix(language, out var lang) || lang.ContainsKey(key)) return false;
            var path = keyInfo.GetLanguagePath(language);
            AddIgnoreFileChange(path,IgnoreType.Add);
            if(!CsvHelper.TryAppendKeyValuePair(path, key, value)) return false;
            lang[key] = value;
            OnTranslationUpdated?.Invoke(language,key);
            return true;
        }

        /// <summary>
        /// This method is used to try edit a translation of a key for a language.
        /// </summary>
        /// <param name="language">The language that you want to edit the translation for.</param>
        /// <param name="key">The key the translation is for.</param>
        /// <param name="value">The new value of the translation.</param>
        /// <returns>True if the translation was edited, otherwise false.</returns>
        /// <remarks>This method is only available in the editor.</remarks>
        public static bool TryEditTranslation(string language, string key, string value) {
            if(!KeyData.TryGetValueFix(key, out var keyInfo)) {
                OnUnknownKey?.Invoke(key);
                return false;
            }
            if(!Data.TryGetValueFix(language, out var lang) || 
                !lang.TryGetValueFix(key, out var translation)) return false;
            if(translation == value) return false;
            var path = keyInfo.GetLanguagePath(language);
            if(!CsvHelper.TryEditValue(path, key, value)) return false;
            lang[key] = value;
            OnTranslationUpdated?.Invoke(language,key);
            return true;
        }

        /// <summary>
        /// This method first trys to edit a translation or add a translation if the translation does not currently
        /// exist.
        /// </summary>
        /// <param name="language">The language that you want to edit or add a translation to.</param>
        /// <param name="key">The key the translation is for.</param>
        /// <param name="value">The value of the translation.</param>
        /// <returns>True if the translation was edited or added, otherwise false.</returns>
        /// <remarks>This method is only available in the editor.</remarks>
        public static bool TryAddOrEditTranslation(string language, string key, string value) {
            if(!Data.TryGetValueFix(language, out var lang)) {
                OnUnknownKey?.Invoke(key);
                return false;
            }
            if(!lang.ContainsKey(key)) return TryAddTranslation(language, key, value);
            return TryEditTranslation(language, key, value);
        }

        /// <summary>
        /// This method is used to try remove a translation.
        /// </summary>
        /// <param name="language">The language that you want to remove the translation from.</param>
        /// <param name="key">The key that you want to remove the translation for.</param>
        /// <returns>True if the translation was removed, otherwise false.</returns>
        /// <remarks>This method is only available in the editor.</remarks>
        public static bool TryRemoveTranslation(string language, string key) {
            if(KeyData.TryGetValueFix(key, out var keyInfo)) 
                return TryRemoveTranslation(language, keyInfo);
            OnUnknownKey?.Invoke(key);
            return false;
        }
        
        /// <summary>
        /// This method is used to try remove a translation.
        /// </summary>
        /// <param name="language">The language that you want to remove the translation from.</param>
        /// <param name="keyInfo">The key that you want to remove the translation for.</param>
        /// <returns>True if the translation was removed, otherwise false.</returns>
        /// <remarks>This method is only available in the editor.</remarks>
        public static bool TryRemoveTranslation(string language, KeyInfo keyInfo) {
            if(!Data.TryGetValueFix(language, out var lang) || !lang.ContainsKey(keyInfo.Key)) 
                return false;
            var path = keyInfo.GetLanguagePath(language);
            if(!CsvHelper.TryRemoveKeys(path, keyInfo.Key)) return false;
            lang.Remove(keyInfo.Key);
            OnTranslationUpdated?.Invoke(language,keyInfo.Key);
            return true;
        }

        public static Dictionary<string, string> GetEditorDictionary() {
            return Data[CurrentLanguage];
        }
        
        public static bool TryAddLanguage(string language) {
            if(HasLanguage(language, true)) return false;
            var path = FileHelper.CreatePath(Application.dataPath, "Amilious", "Core", 
                "Resources", "Languages", language+".csv");
            try {
                var directoryPath = Path.GetDirectoryName(path);
                // Create the directory if it doesn't exist
                if(directoryPath == null) return false;
                if(!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);
                // Create the file if it doesn't exist
                if(File.Exists(path)) return false;
                AddIgnoreFileChange(path,IgnoreType.Add);
                File.Create(path).Close();
            }catch(Exception) { return false; }
            LanguageNames.Add(language);
            OnLanguagesUpdated?.Invoke();
            LoadLanguage(language);
            return true;
        }
        
        public static bool HasKeyPathName(string value) => KeyPaths.ContainsKey(value);

        public static void AddIgnoreFileChange(string file, IgnoreType ignoreType) {
            IgnoreFileChange[file] = ignoreType;
        }
        
        public static bool CheckIgnoreFileChange(string file, IgnoreType ignoreType) {
            file = FileHelper.GetFullPathFromAssetPath(file);
            if(!IgnoreFileChange.TryGetValueFix(file, out var mode)) return false;
            IgnoreFileChange.Remove(file);
            return mode == ignoreType;
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #endif
    }
}