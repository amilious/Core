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
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using Amilious.Core.Extensions;
using Amilious.Core.Localization;

namespace Amilious.Core.Editor.VisualElements {
    
    /// <summary>
    /// This element is used for drawing a language selector.
    /// </summary>
    public class LanguageSelector : VisualElement {
        
        #region Constants //////////////////////////////////////////////////////////////////////////////////////////////
                          
        private const string NEXT = "Next";
        private const string LABEL = "Label";
        private const string MENU = "Menu";
        private const string PREVIOUS = "Previous";
        private const string ASSET_PATH = "Assets/Amilious/Core/Editor/VisualElements/Selector.uxml";

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region UXML Requirement ///////////////////////////////////////////////////////////////////////////////////////

        public new class UxmlFactory : UxmlFactory<LanguageSelector,UxmlTraits> { }

        public new class UxmlTraits : VisualElement.UxmlTraits {
            
            private readonly UxmlStringAttributeDescription _valueDescription = new UxmlStringAttributeDescription() {
                name = "Value", defaultValue = AmiliousLocalization.CurrentLanguage, 
                restriction =new UxmlEnumeration() { values = AmiliousLocalization.Languages }
            };
            
            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc) {
                base.Init(ve, bag, cc);
                if(ve is not LanguageSelector element) return;
                element.Value = _valueDescription.GetValueFromBag(bag, cc);
            }
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Private Fields /////////////////////////////////////////////////////////////////////////////////////////

        private Label _valueLabel;
        private bool _initialized;
        private ToolbarMenu _toolbarMenu;
        private ToolbarButton _nextButton;
        private ToolbarButton _previousButton;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Events /////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is called when the value is changed.
        /// </summary>
        public event Action<string> OnValueChanged;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Public Properties //////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This property is used to get or set the value.
        /// </summary>
        public string Value {
            get => _valueLabel.text;
            set {
                if(_valueLabel.text == value) return;
                _valueLabel.text = value;
                OnValueChanged?.Invoke(value);
            }
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Constructors ///////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This is the default constructor that takes in no values.
        /// </summary>
        public LanguageSelector() => Initialize();
        
        /// <summary>
        /// This constructor can be used to set the initial value.
        /// </summary>
        /// <param name="selectedLanguage">The initial value;</param>
        public LanguageSelector(string selectedLanguage) {
            Initialize();
            //update without triggering the event
            _valueLabel.text = selectedLanguage;
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Private Methods ////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used for initializing the element.
        /// </summary>
        private void Initialize() {
            if(_initialized) return;
            _initialized = true;
            var asset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(ASSET_PATH);
            asset.CloneTree(this);
            _previousButton = this.Q<ToolbarButton>(PREVIOUS);
            _valueLabel = this.Q<Label>(LABEL);
            _nextButton = this.Q<ToolbarButton>(NEXT);
            _toolbarMenu = this.Q<ToolbarMenu>(MENU);
            _previousButton.clicked -= PreviousButtonClicked;
            _previousButton.clicked += PreviousButtonClicked;
            _nextButton.clicked -= NextButtonClicked;
            _nextButton.clicked += NextButtonClicked;
            _valueLabel.text = AmiliousLocalization.CurrentLanguage;
            AmiliousLocalization.OnLanguagesUpdated -= OnLanguagesUpdated;
            AmiliousLocalization.OnLanguagesUpdated += OnLanguagesUpdated;
            OnLanguagesUpdated();
        }

        /// <summary>
        /// This method is used to update the menu.
        /// </summary>
        private void OnLanguagesUpdated() {
            var total = _toolbarMenu.menu.MenuItems().Count;
            for(var i=0;i<total;i++) _toolbarMenu.menu.RemoveItemAt(0);
            foreach(var lang in AmiliousLocalization.Languages) {
                _toolbarMenu.menu.AppendAction(lang, UpdateLanguageSelection, UpdateMenuChecks);
            }
        }

        /// <summary>
        /// This method is used to update the checks in the menu.
        /// </summary>
        /// <param name="arg">The arg that you want to update.</param>
        /// <returns>The updated status.</returns>
        private DropdownMenuAction.Status UpdateMenuChecks(DropdownMenuAction arg) {
            return arg.name == _valueLabel.text ? DropdownMenuAction.Status.Checked : 
                DropdownMenuAction.Status.Normal;
        }

        /// <summary>
        /// This method is triggered when a language is chosen from the dropdown.
        /// </summary>
        /// <param name="obj">The object.</param>
        private void UpdateLanguageSelection(DropdownMenuAction obj) {
            if(AmiliousLocalization.HasLanguage(obj.name, true))
                _valueLabel.text = obj.name;
            OnLanguagesUpdated();
            OnValueChanged?.Invoke(obj.name);
        }

        /// <summary>
        /// This method is called when the next button is clicked.
        /// </summary>
        private void NextButtonClicked() => Value = AmiliousLocalization.Languages.GetPrevious(Value);

        /// <summary>
        /// This method is called when the previous button is clicked.
        /// </summary>
        private void PreviousButtonClicked() => Value = AmiliousLocalization.Languages.GetPrevious(Value);

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}