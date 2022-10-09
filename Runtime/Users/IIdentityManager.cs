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

namespace Amilious.Core.Users {
    
    public interface IIdentityManager {

        /// <summary>
        /// This method is used to try get the <see cref="UserIdentity"/> for the given id.
        /// </summary>
        /// <param name="id">The id of the <see cref="UserIdentity"/> that you want to get.</param>
        /// <param name="identity">The identity for the given id.</param>
        /// <returns>True if an <see cref="UserIdentity"/> was found with the given id, otherwise false.</returns>
        /// <remarks>This can be called from the client or server!</remarks>
        bool TryGetIdentity(int id, out UserIdentity identity);

        /// <summary>
        /// This method is used to try get the <see cref="UserIdentity"/> for the given user name.
        /// </summary>
        /// <param name="userName">The user name that you want to get the user identity for.</param>
        /// <param name="identity">The identity for the given user name.</param>
        /// <returns>True if an <see cref="UserIdentity"/> was found with the given user name, otherwise false.</returns>
        /// <remarks>This can be called from the client or server!</remarks>
        bool TryGetIdentity(string userName, out UserIdentity identity);

        /// <summary>
        /// This method is used to get the current user's identity.
        /// </summary>
        /// <returns>The identity associated with the current user.</returns>
        /// <remarks>This method should only be called from the client!</remarks>
        UserIdentity GetIdentity();

        /// <summary>
        /// This method is used to check if a user is able to send a message to another user.
        /// </summary>
        /// <param name="sender">The sender of the message.</param>
        /// <param name="recipient">The receiver of the message.</param>
        /// <returns>True if the sender is able to send a message to the recipient, otherwise false.</returns>
        /// <remarks>This method should only be called from the server!</remarks>
        bool CanSendMessageTo(int sender, int recipient);
        
    }
    
}