using Amilious.Core.Attributes;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Amilious.Core.UI.Component {
    
    [AmiHelpBox(MSG_BOX,HelpBoxType.Info)]
    public class ComponentFocusRelay : AmiliousBehavior, IPointerDownHandler {

        private const string MSG_BOX = "This component is used to notify the ui component about the moused down event.";
        
        [SerializeField] private UIComponent uiComponent;

        private void Awake() {
            if(uiComponent == null) uiComponent = GetComponentInParent<UIComponent>();
        }

        public void OnPointerDown(PointerEventData eventData) {
            if(uiComponent!=null) uiComponent.InteractableMouseDown(eventData);
        }
    }
}