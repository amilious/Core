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
using UnityEditor;

namespace Amilious.Core.Editor.Extensions {
    
    /// <summary>
    /// This class is used to add methods to the <see cref="SerializedObject"/> class.
    /// </summary>
    public static class SerializedObjectExtensions {
        
        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to get the full name of the script that the serialized object represents.
        /// </summary>
        /// <param name="obj">The serialized object.</param>
        /// <returns>The full name of the script that the serialized object represents.</returns>
        /// <seealso cref="GetScriptName"/>
        public static string GetScriptFullName(this SerializedObject obj) => obj?.targetObject.GetType().FullName;

        /// <summary>
        /// This method is used to get the name of the script that the serialized object represents.
        /// </summary>
        /// <param name="obj">The serialized object.</param>
        /// <returns>The name of the script that the serialized object represents.</returns>
        /// <seealso cref="GetScriptFullName"/>
        public static string GetScriptName(this SerializedObject obj) => obj?.targetObject.GetType().Name;

        /// <summary>
        /// This method is used to get the type of the script that the serialized object represents.
        /// </summary>
        /// <param name="obj">The serialized object.</param>
        /// <returns>The type of the script that the serialized object represents.</returns>
        public static Type GetScriptType(this SerializedObject obj) => obj?.targetObject.GetType();

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}