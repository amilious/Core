using System;
using UnityEngine.EventSystems;

namespace Amilious.Core.TextUtils {
    public class UIEnterExitListener : AmiliousBehavior , IPointerEnterHandler, IPointerExitHandler {

        public event Action OnMouseEnterUI;
        public event Action OnMouseExitUI;

        public void OnPointerEnter(PointerEventData eventData) => OnMouseEnterUI?.Invoke();

        public void OnPointerExit(PointerEventData eventData) => OnMouseExitUI?.Invoke();
        
    }
}