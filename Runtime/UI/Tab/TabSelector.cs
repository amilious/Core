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

using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Amilious.Core.Attributes;
using Amilious.Core.Extensions;

namespace Amilious.Core.UI.Tab {
    
    /// <summary>
    /// This class is used to control a single tab within a <see cref="TabController"/>.
    /// </summary>
    /// ReSharper disable MemberCanBeProtected.Global
    /// ReSharper disable ClassWithVirtualMembersNeverInherited.Global
    /// ReSharper disable MemberCanBePrivate.Global
    [ExecuteAlways]
    [AddComponentMenu(AmiliousCore.TAB_CONTEXT_MENU+"Tab Selector")]
    public class TabSelector : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, 
        IPointerClickHandler, ISelectHandler, IDeselectHandler, ISubmitHandler {

        #region Inspector Fields ///////////////////////////////////////////////////////////////////////////////////////
        
        [Header("Tab Settings")]
        [Tooltip("The group that the tab belongs to.")]
        [SerializeField] public TabController tabController;
        [Tooltip("(Optional) The game object that you want to be active when this tab is selected.")]
        [SerializeField] public GameObject tabPanel;

        [Tooltip("If true the tab will be selectable, otherwise it will not be selectable.")] 
        [SerializeField] private bool selectable;
        
        [Header("Sprite Settings")]
        [Tooltip("If true the image's sprite will be changed based on the tab's state.")]
        [SerializeField, AmiBool(true)] 
        private bool changeSpriteWithState;
        [Tooltip("The sprite that should be used by default when the tab is not hovered or selected.")]
        [SerializeField, AmiShowIf(nameof(changeSpriteWithState))]
        private Sprite idleSprite;
        [Tooltip("The sprite that should be used when the tab is hovered over.")]
        [SerializeField, AmiShowIf(nameof(changeSpriteWithState))]
        private Sprite hoverSprite;
        [Tooltip("The sprite that should be used when the tab is selected.")]
        [SerializeField, AmiShowIf(nameof(changeSpriteWithState))]
        private Sprite selectedSprite;

        [Header("Color Settings")]
        [Tooltip("If true the image's color will be changed based on the tab's state.")]
        [SerializeField, AmiBool(true)] 
        private bool changeColorWithState;
        [Tooltip("The color that should be used by default when the tab is not hovered or selected.")]
        [SerializeField, AmiShowIf(nameof(changeColorWithState)), AmiColor]
        private Color idleColor;
        [Tooltip("The color that should be used when the tab is hovered over.")]
        [SerializeField, AmiShowIf(nameof(changeColorWithState)), AmiColor]
        private Color hoverColor;
        [Tooltip("The color that should be used when the tab is selected.")]
        [SerializeField, AmiShowIf(nameof(changeColorWithState)), AmiColor]
        private Color selectedColor;
        
        [Header("Events")]
        [Tooltip("This event is triggered when the tab is hovered over.")]
        [SerializeField] private UnityEvent onHoverEnter;
        [Tooltip("This event is triggered when the tab is no longer hovered over.")]
        [SerializeField] private UnityEvent onHoverExit;
        [Tooltip("This event is triggered when the tab is selected.")]
        [SerializeField] private UnityEvent onSelect;
        [Tooltip("This event is triggered when the tab is deselected.")]
        [SerializeField] private UnityEvent onDeselect;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Private Fields /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This field is used to cache the background image.
        /// </summary>
        private Image _background;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This property contains the <see cref="GameObject"/> that will be set to active when this tab is selected.
        /// </summary>
        public GameObject TabPanel => tabPanel;
        
        /// <summary>
        /// If this property is true the background image's color will be changed based on the tab's state.
        /// </summary>
        public bool WillChangeColorWithState {
            get => changeColorWithState;
            set { changeColorWithState = value; UpdateState(); }
        }

        /// <summary>
        /// This property contains sprite that will be used when the tab is in the idle state, if change sprite with
        /// state is enabled.
        /// </summary>
        /// <seealso cref="WillChangeSpriteWithState"/>
        public Sprite IdleSprite {
            get => idleSprite;
            set { idleSprite = value; UpdateState(); }
        }

        /// <summary>
        /// This property contains sprite that will be used when the tab is in the hover state, if change sprite with
        /// state is enabled.
        /// </summary>
        /// <seealso cref="WillChangeSpriteWithState"/>
        public Sprite HoverSprite {
            get => hoverSprite;
            set { hoverSprite = value; UpdateState(); }
        }

        /// <summary>
        /// This property contains sprite that will be used when the tab is in the selected state, if change sprite with
        /// state is enabled.
        /// </summary>
        /// <seealso cref="WillChangeSpriteWithState"/>
        public Sprite SelectedSprite {
            get => selectedSprite;
            set { selectedSprite = value; UpdateState(); }
        }

        /// <summary>
        /// If this property is true the background image's sprite will be changed based on the tab's state.
        /// </summary>
        public bool WillChangeSpriteWithState {
            get => changeSpriteWithState;
            set { changeSpriteWithState = value; UpdateState(); }
        }

        /// <summary>
        /// This property contains image color that will be used when the tab is in the idle state, if change color with
        /// state is enabled.
        /// </summary>
        /// <seealso cref="WillChangeColorWithState"/>
        public Color IdleColor {
            get => idleColor;
            set { idleColor = value; UpdateState(); }
        }

        /// <summary>
        /// This property contains image color that will be used when the tab is in the hover state, if change color
        /// with state is enabled.
        /// </summary>
        /// <seealso cref="WillChangeColorWithState"/>
        public Color HoverColor {
            get => hoverColor;
            set { hoverColor = value; UpdateState(); }
        }

        /// <summary>
        /// This property contains image color that will be used when the tab is in the selected state, if change color
        /// with state is enabled.
        /// </summary>
        /// <seealso cref="WillChangeColorWithState"/>
        public Color SelectedColor {
            get => selectedColor;
            set { selectedColor = value; UpdateState(); }
        }

        /// <summary>
        /// This property contains the background image for this <see cref="TabSelector"/>.
        /// </summary>
        public Image BackgroundImage => this.GetCacheComponent(ref _background);
        
        /// <summary>
        /// This property is used to get or set the select-ability of this tab.
        /// </summary>
        public bool IsSelectable { 
            get => selectable;
            set => selectable = value;
        }
        
        /// <summary>
        /// This property is true if the tab is currently being hovered over, otherwise false.
        /// </summary>
        public bool IsHovered { get; private set; }

        /// <summary>
        /// This property is true if the tab is currently the selected tab for the <see cref="TabController"/>, otherwise
        /// false.
        /// </summary>
        public bool IsSelected => TabController && TabController.SelectedTab == this;

        /// <summary>
        /// This property contains the tab's <see cref="TabController"/>.
        /// </summary>
        public TabController TabController => tabController;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region MonoBehavior Methods ///////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is called when the script is being loaded.
        /// </summary>
        private void Awake() {
            tabController ??= gameObject.GetComponentInParent<TabController>(true);
            tabController.RegisterTab(this);
            UpdateState();
        }

        /// <summary>
        /// This method is called when the component is validated.
        /// </summary>
        private void OnValidate() {
            //if the tab group is null try find it in the parent gameObjects
            tabController ??= gameObject.GetComponentInParent<TabController>(true);
            UpdateState();
        }

        /// <summary>
        /// This method is called when the component is destroyed.
        /// </summary>
        private void OnDestroy() {
            if(tabController == null) return;
            TabController.DeselectTab(this);
            TabController.UnregisterTab(this);
            UpdateState();
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Interface Methods //////////////////////////////////////////////////////////////////////////////////////

        /// <inheritdoc />
        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData) => HoverEnter();
        /// <inheritdoc />
        void ISelectHandler.OnSelect(BaseEventData eventData) => HoverEnter();
        /// <inheritdoc />
        void IPointerExitHandler.OnPointerExit(PointerEventData eventData) => HoverExit();
        /// <inheritdoc />
        void IDeselectHandler.OnDeselect(BaseEventData eventData) => HoverExit();
        /// <inheritdoc />
        void IPointerClickHandler.OnPointerClick(PointerEventData eventData) => Select();
        /// <inheritdoc />
        void ISubmitHandler.OnSubmit(BaseEventData eventData) => Select();
        private void OnMouseUpAsButton() => Select();
        private void OnMouseEnter() => HoverEnter();
        private void OnMouseExit() => HoverExit();

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to update the state of the tab.
        /// </summary>
        /// <remarks>If this method is overwritten, make sure to call the base.UpdateState() method.</remarks>
        public virtual void UpdateState() {
            if(changeSpriteWithState) //update the sprite if it changes with the state
                BackgroundImage.sprite = IsSelected ? selectedSprite : IsHovered ? hoverSprite : idleSprite;
            if(changeColorWithState) //update the color if it changes with the state.
                BackgroundImage.color = IsSelected ? selectedColor : IsHovered ? hoverColor : idleColor;
            //toggle the tab panel based on if the tab is selected or not.
            if(tabPanel) tabPanel.SetActive(IsSelected);
        }
        
        /// <summary>
        /// This method is used to trigger the start of the hovering state for the tab.
        /// </summary>
        /// <remarks>If this method is overwritten, make sure to call the base.HoverEnter() method.</remarks>
        public virtual void HoverEnter() {
            IsHovered = true;
            if(TabController) TabController.TabHoverEnter(this);
            UpdateState();
            onHoverEnter?.Invoke();
        }

        /// <summary>
        /// This method is used to trigger the end of the hovering state for the tab.
        /// </summary>
        /// <remarks>If this method is overwritten, make sure to call the base.HoverExit() method.</remarks>
        public virtual void HoverExit() {
            IsHovered = false;
            if(TabController) TabController.TabHoverExit(this);
            UpdateState();
            onHoverExit?.Invoke();
        }

        /// <summary>
        /// This method is used to trigger the selection of this tab if the tab is selectable.
        /// </summary>
        /// <remarks>If this method is overwritten, make sure to call the base.Select() method.</remarks>
        public virtual void Select() {
            if(!IsSelectable) return;
            if(TabController == null) {
                Debug.LogWarning("No TabGroup assigned!");
                return;
            }
            if(TabController.SelectedTab == this) return;
            tabController.SelectTab(this);
            UpdateState();
            onSelect?.Invoke();
        }

        /// <summary>
        /// This method is used to trigger the deselection of this tab.
        /// </summary>
        /// <remarks>If this method is overwritten, make sure to call the base.Deselect() method.</remarks>
        public virtual void Deselect() {
            if(IsSelected) TabController.DeselectTab(this);
            UpdateState();
            onDeselect?.Invoke();
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}