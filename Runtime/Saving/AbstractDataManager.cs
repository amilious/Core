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

using System.Collections.Generic;

namespace Amilious.Core.Saving {
    
    /// <summary>
    /// This class is used to access, store, and edit information.
    /// </summary>
    public abstract class AbstractDataManager : AmiliousBehavior {

        #region Server & Users /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to get the server's identifier.
        /// </summary>
        /// <returns>The servers identifier.</returns>
        public abstract string Server_GetServerIdentifier();
        
        /// <summary>
        /// This method is used to check if the given user id is valid.
        /// </summary>
        /// <param name="userId">The user id that you want to check.</param>
        /// <returns>True if the given user id is valid, otherwise false.</returns>
        public abstract bool Server_IsUserIdValid(int userId);

        /// <summary>
        /// This method is used to try get a user id from the given user name.
        /// </summary>
        /// <param name="userName">The user name.</param>
        /// <param name="userId">The user id associated with the user id.</param>
        /// <param name="caseSensitive">True if the user name is case sensitive, otherwise false.</param>
        /// <returns>True if able to find a user with the given user name, otherwise false.</returns>
        public abstract bool Server_TryGetIdFromUserName(string userName, out int userId, bool caseSensitive = true);

        /// <summary>
        /// This method is used to try read data for the given user.
        /// </summary>
        /// <param name="userId">The id of the user.</param>
        /// <param name="key">The key for the data that you want to read.</param>
        /// <param name="value">The read value.</param>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <returns>True if the data exists, otherwise false.</returns>
        public abstract bool Server_TryReadUserData<T>(int userId, string key, out T value);

        /// <summary>
        /// This method is used to store data for the given user.
        /// </summary>
        /// <param name="userId">The id of the user.</param>
        /// <param name="key">The key for the data that you want to store.</param>
        /// <param name="value">The value that you want to store for the given key.</param>
        /// <typeparam name="T">The type of the value.</typeparam>
        public abstract void Server_StoreUserData<T>(int userId, string key, T value);

        /// <summary>
        /// This method is used to add a new user to the server.
        /// </summary>
        /// <param name="userName">The user name for the user.</param>
        /// <returns>The id for the new user.</returns>
        public abstract int Server_AddUser(string userName=null);

        /// <summary>
        /// This method is used to get the id's for all of the registered users.
        /// </summary>
        /// <returns>The id's of the registered users.</returns>
        public abstract IEnumerable<int> Server_GetStoredUserIds();

        /// <summary>
        /// This method is used to block a user.
        /// </summary>
        /// <param name="blocker">The user id of the user who is blocking another user.</param>
        /// <param name="blocked">The user id of the user that will be blocked.</param>
        public abstract void Server_BlockUser(int blocker, int blocked);

        /// <summary>
        /// This method is used to check if a user has blocked another.
        /// </summary>
        /// <param name="blocker">The user id of the user who blocked another user.</param>
        /// <param name="blocked">The user id of the user who was blocked.</param>
        /// <returns>True if the <paramref name="blocker"/> has blocked the <paramref name="blocked"/>, otherwise false.
        /// </returns>
        public abstract bool Server_HasBlocked(int blocker, int blocked);

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
    
}