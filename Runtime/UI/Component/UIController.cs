using Amilious.Core.UI.Component;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Amilious.Core.UI {
    
    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent,AddComponentMenu("Amilious/UI/UI Controller")]
    public class UIController : AmiliousBehavior {

        #region Inspector Fields ///////////////////////////////////////////////////////////////////////////////////////

        [SerializeField] private Canvas canvas;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Private Fields /////////////////////////////////////////////////////////////////////////////////////////
        
        private RectTransform _rectTransform;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
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
        /// This property contains the focussed component.
        /// </summary>
        public UIComponent FocussedComponent { get; private set; }

        /// <summary>
        /// This property contains the controllers canvas.
        /// </summary>
        public Canvas Canvas {
            get {
                if(canvas != null) return canvas;
                canvas = GetComponentInChildren<Canvas>();
                return canvas;
            }
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Delegates //////////////////////////////////////////////////////////////////////////////////////////////

        public delegate void FocusChangedDelegate(UIController controller, UIComponent component);
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Events /////////////////////////////////////////////////////////////////////////////////////////////////

        public static FocusChangedDelegate OnFocusChanged;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to focus a ui component.
        /// </summary>
        /// <param name="uiComponent">The component that you want to focus.</param>
        /// <returns>True if the component is focussed, otherwise false.</returns>
        public bool FocusUIComponent(UIComponent uiComponent) {
            if(!uiComponent.Focusable) return false;
            var hadFocus = uiComponent != FocussedComponent;
            uiComponent.transform.SetAsLastSibling();
            if(!hadFocus) FocussedComponent.OnLoseFocus();
            FocussedComponent = uiComponent;
            if(!hadFocus) FocussedComponent.OnGainFocus();
            OnFocusChanged?.Invoke(this,FocussedComponent);
            return true;
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}