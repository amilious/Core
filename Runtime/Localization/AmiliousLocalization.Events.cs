using System;

namespace Amilious.Core.Localization {

    public static partial class AmiliousLocalization {

        #region Delegates //////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This method is called when the selected language changes.
        /// </summary>
        /// <param name="previous">The previous language.</param>
        /// <param name="current">The current language.</param>
        public delegate void LanguageChangedDelegate(string previous, string current);

        /// <summary>
        /// This method is called when a translation changes.
        /// </summary>
        /// <param name="language">The language of the value that was changed.</param>
        /// <param name="key">The key of the value that changed.</param>
        public delegate void TranslationUpdatedDelegate(string language, string key);

        /// <summary>
        /// This method is called when a key description is updated.
        /// </summary>
        /// <param name="key">The key that was updated.</param>
        public delegate void KeyDescriptionUpdatedDelegate(string key);

        /// <summary>
        /// This method is called when a key is added.
        /// </summary>
        /// <param name="key">The key that was added.</param>
        public delegate void KeyAddedDelegate(string key);

        /// <summary>
        /// This method is called when a key is removed.
        /// </summary>
        /// <param name="key">The key that was removed.</param>
        public delegate void KeyRemovedDelegate(string key);

        /// <summary>
        /// This method is called when a language is loaded.
        /// </summary>
        /// <param name="language">The language that was loaded.</param>
        public delegate void LanguageLoadedDelegate(string language);

        /// <summary>
        /// This method is called when a language is unloaded.
        /// </summary>
        /// <param name="language">The language that was unloaded.</param>
        public delegate void LanguageUnloadedDelegate(string language);

        /// <summary>
        /// This method is called when an unknown key is used.
        /// </summary>
        /// <param name="key">The unknown key that was used.</param>
        public delegate void UnknownKeyDelegate(string key);
        
        /// <summary>
        /// This method is called when a language is missing a translation.
        /// </summary>
        /// <param name="language">The language that is missing the translation.</param>
        /// <param name="key">The key that has a missing translation.</param>
        public delegate void MissingTranslationDelegate(string language, string key);

        /// <summary>
        /// This method is called when a key is moved from one location to another.
        /// </summary>
        /// <param name="key">The key that was moved.</param>
        /// <param name="keyInfo">The new key info.</param>
        public delegate void KeyMovedDelegate(string key, KeyInfo keyInfo);

        /// <summary>
        /// This method is called when the count of a key changes.
        /// </summary>
        /// <param name="key">The key whose usage count changed.</param>
        /// <param name="count">The current usage count of the key.</param>
        public delegate void UsageCountChangedDelegate(string key, int count);

        public delegate void ToggleUpdatedDelegate(bool value);
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Events /////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This event is triggered when a key is moved.
        /// </summary>
        public static event KeyMovedDelegate OnKeyMoved;
        
        /// <summary>
        /// This event is called when the selected language changes.
        /// </summary>
        public static event LanguageChangedDelegate OnLanguageChanged;

        /// <summary>
        /// This event is triggered when a translation is updated.
        /// </summary>
        public static event TranslationUpdatedDelegate OnTranslationUpdated;

        /// <summary>
        /// This event is triggered when a key's description is updated.
        /// </summary>
        public static event KeyDescriptionUpdatedDelegate OnKeyDescriptionUpdated;
        
        /// <summary>
        /// This event is triggered when the keys are updated.
        /// </summary>
        public static event Action OnKeysUpdated;

        /// <summary>
        /// This event is triggered when the languages are updated.
        /// </summary>
        public static event Action OnLanguagesUpdated;

        /// <summary>
        /// This event is triggered when a key is added.
        /// </summary>
        public static event KeyAddedDelegate OnKeyAdded;

        /// <summary>
        /// This event is triggered when a key is removed.
        /// </summary>
        public static event KeyRemovedDelegate OnKeyRemoved;

        /// <summary>
        /// This event is triggered when a language is loaded.
        /// </summary>
        public static event LanguageLoadedDelegate OnLanguageLoaded;

        /// <summary>
        /// This event is triggered when a language is unloaded.
        /// </summary>
        public static event LanguageUnloadedDelegate OnLanguageUnloaded;

        /// <summary>
        /// This event is triggered when an unknown key is used.
        /// </summary>
        public static event UnknownKeyDelegate OnUnknownKey;

        /// <summary>
        /// This event is triggered when trying to get a translation for a language and not having a value.
        /// </summary>
        public static event MissingTranslationDelegate OnMissingTranslation;

        /// <summary>
        /// This event is triggered when key paths are updated.
        /// </summary>
        public static event Action OnKeyPathsUpdate;

        /// <summary>
        /// This event is triggered when a key's usage count is updated.
        /// </summary>
        public static event UsageCountChangedDelegate OnUsageCountChanged;

        public static event ToggleUpdatedDelegate OnShowUsageToggled;

        public static event ToggleUpdatedDelegate OnShowPathToggled;

        public static event ToggleUpdatedDelegate OnShowDescriptionToggled;

        public static event ToggleUpdatedDelegate OnShowTranslationToggled;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////


    }

}