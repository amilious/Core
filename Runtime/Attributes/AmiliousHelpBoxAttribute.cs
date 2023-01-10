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

namespace Amilious.Core.Attributes {
    
    /// <summary>
    /// This attribute is used to show a help box in the inspector.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field|AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class AmiliousHelpBoxAttribute : AmiliousModifierAttribute {
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This is the help box message!
        /// </summary>
        public string Message { get; }
        
        /// <summary>
        /// This is the helpbox message type!
        /// </summary>
        public HelpBoxType HelpBoxType { get; }
        
        /// <inheritdoc />
        public override bool ShouldHide<T>(T property) => false;

        /// <inheritdoc />
        public override bool ShouldDisable<T>(T property) => false;

        /// <summary>
        /// The conditional property name.
        /// </summary>
        public string ConditionalPropertyName { get; }
        
        /// <summary>
        /// The conditional value.
        /// </summary>
        public object ConditionalValue { get; }
        
        /// <summary>
        /// If true the condition value was given.
        /// </summary>
        public bool ConditionValueSet { get; }
        
        /// <summary>
        /// If true the help box is using a condition.
        /// </summary>
        public bool UsingCondition { get; }
        
        /// <summary>
        /// If true the help box will be hidden if the condition is true, otherwise it will be shown when the condition
        /// is true.
        /// </summary>
        public bool HideIfCondition { get; }
        
        #if UNITY_EDITOR

        /// <summary>
        /// This method is used to get the message type for the help box.
        /// </summary>
        public UnityEditor.MessageType MessageType => HelpBoxType.ToMessageType();
        
        #endif
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Constructors ///////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This attribute is used to show a help box in the inspector.
        /// </summary>
        /// <param name="message">The message that you want to display in the help box.</param>
        /// <param name="helpBoxType">The type of message for the help box.</param>
        public AmiliousHelpBoxAttribute(string message, HelpBoxType helpBoxType = HelpBoxType.None) {
            Message = message;
            HelpBoxType = helpBoxType;
            ConditionalPropertyName = String.Empty;
            ConditionalValue = null;
            ConditionValueSet = false;
            UsingCondition = false;
            HideIfCondition = false;
        }
        
        /// <summary>
        /// This attribute is used to show a help box in the inspector.
        /// </summary>
        /// <param name="message">The message that you want to display in the help box.</param>
        /// <param name="conditionalPropertyName">The name of the property that you want to use as a condition
        /// for the help box.</param>
        /// <param name="hideIfCondition">If true the help box will be hidden when the condition is true, otherwise
        /// the helpbox will be shown when the condition is true.</param>
        /// <param name="helpBoxType">The type of message for the help box.</param>
        public AmiliousHelpBoxAttribute(string message, string conditionalPropertyName, 
            bool hideIfCondition = false, HelpBoxType helpBoxType = HelpBoxType.None) {
            Message = message;
            HelpBoxType = helpBoxType;
            ConditionalPropertyName = conditionalPropertyName;
            ConditionalValue = null;
            ConditionValueSet = false;
            UsingCondition = true;
            HideIfCondition = hideIfCondition;
        }
        
        /// <summary>
        /// This attribute is used to show a help box in the inspector.
        /// </summary>
        /// <param name="message">The message that you want to display in the help box.</param>
        /// <param name="conditionalPropertyName">The name of the property that you want to use as a condition
        /// for the help box.</param>
        /// <param name="conditionValue">This is the value that will make the condition true.</param>
        /// <param name="hideIfCondition">If true the help box will be hidden when the condition is true, otherwise
        /// the helpbox will be shown when the condition is true.</param>
        /// <param name="helpBoxType">The type of message for the help box.</param>
        public AmiliousHelpBoxAttribute(string message, string conditionalPropertyName, object conditionValue, 
            bool hideIfCondition = false, HelpBoxType helpBoxType = HelpBoxType.None){
            Message = message;
            HelpBoxType = helpBoxType;
            ConditionalPropertyName = conditionalPropertyName;
            ConditionalValue = conditionValue;
            ConditionValueSet = true;
            UsingCondition = true;
            HideIfCondition = hideIfCondition;
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to check if the help box should be drawn.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public bool ShouldShowHelp<T>(T serializedProperty) {
            if(!UsingCondition) return true;
            var conditionMet = 
                CompareProperty(serializedProperty, ConditionalPropertyName, ConditionValueSet, ConditionalValue);
            if(HideIfCondition) conditionMet = !conditionMet;
            return conditionMet;
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
    
}