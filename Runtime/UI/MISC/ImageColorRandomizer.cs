using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Amilious.Core.UI.MISC {
    
    [ExecuteAlways]
    [RequireComponent(typeof(Image))]
    public class ImageColorRandomizer : AmiliousBehavior {
        private void Awake() {
            var color = Random.ColorHSV();
            color.a = 1f;
            GetComponent<Image>().color = color;
        }
    }
    
}