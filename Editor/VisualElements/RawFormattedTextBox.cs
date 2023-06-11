
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using Amilious.Core.Localization;
using Amilious.Core.Editor.Extensions;

namespace Amilious.Core.Editor.VisualElements {
    
    /// <summary>
    /// This class is used to control the raw formatted text box.
    /// </summary>
    public class RawFormattedTextBox : VisualElement {

        #region Constants //////////////////////////////////////////////////////////////////////////////////////////////
        
        private const string LABEL = "Label";
        private const string RAW_FIELD = "RawField";
        private const string SCROLL_VIEW = "ScrollView";
        private const string SWITCH_BUTTON = "SwitchButton";
        private const string FORMATTED_FIELD = "FormattedField";
        private const string ASSET_PATH = "Assets/Amilious/Core/Editor/VisualElements/RawFormattedTextBox.uxml";

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
      
        #region Private Fields /////////////////////////////////////////////////////////////////////////////////////////
        
        private Label _label;
        private bool _localized;
        //private string _localizedKey;
        private TextField _rawField;
        private Label _formattedField;
        private ScrollView _scrollView;
        private ToolbarToggle _switchButton;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Delegates //////////////////////////////////////////////////////////////////////////////////////////////
        
        public delegate void ValueChangedDelegate(string value);

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Events /////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This event is triggered when the value changes
        /// </summary>
        public event ValueChangedDelegate OnValueChanged;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region UXML Requirement ///////////////////////////////////////////////////////////////////////////////////////

        public new class UxmlFactory : UxmlFactory<RawFormattedTextBox,UxmlTraits> { }

        public new class UxmlTraits : VisualElement.UxmlTraits {

            private readonly UxmlStringAttributeDescription _labelDescription =
                new UxmlStringAttributeDescription() { name = "Label", defaultValue = "Label"};

            /*private readonly UxmlBoolAttributeDescription _localizationDescription =
                new UxmlBoolAttributeDescription() { name = "Is Localized Label", defaultValue = false };*/
            
            private readonly UxmlStringAttributeDescription _valueDescription = 
                new UxmlStringAttributeDescription() { name = "Value", defaultValue = ""};

            private readonly UxmlBoolAttributeDescription _editModeDescription =
                new UxmlBoolAttributeDescription() { name = "Edit Mode", defaultValue = false };

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc) {
                base.Init(ve, bag, cc);
                //return if the element is not of the correct type.
                if(ve is not RawFormattedTextBox element) return;
                element.Value = _valueDescription.GetValueFromBag(bag, cc);
                element.Label = _labelDescription.GetValueFromBag(bag, cc);
                element.EditMode = _editModeDescription.GetValueFromBag(bag, cc);
                //element.LocalizedLabel = _localizationDescription.GetValueFromBag(bag, cc);
            }

        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This property is used to get and set the value.
        /// </summary>
        public string Value {
            get => _rawField.value;
            set {
                if(_rawField.value == value) return;
                _rawField.value = value;
                _formattedField.text = value;
                OnValueChanged?.Invoke(value);
            }
        }

        /// <summary>
        /// This property is used to get or set the edit mode.
        /// </summary>
        public bool EditMode {
            get => _switchButton.value;
            set {
                _scrollView.style.display = value ? DisplayStyle.None : DisplayStyle.Flex;
                _rawField.style.display = value ? DisplayStyle.Flex : DisplayStyle.None;
                if(_switchButton.value == value) return;
                _switchButton.value = value;
            }
        }

        /// <summary>
        /// This property is used to get or set the label text.
        /// </summary>
        public string Label {
            get => /*_localized? _localizedKey:*/ _label.text;
            set {
                if(_label.text == value) return;
                _label.text = value;
            }
        }

        /// <summary>
        /// This property is used to get or set if the label is localized.
        /// </summary>
        /*public bool LocalizedLabel {
            get => _localized;
            set {
                if(_localized == value) return;
                _localized = value;
                if(_localized) {
                    AmiliousLocalization.OnTranslationUpdated += TranslationUpdated;
                    AmiliousLocalization.OnLanguageChanged += LanguageChanged;
                    _localizedKey = Label;
                    Label = AmiliousLocalization.GetTranslation(_localizedKey);
                } else {
                    AmiliousLocalization.OnTranslationUpdated -= TranslationUpdated;
                    AmiliousLocalization.OnLanguageChanged -= LanguageChanged;
                }
            }
        }

        private void LanguageChanged(string previous, string current) {
            if(!_localized || current!=AmiliousLocalization.CurrentLanguage) return;
            Label = AmiliousLocalization.GetTranslation(_localizedKey);
        }

        private void TranslationUpdated(string language, string key) {
            if(!_localized || key != _localizedKey|| language!=AmiliousLocalization.CurrentLanguage) return;
            Label = AmiliousLocalization.GetTranslation(key);
        }*/

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Constructors ///////////////////////////////////////////////////////////////////////////////////////////

        public RawFormattedTextBox() {
            Initialize();
            EditMode = false;
            Value = string.Empty;
            Label = string.Empty;
        }

        public RawFormattedTextBox(bool editMode) {
            Initialize();
            EditMode = editMode;
            Value = string.Empty;
            Label = string.Empty;
        }

        public RawFormattedTextBox(string value, bool editMode = false) {
            Initialize();
            EditMode = editMode;
            Value = value;
            Label = string.Empty;
        }

        public RawFormattedTextBox(string value, string label, bool editMode = false) {
            Initialize();
            EditMode = editMode;
            Value = value;
            Label = label;
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Private Fields /////////////////////////////////////////////////////////////////////////////////////////
        
        private void Initialize() {
            var asset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(ASSET_PATH);
            asset.CloneTree(this);
            this.Q(LABEL, out _label);
            this.Q(RAW_FIELD, out _rawField);
            this.Q(SCROLL_VIEW, out _scrollView);
            this.Q(SWITCH_BUTTON, out _switchButton);
            this.Q(FORMATTED_FIELD, out _formattedField);
            _rawField.RegisterValueChangedCallback(OnRawFieldEdited);
            _switchButton.RegisterValueChangedCallback(OnEditModeToggled);
        }

        private void OnRawFieldEdited(ChangeEvent<string> evt) => Value = evt.newValue;

        private void OnEditModeToggled(ChangeEvent<bool> evt) { EditMode = evt.newValue; }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

    }
}