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

using System.Linq;
using UnityEditor;
using UnityEngine;
using Amilious.Core.Attributes;
using Amilious.Core.Editor.Extensions;

namespace Amilious.Core.Editor.Drawers {
    
    /// <summary>
    /// This class is used to draw vectors in the inspector
    /// </summary>
    [CustomPropertyDrawer(typeof(AmiliousVectorAttribute))]
    public class AmiliousVectorDrawer : AmiliousPropertyDrawer {
        
        #region Cache //////////////////////////////////////////////////////////////////////////////////////////////////

        private AmiliousVectorAttribute _attribute;
        
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
        private AmiliousVectorAttribute GetAttribute(SerializedProperty property) {
            if(_attribute != null) return _attribute;
            _attribute = property.GetAttributes<AmiliousVectorAttribute>().FirstOrDefault()??
                new AmiliousVectorAttribute();
            return _attribute;
        }
        
        /// <summary>
        /// This method is used to draw a vector.
        /// </summary>
        /// <param name="position">The starting position.</param>
        /// <param name="value">The current value.</param>
        /// <param name="att">The attribute.</param>
        /// <returns>The result value.</returns>
        private Vector2 DrawVector2(Rect position, Vector2 value, AmiliousVectorAttribute att) {
            var move = CalculateMove(2, att, ref position, out var position2);
            EditorGUI.LabelField(position,att.XLabel);
            value.x = EditorGUI.FloatField(position2, value.x);
            position.x += move; position2.x += move;
            EditorGUI.LabelField(position,att.YLabel);
            value.y = EditorGUI.FloatField(position2, value.y);
            return value; 
        }
        
        /// <summary>
        /// This method is used to draw a vector.
        /// </summary>
        /// <param name="position">The starting position.</param>
        /// <param name="value">The current value.</param>
        /// <param name="att">The attribute.</param>
        /// <returns>The result value.</returns>
        private Vector2Int DrawVector2Int(Rect position, Vector2Int value, AmiliousVectorAttribute att) {
            var move = CalculateMove(2, att, ref position, out var position2);
            EditorGUI.LabelField(position,att.XLabel);
            value.x = EditorGUI.IntField(position2, value.x);
            position.x += move; position2.x += move;
            EditorGUI.LabelField(position,att.YLabel);
            value.y = EditorGUI.IntField(position2, value.y);
            position.x += move; position2.x += move;
            return value; 
        }
        
        /// <summary>
        /// This method is used to draw a vector.
        /// </summary>
        /// <param name="position">The starting position.</param>
        /// <param name="value">The current value.</param>
        /// <param name="att">The attribute.</param>
        /// <returns>The result value.</returns>
        private Vector3 DrawVector3(Rect position, Vector3 value, AmiliousVectorAttribute att) {
            var move = CalculateMove(3, att, ref position, out var position2);
            EditorGUI.LabelField(position,att.XLabel);
            value.x = EditorGUI.FloatField(position2, value.x);
            position.x += move; position2.x += move;
            EditorGUI.LabelField(position,att.YLabel);
            value.y = EditorGUI.FloatField(position2, value.y);
            position.x += move; position2.x += move;
            EditorGUI.LabelField(position,att.ZLabel);
            value.z = EditorGUI.FloatField(position2, value.z);
            return value; 
        }
        
        /// <summary>
        /// This method is used to draw a vector.
        /// </summary>
        /// <param name="position">The starting position.</param>
        /// <param name="value">The current value.</param>
        /// <param name="att">The attribute.</param>
        /// <returns>The result value.</returns>
        private Vector3Int DrawVector3Int(Rect position, Vector3Int value, AmiliousVectorAttribute att) {
            var move = CalculateMove(3, att, ref position, out var position2);
            EditorGUI.LabelField(position,att.XLabel);
            value.x = EditorGUI.IntField(position2, value.x);
            position.x += move; position2.x += move;
            EditorGUI.LabelField(position,att.YLabel);
            value.y = EditorGUI.IntField(position2, value.y);
            position.x += move; position2.x += move;
            EditorGUI.LabelField(position,att.ZLabel);
            value.z = EditorGUI.IntField(position2, value.z);
            return value; 
        }
        
        /// <summary>
        /// This method is used to draw a vector.
        /// </summary>
        /// <param name="position">The starting position.</param>
        /// <param name="value">The current value.</param>
        /// <param name="att">The attribute.</param>
        /// <returns>The result value.</returns>
        private Vector4 DrawVector4(Rect position, Vector4 value, AmiliousVectorAttribute att) {
            var move = CalculateMove(4, att, ref position, out var position2);
            EditorGUI.LabelField(position,att.XLabel);
            value.x = EditorGUI.FloatField(position2, value.x);
            position.x += move; position2.x += move;
            EditorGUI.LabelField(position,att.YLabel);
            value.y = EditorGUI.FloatField(position2, value.y);
            position.x += move; position2.x += move;
            EditorGUI.LabelField(position,att.ZLabel);
            value.z = EditorGUI.FloatField(position2, value.z);
            position.x += move; position2.x += move;
            EditorGUI.LabelField(position,att.WLabel);
            value.w = EditorGUI.FloatField(position2, value.w);
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
        private float CalculateMove(float vectors, AmiliousVectorAttribute att, ref Rect position, out Rect position2) {
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
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
    
}