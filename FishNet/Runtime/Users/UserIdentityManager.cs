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
//  Discord Server: https://discord.gg/SNqyDWu            CopyrightÂ© Amilious since 2022                              //                    
//  This code is part of an asset on the unity asset store. If you did not get this from the asset store you are not  //
//  using it legally. Check the asset store or join the discord for the license that applies for this script.         //
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/

using System;
using UnityEngine;
using FishNet.Object;
using FishNet.Connection;
using FishNet.Transporting;
using Amilious.Core.Attributes;
using Amilious.Core.Extensions;
using System.Collections.Generic;
using Amilious.Core.Identity.User;
using FishNet.Object.Synchronizing;

namespace Amilious.Core.FishNet.Users {
    
    public class UserIdentityManager : NetworkBehaviour, IUserIdentityManager {

        #region Inspector Fields ///////////////////////////////////////////////////////////////////////////////////////

        [SerializeField,AmiliousBool(true)]
        [Tooltip("If true friendships must be accepted, otherwise no acceptance is needed.")]
        private bool friendshipsRequireAcceptance = true;
        
        #endregion
        
        #region Private Fields /////////////////////////////////////////////////////////////////////////////////////////

        private int _localUserId;

        /// <summary>
        /// This list is used to store all of the user's friends on the client.
        /// </summary>
        private readonly List<int> _friends = new List<int>();

        /// <summary>
        /// This list is used to store all of the user's friendships that are currently pending.
        /// </summary>
        private readonly List<int> _pendingFriends = new List<int>();
        
        /// <summary>
        /// This list is used to store all of the user's friendship requests.
        /// </summary>
        private readonly List<int> _requestingFriends = new List<int>();
        
        /// <summary>
        /// This list is used to store all of the user's blocked users on the client.
        /// </summary>
        private readonly List<int> _blocked = new List<int>();
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Events /////////////////////////////////////////////////////////////////////////////////////////////////

        ///<inheritdoc />
        public event IUserIdentityManager.UserConnectionChangedDelegate OnUserConnectionChanged;
        
        ///<inheritdoc />
        public event Action Client_OnFriendListUpdated;

        ///<inheritdoc />
        public event Action Client_OnPendingFriendListUpdated;
        
        ///<inheritdoc />
        public event Action Client_OnRequestingFriendListUpdated;

        ///<inheritdoc />
        public event Action Client_OnBlockedListUpdated;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Sync Variables /////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This dictionary is used to store all of the user's identities.
        /// </summary>
        [SyncObject] 
        private readonly SyncDictionary<int, UserIdentity> _userLookup = new SyncDictionary<int, UserIdentity>();
        
        /// <summary>
        /// This hash set is used to store all of the online user ids.
        /// </summary>
        [SyncObject] private readonly SyncHashSet<int> _online = new SyncHashSet<int>();

        [SyncVar(Channel = Channel.Reliable)] 
        private string _serverIdentifier;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////

        public static UserIdentityDataManager UserIdDataManager => UserIdentityDataManager.Instance;
        
        /// <summary>
        /// This property is used to get the server identifier.
        /// </summary>
        public string ServerIdentifier => _serverIdentifier;

