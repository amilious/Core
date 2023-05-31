
using UnityEditor;
using UnityEngine;
using System.Linq;
using Amilious.Core.IO;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using Amilious.Core.Localization;
using System.Collections.Generic;
using Amilious.Core.Editor.Extensions;
using UnityEditor.Experimental.GraphView;
using Amilious.Core.Editor.VisualElements;
using Amilious.Core.Editor.SearchProviders;
using Status = UnityEngine.UIElements.DropdownMenuAction.Status;

namespace Amilious.Core.Editor.Windows {
    
    public class LocalizationBrowserWindow : EditorWindow {

        #region Constants //////////////////////////////////////////////////////////////////////////////////////////////
        
        private const float RESET_DELAY = .7f;
        private const string SEARCH = "Search";
        private const string SHOW_USAGE = "ShowUsage";
        private const string FIND_BUTTON = "FindButton";
        private const string SCROLL_VIEW = "ScrollView";
        private const string ACTION_MENU = "ActionMenu";
        private const string SHOW_PATH = "ShowLocation";
        private const string SORT_BY_MENU = "SortByMenu";
        private const string SHOW_DESCRIPTION = "ShowDescription";
        private const string SHOW_TRANSLATION = "ShowTranslation";
        private const string ITEMS_PER_PAGE = "Amilious/Localization/Items Per Page";

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        private delegate void DisplayLanguageChangedDelegate(string language);

        private event DisplayLanguageChangedDelegate OnDisplayLanguageChanged;
        
        #region Private Fields /////////////////////////////////////////////////////////////////////////////////////////

        private bool _initialized;
        private int _itemsPerPage;
        private Button _findButton;
        private NoMatches _noMatches;
        private bool _resetScheduled;
        private Label _languageLabel;
        private double _lastResetTime;
        private ScrollView _scrollView;
        private ToolbarMenu _actionMenu;
        private ToolbarMenu _sortByMenu;
        private ToolbarToggle _showPath;
        private ToolbarToggle _showUsage;
        private PageControl _pageControl;
        private LocalizedGroup _localGroup;
        private ToolbarSearchField _search;
        private KeyPathSelector _pathSelector;
        private ToolbarToggle _showDescription;
        private ToolbarToggle _showTranslation;
        private LanguageSelector _languageSelector;
        private static VisualTreeAsset WindowAsset;

        private readonly Dictionary<string, LocalizationInformation> Elements =
            new Dictionary<string, LocalizationInformation>();

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This property is true if loading a language should be logged, otherwise false.
        /// </summary>
        public int ItemsPerPage {
            get => _itemsPerPage;
            set {
                if(_itemsPerPage == value) return;
                _itemsPerPage = value;
                BasicSave.StoreData(ITEMS_PER_PAGE,value);
            } 
        }
        
        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////
        
        [MenuItem("Amilious/Localization/Localization Browser")]
        public static void ShowWindow() => GetWindow(typeof(LocalizationBrowserWindow));

