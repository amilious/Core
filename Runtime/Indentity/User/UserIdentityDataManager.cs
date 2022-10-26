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

using Amilious.Core.Saving;
using System.Collections.Generic;

namespace Amilious.Core.Indentity.User {
    
    /// <summary>
    /// This class is used to access, store, and edit information.
    /// </summary>
    public class UserIdentityDataManager : AmiliousBehavior {

        #region Server Data ////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This method is used to get the server's identifier.
        /// </summary>
        /// <returns>The servers identifier.</returns>
        public virtual string Server_GetServerIdentifier() => IdentitySave.Server_GetServerIdentifier();

        /// <summary>
        /// This method is used to check if the given user id is valid.
        /// </summary>
        /// <param name="userId">The user id that you want to check.</param>
        /// <returns>True if the given user id is valid, otherwise false.</returns>
        public virtual bool Server_IsUserIdValid(int userId) => IdentitySave.Server_IsUserIdValid(userId);

        /// <summary>
        /// This method is used to try get a user identity from the user's user id.
        /// </summary>
        /// <param name="userId">The user's id.</param>
        /// <param name="identity">The user's identity.</param>
        /// <returns>True if the user exists, otherwise false.</returns>
        public virtual bool Server_TryGetUserIdentity(int userId, out UserIdentity identity) =>
            IdentitySave.Server_TryGetUserIdentity(userId, out identity);

        /// <summary>
        /// This method is used to get a user identity from the user's user name.
        /// </summary>
        /// <param name="userName">The user's user name.</param>
        /// <param name="identity">The user's identity.</param>
        /// <returns>True if the user exists, otherwise false.</returns>
        public virtual bool Server_TryGetUserIdentity(string userName, out UserIdentity identity) =>
            IdentitySave.Server_TryGetUserIdentity(userName, out identity);

        /// <summary>
        /// This method is used to try get a user id from the given user name.
        /// </summary>
        /// <param name="userName">The user name.</param>
        /// <param name="userId">The user id associated with the user id.</param>
        /// <param name="caseSensitive">True if the user name is case sensitive, otherwise false.</param>
        /// <returns>True if able to find a user with the given user name, otherwise false.</returns>
        public virtual bool Server_TryGetIdFromUserName(string userName, out int userId, bool caseSensitive = false) =>
            IdentitySave.Server_TryGetIdFromUserName(userName, out userId, caseSensitive);

        /// <summary>
        /// This method is used to try read data for the given user.
        /// </summary>
        /// <param name="userId">The id of the user.</param>
        /// <param name="key">The key for the data that you want to read.</param>
        /// <param name="value">The read value.</param>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <returns>True if the data exists, otherwise false.</returns>
        public virtual bool Server_TryReadUserData<T>(int userId, string key, out T value) =>
            IdentitySave.Server_TryReadUserData(userId, key, out value);

        /// <summary>
        /// This method is used to store data for the given user.
        /// </summary>
        /// <param name="userId">The id of the user.</param>
        /// <param name="key">The key for the data that you want to store.</param>
        /// <param name="value">The value that you want to store for the given key.</param>
        /// <typeparam name="T">The type of the value.</typeparam>
        public virtual void Server_StoreUserData<T>(int userId, string key, T value) =>
            IdentitySave.Server_StoreUserData(userId, key, value);

        /// <summary>
        /// This method is used to add a new user to the server.
        /// </summary>
        /// <param name="userName">The user name for the user.</param>
        /// <returns>The id for the new user.</returns>
        public virtual UserIdentity Server_AddUser(string userName = null) => IdentitySave.Server_AddUser(userName);

        /// <summary>
        /// This method is used to get the id's for all of the registered users.
        /// </summary>
        /// <returns>The id's of the registered users.</returns>
        public virtual IEnumerable<int> Server_GetStoredUserIds() => IdentitySave.Server_GetStoredUserIds();

        /// <summary>
        /// This method is used to block a user.
        /// </summary>
        /// <param name="blocker">The user id of the user who is blocking another user.</param>
        /// <param name="blocked">The user id of the user that will be blocked.</param>
        public virtual void Server_BlockUser(int blocker, int blocked) =>
            IdentitySave.Server_BlockUser(blocker, blocked);

        /// <summary>
        /// This method is used to unblock a user.
        /// </summary>
        /// <param name="blocker">The user id of the user who is blocking another user.</param>
        /// <param name="blocked">The user id of the user that will be unblocked.</param>
        public virtual void Server_UnblockUser(int blocker, int blocked) =>
            IdentitySave.Server_UnblockUser(blocker, blocked);

