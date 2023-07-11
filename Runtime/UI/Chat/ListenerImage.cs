using System;
using UnityEngine;
using UnityEngine.UI;

namespace Amilious.Core.UI.Chat {
    public class ListenerImage : Image {

        public delegate void SizeChangedDelegate(Vector2 before, Vector2 after);
        
        public event SizeChangedDelegate OnRectSizeChanged;

        private Vector2 _size;

        public Vector2 RectSize => _size;
        
        protected override void Awake() {
            base.Awake();
            _size = rectTransform.sizeDelta;
        }

        protected override void OnRectTransformDimensionsChange() {
            base.OnRectTransformDimensionsChange();
            var old = _size;
            _size = rectTransform.sizeDelta;
            if(_size == old) return;
            OnRectSizeChanged?.Invoke(old,_size);
        }
    }
}