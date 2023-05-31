using Amilious.Core.Attributes;
using Amilious.Core.Localization;
using UnityEditor;
using UnityEngine;

namespace Amilious.Core.Editor.Modifiers {
    
    [CustomPropertyDrawer(typeof(AmiLocalizedLabelAttribute))]
    public class LocalizedLabelModifier : AmiliousPropertyModifier<AmiLocalizedLabelAttribute> {
        
        public override void BeforeOnGUI(SerializedProperty property, GUIContent label, bool hidden, bool disabled) {
            if(!AmiliousLocalization.HasKey(Attribute.LabelKey)) {
                label.image = EditorGUIUtility.IconContent("CollabError").image;
                label.tooltip += $"\nUnable to find a value for the localization key \"{Attribute.LabelKey}\"";
            }else if(!AmiliousLocalization.HasTranslation(Attribute.LabelKey,AmiliousLocalization.CurrentLanguage)) {
                label.image = EditorGUIUtility.FindTexture("console.warnicon.sml");
                label.tooltip += $"\nThe localization key \"{Attribute.LabelKey}\" has no translation for {AmiliousLocalization.CurrentLanguage}!";
            }
            label.text = AmiliousLocalization.GetTranslation(Attribute.LabelKey);
            if(Attribute.ToolTipKey!=null) label.tooltip = AmiliousLocalization.GetTranslation(Attribute.ToolTipKey);
        }
        
    }
}