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
using UnityEditor;
using UnityEngine.UIElements;
using Amilious.Core.Localization;

namespace Amilious.Core.Editor.VisualElements {
    
    /// <summary>
    /// This element is used to display the add button and the no matches found text.
    /// </summary>
    public class NoMatches : VisualElement {
        
        #region Constants //////////////////////////////////////////////////////////////////////////////////////////////
        
        private const string LOCALIZATION_GROUP = "amilious/localization/editor/";
        private const string ASSET_PATH = "Assets/Amilious/Core/Editor/VisualElements/NoMatches.uxml";

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Private Fields /////////////////////////////////////////////////////////////////////////////////////////
        
        private Label _label;
        private Button _button;
        private Action _onClick;
        private bool _initialized;
        private LocalizedGroup _localGroup;
        private static VisualTreeAsset Asset;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region UXML Requirement ///////////////////////////////////////////////////////////////////////////////////////
        
        public new class UxmlFactory : UxmlFactory<NoMatches>{}

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This property can be used to set if any matches were found.
        /// </summary>
        public bool HasMatches {
            set => _label.style.display = value?DisplayStyle.None:DisplayStyle.Flex;
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Constructors ///////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This constructor is used by the UI Builder.
        /// </summary>
        public NoMatches() => Initialize();

        /// <summary>
        /// This constructor is used to create a NoMatches element.
        /// </summary>
        /// <param name="onClick">The method that you want to be called when the button is clicked.</param>
        public NoMatches(Action onClick = null) {
            Initialize();
            _onClick = onClick;
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method can be used to set the callback method that is triggered when the button is clicked.
        /// </summary>
        /// <param name="onClickCallback">The method that you want to be triggered when the button is clicked.</param>
        public void SetOnClickCallback(Action onClickCallback) => _onClick = onClickCallback;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Private Methods ////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to initialized the components.
        /// </summary>
        private void Initialize() {
            if(_initialized) return;
            _initialized = true;
            _localGroup = new LocalizedGroup(LOCALIZATION_GROUP);
            Asset ??= AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(ASSET_PATH);
            Asset.CloneTree(this);
            _label = this.Q<Label>("Label");
            _button = this.Q<Button>("Button");
            _button.clicked -= OnButtonClicked;
            AmiliousLocalization.OnTranslationUpdated -= TranslationUpdated;
            AmiliousLocalization.OnLanguageChanged -= LanguageChanged;
            _button.clicked += OnButtonClicked;
            AmiliousLocalization.OnTranslationUpdated += TranslationUpdated;
            AmiliousLocalization.OnLanguageChanged += LanguageChanged;
            UpdateLocalizedText();
        }

        /// <summary>
        /// This method is called when the language changes.
        /// </summary>
        private void LanguageChanged(string previous, string current) => UpdateLocalizedText();

        /// <summary>
        /// This method is called to update the localized text.
        /// </summary>
        private void UpdateLocalizedText() {
            Initialize();
            _label.text = _localGroup["no_results"];
            _button.tooltip = _localGroup["add_tooltip"];
            _button.text = _localGroup["add_button"];
        }

        /// <summary>
        /// This method is called when a translation is updated.
        /// </summary>
        private void TranslationUpdated(string language, string key) {
            if(_localGroup.IsInGroup(key)) UpdateLocalizedText();
        }

        /// <summary>
        /// This method is triggered when the button is clicked.
        /// </summary>
        private void OnButtonClicked() => _onClick?.Invoke();

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}