        /// <summary>
        /// This method is called when the gui is being created.
        /// </summary>
        public void CreateGUI() {
            _initialized = false;
            Initialize();
            _pathSelector.OnValueChanged += OnPathChanged;
            _languageSelector.OnValueChanged += OnLanguageChanged;
            _noMatches = new NoMatches(AddLocalizationKey);
            RestValues();
            _findButton.clicked += FindButtonClicked;
            _search.RegisterValueChangedCallback(OnSearchQueryChanged);
            AmiliousLocalization.OnKeyMoved -= KeyMoved;
            AmiliousLocalization.OnKeyMoved += KeyMoved;
            AmiliousLocalization.OnKeyAdded -= RestValues;
            AmiliousLocalization.OnKeyAdded += RestValues;
            AmiliousLocalization.OnKeyRemoved -= RestValues;
            AmiliousLocalization.OnKeyRemoved += RestValues;
            LocalizationInformation.OnRequestSearch -= SearchRequested;
            LocalizationInformation.OnRequestSearch += SearchRequested;
            AmiliousLocalization.OnLanguageChanged -= LanguageChanged;
            AmiliousLocalization.OnLanguageChanged += LanguageChanged;
            AmiliousLocalization.OnTranslationUpdated -= TranslationUpdated;
            AmiliousLocalization.OnTranslationUpdated += TranslationUpdated;
            AmiliousLocalization.OnLanguagesUpdated -= UpdateLocalizationText;
            AmiliousLocalization.OnLanguagesUpdated += UpdateLocalizationText;
            AmiliousLocalization.OnShowUsageToggled -= ShowUsageToggled;
            AmiliousLocalization.OnShowUsageToggled += ShowUsageToggled;
            AmiliousLocalization.OnShowPathToggled -= ShowPathToggled;
            AmiliousLocalization.OnShowPathToggled += ShowPathToggled;
            AmiliousLocalization.OnShowDescriptionToggled -= ShowDescriptionToggled;
            AmiliousLocalization.OnShowDescriptionToggled += ShowDescriptionToggled;
            AmiliousLocalization.OnShowTranslationToggled -= ShowTranslationToggled;
            AmiliousLocalization.OnShowTranslationToggled += ShowTranslationToggled;
            _pageControl.OnRequestPageChange += PageChanged;
            _pageControl.OnChangeItemsPerPage += (value) => { ItemsPerPage = value; };
            ShowUsageToggled(AmiliousLocalization.ShowUsage);
            ShowPathToggled(AmiliousLocalization.ShowPath);
            ShowDescriptionToggled(AmiliousLocalization.ShowDescription);
            ShowTranslationToggled(AmiliousLocalization.ShowTranslation);
            _showUsage.UnregisterValueChangedCallback(ShowUsageToggleClicked);
            _showDescription.UnregisterValueChangedCallback(ShowDescriptionToggleClicked);
            _showPath.UnregisterValueChangedCallback(ShowPathToggleClicked);
            _showTranslation.UnregisterValueChangedCallback(ShowTranslationToggleClicked);
            _showUsage.RegisterValueChangedCallback(ShowUsageToggleClicked);
            _showDescription.RegisterValueChangedCallback(ShowDescriptionToggleClicked);
            _showPath.RegisterValueChangedCallback(ShowPathToggleClicked);
            _showTranslation.RegisterValueChangedCallback(ShowTranslationToggleClicked);
            UpdateLocalizationText();
        }

        private void ShowTranslationToggleClicked(ChangeEvent<bool> evt) =>
            AmiliousLocalization.ShowTranslation = evt.newValue;

        private void ShowPathToggleClicked(ChangeEvent<bool> evt) => AmiliousLocalization.ShowPath = evt.newValue;

        private void ShowDescriptionToggleClicked(ChangeEvent<bool> evt) =>
            AmiliousLocalization.ShowDescription = evt.newValue;

        private void ShowUsageToggleClicked(ChangeEvent<bool> evt) => AmiliousLocalization.ShowUsage = evt.newValue;

        private void ShowTranslationToggled(bool value) {
            if(_showTranslation.value != value) _showTranslation.value = value;
        }

        private void ShowDescriptionToggled(bool value) {
            if(_showDescription.value != value) _showDescription.value = value;
        }

        private void ShowPathToggled(bool value) {
            if(_showPath.value != value) _showPath.value = value;
        }

        private void ShowUsageToggled(bool value) {
            if(_showUsage.value != value) _showUsage.value = value;
        }

        private void PageChanged(int page) {
            _scrollView.contentContainer.Clear();
            foreach(var e in _pageControl.ReturnPageItems(_toDraw, page))
                _scrollView.contentContainer.Add(e);
            _scrollView.scrollOffset = Vector2.zero;
        }

        private void OnDestroy() {
            AmiliousLocalization.OnKeyMoved -= KeyMoved;
            AmiliousLocalization.OnKeyAdded -= RestValues;
            AmiliousLocalization.OnKeyRemoved -= RestValues;
            AmiliousLocalization.OnLanguageChanged -= LanguageChanged;
            LocalizationInformation.OnRequestSearch -= SearchRequested;
            AmiliousLocalization.OnTranslationUpdated -= TranslationUpdated;
        }

