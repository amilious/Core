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

using System.Text;

namespace Amilious.Core.Extensions {
    
    /// <summary>
    /// This class is used to add methods to the <see cref="StringBuilder"/> class.
    /// </summary>
    public static class StringBuilderExtensions {
        
        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to return a string builder to the string builder pool.
        /// </summary>
        /// <param name="builder">The string builder.</param>
        public static void ReturnToPool(this StringBuilder builder){
            StringBuilderPool.Return(builder);
        }

        /// <summary>
        /// This method is used to return a string builder to the string builder pool and also
        /// return its current text.
        /// </summary>
        /// <param name="builder">The string builder.</param>
        /// <returns>The string builder's text.</returns>
        public static string ToStringAndReturnToPool(this StringBuilder builder) {
            var result = builder.ToString();
            builder.ReturnToPool();
            return result;
        }

        /// <summary>
        /// This method is used to add a character if the string builder is not empty.
        /// </summary>
        /// <param name="builder">The string builder.</param>
        /// <param name="c">The character that you want to add.</param>
        /// <returns>The string builder.</returns>
        public static StringBuilder AddIfNotEmpty(this StringBuilder builder,char c) {
            if(builder.Length != 0) builder.Append(c);
            return builder;
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
    
}