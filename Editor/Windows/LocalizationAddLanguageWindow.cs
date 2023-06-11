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

using UnityEditor;
using UnityEngine;
using Amilious.Core.IO;
using UnityEngine.UIElements;
using Amilious.Core.Localization;
using Amilious.Core.Editor.Extensions;

namespace Amilious.Core.Editor.Windows {
    
    /// <summary>
    /// This class is use to create a window for adding a localization window.
    /// </summary>
    public class LocalizationAddLanguageWindow : EditorWindow {
        
        #region Constants //////////////////////////////////////////////////////////////////////////////////////////////
        
        private const string ADD = "Add";
        private const string CANCEL = "Cancel";
        private const string MESSAGE = "Message";
        private const string LANGUAGE = "Language";
        private const string VALIDATION = "Validation";

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Private Fields /////////////////////////////////////////////////////////////////////////////////////////
        
        private bool _invalid;
        private string _invalidText;
        private bool _initialized;
        private Button _addButton;
        private Label _messageLabel;
        private Button _cancelButton;
        private Label _validationLabel;
        private TextField _languageField;
        private LocalizedGroup _localGroup;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to display the add language dialogue.
        /// </summary>
        public static void Open() {
            var window = CreateWindow<LocalizationAddLanguageWindow>();
            window.ShowModal();
        }
        
        /// <summary>
        /// This method is called when the unity editor creates the GUI.
        /// </summary>
        public void CreateGUI() {
            _initialized = false;
            Initialize();
            UpdateLocalizationText();
            Validate();
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Private Methods ////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is called when the localization text is updated.
        /// </summary>
        private void UpdateLocalizationText() {
            Initialize();
            _addButton.text = _localGroup[E.ADD_LANG_ADD_BUTTON];
            _cancelButton.text = _localGroup[E.ADD_LANG_CANCEL_BUTTON];
            _messageLabel.text = _localGroup[E.ADD_LANG_MESSAGE];
            _languageField.label = _localGroup[E.ADD_LANG_LANGUAGE_LABEL];
            _invalidText = _localGroup[E.ADD_LANG_INVALID_TEXT];
            if(_invalid) _validationLabel.text = _invalidText;
        }

        /// <summary>
        /// This method is used to initialize the window.
        /// </summary>
        private void Initialize() {
            if(_initialized) return;
            _initialized = true;
            this.CloneAssetTree();
            _localGroup = new LocalizedGroup(E.ADD_LANG_GROUP);
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

        /// <summary>
        /// This method is called when a translation is updated.
        /// </summary>
        /// <param name="language">The language that updated.</param>
        /// <param name="key">The key that updated.</param>
        private void TranslationUpdated(string language, string key) {
            //check if the key is present in the group and update text if it is.
            if(_localGroup.IsInGroup(key)) UpdateLocalizationText();
        }

        /// <summary>
        /// This method is called when the current language changes.
        /// </summary>
        /// <param name="previous">The previous language.</param>
        /// <param name="current">The current language.</param>
        private void LanguageChanged(string previous, string current) => UpdateLocalizationText();

        /// <summary>
        /// This method is used to validate the currently entered language name.
        /// </summary>
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

        /// <summary>
        /// This method is called when the typed language name changes.
        /// </summary>
        private void LanguageValueChanged(ChangeEvent<string> evt) => Validate();

        /// <summary>
        /// This method is called when the add button is clicked.
        /// </summary>
        private void AddButtonClicked() {
            AmiliousLocalization.TryAddLanguage(_languageField.value);
            Close();
        }

        /// <summary>
        /// This method is called when the cancel button is clicked.
        /// </summary>
        private void CancelButtonClicked() => Close();

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }

}