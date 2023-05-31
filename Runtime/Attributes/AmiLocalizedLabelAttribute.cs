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

namespace Amilious.Core.Attributes {
    
    /// <summary>
    /// This attribute is used to localize a label and tooltip in the inspector.
    /// </summary>
    public class AmiLocalizedLabelAttribute : AmiModifierAttribute {
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This property contains the key of the label.
        /// </summary>
        public string LabelKey { get; }
        
        /// <summary>
        /// This property contains the key of the tooltip.
        /// </summary>
        public string ToolTipKey { get; }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Constructors ///////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This attribute is used to localize a label and tooltip in the inspector.
        /// </summary>
        public AmiLocalizedLabelAttribute(string labelKey, string toolTipKey = null) {
            LabelKey = labelKey;
            ToolTipKey = toolTipKey;
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Override Methods ///////////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        public override bool ShouldHide<T>(T property) => false;

        /// <inheritdoc />
        public override bool ShouldDisable<T>(T property) => false;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}