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

using UnityEditor;

namespace Amilious.Core.Attributes {
    
    /// <summary>
    /// This attribute is used to hide a property if a condition is met.
    /// </summary>
    public class HideIfAttribute : AmiliousModifierAttribute {
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// The property to use as the condition.
        /// </summary>
        public string PropertyName { get; protected set; }
        
        /// <summary>
        /// If this value is true the comparison value was provided, otherwise it was not.
        /// </summary>
        private bool SetValue { get; set; }
        
        /// <summary>
        /// This property contains the value that you want to compare to.
        /// </summary>
        private object Value { get; set; }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Constructors ///////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This attribute is used to hide a property if a condition is met.
        /// </summary>
        /// <param name="propertyName">The name of the field that you want use as a condition.</param>
        public HideIfAttribute(string propertyName) {
            PropertyName = propertyName;
        }
        
        /// <summary>
        /// This attribute is used to hide a property if a condition is met.
        /// </summary>
        /// <param name="propertyName">The name of the field that you want use as a condition.</param>
        /// <param name="value">The value that you want to use for comparison.</param>
        public HideIfAttribute(string propertyName, object value){
            PropertyName = propertyName;
            Value = value;
            SetValue = true;
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Methods ////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        public override bool ShouldHide<T>(T property) {
            return CompareProperty(property, PropertyName, SetValue, Value);
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
    
}