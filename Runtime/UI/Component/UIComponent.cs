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
using UnityEngine.Events;
using Amilious.Core.Attributes;
using Amilious.Core.Extensions;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using System.Collections.Generic;
using Amilious.Core.UI.Component.ShowHide;
using Amilious.Core.UI.Component.States;

namespace Amilious.Core.UI.Component {
    
    /// <summary>
    /// This class is used to make a rect transform a UI component.
    /// </summary>
    /// ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
    [AmiHelpBox(HELP_MESSAGE,HelpBoxType.Info)]
    [HelpURL("https://amilious.gitbook.io/core/runtime/ui/ui-component")]
    [DisallowMultipleComponent,AddComponentMenu("Amilious/UI/Component/UI Component")]
    [RequireComponent(typeof(RectTransform), typeof(CanvasGroup))]
    public class UIComponent : AmiliousBehavior, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerDownHandler, 
        IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler, IPointerUpHandler {

        #region Constants //////////////////////////////////////////////////////////////////////////////////////////////
        
        private const string HELP_MESSAGE = "This component is used to make the object act as a UI component.  " +
            "The parent object must have a UIController for it to work properly.";
        private const string OPTIONS = "Options";
        private const string EVENTS = "Events";

        [Serializable] public class StateEvent : UnityEvent<UIStatesType> { }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Inspector Fields ///////////////////////////////////////////////////////////////////////////////////////

        [Tooltip("If true the component will be visible.")]
        [SerializeField, AmiTab(OPTIONS), AmiBool(true)] private bool visible = true;
        [Tooltip("If true the component will be focusable.")]
        [SerializeField, AmiTab(OPTIONS), AmiBool(true)] private bool focusable = true;
        [Tooltip("If true the component will be movable.")]
        [SerializeField, AmiTab(OPTIONS), AmiBool(true)] private bool movable = true;
        [Tooltip("If true the component will be resizable.")]
        [SerializeField, AmiTab(OPTIONS), AmiBool(true)] private bool resizable = false;
        
        [FormerlySerializedAs("edgeDetectionThreshold")]
        [Header("Resizing Options")]
        [Tooltip("This value controls the resizing edge size.")]
        [SerializeField, AmiTab(OPTIONS), AmiShowIf(nameof(resizable))] private float edgeThreshold = 4;
        [Tooltip("This value is the minimum size of the component.")]
        [SerializeField, AmiTab(OPTIONS), AmiShowIf(nameof(resizable)), AmiVector(xLabel:"width",yLabel:"height")] 
        private Vector2 minSize = new Vector2(200,200);
        
        [Tooltip("This event is triggered when the component gains focus.")]
        [SerializeField, AmiTab(EVENTS)] private UnityEvent onGainedFocus;
        [Tooltip("This event is triggered when the component loses focus.")]
        [SerializeField, AmiTab(EVENTS)] private UnityEvent onLostFocus;
        [FormerlySerializedAs("OnLoseState")]
        [Tooltip("This event is triggered when the component's state is lost.")]
        [SerializeField, AmiTab(EVENTS)] private StateEvent onExitState = new StateEvent();
        [FormerlySerializedAs("OnGainState")]
        [Tooltip("This event is triggered when the component gains a state.")] 
        [SerializeField, AmiTab(EVENTS)] private StateEvent onEnterState = new StateEvent();
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Private Fields /////////////////////////////////////////////////////////////////////////////////////////
        
        private Vector3 _moveOffset;
        private bool _isMoving;
        private bool _isResizing;
        private bool _overChild;
        private CanvasGroup _canvasGroup;
        private UIController _controller;
        private RectTransform _rectTransform;
        private UIState _state;
        private Dictionary<UIStatesType, UIState> UIStates = new Dictionary<UIStatesType, UIState>();
        private AbstractUIShowHide _uiShowHide;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////

        public UIState CurrentState => _state;

        public Vector2 MinSize => minSize;

        /// <summary>
        /// This property contains the rect transform.
        /// </summary>
        public RectTransform RectTransform {
            get {
                if(_rectTransform != null) return _rectTransform;
                _rectTransform = (RectTransform)transform;
                return _rectTransform;
            }
        }

        /// <summary>
        /// This property contains the <see cref="UIController"/>.
        /// </summary>
        public UIController UIController {
            get {
                if(_controller != null) return _controller;
                _controller = GetComponentInParent<UIController>(true);
                if(_controller==null)Debug.LogException(
                    new Exception("UIComponents must have a parent object with a UIController!"),this);
                return _controller;
            }
        }

        /// <summary>
        /// This property contains the <see cref="CanvasGroup"/>.
        /// </summary>
        public CanvasGroup CanvasGroup => this.GetCacheComponent(ref _canvasGroup);

        public bool IsFocussed => (UIController!=null) && UIController.FocussedComponent == this;
        
        public bool Focusable {
            get => focusable;
            set {
                focusable = value;
                if(UIController && UIController.FocussedComponent == this) {
                    UIController.FocusUIComponent(null);
                }
            }
        }

        public bool Movable {
            get => movable;
            set {
                movable = value;
                if(!movable&&CurrentState?.Type == UIStatesType.Moving)
                    SetState(UIStatesType.Idle,null);
            }
        }

        public bool Resizable {
            get => resizable;
            set {
                resizable = value;
                if(!resizable&&CurrentState?.Type == UIStatesType.Resizing)
                    SetState(UIStatesType.Idle,null);
            }
        }

        public bool Visible {
            get => visible;
            set {
                if(visible == value) return;
                visible = value;
                SetState(UIStatesType.Idle,null);
                if(value) {
                    if(!_uiShowHide) CanvasGroup.alpha = 1f;
                    else _uiShowHide.OnShow(CanvasGroup);
                    CanvasGroup.blocksRaycasts = true;
                }else {
                    if(!_uiShowHide)CanvasGroup.alpha = 0f;
                    else _uiShowHide.OnHide(CanvasGroup);
                    CanvasGroup.blocksRaycasts = false;
                    if(UIController && UIController.FocussedComponent == this) {
                        UIController.FocusUIComponent(null);
                    }
                }
            }
        }

        public bool Enabled {
            get => gameObject.activeSelf;
            set {
                gameObject.SetActive(value);
                if(UIController && UIController.FocussedComponent == this) {
                    UIController.FocusUIComponent(null);
                }
            }
        }

        /// <summary>
        /// This property is true when the component is being moved.
        /// </summary>
        public bool IsMoving => CurrentState.Type == UIStatesType.Moving;

        public bool IsResizing => CurrentState.Type == UIStatesType.Resizing;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        private void Awake() {
            UIStates.Add(UIStatesType.Idle, new Idle(this));
            UIStates.Add(UIStatesType.Moving, new Moving(this));
            UIStates.Add(UIStatesType.Resizing, new Resizing(this));
            SetState(UIStatesType.Idle,null);
            _uiShowHide = GetComponent<AbstractUIShowHide>();
            if(visible) {
                CanvasGroup.blocksRaycasts = true;
                CanvasGroup.alpha = 1f;
            } else {
                CanvasGroup.blocksRaycasts = false;
                CanvasGroup.alpha = 0f;
            }
        }

        private void OnValidate() {
            if(Application.isPlaying) return;
            if(visible) {
                CanvasGroup.blocksRaycasts = true;
                CanvasGroup.alpha = 1f;
            } else {
                CanvasGroup.blocksRaycasts = false;
                CanvasGroup.alpha = 0f;
            }
        }

        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////

        public void Show() { Visible = true; }

        public void Hide() { Visible = false; }
        
        /// <summary>
        /// This method is used to focus the ui component and move it to the topmost position.
        /// </summary>
        public bool FocusUIComponent() {
            if(!Focusable) return false;
            if(!Visible) return false;
            if(!Enabled) return false;
            return UIController != null && UIController.FocusUIComponent(this);
        }

        public void AddOnExitStateListener(UnityAction<UIStatesType> callback) => 
            onExitState.AddListener(callback);

        public void RemoveOnExitStateListener(UnityAction<UIStatesType> callback) =>
            onExitState.RemoveListener(callback);
        
        public void AddOnEnterStateListener(UnityAction<UIStatesType> callback) => 
            onEnterState.AddListener(callback);
        
        public void RemoveOnEnterStateListener(UnityAction<UIStatesType> callback) =>
            onEnterState.RemoveListener(callback);

        public void AddOnGainedFocusListener(UnityAction callback) => onGainedFocus.AddListener(callback);

        public void RemoveOnGainedFocusListener(UnityAction callback) => onGainedFocus.RemoveListener(callback);

        public void AddOnLostFocusListener(UnityAction callback) => onLostFocus.AddListener(callback);

        public void RemoveLostFocusListener(UnityAction callback) => onLostFocus.RemoveListener(callback);
        
        /// <summary>
        /// This method is called when the ui component gains focus.
        /// </summary>
        /// <remarks>This method triggers the <see cref="onGainedFocus"/> event.</remarks>
        public virtual void OnGainFocus() {
            CurrentState?.OnGainFocus();
            onGainedFocus?.Invoke();
        }

        /// <summary>
        /// This method is called when the ui component loses focus.
        /// </summary>
        /// <remarks>This method triggers the <see cref="onLostFocus"/> event.</remarks>
        public virtual void OnLoseFocus() {
            CurrentState?.OnLoseFocus();
            onLostFocus?.Invoke();
        }

        /// <inheritdoc />
        public void OnDrag(PointerEventData eventData) => CurrentState?.OnDrag(eventData);

        /// <inheritdoc />
        public void OnBeginDrag(PointerEventData eventData) => CurrentState?.OnBeginDrag(eventData);

        /// <inheritdoc />
        public void OnEndDrag(PointerEventData eventData) => CurrentState?.OnEndDrag(eventData);
        
        /// <inheritdoc />
        public void OnPointerDown(PointerEventData eventData) {
            FocusUIComponent();
            CurrentState?.OnPointerDown(eventData);
        }

        public void InteractableMouseDown(PointerEventData eventData) { FocusUIComponent();        }

        /// <inheritdoc />
        public void OnPointerEnter(PointerEventData eventData) => CurrentState?.OnPointerEnter(eventData);

        /// <inheritdoc />
        public void OnPointerExit(PointerEventData eventData) => CurrentState?.OnPointerExit(eventData);

        public void OnPointerUp(PointerEventData eventData) => CurrentState?.OnPointerUp(eventData);

        /// <inheritdoc />
        public void OnPointerMove(PointerEventData eventData) {
            var overChild = eventData.pointerCurrentRaycast.gameObject != gameObject;
            CurrentState?.OnPointerMove(eventData,overChild);
            if(overChild && !_overChild) { _overChild = true; CurrentState?.OnPointerExit(eventData,true); }
            if(!overChild && _overChild) { _overChild = false; CurrentState?.OnPointerEnter(eventData,true); }
        }
        
        public void SetState(UIStatesType state, PointerEventData pointerEventData) {
            if(!UIStates.TryGetValueFix(state, out var newState)) newState = UIStates[UIStatesType.Idle];
            if(newState == _state) return;
            var previous = _state;
            _state = newState;
            var stateType = previous?.Type ?? UIStatesType.Idle;
            previous?.OnExitState(_state.Type);
            onExitState?.Invoke(stateType);
            _state.OnEnterState(stateType, pointerEventData);
            onEnterState?.Invoke(_state.Type);
        }
        
        public ComponentEdge GetEdge(Vector3 mousePosition) {
            //get the corners
            var corners = new Vector3[4];
            RectTransform.GetWorldCorners(corners);
            //get basic directions
            var isLeft = Mathf.Abs(corners[1].x - mousePosition.x)<=edgeThreshold;
            var isTop = Mathf.Abs(corners[1].y - mousePosition.y) <= edgeThreshold;
            var isRight = Mathf.Abs(corners[3].x - mousePosition.x)<=edgeThreshold;
            var isBottom = Mathf.Abs(corners[3].y - mousePosition.y)<=edgeThreshold;
            // Check the relative position of the mouse to determine the edge
            if (isLeft && isTop) return ComponentEdge.TopLeft;
            if (!isRight && isTop) return ComponentEdge.Top;
            if (isRight && isTop) return ComponentEdge.TopRight;
            if (isLeft && isBottom) return ComponentEdge.BottomLeft;
            if (isRight && isBottom) return ComponentEdge.BottomRight;
            if (isBottom) return ComponentEdge.Bottom;
            if (isRight) return ComponentEdge.Right;
            return isLeft ? ComponentEdge.Left : ComponentEdge.None;
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        
    }
}