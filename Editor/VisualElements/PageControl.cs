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

using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using Amilious.Core.Extensions;
using System.Collections.Generic;
using Amilious.Core.Localization;
using Amilious.Core.Editor.Extensions;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable ClassWithVirtualMembersNeverInherited.Global
namespace Amilious.Core.Editor.VisualElements {
    
    /// <summary>
    /// This element is used to act as a page control.
    /// </summary>
    public class PageControl : VisualElement {

        #region Constants //////////////////////////////////////////////////////////////////////////////////////////////
        
        protected const string OF = "Of";
        protected const string NEXT = "Next";
        protected const string PER_PAGE = "PerPage";
        protected const string PREVIOUS = "Previous";
        protected const string MAX_PAGES = "MaxPages";
        protected const string PAGE_LABEL = "PageLabel";
        protected const string CURRENT_PAGE = "CurrentPage";
        protected const string PER_PAGE_LABEL = "PerPageLabel";
        protected static readonly Dictionary<string, int> LookUp = new Dictionary<string, int>() {
            {"5", 5}, {"10", 10}, {"15", 15}, {"20", 20}, {"25", 25}, {"50",50}, {"100",100}
        };
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Private Fields /////////////////////////////////////////////////////////////////////////////////////////
        
        private Label _of;
        private int _maxPage;
        private Label _maxPages;
        private int _activePage;
        private Label _pageLabel;
        private int _itemsPerPage;
        private Label _perPageLabel;
        private ToolbarMenu _perPage;
        private ToolbarButton _nextButton;
        private IntegerField _currentPage;
        private ToolbarButton _previousButton;
        private static LocalizedGroup LocalGroup;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region UXML Requirement ///////////////////////////////////////////////////////////////////////////////////////

        public new class UxmlFactory : UxmlFactory<PageControl,UxmlTraits> { }

        public new class UxmlTraits : VisualElement.UxmlTraits { }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Events /////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This delegate is used for the <see cref="PageControl.OnRequestPageChange"/> event.
        /// </summary>
        /// <param name="pageNumber">The page number that is being requested.</param>
        public delegate void RequestPageChangeDelegate(int pageNumber);

        /// <summary>
        /// This delegate is used for the <see cref="PageControl.OnChangeItemsPerPage"/> event.
        /// </summary>
        /// <param name="numberPerPage">The number of items to display per page.</param>
        public delegate void ChangeItemsPerPageDelegate(int numberPerPage);
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Events /////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This event is triggered when the page control is requesting a redraw for a different page.  You should call
        /// the <see cref="ReturnPageItems{T}"/> method with the same values to change the page.
        /// </summary>
        public event RequestPageChangeDelegate OnRequestPageChange;

        /// <summary>
        /// This event is called when the number of items that should be displayed per page changes.  You do not need to
        /// call the <see cref="ReturnPageItems{T}"/> method because the <see cref="OnChangeItemsPerPage"/> event will
        /// be triggered after this event.
        /// </summary>
        public event ChangeItemsPerPageDelegate OnChangeItemsPerPage;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Constructors ///////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This is the default constructor which will set up the items per page to be 15. 
        /// </summary>
        public PageControl() => Initialize();
        
