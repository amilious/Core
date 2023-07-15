using Amilious.Core.Attributes;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Amilious.Core.UI.Cursors {
    public class CursorProvider : AmiliousBehavior, IPointerEnterHandler, IPointerExitHandler {

        [SerializeField, AmiBool(true)] private bool useCustomCursor;
        [SerializeField, AmiHideIf(nameof(useCustomCursor))] private DefaultCursors defaultCursor;
        [SerializeField, AmiShowIf(nameof(useCustomCursor))] private DefaultCursors customCursor;
        
        public void OnPointerEnter(PointerEventData eventData) {
            CursorController.SetCursor(useCustomCursor ? customCursor : defaultCursor);
        }

        public void OnPointerExit(PointerEventData eventData) {
            CursorController.ReturnToDefault();
        }
    }
}