        /// <inheritdoc cref="AmiliousLocalization.KeyMovedDelegate"/>
        private void KeyMoved(string key, KeyInfo keyInfo) => RestValues();

        private void UpdateLocalizationText() {
            Initialize();
            _findButton.text = _localGroup[E.LOCAL_BROWSER_FIND_BUTTON];
            _findButton.tooltip = _localGroup[E.LOCAL_BROWSER_FIND_TOOLTIP];
            _actionMenu.ClearMenu();
            _actionMenu.menu.AppendAction(_localGroup[E.LOCAL_BROWSER_MENU_ADD_LANGUAGE],MenuAddLanguageClicked);
            _actionMenu.menu.AppendAction(_localGroup[E.LOCAL_BROWSER_MENU_RELOAD],MenuReloadDataClicked);
            _actionMenu.menu.AppendAction(_localGroup[E.LOCAL_BROWSER_MENU_CLEAR_COUNT],MenuClearCountClicked);
            _actionMenu.menu.AppendSeparator();
            foreach(var lang in AmiliousLocalization.Languages) _actionMenu.menu.AppendAction(
                _localGroup[E.LOCAL_BROWSER_LANGUAGE_MENU,lang],MenuSetCurrentLanguage, CheckCurrentLanguage,lang);
            #if AMILIOUS_DEVELOPMENT
            _actionMenu.menu.AppendSeparator();
            _actionMenu.menu.AppendAction("Development/Redraw Keys",RedrawKeys);
            _actionMenu.menu.AppendAction("Development/Redraw Window", RedrawWindow);
            #endif
            _sortByMenu.ClearMenu();
            
        }

        private Status CheckCurrentLanguage(DropdownMenuAction arg) =>
            AmiliousLocalization.CurrentLanguage==(string)arg.userData? Status.Checked: Status.Normal;

        private void MenuAddLanguageClicked(DropdownMenuAction obj) => LocalizationAddLanguageWindow.Open();

        private void MenuClearCountClicked(DropdownMenuAction obj) {
            if(EditorUtility.DisplayDialog(_localGroup["clear_all_title"], _localGroup["clear_all_message"], 
                _localGroup["clear_all_ok"], _localGroup["clear_all_cancel"])) {
                AmiliousLocalization.ClearAllCount();
            }
        }

        private void RedrawWindow(DropdownMenuAction obj) {
            rootVisualElement.Clear();
            CreateGUI();
        }

        private void RedrawKeys(DropdownMenuAction obj) {
            Elements.Clear();
            RestValues();
        }

        private void MenuReloadDataClicked(DropdownMenuAction obj) => AmiliousLocalization.ReloadData();

        private void MenuSetCurrentLanguage(DropdownMenuAction obj) {
            AmiliousLocalization.CurrentLanguage = (string)obj.userData;
        }

        private void TranslationUpdated(string language, string key) {
            if(_localGroup.IsInGroup(key)) UpdateLocalizationText();
        }

        private void LanguageChanged(string previous, string current) => UpdateLocalizationText();

        private void AddLocalizationKey() {
            if(!AmiliousLocalization.HasKey(_search.value)) 
                LocalizationEntryWindow.Open(_search.value,AmiliousLocalization.GetLocationPath(_pathSelector.Value));
        }

        private void SearchRequested(string query) {
            _search.value = query;
        }

        private List<VisualElement> _toDraw = new List<VisualElement>();
        
