using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Amilious.Core.UI.Component.States {
    
    [Serializable]
    public abstract class UIState {

        /// <summary>
        /// This property contains the component that the state is for.
        /// </summary>
        protected UIComponent Component { get; }
        
        /// <summary>
        /// This property contains the rect transform of the component.
        /// </summary>
        protected RectTransform RectTransform { get; }

        /// <summary>
        /// This property contains the UIController for the component or null.
        /// </summary>
        protected UIController UIController => Component.UIController;
        
        /// <summary>
        /// This property contains the CanvasGroup for the component.
        /// </summary>
        protected CanvasGroup CanvasGroup { get; }
        
        /// <summary>
        /// This property contains the state type
        /// </summary>
        public UIStatesType Type { get; }

        #region Constructors ///////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This constructor is used for all ui states.
        /// </summary>
        /// <param name="component">The component the state is for.</param>
        /// <param name="stateType">The state type.</param>
        protected UIState(UIComponent component, UIStatesType stateType) {
            Component = component;
            RectTransform = component.RectTransform;
            CanvasGroup = component.CanvasGroup;
            Type = stateType;
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region State Change Methods ///////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This method is called when the state is changing from active.
        /// </summary>
        /// <param name="nextState">The next state.</param>
        public abstract void OnExitState(UIStatesType nextState);

        /// <summary>
        /// This method is called when the state is changing to active.
        /// </summary>
        /// <param name="previousState">The previous state before the change.</param>
        /// <param name="pointerData">The pointer data that changed the state or null if first state.</param>
        public abstract void OnEnterState(UIStatesType previousState, PointerEventData pointerData);
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        public virtual void OnDrag(PointerEventData eventData) { }

        public virtual  void OnBeginDrag(PointerEventData eventData) { }

        public virtual  void OnEndDrag(PointerEventData eventData) { }
        
        public virtual void OnPointerUp(PointerEventData eventData) { }

        public virtual  void OnPointerDown(PointerEventData eventData) { }

        public virtual  void OnPointerEnter(PointerEventData eventData, bool softEnter = false) { }

        public virtual  void OnPointerExit(PointerEventData eventData, bool softExit = false) { }

        public virtual  void OnPointerMove(PointerEventData eventData, bool overChild) { }

        public virtual void OnGainFocus() { }

        public virtual void OnLoseFocus() => Component.SetState(UIStatesType.Idle, null);

        
    }

}