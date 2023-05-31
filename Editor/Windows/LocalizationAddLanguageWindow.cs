using UnityEditor;
using UnityEngine;
using Amilious.Core.IO;
using UnityEngine.UIElements;
using Amilious.Core.Localization;
using Amilious.Core.Editor.Extensions;

namespace Amilious.Core.Editor.Windows {
    
    public class LocalizationAddLanguageWindow : EditorWindow {
        
        private const string ADD = "Add";
        private const string CANCEL = "Cancel";
        private const string MESSAGE = "Message";
        private const string LANGUAGE = "Language";
        private const string VALIDATION = "Validation";
        private const string LOCALIZATION_GROUP = "amilious/localization/editor/";

        private bool _invalid;
        private string _invalidText;
        private bool _initialized;
        private Button _addButton;
        private Label _messageLabel;
        private Button _cancelButton;
        private Label _validationLabel;
        private TextField _languageField;
        private LocalizedGroup _localGroup;
        
        [MenuItem("Amilious/Localization/Add Language")]
        public static void Open() {
            var window = CreateWindow<LocalizationAddLanguageWindow>();
            window.ShowModal();
        }
        
        public void CreateGUI() {
            _initialized = false;
            Initialize();
            UpdateLocalizationText();
            Validate();
        }

        private void UpdateLocalizationText() {
            Initialize();
            _addButton.text = _localGroup["add_language"];
            _cancelButton.text = _localGroup["cancel"];
            _messageLabel.text = _localGroup["add_message"];
            _languageField.label = _localGroup["language_name"];
            _invalidText = _localGroup["language_exists"];
            if(_invalid) _validationLabel.text = _invalidText;
        }

        private void Initialize() {
            if(_initialized) return;
            _initialized = true;
            this.CloneAssetTree();
            _localGroup = new LocalizedGroup(LOCALIZATION_GROUP);
            var texture = Resources.Load<Texture>("Icons/translation");
            titleContent = new GUIContent("Amilious Localization",texture);
            this.Q(MESSAGE, out _messageLabel);
            this.Q(VALIDATION, out _validationLabel);
            this.Q(ADD, out _addButton);
            this.Q(CANCEL, out _cancelButton);
            this.Q(LANGUAGE, out _languageField);
            _addButton.clicked -= AddButtonClicked;
            _addButton.clicked += AddButtonClicked;
            _cancelButton.clicked -= CancelButtonClicked;
            _cancelButton.clicked += CancelButtonClicked;
            _languageField.UnregisterValueChangedCallback(LanguageValueChanged);
            _languageField.RegisterValueChangedCallback(LanguageValueChanged);
            AmiliousLocalization.OnLanguageChanged -= LanguageChanged;
            AmiliousLocalization.OnLanguageChanged -= LanguageChanged;
            AmiliousLocalization.OnTranslationUpdated -= TranslationUpdated;
            AmiliousLocalization.OnTranslationUpdated += TranslationUpdated;
            AmiliousLocalization.OnLanguagesUpdated -= Validate;
            AmiliousLocalization.OnLanguagesUpdated += Validate;
        }

        private void TranslationUpdated(string language, string key) {
            if(_localGroup.IsInGroup(key)) UpdateLocalizationText();
        }

        private void LanguageChanged(string previous, string current) => UpdateLocalizationText();

        private void Validate() {
            var text = _languageField.text;
            if(AmiliousLocalization.HasLanguage(text, true)) {
                _invalid = true;
                _validationLabel.text = _invalidText;
                _addButton.SetEnabled(false);
                return;
            }
            text = text.Replace(" ", "");
            if(string.IsNullOrWhiteSpace(text)) {
                _languageField.value = string.Empty;
                _addButton.SetEnabled(false);
                return;
            }
            text = FileHelper.RemoveInvalidFileNameCharacters(text);
            _languageField.value = text;
            _addButton.SetEnabled(true);
        }

        private void LanguageValueChanged(ChangeEvent<string> evt) => Validate();

        private void AddButtonClicked() {
            AmiliousLocalization.TryAddLanguage(_languageField.value);
            Close();
        }

        private void CancelButtonClicked() => Close();

    }
    
    
}