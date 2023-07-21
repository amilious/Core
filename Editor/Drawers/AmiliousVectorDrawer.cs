/*//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                                                    //
//    _____            .__ .__   .__                             _________  __              .___.__                   //
//   /  _  \    _____  |__||  |  |__|  ____   __ __  ______     /   _____/_/  |_  __ __   __| _/|__|  ____   ______   //
//  /  /_\  \  /     \ |  ||  |  |  | /  _ \ |  |  \/  ___/     \_____  \ \   __\|  |  \ / __ | |  | /  _ \ /  ___/   //
// /    |    \|  Y Y  \|  ||  |__|  |(  <_> )|  |  /\___ \      /        \ |  |  |  |  // /_/ | |  |(  <_> )\___ \    //
// \____|__  /|__|_|  /|__||____/|__| \____/ |____//____  >    /_______  / |__|  |____/ \____ | |__| \____//____  >   //
//         \/       \/                                  \/             \/                    \/                 \/    //
//                                                                                                                    //
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//  Website:        http://www.amilious.com         Unity Asset Store: https://assetstore.unity.com/publishers/62511  //
//  Discord Server: https://discord.gg/SNqyDWu            Copyright© Amilious since 2022                              //                    
//  This code is part of an asset on the unity asset store. If you did not get this from the asset store you are not  //
//  using it legally. Check the asset store or join the discord for the license that applies for this script.         //
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/

using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Amilious.Core.Attributes;
using Amilious.Core.Editor.Extensions;
using Amilious.Core.Extensions;

namespace Amilious.Core.Editor.Drawers {
    
    /// <summary>
    /// This class is used to draw vectors in the inspector
    /// </summary>
    [CustomPropertyDrawer(typeof(AmiVectorAttribute))]
    public class AmiliousVectorDrawer : AmiliousPropertyDrawer {
        
        #region Cache //////////////////////////////////////////////////////////////////////////////////////////////////

        private AmiVectorAttribute _attribute;
        private GUIStyle _labelStyle = new GUIStyle(GUI.skin.label) {
            fontSize = 10, fontStyle = FontStyle.Bold, padding = { right = 4 }
        };

        private readonly GUIContent _sizeContent = new GUIContent();
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Override Methods ///////////////////////////////////////////////////////////////////////////////////////

        /// <inheritdoc />
        protected override void AmiliousOnGUI(Rect position, SerializedProperty property, GUIContent label) {
            
            var att = GetAttribute(property);
            
            if(att.IsDoubleLine) position.y -= EditorGUIUtility.singleLineHeight/2f;
            if(att.Layout == VLayout.TripleLine) position.y -= EditorGUIUtility.singleLineHeight;
            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.LabelField(position, label);

            switch(property.propertyType) {
                case SerializedPropertyType.Vector2: 
                    property.vector2Value = DrawVector2(position,property.vector2Value,att); break;
                case SerializedPropertyType.Vector2Int: 
                    property.vector2IntValue = DrawVector2Int(position,property.vector2IntValue,att); break;
                case SerializedPropertyType.Vector3: 
                    property.vector3Value = DrawVector3(position,property.vector3Value,att); break;
                case SerializedPropertyType.Vector3Int: 
                    property.vector3IntValue = DrawVector3Int(position,property.vector3IntValue,att); break;
                case SerializedPropertyType.Vector4: 
                    property.vector4Value = DrawVector4(position,property.vector4Value,att); break;
                default: return;
            }
            
            EditorGUI.EndProperty();
        }

        /// <inheritdoc />
        protected override float AmiliousGetPropertyHeight(SerializedProperty property, GUIContent label) {
            var att = GetAttribute(property);
            var height = EditorGUIUtility.singleLineHeight;
            if(att.IsDoubleLine) height+=EditorGUIUtility.singleLineHeight+5;
            if(att.Layout==VLayout.TripleLine) height+= (EditorGUIUtility.singleLineHeight+5)*2;
            return height;
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Private Methods ////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This method is used to get and cache the attribute.
        /// </summary>
        /// <param name="property">The serialized property.</param>
        /// <returns>The attribute.</returns>
        private AmiVectorAttribute GetAttribute(SerializedProperty property) {
            if(_attribute != null) return _attribute;
            _attribute = property.GetAttributes<AmiVectorAttribute>().FirstOrDefault()??
                new AmiVectorAttribute();
            return _attribute;
        }
        
        /// <summary>
        /// This method is used to draw a vector.
        /// </summary>
        /// <param name="position">The starting position.</param>
        /// <param name="value">The current value.</param>
        /// <param name="att">The attribute.</param>
        /// <returns>The result value.</returns>
        private Vector2 DrawVector2(Rect position, Vector2 value, AmiVectorAttribute att) {
            var move = CalculateMove(2, att, ref position, out var position2);
            for(var axis = 0; axis < 2; axis++) {
                value = value.SetAxis(axis, DrawField(axis, value.GetAxis(axis), position, 
                    position2, att, 25)); position.x += move; position2.x += move;
            }
            return value; 
        }
        
        /// <summary>
        /// This method is used to draw a vector.
        /// </summary>
        /// <param name="position">The starting position.</param>
        /// <param name="value">The current value.</param>
        /// <param name="att">The attribute.</param>
        /// <returns>The result value.</returns>
        private Vector2Int DrawVector2Int(Rect position, Vector2Int value, AmiVectorAttribute att) {
            var move = CalculateMove(2, att, ref position, out var position2);
            for(var axis = 0; axis < 2; axis++) {
                value = value.SetAxis(axis, DrawField(axis, value.GetAxis(axis), position, 
                    position2, att, 25)); position.x += move; position2.x += move;
            }
            return value;
        }
        
        /// <summary>
        /// This method is used to draw a vector.
        /// </summary>
        /// <param name="position">The starting position.</param>
        /// <param name="value">The current value.</param>
        /// <param name="att">The attribute.</param>
        /// <returns>The result value.</returns>
        private Vector3 DrawVector3(Rect position, Vector3 value, AmiVectorAttribute att) {
            var move = CalculateMove(3, att, ref position, out var position2);
            for(var axis = 0; axis < 3; axis++) {
                value = value.SetAxis(axis, DrawField(axis, value.GetAxis(axis), position, 
                    position2, att, 25)); position.x += move; position2.x += move;
            }
            return value; 
        }
        
        /// <summary>
        /// This method is used to draw a vector.
        /// </summary>
        /// <param name="position">The starting position.</param>
        /// <param name="value">The current value.</param>
        /// <param name="att">The attribute.</param>
        /// <returns>The result value.</returns>
        private Vector3Int DrawVector3Int(Rect position, Vector3Int value, AmiVectorAttribute att) {
            var move = CalculateMove(3, att, ref position, out var position2);
            for(var axis = 0; axis < 3; axis++) {
                value = value.SetAxis(axis, DrawField(axis, value.GetAxis(axis), position, 
                    position2, att, 25)); position.x += move; position2.x += move;
            }
            return value; 
        }
        
        /// <summary>
        /// This method is used to draw a vector.
        /// </summary>
        /// <param name="position">The starting position.</param>
        /// <param name="value">The current value.</param>
        /// <param name="att">The attribute.</param>
        /// <returns>The result value.</returns>
        private Vector4 DrawVector4(Rect position, Vector4 value, AmiVectorAttribute att) {
            var move = CalculateMove(4, att, ref position, out var position2);
            for(var axis = 0; axis < 4; axis++) {
                value = value.SetAxis(axis, DrawField(axis, value.GetAxis(axis), position, 
                    position2, att, 25)); position.x += move; position2.x += move;
            }
            return value; 
        }

        /// <summary>
        /// This method is used to calculate the values needed to draw a vector in the inspector.
        /// </summary>
        /// <param name="vectors">The number of vectors.</param>
        /// <param name="att">The attribute.</param>
        /// <param name="position">The first position.</param>
        /// <param name="position2">The second position.</param>
        /// <returns>The rate at which the drawer should move.</returns>
        private float CalculateMove(float vectors, AmiVectorAttribute att, ref Rect position, out Rect position2) {
            //move the cursor over away from the label.
            if(att.Layout != VLayout.FullDoubleLine && att.Layout != VLayout.TripleLine) {
                position.x += EditorGUIUtility.labelWidth + 2;
                position.width -= EditorGUIUtility.labelWidth + 2;
            }
            if(att.Layout == VLayout.FullDoubleLine){
                position.x += 7;
                position.width -= 7;
            }
            if(att.Layout == VLayout.TripleLine){
                position.x += 12;
                position.width -= 12;
            }
            var width = att.Layout== VLayout.DoubleLine||att.Layout== VLayout.TripleLine? 
                (position.width/vectors) : (position.width /vectors) / 2f;
            var move = att.Layout== VLayout.DoubleLine||att.Layout== VLayout.TripleLine? width : width * 2f;
            position.width = width-2;
            position2 = new Rect(position.x,position.y,position.width,position.height);
            if(att.IsDoubleLine) {
                position2.y += EditorGUIUtility.singleLineHeight *1.8f;
                position2.height = EditorGUIUtility.singleLineHeight;
            }
            if(att.Layout==VLayout.TripleLine) {
                position2.y += (EditorGUIUtility.singleLineHeight)*1.8f*2;
                position2.height = EditorGUIUtility.singleLineHeight;
            }
            if(att.Layout!=VLayout.DoubleLine&&att.Layout!=VLayout.TripleLine) position2.x += width-2;
            if(att.IsFull||att.Layout == VLayout.TripleLine) position.y += EditorGUIUtility.singleLineHeight+2;
            return move;
        }

        private T DrawField<T>(int axis, T value, Rect label, Rect field, AmiVectorAttribute att, float minField) 
            where T : struct, IComparable, IConvertible, IFormattable {
            var text = att.GetLabel(axis);
            if(att.IsSingleLine || att.Layout == VLayout.FullDoubleLine) {
                _sizeContent.text = text;
                var width = _labelStyle.CalcSize(_sizeContent).x;
                width = Mathf.Min(width,  label.width + field.width - minField);
                var offset = label.width - width;
                var fixedLabel = new Rect(label.x, label.y, width, label.height);
                var fixedField = new Rect(field.x - offset, field.y, field.width + offset, field.height);
                EditorGUI.LabelField(fixedLabel,text, _labelStyle);
                if (typeof(T) == typeof(float) && value is float floatValue) {
                    return (T)(object)EditorGUI.FloatField(fixedField, floatValue);
                }
                if(typeof(T) == typeof(int) && value is int intValue) {
                    return (T)(object)EditorGUI.IntField(fixedField, intValue);
                }
                return value;
            }
            EditorGUI.LabelField(label,text);
            if (typeof(T) == typeof(float) && value is float floatValue2) {
                EditorGUI.FloatField(field, floatValue2);
            }
            if (typeof(T) == typeof(int) && value is int intValue2) {
                return (T)(object)EditorGUI.IntField(field, intValue2);
            }
            return value;
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
    
}