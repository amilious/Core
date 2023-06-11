
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Amilious.Core.Extensions;
using Amilious.Core.Localization;
using System.Collections.Generic;
using Amilious.Core.Editor.Extensions;
using Amilious.Core.Editor.VisualElements;

namespace Amilious.Core.Editor.Windows {
    
    public class LocalizationEntryWindow : EditorWindow {

        #region Constants //////////////////////////////////////////////////////////////////////////////////////////////
        
        private const string KEY = "Key";
        private const string KEY_LABEL = "KeyLabel";
        private const string ADD_SAVE = "AddSave";
        private const string VALIDATION_FIELD = "ValidationField";
        private const string TRANSLATION_FIELD = "TranslationField";
        private const string DESCRIPTION_FIELD = "DescriptionField";
        private const string RESTORE_KEY_BUTTON = "RestoreKeyButton";
        private const string LANGUAGE_SELECTOR = "LanguageSelector";
        private const string KEY_PATH_SELECTOR = "KeyPathSelector";
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region PrivateFields //////////////////////////////////////////////////////////////////////////////////////////

        private bool _initialized;
        private bool _settingLanguage;
        private string _key;
        private string _path;
        private string _language;
        private bool _adding;
        private KeyInfo _keyInfo;
        private string _description;
        private Dictionary<string, string> _translations;
        private static Texture Texture;
        private LocalizedGroup _localGroup;
        private static VisualTreeAsset WindowAsset;
        
        private TextField _keyField;
        private RawFormattedTextBox _translationField;
        private RawFormattedTextBox _descriptionField;
        private LanguageSelector _languageSelector;
        private KeyPathSelector _pathSelector;
        private Label _validationField;
        private Label _keyLabel;
        private Button _actionButton;
        private Button _restoreKeyButton;
        //private TextField _translationField;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////
        
        public void CreateGUI() {
            _initialized = false;
            LoadValues();
            Initialize();
            if(_adding) {
                _descriptionField.EditMode = true;
                _translationField.EditMode = true;
            }
            //add button triggers
            _actionButton.clicked -= OnActionButtonClicked;
            _actionButton.clicked += OnActionButtonClicked;
            _restoreKeyButton.clicked -= OnRestoreKeyButtonClicked;
            _restoreKeyButton.clicked += OnRestoreKeyButtonClicked;
            //update text
            _languageSelector.Value = _language;
            _keyField.value = _key;
            _descriptionField.Value = _description;
            _translationField.Value = _translations[_language];
            AmiliousLocalization.OnKeysUpdated -= KeysUpdated;
            AmiliousLocalization.OnKeysUpdated += KeysUpdated;
            AmiliousLocalization.OnLanguageChanged -= LanguageChanged;
            AmiliousLocalization.OnLanguageChanged += LanguageChanged;
            AmiliousLocalization.OnTranslationUpdated -= TranslationUpdated;
            AmiliousLocalization.OnTranslationUpdated += TranslationUpdated;
            _languageSelector.OnValueChanged -= OnLanguageChanged;
            _languageSelector.OnValueChanged += OnLanguageChanged;
            _keyField.UnregisterValueChangedCallback(OnKeyValueChanged);
            _keyField.RegisterValueChangedCallback(OnKeyValueChanged);
            UpdateLocalizationText();
        }

        private void OnRestoreKeyButtonClicked() {
            _keyField.value = _key;
            ValidateKey();
        }

        private void OnKeyValueChanged(ChangeEvent<string> evt) => ValidateKey();

        private void ValidateKey() {
            var key = _keyField.text;
            if(key == _key) {
                _validationField.text = string.Empty;
                _actionButton.SetEnabled(true);
            } else {
                if(AmiliousLocalization.HasKey(key)) {
                    _validationField.text = "<color=red>already exists</color>";
                    _actionButton.SetEnabled(false);
                }else {
                    _validationField.text = "<color=green>new key name available</color>";
                    _actionButton.SetEnabled(true);
                }  
            }
            
            
        }

        private void OnLanguageChanged(string language) {
            _translations[_language] = _translationField.Value;
            if(!_translations.TryGetValueFix(language, out var translation))
                _translations[language] = translation = string.Empty;
            _translationField.Value = translation;
            _language = language;
        }

