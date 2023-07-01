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

namespace Amilious.Core.Identity.User {
    
    public interface IUserIdentityManager {
        
        #region Delegates //////////////////////////////////////////////////////////////////////////////////////////////
        
        public delegate void UserConnectionChangedDelegate(UserIdentity identity, bool connected);
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Events /////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This event is triggered when a connection changes.
        /// </summary>
        public event UserConnectionChangedDelegate OnUserConnectionChanged;

        /// <summary>
        /// This event is triggered when the friend list is updated on the client.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public event Action Client_OnFriendListUpdated;

        /// <summary>
        /// This event is triggered when the pending friend list is updated on the client.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public event Action Client_OnPendingFriendListUpdated;

        /// <summary>
        /// This event is triggered when the requesting friend list is updated on the client.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public event Action Client_OnRequestingFriendListUpdated;

        /// <summary>
        /// This event is triggered when the blocked list is updated on the client.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public event Action Client_OnBlockedListUpdated;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Client & Server Properties  ////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This property is used to get the user identity for the given id.
        /// </summary>
        /// <param name="userId">The user's id.</param>
        public UserIdentity this[uint userId] { get; }

        /// <summary>
        /// This property is used to get a collection of identities.
        /// </summary>
        IEnumerable<UserIdentity> Identities { get; }
        
        /// <summary>
        /// If this property is true, friends require acceptance, otherwise only one side needs to add.
        /// </summary>
        public bool FriendsNeedAcceptance { get; }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Client Only Properties /////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This property is used to get a collection of the identities that meet the given flags.
        /// </summary>
        /// <param name="flags">The user filter flags.</param>
        /// <remarks>This property should be called from the client.</remarks>
        public IEnumerable<UserIdentity> this[UserFilterFlags flags] { get; }

        /// <summary>
        /// This property is used to get a collection of friend identities.
        /// </summary>
        /// <remarks>This property should be called from the client.</remarks>
        IEnumerable<UserIdentity> Friends { get;}
        
        /// <summary>
        /// This property is used to get a collection of identities that are awaiting friendship approval.
        /// </summary>
        /// <remarks>This property should be called from the client.</remarks>
        IEnumerable<UserIdentity> PendingFriends { get; }
        
        /// <summary>
        /// This property is used to get a collection of identities that are requesting approval.
        /// </summary>
        /// <remarks>This property should be called from the client.</remarks>
        IEnumerable<UserIdentity> RequestingFriendShip { get; }

        /// <summary>
        /// This property is used to a collection of blocked users.
        /// </summary>
        /// <remarks>This property should be called from the client.</remarks>
        IEnumerable<UserIdentity> BlockedUsers { get; }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Client Only Methods ////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to remove a friend.
        /// </summary>
        /// <param name="friendId">The friend's identity id.</param>
        /// <seealso cref="Client_RemoveFriend(UserIdentity)"/>
        /// <remarks>This method should be called from the client.</remarks>
        void Client_RemoveFriend(uint friendId);

        /// <summary>
        /// This method is used to remove a friend.
        /// </summary>
        /// <param name="friend">The friend's identity.</param>
        /// <seealso cref="Client_RemoveFriend(uint)"/>
        /// <remarks>This method should be called from the client.</remarks>
        void Client_RemoveFriend(UserIdentity friend);

        /// <summary>
        /// This method is used to add a friend.
        /// </summary>
        /// <param name="friendId">The friend's identity id.</param>
        /// <seealso cref="Client_AddFriend(UserIdentity)"/>
        /// <remarks>This method should be called from the client.</remarks>
        void Client_AddFriend(uint friendId);

        /// <summary>
        /// This method is used to add a friend.
        /// </summary>
        /// <param name="friend">The friend's identity.</param>
        /// <seealso cref="Client_AddFriend(uint)"/>
        /// <remarks>This method should be called from the client.</remarks>
        void Client_AddFriend(UserIdentity friend);

        /// <summary>
        /// This method is used to block a user.
        /// </summary>
        /// <param name="user">The user that you want to block.</param>
        /// <seealso cref="Client_BlockUser(uint)"/>
        /// <remarks>This method should be called from the client.</remarks>
        void Client_BlockUser(UserIdentity user);

        /// <summary>
        /// This method is used to block a user.
        /// </summary>
        /// <param name="userId">The user id of the user that you want to block.</param>
        /// <seealso cref="Client_BlockUser(UserIdentity)"/>
        /// <remarks>This method should be called from the client.</remarks>
        void Client_BlockUser(uint userId);

        /// <summary>
        /// This method is used to block a user.
        /// </summary>
        /// <param name="user">The user that you want to unblock.</param>
        /// <seealso cref="Client_UnblockUser(uint)"/>
        /// <remarks>This method should be called from the client.</remarks>
        void Client_UnblockUser(UserIdentity user);

        /// <summary>
        /// This method is used to unblock a user.
        /// </summary>
        /// <param name="userId">The user id of the user that you want to unblock.</param>
        /// <seealso cref="Client_UnblockUser(UserIdentity)"/>
        /// <remarks>This method should be called from the client.</remarks>
        void Client_UnblockUser(uint userId);
        
        /// <summary>
        /// This method is used to check if a user is a friend.
        /// </summary>
        /// <param name="identity">The identity of the user.</param>
        /// <returns>True if the given identity is a friend, otherwise false.</returns>
        /// <seealso cref="Client_IsFriend(uint)"/>
        /// <remarks>This method should be called from the client.</remarks>
        bool Client_IsFriend(UserIdentity identity);
        
