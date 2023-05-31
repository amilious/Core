
using UnityEditor;
using UnityEngine;
using System.Linq;
using UnityEngine.UIElements;
using Amilious.Core.Localization;
using System.Collections.Generic;
using Amilious.Core.Editor.Extensions;
using Amilious.Core.Extensions;
using Amilious.Core.Editor.VisualElements;

namespace Amilious.Core.Editor.Windows {
    
    public class LocalizationEntryWindow : EditorWindow {

        #region Constants //////////////////////////////////////////////////////////////////////////////////////////////
        
        private const string KEY = "Key";
        private const string KEY_LABEL = "KeyLabel";
        private const string ADD_SAVE = "AddSave";
        private const string LOCATION_LABEL = "LocationLabel";
        private const string LOCATION = "LocationDropdown";
        private const string LANGUAGE = "Language";
        private const string DESCRIPTION = "Description";
        private const string DESCRIPTION_LABEL = "DescriptionLabel";
        private const string TRANSLATION = "Translation";
        private const string TRANSLATION_LABEL = "TranslationLabel";
        private const string LOCALIZATION_GROUP = "amilious/localization/editor/";
        private const string KEY_VALIDATION_ELEMENT = "KeyValidationElement";
        private const string VALIDATION_FIELD = "ValidationField";
        private const string ASSET_PATH = "Assets/Amilious/Core/Editor/Windows/LocalizationEntryWindow.uxml";
        
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
        private Label _locationLabel;
        private LanguageSelector _languageSelector;
        private Label _validationField;
        private VisualElement _keyValidationElement;
        private Label _keyLabel;
        private Label _descriptionLabel;
        private Label _translationLabel;
        private Button _actionButton;
        private TextField _descriptionField;
        private TextField _translationField;
        private DropdownField _locationDropdown;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////
        
        public void CreateGUI() {
            _initialized = false;
            LoadValues();
            Initialize(); 
            //add button triggers
            _actionButton.clicked += OnActionButtonClicked;
            //update text
            _languageSelector.Value = _language;
            _keyField.value = _key;
            _keyField.SetEnabled(false);
            _locationDropdown.value = AmiliousLocalization.GetLocationName(_path);
            _descriptionField.value = _description;
            _translationField.value = _translations[_language];
            AmiliousLocalization.OnKeysUpdated -= KeysUpdated;
            AmiliousLocalization.OnLanguageChanged -= LanguageChanged;
            AmiliousLocalization.OnTranslationUpdated -= TranslationUpdated;
            AmiliousLocalization.OnKeysUpdated += KeysUpdated;
            AmiliousLocalization.OnLanguageChanged += LanguageChanged;
            AmiliousLocalization.OnTranslationUpdated += TranslationUpdated;
            _languageSelector.OnValueChanged -= OnLanguageChanged;
            _languageSelector.OnValueChanged += OnLanguageChanged;
            UpdateLocalizationText();
        }

        private void OnLanguageChanged(string language) {
            _translations[_language] = _translationField.value;
            if(!_translations.TryGetValueFix(language, out var translation))
                _translations[language] = translation = string.Empty;
            _translationField.value = translation;
            _language = language;
        }

        private void UpdateLocalizationText() {
            Initialize();
            if(_adding) {
                _actionButton.text = _localGroup["add_button"];
                _locationDropdown.value = AmiliousLocalization.GetLocationName(_path);
            } else {
                _actionButton.text = _localGroup["save_button"];
                _locationDropdown.value = AmiliousLocalization.GetLocationName(_path);
            }
            _locationLabel.text = _localGroup["location"];
            _descriptionLabel.text = _localGroup["description"];
            _translationLabel.text = _localGroup["translation"];
            _keyLabel.text = _localGroup["key"];
            _locationDropdown.choices = AmiliousLocalization.KeyFileNames.ToList();
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
            _translationField.value = trans;
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
            _translations[_language] = _translationField.value;
            var updateDescription = _description != _descriptionField.value;
            if(updateDescription)_description = _descriptionField.value;
            //update values
            if(_adding) AmiliousLocalization.AddKey(_key,_description,_path);
            else {
                if(AmiliousLocalization.GetLocationName(_path)!=_locationDropdown.value)
                    AmiliousLocalization.TryMoveKey(_key, _locationDropdown.value);
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
            _localGroup = new LocalizedGroup(LOCALIZATION_GROUP);
            //get the fields
            _keyField = rootVisualElement.Q<TextField>(KEY);
            _keyLabel = rootVisualElement.Q<Label>(KEY_LABEL);
            _locationLabel = rootVisualElement.Q<Label>(LOCATION_LABEL);
            _descriptionLabel = rootVisualElement.Q<Label>(DESCRIPTION_LABEL);
            _translationLabel = rootVisualElement.Q<Label>(TRANSLATION_LABEL);
            _descriptionField = rootVisualElement.Q<TextField>(DESCRIPTION);
            _keyValidationElement = rootVisualElement.Q<VisualElement>(KEY_VALIDATION_ELEMENT);
            _validationField = rootVisualElement.Q<Label>(VALIDATION_FIELD);
            
            var holder = rootVisualElement.Q<VisualElement>("LanguageSelectorHolder");
            _languageSelector = new LanguageSelector(_language);
            holder.Clear();
            holder.Add(_languageSelector);
            
            //_languageSelector = rootVisualElement.Q<LanguageSelector>("LanguageSelector");
            if(_languageSelector== null) Debug.Log("no inspector");
            _translationField = rootVisualElement.Q<TextField>(TRANSLATION);
            _actionButton = rootVisualElement.Q<Button>(ADD_SAVE);
            _locationDropdown = rootVisualElement.Q<DropdownField>(LOCATION);
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
    
}