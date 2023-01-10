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
//  Website:        http://www.amilious.comUnity          Asset Store: https://assetstore.unity.com/publishers/62511  //
//  Discord Server: https://discord.gg/SNqyDWu            Copyright© Amilious since 2022                              //                    
//  This code is part of an asset on the unity asset store. If you did not get this from the asset store you are not  //
//  using it legally. Check the asset store or join the discord for the license that applies for this script.         //
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/

using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Linq;

namespace Amilious.Core.UI.Tab {
    
    /// <summary>
    /// This class is used to control a tab group.
    /// </summary>
    /// ReSharper disable ClassWithVirtualMembersNeverInherited.Global
    /// ReSharper disable MemberCanBePrivate.Global
    /// ReSharper disable UnassignedField.Global
    [ExecuteAlways]
    [AddComponentMenu(AmiliousCore.TAB_CONTEXT_MENU+"Tab Controller")]
    public class TabController : MonoBehaviour {

        #region Inspector Fields ///////////////////////////////////////////////////////////////////////////////////////
        
        [Tooltip("The tab that you want to be selected by default.")] 
        [SerializeField] private TabSelector selectedTab;
        [Tooltip("The tabs that are controlled by this tab controller.")]
        [SerializeField] private List<TabSelector> tabSelectors = new List<TabSelector>();
        [Tooltip("This event is triggered when the selected tab changes.")]
        [SerializeField] private SelectorUnityEvent onSelectionChanged;
        [Tooltip("This event is triggered when a tab is deselected.")]
        [SerializeField] private SelectorUnityEvent onDeselected;
        [Tooltip("This event is triggered when a tab starts being hovered over.")]
        [SerializeField] private SelectorUnityEvent onHoverEnter;
        [Tooltip("This event is triggered when a tab stops being hovered over.")]
        [SerializeField] private SelectorUnityEvent onHoverExit;
        [Tooltip("This event is triggered when a tab is registered.")]
        [SerializeField] private SelectorUnityEvent onTabRegistered;
        [Tooltip("This event is triggered when a tab is unregistered")]
        [SerializeField] private SelectorUnityEvent onTabUnregistered;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Delegates //////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This delegate is used for the <see cref="TabController.OnSelectionChanged"/> event.
        /// <param name="controller">The tab controller.</param>
        /// <param name="previous">The previously selected tab, otherwise null for no previous selection.</param>
        /// <param name="current">The current selected tab, otherwise null for no selected tab.</param>
        /// </summary>
        public delegate void TabActionDelegate(TabController controller, TabSelector previous, TabSelector current);

        /// <summary>
        /// This delegate is used for tab events.
        /// <param name="controller">The tab controller.</param>
        /// <param name="tab">The tab that triggered the event.</param>
        /// </summary>
        public delegate void SimpleTabActionDelegate(TabController controller, TabSelector tab);
       
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Events /////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This class is used for unity events related to tabs.
        /// </summary>
        [System.Serializable] public class SelectorUnityEvent : UnityEvent<TabSelector> { }
        
        /// <summary>
        /// This event is triggered when the selected tab changes.
        /// </summary>
        public static TabActionDelegate OnSelectionChanged;

        /// <summary>
        /// This event is triggered when the pointer or selector exits a tab.
        /// </summary>
        public static SimpleTabActionDelegate OnHoverExit;
        
        /// <summary>
        /// This event is triggered when the pointer or selector enters a tab.
        /// </summary>
        public static SimpleTabActionDelegate OnHoverEnter;

        /// <summary>
        /// This event is triggered when a tab is registered.
        /// </summary>
        public static SimpleTabActionDelegate OnTabRegistered;

        /// <summary>
        /// This event is triggered when a tab is unregistered.
        /// </summary>
        public static SimpleTabActionDelegate OnTabUnregistered;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This property contains the selected tab, otherwise null if no tab is selected.
        /// </summary>
        public TabSelector SelectedTab {
            get => selectedTab;
            private set {
                if(selectedTab == value || !tabSelectors.Contains(value)) return;
                PreviousSelectedTab = selectedTab;
                selectedTab = value;
                if(PreviousSelectedTab)PreviousSelectedTab.Deselect();
                OnSelectionChanged?.Invoke(this,PreviousSelectedTab, selectedTab);
                onSelectionChanged?.Invoke(selectedTab);
            }
        }
        