        /// <summary>
        /// This method is used to get a read only list of the blocked user's for the given user.
        /// </summary>
        /// <param name="blockerId">The id of the blocker.</param>
        /// <returns>A list of the user's currently blocked by the given user.</returns>
        public virtual List<int> Server_GetBlockedUsers(int blockerId) => IdentitySave.Server_GetBlockedUsers(blockerId);

        /// <summary>
        /// This method is used to check if a user has blocked another.
        /// </summary>
        /// <param name="blocker">The user id of the user who blocked another user.</param>
        /// <param name="blocked">The user id of the user who was blocked.</param>
        /// <returns>True if the <paramref name="blocker"/> has blocked the <paramref name="blocked"/>, otherwise false.
        /// </returns>
        public virtual bool Server_HasBlocked(int blocker, int blocked) =>
            IdentitySave.Server_HasBlocked(blocker, blocked);

        /// <summary>
        /// This method is used to friend a user.
        /// </summary>
        /// <param name="userId">The user id of the user who is friending another user.</param>
        /// <param name="friendId">The user id of the user that will be friended.</param>
        public virtual void Server_FriendUser(int userId, int friendId) =>
            IdentitySave.Server_FriendUser(userId, friendId);

        /// <summary>
        /// This method is used to unfriend a user.
        /// </summary>
        /// <param name="userId">The user id of the user who is unfriending another user.</param>
        /// <param name="friendId">The user id of the user that will be unfriended.</param>
        public virtual void Server_UnfriendUser(int userId, int friendId) =>
            IdentitySave.Server_UnfriendUser(userId, friendId);

        /// <summary>
        /// This method is used to check if a user has friended another.
        /// </summary>
        /// <param name="userId">The user id of the user who friended another user.</param>
        /// <param name="friendId">The user id of the user who was friended.</param>
        /// <returns>True if the <paramref name="userId"/> has friended the <paramref name="friendId"/>, otherwise
        /// false. This does not mean that the friendship has been approved.
        /// </returns>
        public virtual bool Server_HasFriended(int userId, int friendId) =>
            IdentitySave.Server_HasFriended(userId, friendId);

        /// <summary>
        /// This method is used to check if their is an approved friendship between the two users.
        /// </summary>
        /// <param name="userId1">The first user.</param>
        /// <param name="userId2">The second user.</param>
        /// <returns>True if the two user's have an approved friendship together, otherwise false.</returns>
        public virtual bool Server_HasApprovedFriendship(int userId1, int userId2) =>
            IdentitySave.Server_HasApprovedFriendship(userId1, userId2);

        /// <summary>
        /// This method is used to get a list of the friends for the given user.
        /// </summary>
        /// <param name="userId">The id of the user.</param>
        /// <returns>A list of the user's friends including unaccepted friends.</returns>
        public virtual List<int> Server_GetFriends(int userId) => IdentitySave.Server_GetFriends(userId);

        /// <summary>
        /// This method is used to get a list of the approved friends for the given user.
        /// </summary>
        /// <param name="userId">The id of the user.</param>
        /// <returns>A list of the user's friends excluding unaccepted friends.</returns>
        public virtual List<int> Server_GetApprovedFriends(int userId) =>
            IdentitySave.Server_GetApprovedFriends(userId);

        /// <summary>
        /// This method is used to check if the user with the given id has set their password.
        /// </summary>
        /// <param name="userId">The user id of the user you want to check.</param>
        /// <returns>True if the user has set their password, otherwise false.</returns>
        public virtual bool Server_HasUserSetPassword(int userId) => IdentitySave.Server_HasUserSetPassword(userId);
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Client Data ////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to remember the last entered user name.
        /// </summary>
        /// <param name="userName">The user name that you want to remember.</param>
        public virtual void Client_StoreLastUserName(string userName) =>
            IdentitySave.Client_StoreLastUserName(userName);

        /// <summary>
        /// This method is used to remember the last entered password.
        /// </summary>
        /// <param name="password">The password that you want to remember.</param>
        public virtual void Client_StoreLastPassword(string password) =>
            IdentitySave.Client_StoreLastPassword(password);

        /// <summary>
        /// This method is used to get the saved user name.
        /// </summary>
        /// <param name="userName">The user name that you saved.</param>
        /// <returns>True if there is a saved user name, otherwise false.</returns>
        public virtual bool Client_TryGetLastUserName(out string userName) =>
            IdentitySave.Client_TryGetLastUserName(out userName);

        /// <summary>
        /// This method is used to get the saved password.
        /// </summary>
        /// <param name="password">The password that you saved.</param>
        /// <returns>True if there is a saved password, otherwise false.</returns>
        public virtual bool Client_TryGetLastPassword(out string password) =>
            IdentitySave.Client_TryGetLastPassword(out password);

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
    
}