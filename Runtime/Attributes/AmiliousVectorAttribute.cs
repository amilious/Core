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

using UnityEngine;

namespace Amilious.Core.Attributes {

    /// <summary>
    /// This enum is used to control the way that a vector will be displayed in the inspector.
    /// </summary>
    public enum VLayout {
        
        /// <summary>
        /// This value means that all values will be displayed on a single line.
        /// </summary>
        SingleLine,
        
        /// <summary>
        /// This value means that the labels will be displayed on the first line and the values on the second line.
        /// </summary>
        DoubleLine,
        
        /// <summary>
        /// This value means that the the labels and values will be displayed on the same line under the main label.
        /// </summary>
        FullDoubleLine,
        
        /// <summary>
        /// This value means that the labels will be displayed on the second line and the values on the third line.
        /// </summary>
        TripleLine
        
    }

    /// <summary>
    /// This attribute is used to customize the look of a vector in the inspector.
    /// </summary>
    public class AmiliousVectorAttribute : PropertyAttribute {
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This property is true if all the vector values should be shown on the same line, otherwise false.
        /// </summary>
        public bool IsSingleLine => Layout == VLayout.SingleLine;
        
        /// <summary>
        /// This property is true if the property will be displayed on three lines, otherwise false.
        /// </summary>
        public bool IsTripleLine => Layout == VLayout.TripleLine;
        
        /// <summary>
        /// This property is true if the property will be displayed on two lines, otherwise false.
        /// </summary>
        public bool IsDoubleLine => Layout is VLayout.DoubleLine or VLayout.FullDoubleLine;
        
        /// <summary>
        /// This property is true if the property will take up the entire width of the inspector, otherwise false.
        /// </summary>
        public bool IsFull => Layout == VLayout.FullDoubleLine;
        
        /// <summary>
        /// This property is true if the property will be displayed on multiple lines, otherwise false.
        /// </summary>
        public bool IsMultiLine => IsDoubleLine || IsTripleLine;

        /// <summary>
        /// This property contains the label for the x float.
        /// </summary>
        public readonly string XLabel;

        /// <summary>
        /// This property contains the label for the y float.
        /// </summary>
        public readonly string YLabel;

        /// <summary>
        /// This property contains the label for the z float.
        /// </summary>
        public readonly string ZLabel;

        /// <summary>
        /// This property contains the label for the w float.
        /// </summary>
        public readonly string WLabel;

        /// <summary>
        /// This property contains the layout.
        /// </summary>
        public readonly VLayout Layout;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Constructors ///////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This attribute is used to customize the look of a vector in the inspector.
        /// </summary>
        /// <param name="layout">The layout of the vector in the inspector.</param>
        /// <param name="xLabel">The label of the x value of the vector.</param>
        /// <param name="yLabel">The label of the y value of the vector.</param>
        /// <param name="zLabel">The label of the z value of the vector if it exists.</param>
        /// <param name="wLabel">The label of the w value of the vector if it exists.</param>
        public AmiliousVectorAttribute(VLayout layout = Attributes.VLayout.SingleLine, string xLabel="x",
            string yLabel = "y", string zLabel = "z", string wLabel = "w") {
            Layout = layout;
            XLabel = xLabel;
            YLabel = yLabel;
            ZLabel = zLabel;
            WLabel = wLabel;
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
    
}