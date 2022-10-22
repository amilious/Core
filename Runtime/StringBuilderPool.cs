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
using System.Collections.Concurrent;

namespace Amilious.Core {
    
    /// <summary>
    /// This class is for reusing string builders.
    /// </summary>
    public static class StringBuilderPool {

        #region Private Fields /////////////////////////////////////////////////////////////////////////////////////////
        
        private const int MAX_POOL_SIZE = 20;
        private static readonly ConcurrentQueue<StringBuilder> Pool = new ConcurrentQueue<StringBuilder>();
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This property is used to rent a string builder from the pool.
        /// </summary>
        public static StringBuilder Rent => Pool.TryDequeue(out var builder) ? builder.Clear() : new StringBuilder();
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to return a string builder to the pool.
        /// </summary>
        /// <param name="stringBuilder"></param>
        public static void Return(StringBuilder stringBuilder) {
            if(Pool.Count >= MAX_POOL_SIZE) return;
            Pool.Enqueue(stringBuilder);
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

    }

}