        public IEnumerable<UserIdentity> this[UserFilterFlags flags] {
            get {
                foreach(var identity in Identities) {
                    //check if exclude self
                    if(flags.HasFlag(UserFilterFlags.ExcludeSelf) && identity.Id == Client_GetIdentity().Id) continue;
                    //check if all excluding self (exclude self is already handled)
                    if(flags.HasFlag(UserFilterFlags.AllIncludingSelf)) yield return identity;
                    //check online status
                    var isOnline = _online.Contains(identity.Id);
                    var hasOnlineFlag = flags.HasFlag(UserFilterFlags.Online);
                    var hasOfflineFlag = flags.HasFlag(UserFilterFlags.Offline);
                    if(hasOfflineFlag || hasOnlineFlag) { 
                        //only filter if has online or offline flag
                        if(isOnline && !hasOnlineFlag) continue;
                        if(!isOnline && !hasOfflineFlag) continue;
                    }
                    //check blocked status
                    var isBlocked = _blocked.Contains(identity.Id);
                    var hasBlockedFlag = flags.HasFlag(UserFilterFlags.Blocked);
                    var hasNotBlockedFlag = flags.HasFlag(UserFilterFlags.NotBlocked);
                    if(hasBlockedFlag || hasNotBlockedFlag) {
                        //only filter if has blocked or NotBlocked flag
                        if(isBlocked && !hasBlockedFlag) continue;
                        if(!isBlocked && !hasNotBlockedFlag) continue;
                    }
                    //check if friend
                    var isFriend = _friends.Contains(identity.Id);
                    if(isFriend && flags.HasFlag(UserFilterFlags.Friend)){
                        yield return identity;
                        continue;
                    }
                    //check if pending
                    var isPending = _pendingFriends.Contains(identity.Id);
                    if(isPending && flags.HasFlag(UserFilterFlags.PendingFriend)){
                        yield return identity;
                        continue;
                    }
                    //check if request
                    var isRequest = _requestingFriends.Contains(identity.Id);
                    if(isRequest && flags.HasFlag(UserFilterFlags.RequestingFriendship)) {
                        yield return identity;
                        continue;
                    }
                    //return true if noFriendship flag otherwise false
                    if(flags.HasFlag(UserFilterFlags.NoFriendship))  yield return identity;
                }
            }
        }

        /// <inheritdoc />
        public IEnumerable<UserIdentity> Identities => _userLookup.Values;

        /// <inheritdoc />
        public UserIdentity this[int userId] {
            get {
                TryGetIdentity(userId, out var userIdentity);
                return userIdentity;
            }
        }

        /// <inheritdoc />
        public IEnumerable<UserIdentity> Friends {
            get {
                foreach(var id in _friends)
                    if(_userLookup.TryGetValueFix(id, out var identity)) yield return identity;
            }
        }

        /// <inheritdoc />
        public IEnumerable<UserIdentity> PendingFriends {
            get {
                foreach(var id in _pendingFriends)
                    if(_userLookup.TryGetValueFix(id, out var identity)) yield return identity;
            }
        }

        /// <inheritdoc />
        public IEnumerable<UserIdentity> RequestingFriendShip {
            get {
                foreach(var id in _requestingFriends)
                    if(_userLookup.TryGetValueFix(id, out var identity)) yield return identity;
            }
        }

        /// <inheritdoc />
        public IEnumerable<UserIdentity> BlockedUsers {
            get {
                foreach(var id in _blocked)
                    if(_userLookup.TryGetValueFix(id, out var identity)) yield return identity;
            }
        }

        /// <inheritdoc />
        public bool FriendsNeedAcceptance => friendshipsRequireAcceptance;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Monobehavior Methods ///////////////////////////////////////////////////////////////////////////////////
        
