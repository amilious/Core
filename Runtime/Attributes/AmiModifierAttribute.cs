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
using UnityEngine;

#if UNITY_EDITOR
using System.Reflection;
using TMPro;
using UnityEditor;
#endif

namespace Amilious.Core.Attributes {

    /// <summary>
    /// All property modifiers should be extended from this class.
    /// </summary>
    public abstract class AmiModifierAttribute : PropertyAttribute {

        
        /// <summary>
        /// This method is used to check if the property should be hidden.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns>True if the property should be hidden, otherwise false.</returns>
        public abstract bool ShouldHide<T>(T property);
        
        /// <summary>
        /// This method is used to check if the property should be disabled.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns>True if the property should be disabled, otherwise false.</returns>
        public abstract bool ShouldDisable<T>(T property);

        #if UNITY_EDITOR
        
        private SerializedProperty _comparisonProperty;
        
        #endif
        
        /// <summary>
        /// This method is used to check if the property should be hidden.
        /// </summary>
        /// <param name="property">The serialized property that you want to check.</param>
        /// <param name="propertyName">The property name.</param>
        /// <param name="setValue">True if the value was set.</param>
        /// <param name="value">The set value.</param>
        /// <returns>True if the property should be hidden, otherwise false.</returns>
        protected bool CompareProperty<T>(T property,string propertyName, bool setValue, object value) {
            #if !UNITY_EDITOR
            return true;
            #else
            SerializedObject serializedObject;
            if(property is SerializedObject so) {
                serializedObject = so;
                _comparisonProperty ??= serializedObject.FindProperty(propertyName);
            } else if(property is SerializedProperty serializedProperty) {
                serializedObject = serializedProperty.serializedObject;
                if(_comparisonProperty == null) {
                    _comparisonProperty = 
                        FindSiblingProperty(serializedProperty,propertyName) ??
                        serializedProperty.serializedObject.FindProperty(propertyName) ??
                        serializedProperty.FindPropertyRelative(propertyName);
                }
            } else return false;

            if(_comparisonProperty != null) {
                switch(_comparisonProperty.propertyType) {
                    case SerializedPropertyType.Generic: return false;
                    case SerializedPropertyType.Integer: return Validate(_comparisonProperty.intValue,setValue,value);
                    case SerializedPropertyType.Boolean: return Validate(_comparisonProperty.boolValue,setValue,value);
                    case SerializedPropertyType.Float: return Validate(_comparisonProperty.floatValue,setValue,value);
                    case SerializedPropertyType.String: return Validate(_comparisonProperty.stringValue,setValue,value);
                    case SerializedPropertyType.Color: return Validate(_comparisonProperty.colorValue,setValue,value);
                    case SerializedPropertyType.ObjectReference:
                    case SerializedPropertyType.Enum:
                        #if UNITY_2021_1_OR_NEWER
                        return ValidateEnumValue(_comparisonProperty.enumValueIndex,_comparisonProperty.enumValueFlag,value);
                        #else
                        return ValidateEnumValue(_comparisonProperty.enumValueIndex, -1,value);
                        #endif
                    case SerializedPropertyType.Vector2:
                        return Validate(_comparisonProperty.vector2Value,setValue,value);
                    case SerializedPropertyType.Vector3:
                        return Validate(_comparisonProperty.vector3Value,setValue,value);
                    case SerializedPropertyType.Vector4:
                        return Validate(_comparisonProperty.vector4Value,setValue,value);
                    case SerializedPropertyType.Rect:
                        return Validate(_comparisonProperty.rectValue,setValue,value);
                    case SerializedPropertyType.ArraySize:
                        return Validate(_comparisonProperty.arraySize,setValue,value);
                    case SerializedPropertyType.Character:
                        return Validate(_comparisonProperty.stringValue[0],setValue,value);
                    case SerializedPropertyType.AnimationCurve:
                        return Validate(_comparisonProperty.animationCurveValue,setValue,value);
                    case SerializedPropertyType.Bounds:
                        return Validate(_comparisonProperty.boundsValue,setValue,value);
                    case SerializedPropertyType.Quaternion:
                        return Validate(_comparisonProperty.quaternionValue,setValue,value);
                    case SerializedPropertyType.ExposedReference:
                        return Validate(_comparisonProperty.exposedReferenceValue,setValue,value);
                    case SerializedPropertyType.FixedBufferSize:
                        return Validate(_comparisonProperty.fixedBufferSize,setValue,value);
                    case SerializedPropertyType.Vector2Int:
                        return Validate(_comparisonProperty.vector2IntValue,setValue,value);
                    case SerializedPropertyType.Vector3Int:
                        return Validate(_comparisonProperty.vector3IntValue,setValue,value);
                    case SerializedPropertyType.RectInt:
                        return Validate(_comparisonProperty.rectIntValue,setValue,value);
                    case SerializedPropertyType.BoundsInt:
                        return Validate(_comparisonProperty.boundsIntValue,setValue,value);
                    #if UNITY_2021_2_OR_NEWER
                    case SerializedPropertyType.ManagedReference:
                        return Validate(_comparisonProperty.managedReferenceValue,setValue,value);
                    #endif
                    #if UNITY_2021_1_OR_NEWER
                    case SerializedPropertyType.Hash128:
                        return Validate(_comparisonProperty.hash128Value,setValue,value);
                    #endif
                    default:
                        return false;
                }
            }

            var binding = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;
            if(_hasTriedToGet && !_hasFound) return false;
            
            if(!_hasTriedToGet) {
                _field = serializedObject.targetObject.GetType().GetField(propertyName,binding);
                _property = serializedObject.targetObject.GetType().GetProperty(propertyName,binding);
                _method = serializedObject.targetObject.GetType().GetMethod(propertyName);
                if(_method == null || _method.GetParameters().Length >= 1 || _method.ReturnParameter == null)
                    _method = null;
                _hasTriedToGet = true;
                if(_field != null || _property != null || _method != null) _hasFound = true;
                else return false;
            }
            if(_field != null) return Validate(_field.GetValue(serializedObject.targetObject),setValue,value);
            if(_property != null) { return Validate(_property.GetValue(serializedObject.targetObject),setValue,value); }
            var result = _method?.Invoke(_method.IsStatic?null:serializedObject.targetObject, null);
            return Validate(result,setValue,value);
            #endif
        }

