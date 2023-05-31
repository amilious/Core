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
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using Amilious.Core.Extensions;
using Amilious.Core.Localization;
using Amilious.Core.Editor.Extensions;

namespace Amilious.Core.Editor.VisualElements {
    
    /// <summary>
    /// This element is used for drawing a key path selector.
    /// </summary>
    public class KeyPathSelector : VisualElement {
        

        #region Constants //////////////////////////////////////////////////////////////////////////////////////////////
                          
        private const string NEXT = "Next";
        private const string LABEL = "Label";
        private const string MENU = "Menu";
        private const string PREVIOUS = "Previous";
        private const string ASSET_PATH = "Assets/Amilious/Core/Editor/VisualElements/Selector.uxml";

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region UXML Requirement ///////////////////////////////////////////////////////////////////////////////////////

        public new class UxmlFactory : UxmlFactory<LanguageSelector,UxmlTraits> { }

        public new class UxmlTraits : VisualElement.UxmlTraits { }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Private Fields /////////////////////////////////////////////////////////////////////////////////////////

        private Label _valueLabel;
        private bool _initialized;
        private readonly bool _includeAll;
        private ToolbarMenu _toolbarMenu;
        private ToolbarButton _nextButton;
        private ToolbarButton _previousButton;
        private static VisualTreeAsset Asset;

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
                if(!(AmiliousLocalization.HasKeyPathName(value)||(_includeAll&&value=="All"))) return;
                _valueLabel.text = value;
                PathsUpdated();
                OnValueChanged?.Invoke(value);
            }
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Constructors ///////////////////////////////////////////////////////////////////////////////////////////

        public KeyPathSelector() {
            _includeAll = true;
            Initialize();
        }

        /// <summary>
        /// This is the default constructor that takes in no values.
        /// </summary>
        public KeyPathSelector(bool includeAll) {
            _includeAll = includeAll;
            Initialize();
        }

        /// <summary>
        /// This constructor can be used to set the initial value.
        /// </summary>
        /// <param name="selectedPath">The initial value;</param>
        public KeyPathSelector(string selectedPath, bool includeAll) {
            _includeAll = includeAll;
            Initialize();
            //update without triggering the event
            Value = selectedPath;
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Private Methods ////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used for initializing the element.
        /// </summary>
        private void Initialize() {
            if(_initialized) return;
            _initialized = true;
            Asset ??= AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(ASSET_PATH);
            Asset.CloneTree(this);
            _previousButton = this.Q<ToolbarButton>(PREVIOUS);
            _valueLabel = this.Q<Label>(LABEL);
            _nextButton = this.Q<ToolbarButton>(NEXT);
            _toolbarMenu = this.Q<ToolbarMenu>(MENU);
            _previousButton.clicked -= PreviousButtonClicked;
            _previousButton.clicked += PreviousButtonClicked;
            _nextButton.clicked -= NextButtonClicked;
            _nextButton.clicked += NextButtonClicked;
            _valueLabel.text = _includeAll ? "All" : AmiliousLocalization.DefaultKeyPathName;
            PathsUpdated();
        }

        /// <summary>
        /// This method is used to update the menu.
        /// </summary>
        private void PathsUpdated() {
            _toolbarMenu.ClearMenu();
            if(_includeAll)_toolbarMenu.menu.AppendAction("All", UpdatePathSelection, UpdateMenuChecks);
            foreach(var path in AmiliousLocalization.KeyFileNames) {
                _toolbarMenu.menu.AppendAction(path, UpdatePathSelection, UpdateMenuChecks);
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
        private void UpdatePathSelection(DropdownMenuAction obj) {
            if(AmiliousLocalization.KeyFileNames.Contains(obj.name)||(_includeAll&&obj.name=="All"))
                Value = obj.name;
        }

        /// <summary>
        /// This method is called when the next button is clicked.
        /// </summary>
        private void NextButtonClicked() {
            Value = _includeAll ? AmiliousLocalization.KeyFileNames.GetNext(Value, "All")
                : AmiliousLocalization.KeyFileNames.GetNext(Value);
        }

        /// <summary>
        /// This method is called when the previous button is clicked.
        /// </summary>
        private void PreviousButtonClicked() {
            Value = _includeAll ? AmiliousLocalization.KeyFileNames.GetPrevious(Value, "All")
                : AmiliousLocalization.KeyFileNames.GetPrevious(Value);
        }


        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}