        private void Awake() {
            _online.OnChange += OnlineChanged;
            UserIdentity.SetIdentityManager(this);
            if(!IsServer) return; //load on server only
            _serverIdentifier = UserIdDataManager.Server_GetServerIdentifier();
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Interface Methods //////////////////////////////////////////////////////////////////////////////////////

        /// <inheritdoc />
        public virtual bool TryGetIdentity(int id, out UserIdentity identity) {
            var found = _userLookup.TryGetValueFix(id, out identity);
            if(!found) identity = UserIdentity.DefaultUser;
            return found;
        }

        /// <inheritdoc />
        public bool TryGetIdentity(string userName, out UserIdentity identity) {
            foreach(var ident in _userLookup.Values) {
                if(!ident.Name.Equals(userName,StringComparison.InvariantCultureIgnoreCase)) continue;
                identity = ident;
                return true;
            }
            identity = UserIdentity.DefaultUser;
            return false;
        }

        /// <inheritdoc />
        public bool IsOnline(int id) => _online.Contains(id);

        /// <inheritdoc />
        public bool IsOnline(UserIdentity identity) => IsOnline(identity.Id);


        /// <inheritdoc />
        public virtual UserIdentity Client_GetIdentity() {
            if(!NetworkManager.TryGetLocalUserId(out var id)) return UserIdentity.DefaultUser;
            return TryGetIdentity(id, out var identity) ? identity : UserIdentity.DefaultUser;
        }

        /// <inheritdoc />
        public virtual bool Server_CanSendMessage(int sender, int recipient) {
            return !UserIdDataManager.Server_HasBlocked(sender, recipient) && 
                   !UserIdDataManager.Server_HasBlocked(recipient, sender) && 
                   _online.Contains(recipient);
        }

        /// <inheritdoc />
        public void Client_RemoveFriend(int friendId) => UpdateFriendsOnServer(friendId,false);

        /// <inheritdoc />
        public void Client_RemoveFriend(UserIdentity friend) => Client_RemoveFriend(friend.Id);

        /// <inheritdoc />
        public void Client_AddFriend(int friendId) => UpdateFriendsOnServer(friendId,true);

        /// <inheritdoc />
        public void Client_AddFriend(UserIdentity friend) => Client_AddFriend(friend.Id);

        /// <inheritdoc />
        public void Client_BlockUser(UserIdentity user) => Client_BlockUser(user.Id);

        /// <inheritdoc />
        public void Client_BlockUser(int userId) => UpdateBlockedOnServer(userId, true);

        /// <inheritdoc />
        public void Client_UnblockUser(UserIdentity user) => Client_UnblockUser(user.Id);

        /// <inheritdoc />
        public void Client_UnblockUser(int userId)  => UpdateBlockedOnServer(userId, false);

        /// <inheritdoc />
        public bool Client_IsFriend(UserIdentity identity) => Client_IsFriend(identity.Id);

        /// <inheritdoc />
        public bool Client_IsFriend(int id) => _friends.Contains(id);

        /// <inheritdoc />
        public bool Client_IsBlocked(UserIdentity identity) => Client_IsBlocked(identity.Id);

        /// <inheritdoc />
        public bool Client_IsBlocked(int id) => _blocked.Contains(id);

        /// <inheritdoc />
        public bool Client_IsRequestingFriendship(UserIdentity identity) => Client_IsRequestingFriendship(identity.Id);

        /// <inheritdoc />
        public bool Client_IsRequestingFriendship(int id) => _requestingFriends.Contains(id);

        /// <inheritdoc />
        public bool Client_IsPendingFriendship(UserIdentity identity) => Client_IsPendingFriendship(identity.Id);

        /// <inheritdoc />
        public bool Client_IsPendingFriendship(int id) => _pendingFriends.Contains(id);

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region FishNet Only Methods ///////////////////////////////////////////////////////////////////////////////////
        
        private void OnlineChanged(SyncHashSetOperation op, int item, bool server) {
            if(server) return;
            var online = op != SyncHashSetOperation.Remove;
            if(!TryGetIdentity(item, out var identity)) return;
            OnUserConnectionChanged?.Invoke(identity,online);
        }
        
        public override void OnStartServer() {
            base.OnStartServer();
            //register authenticator
            NetworkManager.ServerManager.OnAuthenticationResult += OnAuthenticationResult;
            NetworkManager.ServerManager.OnRemoteConnectionState += OnRemoteConnectionState;
            //load the users
            foreach(var id in UserIdDataManager.Server_GetStoredUserIds()) UpdateIdentity(id);
        }

        public override void OnStartClient() {
            base.OnStartClient();
            RequestUniqueData();
        }

        private void OnRemoteConnectionState(NetworkConnection con, RemoteConnectionStateArgs args) {
            if(!con.TryGetUserId(out var userId)) return;
            if(args.ConnectionState != RemoteConnectionState.Stopped) return;
            //update disconnect time
            UserIdDataManager.Server_StoreUserData(userId,UserIdentity.LAST_DISCONNECTED_KEY,DateTime.UtcNow);
            UpdateIdentity(userId);
            _online.Remove(userId);
        }

        /// <summary>
        /// This method should be called when the user identity information is updated.
        /// </summary>
        /// <param name="id">The updated identity id.</param>
        [Server]
        public void UpdateIdentity(int id) {
            if(!UserIdDataManager.Server_TryGetUserIdentity(id, out var identity)) {
                _userLookup.Remove(id); 
                return;
            }
            //update the user info
            if(!_userLookup.TryGetValueFix(id, out var sIdentity) || identity != sIdentity) {
                _userLookup[id] = identity;
                _userLookup.Dirty(id);
            } 
        }

        /// <summary>
        /// This method will be called when a user is authenticated on the server.
        /// </summary>
        /// <param name="con">The user's connection.</param>
        /// <param name="authenticated">True if the user was successfully authenticated, otherwise false.</param>
        private void OnAuthenticationResult(NetworkConnection con, bool authenticated) {
            if(!authenticated) return;
            if(!con.TryGetUserId(out var id)) return;
            if(!_online.Contains(id)) _online.Add(id);
            UserIdDataManager.Server_StoreUserData(id,UserIdentity.LAST_CONNECTED_KEY,DateTime.UtcNow);
            if(_userLookup.ContainsKey(id)) {
                UpdateIdentity(id);
                return;
            }
            if(!UserIdDataManager.Server_TryGetUserIdentity(id,out var identity))return;
            _userLookup.Add(id,identity);
        }
        
        /// <summary>
        /// This method is used to get a <see cref="NetworkConnection"/> from a <see cref="UserIdentity"/>.
        /// </summary>
        /// <param name="identity">The identity that you want to get the connection for.</param>
        /// <param name="connection">The connection associated with the given identity</param>
        /// <returns>True if able to find a connection for the given identity, otherwise false.</returns>
        /// <remarks>This method should only be called from the server!</remarks>
        [Server]
        public virtual bool TryGetConnection(UserIdentity identity, out NetworkConnection connection) {
            return identity.TryGetConnection(NetworkManager,out connection);
        }

        /// <summary>
        /// This method is used to get a <see cref="NetworkConnection"/> from a <see cref="UserIdentity"/>.
        /// </summary>
        /// <param name="identityId">The identity id that you want to get the connection for.</param>
        /// <param name="connection">The connection associated with the given identity id</param>
        /// <returns>True if able to find a connection for the given identity id, otherwise false.</returns>
        /// <remarks>This method should only be called from the server!</remarks>
        [Server]
        public virtual bool TryGetConnection(int identityId, out NetworkConnection connection) {
            if(TryGetIdentity(identityId, out var identity)) 
                return identity.TryGetConnection(NetworkManager,out connection);
            connection = null;
            return false;
        }

        /// <summary>
        /// This method is used to get a <see cref="UserIdentity"/> from the given <see cref="NetworkConnection"/>.
        /// </summary>
        /// <param name="connection">The connection that you want to get the <see cref="UserIdentity"/> for.</param>
        /// <param name="identity">The <see cref="UserIdentity"/> associated with the given connection.</param>
        /// <returns>True if a <see cref="UserIdentity"/> was found for the given connection, otherwise false.</returns>
        /// <remarks>This method should only be called from the server!</remarks>
        [Server]
        public virtual bool TryGetIdentity(NetworkConnection connection, out UserIdentity identity) {
            if(connection.TryGetUserId(out var identityId)) 
                return TryGetIdentity(identityId, out identity);
            identity = default;
            return false;
        }

        /// <inheritdoc />
        [Server]
        public virtual bool Server_TrySetAuthority(UserIdentity identity, int value) => Server_TrySetAuthority(identity.Id, value);

        /// <inheritdoc />
        [Server]
        public virtual bool Server_TrySetAuthority(int userId, int value) {
            if(!UserIdDataManager.Server_IsUserIdValid(userId)) return false;
            UserIdDataManager.Server_StoreUserData(userId,UserIdentity.AUTHORITY_KEY,value);
            UpdateIdentity(userId);
            return true;
        }

        /// <inheritdoc />
        [Server]
        public virtual bool Server_TryUpdateUserName(UserIdentity identity, string userName) =>
            Server_TryUpdateUserName(identity.Id, userName);   
        
        /// <inheritdoc />
        [Server]
        public virtual bool Server_TryUpdateUserName(int userId, string userName) {
            if(!UserIdDataManager.Server_IsUserIdValid(userId)) return false;
            //make sure the user name is not being used
            if(UserIdDataManager.Server_TryGetIdFromUserName(userName, out _)) return false;
            UserIdDataManager.Server_StoreUserData(userId,UserIdentity.USER_NAME_KEY,userName);
            UpdateIdentity(userId);
            return true;
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region RPCs ///////////////////////////////////////////////////////////////////////////////////////////////////

        [TargetRpc]
        // ReSharper disable once UnusedParameter.Local
        private void SendFriendsToClient(NetworkConnection con, int[] friendIds) {
            _friends.Clear();
            foreach(var id in friendIds)_friends.Add(id);
            Client_OnFriendListUpdated?.Invoke();
        }

        [TargetRpc]
        // ReSharper disable once UnusedParameter.Local
        private void SendPendingFriendsToClient(NetworkConnection con, int[] pendingFriends) {
            _pendingFriends.Clear();
            foreach(var id in pendingFriends) _pendingFriends.Add(id);
            Client_OnPendingFriendListUpdated?.Invoke();
        }

        [TargetRpc]
        // ReSharper disable once UnusedParameter.Local
        private void SendRequestingFriendsToClient(NetworkConnection con, int[] requestingFriends) {
            _requestingFriends.Clear();
            foreach(var id in requestingFriends) _requestingFriends.Add(id);
            Client_OnRequestingFriendListUpdated?.Invoke();
        }

        [TargetRpc]
        // ReSharper disable once UnusedParameter.Local
        private void SendBlockedToClient(NetworkConnection con, int[] blockedIds) {
            _blocked.Clear();
            foreach(var id in blockedIds)_blocked.Add(id);
            Client_OnBlockedListUpdated?.Invoke();
        }

        [TargetRpc]
        // ReSharper disable once UnusedParameter.Local
        private void SendBlockedUpdated(NetworkConnection con, int userId, bool isBlocked) {
            if(isBlocked && !_blocked.Contains(userId)) _blocked.Add(userId);
            if(!isBlocked && _blocked.Contains(userId)) _blocked.Remove(userId);
            Client_OnBlockedListUpdated?.Invoke();
        }

        [TargetRpc]
        // ReSharper disable once UnusedParameter.Local
        private void SendFriendUpdated(NetworkConnection con, int userId, bool isFriend) {
            if(isFriend&&!_friends.Contains(userId)) _friends.Add(userId);
            if(!isFriend && _friends.Contains(userId)) _friends.Remove(userId);
            Client_OnFriendListUpdated?.Invoke();
        }

        [TargetRpc]
        // ReSharper disable once UnusedParameter.Local
        private void SendPendingFriendUpdated(NetworkConnection con, int userId, bool isPending) {
            if(isPending&&!_pendingFriends.Contains(userId)) _pendingFriends.Add(userId);
            if(!isPending && _pendingFriends.Contains(userId)) _pendingFriends.Remove(userId);
            Client_OnPendingFriendListUpdated?.Invoke();
        }

        [TargetRpc]
        // ReSharper disable once UnusedParameter.Local
        private void SendRequestingFriendUpdated(NetworkConnection con, int userId, bool isRequesting) {
            if(isRequesting&&!_requestingFriends.Contains(userId)) _requestingFriends.Add(userId);
            if(!isRequesting && _requestingFriends.Contains(userId)) _requestingFriends.Remove(userId);
            Client_OnRequestingFriendListUpdated?.Invoke();
        }

        [ServerRpc(RequireOwnership = false)]
        private void RequestUniqueData(NetworkConnection con = null) {
            if(con == null) con = LocalConnection;
            if(!con.TryGetUserId(out var userId)) return;
            if(FriendsNeedAcceptance) {
                var approved = UserIdDataManager.Server_GetApprovedFriends(userId).ToArray();
                var notApproved = UserIdDataManager.Server_GetNotApprovedFriends(userId).ToArray();
                var requesting = UserIdDataManager.Server_GetRequestingFriends(userId).ToArray();
                SendFriendsToClient(con,approved);
                SendPendingFriendsToClient(con,notApproved);
                SendRequestingFriendsToClient(con,requesting);
            }else {
                var friends = UserIdDataManager.Server_GetFriends(userId).ToArray();
                //send friends
                SendFriendsToClient(con,friends);
            }
            //send blocked
            SendBlockedToClient(con,UserIdDataManager.Server_GetBlockedUsers(userId).ToArray());
        }
        
        [ServerRpc(RequireOwnership = false)]
        private void UpdateFriendsOnServer(int userId, bool isFriend, NetworkConnection con = null) {
            if(!con.TryGetUserId(out var id)) return;
            if(!UserIdDataManager.Server_IsUserIdValid(userId)) return;
            if(isFriend == UserIdDataManager.Server_HasFriended(id, userId)) return;
            //unblock if blocked and adding friend
            if(isFriend && UserIdDataManager.Server_HasBlocked(id, userId)) {
                UserIdDataManager.Server_UnblockUser(id,userId);
                SendBlockedUpdated(con,userId,false);
            }
            //add friend
            if(isFriend)UserIdDataManager.Server_FriendUser(id,userId);
            else UserIdDataManager.Server_UnfriendUser(id,userId);
            if(!friendshipsRequireAcceptance) {
                SendFriendUpdated(con,userId,isFriend);
                return;
            }
            //acceptance is required
            TryGetConnection(userId, out var friendCon);
            //request is pending
            if(isFriend && UserIdDataManager.Server_HasApprovedFriendship(id, userId)) {
                //update friends info
                if(friendCon != null&&friendCon.IsActive) {
                    SendPendingFriendUpdated(friendCon, id, false);
                    SendFriendUpdated(friendCon, id, true);
                }
                //update your info
                SendFriendUpdated(con,userId, true);
                SendRequestingFriendUpdated(con,userId,false);
                return;
            }
            if(!isFriend) {
                if(friendCon != null && friendCon.IsActive) {
                    SendPendingFriendUpdated(friendCon, id, true);
                    SendFriendUpdated(friendCon, id, false);
                }
                SendFriendUpdated(con,userId, false);
                SendRequestingFriendUpdated(con,userId,true);
            }
            //no pending request
            if(friendCon != null && friendCon.IsActive)
                SendRequestingFriendUpdated(friendCon,id,true);
            SendPendingFriendUpdated(con,userId,true);
        }

        [ServerRpc(RequireOwnership = false)]
        private void UpdateBlockedOnServer(int userId, bool isBlocked, NetworkConnection con = null) {
            if(!con.TryGetUserId(out var id)) return;
            if(!UserIdDataManager.Server_IsUserIdValid(userId)) return;
            if(isBlocked == UserIdDataManager.Server_HasBlocked(id, userId)) return;
            if(isBlocked&&!friendshipsRequireAcceptance && UserIdDataManager.Server_HasFriended(id, userId)) {
                UserIdDataManager.Server_UnfriendUser(id,userId);
                SendFriendUpdated(con,userId,false);
            }
            if(isBlocked&&friendshipsRequireAcceptance && UserIdDataManager.Server_HasFriended(id, userId)) {
                TryGetConnection(userId, out var friendCon);
                UserIdDataManager.Server_UnfriendUser(id,userId);
                if(friendCon != null && friendCon.IsActive) {
                    SendFriendUpdated(friendCon, id, false);
                    SendPendingFriendUpdated(friendCon, id, true);
                }
                SendFriendUpdated(con,userId,false);
            }
            if(isBlocked) UserIdDataManager.Server_BlockUser(id,userId);
            else UserIdDataManager.Server_UnblockUser(id,userId);
            SendBlockedUpdated(con,userId,isBlocked);
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

    }
    
}