using UnityEngine;
using Amilious.Core.Extensions;

namespace Amilious.Core.UI.Component.ShowHide {

    [DisallowMultipleComponent]
    [RequireComponent(typeof(UIComponent), typeof(RectTransform))]
    public abstract class AbstractUIShowHide : AmiliousBehavior {

        private UIComponent _uiComponent;
        private RectTransform _rectTransform;

        public UIComponent UIComponent => this.GetCacheComponent(ref _uiComponent);

        public RectTransform RectTransform => this.GetCacheComponent(ref _rectTransform);
        
        public abstract void OnShow(CanvasGroup canvasGroup);

        public abstract void OnHide(CanvasGroup canvasGroup);

    }
}