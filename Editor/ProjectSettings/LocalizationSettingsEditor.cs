﻿using UnityEditor;
using UnityEngine;

namespace Amilious.Core.Editor.ProjectSettings {


    public class LocalizationSettingsEditor : UnityEditor.Editor {
        
        [SettingsProvider]
        public static SettingsProvider CreateCustomSettingsProvider() {
            var provider = new SettingsProvider("Project/Amilious/Localization", SettingsScope.Project) {
                label = "Localization",
                guiHandler = (searchContext) => {
                    EditorGUILayout.LabelField("This is a custom project settings tab.");
                }
            };

            return provider;
        }
        
    }

}