        /// <summary>
        /// This method is used to check if a user is a friend.
        /// </summary>
        /// <param name="id">The id of the user.</param>
        /// <returns>True if the given identity is a friend, otherwise false.</returns>
        /// <seealso cref="Client_IsFriend(UserIdentity)"/>
        /// <remarks>This method should be called from the client.</remarks>
        bool Client_IsFriend(uint id);

        /// <summary>
        /// This method is used to check if a user is blocked.
        /// </summary>
        /// <param name="identity">The identity of the user.</param>
        /// <returns>True if the given user is blocked, otherwise false.</returns>
        /// <seealso cref="Client_IsBlocked(uint)"/>
        /// <remarks>This method should be called from the client.</remarks>
        bool Client_IsBlocked(UserIdentity identity);
        
        /// <summary>
        /// This method is used to check if a user is blocked.
        /// </summary>
        /// <param name="id">The id of the user.</param>
        /// <returns>True if the given user is blocked, otherwise false.</returns>
        /// <seealso cref="Client_IsBlocked(UserIdentity)"/>
        /// <remarks>This method should be called from the client.</remarks>
        bool Client_IsBlocked(uint id);

        /// <summary>
        /// This method is used to check if a user is requesting friendship with you.
        /// </summary>
        /// <param name="identity">The identity of the user.</param>
        /// <returns>True if the given user is requesting friendship with you, otherwise false.</returns>
        /// <seealso cref="Client_IsRequestingFriendship(uint)"/>
        /// <remarks>This method should be called from the client.</remarks>
        bool Client_IsRequestingFriendship(UserIdentity identity);
        
        /// <summary>
        /// This method is used to check if a user is requesting friendship with you.
        /// </summary>
        /// <param name="id">The id of the user.</param>
        /// <returns>True if the given user is requesting friendship with you, otherwise false.</returns>
        /// <seealso cref="Client_IsRequestingFriendship(UserIdentity)"/>
        /// <remarks>This method should be called from the client.</remarks>
        bool Client_IsRequestingFriendship(uint id);

        /// <summary>
        /// This method is used to check if you have a pending friendship with the given user.
        /// </summary>
        /// <param name="identity">The identity of the user.</param>
        /// <returns>True if you are pending a friendship with the given user.</returns>
        /// <seealso cref="Client_IsPendingFriendship(uint)"/>
        /// <remarks>This method should be called from the client.</remarks>
        bool Client_IsPendingFriendship(UserIdentity identity);
        
        /// <summary>
        /// This method is used to check if you have a pending friendship with the given user.
        /// </summary>
        /// <param name="id">The id of the user.</param>
        /// <returns>True if you are pending a friendship with the given user.</returns>
        /// <seealso cref="Client_IsPendingFriendship(UserIdentity)"/>
        /// <remarks>This method should be called from the client.</remarks>
        bool Client_IsPendingFriendship(uint id);

        /// <summary>
        /// This method is used to get the current user's identity.
        /// </summary>
        /// <returns>The identity associated with the current user.</returns>
        /// <remarks>This method should only be called from the client!</remarks>
        UserIdentity Client_GetIdentity();

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Client & Server Methods ////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to try get the <see cref="UserIdentity"/> for the given id.
        /// </summary>
        /// <param name="id">The id of the <see cref="UserIdentity"/> that you want to get.</param>
        /// <param name="identity">The identity for the given id.</param>
        /// <returns>True if an <see cref="UserIdentity"/> was found with the given id, otherwise false.</returns>
        /// <seealso cref="TryGetIdentity(string,out UserIdentity)"/>
        /// <remarks>This can be called from the client or server!</remarks>
        bool TryGetIdentity(uint id, out UserIdentity identity);

        /// <summary>
        /// This method is used to try get the <see cref="UserIdentity"/> for the given user name.
        /// </summary>
        /// <param name="userName">The user name that you want to get the user identity for.</param>
        /// <param name="identity">The identity for the given user name.</param>
        /// <returns>True if an <see cref="UserIdentity"/> was found with the given user name, otherwise false.</returns>
        /// <seealso cref="TryGetIdentity(uint,out UserIdentity)"/>
        /// <remarks>This can be called from the client or server!</remarks>
        bool TryGetIdentity(string userName, out UserIdentity identity);

        /// <summary>
        /// This method is used to check if the user with the given id is currently online.
        /// </summary>
        /// <param name="id">The id of the user.</param>
        /// <seealso cref="IsOnline(UserIdentity)"/>
        /// <returns>True if you are connected and so is the user with the given id, otherwise false.</returns>
        bool IsOnline(uint id);

        /// <summary>
        /// This method is used to check if the user is currently online.
        /// </summary>
        /// <param name="identity">The id of the user.</param>
        /// <seealso cref="IsOnline(uint)"/>
        /// <returns>True if you are connected and so is the given user, otherwise false.</returns>
        bool IsOnline(UserIdentity identity);

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Server Only Methods ////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to check if a user is able to send a message to another user.
        /// </summary>
        /// <param name="sender">The sender of the message.</param>
        /// <param name="recipient">The receiver of the message.</param>
        /// <returns>True if the sender is able to send a message to the recipient, otherwise false.</returns>
        /// <remarks>This method should only be called from the server!</remarks>
        bool Server_CanSendMessage(uint sender, uint recipient);
        
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
        bool Server_TrySetAuthority(uint userId, int authority = int.MaxValue);

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
        bool Server_TryUpdateUserName(uint userId, string userName);
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
    
}