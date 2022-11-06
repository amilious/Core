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
using System.Collections.Generic;

namespace Amilious.Core.Indentity.User {
    
    public interface IUserIdentityManager {
        
        public delegate void UserConnectionChangedDelegate(UserIdentity identity, bool connected);
        
        /// <summary>
        /// This event is triggered when a connection changes.
        /// </summary>
        public event UserConnectionChangedDelegate OnUserConnectionChanged;

        /// <summary>
        /// This event is triggered when the friend list is updated on the client.
        /// </summary>
        public event Action Client_OnFriendListUpdated;

        /// <summary>
        /// This event is triggered when the pending friend list is updated on the client.
        /// </summary>
        public event Action Client_OnPendingFriendListUpdated;

        /// <summary>
        /// This event is triggered when the requesting friend list is updated on the client.
        /// </summary>
        public event Action Client_OnRequestingFriendListUpdated;

        /// <summary>
        /// This event is triggered when the blocked list is updated on the client.
        /// </summary>
        public event Action Client_OnBlockedListUpdated;

        /// <summary>
        /// This property is used to get the user identity for the given id.
        /// </summary>
        /// <param name="userId"></param>
        public UserIdentity this[int userId] { get; }
        
        /// <summary>
        /// This property is used to get a collection of identities.
        /// </summary>
        IEnumerable<UserIdentity> Identities { get; }

        /// <summary>
        /// This property is used to get a collection of friend identities.
        /// </summary>
        IEnumerable<UserIdentity> Friends { get;}
        
        /// <summary>
        /// This property is used to get a collection of identities that are awaiting friendship approval.
        /// </summary>
        IEnumerable<UserIdentity> PendingFriends { get; }
        
        /// <summary>
        /// This property is used to get a collection of identities that are requesting approval.
        /// </summary>
        IEnumerable<UserIdentity> RequestingFriendShip { get; }

        /// <summary>
        /// This property is used to a collection of blocked users.
        /// </summary>
        IEnumerable<UserIdentity> BlockedUsers { get; }
        
        /// <summary>
        /// If this property is true, friends require acceptance, otherwise only one side needs to add.
        /// </summary>
        public bool FriendsNeedAcceptance { get; }
        
        /// <summary>
        /// This method is used to remove a friend.
        /// </summary>
        /// <param name="friendId">The friend's identity id.</param>
        /// <remarks>This method should be called from the client.</remarks>
        void Client_RemoveFriend(int friendId);

        /// <summary>
        /// This method is used to remove a friend.
        /// </summary>
        /// <param name="friend">The friend's identity.</param>
        /// <remarks>This method should be called from the client.</remarks>
        void Client_RemoveFriend(UserIdentity friend);

        /// <summary>
        /// This method is used to add a friend.
        /// </summary>
        /// <param name="friendId">The friend's identity id.</param>
        /// <remarks>This method should be called from the client.</remarks>
        void Client_AddFriend(int friendId);

        /// <summary>
        /// This method is used to add a friend.
        /// </summary>
        /// <param name="friend">The friend's identity.</param>
        /// <remarks>This method should be called from the client.</remarks>
        void Client_AddFriend(UserIdentity friend);

        /// <summary>
        /// This method is used to block a user.
        /// </summary>
        /// <param name="user">The user that you want to block.</param>
        void Client_BlockUser(UserIdentity user);

        /// <summary>
        /// This method is used to block a user.
        /// </summary>
        /// <param name="userId">The user id of the user that you want to block.</param>
        void Client_BlockUser(int userId);

        /// <summary>
        /// This method is used to block a user.
        /// </summary>
        /// <param name="user">The user that you want to unblock.</param>
        void Client_UnblockUser(UserIdentity user);

        /// <summary>
        /// This method is used to unblock a user.
        /// </summary>
        /// <param name="userId">The user id of the user that you want to unblock.</param>
        void Client_UnblockUser(int userId);

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
        /// This method is used to check if the user with the given id is currently online.
        /// </summary>
        /// <param name="id">The id of the user.</param>
        /// <returns>True if you are connected and so is the user with the given id, otherwise false.</returns>
        bool IsOnline(int id);

        /// <summary>
        /// This method is used to check if the user is currently online.
        /// </summary>
        /// <param name="identity">The id of the user.</param>
        /// <returns>True if you are connected and so is the given user, otherwise false.</returns>
        bool IsOnline(UserIdentity identity);

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
        bool Server_CanSendMessage(int sender, int recipient);
        
        /// <summary>
        /// This method is used to change an identity's authority.
        /// </summary>
        /// <param name="identity">The identity that you want to update.</param>
        /// <param name="authority">The identity's authority (bigger is less authority)</param>
        /// <returns>True if the user exists and was updated, otherwise false.</returns>
        /// <remarks>This method should only be called on the server!</remarks>
        bool Server_TrySetAuthority(UserIdentity identity, int authority = int.MaxValue);

        /// <summary>
        /// This method is used to change an identity's authority.
        /// </summary>
        /// <param name="userId">The id of the identity that you want to update.</param>
        /// <param name="authority">The identity's authority (bigger is less authority)</param>
        /// <returns>True if the user exists and was updated, otherwise false.</returns>
        /// <remarks>This method should only be called on the server!</remarks>
        bool Server_TrySetAuthority(int userId, int authority = int.MaxValue);

        /// <summary>
        /// This method is used to change an identity's user name.
        /// </summary>
        /// <param name="identity">The identity that you want to update.</param>
        /// <param name="userName">The identity's new user name.</param>
        /// <returns>True if the user exists, the user name is available, and was updated, otherwise false.</returns>
        /// <remarks>This method should only be called on the server!</remarks>
        bool Server_TryUpdateUserName(UserIdentity identity, string userName);

        /// <summary>
        /// This method is used to change an identity's user name.
        /// </summary>
        /// <param name="userId">The id of the identity that you want to update.</param>
        /// <param name="userName">The identity's new user name.</param>
        /// <returns>True if the user exists, the user name is available, and was updated, otherwise false.</returns>
        /// <remarks>This method should only be called on the server!</remarks>
        bool Server_TryUpdateUserName(int userId, string userName);
        
    }
    
}