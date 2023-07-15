
using UnityEngine;
using Amilious.Core.Extensions;
using Amilious.Core.UI.Cursors;
using UnityEngine.EventSystems;

namespace Amilious.Core.UI.Component.States {
    
    public class Moving : UIState {

        private Vector3 _moveOffset;

        public Moving(UIComponent component) : base(component, UIStatesType.Moving) { }

        public override void OnEnterState(UIStatesType previousState, PointerEventData pointerData) {
            //the ui is not movable
            if(!Component.Movable) Component.SetState(UIStatesType.Idle, null);
            if(!Component.Visible) Component.SetState(UIStatesType.Idle, null);
            if(pointerData!=null) OnBeginDrag(pointerData); //make sure that the begin drag is called
        }
        
        public override void OnExitState(UIStatesType nextState) {
            _moveOffset = Vector3.zero;
            CursorController.ReturnToDefault();
            if(Component.Visible) {
                CanvasGroup.alpha = 1;
                CanvasGroup.blocksRaycasts = true;
                return;
            }
            CanvasGroup.alpha = 0f;
            CanvasGroup.blocksRaycasts = false;
        }

        public override void OnBeginDrag(PointerEventData eventData) {
            CursorController.SetCursor(DefaultCursors.Drag);
            CanvasGroup.alpha = 0.6f;
            CanvasGroup.blocksRaycasts = false;
            RectTransformUtility.ScreenPointToWorldPointInRectangle(RectTransform, eventData.position, 
                eventData.pressEventCamera, out var worldPoint);
            _moveOffset = RectTransform.position - worldPoint;
        }

        public override void OnDrag(PointerEventData eventData) {
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(RectTransform, eventData.position, 
                eventData.pressEventCamera, out var worldPoint)) {
                RectTransform.TryClampUIToScreen(UIController.RectTransform, worldPoint + _moveOffset);
            }
        }

        public override void OnEndDrag(PointerEventData eventData) {
            Component.SetState(UIStatesType.Idle, null);
        }
        
    }
    
}