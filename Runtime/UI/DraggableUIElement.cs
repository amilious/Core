using UnityEngine;
using UnityEngine.EventSystems;

namespace Amilious.Core.UI {
    
    [RequireComponent(typeof(CanvasGroup))]
    public class DraggableUIElement : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler {
        
        private RectTransform _rectTransform;
        private CanvasGroup _canvasGroup;
        private Vector3 _offset;

        private void Awake() {
            _rectTransform = GetComponent<RectTransform>();
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public void OnBeginDrag(PointerEventData eventData) {
            _canvasGroup.alpha = 0.6f;
            _canvasGroup.blocksRaycasts = false;
            RectTransformUtility.ScreenPointToWorldPointInRectangle(_rectTransform, eventData.position, 
                eventData.pressEventCamera, out var worldPoint);
            _offset = _rectTransform.position - worldPoint;
        }

        public void OnDrag(PointerEventData eventData) {
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(_rectTransform, eventData.position, 
                eventData.pressEventCamera, out var worldPoint)) {
                _rectTransform.position = ClampToScreen(worldPoint + _offset);
            }
        }

        public void OnEndDrag(PointerEventData eventData) {
            _canvasGroup.alpha = 1f;
            _canvasGroup.blocksRaycasts = true;
        }
        
        private Vector3 ClampToScreen(Vector3 position) {
            Vector3 clampedPosition = position;
            Vector3 screenSize = new Vector3(Screen.width, Screen.height, 0f);
            Vector3 halfSize = _rectTransform.rect.size / 2f;
            // Adjust the position to stay within the screen bounds
            clampedPosition.x = Mathf.Clamp(clampedPosition.x, halfSize.x, screenSize.x - halfSize.x);
            clampedPosition.y = Mathf.Clamp(clampedPosition.y, halfSize.y, screenSize.y - halfSize.y);
            return clampedPosition;
        }
        
    }
}