        #if UNITY_EDITOR
        private bool _hasTriedToGet = false;
        private bool _hasFound = false;
        private FieldInfo _field;
        private PropertyInfo _property;
        private MethodInfo _method;
        #endif
        
        #if UNITY_EDITOR
        
        /// <summary>
        /// This method is used to validate a value.
        /// </summary>
        /// <param name="value">The value that you want to validate.</param>
        /// <param name="setValue">True if the value was set.</param>
        /// <param name="trueValue">The set value.</param>
        /// <typeparam name="T">The type of the value that you want to validate.</typeparam>
        /// <returns>True if the value is valid otherwise false.</returns>
        private static bool Validate<T>(T value, bool setValue, object trueValue) {
            if(setValue) return trueValue is T casted && casted.Equals(value);
            if(value is bool boolValue) return boolValue;
            if(default(T) == null) return value != null;
            return false;
        }

        /// <summary>
        /// This method is used to validate a value.
        /// </summary>
        /// <param name="value">The value that you want to validate.</param>
        /// <param name="setValue">True if the value was set.</param>
        /// <param name="trueValue">The set value.</param>
        /// <returns>True if the value is valid otherwise false.</returns>
        private static bool Validate(object value, bool setValue, object trueValue) {
            if(setValue) return value.Equals(trueValue);
            if(value is bool boolValue) return boolValue;
            return value != null;
        }

        /// <summary>
        /// This method is used to validate an enum value.
        /// </summary>
        /// <param name="index">The enum index.</param>
        /// <param name="flag">The flag value.</param>
        /// <param name="trueValue">The set value.</param>
        /// <returns>True if the enum value is valid, otherwise false.</returns>
        private static bool ValidateEnumValue(int index, int flag, object trueValue) {
            if(!trueValue.GetType().IsEnum) return false;
            var casted = (int)trueValue;
            return casted == index || casted == flag;
        }
        
        /// <summary>
        /// This method is used to get the base path.
        /// </summary>
        /// <param name="property">The property that you want to get the base path for.</param>
        /// <returns>string.Empty if there is not a base path, otherwise it returns the base path and the ending ".".</returns>
        private static string GetBasePath(SerializedProperty property) {
            if(property == null) return string.Empty;
            var parts = property.propertyPath.Split('.') ?? Array.Empty<string>();
            return parts.Length <= 1 ? string.Empty : string.Join(".", parts,0,parts.Length-1)+".";
        }

        /// <summary>
        /// This method is used to get a properties sibling property.
        /// </summary>
        /// <param name="property">The property that you want to get the sibling for.</param>
        /// <param name="propertyName">The name of the sibling.</param>
        /// <returns>The sibling property or null.</returns>
        private static SerializedProperty FindSiblingProperty(SerializedProperty property,string propertyName) {
            return property?.serializedObject?.FindProperty(GetBasePath(property) + propertyName);
        }
        
        #endif

    }
    
}