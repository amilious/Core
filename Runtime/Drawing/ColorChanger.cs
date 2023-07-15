using UnityEngine;

namespace Amilious.Core.Drawing {
    
    [SerializeField]
    public class ColorChanger {

        [SerializeField] private Color originalColor;
        [SerializeField] private Color newColor;
        [SerializeField] private float threshold;

        public Color OriginalColor => originalColor;
        public Color NewColor => newColor;
        public float Threshold => threshold;

    }
    
}