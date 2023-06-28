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

namespace Amilious.Core.Events {
    
    /// <summary>
    /// This is a cancelable event args
    /// </summary>
    public class AbstractCancelEventArgs : EventArgs {
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This property is true if the operation was canceled.
        /// </summary>
        public bool Canceled { get; private set; }
        
        /// <summary>
        /// This property contains the cancel reason if the operation was canceled.
        /// </summary>
        public string CancelReason { get; private set; }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This method is used to cancel the operation.
        /// </summary>
        /// <param name="reason">The reason that you are canceling.</param>
        public void Cancel(string reason) {
            if(Canceled) return;
            Canceled = true;
            CancelReason = reason;
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}