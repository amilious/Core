using System.Collections.Generic;
using Amilious.Core.Extensions;
using UnityEngine;

namespace Amilious.Core.UI.Component {
    
    [RequireComponent(typeof(RectTransform),typeof(Canvas))]
    [DisallowMultipleComponent,AddComponentMenu("Amilious/UI/UI Controller")]
    public class UIController : AmiliousBehavior {
        
        //TODO: Make UIController focussable, vissible, or enabled so that all components can be set at once.

        #region Inspector Fields ///////////////////////////////////////////////////////////////////////////////////////

        [SerializeField] private Canvas canvas;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Private Fields /////////////////////////////////////////////////////////////////////////////////////////

        private static readonly HashSet<UIController> UIControllers = new HashSet<UIController>();
        private RectTransform _rectTransform;
        private Canvas _canvas;

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

        public Canvas Canvas => this.GetCacheComponent(ref _canvas);
        
        /// <summary>
        /// This property contains the focussed component.
        /// </summary>
        public UIComponent FocussedComponent { get; private set; }

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
            if(!uiComponent.Visible) return false;
            if(!uiComponent.Enabled) return false;
            var lastFocussed = FocussedComponent;
            var newFocus = uiComponent == lastFocussed;
            //make sure on top
            uiComponent.transform.SetAsLastSibling();
            if(!newFocus && lastFocussed!=null) lastFocussed.OnLoseFocus();
            FocussedComponent = uiComponent; //change the focus
            if(!newFocus  && FocussedComponent!=null) FocussedComponent.OnGainFocus();
            if(!newFocus && FocussedComponent!=null) OnFocusChanged?.Invoke(this,FocussedComponent);
            return true;
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        private void Awake() {

            //add reference to the ui controller for static methods that will be added later.
            UIControllers.Add(this);
        }
    }
}