        public void RestValues(string modifiedKey = null) {
            _scrollView.contentContainer.Clear();
            var lang = _languageSelector.Value;
            var keys = _pathSelector.Value == "All" ? AmiliousLocalization.Keys.ToList()
                : AmiliousLocalization.GetValidKeys(_pathSelector.Value).ToList();
            keys.Sort();
            if(!keys.Contains(_search.value)&&!string.IsNullOrWhiteSpace(_search.value))
                _scrollView.contentContainer.Add(_noMatches);
            var hasItems = false;
            _toDraw.Clear();
            foreach(var key in keys) {
                var lowerKey = key.ToLower();
                var lowerSearch = _search.value.ToLower() ?? string.Empty;
                AmiliousLocalization.PauseCounting();
                var hasTranslation = AmiliousLocalization.TryGetTranslation(lang, key, out var translation);
                AmiliousLocalization.ResumeCounting();
                if(!string.IsNullOrEmpty(lowerSearch) && !lowerKey.Contains(lowerSearch) &&
                    (!hasTranslation||!translation.ToLower().Contains(lowerSearch))) continue;
                if(!Elements.TryGetValue(key, out var element)) {
                    element = new LocalizationInformation(key, lang);
                    OnDisplayLanguageChanged += element.SetDisplayLanguage;
                    Elements.Add(key,element);
                }
                _toDraw.Add(element);
                //_scrollView.contentContainer.Add(element);
                hasItems = true;
            }
            foreach(var e in _pageControl.ReturnPageItems(_toDraw, 1))
                _scrollView.contentContainer.Add(e);
            _scrollView.scrollOffset = Vector2.zero;
            _noMatches.HasMatches = hasItems;
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        private void OnSearchQueryChanged(ChangeEvent<string> evt = null) => ScheduleRest();

        private void ScheduleRest() {
            _lastResetTime = EditorApplication.timeSinceStartup;
            if(!_resetScheduled) EditorApplication.update += DelayedReset;
            _resetScheduled = true;
        }
        
        private void DelayedReset() {
            var elapsedTime = EditorApplication.timeSinceStartup - _lastResetTime;
            if(!(elapsedTime >= RESET_DELAY)) return;
            EditorApplication.update -= DelayedReset;
            _resetScheduled = false;
            RestValues();
        }
        
        /// <summary>
        /// This method is called when the find button is called.
        /// </summary>
        private void FindButtonClicked() {
            var rect = _search.contentRect;
            var popPos = rect.position + new Vector2((rect.width/2f)+2,100);
            popPos = GUIUtility.GUIToScreenPoint(popPos);
            var searchProvider = CreateInstance<LocalizationSearchProvider>();
            searchProvider.Initialize(_pathSelector.Value, result => { _search.value = result; });
            var searchWindow = new SearchWindowContext(popPos, rect.width + 1, 150);
            SearchWindow.Open(searchWindow, searchProvider);
        }

        private void OnLanguageChanged(string language) {
            OnDisplayLanguageChanged?.Invoke(language);
            RestValues();
        }

        private void OnPathChanged(string path) => RestValues();

        protected void Initialize() {
            if(_initialized) return;
            _initialized = true;
            //get the visual element
            this.CloneAssetTree();
            _localGroup = new LocalizedGroup(E.LOCAL_BROWSER_GROUP);
            var texture = Resources.Load<Texture>("Icons/translate2");
            titleContent = new GUIContent("Amilious Localization",texture);
            this.Q(ACTION_MENU,out _actionMenu);
            this.Q(SHOW_PATH, out _showPath);
            this.Q(SHOW_USAGE, out _showUsage);
            this.Q(SHOW_DESCRIPTION, out _showDescription);
            this.Q(SHOW_TRANSLATION, out _showTranslation);
            this.Q(SORT_BY_MENU, out _sortByMenu);
            
            var holder = rootVisualElement.Q<VisualElement>("LanguageSelectorHolder");
            _languageSelector = new LanguageSelector();
            holder.Clear();
            holder.Add(_languageSelector);
            _languageSelector.style.flexGrow = new StyleFloat(1);
            
            var holder2 = rootVisualElement.Q<VisualElement>("PathSelectorHolder");
            _pathSelector = new KeyPathSelector();
            holder2.Clear();
            holder2.Add(_pathSelector);
            _pathSelector.style.flexGrow = new StyleFloat(1);
            
            this.Q(FIND_BUTTON, out _findButton);
            this.Q(SEARCH, out _search);
            this.Q(SCROLL_VIEW, out _scrollView);
            
            _itemsPerPage = BasicSave.ReadData(ITEMS_PER_PAGE, 10);
            var holder3 = rootVisualElement.Q<VisualElement>("PageControlHolder");
            _pageControl = new PageControl(ItemsPerPage);
            holder3.Clear();
            holder3.Add(_pageControl);
            
        }

    }
    
}