        private void UpdateLocalizationText() {
            Initialize();
            _actionButton.text = _adding ? _localGroup[E.LOCAL_ENTRY_ADD_BUTTON] : 
                _localGroup[E.LOCAL_ENTRY_SAVE_BUTTON];
            _descriptionField.Label = _localGroup[E.LOCAL_ENTRY_DESCRIPTION_LABEL];
            _translationField.Label = _localGroup[E.LOCAL_ENTRY_TRANSLATION_LABEL];
            _keyLabel.text = _localGroup[E.LOCAL_ENTRY_KEY_LABEL];
        }
        
        private void KeysUpdated() {
            _adding = !AmiliousLocalization.HasKey(_key);
            UpdateLocalizationText();
        }

        private void LanguageChanged(string previous, string current) => UpdateLocalizationText();

        private void TranslationUpdated(string language, string key) {
            if(_localGroup.IsInGroup(key)) UpdateLocalizationText();
            if(key != _key||_settingLanguage) return;
            AmiliousLocalization.PauseCounting();
            AmiliousLocalization.TryGetTranslation(_key, language, out var trans, false);
            AmiliousLocalization.ResumeCounting();
            _translations[language] = trans;
            if(_language != language) return;
            _translationField.Value = trans;
        }

        public static void Open(string key, string keyPath = null) {
            var window = CreateInstance<LocalizationEntryWindow>();
            Texture ??= Resources.Load<Texture>("Icons/translation");
            window.titleContent = new GUIContent("Amilious Localization Entry",Texture);
            window._key = key;
            if(keyPath == null&&AmiliousLocalization.TryGetKeyInfo(key, out var keyInfo)) {
                keyPath = keyInfo.Path;
            }
            window._path = keyPath;
            window.ShowModal();
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
       
        #region Button Clicks //////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is called when the action button is clicked.
        /// </summary>
        private void OnActionButtonClicked() {
            //fix values
            _translations[_language] = _translationField.Value;
            var updateDescription = _description != _descriptionField.Value;
            if(updateDescription)_description = _descriptionField.Value;
            //update values
            if(_adding) {
                _key = _keyField.value;
                AmiliousLocalization.AddKey(_key,_description,_path);
            }else {
                if(_keyField.value != _key) {
                    var tmp = new Dictionary<string, string>(_translations);
                    AmiliousLocalization.RemoveKey(_key);
                    _key = _keyField.value;
                    _translations = tmp;
                    AmiliousLocalization.AddKey(_key,_description,_path);
                }
                if(AmiliousLocalization.GetLocationName(_path)!=_pathSelector.Value)
                    AmiliousLocalization.TryMoveKey(_key, _pathSelector.Value);
                if(updateDescription)AmiliousLocalization.TryEditDescription(_key,_description);
            }
            _settingLanguage = true;
            foreach(var lang in _translations.Keys)
                AmiliousLocalization.TryAddOrEditTranslation(lang, _key, _translations[lang]);
            _settingLanguage = false;
            EditorApplication.QueuePlayerLoopUpdate();
            SceneView.RepaintAll();
            Close();
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Private Methods ////////////////////////////////////////////////////////////////////////////////////////

        private void OnDestroy() {
            AmiliousLocalization.OnKeysUpdated -= KeysUpdated;
            AmiliousLocalization.OnLanguageChanged -= LanguageChanged;
            AmiliousLocalization.OnTranslationUpdated -= TranslationUpdated;
        }

        private void LoadValues() {
            _language = AmiliousLocalization.CurrentLanguage;
            AmiliousLocalization.PauseCounting();
            _adding = !AmiliousLocalization.TryGetTranslationData(_key, out _keyInfo, out _translations);
            AmiliousLocalization.ResumeCounting();
            _description = _keyInfo?.Description ?? string.Empty;
        }

        private void Initialize() {
            if(_initialized) return;
            _initialized = true;
            //get the visual element
            this.CloneAssetTree();
            _localGroup = new LocalizedGroup(E.LOCAL_ENTRY_GROUP);
            //get the fields
            this.Q(KEY, out _keyField);
            this.Q(KEY_LABEL, out _keyLabel);
            this.Q(DESCRIPTION_FIELD, out _descriptionField);
            this.Q(VALIDATION_FIELD, out _validationField);
            this.Q(TRANSLATION_FIELD, out _translationField);
            this.Q(ADD_SAVE, out _actionButton);
            this.Q(RESTORE_KEY_BUTTON, out _restoreKeyButton);
            this.Q(LANGUAGE_SELECTOR, out _languageSelector);
            this.Q(KEY_PATH_SELECTOR, out _pathSelector);
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
    
}