        /// <summary>
        /// This constructor is used to set up the page control with the given number of items per page.
        /// </summary>
        /// <param name="itemsPerPage">The number of items per page.</param>
        public PageControl(int itemsPerPage) {
            Initialize();
            if(!LookUp.ContainsValue(itemsPerPage)) return;
            _itemsPerPage = itemsPerPage;
            _perPage.text = $"{itemsPerPage}";
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method will return the items that should be displayed on the page given all the elements.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <param name="pageNumber">The current page number.</param>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <returns>The items that should be drawn for the current page.</returns>
        public IEnumerable<T> ReturnPageItems<T>(IEnumerable<T> items, int pageNumber) {
            var enumerable = items as T[] ?? items.ToArray();
            if(pageNumber < 1) pageNumber = 1;
            var itemCount = enumerable.Count();
            if(itemCount == 0) {
                _nextButton.SetEnabled(false);
                _previousButton.SetEnabled(false);
                _maxPages.text = "1";
                _currentPage.value = 1;
                yield break;
            }
            _maxPage = Mathf.CeilToInt(itemCount/(float)_itemsPerPage);
            if(pageNumber > _maxPage) pageNumber = _maxPage;
            _activePage = pageNumber;
            var fistIndex = (pageNumber-1) * _itemsPerPage;
            var lastIndex = fistIndex + _itemsPerPage;
            if(lastIndex > itemCount) lastIndex = itemCount;
            for(var i = fistIndex; i < lastIndex; i++) {
                yield return enumerable[i];
            }
            _maxPages.text = $"{_maxPage}";
            _currentPage.value = _activePage;
            _nextButton.SetEnabled(_activePage!=_maxPage);
            _previousButton.SetEnabled(_activePage!=1);
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Private and Protected Methods //////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to initialize the page control. 
        /// </summary>
        protected void Initialize() {
            LocalGroup ??= new LocalizedGroup(E.PAGE_CONTROL_GROUP);
            var asset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(E.PAGE_CONTROL_UXML);
            asset.CloneTree(this);
            this.Q(PAGE_LABEL, out _pageLabel);
            this.Q(PREVIOUS, out _previousButton);
            this.Q(NEXT, out _nextButton);
            this.Q(CURRENT_PAGE, out _currentPage);
            this.Q(MAX_PAGES, out _maxPages);
            this.Q(OF, out _of);
            this.Q(PAGE_LABEL, out _pageLabel);
            this.Q(PER_PAGE, out _perPage);
            this.Q(PER_PAGE_LABEL, out _perPageLabel);
            //add events
            _nextButton.clicked -= NextClicked;
            _nextButton.clicked += NextClicked;
            _previousButton.clicked -= PreviousClicked;
            _previousButton.clicked += PreviousClicked;
            //build per page
            _perPage.ClearMenu();
            foreach(var key in LookUp.Keys) 
                _perPage.menu.AppendAction(key,PerPageChanged,PerPageChecks);
            if(!int.TryParse(_perPage.text, out _itemsPerPage)) {
                _itemsPerPage = 15;
                _perPage.text = "15";
            }
            _currentPage.SetEnabled(false);
            AmiliousLocalization.OnLanguageChanged -= LanguageChanged;
            AmiliousLocalization.OnLanguageChanged += LanguageChanged;
            AmiliousLocalization.OnTranslationUpdated -= TranslationUpdated;
            AmiliousLocalization.OnTranslationUpdated += TranslationUpdated;
            UpdateLocalizationText();
        }

        /// <inheritdoc cref="AmiliousLocalization.TranslationUpdatedDelegate"/>
        private void TranslationUpdated(string language, string key) {
            if(LocalGroup.IsInGroup(key)) UpdateLocalizationText();
        }

        /// <inheritdoc cref="AmiliousLocalization.LanguageChangedDelegate"/>
        private void LanguageChanged(string previous, string current) => UpdateLocalizationText();

        /// <summary>
        /// This method is called when the localized values should be updated.
        /// </summary>
        protected virtual void UpdateLocalizationText() {
            _pageLabel.text = LocalGroup[E.PAGE_CONTROL_PAGE_LABEL];
            _perPageLabel.text = LocalGroup[E.PAGE_CONTROL_PER_PAGE_LABEL];
            _of.text = LocalGroup[E.PAGE_CONTROL_OF_LABEL];
        }

        /// <summary>
        /// This method is used to apply a check to the current option.
        /// </summary>
        /// <param name="arg">The argument that you want to check.</param>
        /// <returns>The correct style for the menu item.</returns>
        private DropdownMenuAction.Status PerPageChecks(DropdownMenuAction arg) {
            if(LookUp.TryGetValueFix(arg.name,out var value)&& _itemsPerPage == value)
                return DropdownMenuAction.Status.Checked;
            return DropdownMenuAction.Status.Normal;
        }

        /// <summary>
        /// This method is called when the item per page value is changed.
        /// </summary>
        /// <param name="obj">The menu item that has been selected.</param>
        protected virtual void PerPageChanged(DropdownMenuAction obj) {
            if(!LookUp.TryGetValueFix(obj.name, out var value)) return;
            _itemsPerPage = value;
            _perPage.text = obj.name;
            OnChangeItemsPerPage?.Invoke(_itemsPerPage);
            OnRequestPageChange?.Invoke(1);
        }

        /// <summary>
        /// This method is called when the next button is clicked.
        /// </summary>
        protected virtual void NextClicked() => OnRequestPageChange?.Invoke(_activePage+1);
        
        /// <summary>
        /// This method is called when the previous button is clicked.
        /// </summary>
        protected virtual void PreviousClicked() => OnRequestPageChange?.Invoke(_activePage-1);
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}