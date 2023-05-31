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
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using Amilious.Core.Extensions;
using Amilious.Core.Localization;
using Amilious.Core.Editor.Windows;
using Amilious.Core.Editor.Extensions;

namespace Amilious.Core.Editor.VisualElements {
    
    /// <summary>
    /// This element is used to display the localization information.
    /// </summary>
    public class LocalizationInformation : VisualElement {

        #region Constants //////////////////////////////////////////////////////////////////////////////////////////////
        
        private const string PATH_ELEMENT = "PathElement";
        private const string KEY_VALUE = "KeyValue";
        private const string PATH_LABEL = "PathLabel";
        private const string PATH_VALUE = "PathValue";
        private const string COPY_BUTTON = "CopyButton";
        private const string COPY_KEY_BUTTON = "CopyKeyButton";
        private const string EDIT_BUTTON = "EditButton";
        private const string MENU_BUTTON = "MenuButton";
        private const string DESC_LABEL = "DescriptionLabel";
        private const string DESC_VALUE = "DescriptionValue";
        private const string TRANS_LABEL = "TranslationLabel";
        private const string TRANS_VALUE = "TranslationValue";
        private const string USAGE_FIELD = "UsageField";
        private const string TRANSLATION_ELEMENT = "TranslationElement";
        private const string DESCRIPTION_ELEMENT = "DescriptionElement";
        private const string USAGE_ELEMENT = "UsageElement";
        private const string TRANSLATION_INFO = "TranslationInformation";
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Private Fields /////////////////////////////////////////////////////////////////////////////////////////
        
        private string _key;
        private string _language;
        private bool _initialized;
        private Label _keyValue;
        private Label _pathLabel;
        private Label _pathValue;
        private Label _descriptionLabel;
        private Label _descriptionValue;
        private Label _translationLabel;
        private Label _translationValue;
        private Label _usageField;
        private ToolbarMenu _menuButton;
        private ToolbarButton _copyButton;
        private ToolbarButton _copyKeyButton;
        private ToolbarButton _editButton;
        private VisualElement _pathElement;
        private VisualElement _usageElement;
        private static VisualTreeAsset Asset;
        private LocalizedGroup _localGroup;
        private VisualElement _descriptionElement;
        private VisualElement _translationElement;
        private VisualElement _translationInformation;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Delegates //////////////////////////////////////////////////////////////////////////////////////////////
        
        public delegate void RequestSearchDelegate(string query);
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Events /////////////////////////////////////////////////////////////////////////////////////////////////
        
        public static event RequestSearchDelegate OnRequestSearch;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This property is used to get or privately set the key.
        /// </summary>
        public string Key {
            get => _key;
            private set {
                _keyValue.text = value;
                _translationInformation.tooltip = value;
                //_keyValue.tooltip = value;
                _key = value;
            }
        }

        /// <summary>
        /// This property is used to get or privately set the key path.
        /// </summary>
        public string Path {
            get => _pathValue.text; 
            private set => _pathValue.text = value;
        }
        
        /// <summary>
        /// This property is used to get or privately set the key description.
        /// </summary>
        public string Description {
            get => _descriptionValue.text;
            private set => _descriptionValue.text = value;
        }

        /// <summary>
        /// This property is used to get or privately set the key translation.
        /// </summary>
        public string Translation {
            get => _translationValue.text;
            private set => _translationValue.text = value;
        }
        
        /// <summary>
        /// This property contains the key's group or the entire key.
        /// </summary>
        public string Group { get; private set; }
        
        /// <summary>
        /// This property contains the key's key name or the entire key.
        /// </summary>
        public string KeyName { get; private set; }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region UXML Requirement ///////////////////////////////////////////////////////////////////////////////////////
        
        public new class UxmlFactory : UxmlFactory<LocalizationInformation>{}

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Constructors ///////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This default constructor is used for the UI Builder.
        /// </summary>
        public LocalizationInformation() => Initialize();