        /// <summary>
        /// This property contains the previously selected tab, otherwise null if it didn't exist.
        /// </summary>
        public TabSelector PreviousSelectedTab { get; private set; }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region MonoBehavior Methods ///////////////////////////////////////////////////////////////////////////////////
        
        private void Start() => SelectFirstAvailableTab();

        private void OnValidate() {
            SelectFirstAvailableTab();
            var temp = SelectedTab;
            SelectedTab = null;
            foreach(var tab in tabSelectors) tab.Deselect();
            if(temp)temp.Select();
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Private Methods ////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to select the first selectable tab if no tab is currently selected.
        /// </summary>
        private void SelectFirstAvailableTab() {
            if(SelectedTab != null) return;
            foreach(var tab in tabSelectors) {
                if(!tab.IsSelectable) continue;
                SelectedTab = tab;
                break;
            }
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to register a tab to the tab controller.
        /// </summary>
        /// <param name="tab">The tab that you want to register.</param>
        /// <returns>True if the tab was not registered and has been registered, otherwise false.</returns>
        public virtual bool RegisterTab(TabSelector tab) {
            if(tabSelectors.Contains(tab)) return false;
            tabSelectors.Add(tab);
            onTabRegistered?.Invoke(tab);
            OnTabRegistered?.Invoke(this,tab);
            SelectFirstAvailableTab();
            return true;
        }

        /// <summary>
        /// This method is used to unregister a tab from the tab controller.
        /// </summary>
        /// <param name="tab">The tab that you want to unregister.</param>
        /// <returns>True if the tab was registered and has been unregistered, otherwise false.</returns>
        public virtual bool UnregisterTab(TabSelector tab) {
            if(!tabSelectors.Contains(tab)) return false;
            tabSelectors.Remove(tab);
            onTabUnregistered?.Invoke(tab);
            OnTabUnregistered?.Invoke(this,tab);
            if(PreviousSelectedTab==tab) SelectFirstAvailableTab();
            return true;
        }

        /// <summary>
        /// This method is called by <see cref="TabSelector"/> when the pointer or selector starts hovering over a tab.
        /// </summary>
        /// <param name="tab">The tab.</param>
        public virtual void TabHoverEnter(TabSelector tab) {
            onHoverEnter?.Invoke(tab);
            OnHoverEnter?.Invoke(this,tab);
        }

        /// <summary>
        /// This method is called by <see cref="TabSelector"/> when the pointer or selector stops hovering over a tab.
        /// </summary>
        /// <param name="tab">The tab.</param>
        public virtual void TabHoverExit(TabSelector tab) {
            onHoverExit?.Invoke(tab);
            OnHoverExit?.Invoke(this,tab);
        }
        
        /// <summary>
        /// This method is used to select a tab.
        /// </summary>
        /// <param name="tab">The tab that you want to select.</param>
        public virtual void SelectTab(TabSelector tab) => SelectedTab = tab;

        /// <summary>
        /// This method is used to deselect a tab.
        /// </summary>
        /// <param name="tab">The tab that you want to deselect.</param>
        public virtual void DeselectTab(TabSelector tab) {
            onDeselected?.Invoke(tab);
            if(tab != SelectedTab) return;
            SelectedTab = null;
            SelectFirstAvailableTab();
        }

        /// <summary>
        /// This method is used to move the selection to the next selectable tab.
        /// </summary>
        public virtual void MoveSelectionToNext() {
            if(tabSelectors.Count < 1) return; //no other tabs
            var selected = tabSelectors.Contains(SelectedTab) ? 
                tabSelectors.IndexOf(SelectedTab): -1;
            for(var i = 1; i <= tabSelectors.Count; i++) {
                var testIndex = (selected + i) % tabSelectors.Count;
                if(!tabSelectors[testIndex].IsSelectable) continue;
                tabSelectors[testIndex].Select();
                return;
            }
        }

        /// <summary>
        /// This method is used to move the selection to the previous selectable tab.
        /// </summary>
        public virtual void MoveSelectionToPrevious() {
            if(tabSelectors.Count < 1) return; //no other tabs
            var selected = tabSelectors.Contains(selectedTab) ? 
                tabSelectors.IndexOf(SelectedTab) : tabSelectors.Count;
            for(var i = 1; i <= tabSelectors.Count; i++) {
                var testIndex = (selected - i) % tabSelectors.Count;
                if(!tabSelectors[testIndex].IsSelectable) continue;
                tabSelectors[testIndex].Select();
                return;
            }
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}