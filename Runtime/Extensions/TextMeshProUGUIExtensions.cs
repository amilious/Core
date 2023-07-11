using TMPro;
using System;
using UnityEngine;

namespace Amilious.Core.Extensions {
    
    public static class TextMeshProUGUIExtensions {

        public static bool TryGetLinkInfo(this TextMeshProUGUI textMesh, int linkIndex, out TMP_LinkInfo linkInfo) {
            linkInfo = default;
            if(textMesh == null || linkIndex == -1 || linkIndex >= textMesh.textInfo.linkInfo.Length) return false;
            linkInfo = textMesh.textInfo.linkInfo[linkIndex];
            return true;
        }

        public static Color32[] GetLinkColors(this TextMeshProUGUI textMesh, int linkIndex) {
            if(!textMesh.TryGetLinkInfo(linkIndex,out var linkInfo))return Array.Empty<Color32>();
            var linkColors = new Color32[linkInfo.linkTextLength];
            for (var i = 0; i < linkInfo.linkTextLength; i++) {
                var charIndex = linkInfo.linkTextfirstCharacterIndex + i;
                var meshIndex = textMesh.textInfo.characterInfo[charIndex].materialReferenceIndex;
                var vertexIndex = textMesh.textInfo.characterInfo[charIndex].vertexIndex;
                var vertexColors = textMesh.textInfo.meshInfo[meshIndex].colors32;
                linkColors[i] = vertexColors[vertexIndex];
            }
            return linkColors;
        }

        public static bool SetLinkColor(this TextMeshProUGUI textMesh, int linkIndex, Color color) {
            if(!textMesh.TryGetLinkInfo(linkIndex,out var linkInfo))return false;
            // Iterate through each character of the link text
            for (var i = 0; i < linkInfo.linkTextLength; i++) {
                var charIndex = linkInfo.linkTextfirstCharacterIndex + i;
                // Skip whitespace characters
                if (textMesh.textInfo.characterInfo[charIndex].character == ' ') continue;
                var meshIndex = textMesh.textInfo.characterInfo[charIndex].materialReferenceIndex;
                var vertexIndex = textMesh.textInfo.characterInfo[charIndex].vertexIndex;
                var vertexColors = textMesh.textInfo.meshInfo[meshIndex].colors32;
                vertexColors[vertexIndex] = color;
                // Update the modified vertex colors
                textMesh.textInfo.meshInfo[meshIndex].colors32 = vertexColors;
            }
            // Update geometry
            textMesh.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
            return true;
        }

        public static bool SetLinkColors(this TextMeshProUGUI textMesh, int linkIndex, Color32[] colors) {
            if(!textMesh.TryGetLinkInfo(linkIndex,out var linkInfo))return false;
            for (var i = 0; i < linkInfo.linkTextLength; i++) {
                var charIndex = linkInfo.linkTextfirstCharacterIndex + i;
                if(textMesh.textInfo.characterInfo[charIndex].character == ' ') continue;
                var meshIndex = textMesh.textInfo.characterInfo[charIndex].materialReferenceIndex;
                var vertexIndex = textMesh.textInfo.characterInfo[charIndex].vertexIndex;
                var vertexColors = textMesh.textInfo.meshInfo[meshIndex].colors32;
                var colorIndex = Mathf.Min(i, colors.Length - 1);
                vertexColors[vertexIndex] = colors[colorIndex];
                // Update the modified vertex color
                textMesh.textInfo.meshInfo[meshIndex].colors32 = vertexColors;
            }
            // Update geometry
            textMesh.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
            return true;
        }
        
        public static bool ModifyLinkTint(this TextMeshProUGUI textMesh, int linkIndex, float tint) {
            if(!textMesh.TryGetLinkInfo(linkIndex, out var linkInfo)) return false;
            // Iterate through each of the characters of the word.
            for (var i = 0; i < linkInfo.linkTextLength; i++) {
                var charIndex = linkInfo.linkTextfirstCharacterIndex + i;
                if(textMesh.textInfo.characterInfo[charIndex].character == ' ') continue;
                var meshIndex = textMesh.textInfo.characterInfo[charIndex].materialReferenceIndex;
                var vertexIndex = textMesh.textInfo.characterInfo[charIndex].vertexIndex;
                var vertexColors = textMesh.textInfo.meshInfo[meshIndex].colors32;
                vertexColors.ModifyValues(vertexIndex,4,color=>color.Tint(tint));
            }
            // Update Geometry
            textMesh.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
            return true;
        }
        
    }
    
}