        /// <summary>
        /// This constructor is used to create a new localized information instance.
        /// </summary>
        /// <param name="key">The key that the instance is for.</param>
        /// <param name="language">The current display language.</param>
        public LocalizationInformation(string key, string language = null) => Setup(key, language);

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to setup the localization information.
        /// </summary>
        /// <param name="key">The key that the localization is for.</param>
        /// <param name="language">The language that you want to display.</param>
        public void Setup(string key, string language = null) {
            //get references
            Initialize();
            Key = key;
            var last = key.LastIndexOf('/');
            Group = last>0? key.Substring(0, last+1) : key;
            KeyName = last > 0 ? Key.Substring(last + 1) : key;
            language ??= AmiliousLocalization.CurrentLanguage;
            if(!AmiliousLocalization.TryGetKeyInfo(key, out var keyInfo)) return;
            Path = AmiliousLocalization.GetLocationName(keyInfo.Path);
            OnUpdateUsageCount(key, AmiliousLocalization.GetUsageCount(key));
            OnDescriptionUpdated(key);
            SetDisplayLanguage(language);
        }

        private void OnUpdateUsageCount(string key, int count) {
            if(key != _key) return;
            _usageField.text = $"{count}";
        }

        /// <summary>
        /// This method is used to set the language of the translation.
        /// </summary>
        /// <param name="language">The language that you want to display.</param>
        public void SetDisplayLanguage(string language) {
            if(language==_language||!AmiliousLocalization.HasLanguage(language)) return;
            _language = language;
            AmiliousLocalization.PauseCounting();
            var tran = AmiliousLocalization.TryGetTranslation(language, _key, out var translation,false);
            AmiliousLocalization.ResumeCounting();
            if(string.IsNullOrWhiteSpace(translation)||!tran)
                translation = _localGroup["no_translation", language];
            Translation = translation;
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Private Methods ////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to initialize the components.
        /// </summary>
        private void Initialize() {
            if(_initialized) return;
            _initialized = true;
            _localGroup = new LocalizedGroup(E.LOCAL_INFO_GROUP);
            Asset ??= AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(E.LOCAL_INFO_UXML);
            Asset.CloneTree(this);
            //find elements
            this.Q(TRANSLATION_INFO, out _translationInformation);
            this.Q(PATH_ELEMENT, out _pathElement);
            this.Q(DESCRIPTION_ELEMENT, out _descriptionElement);
            this.Q(TRANSLATION_ELEMENT, out _translationElement);
            this.Q(USAGE_ELEMENT, out _usageElement);
            this.Q(KEY_VALUE, out _keyValue);
            this.Q(PATH_VALUE, out _pathValue);
            this.Q(DESC_VALUE, out _descriptionValue);
            this.Q(TRANS_VALUE, out _translationValue);
            this.Q(PATH_LABEL, out _pathLabel);
            this.Q(DESC_LABEL, out _descriptionLabel);
            this.Q(TRANS_LABEL, out _translationLabel);
            this.Q(MENU_BUTTON, out _menuButton);
            this.Q(COPY_BUTTON, out _copyButton);
            this.Q(COPY_KEY_BUTTON, out _copyKeyButton);
            this.Q(EDIT_BUTTON, out _editButton);
            this.Q(USAGE_FIELD, out _usageField);
            //listen to events
            _copyButton.clicked -= CopyFullButtonClicked;
            _copyButton.clicked += CopyFullButtonClicked;
            _copyKeyButton.clicked -= CopyKeyButtonClicked;
            _copyKeyButton.clicked += CopyKeyButtonClicked;
            _editButton.clicked -= EditButtonClicked;
            _editButton.clicked += EditButtonClicked;
            AmiliousLocalization.OnKeyDescriptionUpdated -= OnDescriptionUpdated;
            AmiliousLocalization.OnKeyDescriptionUpdated += OnDescriptionUpdated;
            AmiliousLocalization.OnTranslationUpdated -= OnTranslationUpdated;
            AmiliousLocalization.OnTranslationUpdated += OnTranslationUpdated;
            AmiliousLocalization.OnLanguageChanged -= UpdateLocalizedText;
            AmiliousLocalization.OnLanguageChanged += UpdateLocalizedText;
            AmiliousLocalization.OnKeyMoved -= KeyMoved;
            AmiliousLocalization.OnKeyMoved += KeyMoved;
            AmiliousLocalization.OnUsageCountChanged -= OnUpdateUsageCount;
            AmiliousLocalization.OnUsageCountChanged += OnUpdateUsageCount;
            AmiliousLocalization.OnShowUsageToggled -= ShowUsageToggled;
            AmiliousLocalization.OnShowUsageToggled += ShowUsageToggled;
            AmiliousLocalization.OnShowPathToggled -= ShowPathToggled;
            AmiliousLocalization.OnShowPathToggled += ShowPathToggled;
            AmiliousLocalization.OnShowDescriptionToggled -= ShowDescriptionToggled;
            AmiliousLocalization.OnShowDescriptionToggled += ShowDescriptionToggled;
            AmiliousLocalization.OnShowTranslationToggled -= ShowTranslationToggled;
            AmiliousLocalization.OnShowTranslationToggled += ShowTranslationToggled;
            //set up
            ShowUsageToggled(AmiliousLocalization.ShowUsage);
            ShowPathToggled(AmiliousLocalization.ShowPath);
            ShowDescriptionToggled(AmiliousLocalization.ShowDescription);
            ShowTranslationToggled(AmiliousLocalization.ShowTranslation);
            UpdateLocalizedText();
        }

        private void ShowTranslationToggled(bool showTranslation) {
            _translationElement.style.display = showTranslation ? DisplayStyle.Flex : DisplayStyle.None;
        }

        private void ShowDescriptionToggled(bool showDescription) {
            _descriptionElement.style.display = showDescription ? DisplayStyle.Flex : DisplayStyle.None;
        }

        private void ShowPathToggled(bool showPath) {
            _pathElement.style.display = showPath ? DisplayStyle.Flex : DisplayStyle.None;
        }

        private void ShowUsageToggled(bool showUsage) {
            _usageElement.style.display = showUsage ? DisplayStyle.Flex : DisplayStyle.None;
        }

        private void EditButtonClicked() => LocalizationEntryWindow.Open(Key);

        private void CopyKeyButtonClicked() => KeyName.CopyToClipboard();

        /// <inheritdoc cref="AmiliousLocalization.KeyMovedDelegate"/>
        private void KeyMoved(string key, KeyInfo keyInfo) {
            if(key==_key) Setup(key, _language);
        }

        /// <summary>
        /// This method is used to update the localization text.
        /// </summary>
        private void UpdateLocalizedText(string previous = null, string current=null) {
            Initialize();
            _pathLabel.text = _localGroup[E.LOCAL_INFO_LOCATION];
            _descriptionLabel.text = _localGroup[E.LOCAL_INFO_DESCRIPTION];
            _translationLabel.text = _localGroup[E.LOCAL_INFO_TRANSLATION];
            _copyButton.tooltip = _localGroup[E.LOCAL_INFO_COPY_TOOLTIP];
            _copyKeyButton.tooltip = _localGroup[E.LOCAL_INFO_COPY_KEY_TOOLTIP];
            _editButton.tooltip = _localGroup[E.LOCAL_INFO_EDIT_TOOLTIP];
            var items = _menuButton.menu.MenuItems().Count;
            for(var i=0;i<items;i++) _menuButton.menu.RemoveItemAt(0);
            _menuButton.menu.AppendAction(_localGroup[E.LOCAL_INFO_MENU_COPY],CopyKeyMenuClicked);
            _menuButton.menu.AppendAction(_localGroup[E.LOCAL_INFO_MENU_COPY_GROUP],CopyGroupMenuClicked);
            _menuButton.menu.AppendAction(_localGroup[E.LOCAL_INFO_MENU_COPY_KEY_NAME],CopyKeyNameMenuClicked);
            _menuButton.menu.AppendSeparator();
            _menuButton.menu.AppendAction(_localGroup[E.LOCAL_INFO_MENU_SEARCH_GROUP],SearchGroupMenuClicked);
            _menuButton.menu.AppendSeparator();
            _menuButton.menu.AppendAction(_localGroup[E.LOCAL_INFO_MENU_CLEAR_COUNT],MenuClearCountClicked);
            _menuButton.menu.AppendSeparator();
            _menuButton.menu.AppendAction(_localGroup[E.LOCAL_INFO_MENU_EDIT],EditKeyMenuClicked);
            _menuButton.menu.AppendAction(_localGroup[E.LOCAL_INFO_MENU_DELETE],DeleteKeyMenuClicked);
        }

        private void MenuClearCountClicked(DropdownMenuAction obj) {
            AmiliousLocalization.ClearCount(_key);
        }

        /// <summary>
        /// This method is called when the copy key name menu item is clicked.
        /// </summary>
        private void CopyKeyNameMenuClicked(DropdownMenuAction obj) => KeyName.CopyToClipboard();

        /// <summary>
        /// This method is called when the search group menu item is clicked.
        /// </summary>
        private void SearchGroupMenuClicked(DropdownMenuAction obj) => OnRequestSearch?.Invoke(Group);

        /// <summary>
        /// This method is called when the copy group menu item is clicked.
        /// </summary>
        private void CopyGroupMenuClicked(DropdownMenuAction obj) => Group.CopyToClipboard();

        /// <summary>
        /// This method is called when a translation has been updated.
        /// </summary>
        /// <param name="language"></param>
        /// <param name="key"></param>
        private void OnTranslationUpdated(string language, string key) {
            if(_localGroup.IsInGroup(key)) UpdateLocalizedText();
            if(key != Key) return;
            if(_language != language) return;
            AmiliousLocalization.PauseCounting();
            var tran = AmiliousLocalization.TryGetTranslation(language, _key, out var translation,false);
            AmiliousLocalization.ResumeCounting();
            if(string.IsNullOrWhiteSpace(translation)||!tran)
                translation = _localGroup[E.LOCAL_INFO_NO_TRANSLATION, language];
            Translation = translation;
        }

        /// <summary>
        /// This method is called when a key's description is updated.
        /// </summary>
        /// <param name="key">The key that was updated.</param>
        private void OnDescriptionUpdated(string key) {
            if(key != Key) return;
            var description = AmiliousLocalization.GetDescription(key);
            if(string.IsNullOrWhiteSpace(description)) 
                description  = _localGroup[E.LOCAL_INFO_NO_DESCRIPTION];
            Description = description;
        }

        /// <summary>
        /// This method is called when the copy button is clicked.
        /// </summary>
        private void CopyFullButtonClicked() => Key.CopyToClipboard();

        /// <summary>
        /// This method is called when the delete menu item is clicked.
        /// </summary>
        private void DeleteKeyMenuClicked(DropdownMenuAction obj) {
            if(EditorUtility.DisplayDialog(_localGroup["delete_title",Key],
                _localGroup["delete_message"], _localGroup["delete_ok"], _localGroup["delete_cancel"])) {
                AmiliousLocalization.RemoveKey(Key);
                AssetDatabase.Refresh();
            }
        }

        /// <summary>
        /// This method is called when the edit menu item is clicked.
        /// </summary>
        private void EditKeyMenuClicked(DropdownMenuAction obj) => LocalizationEntryWindow.Open(Key);

        /// <summary>
        /// This method is called when the copy menu item is clicked.
        /// </summary>
        private void CopyKeyMenuClicked(DropdownMenuAction obj) => Key.CopyToClipboard();
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}