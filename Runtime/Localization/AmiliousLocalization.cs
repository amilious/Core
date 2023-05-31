/*//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                                                    //
//    _____            .__ .__   .__                             _________  __              .___.__                   //
//   /  _  \    _____  |__||  |  |__|  ____   __ __  ______     /   _____/_/  |_  __ __   __| _/|__|  ____   ______   //
//  /  /_\  \  /     \ |  ||  |  |  | /  _ \ |  |  \/  ___/     \_____  \ \   __\|  |  \ / __ | |  | /  _ \ /  ___/   //
// /    |    \|  Y Y  \|  ||  |__|  |(  <_> )|  |  /\___ \      /        \ |  |  |  |  // /_/ | |  |(  <_> )\___ \    //
// \____|__  /|__|_|  /|__||____/|__| \____/ |____//____  >    /_______  / |__|  |____/ \____ | |__| \____//____  >   //
//         \/       \/                                  \/             \/                    \/                 \/    //
//                                                                                                                    //
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//  Website:        http://www.amilious.com         Unity Asset Store: https://assetstore.unity.com/publishers/62511  //
//  Discord Server: https://discord.gg/SNqyDWu            Copyright© Amilious since 2022                              //                    
//  This code is part of an asset on the unity asset store. If you did not get this from the asset store you are not  //
//  using it legally. Check the asset store or join the discord for the license that applies for this script.         //
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/

using System;
using System.IO;
using UnityEngine;
using System.Linq;
using Cysharp.Text;
using Amilious.Core.IO;
using Amilious.Core.Extensions;
using System.Collections.Generic;

// ReSharper disable MemberCanBePrivate.Global
namespace Amilious.Core.Localization {
    
    public static partial class AmiliousLocalization {

        #region Constants //////////////////////////////////////////////////////////////////////////////////////////////
        
        private const string ENGLISH = "English";
        private const string FALLBACK_NAME = "AmiliousLocalization/Fallback";
        private const string LANGUAGE_NAME = "Amilious/Localization/Language";
        private const string LOG_LOAD_NAME = "Amilious/Localization/Log Load";
        private const string LOG_UNLOAD_NAME = "Amilious/Localization/Log Unload";
        private const string AUTO_UNLOAD_NAME = "Amilious/Localization/Auto Unload Languages";
        private const string MESSAGE_HEADER = "<b><color=#0080ff>Amilious Localization:</color></b> ";

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Private Fields /////////////////////////////////////////////////////////////////////////////////////////

        private static readonly Dictionary<string, Dictionary<string, string>> Data =
            new Dictionary<string, Dictionary<string, string>>();
        private static readonly Dictionary<string, Dictionary<string, string>> OverrideData =
            new Dictionary<string, Dictionary<string, string>>();
        private static readonly Dictionary<string, int> KeyUsage = new Dictionary<string, int>();
        private static readonly Dictionary<string, IgnoreType> IgnoreFileChange = new Dictionary<string, IgnoreType>();
        private static readonly Dictionary<string, string> KeyPaths = new Dictionary<string, string>();
        private static readonly Dictionary<string, KeyInfo> KeyData = new Dictionary<string, KeyInfo>();
        private static readonly List<string> LanguageNames = new List<string>();
        private static readonly List<string> Loaded = new List<string>();
        private static string _language = ENGLISH;
        private static string _fallbackLanguage = ENGLISH;
        private static bool _logLoad;
        private static string _defaultKeyPath;
        private static bool _logUnload;
        private static bool _logSave;
        private static bool _countingPaused;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
      
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This property is true if loading a language should be logged, otherwise false.
        /// </summary>
        public static bool LogLoad {
            get => _logLoad;
            set {
                if(_logLoad == value) return;
                _logLoad = value;
                #if UNITY_EDITOR
                BasicSave.StoreData(LOG_LOAD_NAME,value);
                UnityEditor.Menu.SetChecked(LOG_LOAD_NAME,value);
                #endif
            } 
        }

        /// <summary>
        /// This property is true if unloading a language should be logged, otherwise false.
        /// </summary>
        public static bool LogUnload {
            get => _logUnload;
            set {
                if(_logUnload == value) return;
                _logUnload = value;
                #if UNITY_EDITOR
                BasicSave.StoreData(LOG_UNLOAD_NAME,value);
                UnityEditor.Menu.SetChecked(LOG_UNLOAD_NAME,value);
                #endif
            }
        }

        /// <summary>
        /// This property contains the currently loaded language.
        /// </summary>
        public static string CurrentLanguage {
            get => _language;
            set {
                if(_language == value) return;
                if(!LoadedLanguages.Contains(value)) LoadLanguage(value);
                if(!LoadedLanguages.Contains(value)) {
                    Log("Unable to change the current language to {0} because no language found with that name!",value);
                    return;
                }
                var previous = _language;
                _language = value;
                BasicSave.StoreData(LANGUAGE_NAME,_language);
                UnloadLanguage(previous);
                OnLanguageChanged?.Invoke(previous,_language);
            }
        }

        /// <summary>
        /// This property contains the fallback language.
        /// </summary>
        public static string FallbackLanguage {
            get => _fallbackLanguage;
            set {
                if(_fallbackLanguage == value) return;
                if(!LoadedLanguages.Contains(value)) LoadLanguage(value);
                if(!LoadedLanguages.Contains(value)) {
                    Log("Unable to change the fallback language to {0} because no language found with that name!",value);
                    return;
                }
                var previous = _fallbackLanguage;
                _fallbackLanguage = value;
                BasicSave.StoreData(FALLBACK_NAME,_fallbackLanguage);
                UnloadLanguage(previous);
                OnLanguageChanged?.Invoke(previous,_fallbackLanguage);
            }
        }
        
        /// <summary>
        /// This property returns an array of the available languages.
        /// </summary>
        public static string[] Languages => LanguageNames.ToArray();

        /// <summary>
        /// This property returns an array of the currently loaded languages.
        /// </summary>
        public static string[] LoadedLanguages => Loaded.ToArray();

        /// <summary>
        /// This property returns an array of the current key files.
        /// </summary>
        public static string[] KeyFileNames => KeyPaths.Keys.ToArray();

        /// <summary>
        /// This property return an array of the keys.
        /// </summary>
        public static string[] Keys => KeyData.Keys.ToArray();

        public static string DefaultKeyPathName => @"Amilious|Core";
        
        public static string DefaultKeyPath {
            get {
                if(_defaultKeyPath != null) return _defaultKeyPath;
                _defaultKeyPath = FileHelper.CreatePath(Application.dataPath, "Amilious", "Core", 
                    "Resources", "Languages", "Keys.csv");
                return _defaultKeyPath;
            }
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Constructor ////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This constructor will set up the static class the first time it is called.
        /// </summary>
        static AmiliousLocalization() {
            //load starting values
            _logLoad = BasicSave.ReadData(LOG_LOAD_NAME, false);
            _logUnload = BasicSave.ReadData(LOG_UNLOAD_NAME, false);
            LoadLanguageNames();
            LoadKeys();
            #if UNITY_EDITOR //set the checks
            UnityEditor.Menu.SetChecked(LOG_LOAD_NAME,_logLoad);
            UnityEditor.Menu.SetChecked(LOG_UNLOAD_NAME,_logUnload);
            //load all of the language if running in the editor
            foreach(var language in LanguageNames) LoadLanguage(language);
            #endif
            //load the language
            CurrentLanguage = BasicSave.ReadData(LANGUAGE_NAME, ENGLISH);
            FallbackLanguage = BasicSave.ReadData(FALLBACK_NAME, ENGLISH);
        } 

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////
       
        /// <summary>
        /// This method is used to get the streaming asset path for the given language.
        /// </summary>
        /// <param name="language">The language that you want to get the streaming asset path for.</param>
        /// <returns>The streaming asset path for the given language.</returns>
        public static string GetStreamingPath(string language) =>
            FileHelper.CreatePath(Application.streamingAssetsPath, "Language", language + ".csv");

        /// <summary>
        /// This method is used to check if the given key exists.
        /// </summary>
        /// <param name="key">The key that you want to check for.</param>
        /// <returns>True if the key exists, otherwise returns false.</returns>
        public static bool HasKey(string key) => key != null && KeyData.ContainsKey(key);

        /// <summary>
        /// This method is used to check if a language exists.
        /// </summary>
        /// <param name="language">The language that you want to check for.</param>
        /// <param name="includeNotLoaded">If true unloaded languages will be included in the check, otherwise only
        /// loaded languages will be included.</param>
        /// <returns>True if the given language is currently loaded, otherwise false.</returns>
        public static bool HasLanguage(string language, bool includeNotLoaded=false) => 
            includeNotLoaded?LanguageNames.Contains(language): LoadedLanguages.Contains(language);
        
        /// <summary>
        /// This method is used to check if there is a base value for the given key in the given language.
        /// </summary>
        /// <param name="key">The key of the value that you want to check for.</param>
        /// <param name="language">The language or null for the current language.</param>
        /// <returns>True if there is a base value for the given key in the given language, otherwise false.</returns>
        /// <remarks>This method will only check currently loaded languages.</remarks>
        public static bool HasBaseValue(string key, string language = null) => Data
            .TryGetValueFix(language??CurrentLanguage, out var data) && data.ContainsKey(key);

        /// <summary>
        /// This method is used to check if there is an override value for the given key in the given language.
        /// </summary>
        /// <param name="key">The key of the value that you want to check for.</param>
        /// <param name="language">The language or null for the current language.</param>
        /// <returns>True if there is an override value for the given key in the given language, otherwise false.</returns>
        /// <remarks>This method will only check currently loaded languages.</remarks>
        public static bool HasOverrideValue(string key, string language = null) => OverrideData
            .TryGetValueFix(language??CurrentLanguage, out var data) && data.ContainsKey(key);
        
        /// <summary>
        /// This method is used to check if there is currently a translation for the given key.
        /// </summary>
        /// <param name="key">The key that you want to check.</param>
        /// <param name="language">The language or null for the current language.</param>
        /// <returns>True if there is a translation for the given key, otherwise false.</returns>
        /// <remarks>This method will only check currently loaded languages.</remarks>
        public static bool HasTranslation(string key, string language = null) =>
            HasBaseValue(key, language) || HasOverrideValue(key, language);
        
        /// <summary>
        /// This method is used to get the translation for the given key.
        /// </summary>
        /// <param name="key">The key of the translation that you want to get.</param>
        /// <returns>The value of the given key in the current language.  If the key does not exist in the
        /// current language it will return tha key's value in the fallback language.  If that does not exist, it will
        /// return the key.</returns>
        public static string GetTranslation(string key) {
            AddToCount(key);
            if(!HasKey(key)) {
                OnUnknownKey?.Invoke(key);
                return key;
            }
            if(TryGetTranslation(CurrentLanguage, key, out var translation)) return translation;
            if(FallbackLanguage != CurrentLanguage && TryGetTranslation(FallbackLanguage, key, out translation)) 
                return translation;
            if(Application.isEditor)Log("There was not value for the \"{0}\" key.",key);
            return key;
        }
        
        /// <summary>
        /// This method is used to try get the translation for the given key.
        /// </summary>
        /// <param name="key">The key of the translation that you want to get.</param>
        /// <param name="translation">The value of the given key in the current language.  If the key does not exist in
        /// the current language it will return tha key's value in the fallback language.  If that does not exist, it
        /// will return the key.</param>
        /// <returns>True if the current language has a translation, otherwise false if using fallback or no
        /// translation.</returns>
        public static bool TryGetTranslation(string key, out string translation) {
            AddToCount(key);
            translation = key;
            if(!HasKey(key)) {
                OnUnknownKey?.Invoke(key);
                return false;
            }
            if(TryGetTranslation(CurrentLanguage, key, out translation)) return true;
            if(FallbackLanguage == CurrentLanguage) return false;
            TryGetTranslation(FallbackLanguage, key, out translation);
            return false;
        }

        /// <summary>
        /// This method is used to try get the translation for the given language and key.
        /// </summary>
        /// <param name="language">The language that you want to get the key for.</param>
        /// <param name="key">The key that you want to get the translation for.</param>
        /// <param name="translation">The translation.</param>
        /// <returns>True if able to get a translation, otherwise false.</returns>
        public static bool TryGetTranslation(string language, string key, out string translation, bool includeOverride = true) {
            AddToCount(key);
            translation = string.Empty;
            if(!HasKey(key)) {
                OnUnknownKey?.Invoke(key);
                return false;
            }
            if(Data.TryGetValueFix(language, out var lang) && 
                lang.TryGetValueFix(key, out translation)&& !string.IsNullOrWhiteSpace(translation)) return true;
            if(includeOverride && OverrideData.TryGetValueFix(language, out lang) && 
                lang.TryGetValueFix(key, out translation)&& !string.IsNullOrWhiteSpace(translation)) return true;
            OnMissingTranslation?.Invoke(language,key);
            translation = key;
            return false;
        }

        /// <summary>
        /// This method is used to get a description for the given key if it exists.
        /// </summary>
        /// <param name="key">The key that you want to get the description for.</param>
        /// <returns>The description of the key if it exists, otherwise empty string.</returns>
        public static string GetDescription(string key) {
            if(KeyData.TryGetValueFix(key, out var keyInfo)) return keyInfo.Description;
            OnUnknownKey?.Invoke(key);
            return string.Empty;
        }

        /// <summary>
        /// This method is used to try get the description of the given key.
        /// </summary>
        /// <param name="key">The key that you want to get the description of.</param>
        /// <param name="description">The description of the key.</param>
        /// <returns>True if the key has a description, otherwise false.</returns>
        public static bool TryGetDescription(string key, out string description) {
            description = string.Empty;
            if(!KeyData.TryGetValueFix(key, out var keyInfo)) {
                OnUnknownKey?.Invoke(key);
                return false;
            }
            description = keyInfo.Description;
            return keyInfo.HasDescription;
        }

        public static bool TryGetTranslationData(string key, out KeyInfo keyInfo,
            out Dictionary<string, string> translations) {
            translations = new Dictionary<string, string>();
            foreach(var lang in LanguageNames) {
                var value = string.Empty;
                if(Data.TryGetValueFix(lang, out var data) && data.TryGetValue(key, out var v)) {
                    value = v;
                }
                translations.Add(lang,value);
            }
            return KeyData.TryGetValueFix(key, out keyInfo);
        }

        public static bool TryGetKeyInfo(string key, out KeyInfo keyInfo) {
            return KeyData.TryGetValueFix(key, out keyInfo);
        }
        
        #if UNITY_EDITOR 
        [UnityEditor.MenuItem("Amilious/Localization/Reload Data")] 
        #endif
        public static void ReloadData() {
            KeyData.Clear();
            Data.Clear();
            OverrideData.Clear();
            Loaded.Clear();
            //load starting values
            LoadLanguageNames();
            LoadKeys();
            #if UNITY_EDITOR //set the checks
            foreach(var language in LanguageNames) LoadLanguage(language);
            #endif
        }

        /// <summary>
        /// This method is used to pause usage counting.
        /// </summary>
        public static void PauseCounting() => _countingPaused = true;

        /// <summary>
        /// This method is used to resume usage counting.
        /// </summary>
        public static void ResumeCounting() => _countingPaused = false;
        
        /// <summary>
        /// This method is used to get the count of the given key.
        /// </summary>
        /// <param name="key">The key that you want to get the count for.</param>
        /// <returns>The count of the given key.</returns>
        public static int GetUsageCount(string key) => KeyUsage.TryGetValueFix(key, out var count) ? count : 0;

        /// <summary>
        /// This method is used to clear the count of the given key.
        /// </summary>
        /// <param name="key">The key that you want to clear the count of.</param>
        public static void ClearCount(string key) {
            if(!KeyUsage.ContainsKey(key)) return;
            KeyUsage[key] = 0;
            OnUsageCountChanged?.Invoke(key,0);
        }

        /// <summary>
        /// This method is used to clear the count for all keys.
        /// </summary>
        public static void ClearAllCount() {
            var keys = KeyUsage.Keys.ToArray();
            foreach(var key in keys) {
                KeyUsage[key] = 0;
                OnUsageCountChanged?.Invoke(key,0);
            }
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Private Methods ////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This method is used to add to the usage count.
        /// </summary>
        /// <param name="key">They key that you want to add to the count of.</param>
        private static void AddToCount(string key) {
            if(_countingPaused||!Application.isEditor) return;
            if(!KeyUsage.TryGetValueFix(key,out var count))count = 0;
            count++;
            KeyUsage[key] = count;
            OnUsageCountChanged?.Invoke(key,count);
        }
        
        /// <summary>
        /// This method is used to get all of the language names.
        /// </summary>
        /// <returns>A list containing all of the language names.</returns>
        private static void LoadLanguageNames() {
            LanguageNames.Clear();
            //get the names from the resources
            // ReSharper disable once Unity.UnknownResource
            foreach(var asset in Resources.LoadAll<TextAsset>("Languages/")){
                if(!asset.name.Equals("keys",StringComparison.OrdinalIgnoreCase)&&!LanguageNames.Contains(asset.name)) 
                    LanguageNames.Add(asset.name);
                Resources.UnloadAsset(asset);
            }
            //get the names from the StreamingAssets
            foreach(var asset in FileHelper.GetStreamingAssetsPaths("Languages","*.csv")
                .Select(Path.GetFileNameWithoutExtension).Where(x => x != "Keys")) {
                if(!asset.Equals("keys",StringComparison.OrdinalIgnoreCase)&&!LanguageNames.Contains(asset)) 
                    LanguageNames.Add(asset);
            }
            OnLanguagesUpdated?.Invoke();
        }

        /// <summary>
        /// This method is used to load a language based on its name.
        /// </summary>
        /// <param name="language">The language that you want to load.</param>
        private static void LoadLanguage(string language) {
            //load default values for the language
            if(!Data.ContainsKey(language)) Data[language] = new Dictionary<string, string>();
            var fileData = Data[language];
            fileData.Clear(); //clear any existing data before loading
            foreach(var asset in Resources.LoadAll<TextAsset>($"Languages/{language}")) {
                if(!CsvHelper.TryLoadKeyValuePairs(asset, out var values, true))
                    continue;
                foreach(var keyValue in values) { //only load values for valid keys
                    if(KeyData.ContainsKey(keyValue.Key)) fileData[keyValue.Key] = keyValue.Value;
                }
            }
            //load override values for the language in the StreamingAssets folder
            if(!OverrideData.ContainsKey(language)) OverrideData[language] = new Dictionary<string, string>();
            var overrideData = OverrideData[language];
            overrideData.Clear();
            var path = Path.Combine(Application.streamingAssetsPath, "Languages", language + ".csv");
            if(File.Exists(path)&&CsvHelper.TryLoadKeyValuePairs(path, out var values2, true)) {
                foreach(var keyValue in values2) { //only load values for valid keys
                    if(KeyData.ContainsKey(keyValue.Key)) overrideData[keyValue.Key] = keyValue.Value;
                }
            }
            //display the loading result
            if(LogLoad) Log("The {0} language has been loaded with {1} {2}!",language,
                Data[language].Count,Data[language].Count==1?"value":"values");
            if(!Loaded.Contains(language))Loaded.Add(language);
            OnLanguageLoaded?.Invoke(language);
            OnLanguagesUpdated?.Invoke();
        }

        /// <summary>
        /// This method is used to unload a language based on its name.
        /// </summary>
        /// <param name="language">The language that you want to unload.</param>
        private static void UnloadLanguage(string language) {
            //if running in the editor or not loaded return
            if(Application.isEditor || (!Data.ContainsKey(language)&&!OverrideData.ContainsKey(language))) return;
            if(language == FallbackLanguage||language == CurrentLanguage) return; //do not unload the fallback language.
            Data.Remove(language);
            OverrideData.Remove(language);
            if(!Loaded.Contains(language))Loaded.Remove(language);
            if(LogUnload) Log("{0}Unloaded the {0} language!",language);
            OnLanguageUnloaded?.Invoke(language);
            OnLanguagesUpdated?.Invoke();
        }
        
        /// <summary>
        /// This method is used to load the keys.
        /// </summary>
        private static void LoadKeys() {
            KeyData.Clear();
            KeyPaths.Clear();
            //read all Key files within a Resources/Language/ folder 
            foreach(var asset in Resources.LoadAll<TextAsset>("Languages/Keys")) {
                #if UNITY_EDITOR
                if(LogLoad) Log("loading {0}",asset.GetAssetPath());
                #endif
                if(CsvHelper.TryLoadKeyValuePairs(asset, out var values)) {
                    foreach(var keyValue in values)
                        KeyData[keyValue.Key] = new KeyInfo(asset, keyValue.Key, keyValue.Value);
                }
                Resources.UnloadAsset(asset); //unload the asset to clear up memory
                #if UNITY_EDITOR
                var key = FileHelper.TrimEndDirectories(asset.GetAssetPath(), 3);
                key = key.Replace("/", "|").Replace("\\", "|");
                if(string.IsNullOrWhiteSpace(key)) key = "Root";
                KeyPaths[key] = FileHelper.CreatePath(Application.dataPath,asset.GetAssetPath());
                #endif
            }
            OnKeyPathsUpdate?.Invoke();
            OnKeysUpdated?.Invoke();
            if(LogLoad) Log("{0} {1} been loaded.",KeyData.Count,KeyData.Count==1?"key has":"keys have");
        }
        
        /// <summary>
        /// This method is used to log a message.
        /// </summary>
        /// <param name="format">The format of the message that you want to log.</param>
        /// <param name="args">The message parameters for the log.</param>
        private static void Log(string format, params object[] args) {
            Debug.LogFormat(ZString.Format("{0}{1}",MESSAGE_HEADER,format),args);
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}