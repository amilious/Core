using Amilious.Core.Attributes;
using TMPro;
using UnityEngine;
using Amilious.Core.Extensions;

namespace Amilious.Core.UI.MISC {
    
    [ExecuteAlways]
    public class ChildIdDisplay : AmiliousBehavior {
    
        [SerializeField] private TextMeshProUGUI textComponent;
        [SerializeField] private string textPretext;
        [SerializeField] private int parentLevel = 0;
        [SerializeField] private bool setTextComponentName;
        [SerializeField, AmiShowIf(nameof(setTextComponentName))]
        private string textComponentPreText;
        [SerializeField] private bool setGameObjectName;
        [SerializeField, AmiShowIf(nameof(setGameObjectName))]
        private string gameObjectPreText;
        
        private TextMeshProUGUI TextComponent => this.GetCacheComponent(ref textComponent);

        private void OnValidate() => UpdateText();

        private void Awake() => UpdateText();

        public void UpdateText() {
            var text = gameObject.GetChildId(parentLevel).ToString();
            if(TextComponent != null) {
                TextComponent.text = textPretext + text;
                if(setTextComponentName)
                    TextComponent.gameObject.name = textComponentPreText + text;
            }
            if(setGameObjectName) name = gameObjectPreText + text;
        }
        
    }
}