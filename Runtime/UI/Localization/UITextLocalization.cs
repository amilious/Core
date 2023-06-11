
using TMPro;
using System;
using UnityEngine;
using Amilious.Core.Attributes;
using Amilious.Core.Localization;

namespace Amilious.Core.UI.Localization {
    
    public class UITextLocalization : MonoBehaviour {
        
        [SerializeField] private bool useTextMeshPro = true;
        [SerializeField,AmiShowIf(nameof(useTextMeshPro))] private TextMeshProUGUI textMeshProField;
        [SerializeField,AmiHideIf(nameof(useTextMeshPro))] private UnityEngine.UI.Text textField;
        [SerializeField] 
        /*[LocalizedLabel("amilious/localization/ui_text_localizer/localization_key",
             "amilious/localization/ui_text_localizer/localization_key_tooltip")] */
        private LocalizedString localizationKey;
        
        private void Awake() {
            UpdateText();
        }

        #if UNITY_EDITOR
        
        private void OnEnable() {
            AmiliousLocalization.OnLanguageChanged += OnLanguageChanged;
            AmiliousLocalization.OnTranslationUpdated += OnTranslationUpdated;
        }

        private void OnDisable() {
            AmiliousLocalization.OnLanguageChanged -= OnLanguageChanged;
            AmiliousLocalization.OnTranslationUpdated -= OnTranslationUpdated;
        }

        /// <inheritdoc cref="AmiliousLocalization.TranslationUpdatedDelegate" />
        private void OnTranslationUpdated(string language, string key) {
            if(key.Equals(localizationKey.Key,StringComparison.InvariantCulture)) UpdateText();
        }

        /// <inheritdoc cref="AmiliousLocalization.LanguageChangedDelegate"/>
        private void OnLanguageChanged(string previous, string current) => UpdateText();

        private void OnValidate() => UpdateText();
        #endif
        
        private void UpdateText() {
            switch(useTextMeshPro) {
                case false: {
                    if(textField == null) textField = GetComponent<UnityEngine.UI.Text>();
                    if(textField == null) return;
                    textField.text = localizationKey.Value;
                    break;
                }
                case true: {
                    if(textMeshProField == null) textMeshProField = GetComponent<TextMeshProUGUI>();
                    if(textMeshProField == null) return;
                    //the enable is to make sure that the text updates in the scene if not in play mode
                    textMeshProField.enabled = false;
                    textMeshProField.text = localizationKey.Value;
                    // ReSharper disable once Unity.InefficientPropertyAccess
                    textMeshProField.enabled = true;
                    break;
                }
            }
        }
        
    }
}